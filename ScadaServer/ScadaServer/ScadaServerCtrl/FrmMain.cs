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
 * Module   : SCADA-Server Control
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2018
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Server.Modules;
using Scada.Svc;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Scada.Server.Ctrl
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Информация, ассоциированная с узлом дерева
        /// </summary>
        private class NodeTag
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public NodeTag(params TabPage[] tabPages)
            {
                TabPages = tabPages;
            }

            /// <summary>
            /// Получить или установить страницы, ассоциированные с узлом дерева
            /// </summary>
            public TabPage[] TabPages { get; set; }
        }


        /// <summary>
        /// Имя файла состояния
        /// </summary>
        private const string StateFileName = "ScadaServerSvc.txt";
        /// <summary>
        /// Имя файла журнала
        /// </summary>
        private const string LogFileName = "ScadaServerSvc.log";
        /// <summary>
        /// Имя файла ошибок
        /// </summary>
        private const string ErrFileName = "ScadaServerCtrl.err";
        /// <summary>
        /// Имя службы по умолчанию
        /// </summary>
        private const string DefServiceName = "ScadaServerService";
        /// <summary>
        /// Интервал ожидания перезапуска службы
        /// </summary>
        private static readonly TimeSpan WaitForRestartSpan = TimeSpan.FromSeconds(30);
        /// <summary>
        /// Значения периода записи текущего среза, соответствующие элементам выпадающего списка
        /// </summary>
        private static readonly int[] WriteCurPerItemVals = { 0, 1, 2, 3, 4, 5, 10, 20, 30, 60 };
        /// <summary>
        /// Значения времени установки недостоверности при неактивности, соответствующие элементам выпадающего списка
        /// </summary>
        private static readonly int[] InactUnrelTimeItemVals = { 0, 1, 2, 3, 4, 5, 10, 20, 30, 60 };
        /// <summary>
        /// Значения периода записи минутных срезов, соответствующие элементам выпадающего списка
        /// </summary>
        private static readonly int[] WriteMinPerItemVals = { 30, 60, 120, 180, 240, 300, 600 };

        private AppDirs appDirs;              // директории приложения
        private Log errLog;                   // журнал ошибок приложения
        private Mutex mutex;                  // объект для проверки запуска второй копии программы
        private string serviceName;           // имя службы
        private ServiceController svcContr;   // контроллер службы
        private ServiceControllerStatus prevSvcStatus; // предыдущее состояние службы
        private Icon[] notifyIcons;           // значки для представления состояния службы
        private int notifyIconIndex;          // индекс текущего значка для представления состояния службы
        private string stateFileName;         // полное имя файла состояния
        private string logFileName;           // полное имя файла журнала
        private DateTime stateFileAge;        // время изменения файла состояния
        private DateTime logFileAge;          // время изменения файла журнала

        private TreeNode nodeCommonParams;    // узел общих параметров
        private TreeNode nodeDirectories;     // узел директорий
        private TreeNode nodeSaveParams;      // узел параметров записи данных
        private TreeNode nodeFiles;           // узел файлов данных
        private TreeNode nodeBase;            // узел файлов базы конфигурации
        private TreeNode nodeCurSrez;         // узел файлов текущего среза
        private TreeNode nodeMinSrez;         // узел файлов минутных срезов
        private TreeNode nodeHrSrez;          // узел файлов часовых срезов
        private TreeNode nodeEvents;          // узел файлов событий
        private TreeNode nodeModules;         // узел модулей
        private TreeNode nodeGenerator;       // узел генератора
        private TreeNode nodeStats;           // узел статистики
        private TreeNode lastNode;            // последний выбранный узел дерева, с которым связаны отображаемые страницы

        private Settings settings;            // настройки приложения
        private CommSettings commSettings;    // настройки соединения
        private ServerComm serverComm;        // объект для обмена данными со SCADA-Сервером
        private bool changing;                // признак программного изменения элементов управления
        private Dictionary<string, ModView> modViewDict; // словарь пользовательских интерфейсов модулей
        private ModView lastModView;          // пользовательский интерфейс последнего выбранного модуля


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            // установка формата дат и времени элементов управления
            dtpSrezDate.CustomFormat = dtpEvDate1.CustomFormat = dtpEvDate2.CustomFormat =
                Localization.Culture.DateTimeFormat.ShortDatePattern;
            dtpSrezTime.CustomFormat = dtpEvTime.CustomFormat =
                Localization.Culture.DateTimeFormat.LongTimePattern;

            // инициализация полей
            appDirs = new AppDirs();
            errLog = new Log(Log.Formats.Simple);
            errLog.Encoding = Encoding.UTF8;
            mutex = null;
            serviceName = DefServiceName;
            svcContr = null;
            prevSvcStatus = ServiceControllerStatus.Stopped;

            notifyIcons = new Icon[ilNotify.Images.Count];
            for (int i = 0; i < notifyIcons.Length; i++)
                notifyIcons[i] = Icon.FromHandle(((Bitmap)ilNotify.Images[i]).GetHicon());
            notifyIconIndex = 0;

            stateFileName = "";
            logFileName = "";
            stateFileAge = DateTime.MinValue;
            logFileAge = DateTime.MinValue;

            nodeCommonParams = treeView.Nodes["nodeCommonParams"];
            nodeSaveParams = treeView.Nodes["nodeSaveParams"];
            nodeDirectories = treeView.Nodes["nodeDirectories"];
            nodeFiles = treeView.Nodes["nodeFiles"];
            nodeBase = nodeFiles.Nodes["nodeBase"];
            nodeCurSrez = nodeFiles.Nodes["nodeCurSrez"];
            nodeMinSrez = nodeFiles.Nodes["nodeMinSrez"];
            nodeHrSrez = nodeFiles.Nodes["nodeHrSrez"];
            nodeEvents = nodeFiles.Nodes["nodeEvents"];
            nodeModules = treeView.Nodes["nodeModules"];
            nodeGenerator = treeView.Nodes["nodeGenerator"];
            nodeStats = treeView.Nodes["nodeStats"];
            lastNode = null;

            settings = new Settings();
            commSettings = new CommSettings();
            serverComm = null;
            changing = false;
            modViewDict = new Dictionary<string, ModView>();
            lastModView = null;

            Application.ThreadException += Application_ThreadException;
        }


        /// <summary>
        /// Получить объект для обмена данными с сервером, создав его при необходимости
        /// </summary>
        private ServerComm ServerComm
        {
            get
            {
                if (serverComm == null)
                    serverComm = new ServerComm(commSettings, new LogStub());
                return serverComm;
            }
        }


        /// <summary>
        /// Отобразить форму - развернуть и сделать активной
        /// </summary>
        private void ShowForm()
        {
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();

            // развёртывание узла файлов данных, т.к. состояние узлов не сохраняется при скрытии формы
            nodeFiles.Expand();
        }

        /// <summary>
        /// Загрузить локализацию приложения
        /// </summary>
        private void Localize(StringBuilder sbError)
        {
            string errMsg;

            if (Localization.LoadDictionaries(appDirs.LangDir, "ScadaData", out errMsg))
                CommonPhrases.Init();
            else
                sbError.AppendLine(errMsg);

            if (Localization.LoadDictionaries(appDirs.LangDir, "ScadaServer", out errMsg))
            {
                ModPhrases.InitFromDictionaries();
                Translator.TranslateForm(this, "Scada.Server.Ctrl.FrmMain", toolTip, cmsNotify);
                AppPhrases.Init();
                dlgMod.Filter = AppPhrases.ModuleFileFilter;
                TranslateTree();
            }
            else
            {
                sbError.AppendLine(errMsg);
            }
        }

        /// <summary>
        /// Загрузить имя службы
        /// </summary>
        private void LoadServiceName(StringBuilder sbError)
        {
            SvcProps svcProps = new SvcProps();
            string svcPropsFileName = appDirs.ExeDir + SvcProps.SvcPropsFileName;

            if (File.Exists(svcPropsFileName))
            {
                string errMsg;
                if (svcProps.LoadFromFile(svcPropsFileName, out errMsg))
                    serviceName = svcProps.ServiceName;
                else
                    sbError.AppendLine(errMsg);
            }
        }

        /// <summary>
        /// Проверить, что вторая копия приложения не запущена
        /// </summary>
        private void CheckSecondInstance(StringBuilder sbError, out bool closeApp)
        {
            closeApp = false;

            try
            {
                bool createdNew;
                mutex = new Mutex(true, "ScadaServerCtrlMutex - " + serviceName, out createdNew);

                if (!createdNew)
                    closeApp = true;
            }
            catch (Exception ex)
            {
                sbError.AppendLine(AppPhrases.CheckSecondInstanceError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Установить текст строки состояния в зависимости от статуса службы
        /// </summary>
        private void SetServiceStateText(ServiceControllerStatus? status)
        {
            string state;

            switch (status)
            {
                case ServiceControllerStatus.ContinuePending:
                    state = CommonPhrases.ContinuePendingSvcState;
                    break;
                case ServiceControllerStatus.Paused:
                    state = CommonPhrases.PausedSvcState;
                    break;
                case ServiceControllerStatus.PausePending:
                    state = CommonPhrases.PausedSvcState;
                    break;
                case ServiceControllerStatus.Running:
                    state = CommonPhrases.RunningSvcState;
                    break;
                case ServiceControllerStatus.StartPending:
                    state = CommonPhrases.StartPendingSvcState;
                    break;
                case ServiceControllerStatus.Stopped:
                    state = CommonPhrases.StoppedSvcState;
                    break;
                case ServiceControllerStatus.StopPending:
                    state = CommonPhrases.StopPendingSvcState;
                    break;
                default: // null
                    state = CommonPhrases.NotInstalledSvcState;
                    break;
            }

            lblServiceState.Text = string.Format(AppPhrases.ServiceState, serviceName, state);
        }

        /// <summary>
        /// Установить разрешения кнопок управления службой
        /// </summary>
        private void SetServiceButtonsEnabled(ServiceControllerStatus? status)
        {
            if (status == ServiceControllerStatus.Running)
            {
                btnServiceStart.Enabled = miNotifyStart.Enabled = false;
                btnServiceStop.Enabled = miNotifyStop.Enabled = true;
                btnServiceRestart.Enabled = miNotifyRestart.Enabled = true;
            }
            else if (status == ServiceControllerStatus.Stopped)
            {
                btnServiceStart.Enabled = miNotifyStart.Enabled = true;
                btnServiceStop.Enabled = miNotifyStop.Enabled = false;
                btnServiceRestart.Enabled = miNotifyRestart.Enabled = false;
            }
            else
            {
                btnServiceStart.Enabled = miNotifyStart.Enabled = false;
                btnServiceStop.Enabled = miNotifyStop.Enabled = false;
                btnServiceRestart.Enabled = miNotifyRestart.Enabled = false;
            }
        }

        /// <summary>
        /// Перевести текст узлов дерева
        /// </summary>
        private void TranslateTree()
        {
            nodeCommonParams.Text = AppPhrases.CommonParamsNode;
            nodeSaveParams.Text = AppPhrases.SaveParamsNode;
            nodeBase.Text = AppPhrases.BaseNode;
            nodeCurSrez.Text = AppPhrases.CurSrezNode;
            nodeMinSrez.Text = AppPhrases.MinSrezNode;
            nodeHrSrez.Text = AppPhrases.HrSrezNode;
            nodeEvents.Text = AppPhrases.EventsNode;
            nodeModules.Text = AppPhrases.ModulesNode;
            nodeGenerator.Text = AppPhrases.GeneratorNode;
            nodeStats.Text = AppPhrases.StatsNode;
            nodeFiles.Text = AppPhrases.FilesNode;
        }

        /// <summary>
        /// Подготовить дерево проводника к работе
        /// </summary>
        private void PrepareTree()
        {
            nodeCommonParams.Tag = new NodeTag(pageCommonParams);
            nodeSaveParams.Tag = new NodeTag(pageSaveParams);
            nodeBase.Tag = new NodeTag(pageFiles);
            nodeCurSrez.Tag = new NodeTag(pageFiles);
            nodeMinSrez.Tag = new NodeTag(pageFiles);
            nodeHrSrez.Tag = new NodeTag(pageFiles);
            nodeEvents.Tag = new NodeTag(pageFiles);
            nodeModules.Tag = new NodeTag(pageModules);
            nodeGenerator.Tag = new NodeTag(pageGenSrez, pageGenEv, pageGenCmd);
            nodeStats.Tag = new NodeTag(pageStats);
            nodeFiles.Expand();
            treeView.SelectedNode = nodeCommonParams;
        }

        /// <summary>
        /// Подготовить страницу файлов к работе
        /// </summary>
        private void PrepareFilesPage()
        {
            // настройка элементов управления и определение директории просматриваемых данных
            if (lastNode == nodeBase)
            {
                rbFilesMain.CheckedChanged -= rbFiles_CheckedChanged;
                rbFilesMain.Checked = true;
                rbFilesMain.CheckedChanged += rbFiles_CheckedChanged;
                rbFilesMain.Enabled = rbFilesCopy.Enabled = false;
                txtDataDir.Text = settings.BaseDATDir;
            }
            else
            {
                rbFilesMain.Enabled = rbFilesCopy.Enabled = true;
                string arcDir = rbFilesMain.Checked ? settings.ArcDir : settings.ArcCopyDir;

                if (lastNode == nodeCurSrez)
                    txtDataDir.Text = arcDir + @"Cur\";
                else if (lastNode == nodeMinSrez)
                    txtDataDir.Text = arcDir + @"Min\";
                else if (lastNode == nodeHrSrez)
                    txtDataDir.Text = arcDir + @"Hour\";
                else if (lastNode == nodeEvents)
                    txtDataDir.Text = arcDir + @"Events\";
                else
                    txtDataDir.Text = "";
            }

            // получение списка файлов данных
            lbFiles.Items.Clear();

            if (txtDataDir.Text != "")
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(txtDataDir.Text);
                    FileInfo[] fileInfoArr = dirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in fileInfoArr)
                        lbFiles.Items.Add(Path.GetFileName(fileInfo.Name));
                }
                catch (Exception ex)
                {
                    string errMsg = AppPhrases.GetFileListError + ":\r\n" + ex.Message;
                    errLog.WriteAction(errMsg);
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // выбор первого файла в списке и установка доступности кнопок просмотра и редактирования файлов
            if (lbFiles.Items.Count > 0)
            {
                lbFiles.SelectedIndex = 0;
                btnViewFile.Enabled = true;
                btnEditFile.Enabled = lastNode != nodeBase;
            }
            else
            {
                btnViewFile.Enabled = btnEditFile.Enabled = false;
            }
        }

        /// <summary>
        /// Установить доступность кнопок действий с модулями
        /// </summary>
        private void SetModulesButtonsEnabled()
        {
            int selInd = lbModDll.SelectedIndex;
            btnMoveUpMod.Enabled = selInd > 0;
            btnMoveDownMod.Enabled = selInd < lbModDll.Items.Count - 1;
            btnDelMod.Enabled = selInd >= 0;
            if (selInd < 0)
                btnModProps.Enabled = false;
        }

        /// <summary>
        /// Настроить элементы управления генератора команды
        /// </summary>
        private void TuneGenCmd()
        {
            const int margin = 6;

            if (rbCmdStand.Checked)
            {
                pnlCmdVal.Visible = true;
                pnlCmdData.Visible = false;
                pnlCmdKP.Visible = false;
                btnSendCmd.Top = pnlCmdVal.Bottom + margin;
            }
            else if (rbCmdBin.Checked)
            {
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = true;
                pnlCmdKP.Visible = false;
                btnSendCmd.Top = pnlCmdData.Bottom + margin;
            }
            else // rbCmdReq.Checked
            {
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = false;
                pnlCmdKP.Visible = true;
                btnSendCmd.Top = pnlCmdKP.Bottom + margin;
            }

            gbGenCmd.Height = btnSendCmd.Bottom + gbGenCmd.Padding.Bottom + 2;
        }

        /// <summary>
        /// Выполнить действия при изменении настроек
        /// </summary>
        private void SetModified()
        {
            // разрешение кнопок применения и отмены настроек
            btnSettingsApply.Enabled = true;
            btnSettingsCancel.Enabled = true;
        }

        /// <summary>
        /// Установить значения элементов управления в соответствии с настройками
        /// </summary>
        private void SettingsToControls()
        {
            changing = true;

            // общие параметры
            numTcpPort.SetValue(settings.TcpPort);
            chkUseAD.Checked = settings.UseAD;
            txtLdapPath.Text = settings.LdapPath;

            // директории системы
            txtBaseDATDir.Text = settings.BaseDATDir;
            txtItfDir.Text = settings.ItfDir;
            txtArcDir.Text = settings.ArcDir;
            txtArcCopyDir.Text = settings.ArcCopyDir;

            // запись данных
            int ind = Array.IndexOf<int>(WriteCurPerItemVals, settings.WriteCurPer);
            cbWriteCurPer.SelectedIndex = ind >= 0 ? ind : 0;
            ind = Array.IndexOf<int>(InactUnrelTimeItemVals, settings.InactUnrelTime);
            cbInactUnrelTime.SelectedIndex = ind >= 0 ? ind : 0;
            chkWriteCur.Checked = settings.WriteCur;
            chkWriteCurCopy.Checked = settings.WriteCurCopy;

            ind = Array.IndexOf<int>(WriteMinPerItemVals, settings.WriteMinPer);
            cbWriteMinPer.SelectedIndex = ind >= 0 ? ind : 0;
            numStoreMinPer.SetValue(settings.StoreMinPer);
            chkWriteMin.Checked = settings.WriteMin;
            chkWriteMinCopy.Checked = settings.WriteMinCopy;

            cbWriteHrPer.SelectedIndex = settings.WriteHrPer == 1800 /*30 минут*/ ? 0 : 1;
            numStoreHrPer.SetValue(settings.StoreHrPer);
            chkWriteHr.Checked = settings.WriteHr;
            chkWriteHrCopy.Checked = settings.WriteHrCopy;

            numStoreEvPer.SetValue(settings.StoreEvPer);
            chkWriteEv.Checked = settings.WriteEv;
            chkWriteEvCopy.Checked = settings.WriteEvCopy;

            // модули
            lbModDll.Items.Clear();
            lbModDll.Items.AddRange(settings.ModuleFileNames.ToArray());

            if (lbModDll.Items.Count > 0)
            {
                lbModDll.SelectedIndex = 0;
            }
            else
            {
                SetModulesButtonsEnabled();
                lastModView = null;
            }
            
            // статистика
            chkDetailedLog.Checked = settings.DetailedLog;

            changing = false;
        }

        /// <summary>
        /// Установить настройки в соответствии с элементами управления 
        /// </summary>
        private void ControlsToSettings(Settings settings)
        {
            // общие параметры
            settings.TcpPort = decimal.ToInt32(numTcpPort.Value);
            settings.UseAD = chkUseAD.Checked;
            settings.LdapPath = txtLdapPath.Text;

            // директории системы
            settings.BaseDATDir = txtBaseDATDir.Text;
            settings.ItfDir = txtItfDir.Text;
            settings.ArcDir = txtArcDir.Text;
            settings.ArcCopyDir = txtArcCopyDir.Text;

            // запись данных
            settings.WriteCurPer = WriteCurPerItemVals[cbWriteCurPer.SelectedIndex];
            settings.InactUnrelTime = InactUnrelTimeItemVals[cbInactUnrelTime.SelectedIndex];
            settings.WriteCur = chkWriteCur.Checked;
            settings.WriteCurCopy = chkWriteCurCopy.Checked;

            settings.WriteMinPer = WriteMinPerItemVals[cbWriteMinPer.SelectedIndex];
            settings.StoreMinPer = decimal.ToInt32(numStoreMinPer.Value);
            settings.WriteMin = chkWriteMin.Checked;
            settings.WriteMinCopy = chkWriteMinCopy.Checked;

            settings.WriteHrPer = cbWriteHrPer.SelectedIndex > 0 ? 3600 /*1 час*/ : 1800 /*30 минут*/;
            settings.StoreHrPer = decimal.ToInt32(numStoreHrPer.Value);
            settings.WriteHr = chkWriteHr.Checked;
            settings.WriteHrCopy = chkWriteHrCopy.Checked;

            settings.StoreEvPer = decimal.ToInt32(numStoreEvPer.Value);
            settings.WriteEv = chkWriteEv.Checked;
            settings.WriteEvCopy = chkWriteEvCopy.Checked;

            // модули
            settings.ModuleFileNames.Clear();
            settings.ModuleFileNames.AddRange(lbModDll.Items.Cast<string>());

            // статистика
            settings.DetailedLog = chkDetailedLog.Checked;
        }

        /// <summary>
        /// Применить изменения настроек
        /// </summary>
        private bool ApplySettings()
        {
            // фиксация изменений для активного элемента управления типа NumericUpDown
            if (ActiveControl is NumericUpDown)
            {
                Control control = ActiveControl;
                ActiveControl = null;
                ActiveControl = control;
            }

            // применение изменений настроек
            Settings newSettings = new Settings();
            ControlsToSettings(newSettings);
            string errMsg;

            if (newSettings.Save(appDirs.ConfigDir + Settings.DefFileName, out errMsg))
            {
                settings = newSettings;
                btnSettingsApply.Enabled = false;
                btnSettingsCancel.Enabled = false;
                ScadaUiUtils.ShowInfo(AppPhrases.RestartNeeded);
                return true;
            }
            else
            {
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Получить пользовательский интерфейс модуля из словаря, создав его при необходимости
        /// </summary>
        private ModView GetModView(string fileName, out string errMsg)
        {
            ModView modView;

            if (modViewDict.TryGetValue(fileName, out modView))
            {
                errMsg = "";
                return modView;
            }
            else
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(appDirs.ModDir + fileName);
                    Type type = asm.GetType("Scada.Server.Modules." +
                        Path.GetFileNameWithoutExtension(fileName) + "View", true);
                    modView = Activator.CreateInstance(type) as ModView;
                    modView.AppDirs = appDirs;
                    modView.ServerComm = ServerComm;
                    modViewDict.Add(fileName, modView);

                    errMsg = "";
                    return modView;
                }
                catch (Exception ex)
                {
                    errMsg = AppPhrases.LoadModuleError + ":\r\n" + ex.Message;
                    return null;
                }
            }
        }


        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            errLog.WriteException(e.Exception, CommonPhrases.UnhandledException);
            ScadaUiUtils.ShowError(CommonPhrases.UnhandledException + ":\r\n" + e.Exception.Message);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // инициализация директорий приложения
            appDirs.Init(Path.GetDirectoryName(Application.ExecutablePath));

            // установка имён файлов журналов
            errLog.FileName = appDirs.LogDir + ErrFileName;
            stateFileName = appDirs.LogDir + StateFileName;
            logFileName = appDirs.LogDir + LogFileName;

            // переменные для записи информации об ошибках
            StringBuilder sbError = new StringBuilder();
            string errMsg;

            // локализация приложения
            Localize(sbError);

            // загрузка имени службы
            LoadServiceName(sbError);

            // проверка запуска второй копии программы
            bool closeApp;
            CheckSecondInstance(sbError, out closeApp);

            if (closeApp)
            {
                ScadaUiUtils.ShowInfo(AppPhrases.SecondInstanceClosed);
                Close();
                return;
            }

            // подготовка интерфейса
            tabControl.TabPages.Clear();
            PrepareTree();
            lblCurDate.Text = "";
            lblCurTime.Text = "";
            cbWriteEvPer.SelectedIndex = 0;
            dtpSrezDate.Value = dtpSrezTime.Value = dtpEvDate1.Value =
                dtpEvTime.Value = dtpEvDate2.Value = DateTime.Today;
            pnlCmdData.Top = pnlCmdKP.Top = pnlCmdVal.Top;
            TuneGenCmd();
            notifyIcon.Text = serviceName == DefServiceName ? AppPhrases.MainFormTitle : serviceName;

            // определение состояния службы
            try
            {
                svcContr = new ServiceController(serviceName);
                prevSvcStatus = svcContr.Status;
                SetServiceStateText(prevSvcStatus);
                SetServiceButtonsEnabled(prevSvcStatus);
            }
            catch
            {
                sbError.AppendLine(AppPhrases.ServiceNotInstalled);
                SetServiceStateText(null);
                SetServiceButtonsEnabled(null);
            }

            // загрузка и отображение настроек приложения
            if (!settings.Load(appDirs.ConfigDir + Settings.DefFileName, out errMsg))
                sbError.AppendLine(errMsg);
            SettingsToControls();

            // загрузка настроек соединения
            string fileName = appDirs.ConfigDir + CommSettings.DefFileName;
            if (File.Exists(fileName) && !commSettings.LoadFromFile(fileName, out errMsg))
                sbError.AppendLine(errMsg);

            // запуск таймера для обновления информации на форме
            tmrRefr.Enabled = true;

            // вывод сообщения об ошибке или успешный запуск
            if (sbError.Length > 0)
            {
                ShowForm();
                errMsg = sbError.ToString().TrimEnd();
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
            else
            {
                Hide();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверка необходимости сохранения изменений
            if (btnSettingsApply.Enabled)
            {
                DialogResult dlgRes = MessageBox.Show(AppPhrases.SaveSettingsConfirm, 
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                    e.Cancel = !ApplySettings();
                else if (dlgRes != DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            // скрытие формы
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                ShowInTaskbar = false;
            }
        }


        private void btnServiceStart_Click(object sender, EventArgs e)
        {
            // запуск службы
            try
            {
                svcContr.Start();
            }
            catch (Exception ex)
            {
                ShowForm();
                string errMsg = AppPhrases.ServiceStartFailed + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnServiceStop_Click(object sender, EventArgs e)
        {
            // остановка службы
            try
            {
                svcContr.Stop();
            }
            catch (Exception ex)
            {
                ShowForm();
                string errMsg = AppPhrases.ServiceStopFailed + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnServiceRestart_Click(object sender, EventArgs e)
        {
            // перезапуск службы
            try
            {
                svcContr.Stop();
                svcContr.WaitForStatus(ServiceControllerStatus.Stopped, WaitForRestartSpan);
                svcContr.Start();
            }
            catch (Exception ex)
            {
                ShowForm();
                string errMsg = AppPhrases.ServiceRestartFailed + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnSettingsApply_Click(object sender, EventArgs e)
        {
            // применение изменений настроек
            ApplySettings();
        }

        private void btnSettingsCancel_Click(object sender, EventArgs e)
        {
            // отмена изменений настроек
            SettingsToControls();
            btnSettingsApply.Enabled = false;
            btnSettingsCancel.Enabled = false;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            FrmAbout.ShowAbout(appDirs.ExeDir, errLog);
        }


        private void tmrRefr_Tick(object sender, EventArgs e)
        {
            // вывод текущей даты и времени
            DateTime nowDT = DateTime.Now;
            lblCurDate.Text = nowDT.ToString("d", Localization.Culture);
            lblCurTime.Text = nowDT.ToString("T", Localization.Culture);

            // обработка состояния службы
            if (svcContr != null)
            {
                try
                {
                    svcContr.Refresh();
                    ServiceControllerStatus newSvcStatus = svcContr.Status;

                    if (newSvcStatus != prevSvcStatus)
                    {
                        SetServiceStateText(newSvcStatus);
                        SetServiceButtonsEnabled(newSvcStatus);
                    }

                    if (newSvcStatus == ServiceControllerStatus.Running)
                    {
                        if (++notifyIconIndex == notifyIcons.Length)
                            notifyIconIndex = 0;
                        notifyIcon.Icon = notifyIcons[notifyIconIndex];
                    }

                    prevSvcStatus = newSvcStatus;
                }
                catch
                {
                    SetServiceStateText(null);
                    SetServiceButtonsEnabled(null);
                }
            }

            // обновление журналов
            if (WindowState == FormWindowState.Normal && lastNode == nodeStats)
            {
                lbAppState.ReloadItems(stateFileName, true, ref stateFileAge);
                lbAppLog.ReloadItems(logFileName, false, ref logFileAge);
            }
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            // отображение формы по щелчку мыши на пиктограмме
            if (e.Button == MouseButtons.Left)
                ShowForm();
        }

        private void miNotifyOpen_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void miNotifyExit_Click(object sender, EventArgs e)
        {
            Close();
        }        
        
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            NodeTag nodeTag = node.Tag as NodeTag;

            if (node != null && node != lastNode && nodeTag != null)
            {
                Text = AppPhrases.MainFormTitle + " - " + node.Text;
                lastNode = node;

                // отображение ассоциированных с узлом дерева страниц
                tabControl.TabPages.Clear();
                foreach (TabPage tabPage in nodeTag.TabPages)
                {
                    tabControl.TabPages.Add(tabPage);

                    if (tabPage == pageFiles)
                        PrepareFilesPage();
                    else if (tabPage == pageGenSrez)
                        txtGenPwd1.Text = "";
                    else if (tabPage == pageStats)
                        lbAppLog.SelectedIndex = -1; // для прокрутки журанала в конец
                }

                if (!treeView.Focused)
                    treeView.Focus();
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // выбор узла дерева по щелчку правой кнопкой мыши
            if (e.Button == MouseButtons.Right && e.Node != null)
                treeView.SelectedNode = e.Node;
        }


        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                SetModified();
        }

        private void btnDir_Click(object sender, EventArgs e)
        {
            // выбор директории
            Button btnDir = (Button)sender;
            TextBox txtDir;
            string descr;

            if (btnDir == btnBaseDATDir)
            {
                txtDir = txtBaseDATDir;
                descr = CommonPhrases.ChooseBaseDATDir;
            }
            else if (btnDir == btnItfDir)
            {
                txtDir = txtItfDir;
                descr = AppPhrases.ChooseItfDir;
            }
            else if (btnDir == btnArcDir)
            {
                txtDir = txtArcDir;
                descr = AppPhrases.ChooseArcDir;
            }
            else if (btnDir == btnArcCopyDir)
            {
                txtDir = txtArcCopyDir;
                descr = AppPhrases.ChooseArcCopyDir;
            }
            else
            {
                txtDir = null;
                descr = "";
            }

            if (txtDir != null)
            {
                dlgDir.SelectedPath = txtDir.Text.Trim();
                dlgDir.Description = descr;
                if (dlgDir.ShowDialog() == DialogResult.OK)
                    txtDir.Text = ScadaUtils.NormalDir(dlgDir.SelectedPath);
                txtDir.Focus();
                txtDir.DeselectAll();
            }
        }

        private void rbFiles_CheckedChanged(object sender, EventArgs e)
        {
            PrepareFilesPage();
        }

        private void lbFiles_DoubleClick(object sender, EventArgs e)
        {
            // просмотр файла по двойному щелчку
            if (lbFiles.SelectedIndex >= 0)
                btnViewFile_Click(null, null);
        }

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            // просмотр файла по нажатию Enter
            if (e.KeyCode == Keys.Enter)
                btnViewFile_Click(null, null);
        }

        private void btnViewFile_Click(object sender, EventArgs e)
        {
            // просмотр таблицы базы конфигурации, срезов или событий
            if (lbFiles.SelectedItem != null)
            {
                string dataDir = txtDataDir.Text;
                string tableName = lbFiles.SelectedItem.ToString();

                if (lastNode == nodeBase)
                    FrmBaseTableView.Show(dataDir, tableName, errLog);
                else if (lastNode == nodeCurSrez || lastNode == nodeMinSrez || lastNode == nodeHrSrez)
                    FrmSrezTableEdit.Show(dataDir, tableName, false, errLog);
                else if (lastNode == nodeEvents)
                    FrmEventTableEdit.Show(dataDir, tableName, false, errLog);
            }
        }

        private void btnEditFile_Click(object sender, EventArgs e)
        {
            // редактирование таблицы срезов или событий
            if (lbFiles.SelectedItem != null)
            {
                string dataDir = txtDataDir.Text;
                string tableName = lbFiles.SelectedItem.ToString();

                if (lastNode == nodeCurSrez || lastNode == nodeMinSrez || lastNode == nodeHrSrez)
                    FrmSrezTableEdit.Show(dataDir, tableName, true, errLog);
                else if (lastNode == nodeEvents)
                    FrmEventTableEdit.Show(dataDir, tableName, true, errLog);
            }
        }

        private void btnAddMod_Click(object sender, EventArgs e)
        {
            // выбор и добавление модуля
            dlgMod.InitialDirectory = appDirs.ModDir;
            dlgMod.FileName = "";

            if (dlgMod.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(dlgMod.FileName);
                int ind = lbModDll.Items.IndexOf(fileName);

                if (ind >= 0)
                {
                    lbModDll.SelectedIndex = ind;
                    ScadaUiUtils.ShowInfo(AppPhrases.ModuleAlreadyAdded);
                }
                else
                {
                    ind = lbModDll.SelectedIndex + 1;
                    lbModDll.Items.Insert(ind, fileName);
                    lbModDll.SelectedIndex = ind;
                }

                SetModified();
            }

            lbModDll.Focus();
        }

        private void btnMoveUpMod_Click(object sender, EventArgs e)
        {
            // перемещение выбранного модуля вверх
            int ind1 = lbModDll.SelectedIndex;

            if (ind1 > 0)
            {
                int ind2 = ind1 - 1;
                object obj2 = lbModDll.Items[ind2];
                lbModDll.Items[ind2] = lbModDll.Items[ind1];
                lbModDll.Items[ind1] = obj2;
                lbModDll.SelectedIndex = ind2;
                SetModified();
            }

            lbModDll.Focus();
        }

        private void btnMoveDownMod_Click(object sender, EventArgs e)
        {
            // перемещение выбранного модуля вниз
            int ind1 = lbModDll.SelectedIndex;

            if (0 <= ind1 && ind1 < lbModDll.Items.Count - 1)
            {
                int ind2 = ind1 + 1;
                object obj2 = lbModDll.Items[ind2];
                lbModDll.Items[ind2] = lbModDll.Items[ind1];
                lbModDll.Items[ind1] = obj2;
                lbModDll.SelectedIndex = ind2;
                SetModified();
            }

            lbModDll.Focus();
        }

        private void btnDelMod_Click(object sender, EventArgs e)
        {
            // удаление выбранного модуля
            int ind = lbModDll.SelectedIndex;

            if (ind >= 0)
            {
                lbModDll.Items.RemoveAt(ind);
                int cnt = lbModDll.Items.Count;

                if (ind < cnt)
                    lbModDll.SelectedIndex = ind;
                else if (cnt > 0)
                    lbModDll.SelectedIndex = cnt - 1;

                SetModified();
            }

            lbModDll.Focus();
        }

        private void lbModDll_SelectedIndexChanged(object sender, EventArgs e)
        {
            // установка доступности кнопок действий с модулями
            SetModulesButtonsEnabled();

            // вывод описания модуля
            if (lbModDll.SelectedIndex >= 0)
            {
                string fileName = lbModDll.Items[lbModDll.SelectedIndex].ToString();
                string errMsg;
                lastModView = GetModView(fileName, out errMsg);

                if (lastModView == null)
                {
                    txtModDescr.Text = errMsg;
                    btnModProps.Enabled = false;
                }
                else
                {
                    txtModDescr.Text = lastModView.Descr;
                    btnModProps.Enabled = lastModView.CanShowProps;
                }
            }
            else
            {
                txtModDescr.Text = "";
                btnModProps.Enabled = false;
                lastModView = null;
            }
        }

        private void lbModDll_DoubleClick(object sender, EventArgs e)
        {
            // просмотр свойств модуля по двойному щелчку на элементе списка модулей
            if (lbModDll.SelectedIndex >= 0)
                btnModProps_Click(null, null);
        }

        private void btnModProps_Click(object sender, EventArgs e)
        {
            // отоюражение свойств выбранного модуля
            if (lastModView != null && lastModView.CanShowProps)
                lastModView.ShowProps();
        }

        private void btnCommSettings_Click(object sender, EventArgs e)
        {
            // отображение формы настроек соединения
            if (FrmCommSettings.Show(commSettings) == DialogResult.OK)
            {
                serverComm = null; // для последующего повторного создания объекта
                string errMsg;

                // сохранение настроек соединения
                if (!commSettings.SaveToFile(appDirs.ConfigDir + CommSettings.DefFileName, out errMsg))
                {
                    errLog.WriteAction(errMsg);
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        private void txtGenPwd_TextChanged(object sender, EventArgs e)
        {
            // установка видимости элементов генератора в зависимости от правильности пароля
            string pwd = ((TextBox)sender).Text;
            txtGenPwd1.TextChanged -= txtGenPwd_TextChanged;
            txtGenPwd2.TextChanged -= txtGenPwd_TextChanged;
            txtGenPwd3.TextChanged -= txtGenPwd_TextChanged;

            if (sender != txtGenPwd1)
                txtGenPwd1.Text = pwd;
            if (sender != txtGenPwd2)
                txtGenPwd2.Text = pwd;
            if (sender != txtGenPwd3)
                txtGenPwd3.Text = pwd;

            txtGenPwd1.TextChanged += txtGenPwd_TextChanged;
            txtGenPwd2.TextChanged += txtGenPwd_TextChanged;
            txtGenPwd3.TextChanged += txtGenPwd_TextChanged;
            gbGenSrez.Visible = gbGenEv.Visible = gbCheckEv.Visible = gbGenCmd.Visible = pwd == commSettings.ServerPwd;
        }

        private void rbArcSrez_CheckedChanged(object sender, EventArgs e)
        {
            // установка доступности элементов выбора даты и времени генерируемого архивного среза
            dtpSrezDate.Enabled = dtpSrezTime.Enabled = rbArcSrez.Checked;
        }

        private void btnSrezVal_Click(object sender, EventArgs e)
        {
            // установка значения канала в генерируемом срезе
            txtSrezCnlVal.Text = sender == btnSrezValMinusOne ? "-1" : "1";
            txtSrezCnlVal.Focus();
            txtSrezCnlVal.DeselectAll();
        }

        private void btnSrezCnlStat_Click(object sender, EventArgs e)
        {
            // установка статуса канала в генерируемом срезе
            numSrezCnlStat.Value = sender == btnSrezCnlStatZero ? 0 : 1;
            numSrezCnlStat.Focus();
        }

        private void btnSendSrez_Click(object sender, EventArgs e)
        {
            // отправка среза SCADA-Серверу
            int cnlNum;
            if (!int.TryParse(cbSrezCnlNum.Text, out cnlNum))
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectCnlNum);
                return;
            }

            double cnlVal = ScadaUtils.StrToDouble(txtSrezCnlVal.Text);
            if (double.IsNaN(cnlVal))
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectCnlVal);
                return;
            }

            DateTime srezDT = rbCurSrez.Checked ? DateTime.MinValue : 
                dtpSrezDate.Value.Date.Add(dtpSrezTime.Value.TimeOfDay);
            SrezTableLight.Srez srez = new SrezTableLight.Srez(srezDT, 1);
            srez.CnlNums[0] = cnlNum;
            srez.CnlData[0] = new SrezTableLight.CnlData(cnlVal, decimal.ToInt32(numSrezCnlStat.Value));
            
            bool result;
            bool sendOk = rbCurSrez.Checked ? ServerComm.SendSrez(srez, out result) : 
                ServerComm.SendArchive(srez, out result);

            if (sendOk)
            {
                string cnlNumStr = cbSrezCnlNum.Text.Trim();
                cbSrezCnlNum.Items.Remove(cnlNumStr);
                cbSrezCnlNum.Items.Insert(0, cnlNumStr);
                cbSrezCnlNum.Text = cnlNumStr;
                ScadaUiUtils.ShowInfo(AppPhrases.DataSentSuccessfully);
            }
            else
            {
                ScadaUiUtils.ShowError(ServerComm.ErrMsg);
            }
        }

        private void btnSetEvDT_Click(object sender, EventArgs e)
        {
            // установка текущего времени генерируемого события
            dtpEvDate1.Value = dtpEvTime.Value = DateTime.Now;
            dtpEvTime.Focus();
        }

        private void btnSendEvent_Click(object sender, EventArgs e)
        {
            // отправка события SCADA-Серверу
            double oldCnlVal = ScadaUtils.StrToDouble(txtEvOldCnlVal.Text);
            if (double.IsNaN(oldCnlVal))
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectOldCnlVal);
                return;
            }

            double newCnlVal = ScadaUtils.StrToDouble(txtEvNewCnlVal.Text);
            if (double.IsNaN(newCnlVal))
            {
                ScadaUiUtils.ShowError(AppPhrases.IncorrectNewCnlVal);
                return;
            }

            EventTableLight.Event ev = new EventTableLight.Event();
            ev.DateTime = dtpEvDate1.Value.Date.Add(dtpEvTime.Value.TimeOfDay);
            ev.ObjNum = decimal.ToInt32(numEvObjNum.Value);
            ev.KPNum = decimal.ToInt32(numEvKPNum.Value);
            ev.ParamID = decimal.ToInt32(numEvParamID.Value);
            ev.CnlNum = decimal.ToInt32(numEvCnlNum.Value);
            ev.OldCnlVal = oldCnlVal;
            ev.OldCnlStat = decimal.ToInt32(numEvOldCnlStat.Value);
            ev.NewCnlVal = newCnlVal;
            ev.NewCnlStat = decimal.ToInt32(numEvNewCnlStat.Value);
            ev.UserID = decimal.ToInt32(numEvUserID1.Value);
            ev.Checked = ev.UserID > 0;
            ev.Descr = txtEvDescr.Text;
            ev.Data = txtEvData.Text;
            
            bool result;
            if (ServerComm.SendEvent(ev, out result))
                ScadaUiUtils.ShowInfo(AppPhrases.EventSentSuccessfully);
            else
                ScadaUiUtils.ShowError(ServerComm.ErrMsg);
        }

        private void btnCheckEvent_Click(object sender, EventArgs e)
        {
            // отправка команды квитирования события SCADA-Серверу
            bool result;
            DateTime evDate = dtpEvDate2.Value.Date;
            int evNum = decimal.ToInt32(numEvNum.Value);
            int userID = decimal.ToInt32(numEvUserID2.Value);

            if (ServerComm.CheckEvent(userID, evDate, evNum, out result))
                ScadaUiUtils.ShowInfo(AppPhrases.EventCheckSentSuccessfully);
            else
                ScadaUiUtils.ShowError(ServerComm.ErrMsg);
        }

        private void rbCmdType_CheckedChanged(object sender, EventArgs e)
        {
            // настройка элементов управления генератора команды
            TuneGenCmd();
        }

        private void btnCmdVal_Click(object sender, EventArgs e)
        {
            // установка значения генерируемой команды
            txtCmdVal.Text = sender == btnCmdValOff ? "0" : "1";
            txtCmdVal.Focus();
            txtCmdVal.DeselectAll();
        }

        private void btnSendCmd_Click(object sender, EventArgs e)
        {
            // отправка команды ТУ SCADA-Серверу
            bool sendOk;
            bool result;
            int userID = decimal.ToInt32(numCmdUserID.Value);
            int ctrlCnlNum = decimal.ToInt32(numCmdCtrlCnlNum.Value);

            if (rbCmdStand.Checked)
            {
                double cmdVal = ScadaUtils.StrToDouble(txtCmdVal.Text);
                if (double.IsNaN(cmdVal))
                {
                    ScadaUiUtils.ShowError(AppPhrases.IncorrectCmdVal);
                    return;
                }

                sendOk = ServerComm.SendStandardCommand(userID, ctrlCnlNum, cmdVal, out result);
            }
            else if (rbCmdBin.Checked)
            {
                byte[] cmdData;
                string cmdDataStr = txtCmdData.Text;

                if (rbCmdHex.Checked)
                {
                    if (!ScadaUtils.HexToBytes(cmdDataStr.Trim(), out cmdData))
                    {
                        ScadaUiUtils.ShowError(AppPhrases.IncorrectHexCmdData);
                        return;
                    }
                }
                else if (cmdDataStr.Length > 0)
                {
                    cmdData = Command.StrToCmdData(cmdDataStr);
                }
                else
                {
                    cmdData = null;
                    ScadaUiUtils.ShowError(AppPhrases.CmdDataRequired);
                    return;
                }

                sendOk = ServerComm.SendBinaryCommand(userID, ctrlCnlNum, cmdData, out result);
            }
            else // rbCmdReq.Checked
            {
                int kpNum = decimal.ToInt32(numCmdKPNum.Value);
                sendOk = ServerComm.SendRequestCommand(userID, ctrlCnlNum, kpNum, out result);
            }

            if (sendOk)
                ScadaUiUtils.ShowInfo(AppPhrases.CmdSentSuccessfully);
            else
                ScadaUiUtils.ShowError(ServerComm.ErrMsg);
        }

        private void lbLog_KeyDown(object sender, KeyEventArgs e)
        {
            // копирование выбранной строки журнала
            ListBox listBox = (ListBox)sender;
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                lock (listBox)
                {
                    if (listBox.SelectedIndex >= 0)
                    {
                        string text = listBox.Items[listBox.SelectedIndex] as string ?? "";
                        if (text != "")
                            Clipboard.SetText(text);
                    }
                }
            }
        }
    }
}