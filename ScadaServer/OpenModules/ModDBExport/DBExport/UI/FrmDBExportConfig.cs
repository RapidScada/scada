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
 * Module   : ModDBExport
 * Summary  : Module configuration form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using Scada.Client;
using Scada.UI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Module configuration form.
    /// <para>Форма конфигурации КП.</para>
    /// </summary>
    internal partial class FrmDBExportConfig : Form
    {
        private AppDirs appDirs;       // директории приложения
        private ServerComm serverComm; // объект для обмена данными со SCADA-Сервером

        private ModConfig config;         // конфигурация модуля
        private ModConfig configCopy;     // копия конфигурации модуля для реализации отмены изменений
        private bool modified;         // признак изменения конфигурации
        private bool changing;         // происходит изменение значений элементов управления
        private ModConfig.ExportDestination selExpDest; // выбранное назначение экспорта
        private TreeNode selExpDestNode;             // узел дерева выбранного назначения экспорта


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmDBExportConfig()
        {
            InitializeComponent();

            config = null;
            configCopy = null;
            modified = false;
            changing = false;
            selExpDest = null;
            selExpDestNode = null;
        }


        /// <summary>
        /// Получить или установить признак изменения конфигурации
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
                btnSave.Enabled = modified;
                btnCancel.Enabled = modified;
            }
        }


        /// <summary>
        /// Отобразить форму модально
        /// </summary>
        public static void ShowDialog(AppDirs appDirs, ServerComm serverComm)
        {
            FrmDBExportConfig frmDBExportConfig = new FrmDBExportConfig();
            frmDBExportConfig.appDirs = appDirs;
            frmDBExportConfig.serverComm = serverComm;
            frmDBExportConfig.ShowDialog();
        }


        /// <summary>
        /// Создать узел дерева, соответствующий назначению экспорта
        /// </summary>
        private TreeNode NewExpDestNode(ModConfig.ExportDestination expDest)
        {
            TreeNode node = new TreeNode(expDest.DataSource.Name);
            node.Tag = expDest;

            string imageKey;
            switch (expDest.DataSource.DBType)
            {
                case DBType.MSSQL:
                    imageKey = "mssql.png";
                    break;
                case DBType.Oracle:
                    imageKey = "oracle.png";
                    break;
                case DBType.PostgreSQL:
                    imageKey = "postgresql.png";
                    break;
                case DBType.MySQL:
                    imageKey = "mysql.png";
                    break;
                case DBType.OLEDB:
                    imageKey = "oledb.png";
                    break;
                default:
                    imageKey = "";
                    break;
            }

            node.ImageKey = node.SelectedImageKey = imageKey;
            return node;
        }

        /// <summary>
        /// Отобразить конфигурацию
        /// </summary>
        private void ConfigToControls()
        {
            // обнуление выбранного объекта
            selExpDest = null;
            selExpDestNode = null;

            // очистка и заполнение дерева
            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            foreach (ModConfig.ExportDestination expDest in config.ExportDestinations)
                treeView.Nodes.Add(NewExpDestNode(expDest));

            treeView.ExpandAll();
            treeView.EndUpdate();

            // выбор первого узла дерева
            if (treeView.Nodes.Count > 0)
                treeView.SelectedNode = treeView.Nodes[0];

            SetControlsEnabled();
        }

        /// <summary>
        /// Отобразить параметры экспорта для выбранного назначения
        /// </summary>
        private void ShowSelectedExportParams()
        {
            if (selExpDest != null)
            {
                changing = true;

                // вывод параметров соединения с БД
                tabControl.SelectedIndex = 0;
                DataSource dataSource = selExpDest.DataSource;
                txtServer.Text = dataSource.Server;
                txtDatabase.Text = dataSource.Database;
                txtUser.Text = dataSource.User;
                txtPassword.Text = dataSource.Password;

                // вывод строки соединения
                string bldConnStr = dataSource.BuildConnectionString();

                if (!string.IsNullOrEmpty(bldConnStr) && bldConnStr == dataSource.ConnectionString)
                {
                    txtConnectionString.Text = dataSource.BuildConnectionString(true);
                    SetConnControlsBackColor(KnownColor.Window, KnownColor.Control);
                }
                else
                {
                    txtConnectionString.Text = dataSource.ConnectionString;
                    SetConnControlsBackColor(KnownColor.Control, KnownColor.Window);
                }

                // вывод параметров экспорта
                ModConfig.ExportParams expParams = selExpDest.ExportParams;
                ctrlExportCurDataQuery.Export = expParams.ExportCurData;
                ctrlExportCurDataQuery.Query = expParams.ExportCurDataQuery;
                ctrlExportArcDataQuery.Export = expParams.ExportArcData;
                ctrlExportArcDataQuery.Query = expParams.ExportArcDataQuery;
                ctrlExportEventQuery.Export = expParams.ExportEvents;
                ctrlExportEventQuery.Query = expParams.ExportEventQuery;

                // вывод разных параметров
                numMaxQueueSize.SetValue(expParams.MaxQueueSize);

                changing = false;
            }
        }

        /// <summary>
        /// Установить цвет фона элементов управления параметров соединения с БД
        /// </summary>
        private void SetConnControlsBackColor(KnownColor connParamsColor, KnownColor connStrColor)
        {
            txtServer.BackColor = txtDatabase.BackColor = txtUser.BackColor = txtPassword.BackColor = 
                Color.FromKnownColor(connParamsColor);
            txtConnectionString.BackColor = Color.FromKnownColor(connStrColor);
        }

        /// <summary>
        /// Установить и отобразить автоматически построенную строку соединения
        /// </summary>
        private void SetConnectionString()
        {
            if (selExpDest != null)
            {
                string bldConnStr = selExpDest.DataSource.BuildConnectionString(true);
                if (!string.IsNullOrEmpty(bldConnStr))
                {
                    selExpDest.DataSource.ConnectionString = bldConnStr;
                    changing = true;
                    txtConnectionString.Text = bldConnStr;
                    changing = false;
                    SetConnControlsBackColor(KnownColor.Window, KnownColor.Control);
                }
            }
        }

        /// <summary>
        /// Установить доступность и видимость кнопок и параметров экспорта
        /// </summary>
        private void SetControlsEnabled()
        {
            if (selExpDest == null) // конфигурация пуста
            {
                btnDelDataSource.Enabled = false;
                btnManualExport.Enabled = false;
                lblInstruction.Visible = true;
                tabControl.Visible = false;
            }
            else
            {
                btnDelDataSource.Enabled = true;
                btnManualExport.Enabled = true;
                lblInstruction.Visible = false;
                tabControl.Visible = true;
            }
        }

        /// <summary>
        /// Сохранить конфигурацию модуля
        /// </summary>
        private bool SaveConfig()
        {
            if (Modified)
            {
                string errMsg;
                if (config.Save(out errMsg))
                {
                    Modified = false;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }


        private void FrmDBExportConfig_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(appDirs.LangDir, "ModDBExport", out errMsg))
                    Translator.TranslateForm(this, "Scada.Server.Modules.DBExport.FrmDBExportConfig");
                else
                    ScadaUiUtils.ShowError(errMsg);
            }

            // настройка элементов управления
            lblInstruction.Top = treeView.Top;

            // загрузка конфигурации
            config = new ModConfig(appDirs.ConfigDir);
            if (File.Exists(config.FileName) && !config.Load(out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // создание копии конфигурации
            configCopy = config.Clone();

            // отображение конфигурации
            ConfigToControls();

            // снятие признака изменения конфигурации
            Modified = false;
        }

        private void FrmDBExportConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(ModPhrases.SaveModSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!SaveConfig())
                            e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // определение и отображение свойств выбранного объекта
            TreeNode selNode = e.Node;
            selExpDest = selNode.Tag as ModConfig.ExportDestination;
            selExpDestNode = selExpDest == null ? null : selNode;
            ShowSelectedExportParams();
        }

        private void miAddDataSource_Click(object sender, EventArgs e)
        {
            // добавление назначения экспорта
            DataSource dataSource = null;

            if (sender == miAddSqlDataSource)
                dataSource = new SqlDataSource();
            else if (sender == miAddOraDataSource)
                dataSource = new OraDataSource();
            else if (sender == miAddPgSqlDataSource)
                dataSource = new PgSqlDataSource();
            else if (sender == miAddMySqlDataSource)
                dataSource = new MySqlDataSource();
            else if (sender == miAddOleDbDataSource)
                dataSource = new OleDbDataSource();

            if (dataSource != null)
            {
                ModConfig.ExportDestination expDest = new ModConfig.ExportDestination(dataSource, new ModConfig.ExportParams());
                TreeNode treeNode = NewExpDestNode(expDest);

                int ind = config.ExportDestinations.BinarySearch(expDest);
                if (ind >= 0)
                    ind++;
                else
                    ind = ~ind;

                config.ExportDestinations.Insert(ind, expDest);
                treeView.Nodes.Insert(ind, treeNode);
                treeView.SelectedNode = treeNode;

                SetConnectionString();
                SetControlsEnabled();
                Modified = true;
            }
        }

        private void btnDelDataSource_Click(object sender, EventArgs e)
        {
            // удаление назначения экспорта
            if (selExpDestNode != null)
            {
                TreeNode prevNode = selExpDestNode.PrevNode;
                TreeNode nextNode = selExpDestNode.NextNode;

                int ind = selExpDestNode.Index;
                config.ExportDestinations.RemoveAt(ind);
                treeView.Nodes.RemoveAt(ind);

                treeView.SelectedNode = nextNode == null ? prevNode : nextNode;
                if (treeView.SelectedNode == null)
                {
                    selExpDest = null;
                    selExpDestNode = null;
                }

                SetControlsEnabled();
                Modified = true;
            }
        }

        private void btnManualExport_Click(object sender, EventArgs e)
        {
            // отображение формы экспорта в ручном режиме
            int curDataCtrlCnlNum = config.CurDataCtrlCnlNum;
            int arcDataCtrlCnlNum = config.ArcDataCtrlCnlNum;
            int eventsCtrlCnlNum = config.EventsCtrlCnlNum;
            
            if (FrmManualExport.ShowDialog(serverComm, config.ExportDestinations, selExpDest, 
                ref curDataCtrlCnlNum, ref arcDataCtrlCnlNum, ref eventsCtrlCnlNum) &&
                (config.CurDataCtrlCnlNum != curDataCtrlCnlNum || 
                config.ArcDataCtrlCnlNum != arcDataCtrlCnlNum ||
                config.EventsCtrlCnlNum != eventsCtrlCnlNum))
            {
                // установка изменившихся номеров каналов управления
                config.CurDataCtrlCnlNum = curDataCtrlCnlNum;
                config.ArcDataCtrlCnlNum = arcDataCtrlCnlNum;
                config.EventsCtrlCnlNum = eventsCtrlCnlNum;
                Modified = true;
            }
        }


        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null && selExpDestNode != null)
            {
                selExpDest.DataSource.Server = txtServer.Text;
                selExpDestNode.Text = selExpDest.DataSource.Name;
                SetConnectionString();
                Modified = true;
            }
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.DataSource.Database = txtDatabase.Text;
                SetConnectionString();
                Modified = true;
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.DataSource.User = txtUser.Text;
                SetConnectionString();
                Modified = true;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.DataSource.Password = txtPassword.Text;
                SetConnectionString();
                Modified = true;
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.DataSource.ConnectionString = txtConnectionString.Text;
                SetConnControlsBackColor(KnownColor.Control, KnownColor.Window);
                Modified = true;
            }
        }

        private void ctrlExportCurDataQuery_PropChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.ExportParams.ExportCurData = ctrlExportCurDataQuery.Export;
                selExpDest.ExportParams.ExportCurDataQuery = ctrlExportCurDataQuery.Query;
                Modified = true;
            }
        }

        private void ctrlExportArcDataQuery_PropChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.ExportParams.ExportArcData = ctrlExportArcDataQuery.Export;
                selExpDest.ExportParams.ExportArcDataQuery = ctrlExportArcDataQuery.Query;
                Modified = true;
            }
        }

        private void ctrlExportEventQuery_PropChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.ExportParams.ExportEvents = ctrlExportEventQuery.Export;
                selExpDest.ExportParams.ExportEventQuery = ctrlExportEventQuery.Query;
                Modified = true;
            }
        }


        private void numMaxQueueSize_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && selExpDest != null)
            {
                selExpDest.ExportParams.MaxQueueSize = Convert.ToInt32(numMaxQueueSize.Value);
                Modified = true;
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение конфигурации модуля
            SaveConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // отмена изменений конфигурации
            config = configCopy;
            configCopy = config.Clone();
            ConfigToControls();
            Modified = false;
        }
    }
}
