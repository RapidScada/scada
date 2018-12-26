/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : Creating channels form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using Scada.Comm.Devices;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Creating channels form
    /// <para>Форма создания каналов</para>
    /// </summary>
    public partial class FrmCreateCnls : Form
    {
        private static string lastCommDir = "";     // последняя использованная директория SCADA-Коммуникатора
        private static Dictionary<string, Type> kpViewTypes = null; // словарь типов интерфейса КП

        private Scada.Comm.AppDirs commDirs;        // директории SCADA-Коммуникатора
        private List<CreateCnls.KPInfo> kpInfoList; // список информации о выбираемых КП
        private List<int> inCnlNums;                // список номеров входных каналов
        private List<int> ctrlCnlNums;              // список номеров каналов управления


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmCreateCnls()
        {
            InitializeComponent();

            kpInfoList = new List<CreateCnls.KPInfo>();
            inCnlNums = null;
            ctrlCnlNums = null;
            commDirs = null;
            gvKPSel.AutoGenerateColumns = false;
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(string commDir)
        {
            FrmCreateCnls frmCreateCnls = new FrmCreateCnls();
            frmCreateCnls.commDirs = new Scada.Comm.AppDirs();
            frmCreateCnls.commDirs.Init(commDir);
            frmCreateCnls.ShowDialog();
        }


        /// <summary>
        /// Загрузить библиотеки КП
        /// </summary>
        private void LoadKPDlls()
        {
            if (kpViewTypes == null || lastCommDir != commDirs.ExeDir)
            {
                lastCommDir = commDirs.ExeDir;
                kpViewTypes = new Dictionary<string, Type>();

                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(commDirs.KPDir);
                    FileInfo[] fileInfoAr = dirInfo.GetFiles("kp*.dll", SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in fileInfoAr)
                    {
                        if (!fileInfo.Name.Equals("kp.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            Type kpViewType;
                            try { kpViewType = KPFactory.GetKPViewType(fileInfo.FullName); }
                            catch { kpViewType = null; }

                            kpViewTypes.Add(fileInfo.Name, kpViewType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppUtils.ProcError(AppPhrases.LoadKPDllError + ":\r\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Заполнить фильтр КП по линии связи
        /// </summary>
        private void FillKPFilter()
        {
            try
            {
                DataTable tblCommLine = Tables.GetCommLineTable();

                DataRow noFilterRow = tblCommLine.NewRow();
                noFilterRow["CommLineNum"] = 0;
                noFilterRow["Name"] = cbKPFilter.Items[0];
                tblCommLine.Rows.InsertAt(noFilterRow, 0);

                cbKPFilter.DataSource = tblCommLine;
                cbKPFilter.SelectedIndexChanged += cbKPFilter_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.FillKPFilterError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Заполнить таблицу КП
        /// </summary>
        private void FillKPGrid()
        {
            try
            {
                DataTable tblObj = Tables.GetObjTable();
                tblObj.Rows.Add(DBNull.Value, AppPhrases.UndefinedItem);
                DataGridViewComboBoxColumn colObjNum = (DataGridViewComboBoxColumn)gvKPSel.Columns["colObjNum"];
                colObjNum.DataSource = tblObj;
                colObjNum.DisplayMember = "Name";
                colObjNum.ValueMember = "ObjNum";

                DataTable tblKP = Tables.GetKPTable();
                DataTable tblKPType = Tables.GetKPTypeTable();
                foreach (DataRow rowKP in tblKP.Rows)
                {
                    CreateCnls.KPInfo kpInfo = CreateCnls.KPInfo.Create(rowKP, tblKPType);

                    if (kpInfo.DllFileName != "")
                    {
                        Type kpViewType;
                        if (kpViewTypes.TryGetValue(kpInfo.DllFileName, out kpViewType))
                        {
                            if (kpViewType == null)
                            {
                                kpInfo.Color = Color.Red;
                                kpInfo.DllState = CreateCnls.DllStates.Error;
                            }
                            else
                            {
                                kpInfo.Enabled = true;
                                kpInfo.Color = Color.Black;
                                kpInfo.DllState = CreateCnls.DllStates.Loaded;
                            }
                        }
                        else
                        {
                            kpInfo.DllState = CreateCnls.DllStates.NotFound;
                        }
                    }

                    kpInfoList.Add(kpInfo);
                }

                gvKPSel.DataSource = kpInfoList;
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.FillKPGridError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Рассчитать и отобразить номера каналов
        /// </summary>
        private void CalcAndShowCnlNums(bool showError)
        {
            // получение номеров существующих каналов
            if (inCnlNums == null)
                inCnlNums = Tables.GetInCnlNums();
            if (ctrlCnlNums == null)
                ctrlCnlNums = Tables.GetCtrlCnlNums();

            // получение параметров нумерации
            CreateCnls.CnlNumParams inCnlNumParams = new CreateCnls.CnlNumParams()
            {
                Start = decimal.ToInt32(numInCnlsStart.Value),
                Multiple = decimal.ToInt32(numInCnlsMultiple.Value),
                Shift = decimal.ToInt32(numInCnlsShift.Value),
                Space = decimal.ToInt32(numInCnlsSpace.Value)
            };

            CreateCnls.CnlNumParams ctrlCnlNumParams = new CreateCnls.CnlNumParams()
            {
                Start = decimal.ToInt32(numCtrlCnlsStart.Value),
                Multiple = decimal.ToInt32(numCtrlCnlsMultiple.Value),
                Shift = decimal.ToInt32(numCtrlCnlsShift.Value),
                Space = decimal.ToInt32(numCtrlCnlsSpace.Value)
            };
            
            // рассчёт номеров каналов
            string errMsg;
            bool calcOk = CreateCnls.CalcCnlNums(kpViewTypes, kpInfoList, commDirs,
                inCnlNums, inCnlNumParams, ctrlCnlNums, ctrlCnlNumParams, out errMsg);

            // вывод на форму
            SwitchCalcCreateEnabled(!calcOk);
            gvKPSel.Invalidate();
            if (showError && errMsg != "")
                AppUtils.ProcError(errMsg);
        }

        /// <summary>
        /// Переключить доступность кнопок расчёта номеров каналов и создания каналов
        /// </summary>
        private void SwitchCalcCreateEnabled(bool calcEnabled)
        {
            btnCalc.Enabled = calcEnabled;
            btnCreate.Enabled = !calcEnabled;
        }

        /// <summary>
        /// Разрешить расчёт номеров каналов и запретить создание каналов
        /// </summary>
        private void EnableCalc()
        {
            SwitchCalcCreateEnabled(true);
        }


        private void FrmCreateCnls_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmCreateCnls");
        }

        private void FrmCreateCnls_Shown(object sender, EventArgs e)
        {
            // загрузка библиотек КП
            LoadKPDlls();

            // заполнение фильтра КП по линии связи
            FillKPFilter();

            // заполнение таблицы КП
            FillKPGrid();

            // установка доступности кнопок расчёта и создания каналов
            EnableCalc();
        }

        private void cbKPFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // фильтрация таблицы КП по линии связи
            if (cbKPFilter.SelectedIndex > 0)
            {
                int commLineNum = (int)cbKPFilter.SelectedValue;
                gvKPSel.DataSource = kpInfoList.Where(x => x.CommLineNum == commLineNum)
                    .ToList<CreateCnls.KPInfo>();
            }
            else
            {
                gvKPSel.DataSource = kpInfoList;
            }

            // отмена выбора всех КП
            btnDeselectAll_Click(null, null);
        }

        private void gvKPSel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // установка цвета ячейки
            int rowInd = e.RowIndex;
            if (0 <= rowInd && rowInd < kpInfoList.Count)
            {
                int colInd = e.ColumnIndex;
                if (colInd == colInCnls.Index && kpInfoList[rowInd].InCnlNumsErr ||
                    colInd == colCtrlCnls.Index && kpInfoList[rowInd].CtrlCnlNumsErr)
                    e.CellStyle.ForeColor = Color.Red;
                else
                    e.CellStyle.ForeColor = kpInfoList[rowInd].Color;
            }
        }

        private void gvKPSel_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int rowInd = e.RowIndex;
            if (0 <= rowInd && rowInd < kpInfoList.Count)
            {
                if (kpInfoList[rowInd].Enabled)
                {
                    if (e.ColumnIndex == colSelected.Index)
                        EnableCalc();
                }
                else
                {
                    // отмена редактирования ячейки
                    e.Cancel = true;
                }
            }
        }

        private void numCnls_ValueChanged(object sender, EventArgs e)
        {
            EnableCalc();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // выбор всех КП, которые отображаются в таблице
            List<CreateCnls.KPInfo> shownList = gvKPSel.DataSource as List<CreateCnls.KPInfo>;
            if (shownList != null)
            {
                foreach (CreateCnls.KPInfo kpInfo in shownList)
                    kpInfo.Selected = kpInfo.Enabled;
                gvKPSel.Invalidate();
                EnableCalc();
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            // отмена выбора всех КП, включая те, которые не отображаются в таблице
            foreach (CreateCnls.KPInfo kpInfo in kpInfoList)
                kpInfo.Selected = false;
            gvKPSel.Invalidate();
            EnableCalc();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            // расчёт и отображение номеров каналов
            CalcAndShowCnlNums(true);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // создание каналов
            string logFileName = AppData.AppDirs.LogDir + "ScadaAdminCreateCnls.txt";
            bool logCreated;
            string msg;
            
            bool createOK = CreateCnls.CreateChannels(kpInfoList, 
                chkInsertKPName.Checked, logFileName, out logCreated, out msg);

            if (msg != "")
            {
                if (createOK)
                    ScadaUiUtils.ShowInfo(msg);
                else
                    AppUtils.ProcError(msg);
            }

            if (logCreated)
                Process.Start(logFileName);
        }
    }
}