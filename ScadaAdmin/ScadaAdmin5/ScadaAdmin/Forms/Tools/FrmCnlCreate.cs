/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Channel creation wizard
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Admin.App.Code;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Configuration;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Tools
{
    /// <summary>
    /// Channel creation wizard.
    /// <para>Мастер создания каналов.</para>
    /// </summary>
    public partial class FrmCnlCreate : Form
    {
        /// <summary>
        /// Indexes for the dictionaries.
        /// <para>Индексы для словарей.</para>
        /// </summary>
        private class DictIndexes
        {
            public DictIndexes(ConfigBase configBase)
            {
                CmdValByName = new Dictionary<string, int>();
                ParamByName = new Dictionary<string, int>();
                UnitByName = new Dictionary<string, int>();

                foreach (CmdVal cmdVal in configBase.CmdValTable.EnumerateItems())
                {
                    if (cmdVal.Name != null)
                        CmdValByName[cmdVal.Name] = cmdVal.CmdValID;
                }

                foreach (Param param in configBase.ParamTable.EnumerateItems())
                {
                    if (param.Name != null)
                        ParamByName[param.Name] = param.ParamID;
                }

                foreach (Unit unit in configBase.UnitTable.EnumerateItems())
                {
                    if (unit.Name != null)
                        UnitByName[unit.Name] = unit.UnitID;
                }
            }

            public Dictionary<string, int> CmdValByName { get; private set; }
            public Dictionary<string, int> ParamByName { get; private set; }
            public Dictionary<string, int> UnitByName { get; private set; }

            public int? GetID(Dictionary<string, int> dict, string name)
            {
                return name != null && dict.TryGetValue(name, out int id) ? (int?)id : null;
            }
        }

        private readonly ScadaProject project; // the project under development
        private readonly AppData appData;      // the common data of the application

        private int step; // the current step of the wizard


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlCreate()
        {
            InitializeComponent();
            step = 1;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlCreate(ScadaProject project, RecentSelection recentSelection, AppData appData)
            : this()
        {
            this.project = project ?? throw new ArgumentNullException("project");
            this.appData = appData ?? throw new ArgumentNullException("appData");
            step = 1;
        }


        /// <summary>
        /// Applies the wizard step.
        /// </summary>
        private void ApplyStep(int offset)
        {
            step += offset;

            if (step < 1)
                step = 1;
            else if (step > 3)
                step = 3;

            switch (step)
            {
                case 1:
                    lblStep.Text = AppPhrases.CreateCnlsStep1;
                    ctrlCnlCreate1.Visible = true;
                    ctrlCnlCreate2.Visible = false;
                    ctrlCnlCreate3.Visible = false;
                    btnCnlMap.Visible = false;
                    btnBack.Visible = false;
                    btnNext.Visible = true;
                    btnCreate.Visible = false;

                    ctrlCnlCreate1.SetFocus();
                    btnNext.Enabled = ctrlCnlCreate1.StatusOK;
                    break;
                case 2:
                    lblStep.Text = AppPhrases.CreateCnlsStep2;
                    ctrlCnlCreate1.Visible = false;
                    ctrlCnlCreate2.Visible = true;
                    ctrlCnlCreate3.Visible = false;
                    btnCnlMap.Visible = false;
                    btnBack.Visible = true;
                    btnNext.Visible = true;
                    btnCreate.Visible = false;

                    ctrlCnlCreate2.DeviceName = ctrlCnlCreate1.SelectedDevice?.Name;
                    ctrlCnlCreate2.SetFocus();
                    break;
                case 3:
                    lblStep.Text = AppPhrases.CreateCnlsStep3;
                    ctrlCnlCreate1.Visible = false;
                    ctrlCnlCreate2.Visible = false;
                    ctrlCnlCreate3.Visible = true;
                    btnCnlMap.Visible = true;
                    btnBack.Visible = true;
                    btnNext.Visible = false;
                    btnCreate.Visible = true;

                    if (ctrlCnlCreate1.StatusOK)
                    {
                        ctrlCnlCreate3.SetInCnlNums(ctrlCnlCreate1.CnlPrototypes.InCnls.Count);
                        ctrlCnlCreate3.SetOutCnlNums(ctrlCnlCreate1.CnlPrototypes.CtrlCnls.Count);
                        btnCreate.Enabled = true;
                    }
                    else
                    {
                        btnCreate.Enabled = false;
                    }

                    ctrlCnlCreate3.DeviceName = ctrlCnlCreate1.SelectedDevice?.Name;
                    ctrlCnlCreate3.SetFocus();
                    break;
            }
        }

        /// <summary>
        /// Creates channels based on the prototypes.
        /// </summary>
        private bool CreateChannels()
        {
            try
            {
                List<KPView.InCnlPrototype> inCnlPrototypes = ctrlCnlCreate1.CnlPrototypes.InCnls;
                List<KPView.CtrlCnlPrototype> ctrlCnlPrototypes = ctrlCnlCreate1.CnlPrototypes.CtrlCnls;
                int? objNum = ctrlCnlCreate2.ObjNum;
                int kpNum = ctrlCnlCreate1.SelectedDevice.KPNum;
                string cnlPrefix = appData.AppSettings.ChannelOptions.PrependDeviceName ? 
                    ctrlCnlCreate1.SelectedDevice.Name + " - " : "";
                int inCnlNum = ctrlCnlCreate3.StartInCnl;
                int ctrlCnlNum = ctrlCnlCreate3.StartOutCnl;
                int inCnlsAdded = 0;
                int ctrlCnlsAdded = 0;
                BaseTable<InCnl> inCnlTable = project.ConfigBase.InCnlTable;
                BaseTable<CtrlCnl> ctrlCnlTable = project.ConfigBase.CtrlCnlTable;
                BaseTable<Format> formatTable = project.ConfigBase.FormatTable;
                DictIndexes dictIndexes = new DictIndexes(project.ConfigBase);

                // create output channels
                foreach (KPView.CtrlCnlPrototype ctrlCnlPrototype in ctrlCnlPrototypes)
                {
                    ctrlCnlPrototype.CtrlCnlNum = ctrlCnlNum;

                    CtrlCnl ctrlCnl = new CtrlCnl
                    {
                        CtrlCnlNum = ctrlCnlNum++,
                        Active = ctrlCnlPrototype.Active,
                        Name = cnlPrefix + ctrlCnlPrototype.CtrlCnlName,
                        CmdTypeID = ctrlCnlPrototype.CmdTypeID,
                        ObjNum = objNum,
                        KPNum = kpNum,
                        CmdNum = ctrlCnlPrototype.CmdNum > 0 ? (int?)ctrlCnlPrototype.CmdNum : null,
                        CmdValID = dictIndexes.GetID(dictIndexes.CmdValByName, ctrlCnlPrototype.CmdVal),
                        FormulaUsed = ctrlCnlPrototype.FormulaUsed,
                        Formula = ctrlCnlPrototype.Formula,
                        EvEnabled = ctrlCnlPrototype.EvEnabled
                    };

                    if (ctrlCnl.Name.Length > ColumnLength.Name)
                        ctrlCnl.Name = ctrlCnl.Name.Substring(0, ColumnLength.Name);

                    ctrlCnlTable.AddItem(ctrlCnl);
                    ctrlCnlsAdded++;

                    if (ctrlCnlNum > ushort.MaxValue)
                        break;
                }

                if (ctrlCnlsAdded > 0)
                    ctrlCnlTable.Modified = true;

                // create input channels
                foreach (KPView.InCnlPrototype inCnlPrototype in inCnlPrototypes)
                {
                    int formatID = inCnlPrototype.FormatID > 0 ?
                        inCnlPrototype.FormatID :
                        inCnlPrototype.ShowNumber ? 
                            Math.Min(inCnlPrototype.DecDigits, BaseValues.Formats.MaxFixedID) :
                            BaseValues.Formats.EnumText;

                    InCnl inCnl = new InCnl
                    {
                        CnlNum = inCnlNum++,
                        Active = inCnlPrototype.Active,
                        Name = cnlPrefix + inCnlPrototype.CnlName,
                        CnlTypeID = inCnlPrototype.CnlTypeID,
                        ObjNum = objNum,
                        KPNum = kpNum,
                        Signal = inCnlPrototype.Signal > 0 ? (int?)inCnlPrototype.Signal : null,
                        FormulaUsed = inCnlPrototype.FormulaUsed,
                        Formula = inCnlPrototype.Formula,
                        Averaging = inCnlPrototype.Averaging,
                        ParamID = dictIndexes.GetID(dictIndexes.ParamByName, inCnlPrototype.ParamName),
                        FormatID = formatTable.PkExists(formatID) ? (int?)formatID : null,
                        UnitID = dictIndexes.GetID(dictIndexes.UnitByName, inCnlPrototype.UnitName),
                        CtrlCnlNum = inCnlPrototype.CtrlCnlProps?.CtrlCnlNum,
                        EvEnabled = inCnlPrototype.EvEnabled,
                        EvSound = inCnlPrototype.EvSound,
                        EvOnChange = inCnlPrototype.EvOnChange,
                        EvOnUndef = inCnlPrototype.EvOnUndef,
                        LimLowCrash = double.IsNaN(inCnlPrototype.LimLowCrash) ?
                            null : (double?)inCnlPrototype.LimLowCrash,
                        LimLow = double.IsNaN(inCnlPrototype.LimLow) ?
                            null : (double?)inCnlPrototype.LimLow,
                        LimHigh = double.IsNaN(inCnlPrototype.LimHigh) ?
                            null : (double?)inCnlPrototype.LimHigh,
                        LimHighCrash = double.IsNaN(inCnlPrototype.LimHighCrash) ?
                            null : (double?)inCnlPrototype.LimHighCrash
                    };

                    if (inCnl.Name.Length > ColumnLength.Name)
                        inCnl.Name = inCnl.Name.Substring(0, ColumnLength.Name);

                    inCnlTable.AddItem(inCnl);
                    inCnlsAdded++;

                    if (inCnlNum > ushort.MaxValue)
                        break;
                }

                if (inCnlsAdded > 0)
                    inCnlTable.Modified = true;

                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.CreateCnlsComplete, inCnlsAdded, ctrlCnlsAdded));
                return true;
            }
            catch (Exception ex)
            {
                appData.ProcError(ex, AppPhrases.CreateCnlsError);
                return false;
            }
        }


        private void FrmCnlCreate_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ctrlCnlCreate1.Init(project, appData);
            ctrlCnlCreate2.Init(project, appData.AppState.RecentSelection);
            ctrlCnlCreate3.Init(project, appData.AppSettings);
            ApplyStep(0);
        }

        private void ctrlCnlCreate1_SelectedDeviceChanged(object sender, EventArgs e)
        {
            if (step == 1)
                btnNext.Enabled = ctrlCnlCreate1.StatusOK;
        }

        private void btnCnlMap_Click(object sender, EventArgs e)
        {
            // generate channel map
            new CnlMap(project.ConfigBase, appData).Generate();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ApplyStep(-1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ApplyStep(1);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (ctrlCnlCreate1.StatusOK && CreateChannels())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
