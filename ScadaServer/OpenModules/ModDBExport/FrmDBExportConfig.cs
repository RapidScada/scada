/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Module configuration form
    /// <para>Форма конфигурации КП</para>
    /// </summary>
    public partial class FrmDBExportConfig : Form
    {
        private string configDir;  // директория конфигурации
        private string langDir;    // директория языковых файлов
        private string logDir;     // директория журналов

        private Config config;     // конфигурация модуля
        private Config configCopy; // копия конфигурации модуля для реализации отмены изменений
        private bool modified;     // признак изменения конфигурации
        private Config.ExportDestination selExpDest; // выбранное назначение экспорта
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
        public static void ShowDialog(string configDir, string langDir, string logDir)
        {
            FrmDBExportConfig frmDBExportConfig = new FrmDBExportConfig();
            frmDBExportConfig.configDir = configDir;
            frmDBExportConfig.langDir = langDir;
            frmDBExportConfig.logDir = logDir;
            frmDBExportConfig.ShowDialog();
        }


        /// <summary>
        /// Создать узел дерева, соответствующий назначению экспорта
        /// </summary>
        private TreeNode NewExpDestNode(Config.ExportDestination expDest)
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

            foreach (Config.ExportDestination expDest in config.ExportDestinations)
                treeView.Nodes.Add(NewExpDestNode(expDest));

            treeView.ExpandAll();
            treeView.EndUpdate();

            // выбор первого узла дерева
            if (treeView.Nodes.Count > 0)
                treeView.SelectedNode = treeView.Nodes[0];

            SetExportParamsVisible();
        }

        /// <summary>
        /// Отобразить параметры экспорта для выбранного назначения
        /// </summary>
        private void ShowSelectedExportParams()
        {
        }

        /// <summary>
        /// Установить видимость параметров экспорта
        /// </summary>
        private void SetExportParamsVisible()
        {
            if (selExpDest == null) // конфигурация пуста
            {
                btnDelDataSource.Enabled = false;
                lblInstruction.Visible = true;
                tabControl.Visible = false;
            }
            else
            {
                btnDelDataSource.Enabled = true;
                lblInstruction.Visible = false;
                tabControl.Visible = true;
            }
        }


        private void FrmDBExportConfig_Load(object sender, EventArgs e)
        {
            // локализация модуля
            string errMsg;
            if (!Localization.UseRussian)
            {
                if (Localization.LoadDictionaries(langDir, "ModDBExport", out errMsg))
                {
                    Localization.TranslateForm(this, "Scada.Server.Modules.FrmDBExportConfig");
                    //ModPhrases.Init();
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }

            // настройка элементов управления
            lblInstruction.Top = treeView.Top;

            // загрузка конфигурации
            config = new Config(configDir);
            if (File.Exists(config.FileName) && !config.Load(out errMsg))
                ScadaUtils.ShowError(errMsg);

            // создание копии конфигурации
            configCopy = config.Clone();

            // отображение конфигурации
            ConfigToControls();

            // снятие признака изменения конфигурации
            Modified = false;
        }

        private void FrmDBExportConfig_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // определение и отображение свойств выбранного объекта
            TreeNode selNode = e.Node;
            selExpDest = selNode.Tag as Config.ExportDestination;
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
                Config.ExportDestination expDest = new Config.ExportDestination(dataSource, new Config.ExportParams());
                TreeNode treeNode = NewExpDestNode(expDest);

                int ind = config.ExportDestinations.BinarySearch(expDest);
                if (ind >= 0)
                    ind++;
                else
                    ind = ~ind;

                config.ExportDestinations.Insert(ind, expDest);
                treeView.Nodes.Insert(ind, treeNode);
                treeView.SelectedNode = treeNode;

                SetExportParamsVisible();
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

                SetExportParamsVisible();
                Modified = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
