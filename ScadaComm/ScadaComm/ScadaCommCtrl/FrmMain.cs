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
 * Module   : SCADA-Communicator Control
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2018
 */

using Scada.Client;
using Scada.Comm.Channels;
using Scada.Comm.Devices;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Svc;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Scada.Comm.Ctrl
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
            public NodeTag(object obj, params TabPage[] tabPages)
            {
                Obj = obj;
                TabPages = tabPages;
            }

            /// <summary>
            /// Получить или установить объект, ассоциированный с узлом дерева
            /// </summary>
            public object Obj { get; set; }
            /// <summary>
            /// Получить или установить страницы, ассоциированные с узлом дерева
            /// </summary>
            public TabPage[] TabPages { get; set; }
        }

        /// <summary>
        /// Информация о библиотеке КП
        /// </summary>
        private class KpDllInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KpDllInfo()
            {
                FileName = "";
                KpType = null;
                KpView = null;
            }

            /// <summary>
            /// Получить или установить имя файла без директории
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// Получить или установить тип пользовательского интерфейса КП
            /// </summary>
            public Type KpType { get; set; }
            /// <summary>
            /// Получить или установить объект для работы с пользовательским интерфейсом КП
            /// </summary>
            public KPView KpView { get; set; }
        }

        /// <summary>
        /// Пользовательский параметр линии связи
        /// </summary>
        private class CustomParam
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CustomParam(string name, string value)
            {
                Name = name;
                Value = value;
            }

            /// <summary>
            /// Получить наименование
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// Получить или установить значение
            /// </summary>
            public string Value { get; set; }
        }


        /// <summary>
        /// Имя файла ошибок
        /// </summary>
        private const string ErrFileName = "ScadaCommCtrl.err";
        /// <summary>
        /// Имя службы по умолчанию
        /// </summary>
        private const string DefServiceName = "ScadaCommService";
        /// <summary>
        /// Нулевое время, отформатированное в соответствии с культурой приложения
        /// </summary>
        private static readonly string ZeroTime = new DateTime(0).ToString("T", Localization.Culture);

        private AppDirs appDirs;            // директории приложения
        private Log errLog;                 // журнал ошибок приложения
        private Mutex mutex;                // объект для проверки запуска второй копии программы
        private Icon icoStart;              // пиктограмма работающей службы
        private Icon icoStop;               // пиктограмма остановленной службы
        private string serviceName;         // имя службы
        private ServiceController svcContr; // контроллер службы
        private ServiceControllerStatus prevSvcStatus; // предыдущее состояние службы

        private TreeNode nodeCommonParams;  // узел общих параметров
        private TreeNode nodeKpDlls;        // узел библиотек КП
        private TreeNode nodeLines;         // узел линий связи
        private TreeNode nodeStats;         // узел статистики

        private Settings origSettings;      // первоначальные настройки
        private Settings modSettings;       // изменённые настройки
        private TreeNode lastNode;          // последний выбранный узел дерева, с которым связаны отображаемые страницы
        private Settings.CommLine lastLine; // последняя выбранная линия связи
        private CustomParam lastParam;      // последний выбранный пользовательский параметр линии связи
        private ListViewItem lastParamItem; // элемент списка последнего выбранного пользовательского параметра
        private Settings.KP lastKP;         // последний выбранный КП
        private ListViewItem lastKpItem;    // элемент списка последнего выбранного КП
        private Settings.KP copiedKP;       // скопированный КП
        private bool changing;              // происходит программное изменение элементов управления

        private ServerComm serverComm;      // объект для обмена данными со SCADA-Сервером
        private DataTable tblCommLine;      // таблица линий связи из базы конфигурации
        private DataTable tblKP;            // таблица КП из базы конфигурации
        private DataTable tblKPType;        // таблица типов КП из базы конфигурации
        private SortedList<string, KpDllInfo> kpDllInfoList; // информация о библиотеках КП, доступная по имени DLL
        private Dictionary<string, SortedList<string, string>> commCnlParamsBuf; // буфер параметров слоёв связи

        private ListBox lbLog1;             // список строк 1-го журнала
        private ListBox lbLog2;             // список строк 2-го журнала
        private string logFileName1;        // имя файла 1-го журнала
        private string logFileName2;        // имя файла 2-го журнала
        private DateTime logFileAge1;       // время изменения файла 1-го журнала
        private DateTime logFileAge2;       // время изменения файла 2-го журнала
        private bool fullLoad1;             // загружать 1-й журнал полностью
        private bool fullLoad2;             // загружать 2-й журнал полностью


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            Application.EnableVisualStyles();
            InitializeComponent();

            // установка формата времени опроса КП
            timeKpTime.CustomFormat = Localization.Culture.DateTimeFormat.LongTimePattern;

            // установка максимально допустимого адреса КП
            numKpAddress.Maximum = int.MaxValue;

            // установка имён столбцов списков для перевода формы
            colParamName.Name = "colParamName";
            colParamValue.Name = "colParamValue";

            colKpOrder.Name = "colKpOrder";
            colKpActive.Name = "colKpActive";
            colKpBind.Name = "colKpBind";
            colKpNumber.Name = "colKpNumber";
            colKpName.Name = "colKpName";
            colKpDll.Name = "colKpDll";
            colKpAddress.Name = "colKpAddress";
            colCallNum.Name = "colCallNum";
            colKpTimeout.Name = "colKpTimeout";
            colKpDelay.Name = "colKpDelay";
            colKpTime.Name = "colKpTime";
            colKpPeriod.Name = "colKpPeriod";
            colKpCmdLine.Name = "colKpCmdLine";

            // инициализация полей
            appDirs = new AppDirs();
            errLog = new Log(Log.Formats.Simple);
            errLog.Encoding = Encoding.UTF8;
            mutex = null;
            icoStart = Icon.FromHandle((ilMain.Images["star_on.ico"] as Bitmap).GetHicon());
            icoStop = Icon.FromHandle((ilMain.Images["star_off.ico"] as Bitmap).GetHicon());
            serviceName = DefServiceName;
            svcContr = null;
            prevSvcStatus = ServiceControllerStatus.Stopped;

            nodeCommonParams = treeView.Nodes["nodeCommonParams"];
            nodeKpDlls = treeView.Nodes["nodeKpDlls"];
            nodeLines = treeView.Nodes["nodeLines"];
            nodeStats = treeView.Nodes["nodeStats"];

            origSettings = new Settings();
            modSettings = null;
            lastNode = null;
            lastLine = null;
            lastParam = null;
            lastParamItem = null;
            lastKP = null;
            lastKpItem = null;
            copiedKP = null;
            changing = false;

            serverComm = null;
            tblCommLine = new DataTable();
            tblKP = new DataTable();
            tblKPType = new DataTable();
            kpDllInfoList = new SortedList<string, KpDllInfo>();
            commCnlParamsBuf = new Dictionary<string, SortedList<string, string>>();

            lbLog1 = null;
            lbLog2 = null;
            logFileName1 = "";
            logFileName2 = "";
            logFileAge1 = DateTime.MinValue;
            logFileAge2 = DateTime.MinValue;
            fullLoad1 = false;
            fullLoad2 = false;

            Application.ThreadException += Application_ThreadException;
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

            // развёртывание узла линий связи, т.к. состояние узлов не сохраняется при скрытии формы
            nodeLines.Expand();
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

            if (Localization.LoadDictionaries(appDirs.LangDir, "ScadaComm", out errMsg))
            {
                Translator.TranslateForm(this, "Scada.Comm.Ctrl.FrmMain", toolTip, cmsNotify, cmsLine, cmsKP);
                AppPhrases.Init();
                CommPhrases.InitFromDictionaries();
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
                mutex = new Mutex(true, "ScadaCommCtrlMutex - " + serviceName, out createdNew);

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
                miStartLine.Enabled = miStopLine.Enabled = miRestartLine.Enabled = true;
            }
            else if (status == ServiceControllerStatus.Stopped)
            {
                btnServiceStart.Enabled = miNotifyStart.Enabled = true;
                btnServiceStop.Enabled = miNotifyStop.Enabled = false;
                btnServiceRestart.Enabled = miNotifyRestart.Enabled = false;
                miStartLine.Enabled = miStopLine.Enabled = miRestartLine.Enabled = false;
            }
            else
            {
                btnServiceStart.Enabled = miNotifyStart.Enabled = false;
                btnServiceStop.Enabled = miNotifyStop.Enabled = false;
                btnServiceRestart.Enabled = miNotifyRestart.Enabled = false;
                miStartLine.Enabled = miStopLine.Enabled = miRestartLine.Enabled = false;
            }
        }

        /// <summary>
        /// Перевести текст узлов дерева
        /// </summary>
        private void TranslateTree()
        {
            nodeCommonParams.Text = AppPhrases.CommonParamsNode;
            nodeKpDlls.Text = AppPhrases.KpDllsNode;
            nodeLines.Text = AppPhrases.LinesNode;
            nodeStats.Text = AppPhrases.StatsNode;
        }

        /// <summary>
        /// Сформировать дерево согласно настройкам
        /// </summary>
        private void MakeTree()
        {
            nodeCommonParams.Tag = new NodeTag(null, pageCommonParams);
            nodeKpDlls.Tag = new NodeTag(null, pageKpDlls);
            nodeStats.Tag = new NodeTag(null, pageStats);

            FillLinesNode();
            treeView.SelectedNode = nodeCommonParams;
        }

        /// <summary>
        /// Заполнить узел линий связи согласно настройкам
        /// </summary>
        /// <remarks>Первоначальные и изменённые настройки должны совпадать</remarks>
        private void FillLinesNode()
        {
            treeView.BeginUpdate();
            nodeLines.Nodes.Clear();
            int lineCnt = modSettings.CommLines.Count;

            for (int i = 0; i < lineCnt; i++)
            {
                Settings.CommLine commLine = modSettings.CommLines[i];
                TreeNode nodeLine = NewLineNode(commLine);
                nodeLines.Nodes.Add(nodeLine);
                FillLineNode(nodeLine, commLine);
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Обновить узел линий связи согласно настройкам
        /// </summary>
        /// <remarks>Первоначальные и изменённые настройки должны совпадать</remarks>
        private void UpdateLinesNode(bool updateCaptions, bool updateChildNodes)
        {
            treeView.BeginUpdate();
            int comLineCnt = origSettings.CommLines.Count;

            for (int i = 0; i < comLineCnt; i++)
            {
                TreeNode nodeLine = nodeLines.Nodes[i];

                if (updateCaptions)
                    nodeLine.Text = modSettings.CommLines[i].Caption;

                if (updateChildNodes)
                {
                    Settings.CommLine commLine = modSettings.CommLines[i];
                    if (!commLine.Active)
                        nodeLine.Collapse();
                    nodeLine.Nodes.Clear();
                    FillLineNode(nodeLine, commLine);
                }
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Создать узел линии связи
        /// </summary>
        private TreeNode NewLineNode(Settings.CommLine commLine)
        {
            TreeNode node = new TreeNode(commLine.Caption);
            node.ImageKey = node.SelectedImageKey = "commline.png";
            node.ContextMenuStrip = cmsLine;
            node.Tag = new NodeTag(commLine, pageLineParams, pageCustomParams, pageReqSequence);
            return node;
        }

        /// <summary>
        /// Заполнить узел линии связи
        /// </summary>
        private void FillLineNode(TreeNode node, Settings.CommLine commLine)
        {
            if (commLine.Active)
            {
                TreeNode nodeLineStats = new TreeNode(AppPhrases.LineStatsNode);
                nodeLineStats.ImageKey = nodeLineStats.SelectedImageKey = "stats.png";
                nodeLineStats.Tag = new NodeTag(commLine, pageLineState, pageLineLog);
                node.Nodes.Add(nodeLineStats);

                foreach (Settings.KP kp in commLine.ReqSequence)
                {
                    if (kp.Active)
                    {
                        TreeNode nodeKP = new TreeNode(kp.Caption);
                        nodeKP.ImageKey = nodeKP.SelectedImageKey = "kp.png";
                        nodeKP.ContextMenuStrip = cmsKP;
                        nodeKP.Tag = new NodeTag(kp, pageKpData, pageKpCmd);
                        node.Nodes.Add(nodeKP);
                    }
                }
            }
        }

        /// <summary>
        /// Создать элемент настроек КП
        /// </summary>
        private ListViewItem NewKPItem(Settings.KP kp, int index)
        {
            ListViewItem item = new ListViewItem(new string[] { (index + 1).ToString(), 
                AppPhrases.Yes, AppPhrases.Yes,  kp.Number.ToString(), kp.Name, kp.Dll, 
                kp.Address.ToString(), kp.CallNum, "0", "0", ZeroTime, "00:00:00", ""});
            item.Tag = kp;
            return item;
        }

        /// <summary>
        /// Получить линию связи, ассоциированную с узлом дерева
        /// </summary>
        private Settings.CommLine GetCommLine(TreeNode node)
        {
            NodeTag selNodeTag = node == null ? null : node.Tag as NodeTag;
            return selNodeTag == null ? null : selNodeTag.Obj as Settings.CommLine;
        }

        /// <summary>
        /// Заполнить список типов каналов связи
        /// </summary>
        private void FillCommCnlTypes()
        {
            cbCommCnlType.Items.AddRange(new CommChannelView[]
            {
                new CommSerialView(),
                new CommTcpClientView(),
                new CommTcpServerView(),
                new CommUdpView()
            });

            cbCommCnlType.SelectedIndex = 0;
        }

        /// <summary>
        /// Настроить элементы управления отправки команды ТУ
        /// </summary>
        private void TuneKpCmd()
        {
            const int margin = 6;

            if (rbCmdStand.Checked)
            {
                numCmdNum.Enabled = true;
                pnlCmdVal.Visible = true;
                pnlCmdData.Visible = false;
                btnSendCmd.Top = pnlCmdVal.Bottom + margin;
            }
            else if (rbCmdBin.Checked)
            {
                numCmdNum.Enabled = true;
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = true;
                btnSendCmd.Top = pnlCmdData.Bottom + margin;
            }
            else // rbCmdReq.Checked
            {
                numCmdNum.Enabled = false;
                pnlCmdVal.Visible = false;
                pnlCmdData.Visible = false;
                btnSendCmd.Top = numCmdNum.Bottom + margin;
            }

            gbKpCmd.Height = btnSendCmd.Bottom + gbKpCmd.Padding.Bottom + 2;
        }

        /// <summary>
        /// Отобразить свойства КП
        /// </summary>
        private bool ShowKPProps(KPView kpView)
        {
            if (kpView.CanShowProps)
            {
                // вызов формы отображения свойств КП
                kpView.KPProps = new KPView.KPProperties(lastLine.CustomParams, lastKP.CmdLine);
                kpView.AppDirs = appDirs;
                kpView.ShowProps();

                if (kpView.KPProps.Modified)
                {
                    // обновление пользовательских параметров линии связи на форме
                    CustomParamsToPage();

                    // обновление командной строки на форме, если необходимо
                    if (tabControl.SelectedTab == pageReqSequence)
                        txtCmdLine.Text = kpView.KPProps.CmdLine;
                    else
                        lastKP.CmdLine = kpView.KPProps.CmdLine;

                    SetModified();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Отобразить свойства выбранного КП
        /// </summary>
        private void ShowLastKPProps(bool showUnsupMsg)
        {
            if (lastLine != null && lastKP != null)
            {
                KpDllInfo kpDllInfo;
                if (kpDllInfoList.TryGetValue(lastKP.Dll, out kpDllInfo))
                {
                    try
                    {
                        KPView kpView = KPFactory.GetKPView(kpDllInfo.KpType, lastKP.Number);
                        if (!ShowKPProps(kpView) && showUnsupMsg)
                            ScadaUiUtils.ShowError(string.Format(AppPhrases.ShowKpPropsUnsupported, kpDllInfo.FileName));
                    }
                    catch (Exception ex)
                    {
                        string errMsg = AppPhrases.ShowKpPropsError + ":\r\n" + ex.Message;
                        errLog.WriteAction(errMsg);
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(AppPhrases.UnknownDLL);
                }
            }
        }

        /// <summary>
        /// Отобразить свойства типа КП
        /// </summary>
        private void ShowKPTypeProps(KPView kpView)
        {
            try
            {
                if (kpView != null && kpView.CanShowProps)
                    kpView.ShowProps();
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.ShowKpPropsError + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }


        /// <summary>
        /// Считать настройки изменёнными
        /// </summary>
        private void SetModified()
        {
            // разрешение кнопок применения и отмены настроек
            btnSettingsApply.Enabled = true;
            btnSettingsCancel.Enabled = true;
        }

        /// <summary>
        /// Отобразить изменённые настройки общих параметров на странице
        /// </summary>
        private void CommonParamsToPage()
        {
            changing = true;

            chkServerUse.Checked = modSettings.Params.ServerUse;
            txtServerHost.Text = modSettings.Params.ServerHost;
            numServerPort.SetValue(modSettings.Params.ServerPort);
            numServerTimeout.SetValue(modSettings.Params.ServerTimeout);
            txtServerUser.Text = modSettings.Params.ServerUser;
            txtServerPwd.Text = modSettings.Params.ServerPwd;
            numWaitForStop.SetValue(modSettings.Params.WaitForStop);
            chkSendModData.Checked = modSettings.Params.SendModData;
            numSendAllDataPer.SetValue(modSettings.Params.SendAllDataPer);

            changing = false;
        }

        /// <summary>
        /// Отобразить изменённые параметры линии связи на странице
        /// </summary>
        private void LineParamsToPage()
        {
            if (lastLine != null)
            {
                changing = true;

                chkLineActive.Checked = lastLine.Active;
                chkLineBind.Checked = lastLine.Bind;
                numLineNumber.SetValue(lastLine.Number);
                txtLineName.Text = lastLine.Name;

                int commCnlTypeInd = 0; // 0 - не задан
                CommChannelView commCnlView = null;

                for (int i = 0; i < cbCommCnlType.Items.Count && commCnlTypeInd <= 0; i++)
                {
                    CommChannelView curCommCnlView = cbCommCnlType.Items[i] as CommChannelView;
                    if (curCommCnlView != null && curCommCnlView.TypeName == lastLine.CommCnlType)
                    {
                        commCnlTypeInd = i;
                        commCnlView = curCommCnlView;
                    }
                }

                cbCommCnlType.SelectedIndex = commCnlTypeInd;
                CommChannelToPage(commCnlView);
                commCnlParamsBuf.Clear();

                numReqTriesCnt.SetValue(lastLine.ReqTriesCnt);
                numCycleDelay.SetValue(lastLine.CycleDelay);
                chkCmdEnabled.Checked = lastLine.CmdEnabled;
                chkReqAfterCmd.Checked = lastLine.ReqAfterCmd;
                chkDetailedLog.Checked = lastLine.DetailedLog;

                changing = false;
            }
        }

        /// <summary>
        /// Отобразить свойства канала связи на странице линии связи
        /// </summary>
        private void CommChannelToPage(CommChannelView commCnlView)
        {
            if (lastLine == null || commCnlView == null)
            {
                lastLine.CommCnlType = "";
                btnCommCnlProps.Enabled = false;
                txtCommCnlParams.Text = "";
            }
            else
            {
                lastLine.CommCnlType = commCnlView.TypeName;
                btnCommCnlProps.Enabled = commCnlView.CanShowProps;
                try { txtCommCnlParams.Text = commCnlView.GetPropsInfo(lastLine.CommCnlParams); }
                catch (Exception ex) { txtCommCnlParams.Text = ex.Message; }
            }
        }

        /// <summary>
        /// Отобразить изменённые пользовательские параметры линии связи на странице
        /// </summary>
        private void CustomParamsToPage()
        {
            if (lastLine != null)
            {
                changing = true;

                btnDelParam.Enabled = false;
                lvCustomParams.Items.Clear();

                foreach (KeyValuePair<string, string> customParam in lastLine.CustomParams)
                {
                    ListViewItem item = new ListViewItem(new string[] { customParam.Key, customParam.Value });
                    item.Tag = new CustomParam(customParam.Key, customParam.Value);
                    lvCustomParams.Items.Add(item);
                }

                txtParamName.Text = "";
                txtParamValue.Text = "";
                gbSelectedParam.Enabled = false;

                changing = false;
            }
        }

        /// <summary>
        /// Отобразить изменённые параметры опроса КП линии связи на странице
        /// </summary>
        private void ReqSequenceToPage()
        {
            if (lastLine != null)
            {
                changing = true;

                btnMoveUpKP.Enabled = false;
                btnMoveDownKP.Enabled = false;
                btnDelKP.Enabled = false;
                btnImportKP.Enabled = origSettings.Params.ServerUse;
                btnCopyKP.Enabled = false;
                btnCutKP.Enabled = false;
                btnPasteKP.Enabled = copiedKP != null;

                lvReqSequence.Items.Clear();
                for (int i = 0; i < lastLine.ReqSequence.Count; i++)
                {
                    Settings.KP kp = lastLine.ReqSequence[i];

                    ListViewItem item = new ListViewItem(new string[] { (i + 1).ToString(), 
                        kp.Active ? AppPhrases.Yes : AppPhrases.No, kp.Bind ? AppPhrases.Yes : AppPhrases.No, 
                        kp.Number.ToString(), kp.Name, kp.Dll, kp.Address.ToString(), kp.CallNum, 
                        kp.Timeout.ToString(), kp.Delay.ToString(), kp.Time.ToString("T", Localization.Culture), 
                        kp.Period.ToString(), kp.CmdLine});
                    item.Tag = kp;
                    lvReqSequence.Items.Add(item);
                }

                chkKpActive.Checked = false;
                chkKpBind.Checked = false;
                numKpNumber.Value = 0;
                txtKpName.Text = "";
                cbKpDll.Text = "";
                numKpAddress.Value = 0;
                txtCallNum.Text = "";
                numKpTimeout.Value = 0;
                numKpDelay.Value = 0;
                timeKpTime.Value = timeKpTime.MinDate;
                timeKpPeriod.Value = timeKpPeriod.MinDate;
                txtCmdLine.Text = "";
                gbSelectedKP.Enabled = false;

                changing = false;
            }
        }


        /// <summary>
        /// Принять таблицы базы конфигурации от SCADA-Сервера
        /// </summary>
        private bool ReceiveBaseTables(out string errMsg)
        {
            CommSettings commSettings = new CommSettings(origSettings.Params.ServerHost,
                origSettings.Params.ServerPort, origSettings.Params.ServerUser,
                origSettings.Params.ServerPwd, origSettings.Params.ServerTimeout);
            if (serverComm == null || !serverComm.CommSettings.Equals(commSettings))
                serverComm = new ServerComm(commSettings, (Log)null);

            if (serverComm.ReceiveBaseTable("commline.dat", tblCommLine) &&
                serverComm.ReceiveBaseTable("kp.dat", tblKP) &&
                serverComm.ReceiveBaseTable("kptype.dat", tblKPType))
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = serverComm.ErrMsg;
                if (errMsg == "")
                    errMsg = AppPhrases.ReceiveBaseTableError;

                if (errMsg.Length > 0 && !errMsg.EndsWith("."))
                    errMsg += ".";

                return false;
            }
        }

        /// <summary>
        /// Обновить настройки по базе конфигурации
        /// </summary>
        private bool UpdateSettings(out string msg)
        {
            try
            {
                tblCommLine.DefaultView.Sort = "CommLineNum";
                tblKP.DefaultView.Sort = "KPNum";
                tblKPType.DefaultView.Sort = "KPTypeID";

                foreach (Settings.CommLine commLine in modSettings.CommLines)
                {
                    if (commLine.Bind)
                    {
                        // обновление наименования линии связи
                        int rowInd = tblCommLine.DefaultView.Find(commLine.Number);
                        if (rowInd >= 0)
                            commLine.Name = Convert.ToString(tblCommLine.DefaultView[rowInd]["Name"]);

                        // обновление КП
                        foreach (Settings.KP kp in commLine.ReqSequence)
                        {
                            if (kp.Bind)
                            {
                                rowInd = tblKP.DefaultView.Find(kp.Number);
                                if (rowInd >= 0)
                                    DefineKPProps(kp, tblKP.DefaultView[rowInd], false);
                            }
                        }
                    }
                }

                msg = AppPhrases.UpdateSettingsCompleted;
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.UpdateSettingsError + ":\r\n" + ex.Message;
                return false;
            }
            finally
            {
                UpdateLinesNode(true, false);
                SetModified();
            }
        }

        /// <summary>
        /// Определить свойства КП по базе конфигурации, установить параметры опроса КП по умолчанию
        /// </summary>
        private void DefineKPProps(Settings.KP kp, DataRowView kpRowView, bool setReqParams)
        {
            // определение свойств КП по базе конфигурации
            kp.Name = Convert.ToString(kpRowView["Name"]);
            kp.Address = (int)kpRowView["Address"];
            kp.CallNum = Convert.ToString(kpRowView["CallNum"]);

            int rowInd = tblKPType.DefaultView.Find(kpRowView["KPTypeID"]);
            kp.Dll = rowInd < 0 ? "" : Convert.ToString(tblKPType.DefaultView[rowInd]["DllFileName"]);

            // установка параметров опроса КП по умолчанию
            if (setReqParams)
            {
                KpDllInfo kpDllInfo;
                if (kpDllInfoList.TryGetValue(kp.Dll, out kpDllInfo))
                {
                    KPReqParams reqParams = kpDllInfo.KpView.DefaultReqParams;
                    kp.Timeout = reqParams.Timeout;
                    kp.Delay = reqParams.Delay;
                    kp.Time = reqParams.Time;
                    kp.Period = reqParams.Period;
                    kp.CmdLine = reqParams.CmdLine;
                }
            }
        }

        /// <summary>
        /// Получить информацию о библиотеках КП
        /// </summary>
        private void GetKpDllInfo()
        {
            kpDllInfoList.Clear();
            lbKpDll.Items.Clear();
            cbKpDll.Items.Clear();

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(appDirs.KPDir);
                FileInfo[] fileInfoArr = dirInfo.GetFiles("kp*.dll", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (!fileInfo.Name.Equals("kp.dll", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            KPView kpView = KPFactory.GetKPView(fileInfo.FullName);
                            kpView.AppDirs = appDirs;
                            KpDllInfo kpDllInfo = new KpDllInfo()
                            {
                                FileName = fileInfo.Name,
                                KpType = kpView.GetType(),
                                KpView = kpView
                            };
                            kpDllInfoList.Add(kpDllInfo.FileName, kpDllInfo);
                        }
                        catch (Exception ex)
                        {
                            errLog.WriteAction(ex.Message);
                        }
                    }
                }

                // заполнение списков cbKpDll и lbKpDll
                if (kpDllInfoList.Count > 0)
                {
                    foreach (KpDllInfo kpDllInfo in kpDllInfoList.Values)
                    {
                        lbKpDll.Items.Add(kpDllInfo.FileName);
                        cbKpDll.Items.Add(kpDllInfo.FileName);
                    }
                    lbKpDll.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                errLog.WriteAction(AppPhrases.GetKpTypeInfoError + ":\r\n" + ex.Message);
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

            // установка имени файла журнала ошибок
            errLog.FileName = appDirs.LogDir + ErrFileName;

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

            // определение состояния службы
            try
            {
                svcContr = new ServiceController(serviceName);
                prevSvcStatus = svcContr.Status;
                SetServiceStateText(prevSvcStatus);
                SetServiceButtonsEnabled(prevSvcStatus);
                notifyIcon.Icon = prevSvcStatus == ServiceControllerStatus.Running ? icoStart : icoStop;
            }
            catch
            {
                sbError.AppendLine(AppPhrases.ServiceNotInstalled);
                SetServiceStateText(null);
                SetServiceButtonsEnabled(null);
                notifyIcon.Icon = icoStop;
            }

            // загрузка конфигурации
            if (!origSettings.Load(appDirs.ConfigDir + Settings.DefFileName, out errMsg))
                sbError.AppendLine(errMsg);
            modSettings = origSettings.Clone();

            // подготовка интерфейса
            btnSettingsUpdate.Enabled = origSettings.Params.ServerUse;
            tabControl.TabPages.Clear();
            pnlCmdData.Top = pnlCmdVal.Top;
            TuneKpCmd();
            MakeTree();
            FillCommCnlTypes();
            notifyIcon.Text = serviceName == DefServiceName ? AppPhrases.MainFormTitle : serviceName;

            // получение информации о типах КП
            GetKpDllInfo();

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
                DialogResult dlgRes =  MessageBox.Show(AppPhrases.SaveSettingsConfirm, 
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgRes == DialogResult.Yes)
                    btnSettingsApply_Click(null, null);
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
                TimeSpan timeSpan = TimeSpan.FromSeconds(30); // ожидание остановки службы, 30 сек.
                svcContr.WaitForStatus(ServiceControllerStatus.Stopped, timeSpan);
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
            // фиксация изменений для активного элемента управления типа NumericUpDown
            if (ActiveControl is NumericUpDown)
            {
                Control control = ActiveControl;
                ActiveControl = null;
                ActiveControl = control;
            }

            string errMsg;
            if (modSettings.Save(appDirs.ConfigDir + Settings.DefFileName, out errMsg))
            {
                // запрет кнопок применения и отмены настроек
                btnSettingsApply.Enabled = false;
                btnSettingsCancel.Enabled = false;

                // принятие изменённых настроек в качестве первоначальных
                origSettings = modSettings.Clone();

                // разрешение или запрет кнопки обновления свойств КП
                btnSettingsUpdate.Enabled = origSettings.Params.ServerUse;

                // выбор узла линии связи, если отображаются данные его дочернего узла
                Settings.CommLine commLine = lastNode == null ? null : GetCommLine(lastNode.Parent);
                if (commLine != null)
                    treeView.SelectedNode = lastNode.Parent;

                // обновление узла линий связи
                UpdateLinesNode(false, true);

                ScadaUiUtils.ShowInfo(AppPhrases.RestartNeeded);
            }
            else
            {
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void btnSettingsCancel_Click(object sender, EventArgs e)
        {
            // запрет кнопок применения и отмены настроек
            btnSettingsApply.Enabled = false;
            btnSettingsCancel.Enabled = false;

            // возврат прежних настроек
            modSettings = origSettings.Clone();
            lastNode = null;

            // отображение страницы общих параметров
            if (treeView.SelectedNode == nodeCommonParams)
                CommonParamsToPage();
            else
                treeView.SelectedNode = nodeCommonParams;

            // восстановление узла линий связи
            FillLinesNode();
        }

        private void btnSettingsUpdate_Click(object sender, EventArgs e)
        {
            // обновление настроек по базе конфигурации
            string msg;
            if (ReceiveBaseTables(out msg))
            {
                bool updateOK = UpdateSettings(out msg);

                // отображение страницы общих параметров
                if (treeView.SelectedNode == nodeCommonParams)
                    CommonParamsToPage();
                else
                    treeView.SelectedNode = nodeCommonParams;

                if (updateOK)
                {
                    ScadaUiUtils.ShowInfo(msg);
                }
                else
                {
                    errLog.WriteAction(msg);
                    ScadaUiUtils.ShowError(msg);
                }
            }
            else
            {
                errLog.WriteAction(msg);
                ScadaUiUtils.ShowError(msg);
            }
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
                        notifyIcon.Icon = icoStart;
                    else if (newSvcStatus == ServiceControllerStatus.Stopped)
                        notifyIcon.Icon = icoStop;

                    prevSvcStatus = newSvcStatus;
                }
                catch
                {
                    SetServiceStateText(null);
                    SetServiceButtonsEnabled(null);
                }
            }

            // обновление журналов
            if (WindowState == FormWindowState.Normal)
            {
                if (lbLog1 != null && !(lbLog1 == lbLineLog && chkLineLogPause.Checked))
                    lbLog1.ReloadItems(logFileName1, fullLoad1, ref logFileAge1);
                if (lbLog2 != null)
                    lbLog2.ReloadItems(logFileName2, fullLoad2, ref logFileAge2);
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
            NodeTag nodeTag = node == null ? null : node.Tag as NodeTag;

            if (nodeTag != null && node != lastNode)
            {
                Text = AppPhrases.MainFormTitle + " - " + node.Text;
                lastNode = node;
                lastLine = nodeTag.Obj as Settings.CommLine;
                lastKP = nodeTag.Obj as Settings.KP;

                if (lastKP != null)
                    lastLine = ((NodeTag)node.Parent.Tag).Obj as Settings.CommLine;

                // отображение ассоциированных с узлом дерева страниц
                tabControl.TabPages.Clear();
                foreach (TabPage tabPage in nodeTag.TabPages)
                {
                    tabControl.TabPages.Add(tabPage);

                    if (tabPage == pageCommonParams)
                        CommonParamsToPage();
                    else if (tabPage == pageLineParams)
                        LineParamsToPage();
                    else if (tabPage == pageCustomParams)
                        CustomParamsToPage();
                    else if (tabPage == pageReqSequence)
                        ReqSequenceToPage();
                    else if (tabPage == pageLineLog)
                        chkLineLogPause.Checked = false;
                    else if (tabPage == pageKpCmd)
                        txtCmdPwd.Text = "";
                }

                // определение отображаемых на странице журналов
                tabControl_SelectedIndexChanged(null, null);

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

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // вызов свойств КП по двойному щечку левой кнопкой мыши
            if (e.Button == MouseButtons.Left && e.Node != null)
            {
                NodeTag nodeTag = e.Node.Tag as NodeTag;
                if (nodeTag != null && nodeTag.Obj is Settings.KP)
                    ShowLastKPProps(false);
            }
        }


        private void cmsLine_Opened(object sender, EventArgs e)
        {
            // настройка пунктов контекстного меню линии связи
            miMoveUpLine.Enabled = lastNode != null && lastNode.PrevNode != null;
            miMoveDownLine.Enabled = lastNode != null && lastNode.NextNode != null;
            miImportLines.Enabled = origSettings.Params.ServerUse;
        }

        private void miAddLine_Click(object sender, EventArgs e)
        {
            // добавление линии связи
            TreeNode selNode = treeView.SelectedNode;

            if (selNode != null)
            {
                Settings.CommLine newLine = new Settings.CommLine();
                TreeNode newNode = NewLineNode(newLine);
                int newIndex = selNode == nodeLines ? selNode.Nodes.Count : selNode.Index + 1;

                nodeLines.Nodes.Insert(newIndex, newNode);
                modSettings.CommLines.Insert(newIndex, newLine);

                treeView.SelectedNode = newNode;
                SetModified();
            }
        }

        private void miMoveUpLine_Click(object sender, EventArgs e)
        {
            // перемещение линии связи вверх
            TreeNode selNode = treeView.SelectedNode;

            if (selNode != null)
            {
                Settings.CommLine commLine = GetCommLine(selNode);
                int selIndex = selNode.Index;

                if (commLine != null && 0 < selIndex && selIndex < modSettings.CommLines.Count &&
                    modSettings.CommLines[selIndex] == commLine)
                {
                    nodeLines.Nodes.RemoveAt(selIndex);
                    modSettings.CommLines.RemoveAt(selIndex);

                    selIndex--;
                    nodeLines.Nodes.Insert(selIndex, selNode);
                    modSettings.CommLines.Insert(selIndex, commLine);

                    treeView.SelectedNode = selNode;
                    SetModified();
                }
            }
        }

        private void miMoveDownLine_Click(object sender, EventArgs e)
        {
            // перемещение линии связи вниз
            TreeNode selNode = treeView.SelectedNode;

            if (selNode != null)
            {
                Settings.CommLine commLine = GetCommLine(selNode);
                int selIndex = selNode.Index;

                if (commLine != null && 0 <= selIndex && selIndex < modSettings.CommLines.Count - 1 &&
                    modSettings.CommLines[selIndex] == commLine)
                {
                    nodeLines.Nodes.RemoveAt(selIndex);
                    modSettings.CommLines.RemoveAt(selIndex);

                    selIndex++;
                    nodeLines.Nodes.Insert(selIndex, selNode);
                    modSettings.CommLines.Insert(selIndex, commLine);

                    treeView.SelectedNode = selNode;
                    SetModified();
                }
            }
        }

        private void miDelLine_Click(object sender, EventArgs e)
        {
            // удаление линии связи
            TreeNode selNode = treeView.SelectedNode;

            if (selNode != null)
            {
                Settings.CommLine commLine = GetCommLine(selNode);
                int selIndex = selNode.Index;

                if (commLine != null && 0 <= selIndex && selIndex < modSettings.CommLines.Count &&
                    modSettings.CommLines[selIndex] == commLine)
                {
                    nodeLines.Nodes.RemoveAt(selIndex);
                    modSettings.CommLines.RemoveAt(selIndex);

                    if (selIndex < nodeLines.Nodes.Count)
                        treeView.SelectedNode = nodeLines.Nodes[selIndex];
                    else if (nodeLines.Nodes.Count > 0)
                        treeView.SelectedNode = nodeLines.Nodes[nodeLines.Nodes.Count - 1];
                    else
                        treeView.SelectedNode = nodeCommonParams;

                    SetModified();
                }
            }
        }

        private void miImportLines_Click(object sender, EventArgs e)
        {
            string errMsg;
            if (ReceiveBaseTables(out errMsg))
            {
                FrmImport frmImport;
                if (FrmImport.Import(tblCommLine, tblKP, errLog, out frmImport) == DialogResult.OK)
                {
                    // импорт линий связи и КП
                    try
                    {
                        treeView.BeginUpdate();
                        TreeNode selNode = treeView.SelectedNode;

                        if (selNode != null)
                        {
                            int lineIndex = selNode == nodeLines ?
                                selNode.Nodes.Count : selNode.Index + 1;
                            TreeNode newLineNode = selNode;

                            foreach (TreeNode nodeLine in frmImport.TreeView.Nodes)
                            {
                                if (nodeLine.Checked)
                                {
                                    // импорт линии связи
                                    DataRowView rowLine = (DataRowView)nodeLine.Tag;
                                    Settings.CommLine newLine = new Settings.CommLine();
                                    newLine.Number = (int)rowLine["CommLineNum"];
                                    newLine.Name = Convert.ToString(rowLine["Name"]);

                                    newLineNode = NewLineNode(newLine);
                                    nodeLines.Nodes.Insert(lineIndex, newLineNode);
                                    modSettings.CommLines.Insert(lineIndex, newLine);
                                    lineIndex++;

                                    // импорт КП
                                    int kpIndex = 0;
                                    foreach (TreeNode nodeKP in nodeLine.Nodes)
                                    {
                                        if (nodeKP.Checked)
                                        {
                                            DataRowView rowKP = (DataRowView)nodeKP.Tag;
                                            Settings.KP newKP = new Settings.KP();
                                            newKP.Number = (int)rowKP["KPNum"];
                                            DefineKPProps(newKP, rowKP, true);
                                            newLine.ReqSequence.Add(newKP);
                                            kpIndex++;
                                        }
                                    }
                                }
                            }

                            treeView.SelectedNode = newLineNode;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsg = AppPhrases.ImportLinesAndKpError + ":\r\n" + ex.Message;
                        errLog.WriteAction(errMsg);
                        ScadaUiUtils.ShowError(errMsg);
                    }
                    finally
                    {
                        SetModified();
                        treeView.EndUpdate();
                    }
                }
            }
            else
            {
                errLog.WriteError(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }

        private void miStartStopLine_Click(object sender, EventArgs e)
        {
            // запуск, остановка или перезапуск линии связи
            if (lastLine != null)
            {
                string cmdType = ((ToolStripMenuItem)sender).Tag.ToString();
                string[] cmdParams = new string[] { "LineNum=" + lastLine.Number };
                string msg;

                if (CommUtils.SaveCmd(appDirs.CmdDir, "ScadaCommCtrl", cmdType, cmdParams, out msg))
                {
                    ScadaUiUtils.ShowInfo(msg);
                }
                else
                {
                    errLog.WriteAction(msg);
                    ScadaUiUtils.ShowError(msg);
                }
            }
        }


        private void cmsKP_Opened(object sender, EventArgs e)
        {
            // настройка пункта контекстного меню для отображения свойств КП
            KPView kpView = null;
            bool enabled = false;

            if (lastKP != null)
            {
                int index = kpDllInfoList.IndexOfKey(lastKP.Dll);

                if (index >= 0)
                {
                    KpDllInfo kpDllInfo = kpDllInfoList.Values[index];

                    try
                    {
                        kpView = KPFactory.GetKPView(kpDllInfo.KpType, lastKP.Number);
                        enabled = kpView.CanShowProps;
                    }
                    catch { }
                }
            }

            miKpProps.Enabled = enabled;
            miKpProps.Tag = enabled ? kpView : null;
        }

        private void miKpProps_Click(object sender, EventArgs e)
        {
            // отображение свойств КП
            try
            {
                KPView kpView = miKpProps.Tag as KPView;
                if (kpView != null && lastLine != null && lastKP != null)
                    ShowKPProps(kpView);
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.ShowKpPropsError + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
        }


        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // настройка отображения журналов
            TabPage tabPage = tabControl.SelectedTab;
            if (tabPage == null && tabControl.TabPages.Count > 0)
                tabPage = tabControl.TabPages[0];

            if (tabPage != null)
            {
                lbLog1 = null;
                lbLog2 = null;
                logFileName1 = "";
                logFileName2 = "";
                logFileAge1 = DateTime.MinValue;
                logFileAge2 = DateTime.MinValue;
                fullLoad1 = false;
                fullLoad2 = false;

                if (tabPage == pageLineState && lastLine != null)
                {
                    string prefix;
                    if (lastLine.Number < 10) prefix = "line00";
                    else if (lastLine.Number < 100) prefix = "line0";
                    else prefix = "line";

                    lbLog1 = lbLineState;
                    logFileName1 = appDirs.LogDir + prefix + lastLine.Number + ".txt";
                    fullLoad1 = true;
                }
                else if (tabPage == pageLineLog && lastLine != null)
                {
                    string prefix;
                    if (lastLine.Number < 10) prefix = "line00";
                    else if (lastLine.Number < 100) prefix = "line0";
                    else prefix = "line";

                    lbLog1 = lbLineLog;
                    lbLog1.SelectedIndex = -1; // для последующей прокрутки в конец списка
                    logFileName1 = appDirs.LogDir + prefix + lastLine.Number + ".log";
                }
                else if (tabPage == pageKpData && lastKP != null)
                {
                    string prefix;
                    if (lastKP.Number < 10) prefix = "kp00";
                    else if (lastKP.Number < 100) prefix = "kp0";
                    else prefix = "kp";

                    lbLog1 = lbKpData;
                    logFileName1 = appDirs.LogDir + prefix + lastKP.Number + ".txt";
                    fullLoad1 = true;
                }
                else if (tabPage == pageStats)
                {
                    lbLog1 = lbAppState;
                    lbLog2 = lbAppLog;
                    lbLog2.SelectedIndex = -1; // для последующей прокрутки в конец списка
                    logFileName1 = appDirs.LogDir + CommUtils.AppStateFileName;
                    logFileName2 = appDirs.LogDir + CommUtils.AppLogFileName;
                    fullLoad1 = true;
                }
            }
        }
        
        private void LogListBox_KeyDown(object sender, KeyEventArgs e)
        {
            // копирование выбранной строки списка, отображающего файл журнала или состояния
            ListBox listBox = sender as ListBox;

            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control && listBox != null)
            {
                Monitor.Enter(listBox);
                try
                {
                    object item = listBox.Items[listBox.SelectedIndex]; // listBox.SelectedItem работает некорректно
                    string text = item == null ? "" : item.ToString();
                    if (!string.IsNullOrEmpty(text))
                        Clipboard.SetText(text);
                }
                finally
                {
                    Monitor.Exit(listBox);
                }
            }
        }


        #region Обработка событий на странице общих параметров
        private void chkServerUse_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerUse = chkServerUse.Checked;
                SetModified();
            }
        }

        private void txtServerHost_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerHost = txtServerHost.Text;
                SetModified();
            }
        }

        private void numServerPort_ValueChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerPort = decimal.ToInt32(numServerPort.Value);
                SetModified();
            }
        }

        private void numServerTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerTimeout = decimal.ToInt32(numServerTimeout.Value);
                SetModified();
            }
        }

        private void txtServerUser_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerUser = txtServerUser.Text;
                SetModified();
            }
        }

        private void txtServerPwd_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.ServerPwd = txtServerPwd.Text;
                SetModified();
            }
        }

        private void numWaitForStop_ValueChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.WaitForStop = decimal.ToInt32(numWaitForStop.Value);
                SetModified();
            }
        }

        private void chkSendModData_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.SendModData = chkSendModData.Checked;
                SetModified();
            }
        }

        private void numSendAllDataPer_ValueChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                modSettings.Params.SendAllDataPer = decimal.ToInt32(numSendAllDataPer.Value);
                SetModified();
            }
        }
        #endregion

        #region Обработка событий на странице библиотек КП
        private void lbKpDll_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lbKpDll.SelectedIndex;

            if (0 <= index && index < kpDllInfoList.Count)
            {
                KPView kpView = kpDllInfoList.Values[index].KpView;
                if (kpView != null)
                {
                    try { txtKpDllDescr.Lines = kpView.KPDescr.Split(new string[] { "\n" }, StringSplitOptions.None); }
                    catch (Exception ex) { txtKpDllDescr.Text = ex.Message; }
                    btnKpDllProps.Enabled = kpView.CanShowProps;
                    return;
                }
            }

            txtKpDllDescr.Text = "";
            btnKpDllProps.Enabled = false;
        }

        private void btnKpTypeProps_Click(object sender, EventArgs e)
        {
            int index = lbKpDll.SelectedIndex;
            if (0 <= index && index < kpDllInfoList.Count)
                ShowKPTypeProps(kpDllInfoList.Values[index].KpView);
        }
        #endregion

        #region Обработка событий на странице параметров линии связи
        private void chkLineActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.Active = chkLineActive.Checked;
                SetModified();
            }
        }

        private void chkLineBind_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.Bind = chkLineBind.Checked;
                SetModified();
            }
        }

        private void numLineNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.Number = decimal.ToInt32(numLineNumber.Value);
                if (lastNode != null)
                {
                    lastNode.Text = lastLine.Caption;
                    Text = AppPhrases.MainFormTitle + " - " + lastNode.Text;
                }
                SetModified();
            }
        }

        private void txtLineName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.Name = txtLineName.Text;
                if (lastNode != null)
                {
                    lastNode.Text = lastLine.Caption;
                    Text = AppPhrases.MainFormTitle + " - " + lastNode.Text;
                }
                SetModified();
            }
        }

        private void cbCommCnlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                // копирование предыдущих параметров канала связи в буфер
                if (lastLine.CommCnlType != "")
                {
                    SortedList<string, string> oldCommCnlParams = 
                        new SortedList<string, string>(lastLine.CommCnlParams);
                    commCnlParamsBuf[lastLine.CommCnlType] = oldCommCnlParams;
                }

                // загрузка параметров выбранного канала связи из буфера или установка по умолчанию
                lastLine.CommCnlParams.Clear();
                CommChannelView commChannelView = cbCommCnlType.SelectedItem as CommChannelView;
                SortedList<string, string> storedCommCnlParams;
                if (commChannelView != null)
                {
                    if (commCnlParamsBuf.TryGetValue(commChannelView.TypeName, out storedCommCnlParams))
                    {
                        foreach (KeyValuePair<string, string> commCnlParam in storedCommCnlParams)
                            lastLine.CommCnlParams.Add(commCnlParam.Key, commCnlParam.Value);
                    }
                    else
                    {
                        commChannelView.SetCommCnlParamsToDefault(lastLine.CommCnlParams);
                    }
                }

                // отображение параметров выбранного канала связи
                CommChannelToPage(commChannelView);
                SetModified();
            }
        }

        private void btnCommCnlProps_Click(object sender, EventArgs e)
        {
            // отображение свойств канала связи
            CommChannelView commCnlView = cbCommCnlType.SelectedItem as CommChannelView;
            if (lastLine != null && commCnlView != null && commCnlView.CanShowProps)
            {
                try
                {
                    bool modified;
                    commCnlView.ShowProps(lastLine.CommCnlParams, out modified);

                    if (modified)
                    {
                        try { txtCommCnlParams.Text = commCnlView.GetPropsInfo(lastLine.CommCnlParams); }
                        catch (Exception ex) { txtCommCnlParams.Text = ex.Message; }
                        SetModified();
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = AppPhrases.ShowCommCnlPropsError + ":\r\n" + ex.Message;
                    errLog.WriteAction(errMsg);
                    ScadaUiUtils.ShowError(errMsg);
                }
            }
        }

        private void numReqTriesCnt_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.ReqTriesCnt = decimal.ToInt32(numReqTriesCnt.Value);
                SetModified();
            }
        }

        private void numCycleDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.CycleDelay = decimal.ToInt32(numCycleDelay.Value);
                SetModified();
            }
        }

        private void chkCmdEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.CmdEnabled = chkCmdEnabled.Checked;
                SetModified();
            }
        }

        private void lblCmdEnabled_Click(object sender, EventArgs e)
        {
            chkCmdEnabled.Checked = !chkCmdEnabled.Checked;
        }

        private void chkReqAfterCmd_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.ReqAfterCmd = chkReqAfterCmd.Checked;
                SetModified();
            }
        }

        private void lblReqAfterCmd_Click(object sender, EventArgs e)
        {
            chkReqAfterCmd.Checked = !chkReqAfterCmd.Checked;
        }

        private void chkDetailedLog_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null)
            {
                lastLine.DetailedLog = chkDetailedLog.Checked;
                SetModified();
            }
        }

        private void lblDetailedLog_Click(object sender, EventArgs e)
        {
            chkDetailedLog.Checked = !chkDetailedLog.Checked;
        }
        #endregion

        #region Обработка событий на странице пользовательских параметров линии связи
        private void lvCustomParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            changing = true;

            if (lvCustomParams.SelectedItems.Count > 0)
            {
                lastParamItem = lvCustomParams.SelectedItems[0];
                lastParam = lastParamItem.Tag as CustomParam;

                if (lastParam == null)
                {
                    lastParamItem = null;
                }
                else
                {
                    txtParamName.Text = lastParam.Name;
                    txtParamValue.Text = lastParam.Value;
                    gbSelectedParam.Enabled = true;
                }
            }
            else
            {
                lastParam = null;
                lastParamItem = null;

                txtParamName.Text = "";
                txtParamValue.Text = "";
                gbSelectedParam.Enabled = false;
            }

            // установка доступности кнопки удаления пользовательского параметра
            btnDelParam.Enabled = lastParamItem != null;

            changing = false;
        }

        private void txtParamName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastParam != null && lastParamItem != null)
            {
                lastParamItem.SubItems[0].Text = txtParamName.Text;
                SetModified();
            }
        }

        private void txtParamName_Leave(object sender, EventArgs e)
        {
            if (lastLine != null && lastParam != null && lastParamItem != null && lastParam.Name != txtParamName.Text)
            {
                Control activeControl = ActiveControl;

                // создание нового пользовательского параметра
                string newName = txtParamName.Text;
                CustomParam newParam = new CustomParam(newName, lastParam.Value);

                // удаление пользовательского параметра со старым наименованием
                lastLine.CustomParams.Remove(lastParam.Name);
                lvCustomParams.Items.RemoveAt(lastParamItem.Index);

                // удаление элемента списка, если новое наименование дублируется
                int newInd = lastLine.CustomParams.IndexOfKey(newName);
                if (newInd >= 0)
                    lvCustomParams.Items.RemoveAt(newInd);

                // добавление нового пользовательского параметра
                lastLine.CustomParams[newName] = newParam.Value;
                newInd = lastLine.CustomParams.IndexOfKey(newName);
                ListViewItem newItem = new ListViewItem(new string[] { newName, newParam.Value }) { Tag = newParam };
                lvCustomParams.Items.Insert(newInd, newItem);

                newItem.Selected = true;
                activeControl.Focus();
            }
        }

        private void txtParamName_KeyDown(object sender, KeyEventArgs e)
        {
            // применить изменение наименования пользовательского параметра
            if (e.KeyCode == Keys.Enter)
                txtParamName_Leave(null, null);
        }

        private void txtParamValue_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastLine != null && lastParam != null && lastParamItem != null)
            {
                lastParam.Value = txtParamValue.Text;
                lastLine.CustomParams[lastParam.Name] = lastParam.Value;
                lastParamItem.SubItems[1].Text = lastParam.Value;
                SetModified();
            }
        }

        private void btnAddParam_Click(object sender, EventArgs e)
        {
            if (lastLine != null)
            {
                CustomParam newParam = new CustomParam("", "");
                ListViewItem newItem = new ListViewItem(new string[] { "", "" }) { Tag = newParam };
                lvCustomParams.Items.Add(newItem);

                newItem.Selected = true;
                txtParamName.Focus();
                SetModified();
            }
        }

        private void btnDelParam_Click(object sender, EventArgs e)
        {
            if (lastLine != null && lastParam != null && lastParamItem != null)
            {
                lastLine.CustomParams.Remove(lastParam.Name);
                int selIndex = lastParamItem.Index;
                lvCustomParams.Items.RemoveAt(selIndex);

                if (lvCustomParams.Items.Count > 0)
                {
                    if (selIndex >= lvCustomParams.Items.Count)
                        selIndex = lvCustomParams.Items.Count - 1;
                    lvCustomParams.Items[selIndex].Selected = true;
                }

                SetModified();
            }
            lvCustomParams.Focus();
        }
        #endregion

        #region Обработка событий на странице опроса КП
        private void lvReqSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            changing = true;

            if (lvReqSequence.SelectedItems.Count > 0)
            {
                lastKpItem = lvReqSequence.SelectedItems[0];
                lastKP = lastKpItem.Tag as Settings.KP;

                if (lastKP == null)
                {
                    lastKpItem = null;
                }
                else
                {
                    chkKpActive.Checked = lastKP.Active;
                    chkKpBind.Checked = lastKP.Bind;
                    numKpNumber.SetValue(lastKP.Number);
                    txtKpName.Text = lastKP.Name;
                    cbKpDll.Text = lastKP.Dll;
                    numKpAddress.SetValue(lastKP.Address);
                    txtCallNum.Text = lastKP.CallNum;
                    numKpTimeout.SetValue(lastKP.Timeout);
                    numKpDelay.SetValue(lastKP.Delay);
                    timeKpTime.Value = new DateTime(timeKpTime.MinDate.Year, timeKpTime.MinDate.Month, 
                        timeKpTime.MinDate.Day, lastKP.Time.Hour, lastKP.Time.Minute, lastKP.Time.Second);
                    timeKpPeriod.Value = new DateTime(timeKpPeriod.MinDate.Year, timeKpPeriod.MinDate.Month,
                        timeKpPeriod.MinDate.Day, lastKP.Period.Hours, lastKP.Period.Minutes, lastKP.Period.Seconds);
                    txtCmdLine.Text = lastKP.CmdLine;
                    gbSelectedKP.Enabled = true;
                }
            }
            else
            {
                lastKP = null;
                lastKpItem = null;

                chkKpActive.Checked = false;
                chkKpBind.Checked = false;
                numKpNumber.Value = 0;
                txtKpName.Text = "";
                cbKpDll.Text = "";
                numKpAddress.Value = 0;
                txtCallNum.Text = "";
                numKpTimeout.Value = 0;
                numKpDelay.Value = 0;
                timeKpTime.Value = timeKpTime.MinDate;
                timeKpPeriod.Value = timeKpPeriod.MinDate;
                txtCmdLine.Text = "";
                gbSelectedKP.Enabled = false;
            }

            // установка доступности кнопок для действий с КП
            bool itemSelected = lastKpItem != null;
            btnMoveUpKP.Enabled = itemSelected && lastKpItem.Index > 0;
            btnMoveDownKP.Enabled = itemSelected && lastKpItem.Index < lvReqSequence.Items.Count - 1;
            btnDelKP.Enabled = itemSelected;
            btnCopyKP.Enabled = itemSelected;
            btnCutKP.Enabled = itemSelected;

            changing = false;
        }

        private void chkKpActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Active = chkKpActive.Checked;
                lastKpItem.SubItems[1].Text = chkKpActive.Checked ? AppPhrases.Yes : AppPhrases.No;
                SetModified();
            }
        }

        private void chkKpBind_CheckedChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Bind = chkKpBind.Checked;
                lastKpItem.SubItems[2].Text = chkKpBind.Checked ? AppPhrases.Yes : AppPhrases.No;
                SetModified();
            }
        }

        private void numKpNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Number = decimal.ToInt32(numKpNumber.Value);
                lastKpItem.SubItems[3].Text = numKpNumber.Value.ToString();
                SetModified();
            }
        }

        private void txtKpName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Name = txtKpName.Text;
                lastKpItem.SubItems[4].Text = txtKpName.Text;
                SetModified();
            }
        }

        private void cbKpType_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Dll = cbKpDll.Text;
                lastKpItem.SubItems[5].Text = cbKpDll.Text;
                SetModified();
            }
        }

        private void numKpAddress_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Address = decimal.ToInt32(numKpAddress.Value);
                lastKpItem.SubItems[6].Text = numKpAddress.Value.ToString();
                SetModified();
            }
        }

        private void txtCallNum_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.CallNum = txtCallNum.Text;
                lastKpItem.SubItems[7].Text = txtCallNum.Text;
                SetModified();
            }
        }

        private void numKpTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Timeout = decimal.ToInt32(numKpTimeout.Value);
                lastKpItem.SubItems[8].Text = numKpTimeout.Value.ToString();
                SetModified();
            }
        }

        private void numKpDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Delay = decimal.ToInt32(numKpDelay.Value);
                lastKpItem.SubItems[9].Text = numKpDelay.Value.ToString();
                SetModified();
            }
        }

        private void timeKpTime_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Time = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    timeKpTime.Value.Hour, timeKpTime.Value.Minute, timeKpTime.Value.Second);
                lastKpItem.SubItems[10].Text = lastKP.Time.ToString("T", Localization.Culture);
                SetModified();
            }
        }

        private void timeKpPeriod_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.Period = new TimeSpan(timeKpPeriod.Value.Hour, timeKpPeriod.Value.Minute, timeKpPeriod.Value.Second);
                lastKpItem.SubItems[11].Text = lastKP.Period.ToString();
                SetModified();
            }
        }

        private void txtCmdLine_TextChanged(object sender, EventArgs e)
        {
            if (!changing && lastKP != null && lastKpItem != null)
            {
                lastKP.CmdLine = txtCmdLine.Text;
                lastKpItem.SubItems[12].Text = txtCmdLine.Text;
                SetModified();
            }
        }

        private void btnAddKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null)
            {
                int newIndex = lastLine.ReqSequence.IndexOf(lastKP);
                newIndex = newIndex < 0 ? lastLine.ReqSequence.Count : newIndex + 1;

                Settings.KP newKP = new Settings.KP();
                ListViewItem newItem = NewKPItem(newKP, newIndex);

                if (newIndex < lastLine.ReqSequence.Count)
                {
                    lastLine.ReqSequence.Insert(newIndex, newKP);
                    lvReqSequence.Items.Insert(newIndex, newItem);

                    for (int i = newIndex + 1; i < lvReqSequence.Items.Count; i++)
                        lvReqSequence.Items[i].Text = (i + 1).ToString();
                }
                else
                {
                    lastLine.ReqSequence.Add(newKP);
                    lvReqSequence.Items.Add(newItem);
                }

                newItem.Selected = true;
                SetModified();
            }
            numKpNumber.Focus();
        }

        private void btnMoveUpKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null && lastKP != null && lastKpItem != null)
            {
                int selIndex = lastKpItem.Index;
                if (0 < selIndex && selIndex < lastLine.ReqSequence.Count && selIndex < lvReqSequence.Items.Count)
                {
                    lastLine.ReqSequence.RemoveAt(selIndex);

                    lvReqSequence.SelectedIndexChanged -= lvReqSequence_SelectedIndexChanged;
                    lvReqSequence.Items.RemoveAt(selIndex);
                    lvReqSequence.SelectedIndexChanged += lvReqSequence_SelectedIndexChanged;

                    selIndex--;
                    lastLine.ReqSequence.Insert(selIndex, lastKP);
                    lvReqSequence.Items.Insert(selIndex, lastKpItem);

                    for (int i = selIndex; i < lvReqSequence.Items.Count; i++)
                        lvReqSequence.Items[i].Text = (i + 1).ToString();

                    lastKpItem.Selected = true;
                    SetModified();
                }
            }
            lvReqSequence.Focus();
        }

        private void btnMoveDownKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null && lastKP != null && lastKpItem != null)
            {
                int selIndex = lastKpItem.Index;
                if (0 <= selIndex && selIndex < lastLine.ReqSequence.Count - 1 &&
                    selIndex < lvReqSequence.Items.Count - 1)
                {
                    lastLine.ReqSequence.RemoveAt(selIndex);

                    lvReqSequence.SelectedIndexChanged -= lvReqSequence_SelectedIndexChanged;
                    lvReqSequence.Items.RemoveAt(selIndex);
                    lvReqSequence.SelectedIndexChanged += lvReqSequence_SelectedIndexChanged;

                    selIndex++;
                    lastLine.ReqSequence.Insert(selIndex, lastKP);
                    lvReqSequence.Items.Insert(selIndex, lastKpItem);

                    for (int i = 0; i <= selIndex; i++)
                        lvReqSequence.Items[i].Text = (i + 1).ToString();

                    lastKpItem.Selected = true;
                    SetModified();
                }
            }
            lvReqSequence.Focus();
        }

        private void btnDelKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null && lastKP != null && lastKpItem != null)
            {
                int selIndex = lastKpItem.Index;
                if (0 <= selIndex && selIndex < lastLine.ReqSequence.Count && selIndex < lvReqSequence.Items.Count)
                {
                    lastLine.ReqSequence.RemoveAt(selIndex);
                    lvReqSequence.Items.RemoveAt(selIndex);

                    for (int i = selIndex; i < lvReqSequence.Items.Count; i++)
                        lvReqSequence.Items[i].Text = (i + 1).ToString();

                    if (lvReqSequence.Items.Count > 0)
                    {
                        if (selIndex >= lvReqSequence.Items.Count)
                            selIndex = lvReqSequence.Items.Count - 1;
                        lvReqSequence.Items[selIndex].Selected = true;
                    }

                    SetModified();
                }
            }
            lvReqSequence.Focus();
        }

        private void btnImportKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null)
            {
                string errMsg;
                if (ReceiveBaseTables(out errMsg))
                {
                    FrmImport frmImport;
                    if (FrmImport.Import(tblKP, lastLine.Number, errLog, out frmImport) == DialogResult.OK)
                    {
                        // импорт КП
                        try
                        {
                            lvReqSequence.BeginUpdate();

                            int newIndex = lastLine.ReqSequence.IndexOf(lastKP);
                            newIndex = newIndex < 0 ? lastLine.ReqSequence.Count : newIndex + 1;
                            bool insert = newIndex < lastLine.ReqSequence.Count;
                            ListViewItem newItem = null;
                            int kpIndex = newIndex;

                            foreach (TreeNode nodeKP in frmImport.TreeView.Nodes)
                            {
                                if (nodeKP.Checked)
                                {
                                    DataRowView rowKP = (DataRowView)nodeKP.Tag;
                                    Settings.KP newKP = new Settings.KP();
                                    newKP.Number = (int)rowKP["KPNum"];
                                    DefineKPProps(newKP, rowKP, true);
                                    newItem = NewKPItem(newKP, kpIndex);

                                    if (insert)
                                    {
                                        lastLine.ReqSequence.Insert(kpIndex, newKP);
                                        lvReqSequence.Items.Insert(kpIndex, newItem);
                                    }
                                    else
                                    {
                                        lastLine.ReqSequence.Add(newKP);
                                        lvReqSequence.Items.Add(newItem);
                                    }

                                    kpIndex++;
                                }
                            }

                            // обновление порядковых номеров КП
                            if (insert && kpIndex > newIndex)
                            {
                                for (int i = kpIndex + 1; i < lvReqSequence.Items.Count; i++)
                                    lvReqSequence.Items[i].Text = (i + 1).ToString();
                            }

                            // выбор последнего добавленного элемента
                            if (newItem != null)
                                newItem.Selected = true;
                        }
                        catch (Exception ex)
                        {
                            errMsg = AppPhrases.ImportKpError + ":\r\n" + ex.Message;
                            errLog.WriteAction(errMsg);
                            ScadaUiUtils.ShowError(errMsg);
                        }
                        finally
                        {
                            SetModified();
                            lvReqSequence.EndUpdate();
                        }
                    }
                }
                else
                {
                    errLog.WriteError(errMsg);
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            lvReqSequence.Focus();
        }

        private void btnCutKP_Click(object sender, EventArgs e)
        {
            btnCopyKP_Click(null, null);
            btnDelKP_Click(null, null);
        }

        private void btnCopyKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null && lastKP != null && lastKpItem != null)
            {
                copiedKP = lastKP;
                btnPasteKP.Enabled = true;
            }
            lvReqSequence.Focus();
        }

        private void btnPasteKP_Click(object sender, EventArgs e)
        {
            if (lastLine != null && copiedKP != null)
            {
                Settings.KP newKP = copiedKP.Clone();

                int newIndex = lastLine.ReqSequence.IndexOf(lastKP);
                if (newIndex < 0)
                    newIndex = lastLine.ReqSequence.Count;
                else
                    newIndex++;

                ListViewItem newItem = new ListViewItem(new string[] { (newIndex + 1).ToString(), 
                    newKP.Active ? AppPhrases.Yes : AppPhrases.No, newKP.Bind ? AppPhrases.Yes : AppPhrases.No, 
                    newKP.Number.ToString(), newKP.Name, newKP.Dll, newKP.Address.ToString(), newKP.CallNum, 
                    newKP.Timeout.ToString(), newKP.Delay.ToString(), newKP.Time.ToString("T", Localization.Culture), 
                    newKP.Period.ToString(), newKP.CmdLine});
                newItem.Tag = newKP;

                if (newIndex < lastLine.ReqSequence.Count)
                {
                    lastLine.ReqSequence.Insert(newIndex, newKP);
                    lvReqSequence.Items.Insert(newIndex, newItem);

                    for (int i = newIndex + 1; i < lvReqSequence.Items.Count; i++)
                        lvReqSequence.Items[i].Text = (i + 1).ToString();
                }
                else
                {
                    lastLine.ReqSequence.Add(newKP);
                    lvReqSequence.Items.Add(newItem);
                }

                newItem.Selected = true;
                numKpNumber.Focus();
                SetModified();
            }
            else
                lvReqSequence.Focus();
        }

        private void btnResetReqParams_Click(object sender, EventArgs e)
        {
            // установка параметров опроса КП по умолчанию
            if (lastKP != null)
            {
                int index = kpDllInfoList.IndexOfKey(lastKP.Dll);

                if (index >= 0)
                {
                    numKpTimeout.Focus();

                    try
                    {
                        KpDllInfo kpDllInfo = kpDllInfoList.Values[index];
                        if (kpDllInfo.KpView != null)
                        {
                            KPReqParams reqParams = kpDllInfo.KpView.DefaultReqParams;
                            numKpTimeout.SetValue(reqParams.Timeout);
                            numKpDelay.SetValue(reqParams.Delay);
                            DateTime date = timeKpTime.Value.Date;
                            DateTime time = reqParams.Time;
                            timeKpTime.Value = new DateTime(date.Year, date.Month, date.Day, 
                                time.Hour, time.Minute, time.Second);
                            date = timeKpPeriod.Value.Date;
                            TimeSpan period = reqParams.Period;
                            timeKpPeriod.Value = new DateTime(date.Year, date.Month, date.Day, 
                                period.Hours, period.Minutes, period.Seconds);
                            txtCmdLine.Text = reqParams.CmdLine;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errMsg = AppPhrases.ResetReqParamsError + ":\r\n" + ex.Message;
                        errLog.WriteAction(errMsg);
                        ScadaUiUtils.ShowError(errMsg);
                    }
                }
                else
                {
                    cbKpDll.Focus();
                    ScadaUiUtils.ShowError(AppPhrases.UnknownDLL);
                }
            }
        }

        private void btnKpProps_Click(object sender, EventArgs e)
        {
            // отображение свойств выбранного КП
            txtCmdLine.Focus();
            ShowLastKPProps(true);
        }
        #endregion

        #region Обработка событий на странице команд
        private void txtCmdPwd_TextChanged(object sender, EventArgs e)
        {
            gbKpCmd.Visible = txtCmdPwd.Text == origSettings.Params.ServerPwd;

            if (gbKpCmd.Visible)
            {
                numCmdNum.Value = numCmdNum.Minimum;
                rbCmdStand.Checked = true;
                txtCmdVal.Text = "0";
                rbCmdStr.Checked = true;
                txtCmdData.Text = "";
            }
        }

        private void rbCmdType_CheckedChanged(object sender, EventArgs e)
        {
            // настройка элементов управления отправки команды
            TuneKpCmd();
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
            if (lastKP != null)
            {
                bool cmdOK = false;
                Command cmd = new Command();
                cmd.KPNum = lastKP.Number;

                if (rbCmdStand.Checked)
                {
                    txtCmdVal.Focus();
                    cmd.CmdTypeID = BaseValues.CmdTypes.Standard;
                    cmd.CmdNum = decimal.ToInt32(numCmdNum.Value);

                    double cmdVal = ScadaUtils.StrToDouble(txtCmdVal.Text);
                    if (double.IsNaN(cmdVal))
                    {
                        ScadaUiUtils.ShowError(AppPhrases.IncorrectCmdVal);
                    }
                    else
                    {
                        cmd.CmdVal = cmdVal;
                        cmdOK = true;
                    }
                }
                else if (rbCmdBin.Checked)
                {
                    cmd.CmdTypeID = BaseValues.CmdTypes.Binary;
                    cmd.CmdNum = decimal.ToInt32(numCmdNum.Value);

                    byte[] cmdData;
                    string cmdDataStr = txtCmdData.Text;

                    if (rbCmdHex.Checked)
                    {
                        if (ScadaUtils.HexToBytes(cmdDataStr.Trim(), out cmdData))
                        {
                            cmdOK = true;
                            cmd.CmdData = cmdData;
                        }
                        else
                        {
                            ScadaUiUtils.ShowError(AppPhrases.IncorrectHexCmdData);
                        }
                    }
                    else if (cmdDataStr.Length > 0)
                    {
                        cmd.CmdData = Command.StrToCmdData(cmdDataStr);
                        cmdOK = true;
                    }
                    else
                    {
                        ScadaUiUtils.ShowError(AppPhrases.CmdDataRequired);
                    }
                }
                else
                {
                    cmd.CmdTypeID = BaseValues.CmdTypes.Request;
                    cmdOK = true;
                }

                // сохранение команды в файле
                if (cmdOK)
                {
                    string msg;
                    if (CommUtils.SaveCmd(appDirs.CmdDir, "ScadaCommCtrl", cmd, out msg))
                    {
                        ScadaUiUtils.ShowInfo(msg);
                    }
                    else
                    {
                        errLog.WriteAction(msg);
                        ScadaUiUtils.ShowError(msg);
                    }
                }
            }
        }
        #endregion
    }
}