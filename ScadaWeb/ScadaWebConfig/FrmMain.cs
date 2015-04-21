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
 * Module   : SCADA-Web Configurator
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2015
 */

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Scada;
using Scada.Client;
using Scada.Web;
using System.Xml;

namespace ScadaWebConfig
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Директория конфигурации по умолчанию
        /// </summary>
        private const string DefaultConfigDir = @"C:\SCADA\ScadaWeb\config\";
        /// <summary>
        /// Имя файла настроек приложения
        /// </summary>
        private const string SettingsFileName = "ScadaWebConfig.xml";

        private string exeDir;    // директория исполняемого файла приложения
        private string configDir; // директория конфигурации
        private string webAppDir; // директория веб-приложения

        // редактируемые настройки
        private CommSettings commSettings;
        private WebSettings webSettings;
        private ViewSettings viewSettings;
        private string regKey;

        // загруженные настройки
        private CommSettings loadedCommSettings;
        private WebSettings loadedWebSettings;
        private ViewSettings loadedViewSettings;
        private string loadedRegKey;

        // признаки изменения настроек
        private bool commSettingsChanged;
        private bool webSettingsChanged;
        private bool viewSettingsChanged;
        private bool regKeyChanged;
        private bool showing;

        // выбранный в дереве набор представлений и представление
        private ViewSettings.ViewSet selViewSet;
        private ViewSettings.ViewInfo selViewInfo;


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            exeDir = "";
            configDir = "";
            webAppDir = "";
            commSettings = new CommSettings();
            webSettings = new WebSettings();
            viewSettings = new ViewSettings();
            regKey = "";

            loadedCommSettings = null;
            loadedWebSettings = null;
            loadedViewSettings = null;
            loadedRegKey = "";

            commSettingsChanged = false;
            webSettingsChanged = false;
            viewSettingsChanged = false;
            regKeyChanged = false;
            showing = false;

            selViewSet = null;
            selViewInfo = null;
        }


        /// <summary>
        /// Установить значение элемента управления NumericUpDown, проверив допустимые границы
        /// </summary>
        private void SetNumValue(NumericUpDown numUpDown, decimal val)
        {
            if (val < numUpDown.Minimum)
                numUpDown.Value = numUpDown.Minimum;
            else if (val > numUpDown.Maximum)
                numUpDown.Value = numUpDown.Maximum;
            else
                numUpDown.Value = val;
        }

        /// <summary>
        /// Снять признаки изменения настроек
        /// </summary>
        private void SetSettingsUnchanged()
        {
            commSettingsChanged = false;
            webSettingsChanged = false;
            viewSettingsChanged = false;
            regKeyChanged = false;

            foreach (TabPage tabPage in tabControl.TabPages)
                tabPage.Text = tabPage.Text.TrimEnd('*');

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        /// <summary>
        /// Отобразить настройки
        /// </summary>
        private void SettingsToControls()
        {
            showing = true;

            // вывод настроек соединения
            txtServerHost.Text = commSettings.ServerHost;
            SetNumValue(numServerPort, commSettings.ServerPort);
            SetNumValue(numServerTimeout, commSettings.ServerTimeout);
            txtServerUser.Text = commSettings.ServerUser;
            txtServerPwd.Text = commSettings.ServerPwd;

            // вывод настроек отображения
            SetNumValue(numSrezRefrFreq, webSettings.SrezRefrFreq);
            SetNumValue(numEventCnt, webSettings.EventCnt);
            chkEventFltr.Checked = webSettings.EventFltr;
            SetNumValue(numEventRefrFreq, webSettings.EventRefrFreq);
            SetNumValue(numDiagBreak, webSettings.DiagBreak);
            chkCmdEnabled.Checked = webSettings.CmdEnabled;
            chkRemEnabled.Checked = webSettings.RemEnabled;
            chkSimpleCmd.Checked = webSettings.SimpleCmd;

            // заполнение дерева наборов представлений
            tvTableSets.Nodes.Clear();

            foreach (ViewSettings.ViewSet viewSet in viewSettings.ViewSetList)
            {
                TreeNode nodeTableSet = tvTableSets.Nodes.Add(viewSet.Name);
                nodeTableSet.Tag = viewSet;

                foreach (ViewSettings.ViewInfo viewInfo in viewSet.Items)
                {
                    TreeNode nodeTable = nodeTableSet.Nodes.Add(viewInfo.Title);
                    nodeTable.Tag = viewInfo;
                }
            }

            tvTableSets.ExpandAll();
            txtName.Enabled = txtDirOrFile.Enabled = viewSettings.ViewSetList.Count > 0;
            lblDir.Visible = true;
            lblFile.Visible = false;
            lblType.Visible = cbType.Visible = false;

            if (tvTableSets.Nodes.Count > 0)
                tvTableSets.SelectedNode = tvTableSets.Nodes[0];

            showing = false;
        }
        
        /// <summary>
        /// Перенести информацию из элементов управления в настройки
        /// </summary>
        private void ControlsToSettings()
        {
            // заполнение настроек доступа к данным
            commSettings.ServerHost = txtServerHost.Text;
            commSettings.ServerPort = decimal.ToInt32(numServerPort.Value);
            commSettings.ServerTimeout = decimal.ToInt32(numServerTimeout.Value);
            commSettings.ServerUser = txtServerUser.Text;
            commSettings.ServerPwd = txtServerPwd.Text;

            // заполнение настроек отображения
            webSettings.SrezRefrFreq = decimal.ToInt32(numSrezRefrFreq.Value);
            webSettings.EventCnt = decimal.ToInt32(numEventCnt.Value);
            webSettings.EventFltr = chkEventFltr.Checked;
            webSettings.EventRefrFreq = decimal.ToInt32(numEventRefrFreq.Value);
            webSettings.DiagBreak = decimal.ToInt32(numDiagBreak.Value);
            webSettings.CmdEnabled = chkCmdEnabled.Checked;
            webSettings.RemEnabled = chkRemEnabled.Checked;
            webSettings.SimpleCmd = chkSimpleCmd.Checked;
        }

        /// <summary>
        /// Загрузить настройки из файлов
        /// </summary>
        private void LoadSettings()
        {
            StringBuilder sbErr = new StringBuilder();
            string errMsg;

            SetSettingsUnchanged();

            // загрузка настроек соединения
            if (!commSettings.LoadFromFile(configDir + CommSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            // загрузка настроек отображения
            if (!webSettings.LoadFromFile(configDir + WebSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            // загрузка настроек представлений
            if (!viewSettings.LoadFromFile(configDir + ViewSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            // отображение настроек
            SettingsToControls();

            if (sbErr.Length > 0)
                ScadaUtils.ShowError(sbErr.ToString().TrimEnd());

            loadedCommSettings = commSettings.Clone();
            loadedWebSettings = webSettings.Clone();
            loadedViewSettings = viewSettings.Clone();
            loadedRegKey = regKey;
        }

        /// <summary>
        /// Сохранить настройки в файлах
        /// </summary>
        private void SaveSettings()
        {
            StringBuilder sbErr = new StringBuilder();
            string errMsg;

            // сохранение настроек доступа к данным
            if (commSettingsChanged && !commSettings.SaveToFile(configDir + CommSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            // сохранение настроек отображения
            if (webSettingsChanged && !webSettings.SaveToFile(configDir + WebSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            // сохранение настроек представлений
            if (viewSettingsChanged && !viewSettings.SaveToFile(configDir + ViewSettings.DefFileName, out errMsg))
                sbErr.AppendLine(errMsg);

            if (sbErr.Length > 0)
            {
                ScadaUtils.ShowError(sbErr.ToString().TrimEnd());
            }
            else
            {
                SetSettingsUnchanged();
                loadedCommSettings = commSettings.Clone();
                loadedWebSettings = webSettings.Clone();
                loadedViewSettings = viewSettings.Clone();
                loadedRegKey = regKey;
            }
        }

        /// <summary>
        /// Загрузить директорию конфигурации из файла
        /// </summary>
        private void LoadConfigDir()
        {
            // установка директории конфигурации по умолчанию
            configDir = DefaultConfigDir;

            // загрузка директории конфигурации
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
                    if (paramElem.GetAttribute("name").Trim().ToLower() == "configdir")
                        configDir = ScadaUtils.NormalDir(paramElem.GetAttribute("value"));
                }
            }
            catch (Exception ex)
            {
                ScadaUtils.ShowError(AppPhrases.LoadConfigDirError + ":\n" + ex.Message);
            }

            // вывод директории конфигурации
            txtConfigDir.Text = configDir;

            // определение директории веб-приложения
            webAppDir = configDir.EndsWith(@"\config\") ? configDir.Substring(0, configDir.Length - 7) : "";
        }

        /// <summary>
        /// Сохранить директорию конфигурации в файле
        /// </summary>
        private void SaveConfigDir()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);
                XmlElement rootElem = xmlDoc.CreateElement("ScadaWebConfig");
                xmlDoc.AppendChild(rootElem);
                rootElem.AppendParamElem("ConfigDir", configDir);
                xmlDoc.Save(exeDir + SettingsFileName);
            }
            catch (Exception ex)
            {
                ScadaUtils.ShowError(AppPhrases.SaveConfigDirError + ":\n" + ex.Message);
            }
        }

        /// <summary>
        /// Получить индекс типа представления в выпадающем списке
        /// </summary>
        private int GetViewTypeIndex(string viewType)
        {
            if (selViewInfo.Type == "WebPageView")
                return 3;
            else if (selViewInfo.Type == "FacesView")
                return 2;
            else if (selViewInfo.Type == "SchemeView")
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Получить тип представления по индексу в выпадающем списке
        /// </summary>
        private string GetViewType(int index)
        {
            if (index == 3)
                return "WebPageView";
            else if (index == 2)
                return "FacesView";
            else if (index == 1)
                return "SchemeView";
            else
                return "TableView";
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            // определение директории исполняемого файла приложения
            exeDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Application.ExecutablePath));

            // локализация приложения
            if (!Localization.UseRussian)
            {
                string langDir = exeDir + "Lang\\";
                string errMsg;

                if (Localization.LoadDictionaries(langDir, "ScadaData", out errMsg))
                    CommonPhrases.Init();
                else
                    ScadaUtils.ShowError(errMsg);

                if (Localization.LoadDictionaries(langDir, "ScadaWebConfig", out errMsg))
                {
                    Localization.TranslateForm(this, "ScadaWebConfig.FrmMain");
                    WebPhrases.Init();
                    AppPhrases.Init();
                    folderBrowserDialog.Description = AppPhrases.ChooseConfigDir;
                    openFileDialog.Title = AppPhrases.ChooseViewFile;
                    openFileDialog.Filter = AppPhrases.FileFilter;
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            LoadConfigDir();
            LoadSettings();
            ActiveControl = tabControl;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (commSettingsChanged || webSettingsChanged || viewSettingsChanged || regKeyChanged)
            {
                DialogResult dlgRes = MessageBox.Show(CommonPhrases.SaveSettingsConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (dlgRes)
                {
                    case DialogResult.Yes:
                        ControlsToSettings();
                        SaveSettings();
                        SaveConfigDir();
                        break;
                    case DialogResult.No:
                        SaveConfigDir();
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
            else
            {
                SaveConfigDir();
            }
        }


        private void btnConfigDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtConfigDir.Text.Trim();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = ScadaUtils.NormalDir(folderBrowserDialog.SelectedPath);

                if (configDir != selectedPath)
                {
                    configDir = txtConfigDir.Text = selectedPath;
                    LoadSettings();
                }
            }

            txtConfigDir.Focus();
            txtConfigDir.DeselectAll();
        }

        private void btnAddViewSet_Click(object sender, EventArgs e)
        {
            // добавление нового набора представлений
            int ind = viewSettings.ViewSetList.IndexOf(selViewSet) + 1;
            if (ind <= 0 || ind > viewSettings.ViewSetList.Count)
                ind = viewSettings.ViewSetList.Count;

            ViewSettings.ViewSet newTableSet = new ViewSettings.ViewSet(AppPhrases.NewViewSetName, "");
            viewSettings.ViewSetList.Insert(ind, newTableSet);
            TreeNode nodeTableSet = tvTableSets.Nodes.Insert(ind, newTableSet.Name);
            nodeTableSet.Tag = newTableSet;

            txtName.Enabled = true;
            txtDirOrFile.Enabled = true;
            tvTableSets.SelectedNode = nodeTableSet;
            ActiveControl = txtName;
            viewSets_Changed();
        }

        private void btnAddView_Click(object sender, EventArgs e)
        {
            // добавление нового представления
            ViewSettings.ViewSet viewSet = selViewSet;
            if (viewSet == null && viewSettings.ViewSetList.Count > 0)
                viewSet = viewSettings.ViewSetList[viewSettings.ViewSetList.Count - 1];

            if (viewSet == null)
            {
                ScadaUtils.ShowError(AppPhrases.ChooseSetToAddView);
            }
            else
            {
                int viewInd = viewSet.Items.IndexOf(selViewInfo) + 1;
                if (viewInd <= 0 || viewInd > viewSet.Count)
                    viewInd = viewSet.Count;

                ViewSettings.ViewInfo newViewInfo = new ViewSettings.ViewInfo(AppPhrases.NewViewTitle, "TableView", "");
                viewSet.Items.Insert(viewInd, newViewInfo);
                int tableSetInd = viewSettings.ViewSetList.IndexOf(viewSet);
                TreeNode nodeTable = tvTableSets.Nodes[tableSetInd].Nodes.Insert(viewInd, newViewInfo.Title);
                nodeTable.Tag = newViewInfo;

                txtName.Enabled = true;
                txtDirOrFile.Enabled = true;
                tvTableSets.SelectedNode = nodeTable;
                ActiveControl = txtName;
                viewSets_Changed();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // удаление набора или представления
            if (selViewSet == null && selViewInfo == null)
            {
                ScadaUtils.ShowError(AppPhrases.ChooseViewToDelete);
            }
            else
            {
                if (selViewInfo == null)
                {
                    // удаление набора таблиц
                    int ind = viewSettings.ViewSetList.IndexOf(selViewSet);
                    viewSettings.ViewSetList.RemoveAt(ind);
                    tvTableSets.Nodes.RemoveAt(ind);

                    // определение выбранного набора таблиц
                    if (--ind < 0)
                        ind = 0;

                    if (ind < viewSettings.ViewSetList.Count)
                    {
                        selViewSet = viewSettings.ViewSetList[ind];
                        tvTableSets.SelectedNode = tvTableSets.Nodes[ind];
                    }
                    else
                    {
                        selViewSet = null;
                        tvTableSets.SelectedNode = null;

                        txtName.Text = "";
                        txtName.Enabled = false;
                        lblDir.Visible = true;
                        lblFile.Visible = false;
                        txtDirOrFile.Text = "";
                        txtDirOrFile.Enabled = false;
                        btnSelectView.Visible = false;
                    }
                }
                else
                {
                    // удаление представления
                    int tableInd = selViewSet.Items.IndexOf(selViewInfo);
                    selViewSet.Items.RemoveAt(tableInd);
                    int tableSetInd = viewSettings.ViewSetList.IndexOf(selViewSet);
                    TreeNode nodeTableSet = tvTableSets.Nodes[tableSetInd];
                    nodeTableSet.Nodes.RemoveAt(tableInd);

                    // определение выбранного представления
                    if (--tableInd < 0)
                        tableInd = 0;

                    if (tableInd < selViewSet.Count)
                    {
                        selViewInfo = selViewSet[tableInd];
                        tvTableSets.SelectedNode = nodeTableSet.Nodes[tableInd];
                    }
                    else
                    {
                        selViewInfo = null;
                        tvTableSets.SelectedNode = nodeTableSet;
                    }
                }

                viewSets_Changed();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // перемещение набора или представления вверх
            if (selViewSet == null && selViewInfo == null)
            {
                ScadaUtils.ShowError(AppPhrases.ChooseViewToMove);
            }
            else
            {
                if (selViewInfo == null)
                {
                    // перемещение набора представлений
                    int ind = viewSettings.ViewSetList.IndexOf(selViewSet);

                    if (ind > 0)
                    {
                        viewSettings.ViewSetList.Reverse(ind - 1, 2);

                        TreeNode node = tvTableSets.Nodes[ind];
                        tvTableSets.BeginUpdate();
                        tvTableSets.Nodes.RemoveAt(ind);
                        tvTableSets.Nodes.Insert(ind - 1, node);
                        tvTableSets.EndUpdate();
                        tvTableSets.SelectedNode = node;
                    }
                }
                else
                {
                    // перемещение представления
                    int viewInd = selViewSet.Items.IndexOf(selViewInfo);

                    if (viewInd > 0)
                    {
                        selViewSet.Items.Reverse(viewInd - 1, 2);

                        int tableSetInd = viewSettings.ViewSetList.IndexOf(selViewSet);
                        TreeNode nodeTableSet = tvTableSets.Nodes[tableSetInd];
                        TreeNode nodeTable = nodeTableSet.Nodes[viewInd];
                        tvTableSets.BeginUpdate();
                        nodeTableSet.Nodes.RemoveAt(viewInd);
                        nodeTableSet.Nodes.Insert(viewInd - 1, nodeTable);
                        tvTableSets.EndUpdate();
                        tvTableSets.SelectedNode = nodeTable;
                    }
                }

                viewSets_Changed();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            // перемещение набора или представления вниз
            if (selViewSet == null && selViewInfo == null)
            {
                ScadaUtils.ShowError(AppPhrases.ChooseViewToMove);
            }
            else
            {
                if (selViewInfo == null)
                {
                    // перемещение набора представлений
                    int ind = viewSettings.ViewSetList.IndexOf(selViewSet);

                    if (ind < viewSettings.ViewSetList.Count - 1)
                    {
                        viewSettings.ViewSetList.Reverse(ind, 2);

                        TreeNode node = tvTableSets.Nodes[ind];
                        tvTableSets.BeginUpdate();
                        tvTableSets.Nodes.RemoveAt(ind);
                        tvTableSets.Nodes.Insert(ind + 1, node);
                        tvTableSets.EndUpdate();
                        tvTableSets.SelectedNode = node;
                    }
                }
                else
                {
                    // перемещение представления
                    int viewInd = selViewSet.Items.IndexOf(selViewInfo);

                    if (viewInd < selViewSet.Count - 1)
                    {
                        selViewSet.Items.Reverse(viewInd, 2);

                        int tableSetInd = viewSettings.ViewSetList.IndexOf(selViewSet);
                        TreeNode nodeTableSet = tvTableSets.Nodes[tableSetInd];
                        TreeNode nodeTable = nodeTableSet.Nodes[viewInd];
                        tvTableSets.BeginUpdate();
                        nodeTableSet.Nodes.RemoveAt(viewInd);
                        nodeTableSet.Nodes.Insert(viewInd + 1, nodeTable);
                        tvTableSets.EndUpdate();
                        tvTableSets.SelectedNode = nodeTable;
                    }
                }
                viewSets_Changed();
            }
        }

        private void btnSelectView_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // определение наименования, имени файла и типа представления
                string fname = openFileDialog.FileName;
                string ext = Path.GetExtension(fname);

                string viewFile = "";
                string viewTitle = "";
                int viewTypeInd = 0;

                if (ext == ".tbl" || ext == ".ofm" || ext == ".sch" || ext == ".fcs")
                {
                    string dir = selViewSet == null ? "" : ScadaUtils.NormalDir(selViewSet.Directory);
                    int pos = dir == "" ? -1 : fname.IndexOf(dir);
                    viewFile = pos >= 0 ? fname.Substring(pos + dir.Length) : Path.GetFileName(fname);

                    if (ext == ".sch")
                        viewTypeInd = 1;
                    else if (ext == ".fcs")
                        viewTypeInd = 2;

                    try
                    {
                        using (StreamReader reader = new StreamReader(fname, Encoding.Default))
                        {
                            if (ext == ".ofm")
                            {
                                string line = reader.ReadLine();
                                pos = line.IndexOf(": ");
                                viewTitle = pos >= 0 ? line.Substring(pos + 2) : line;
                            }
                            else
                            {
                                reader.ReadLine();
                                string line = reader.ReadLine();
                                pos = line.IndexOf("title=\"");
                                if (pos >= 0)
                                {
                                    int quotPos = line.LastIndexOf("\"");
                                    viewTitle = line.Substring(pos + 7, quotPos - pos - 7);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScadaUtils.ShowError(AppPhrases.ParseViewTitleError + ":\n" + ex.Message);
                    }
                }
                else if (ext == ".aspx" || ext == ".htm" || ext == ".html")
                {
                    int pos = webAppDir == "" ? -1 : fname.IndexOf(webAppDir);
                    viewFile = pos >= 0 ? fname.Substring(pos + webAppDir.Length).Replace('\\', '/') : fname;
                    viewTypeInd = 3;
                }

                txtDirOrFile.Text = viewFile;
                txtName.Text = viewTitle;
                cbType.SelectedIndex = viewTypeInd;
            }
        }


        private void tvTableSets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // определение выбранного набора или представления, вывод их свойств
            TreeNode node = e.Node;
            selViewSet = node.Tag as ViewSettings.ViewSet;
            selViewInfo = node.Tag as ViewSettings.ViewInfo;

            txtName.TextChanged -= txtName_TextChanged;
            txtDirOrFile.TextChanged -= txtDirOrFile_TextChanged;
            cbType.SelectedIndexChanged -= cbType_SelectedIndexChanged;

            if (selViewSet != null)
            {
                txtName.Text = selViewSet.Name;
                lblDir.Visible = true;
                lblFile.Visible = false;
                txtDirOrFile.Text = selViewSet.Directory;
                btnSelectView.Visible = false;
                lblType.Visible = cbType.Visible = false;
            }
            else if (selViewInfo != null)
            {
                selViewSet = node.Parent.Tag as ViewSettings.ViewSet;
                txtName.Text = selViewInfo.Title;
                lblDir.Visible = false;
                lblFile.Visible = true;
                txtDirOrFile.Text = selViewInfo.FileName;
                btnSelectView.Visible = true;
                lblType.Visible = cbType.Visible = true;
                cbType.SelectedIndex = GetViewTypeIndex(selViewInfo.Type);
            }
            else
            {
                txtName.Text = "";
                lblDir.Visible = true;
                lblFile.Visible = false;
                txtDirOrFile.Text = "";
                btnSelectView.Visible = false;
                lblType.Visible = cbType.Visible = false;
            }

            txtName.TextChanged += txtName_TextChanged;
            txtDirOrFile.TextChanged += txtDirOrFile_TextChanged;
            cbType.SelectedIndexChanged += cbType_SelectedIndexChanged;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // сохранение в настройках представлений наименования набора или представления
            TreeNode selNode = tvTableSets.SelectedNode;
            string text = txtName.Text;

            if (selNode != null)
            {
                if (selViewInfo != null)
                {
                    selViewInfo.Title = text;
                    selNode.Text = text;
                } 
                else if (selViewSet != null)
                {
                    selViewSet.Name = text;
                    selNode.Text = text;
                }

                viewSets_Changed();
            }
        }

        private void txtDirOrFile_TextChanged(object sender, EventArgs e)
        {
            // сохранение в настройках представлений директории набора или файла представления
            if (selViewInfo != null)
                selViewInfo.FileName = txtDirOrFile.Text;
            else if (selViewSet != null)
                selViewSet.Directory = txtDirOrFile.Text;
            viewSets_Changed();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // сохранение в настройках представлений типа представления
            if (selViewInfo != null)
                selViewInfo.Type = GetViewType(cbType.SelectedIndex);
            viewSets_Changed();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            ControlsToSettings();
            SaveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            commSettings = loadedCommSettings.Clone();
            viewSettings = loadedViewSettings.Clone();
            regKey = loadedRegKey;
            SettingsToControls();
            SetSettingsUnchanged();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void dataSettings_Changed(object sender, EventArgs e)
        {
            if (!showing && !commSettingsChanged)
            {
                commSettingsChanged = true;
                tabComm.Text += "*";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void webSettings_Changed(object sender, EventArgs e)
        {
            if (!showing && !webSettingsChanged)
            {
                webSettingsChanged = true;
                tabWeb.Text += "*";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void viewSets_Changed()
        {
            if (!showing && !viewSettingsChanged)
            {
                viewSettingsChanged = true;
                tabView.Text += "*";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }
    }
}
