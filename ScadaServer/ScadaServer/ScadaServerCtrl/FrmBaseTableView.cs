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
 * Module   : SCADA-Server Control
 * Summary  : Viewing the configuration base table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2019
 */

using Scada.Data.Tables;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Ctrl
{
    /// <summary>
    /// Viewing the configuration base table form
    /// <para>Форма просмотра таблицы базы конфигурации</para>
    /// </summary>
    public partial class FrmBaseTableView : Form
    {
        /// <summary>
        /// Наименования таблиц базы конфигурации
        /// </summary>
        private static readonly Dictionary<string, string> BaseTableTitles = new Dictionary<string,string>()
        {
            { "cmdtype.dat", CommonPhrases.CmdTypeTable }, 
            { "cmdval.dat", CommonPhrases.CmdValTable }, 
            { "cnltype.dat", CommonPhrases.CnlTypeTable }, 
            { "commline.dat", CommonPhrases.CommLineTable }, 
            { "ctrlcnl.dat", CommonPhrases.CtrlCnlTable }, 
            { "evtype.dat", CommonPhrases.CnlTypeTable }, 
            { "format.dat", CommonPhrases.FormatTable }, 
            { "formula.dat", CommonPhrases.FormulaTable }, 
            { "incnl.dat", CommonPhrases.InCnlTable }, 
            { "interface.dat", CommonPhrases.InterfaceTable }, 
            { "kp.dat", CommonPhrases.KPTable }, 
            { "kptype.dat", CommonPhrases.KPTypeTable }, 
            { "obj.dat", CommonPhrases.ObjTable }, 
            { "param.dat", CommonPhrases.ParamTable }, 
            { "right.dat", CommonPhrases.RightTable }, 
            { "role.dat", CommonPhrases.RoleTable }, 
            { "unit.dat", CommonPhrases.UnitTable }, 
            { "user.dat", CommonPhrases.UserTable }
        };

        private Log errLog;              // журнал ошибок приложения
        private BaseAdapter baseAdapter; // адаптер таблицы базы конфигурации
        private DataTable dataTable;     // загруженная таблица


        /// <summary>
        /// Конструктор
        /// </summary>
        private FrmBaseTableView()
        {
            InitializeComponent();
            errLog = null;
            baseAdapter = null;
            dataTable = null;
        }


        /// <summary>
        /// Загрузить таблицу базы конфигурации
        /// </summary>
        private static bool LoadDataTable(BaseAdapter baseAdapter, Log errLog, ref DataTable dataTable)
        {
            try
            {
                baseAdapter.Fill(dataTable, true);
                return true;
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.IncorrectFilter + ":\r\n" + ex.Message;
                if (errLog != null)
                    errLog.WriteAction(errMsg, Log.ActTypes.Exception);
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Отобразить форму просмотра таблицы базы конфигурации
        /// </summary>
        public static void Show(string directory, string tableName, Log errLog)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("directory");
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("tableName");
            if (errLog == null)
                throw new ArgumentNullException("errLog");

            // загрузка таблицы
            BaseAdapter baseAdapter = new BaseAdapter();
            baseAdapter.Directory = directory;
            baseAdapter.TableName = tableName;
            DataTable dataTable = new DataTable();

            // отображение формы
            if (LoadDataTable(baseAdapter, errLog, ref dataTable))
            {
                FrmBaseTableView frmBaseTableView = new FrmBaseTableView();
                frmBaseTableView.errLog = errLog;
                frmBaseTableView.baseAdapter = baseAdapter;
                frmBaseTableView.dataTable = dataTable;
                frmBaseTableView.ShowDialog();
            }
        }


        private void FrmBaseTableView_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Server.Ctrl.FrmBaseTableView");
            if (lblCount.Text.Contains("{0}"))
                bindingNavigator.CountItemFormat = lblCount.Text;

            // настройка элементов управления
            string tableTitle = BaseTableTitles.TryGetValue(baseAdapter.TableName, out tableTitle) ? 
                " - " + tableTitle : "";
            Text += " - " + baseAdapter.TableName + tableTitle;
            dataGridView.AutoGenerateColumns = true;
            bindingSource.DataSource = dataTable;
            ScadaUiUtils.AutoSizeColumns(dataGridView);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // перезагрузка таблицы
            DataTable newDataTable = new DataTable();

            if (LoadDataTable(baseAdapter, errLog, ref newDataTable))
            {
                dataTable = newDataTable;
                try { dataTable.DefaultView.RowFilter = txtFilter.Text; }
                catch { txtFilter.Text = ""; }
                bindingSource.DataSource = dataTable;
                ScadaUiUtils.AutoSizeColumns(dataGridView);
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // установка фильтра таблицы
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    dataTable.DefaultView.RowFilter = txtFilter.Text;
                }
                catch
                {
                    ScadaUiUtils.ShowError(AppPhrases.IncorrectFilter);
                }
            }
        }
    }
}
