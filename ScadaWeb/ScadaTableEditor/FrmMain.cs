/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : SCADA-Table Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2017
 */

using Scada;
using Scada.Data.Tables;
using Scada.UI;
using Scada.Web.Plugins.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ScadaTableEditor
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Имя файла представления по умолчанию
        /// </summary>
        private const string DefFileName = "NewTable.tbl";
        /// <summary>
        /// Имя файла настроек приложения
        /// </summary>
        private const string SettingsFileName = "ScadaTableEditorConfig.xml";

        private TableView tableView;        // табличное представление
        private List<TableView.Item> items; // список элементов представления
        private string fileName;            // имя файла представления
        private bool modified;              // признак изменения представления
        private string exeDir;              // директория исполняемого файла приложения
        private string baseDATDir;          // директория базы конфигурации в формате DAT
        private bool baseLoaded;            // признак успешной загрузки базы конфигурации
        private DataTable tblInCnl;         // таблица входных каналов
        private DataTable tblCtrlCnl;       // таблица каналов управления
        private DataTable tblObj;           // таблица объектов
        private DataTable tblKP;            // таблица КП
        private bool groupByObj;            // признак группировки каналов по объектам


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            // инициализация полей
            tableView = null;
            items = null;
            fileName = "";
            modified = false;
            exeDir = "";
            baseDATDir = "";
            baseLoaded = false;
            tblInCnl = new DataTable();
            tblCtrlCnl = new DataTable();
            tblObj = new DataTable();
            tblKP = new DataTable();
            groupByObj = false;
        }


        /// <summary>
        /// Создать новое табличное представление
        /// </summary>
        private void CreateTableView()
        {
            if (CanCloseTableView())
            {
                fileName = "";
                tableView = new TableView();
                items = tableView.Items;
                bsItems.DataSource = items;
                modified = false;

                SetFormTitle();
                ShowTableViewTitle();
                SelectItem(0);
                SetItemBtnsEnabled();
            }
        }

        /// <summary>
        /// Открыть табличное представление
        /// </summary>
        private void OpenTableView()
        {
            if (CanCloseTableView() && openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TableView newTableView = new TableView();
                string errMsg;

                if (newTableView.LoadFromFile(openFileDialog.FileName, out errMsg))
                {
                    bool extChanged = false;
                    fileName = openFileDialog.FileName;

                    if (Path.GetExtension(fileName) == ".ofm")
                    {
                        fileName = Path.ChangeExtension(fileName, ".tbl");
                        extChanged = true;
                    }

                    tableView = newTableView;
                    items = tableView.Items;
                    bsItems.DataSource = items;

                    modified = extChanged;
                    SetFormTitle();
                    ShowTableViewTitle();
                    SelectItem(0);
                    SetItemBtnsEnabled();
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        /// <summary>
        /// Сохранить табличное представление
        /// </summary>
        private bool SaveTableView(bool saveAs)
        {
            bool result = false;
            dgvItems.EndEdit();

            if (fileName == "")
            {
                saveFileDialog.FileName = DefFileName;
                saveAs = true;
            }
            else
            {
                saveFileDialog.FileName = fileName;
            }

            if (!saveAs || saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveAs)
                    fileName = saveFileDialog.FileName;
                
                string errMsg;
                if (tableView.SaveToFile(fileName, out errMsg))
                {
                    SetModified(false);
                    SetFormTitle();
                    result = true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            return result;
        }

        /// <summary>
        /// Проверить возможность закрыть табличное представление
        /// </summary>
        private bool CanCloseTableView()
        {
            dgvItems.EndEdit();

            if (modified)
            {
                switch (MessageBox.Show(AppPhrases.SaveConfirm, CommonPhrases.QuestionCaption, 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        return SaveTableView(false);
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Установить признак изменения представления
        /// </summary>
        private void SetModified(bool value)
        {
            if (modified != value)
            {
                modified = value;
                SetFormTitle();
            }
        }

        /// <summary>
        /// Установить заголовок формы
        /// </summary>
        private void SetFormTitle()
        {
            Text = AppPhrases.FormTitle + " - " + (fileName == "" ? DefFileName : Path.GetFileName(fileName)) + 
                (modified ? "*" : "");
        }

        /// <summary>
        /// Отобразить заголовок табличного представления
        /// </summary>
        private void ShowTableViewTitle()
        {
            txtTableTitle.TextChanged -= txtTableTitle_TextChanged;
            txtTableTitle.Text = tableView.Title;
            txtTableTitle.TextChanged += txtTableTitle_TextChanged;
        }

        /// <summary>
        /// Выбрать элемент представления
        /// </summary>
        private void SelectItem(int rowInd, bool setFocus = true)
        {
            dgvItems.ClearSelection();
            int rowCnt = dgvItems.Rows.Count;

            if (rowInd >= 0 && rowCnt > 0)
            {
                if (rowInd >= rowCnt)
                    rowInd = rowCnt - 1;

                dgvItems.Rows[rowInd].Selected = true;
                dgvItems.CurrentCell = dgvItems.Rows[rowInd].Cells[0];
            }

            if (setFocus)
                dgvItems.Select();
        }

        /// <summary>
        /// Вставить новый элемент в представление
        /// </summary>
        private void InsertItem(TableView.Item item)
        {
            // получение индекса для вставки
            int newInd;

            if (dgvItems.SelectedRows.Count > 0)
            {
                newInd = -1;
                foreach (DataGridViewRow row in dgvItems.SelectedRows)
                    if (newInd < row.Index)
                        newInd = row.Index;
                newInd++;
            }
            else if (dgvItems.CurrentRow != null)
            {
                newInd = dgvItems.CurrentRow.Index + 1;
            }
            else
            {
                newInd = dgvItems.Rows.Count;
            }

            // вставка элемента
            tableView.Items.Insert(newInd, item);
            bsItems.ResetBindings(false);
            SelectItem(newInd, !dgvCnls.Focused);
            SetModified(true);
        }

        /// <summary>
        /// Установить доступность кнопок действий с элементами
        /// </summary>
        private void SetItemBtnsEnabled()
        {
            int selRowCnt = dgvItems.SelectedRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                selRowInd = dgvItems.SelectedRows[0].Index;
            }
            else if (dgvItems.CurrentRow != null)
            {
                selRowCnt = 1;
                selRowInd = dgvItems.CurrentRow.Index;
            }

            btnMoveUpItem.Enabled = selRowCnt == 1 && selRowInd > 0;
            btnMoveDownItem.Enabled = selRowCnt == 1 && selRowInd < dgvItems.Rows.Count - 1;
            btnDeleteItem.Enabled = selRowCnt > 0;
            btnShowItemInfo.Enabled = selRowCnt == 1 && baseLoaded;
        }
        
        /// <summary>
        /// Проверить корректность вещественного значения ячейки
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, object cellVal)
        {
            if (0 <= colInd && colInd < dgvItems.ColumnCount && 0 <= rowInd && rowInd < dgvItems.RowCount &&
                cellVal != null)
            {
                Type valType = dgvItems.Columns[colInd].ValueType;
                string valStr = cellVal.ToString();

                if (valType == typeof(int))
                {
                    int intVal;
                    if (!(int.TryParse(valStr, out intVal) && 0 <= intVal && intVal <= ushort.MaxValue))
                    {
                        if (string.IsNullOrWhiteSpace(valStr) && dgvItems.EditingControl is TextBox)
                        {
                            // замена пустого значения на 0
                            ((TextBox)dgvItems.EditingControl).Text = "0";
                        }
                        else
                        {
                            ScadaUiUtils.ShowError(string.Format(CommonPhrases.IntegerRangingRequired, 0, ushort.MaxValue));
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Загрузить настройки приложения из файла
        /// </summary>
        private void LoadSettings()
        {
            // установка директории по умолчанию
            baseDATDir = @"C:\SCADA\BaseDAT\";

            // загрузка директории
            try
            {
                string fileName = exeDir + SettingsFileName;
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList paramNodeList = xmlDoc.DocumentElement.SelectNodes("Param");
                foreach (XmlElement paramElem in paramNodeList)
                {
                    if (paramElem.GetAttribute("name").Trim().ToLower() == "basedatdir")
                        baseDATDir = ScadaUtils.NormalDir(paramElem.GetAttribute("value"));
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(CommonPhrases.LoadAppSettingsError + ":\n" + ex.Message);
            }
        }

        /// <summary>
        /// Сохранить настройки приложения в файле
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);
                XmlElement rootElem = xmlDoc.CreateElement("ScadaTableEditorConfig");
                xmlDoc.AppendChild(rootElem);
                rootElem.AppendParamElem("BaseDATDir", baseDATDir, CommonPhrases.BaseDATDir);
                xmlDoc.Save(exeDir + SettingsFileName);
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(CommonPhrases.SaveAppSettingsError + ":\n" + ex.Message);
            }
        }

        /// <summary>
        /// Загрузить данные базы конфигурации
        /// </summary>
        private void LoadBase()
        {
            try
            {
                tblInCnl.Clear();
                tblCtrlCnl.Clear();
                tblObj.Clear();
                tblKP.Clear();

                BaseAdapter adapter = new BaseAdapter();
                adapter.Directory = baseDATDir;

                adapter.TableName = "incnl.dat";
                adapter.Fill(tblInCnl, false);

                adapter.TableName = "ctrlcnl.dat";
                adapter.Fill(tblCtrlCnl, false);

                adapter.TableName = "obj.dat";
                adapter.Fill(tblObj, false);
                
                DataRow row = tblObj.NewRow();
                row["ObjNum"] = 0;
                row["Name"] = AppPhrases.AllObjItem;
                tblObj.Rows.InsertAt(row, 0);

                adapter.TableName = "kp.dat";
                adapter.Fill(tblKP, false);

                row = tblKP.NewRow();
                row["KPNum"] = 0;
                row["Name"] = AppPhrases.AllKPItem;
                row["KPTypeID"] = 0;
                tblKP.Rows.InsertAt(row, 0);

                baseLoaded = true;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(AppPhrases.LoadBaseError + ":\n" + ex.Message);
                baseLoaded = false;
            }
        }

        /// <summary>
        /// Отобразить данные базы конфигурации
        /// </summary>
        private void ShowBase()
        {
            // настройка фильтра по объектам или КП
            if (groupByObj)
            {
                lblCnlFilterByKP.Visible = false;
                lblCnlFilterByObj.Visible = true;
                btnFilterCnlsByObj.BackColor = SystemColors.ControlLightLight;
                btnFilterCnlsByKP.BackColor = SystemColors.Control;
            }
            else
            {
                lblCnlFilterByKP.Visible = true;
                lblCnlFilterByObj.Visible = false;
                btnFilterCnlsByObj.BackColor = SystemColors.Control;
                btnFilterCnlsByKP.BackColor = SystemColors.ControlLightLight;
            }

            cbCnlFilter.BeginUpdate();
            cbCnlFilter.SelectedIndexChanged -= cbCnlFilter_SelectedIndexChanged;
            cbCnlFilter.DataSource = null;
            cbCnlFilter.ValueMember = groupByObj ? "ObjNum" : "KPNum";
            cbCnlFilter.DisplayMember = "Name";
            cbCnlFilter.DataSource = groupByObj ? tblObj : tblKP;
            if (cbCnlFilter.Items.Count > 0)
                cbCnlFilter.SelectedIndex = 0;
            cbCnlFilter.SelectedIndexChanged += cbCnlFilter_SelectedIndexChanged;
            cbCnlFilter.EndUpdate();

            // настройка таблицы каналов
            bsCnls.DataSource = null;
            
            if (tblInCnl.Columns.IndexOf("Active") > 0)
                tblInCnl.DefaultView.RowFilter = "Active";

            if (tblCtrlCnl.Columns.IndexOf("Active") > 0)
                tblCtrlCnl.DefaultView.RowFilter = "Active";

            if (rbInCnls.Checked)
            {
                colCnlNum.DataPropertyName = "CnlNum";
                bsCnls.DataSource = tblInCnl;
            }
            else
            {
                colCnlNum.DataPropertyName = "CtrlCnlNum";
                bsCnls.DataSource = tblCtrlCnl;
            }
        }

        /// <summary>
        /// Обновить данные базы конфигурации
        /// </summary>
        private void RefreshBase()
        {
            int num;
            try { num = (int)cbCnlFilter.SelectedValue; }
            catch { num = 0; }

            cbCnlFilter.DataSource = null;
            bsCnls.DataSource = null;

            LoadBase();
            ShowBase();

            try { cbCnlFilter.SelectedValue = num; }
            catch { }

            cbCnlFilter.Select();
            SetItemBtnsEnabled();
        }

        /// <summary>
        /// Добавить элемент на основе канала из базы конфигурации
        /// </summary>
        private void AddItem()
        {
            if (dgvCnls.SelectedRows.Count > 0)
            {
                DataGridViewRow cnlRow = dgvCnls.SelectedRows[0];
                TableView.Item item = new TableView.Item();
                int cnlNum = (int)cnlRow.Cells["colCnlNum"].Value;
                item.Caption = (string)cnlRow.Cells["colCnlName"].Value;

                if (bsCnls.DataSource == tblInCnl)
                    item.CnlNum = cnlNum;
                else
                    item.CtrlCnlNum = cnlNum;

                InsertItem(item);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            // определение директории исполняемого файла приложения
            exeDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Application.ExecutablePath));

            // локализация приложения
            string langDir = exeDir + "Lang\\";
            string errMsg;

            if (Localization.LoadDictionaries(langDir, "ScadaData", out errMsg))
                CommonPhrases.Init();
            else
                ScadaUiUtils.ShowError(errMsg);

            if (Localization.LoadDictionaries(langDir, "ScadaTableEditor", out errMsg))
            {
                Translator.TranslateForm(this, "ScadaTableEditor.FrmMain", toolTip);
                PlgPhrases.Init();
                AppPhrases.Init();
                openFileDialog.Filter = AppPhrases.OpenFileFilter;
                saveFileDialog.Filter = AppPhrases.SaveFileFilter;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }

            // настройка элементов управления
            dgvCnls.AutoGenerateColumns = false;
            dgvItems.AutoGenerateColumns = false;

            // создание нового представления
            CreateTableView();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            // загрузка директории базы конфигурации
            LoadSettings();
            lblBaseDATDir.Text = baseDATDir;

            // загрузка и отображение данных базы конфигурации
            LoadBase();
            ShowBase();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверка возможности закрыть представление
            e.Cancel = !CanCloseTableView();
        }


        private void miFileNew_Click(object sender, EventArgs e)
        {
            // создание нового представления
            CreateTableView();
        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {
            // открытие представления
            OpenTableView();
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            // сохранение представления
            SaveTableView(false);
        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {
            // сохранение представления с выбором имени файла
            SaveTableView(true);
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miSettingsParams_Click(object sender, EventArgs e)
        {
            // отображение формы настроек приложения
            if (FrmSettings.Show(ref baseDATDir) == DialogResult.OK)
            {
                lblBaseDATDir.Text = baseDATDir;
                SaveSettings();
                RefreshBase();
            }
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            FrmAbout.ShowAbout(exeDir);
        }


        private void btnGroupCnlsByObj_Click(object sender, EventArgs e)
        {
            // группировка каналов по объектам
            if (!groupByObj)
            {
                groupByObj = true;
                ShowBase();
            }

            cbCnlFilter.Select();
        }

        private void btnGroupCnlsByKP_Click(object sender, EventArgs e)
        {
            // группировка каналов по КП
            if (groupByObj)
            {
                groupByObj = false;
                ShowBase();
            }

            cbCnlFilter.Select();
        }

        private void btnRefreshBase_Click(object sender, EventArgs e)
        {
            // обновление данных базы конфигурации
            RefreshBase();
        }
        
        private void cbCnlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка фильтра на таблицы каналов
            int num;
            try { num = (int)cbCnlFilter.SelectedValue; }
            catch { num = 0; }

            if (num > 0)
            {
                tblInCnl.DefaultView.RowFilter = tblCtrlCnl.DefaultView.RowFilter =
                    "Active AND " + (groupByObj ? "ObjNum = " : "KPNum = ") + num;
            }
            else
            {
                tblInCnl.DefaultView.RowFilter = tblCtrlCnl.DefaultView.RowFilter = "Active";
            }
        }

        private void rbCnls_CheckedChanged(object sender, EventArgs e)
        {
            // переключение между входными каналами и каналами управления
            if (sender == rbInCnls && rbInCnls.Checked)
            {
                bsCnls.DataSource = null;
                colCnlNum.DataPropertyName = "CnlNum";
                bsCnls.DataSource = tblInCnl;
            }
            else if (sender == rbCtrlCnls && rbCtrlCnls.Checked)
            {
                bsCnls.DataSource = null;
                colCnlNum.DataPropertyName = "CtrlCnlNum";
                bsCnls.DataSource = tblCtrlCnl;
            }
        }

        private void dgvCnls_SelectionChanged(object sender, EventArgs e)
        {
            // установка доступности кнопки добавления элемента из базы конфигурации
            btnAddItem.Enabled = dgvCnls.SelectedRows.Count > 0;
        }

        private void dgvCnls_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddItem();
        }

        private void dgvCnls_KeyDown(object sender, KeyEventArgs e)
        {
            // добавление элемента на основе канала из базы конфигурации
            if (e.KeyCode == Keys.Enter)
                AddItem();
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // добавление элемента на основе канала из базы конфигурации
            AddItem();
        }

        private void btnAddEmptyItem_Click(object sender, EventArgs e)
        {
            // добавление пустого элемента
            InsertItem(new TableView.Item());
        }

        private void btnMoveUpDownItem_Click(object sender, EventArgs e)
        {
            // перемещение выбранного элемента вверх или вниз
            DataGridViewSelectedRowCollection selRows = dgvItems.SelectedRows;

            if (selRows.Count == 1 || dgvItems.CurrentRow != null)
            {
                int curInd = selRows.Count > 0 ? selRows[0].Index : dgvItems.CurrentRow.Index;
                bool canMove = sender == btnMoveUpItem && curInd > 0 || 
                    sender == btnMoveDownItem && curInd < items.Count - 1;

                if (canMove)
                {
                    int newInd = sender == btnMoveUpItem ? curInd - 1 : curInd + 1;
                    TableView.Item item = items[curInd];
                    items.RemoveAt(curInd);
                    items.Insert(newInd, item);

                    bsItems.ResetBindings(false);
                    SelectItem(newInd);
                    SetModified(true);
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            // удаление элементов
            DataGridViewSelectedRowCollection selRows = dgvItems.SelectedRows;
            int selRowCnt = selRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                int[] selRowInds = new int[selRowCnt];

                for (int i = 0; i < selRowCnt; i++)
                    selRowInds[i] = selRows[i].Index;

                Array.Sort<int>(selRowInds);
                selRowInd = selRowInds[selRowCnt - 1] - selRowCnt + 1;

                for (int i = selRowCnt - 1; i >= 0; i--)
                    items.RemoveAt(selRowInds[i]);
            }
            else if (dgvItems.CurrentRow != null)
            {
                selRowInd = dgvItems.CurrentRow.Index;
                items.RemoveAt(selRowInd);
            }

            bsItems.ResetBindings(false);
            SelectItem(selRowInd);
            SetModified(true);
        }

        private void btnShowItemInfo_Click(object sender, EventArgs e)
        {
            // отображение информации об элементе
            DataGridViewSelectedRowCollection selRows = dgvItems.SelectedRows;

            if (selRows.Count == 1 || dgvItems.CurrentRow != null)
            {
                int curInd = selRows.Count > 0 ? selRows[0].Index : dgvItems.CurrentRow.Index;
                FrmItemInfo.Show(items[curInd], tblInCnl, tblCtrlCnl, tblObj, tblKP);
                dgvItems.Select();
            }
        }

        private void txtTableTitle_TextChanged(object sender, EventArgs e)
        {
            // изменение заголовка представления
            tableView.Title = txtTableTitle.Text;
            SetModified(true);
        }

        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            // установка доступности кнопок действий с элементами
            SetItemBtnsEnabled();
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // установка признака изменения представления
            if (e.RowIndex >= 0)
                SetModified(true);
        }

        private void dgvItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // отображение ячеек с нулевыми номерами каналов пустыми
            int colInd = e.ColumnIndex;

            if (0 <= colInd && colInd < dgvItems.Columns.Count && e.Value != null)
            {
                DataGridViewColumn col = dgvItems.Columns[colInd];

                if ((col == colItemCnlNum || col == colItemCtrlCnlNum) && (int)e.Value == 0)
                    e.Value = "";
            }
        }

        private void dgvItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // проверка вводимых в ячейку данных
            e.Cancel = dgvItems.CurrentCell != null && dgvItems.CurrentCell.IsInEditMode &&
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue);
        }

        private void dgvItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // установка признака изменения представления
            SetModified(true);
        }

        private void dgvItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ScadaUiUtils.ShowError(CommonPhrases.GridDataError + ":\n" + e.Exception.Message);
            e.ThrowException = false;
        }
    }
}
