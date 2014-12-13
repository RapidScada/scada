/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Import configuration table form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2014
 */

using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Scada;
using Scada.Data;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// Import configuration table form
    /// <para>Форма импорта таблицы базы конфигурации</para>
    /// </summary>
    public partial class FrmImport : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmImport()
        {
            InitializeComponent();

            DefaultTableName = "";
            DefaultDirectory = "";
        }


        /// <summary>
        /// Получить или установить имя таблицы, выбранной по умолчанию
        /// </summary>
        public string DefaultTableName { get; set; }

        /// <summary>
        /// Получить или установить директорию по умолчанию
        /// </summary>
        public string DefaultDirectory { get; set; }


        private void FrmImport_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "ScadaAdmin.FrmImport");
            openFileDialog.Title = AppPhrases.ChooseTableBaseFile;
            openFileDialog.Filter = AppPhrases.BaseTableFileFilter;

            // заполнение выпадающего списка таблиц
            int selInd = 0;

            foreach (Tables.TableInfo tableInfo in Tables.TablesInfo)
            {
                int ind = cbTable.Items.Add(tableInfo);
                if (tableInfo.Name == DefaultTableName)
                    selInd = ind;
            }

            if (cbTable.Items.Count > 0)
                cbTable.SelectedIndex = selInd;
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка имени файла таблицы
            Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;
            if (tableInfo != null)
            {
                txtFileName.Text = DefaultDirectory + tableInfo.FileName;
                gbIDs.Enabled = tableInfo.Name != "Right" && tableInfo.Name != "Formula";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // выбор файла таблицы
            string fileName = txtFileName.Text.Trim();
            openFileDialog.FileName = fileName;
            if (fileName != "")
                openFileDialog.InitialDirectory = Path.GetDirectoryName(fileName);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtFileName.Text = openFileDialog.FileName;
            txtFileName.Focus();
            txtFileName.DeselectAll();
        }

        private void chkStartID_CheckedChanged(object sender, EventArgs e)
        {
            numStartID.Enabled = chkStartID.Checked;
        }

        private void chkFinalID_CheckedChanged(object sender, EventArgs e)
        {
            numFinalID.Enabled = chkFinalID.Checked;
        }

        private void chkNewStartID_CheckedChanged(object sender, EventArgs e)
        {
            numNewStartID.Enabled = chkNewStartID.Checked;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // импорт выбранной таблицы из формата DAT
            StreamWriter writer = null;
            bool writeLog = chkImportLog.Checked;
            bool logCreated = false;
            string logFileName = AppData.ExeDir + "ScadaAdminImport.txt";

            try
            {
                Tables.TableInfo tableInfo = cbTable.SelectedItem as Tables.TableInfo;

                if (tableInfo != null && AppData.Connected)
                {
                    string fileName = txtFileName.Text.Trim();

                    if (writeLog)
                    {
                        writer = new StreamWriter(logFileName, false, Encoding.Default);
                        logCreated = true;

                        string title = DateTime.Now.ToString("G", Localization.Culture) + " " + AppPhrases.ImportTitle;
                        writer.WriteLine(title);
                        writer.WriteLine(new string('-', title.Length));
                        writer.WriteLine(AppPhrases.ImportTable + tableInfo.Name + " (" + tableInfo.Header + ")");
                        writer.WriteLine(AppPhrases.ImportFile + fileName);
                        writer.WriteLine();
                    }

                    // загрузка импортируемой таблицы
                    BaseAdapter baseAdapter = new BaseAdapter();
                    DataTable srcTable = new DataTable();
                    baseAdapter.FileName = fileName;

                    try
                    {
                        baseAdapter.Fill(srcTable, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(AppPhrases.LoadTableError + ":\r\n" + ex.Message);
                    }

                    if (writeLog)
                    {
                        writer.WriteLine(AppPhrases.SrcTableFields);
                        writer.WriteLine(new string('-', AppPhrases.SrcTableFields.Length));
                        if (srcTable.Columns.Count > 0)
                        {
                            foreach (DataColumn column in srcTable.Columns)
                                writer.WriteLine(column.ColumnName + " (" + column.DataType + ")");
                        }
                        else
                        {
                            writer.WriteLine(AppPhrases.NoFields);
                        }
                        writer.WriteLine();
                    }

                    // определение схемы таблицы БД
                    DataTable destTable = new DataTable(tableInfo.Name);
                    Tables.FillTableSchema(destTable);
                    
                    if (writeLog)
                    {
                        writer.WriteLine(AppPhrases.DestTableFields);
                        writer.WriteLine(new string('-', AppPhrases.DestTableFields.Length));
                        if (destTable.Columns.Count > 0)
                        {
                            foreach (DataColumn column in destTable.Columns)
                                writer.WriteLine(column.ColumnName + " (" + column.DataType + ")");
                        }
                        else
                        {
                            writer.WriteLine(AppPhrases.NoFields);
                        }
                        writer.WriteLine();
                    }

                    // установка контроля идентификаторов
                    string firstColumnName = destTable.Columns.Count > 0 ? destTable.Columns[0].ColumnName : "";
                    bool firstColumnIsID = gbIDs.Enabled && (firstColumnName.EndsWith("ID") ||
                        firstColumnName.EndsWith("Num") || firstColumnName == "CnlStatus") &&
                        destTable.Columns[0].DataType == typeof(int);

                    bool checkMinID = chkStartID.Checked && firstColumnIsID;
                    bool checkMaxID = chkFinalID.Checked && firstColumnIsID;
                    bool shiftID = chkNewStartID.Checked && firstColumnIsID;
                    bool shiftDef = false; // смещение определено
                    bool checkID = checkMaxID || checkMinID || shiftID;

                    int minID = checkMinID ? Convert.ToInt32(numStartID.Value) : 0;
                    int maxID = checkMaxID ? Convert.ToInt32(numFinalID.Value) : 0;
                    int newStartID = shiftID ? Convert.ToInt32(numNewStartID.Value) : 0;
                    int shift = 0;

                    // заполнение таблицы БД
                    foreach (DataRow row in srcTable.Rows)
                    {
                        DataRow newRow = destTable.NewRow();
                        bool rowIsOk = true;

                        foreach (DataColumn column in destTable.Columns)
                        {
                            int ind = srcTable.Columns.IndexOf(column.ColumnName);
                            if (ind >= 0 && column.DataType == srcTable.Columns[ind].DataType)
                            {
                                object val = row[ind];
                                if (ind == 0 && checkID && val != null && val != DBNull.Value)
                                {
                                    // проверка идентификатора
                                    int id = (int)val;
                                    if (checkMinID && id < minID || checkMaxID && id > maxID)
                                    {
                                        rowIsOk = false;
                                        break;
                                    }

                                    if (shiftID && !shiftDef)
                                    {
                                        shift = newStartID - id;
                                        shiftDef = true;
                                    }

                                    newRow[column] = id + shift;
                                }
                                else
                                    newRow[column] = val;
                            }
                        }

                        if (rowIsOk)
                            destTable.Rows.Add(newRow);
                    }

                    // сохранение информации в БД
                    int updRows = 0;
                    int errRows = 0;
                    DataRow[] rowsInError = null;

                    try
                    {
                        SqlCeDataAdapter sqlAdapter = destTable.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
                        updRows = sqlAdapter.Update(destTable);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(AppPhrases.WriteDBError + ":\r\n" + ex.Message);
                    }

                    if (destTable.HasErrors)
                    {
                        rowsInError = destTable.GetErrors();
                        errRows = rowsInError.Length;
                    }

                    string msg;
                    if (errRows == 0)
                    {
                        msg = string.Format(AppPhrases.ImportCompleted,  updRows);
                        ScadaUtils.ShowInfo(updRows > 0 ? msg + AppPhrases.RefreshRequired : msg);
                    }
                    else
                    {
                        msg = string.Format(AppPhrases.ImportCompletedWithErr, updRows, errRows);
                        AppData.ErrLog.WriteAction(msg, Log.ActTypes.Error);
                        ScadaUtils.ShowError(updRows > 0 ? msg + AppPhrases.RefreshRequired : msg);
                    }

                    if (writeLog)
                    {
                        writer.WriteLine(AppPhrases.ImportResult);
                        writer.WriteLine(new string('-', AppPhrases.ImportResult.Length));
                        writer.WriteLine(msg);

                        if (errRows > 0)
                        {
                            writer.WriteLine();
                            writer.WriteLine(AppPhrases.ImportErrors);
                            writer.WriteLine(new string('-', AppPhrases.ImportErrors.Length));

                            foreach (DataRow row in rowsInError)
                            {
                                if (firstColumnIsID)
                                {
                                    object objVal = row[0];
                                    string strVal = objVal == null || objVal == DBNull.Value ? "NULL" : objVal.ToString();
                                    writer.Write(firstColumnName + " = " + strVal + " : ");
                                }
                                writer.WriteLine(row.RowError);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.ImportError + ":\r\n" + ex.Message;
                try { if (writeLog) writer.WriteLine(errMsg); }
                catch { }
                AppUtils.ProcError(errMsg);
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }

            if (writeLog && logCreated)
                Process.Start(logFileName);
        }
    }
}
