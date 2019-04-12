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
 * Module   : Table Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Table.Editor.Code;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Scada.Table.Editor.Forms
{
    /// <summary>
    /// Main form of the application.
    /// <para>Главная форма приложения.</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// The default file name for a table view.
        /// </summary>
        private const string DefTableFileName = "NewTable.tbl";
        /// <summary>
        /// Short name of the application error log file.
        /// </summary>
        private const string ErrFileName = "ScadaTableEditor.err";
        /// <summary>
        /// The pattern to search the configuration database directory.
        /// </summary>
        private const string BaseDirPattern = "BaseXML";

        private readonly string exeDir;  // the directory of the executable file
        private readonly string langDir; // the directory of language files
        private readonly Log errLog;     // the application error log
        private TableView tableView;     // the edited table
        private string fileName;         // the table file name
        private bool modified;           // the table was modified

        private string baseDir;          // the configuration database directory
        private BaseTables baseTables;   // the configuration database tables


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            langDir = Path.Combine(exeDir, "Lang");
            errLog = new Log(Log.Formats.Full) { FileName = Path.Combine(exeDir, "Log", ErrFileName) } ;
            Application.ThreadException += Application_ThreadException;

            tableView = null;
            fileName = "";
            modified = false;

            baseDir = "";
            baseTables = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the table was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                DisplayTitle();
            }
        }


        /// <summary>
        /// Applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            if (!Localization.LoadDictionaries(langDir, "ScadaData", out string errMsg))
                errLog.WriteError(errMsg);

            bool tableDictLoaded = Localization.LoadDictionaries(langDir, "ScadaTable", out errMsg);
            if (!tableDictLoaded)
                errLog.WriteError(errMsg);

            CommonPhrases.Init();
            TablePhrases.Init();

            if (tableDictLoaded)
            {
                Translator.TranslateForm(this, GetType().FullName);
                ofdTable.SetFilter(TablePhrases.TableFileFilter);
                sfdTable.SetFilter(TablePhrases.TableFileFilter);
            }
        }

        /// <summary>
        /// Creates a new table.
        /// </summary>
        private void NewTable()
        {
            bsTable.DataSource = null;
            fileName = "";
            tableView = new TableView() { Title = TablePhrases.DefaultTableTitle };

            RefreshBase();
            DisplayTable();
            Modified = false;
        }

        /// <summary>
        /// Opens the table.
        /// </summary>
        private void OpenTable(string fileName)
        {
            bsTable.DataSource = null;
            this.fileName = fileName;
            tableView = new TableView();

            if (!tableView.LoadFromFile(fileName, out string errMsg))
                ProcError(errMsg);

            RefreshBase();
            DisplayTable();
            Modified = false;
        }

        /// <summary>
        /// Opens or creates a table.
        /// </summary>
        private void OpenOrCreateTable(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                NewTable();
            else
                OpenTable(fileName);
        }

        /// <summary>
        /// Saves the table.
        /// </summary>
        private bool SaveTable()
        {
            return string.IsNullOrEmpty(fileName) ? SaveTableAs() : SaveTable(fileName);
        }

        /// <summary>
        /// Saves the table with the file name selected by a user.
        /// </summary>
        private bool SaveTableAs()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                sfdTable.FileName = DefTableFileName;
            }
            else
            {
                sfdTable.InitialDirectory = Path.GetDirectoryName(fileName);
                sfdTable.FileName = Path.GetFileName(fileName);
            }

            if (sfdTable.ShowDialog() == DialogResult.OK && SaveTable(sfdTable.FileName))
            {
                RefreshBase();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Saves the table with the specified file name.
        /// </summary>
        private bool SaveTable(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name must not be empty.", fileName);

            if (tableView.SaveToFile(fileName, out string errMsg))
            {
                this.fileName = fileName;
                Modified = false;
                return true;
            }
            else
            {
                ProcError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Confirms that the current table can be closed.
        /// </summary>
        private bool ConfirmCloseTable()
        {
            if (Modified)
            {
                switch (MessageBox.Show(TablePhrases.SaveTableConfirm, CommonPhrases.QuestionCaption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        return SaveTable();
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
        /// Displays the form title.
        /// </summary>
        private void DisplayTitle()
        {
            Text = string.Format(TablePhrases.EditorTitle,
                (string.IsNullOrEmpty(fileName) ? DefTableFileName : Path.GetFileName(fileName)) +
                (Modified ? "*" : ""));
        }

        /// <summary>
        /// Displays the edited table.
        /// </summary>
        private void DisplayTable()
        {
            txtTableTitle.TextChanged -= txtTableTitle_TextChanged;
            txtTableTitle.Text = tableView == null ? "" : tableView.Title;
            txtTableTitle.TextChanged += txtTableTitle_TextChanged;

            bsTable.DataSource = tableView.Items;
            SetActionBtnsEnabled();
        }

        /// <summary>
        /// Enables or disables the action buttons.
        /// </summary>
        private void SetActionBtnsEnabled()
        {
            int selRowCnt = dgvTable.SelectedRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                selRowInd = dgvTable.SelectedRows[0].Index;
            }
            else if (dgvTable.CurrentRow != null)
            {
                selRowCnt = 1;
                selRowInd = dgvTable.CurrentRow.Index;
            }

            btnMoveUpItem.Enabled = selRowCnt == 1 && selRowInd > 0;
            btnMoveDownItem.Enabled = selRowCnt == 1 && selRowInd < dgvTable.Rows.Count - 1;
            btnDeleteItem.Enabled = selRowCnt > 0;
            btnItemInfo.Enabled = selRowCnt == 1 && baseTables != null;
        }

        /// <summary>
        /// Validates a cell after editing.
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, string cellVal, out string errMsg)
        {
            errMsg = "";

            if (0 <= colInd && colInd < dgvTable.ColumnCount &&
                0 <= rowInd && rowInd < dgvTable.RowCount &&
                dgvTable.Columns[colInd].ValueType == typeof(int))
            {
                if (string.IsNullOrWhiteSpace(cellVal) && dgvTable.EditingControl is TextBox textBox)
                {
                    textBox.Text = "0"; // replace empty value to 0
                }
                else if (!(int.TryParse(cellVal, out int intVal) && 0 <= intVal && intVal <= ushort.MaxValue))
                {
                    errMsg = string.Format(CommonPhrases.IntegerRangingRequired, 0, ushort.MaxValue);
                }
            }

            return string.IsNullOrEmpty(errMsg);
        }

        /// <summary>
        /// Gets the selected row.
        /// </summary>
        private bool GetSelectedRow(out DataGridViewRow row)
        {
            if (dgvTable.SelectedRows.Count == 1)
            {
                row = dgvTable.SelectedRows[0];
                return true;
            }
            else if (dgvTable.CurrentRow != null)
            {
                row = dgvTable.CurrentRow;
                return true;
            }
            else
            {
                row = null;
                return false;
            }
        }
        
        /// <summary>
        /// Selects the table item with the specified index.
        /// Выбрать элемент представления
        /// </summary>
        private void SelectTableItem(int rowInd, bool setFocus = true)
        {
            dgvTable.ClearSelection();
            int rowCnt = dgvTable.Rows.Count;

            if (rowInd >= 0 && rowCnt > 0)
            {
                if (rowInd >= rowCnt)
                    rowInd = rowCnt - 1;

                dgvTable.Rows[rowInd].Selected = true;
                dgvTable.CurrentCell = dgvTable.Rows[rowInd].Cells[0];
            }

            if (setFocus)
                dgvTable.Select();
        }

        /// <summary>
        /// Inserts the item in the table.
        /// </summary>
        private void InsertTableItem(TableView.Item item)
        {
            // get insert index
            int newInd;

            if (dgvTable.SelectedRows.Count > 0)
            {
                newInd = -1;
                foreach (DataGridViewRow row in dgvTable.SelectedRows)
                {
                    if (newInd < row.Index)
                        newInd = row.Index;
                }
                newInd++;
            }
            else if (dgvTable.CurrentRow != null)
            {
                newInd = dgvTable.CurrentRow.Index + 1;
            }
            else
            {
                newInd = dgvTable.Rows.Count;
            }

            // insert the item
            tableView.Items.Insert(newInd, item);
            bsTable.ResetBindings(false);
            SelectTableItem(newInd, !tvCnl.Focused);
            Modified = true;
        }

        /// <summary>
        /// Finds the configuration database, relative to the table file name.
        /// </summary>
        private bool FindConfigBase()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(fileName));

                while (dirInfo.Parent != null)
                {
                    dirInfo = dirInfo.Parent;

                    foreach (DirectoryInfo parentDirInfo in 
                        dirInfo.EnumerateDirectories(BaseDirPattern, SearchOption.TopDirectoryOnly))
                    {
                        baseDir = ScadaUtils.NormalDir(parentDirInfo.FullName);
                        return true;
                    }
                }
            }

            baseDir = "";
            return false;
        }

        /// <summary>
        /// Loads the configuration database.
        /// </summary>
        private void LoadConfigBase()
        {
            try
            {
                if (FindConfigBase())
                {
                    baseTables = new BaseTables();
                    baseTables.Load(baseDir);
                }
                else
                {
                    baseTables = null;
                }
            }
            catch (Exception ex)
            {
                baseTables = null;
                ProcError(ex, TablePhrases.LoadConfigBaseError);
            }
        }

        /// <summary>
        /// Fills the channel tree view.
        /// </summary>
        private void FillCnlTree()
        {
            try
            {
                tvCnl.BeginUpdate();
                tvCnl.Nodes.Clear();

                if (baseTables != null)
                {
                    bool inCnls = cbCnlKind.SelectedIndex == 0;

                    foreach (KP kp in baseTables.KPTable.EnumerateItems())
                    {
                        string nodeText = string.Format("[{0}] {1}", kp.KPNum, kp.Name);
                        TreeNode deviceNode = TreeViewUtils.CreateNode(nodeText, "device.png");
                        tvCnl.Nodes.Add(deviceNode);

                        TableFilter tableFilter = new TableFilter("KPNum", kp.KPNum);

                        if (inCnls)
                        {
                            foreach (InCnl inCnl in baseTables.InCnlTable.SelectItems(tableFilter))
                            {
                                nodeText = string.Format("[{0}] {1}", inCnl.CnlNum, inCnl.Name);
                                TreeNode inCnlNode = TreeViewUtils.CreateNode(nodeText, "in_cnl.png");
                                deviceNode.Nodes.Add(inCnlNode);
                            }
                        }
                        else
                        {
                            foreach (CtrlCnl ctrlCnl in baseTables.CtrlCnlTable.SelectItems(tableFilter))
                            {
                                nodeText = string.Format("[{0}] {1}", ctrlCnl.CtrlCnlNum, ctrlCnl.Name);
                                TreeNode ctrlCnlNode = TreeViewUtils.CreateNode(nodeText, "ctrl_cnl.png");
                                deviceNode.Nodes.Add(ctrlCnlNode);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProcError(ex, TablePhrases.FillCnlTreeError);
            }
            finally
            {
                tvCnl.EndUpdate();
            }
        }

        /// <summary>
        /// Refreshes an displays the configuration database.
        /// </summary>
        private void RefreshBase()
        {
            LoadConfigBase();
            FillCnlTree();
            lblBaseDir.Text = string.IsNullOrEmpty(baseDir) ? TablePhrases.BaseNotFound : baseDir;
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        private void ProcError(string message)
        {
            errLog.WriteError(message);
            ScadaUiUtils.ShowError(message);
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        private void ProcError(Exception ex, string message = null)
        {
            errLog.WriteException(ex, message);
            ScadaUiUtils.ShowError(string.IsNullOrEmpty(message) ?
                ex.Message :
                message + ":" + Environment.NewLine + ex.Message);
        }


        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            errLog.WriteException(e.Exception, CommonPhrases.UnhandledException);
            ScadaUiUtils.ShowError(CommonPhrases.UnhandledException + ":" + Environment.NewLine + e.Exception.Message);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LocalizeForm();

            cbCnlKind.SelectedIndexChanged -= cbCnlKind_SelectedIndexChanged;
            cbCnlKind.SelectedIndex = 0;
            cbCnlKind.SelectedIndexChanged += cbCnlKind_SelectedIndexChanged;

            string[] args = Environment.GetCommandLineArgs();
            OpenOrCreateTable(args.Length > 1 ? args[1] : "");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // confirm saving the table before closing
            e.Cancel = !ConfirmCloseTable();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        private void miFileNew_Click(object sender, EventArgs e)
        {
            if (ConfirmCloseTable())
                NewTable();
        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {
            if (ConfirmCloseTable())
            {
                ofdTable.FileName = "";

                if (ofdTable.ShowDialog() == DialogResult.OK)
                {
                    ofdTable.InitialDirectory = Path.GetDirectoryName(ofdTable.FileName);
                    OpenTable(ofdTable.FileName);
                }
            }
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            SaveTable();
        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveTableAs();
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            FrmAbout.ShowAbout(exeDir);
        }


        private void btnRefreshBase_Click(object sender, EventArgs e)
        {
            RefreshBase();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddEmptyItem_Click(object sender, EventArgs e)
        {
            InsertTableItem(new TableView.Item());
        }

        private void btnMoveUpDownItem_Click(object sender, EventArgs e)
        {
            // move selected table item up or down
            if (GetSelectedRow(out DataGridViewRow row))
            {
                int curInd = row.Index;
                List<TableView.Item> items = tableView.Items;

                if (sender == btnMoveUpItem && curInd > 0 ||
                    sender == btnMoveDownItem && curInd < items.Count - 1)
                {
                    int newInd = sender == btnMoveUpItem ? curInd - 1 : curInd + 1;
                    TableView.Item item = items[curInd];
                    items.RemoveAt(curInd);
                    items.Insert(newInd, item);

                    bsTable.ResetBindings(false);
                    SelectTableItem(newInd);
                    Modified = true;
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            // delete selected table items
            DataGridViewSelectedRowCollection selRows = dgvTable.SelectedRows;
            int selRowCnt = selRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                int[] selRowInds = new int[selRowCnt];

                for (int i = 0; i < selRowCnt; i++)
                {
                    selRowInds[i] = selRows[i].Index;
                }

                Array.Sort(selRowInds);
                selRowInd = selRowInds[selRowCnt - 1] - selRowCnt + 1;

                for (int i = selRowCnt - 1; i >= 0; i--)
                {
                    tableView.Items.RemoveAt(selRowInds[i]);
                }
            }
            else if (dgvTable.CurrentRow != null)
            {
                selRowInd = dgvTable.CurrentRow.Index;
                tableView.Items.RemoveAt(selRowInd);
            }

            bsTable.ResetBindings(false);
            SelectTableItem(selRowInd);
            Modified = true;
        }

        private void btnItemInfo_Click(object sender, EventArgs e)
        {
            // show selected item information
            if (baseTables != null && GetSelectedRow(out DataGridViewRow row))
                new FrmItemInfo(tableView.Items[row.Index], baseTables).ShowDialog();
        }


        private void cbCnlKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCnlTree();
        }

        private void txtTableTitle_TextChanged(object sender, EventArgs e)
        {
            // set the table title
            tableView.Title = txtTableTitle.Text;
            Modified = true;
        }


        private void dgvTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // hide zero channel numbers
            if ((e.ColumnIndex == colItemCnlNum.Index || e.ColumnIndex == colItemCtrlCnlNum.Index) && 
                e.Value is int intVal && intVal <= 0)
            {
                e.Value = "";
            }
        }

        private void dgvTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvTable.CurrentCell != null && dgvTable.CurrentCell.IsInEditMode &&
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue?.ToString(), out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
                e.Cancel = true;
            }
        }

        private void dgvTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                Modified = true;
        }

        private void dgvTable_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Modified = true;
        }

        private void dgvTable_SelectionChanged(object sender, EventArgs e)
        {
            SetActionBtnsEnabled();
        }
    }
}
