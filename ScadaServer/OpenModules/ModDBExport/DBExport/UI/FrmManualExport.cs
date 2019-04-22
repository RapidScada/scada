/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ModDBExport
 * Summary  : Manual export form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada.Client;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Manual export form
    /// <para>Форма экспорта в ручном режиме</para>
    /// </summary>
    internal partial class FrmManualExport : Form
    {
        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmManualExport()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static bool ShowDialog(ServerComm serverComm,
            List<Config.ExportDestination> expDests, Config.ExportDestination selExpDest,
            ref int curDataCtrlCnlNum, ref int arcDataCtrlCnlNum, ref int eventsCtrlCnlNum)
        {
            FrmManualExport frmManualExport = new FrmManualExport();
            frmManualExport.ServerComm = serverComm;

            // заполнение списка источников данных
            foreach (Config.ExportDestination expDest in expDests)
            {
                int ind = frmManualExport.cbDataSource.Items.Add(expDest.DataSource);
                if (expDest == selExpDest)
                    frmManualExport.cbDataSource.SelectedIndex = ind;
            }

            // установка каналов управления
            frmManualExport.CurDataCtrlCnlNum = curDataCtrlCnlNum;
            frmManualExport.ArcDataCtrlCnlNum = arcDataCtrlCnlNum;
            frmManualExport.EventsCtrlCnlNum = eventsCtrlCnlNum;

            if (frmManualExport.ShowDialog() == DialogResult.OK)
            {
                // возврат каналов управления
                curDataCtrlCnlNum = frmManualExport.CurDataCtrlCnlNum;
                arcDataCtrlCnlNum = frmManualExport.ArcDataCtrlCnlNum;
                eventsCtrlCnlNum = frmManualExport.EventsCtrlCnlNum;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Получить или установить объект для обмена данными со SCADA-Сервером
        /// </summary>
        private ServerComm ServerComm { get; set; }

        /// <summary>
        /// Получить или установить номер канала управления для экспорта текущих данных
        /// </summary>
        private int CurDataCtrlCnlNum
        {
            get
            {
                return Convert.ToInt32(numCurDataCtrlCnlNum.Value);
            }
            set
            {
                numCurDataCtrlCnlNum.SetValue(value);
            }
        }

        /// <summary>
        /// Получить или установить номер канала управления для экспорта архивных данных
        /// </summary>
        private int ArcDataCtrlCnlNum
        {
            get
            {
                return Convert.ToInt32(numArcDataCtrlCnlNum.Value);
            }
            set
            {
                numArcDataCtrlCnlNum.SetValue(value);
            }
        }

        /// <summary>
        /// Получить или установить номер канала управления для экспорта событий
        /// </summary>
        private int EventsCtrlCnlNum
        {
            get
            {
                return Convert.ToInt32(numEventsCtrlCnlNum.Value);
            }
            set
            {
                numEventsCtrlCnlNum.SetValue(value);
            }
        }


        private void FrmManualExport_Load(object sender, EventArgs e)
        {
            // перевод формы
            if (!Localization.UseRussian)
                Translator.TranslateForm(this, "Scada.Server.Modules.DBExport.FrmManualExport");

            // настройка элементов управления
            if (cbDataSource.SelectedIndex < 0 && cbDataSource.Items.Count > 0)
                cbDataSource.SelectedIndex = 0;
            btnExportCurData.Enabled = btnExportArcData.Enabled = btnExportEvents.Enabled = 
                cbDataSource.Items.Count > 0;
            dtpArcDataDate.Value = dtpEventsDate.Value = dtpArcDataTime.Value = DateTime.Today;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // отправка команды экспорта
            int ctrlCnlNum;
            string cmdDataStr = cbDataSource.Text;

            if (sender == btnExportArcData)
            {
                ctrlCnlNum = ArcDataCtrlCnlNum;
                DateTime dateTime = dtpArcDataDate.Value.Date.Add(dtpArcDataTime.Value.TimeOfDay);
                cmdDataStr += "\n" + ScadaUtils.XmlValToStr(dateTime);
            }
            else if (sender == btnExportEvents)
            {
                ctrlCnlNum = EventsCtrlCnlNum;
                DateTime date = dtpEventsDate.Value.Date;
                cmdDataStr += "\n" + ScadaUtils.XmlValToStr(date);
            }
            else
            {
                ctrlCnlNum = CurDataCtrlCnlNum;
            }

            bool result;
            byte[] cmdData = Encoding.Default.GetBytes(cmdDataStr);

            if (ServerComm.SendBinaryCommand(0, ctrlCnlNum, cmdData, out result))
                ScadaUiUtils.ShowInfo(ModPhrases.CmdSentSuccessfully);
            else
                ScadaUiUtils.ShowError(ServerComm.ErrMsg);
        }
    }
}
