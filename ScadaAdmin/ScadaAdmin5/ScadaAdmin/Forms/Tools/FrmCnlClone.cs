/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Form for cloning channels
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Form for cloning channels.
    /// <para>Форма для клонирования каналов.</para>
    /// </summary>
    public partial class FrmCnlClone : Form
    {
        /// <summary>
        /// The known functions that contain the channel number argument.
        /// </summary>
        private static readonly string[] KnownFunctions = { "N(", "Val(", "Stat(", "SetVal(", "SetStat(", "SetData(" };
        /// <summary>
        /// The possible end symbols of an argument.
        /// </summary>
        private static readonly char[] ArgumentEnds = { ')', ',' };

        private readonly ConfigBase configBase; // the configuration database
        private readonly AppData appData;       // the common data of the application


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlClone()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlClone(ConfigBase configBase, AppData appData)
            : this()
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
            this.appData = appData ?? throw new ArgumentNullException("appData");
            InCnlsSelected = true;
            InCnlsCloned = false;
            OutCnlsCloned = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to select input channels by default.
        /// </summary>
        public bool InCnlsSelected { get; set; }

        /// <summary>
        /// Gets a value indicating whether input channels have been cloned.
        /// </summary>
        public bool InCnlsCloned { get; private set; }

        /// <summary>
        /// Gets a value indicating whether output channels have been cloned.
        /// </summary>
        public bool OutCnlsCloned { get; private set; }


        /// <summary>
        /// Fills the combo box with the objects.
        /// </summary>
        private void FillObjList()
        {
            DataTable dataTable = configBase.ObjTable.ToDataTable(true);
            AddCustomRow(dataTable, "ObjNum", "Name", -1, AppPhrases.KeepUnchanged);
            AddCustomRow(dataTable, "ObjNum", "Name", 0, " ");
            dataTable.DefaultView.Sort = "ObjNum";

            cbReplaceObj.ValueMember = "ObjNum";
            cbReplaceObj.DisplayMember = "Name";
            cbReplaceObj.DataSource = dataTable;
            cbReplaceObj.SelectedValue = -1;
        }

        /// <summary>
        /// Fills the combo box with the devices.
        /// </summary>
        private void FillDeviceList()
        {
            DataTable dataTable = configBase.KPTable.ToDataTable(true);
            AddCustomRow(dataTable, "KPNum", "Name", -1, AppPhrases.KeepUnchanged);
            AddCustomRow(dataTable, "KPNum", "Name", 0, " ");
            dataTable.DefaultView.Sort = "KPNum";

            cbReplaceKP.ValueMember = "KPNum";
            cbReplaceKP.DisplayMember = "Name";
            cbReplaceKP.DataSource = dataTable;
            cbReplaceKP.SelectedValue = -1;
        }

        /// <summary>
        /// Adds a custom row to the data table.
        /// </summary>
        private void AddCustomRow(DataTable dataTable, string valueMember, string displayMember, 
            int value, string display)
        {
            DataRow customRow = dataTable.NewRow();
            customRow[valueMember] = value;
            customRow[displayMember] = display;
            dataTable.Rows.Add(customRow);
        }

        /// <summary>
        /// Calculates the end destination channel number.
        /// </summary>
        private void CalcDestEndNum()
        {
            numDestEndNum.SetValue(numSrcEndNum.Value - numSrcStartNum.Value + numDestStartNum.Value);
        }

        /// <summary>
        /// Clones input channels.
        /// </summary>
        private bool CloneInCnls(int srcStartNum, int srcEndNum, int destStartNum, 
            int replaceObjNum, int replaceKpNum, bool updateFormulas)
        {
            try
            {
                BaseTable<InCnl> inCnlTable = configBase.InCnlTable;
                int affectedRows = 0;

                if (srcStartNum <= srcEndNum)
                {
                    // create new channels
                    int shiftNum = destStartNum - srcStartNum;
                    List<InCnl> cnlsToAdd = new List<InCnl>(srcEndNum - srcStartNum + 1);

                    foreach (InCnl inCnl in inCnlTable.Items.Values)
                    {
                        int cnlNum = inCnl.CnlNum;

                        if (srcStartNum <= cnlNum && cnlNum <= srcEndNum)
                        {
                            int newCnlNum = cnlNum + shiftNum;

                            if (newCnlNum <= AdminUtils.MaxCnlNum && !inCnlTable.PkExists(newCnlNum))
                            {
                                InCnl newInCnl = ScadaUtils.DeepClone(inCnl);
                                newInCnl.CnlNum = newCnlNum;

                                if (replaceObjNum >= 0)
                                    newInCnl.ObjNum = replaceObjNum > 0 ? replaceObjNum : (int?)null;

                                if (replaceKpNum >= 0)
                                    newInCnl.KPNum = replaceKpNum > 0 ? replaceKpNum : (int?)null;

                                if (updateFormulas)
                                    newInCnl.Formula = UpdateFormula(newInCnl.Formula, shiftNum);

                                cnlsToAdd.Add(newInCnl);
                            }
                        }
                        else if (cnlNum > srcEndNum)
                        {
                            break;
                        }
                    }

                    // add the created channels
                    foreach (InCnl inCnl in cnlsToAdd)
                    {
                        inCnlTable.AddItem(inCnl);
                    }

                    affectedRows = cnlsToAdd.Count;
                }

                if (affectedRows > 0)
                    inCnlTable.Modified = true;

                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.CloneCnlsComplete, affectedRows));
                return true;
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.CloneInCnlsError);
                return false;
            }
        }

        /// <summary>
        /// Clones output channels.
        /// </summary>
        private bool CloneCtrlCnls(int srcStartNum, int srcEndNum, int destStartNum,
            int replaceObjNum, int replaceKpNum, bool updateFormulas)
        {
            try
            {
                BaseTable<CtrlCnl> ctrlCnlTable = configBase.CtrlCnlTable;
                int affectedRows = 0;

                if (srcStartNum <= srcEndNum)
                {
                    // create new channels
                    int shiftNum = destStartNum - srcStartNum;
                    List<CtrlCnl> cnlsToAdd = new List<CtrlCnl>(srcEndNum - srcStartNum + 1);

                    foreach (CtrlCnl ctrlCnl in ctrlCnlTable.Items.Values)
                    {
                        int ctrlCnlNum = ctrlCnl.CtrlCnlNum;

                        if (srcStartNum <= ctrlCnlNum && ctrlCnlNum <= srcEndNum)
                        {
                            int newCnlNum = ctrlCnlNum + shiftNum;

                            if (newCnlNum <= AdminUtils.MaxCnlNum && !ctrlCnlTable.PkExists(newCnlNum))
                            {
                                CtrlCnl newCtrlCnl = ScadaUtils.DeepClone(ctrlCnl);
                                newCtrlCnl.CtrlCnlNum = newCnlNum;

                                if (replaceObjNum >= 0)
                                    newCtrlCnl.ObjNum = replaceObjNum > 0 ? replaceObjNum : (int?)null;

                                if (replaceKpNum >= 0)
                                    newCtrlCnl.KPNum = replaceKpNum > 0 ? replaceKpNum : (int?)null;

                                if (updateFormulas)
                                    newCtrlCnl.Formula = UpdateFormula(newCtrlCnl.Formula, shiftNum);

                                cnlsToAdd.Add(newCtrlCnl);
                            }
                        }
                        else if (ctrlCnlNum > srcEndNum)
                        {
                            break;
                        }
                    }

                    // add the created channels
                    foreach (CtrlCnl ctrlCnl in cnlsToAdd)
                    {
                        ctrlCnlTable.AddItem(ctrlCnl);
                    }

                    affectedRows = cnlsToAdd.Count;
                }

                if (affectedRows > 0)
                    ctrlCnlTable.Modified = true;

                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.CloneCnlsComplete, affectedRows));
                return true;
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.CloneCtrlCnlsError);
                return false;
            }
        }

        /// <summary>
        /// Updates channel numbers in the specified formula.
        /// </summary>
        private string UpdateFormula(string formula, int shiftNum)
        {
            if (!string.IsNullOrEmpty(formula))
            {
                StringBuilder sbFormula = new StringBuilder();

                foreach (string knownFunc in KnownFunctions)
                {
                    bool funcFound;
                    int searchInd = 0;
                    int formulaEndInd = formula.Length - 1;

                    do
                    {
                        funcFound = false;
                        int funcStart = formula.IndexOf(knownFunc, searchInd);

                        if (funcStart == 0 || funcStart > 0 && !char.IsLetter(formula, funcStart - 1))
                        {
                            int argStart = funcStart + knownFunc.Length;
                            int argEnd = formula.IndexOfAny(ArgumentEnds, argStart);

                            if (argEnd >= 0)
                            {
                                string cnlNumStr = formula.Substring(argStart, argEnd - argStart);
                                if (int.TryParse(cnlNumStr, out int cnlNum))
                                {
                                    funcFound = true;
                                    searchInd = argEnd;
                                    formula = formula.Substring(0, argStart) + (cnlNum + shiftNum) + formula.Substring(argEnd);
                                }
                            }
                        }
                    }
                    while (funcFound && searchInd < formulaEndInd);
                }
            }

            return formula;
        }


        private void FrmCnlClone_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);

            if (!InCnlsSelected)
                rbOutCnls.Checked = true;

            FillObjList();
            FillDeviceList();
            CalcDestEndNum();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            CalcDestEndNum();
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            int srcStartNum = Convert.ToInt32(numSrcStartNum.Value);
            int srcEndNum = Convert.ToInt32(numSrcEndNum.Value);
            int destStartNum = Convert.ToInt32(numDestStartNum.Value);
            int replaceObjNum = (int)cbReplaceObj.SelectedValue;
            int replaceKpNum = (int)cbReplaceKP.SelectedValue;
            bool updateFormulas = chkUpdateFormulas.Checked;

            if (rbInCnls.Checked)
            {
                if (CloneInCnls(srcStartNum, srcEndNum, destStartNum, replaceObjNum, replaceKpNum, updateFormulas))
                    InCnlsCloned = true;
            }
            else if (rbOutCnls.Checked)
            {
                if (CloneCtrlCnls(srcStartNum, srcEndNum, destStartNum, replaceObjNum, replaceKpNum, updateFormulas))
                    OutCnlsCloned = true;
            }
        }
    }
}
