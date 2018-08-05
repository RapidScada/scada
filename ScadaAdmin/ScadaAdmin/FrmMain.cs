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
 * Module   : SCADA-Administrator
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using ScadaAdmin.Remote;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceProcess;
using System.Windows.Forms;
using WinControl;

namespace ScadaAdmin
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Действия, связанные с узлами дерева
        /// </summary>
        private enum NodeActions {None, Obj, CommLine, KP, InCnl, InCnlObj, InCnlKP, 
            CtrlCnl, CtrlCnlObj, CtrlCnlKP, Role, User, Interface, Right, CnlType, 
            CmdType, EvType, KPType, Param, Unit, CmdVal, Format, Formula}

        /// <summary>
        /// Информация, связанная с узлом дерева
        /// </summary>
        private class NodeInfo
        {
            /// <summary>
            /// Получить или установить действие, связанное с узлом дерева
            /// </summary>
            public NodeActions NodeAction { get; set; }
            /// <summary>
            /// Получить или установить параметры действия
            /// </summary>
            public object[] Params { get; set; }
            /// <summary>
            /// Получить или установить форму, связанную с узлом дерева
            /// </summary>
            public Form Form { get; set; }


            /// <summary>
            /// Конструктор
            /// </summary>
            public NodeInfo()
            {
                NodeAction = NodeActions.None;
                Params = null;
                Form = null;
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            public NodeInfo(NodeActions nodeAction)
            {
                NodeAction = nodeAction;
                Params = null;
                Form = null;
            }
        }


        /// <summary>
        /// Интервал ожидания завершения действий со службами SCADA-Сервер и SCADA-Коммуникатор
        /// </summary>
        private static readonly TimeSpan ServiceWait = TimeSpan.FromSeconds(30);

        
        // Узлы дерева проводника
        private TreeNode nodDB;          // узел базы конфигурации
        private TreeNode nodSystem;      // узел системных таблиц
        private TreeNode nodDict;        // узел таблиц справочников
        private TreeNode nodInCnl;       // узел входных каналов
        private TreeNode nodCtrlCnl;     // узел каналов управления
        private List<TreeNode> allNodes; // список всех узлов дерева
        private bool preventDblClick;    // отменить двойной щелчок по узлу дерева

        private Settings settings;       // настройки приложения
        private FrmReplace frmReplace;   // форма замены


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            allNodes = new List<TreeNode>();
            preventDblClick = false;
            settings = AppData.Settings;
            frmReplace = null;
        }


        /// <summary>
        /// Инициализировать дерево проводника
        /// </summary>
        private void InitTreeView()
        {
            treeView.Nodes.Clear();
            allNodes.Clear();

            nodDB = treeView.Nodes.Add("DB", AppPhrases.DbNode, "db_gray.gif", "db_gray.gif");
            allNodes.Add(nodDB);


            // системные таблицы
            nodSystem = nodDB.Nodes.Add("System", AppPhrases.SystemNode, "folder_closed.gif", "folder_closed.gif");
            allNodes.Add(nodSystem);

            TreeNode nodTable = nodSystem.Nodes.Add("Obj", CommonPhrases.ObjTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Obj);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("CommLine", CommonPhrases.CommLineTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.CommLine);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("KP", CommonPhrases.KPTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.KP);
            allNodes.Add(nodTable);

            nodInCnl = nodSystem.Nodes.Add("InCnl", CommonPhrases.InCnlTable, "table.gif", "table.gif");
            nodInCnl.Tag = new NodeInfo(NodeActions.InCnl);
            allNodes.Add(nodTable);

            nodCtrlCnl = nodSystem.Nodes.Add("CtrlCnl", CommonPhrases.CtrlCnlTable, "table.gif", "table.gif");
            nodCtrlCnl.Tag = new NodeInfo(NodeActions.CtrlCnl);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("Role", CommonPhrases.RoleTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Role);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("User", CommonPhrases.UserTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.User);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("Interface", CommonPhrases.InterfaceTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Interface);
            allNodes.Add(nodTable);

            nodTable = nodSystem.Nodes.Add("Right", CommonPhrases.RightTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Right);
            allNodes.Add(nodTable);

            // таблицы справочников
            nodDict = nodDB.Nodes.Add("Dict", AppPhrases.DictNode, "folder_closed.gif", "folder_closed.gif");
            allNodes.Add(nodDict);

            nodTable = nodDict.Nodes.Add("CnlType", CommonPhrases.CnlTypeTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.CnlType);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("CmdType", CommonPhrases.CmdTypeTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.CmdType);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("EvType", CommonPhrases.EvTypeTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.EvType);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("KPType", CommonPhrases.KPTypeTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.KPType);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("Param", CommonPhrases.ParamTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Param);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("Unit", CommonPhrases.UnitTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Unit);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("CmdVal", CommonPhrases.CmdValTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.CmdVal);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("Format", CommonPhrases.FormatTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Format);
            allNodes.Add(nodTable);

            nodTable = nodDict.Nodes.Add("Formula", CommonPhrases.FormulaTable, "table.gif", "table.gif");
            nodTable.Tag = new NodeInfo(NodeActions.Formula);
            allNodes.Add(nodTable);
        }

        /// <summary>
        /// Создать форму редактирования таблицы
        /// </summary>
        private FrmTable NewTableForm(string text, DataTable table)
        {
            FrmTable frmTable = new FrmTable();
            frmTable.Text = text;
            frmTable.Table = table;
            return frmTable;
        }

        /// <summary>
        /// Выполнить действие, связанное с узлом дерева
        /// </summary>
        private void ExecNodeAction(TreeNode node)
        {
            NodeInfo nodeInfo = node.Tag as NodeInfo;
            if (nodeInfo != null)
            {
                if (nodeInfo.Form == null)
                {
                    FrmTable frmTable = null;
                    string imageKey = "table.gif";

                    try
                    {
                        int param = -1;
                        object[] paramArr = nodeInfo.Params;
                        if (paramArr != null && paramArr.Length > 0 && paramArr[0] is int)
                            param = (int)paramArr[0];
                        string nodeText = node.Text;

                        switch (nodeInfo.NodeAction)
                        {
                            case NodeActions.Obj:
                                frmTable = NewTableForm(nodeText, Tables.GetObjTable());
                                break;
                            case NodeActions.CommLine:
                                frmTable = NewTableForm(nodeText, Tables.GetCommLineTable());
                                break;
                            case NodeActions.KP:
                                frmTable = NewTableForm(nodeText, Tables.GetKPTable());
                                break;
                            case NodeActions.InCnl:
                                frmTable = NewTableForm(nodeText, Tables.GetInCnlTable());
                                frmTable.GridContextMenu = contextInCnls;
                                break;
                            case NodeActions.InCnlObj:
                                frmTable = NewTableForm(CommonPhrases.InCnlTable + " - " + nodeText, 
                                    Tables.GetInCnlTableByObjNum(param));
                                frmTable.GridContextMenu = contextInCnls;
                                imageKey = "object.gif";
                                break;
                            case NodeActions.InCnlKP:
                                frmTable = NewTableForm(CommonPhrases.InCnlTable + " - " + nodeText, 
                                    Tables.GetInCnlTableByKPNum(param));
                                frmTable.GridContextMenu = contextInCnls;
                                imageKey = "kp.gif";
                                break;
                            case NodeActions.CtrlCnl:
                                frmTable = NewTableForm(nodeText, Tables.GetCtrlCnlTable());
                                break;
                            case NodeActions.CtrlCnlObj:
                                frmTable = NewTableForm(CommonPhrases.CtrlCnlTable + " - " + nodeText, 
                                    Tables.GetCtrlCnlTableByObjNum(param));
                                imageKey = "object.gif";
                                break;
                            case NodeActions.CtrlCnlKP:
                                frmTable = NewTableForm(CommonPhrases.CtrlCnlTable + " - " + nodeText, 
                                    Tables.GetCtrlCnlTableByKPNum(param));
                                imageKey = "kp.gif";
                                break;
                            case NodeActions.Role:
                                frmTable = NewTableForm(nodeText, Tables.GetRoleTable());
                                break;
                            case NodeActions.User:
                                frmTable = NewTableForm(nodeText, Tables.GetUserTable());
                                break;
                            case NodeActions.Interface:
                                frmTable = NewTableForm(nodeText, Tables.GetInterfaceTable());
                                break;
                            case NodeActions.Right:
                                frmTable = NewTableForm(nodeText, Tables.GetRightTable());
                                break;
                            case NodeActions.CnlType:
                                frmTable = NewTableForm(nodeText, Tables.GetCnlTypeTable());
                                break;
                            case NodeActions.CmdType:
                                frmTable = NewTableForm(nodeText, Tables.GetCmdTypeTable());
                                break;
                            case NodeActions.EvType:
                                frmTable = NewTableForm(nodeText, Tables.GetEvTypeTable());
                                break;
                            case NodeActions.KPType:
                                frmTable = NewTableForm(nodeText, Tables.GetKPTypeTable());
                                break;
                            case NodeActions.Param:
                                frmTable = NewTableForm(nodeText, Tables.GetParamTable());
                                break;
                            case NodeActions.Unit:
                                frmTable = NewTableForm(nodeText, Tables.GetUnitTable());
                                break;
                            case NodeActions.CmdVal:
                                frmTable = NewTableForm(nodeText, Tables.GetCmdValTable());
                                break;
                            case NodeActions.Format:
                                frmTable = NewTableForm(nodeText, Tables.GetFormatTable());
                                break;
                            case NodeActions.Formula:
                                frmTable = NewTableForm(nodeText, Tables.GetFormulaTable());
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        AppUtils.ProcError(ex.Message);
                        frmTable = null;
                    }

                    if (frmTable != null)
                    {
                        frmTable.FormClosed += ChildFormClosed;
                        nodeInfo.Form = frmTable;
                        winControl.AddForm(frmTable, "", ilTree.Images[imageKey], node);
                    }
                }
                else
                {
                    winControl.ActivateForm(nodeInfo.Form);
                }
                SetItemsEnabledOnWindowAction();
            }
        }

        /// <summary>
        /// Обработать событие при закрытии дочерней формы
        /// </summary>
        private void ChildFormClosed(object sender, FormClosedEventArgs e)
        {
            // очистка ссылки на форму, связанную с узлом дерева
            TreeNode treeNode = sender is IChildForm childForm ? childForm.ChildFormTag?.TreeNode : null;

            if (treeNode == null)
            {
                foreach (TreeNode node in allNodes)
                {
                    NodeInfo nodeInfo = node.Tag as NodeInfo;
                    if (nodeInfo != null && nodeInfo.Form == sender)
                    {
                        nodeInfo.Form = null;
                        break;
                    }
                }
            }
            else
            {
                if (treeNode.Tag is NodeInfo nodeInfo)
                    nodeInfo.Form = null;
            }
        }

        /// <summary>
        /// Соединиться с БД
        /// </summary>
        private bool Connect(bool expandTree)
        {
            bool result;
            bool connectNeeded = !AppData.Connected;

            try
            {
                // соединение с БД
                if (connectNeeded)
                {
                    AppData.Connect();

                    nodDB.ImageKey = nodDB.SelectedImageKey = "db.gif";
                    nodSystem.Expand();
                    nodDict.Expand();
                    SetItemsEnabledOnConnect();
                }

                result = true;
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.ConnectError + ":\r\n" + ex.Message);
                result = false;
            }

            if (result)
            {
                // раскрытие дерева проводника
                if (expandTree)
                {
                    treeView.BeforeExpand -= treeView_BeforeExpand;
                    nodDB.Expand();
                    treeView.BeforeExpand += treeView_BeforeExpand;
                }

                // группировка каналов
                if (connectNeeded)
                    GroupCnls();
            }

            return result;
        }

        /// <summary>
        /// Разъединиться с БД
        /// </summary>
        private void Disconnect()
        {
            try
            {
                if (AppData.Connected)
                {
                    AppData.Disconnect();

                    nodDB.ImageKey = nodDB.SelectedImageKey = "db_gray.gif";
                    treeView.CollapseAll();
                    SetItemsEnabledOnConnect();
                }
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.DisconnectError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Группировать каналы, создав соответствующие узлы дерева проводника
        /// </summary>
        private void GroupCnls()
        {
            try
            {
                treeView.BeginUpdate();
                nodInCnl.Nodes.Clear();
                nodCtrlCnl.Nodes.Clear();

                DataTable table;
                TreeNode node, clone;
                NodeInfo info;

                if (miViewGroupByObj.Checked)
                {
                    // создание узлов объектов
                    table = Tables.GetObjTable();
                    foreach (DataRow row in table.Rows)
                    {
                        int objID = (int)row["ObjNum"];
                        node = nodInCnl.Nodes.Add("InCnlObj" + objID, (string)row["Name"], "object.gif", "object.gif");
                        info = new NodeInfo(NodeActions.InCnlObj);
                        info.Params = new object[] { objID };
                        node.Tag = info;

                        clone = node.Clone() as TreeNode;
                        info = new NodeInfo(NodeActions.CtrlCnlObj);
                        info.Params = new object[] { objID };
                        clone.Tag = info;
                        nodCtrlCnl.Nodes.Add(clone);
                    }

                    // создание узла с неопределённым объектом
                    node = nodInCnl.Nodes.Add("InCnlObjNull", AppPhrases.UndefObj, "object.gif", "object.gif");
                    info = new NodeInfo(NodeActions.InCnlObj);
                    info.Params = new object[] { null };
                    node.Tag = info;

                    clone = node.Clone() as TreeNode;
                    info = new NodeInfo(NodeActions.CtrlCnlObj);
                    info.Params = new object[] { null };
                    clone.Tag = info;
                    nodCtrlCnl.Nodes.Add(clone);
                }
                else // miViewGroupKP.Checked
                {
                    table = Tables.GetKPTable();
                    foreach (DataRow row in table.Rows)
                    {
                        int kpID = (int)row["KPNum"];
                        node = nodInCnl.Nodes.Add("InCnlKP" + kpID, (string)row["Name"], "kp.gif", "kp.gif");
                        info = new NodeInfo(NodeActions.InCnlKP);
                        info.Params = new object[] { kpID };
                        node.Tag = info;

                        clone = node.Clone() as TreeNode;
                        info = new NodeInfo(NodeActions.CtrlCnlKP);
                        info.Params = new object[] { kpID };
                        clone.Tag = info;
                        nodCtrlCnl.Nodes.Add(clone);
                    }

                    // создание узла с неопределённым КП
                    node = nodInCnl.Nodes.Add("InCnlKPNull", AppPhrases.UndefKP, "kp.gif", "kp.gif");
                    info = new NodeInfo(NodeActions.InCnlKP);
                    info.Params = new object[] { null };
                    node.Tag = info;

                    clone = node.Clone() as TreeNode;
                    info = new NodeInfo(NodeActions.CtrlCnlKP);
                    info.Params = new object[] { null };
                    clone.Tag = info;
                    nodCtrlCnl.Nodes.Add(clone);
                }
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.CnlGroupError + ":\r\n" + ex.Message);
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Устновить разрешения для элементов, зависящих от соединения с БД
        /// </summary>
        private void SetItemsEnabledOnConnect()
        {
            bool connected = AppData.Connected;
            miDbConnect.Enabled = btnConnect.Enabled = !connected;
            miDbDisconnect.Enabled = btnDisconnect.Enabled = connected;
            miDbPassToServer.Enabled = btnPassToServer.Enabled = connected;
            miDbExport.Enabled = connected;
            miDbImport.Enabled = connected;
            miViewGroupByObj.Enabled = btnGroupByObj.Enabled = connected;
            miViewGroupByKP.Enabled = btnGroupByKP.Enabled = connected;
            miServiceCreateCnls.Enabled = connected;
            miServiceCloneCnls.Enabled = connected;
            miServiceCnlsMap.Enabled = connected;
            miRemoteDownload.Enabled = connected;
            miRemoteUpload.Enabled = connected;
        }

        /// <summary>
        /// Устновить разрешения для элементов, зависящих от действий с окнами
        /// </summary>
        private void SetItemsEnabledOnWindowAction()
        {
            bool formsExist = winControl.FormCount > 0;
            miEditCut.Enabled = btnCut.Enabled = formsExist;
            miEditCopy.Enabled = btnCopy.Enabled = formsExist;
            miEditPaste.Enabled = btnPaste.Enabled = formsExist;
            miEditReplace.Enabled = formsExist;
            miWindowCloseActive.Enabled = formsExist;
            miWindowCloseAll.Enabled = formsExist;
            miWindowCloseAllButActive.Enabled = formsExist;
        }

        /// <summary>
        /// Подготовить к закрытию все дочерние формы
        /// </summary>
        private void PrepareCloseAll(bool showError)
        {
            List<Form> forms = winControl.Forms;
            foreach (Form form in forms)
            {
                FrmTable frmTable = form as FrmTable;
                if (frmTable != null)
                    frmTable.PrepareClose(showError);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            // локализация приложения
            if (Localization.LoadDictionaries(AppData.AppDirs.LangDir, "ScadaData", out string errMsg))
                CommonPhrases.Init();
            else
                ScadaUiUtils.ShowError(errMsg);

            if (Localization.LoadDictionaries(AppData.AppDirs.LangDir, "ScadaAdmin", out errMsg))
            {
                Translator.TranslateForm(this, "ScadaAdmin.FrmMain", null, contextExpolorer, contextInCnls);
                AppPhrases.Init();
                winControl.MessageText = AppPhrases.SelectTable;
                winControl.SaveReqCaption = AppPhrases.SaveReqCaption;
                winControl.SaveReqQuestion = AppPhrases.SaveReqQuestion;
                winControl.SaveReqYes = AppPhrases.SaveReqYes;
                winControl.SaveReqNo = AppPhrases.SaveReqNo;
                winControl.SaveReqCancel = AppPhrases.SaveReqCancel;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
            }

            // инициализация дерева проводника
            InitTreeView();

            // установка начального состояния разрешений элементов
            SetItemsEnabledOnConnect();
            SetItemsEnabledOnWindowAction();

            // загрузка состояния формы
            settings.LoadFormState();
            if (settings.FormSt.IsEmpty)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Left = settings.FormSt.Left;
                Top = settings.FormSt.Top;
                Width = settings.FormSt.Width;
                Height = settings.FormSt.Height;
                WindowState = settings.FormSt.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
                pnlLeft.Width = settings.FormSt.ExplorerWidth;
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            string errMsg;
            bool success = settings.LoadAppSettings(out errMsg);
            lblBaseSdfFile.Text = settings.AppSett.BaseSDFFile;
            if (success)
                Connect(true);
            else
                ScadaUiUtils.ShowError(errMsg);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // закрытие всех дочерних форм
            PrepareCloseAll(false);
            bool cancel;
            winControl.CloseAllForms(out cancel);
            e.Cancel = cancel;

            if (!cancel)
            {
                // сохранение состояния формы
                if (WindowState == FormWindowState.Normal)
                {
                    settings.FormSt.Left = Left;
                    settings.FormSt.Top = Top;
                    settings.FormSt.Width = Width;
                    settings.FormSt.Height = Height;
                    settings.FormSt.Maximized = false;
                }
                else
                {
                    settings.FormSt.Left = RestoreBounds.Left;
                    settings.FormSt.Top = RestoreBounds.Top;
                    settings.FormSt.Width = RestoreBounds.Width;
                    settings.FormSt.Height = RestoreBounds.Height;
                    if (WindowState == FormWindowState.Maximized)
                        settings.FormSt.Maximized = true;
                }
                settings.FormSt.ExplorerWidth = pnlLeft.Width;

                string errMsg;
                if (!settings.SaveFormState(out errMsg))
                    ScadaUiUtils.ShowError(errMsg);
            }
        }


        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                if (node == nodInCnl || node == nodCtrlCnl)
                    preventDblClick = true;
            }
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ExecNodeAction(e.Node);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (e.Clicks == 1 && e.Button == MouseButtons.Right &&
                (node == nodInCnl || node.Parent == nodInCnl || node == nodCtrlCnl || node.Parent == nodCtrlCnl))
            {
                contextExpolorer.Show(treeView, e.Location);
            }
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TreeNode selNode = treeView.SelectedNode;
                NodeInfo nodeInfo = selNode == null ? null : selNode.Tag as NodeInfo;

                if (nodeInfo == null)
                {
                    // свернуть или развернуть узел дерева
                    if (selNode.Nodes.Count > 0)
                        if (selNode.IsExpanded)
                            selNode.Collapse(true);
                        else
                            selNode.Expand();
                }
                else
                {
                    // открыть связанную с узлом дерева форму
                    ExecNodeAction(selNode);
                }
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (preventDblClick)
            {
                e.Cancel = true;
                preventDblClick = false;
            }
            else
            {
                TreeNode node = e.Node;
                if (node == nodDB)
                    e.Cancel = !Connect(false);
                else if (node == nodSystem || node == nodDict)
                    node.ImageKey = node.SelectedImageKey = "folder_open.gif";
            }
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (preventDblClick)
            {
                e.Cancel = true;
                preventDblClick = false;
            }
            else
            {
                TreeNode node = e.Node;
                if (node == nodSystem || node == nodDict)
                    node.ImageKey = node.SelectedImageKey = "folder_closed.gif";
            }
        }


        private void winControl_ActiveFormChanged(object sender, EventArgs e)
        {
            // закрыть форму замены
            if (frmReplace != null && frmReplace.Visible)
                frmReplace.Close();
        }


        private void miDbConnect_Click(object sender, EventArgs e)
        {
            Connect(true);
        }

        private void miDbDisconnect_Click(object sender, EventArgs e)
        {
            PrepareCloseAll(true);
            bool cancel;
            winControl.CloseAllForms(out cancel);
            if (!cancel)
            {
                Disconnect();
                SetItemsEnabledOnWindowAction();
            }
        }

        private void miDbPassToServer_Click(object sender, EventArgs e)
        {
            if (AppData.Connected)
            {
                // резервное копирование файла базы конфигурации
                Settings.AppSettings appSettings = settings.AppSett;
                if (appSettings.AutoBackupBase && 
                    !ImportExport.BackupSDF(appSettings.BaseSDFFile, appSettings.BackupDir, out string msg))
                    AppUtils.ProcError(msg);

                // конвертирование базы конфигурации в формат DAT
                if (ImportExport.PassBase(Tables.TableInfoList, appSettings.BaseDATDir, out msg))
                    ScadaUiUtils.ShowInfo(msg);
                else
                    AppUtils.ProcError(msg);
            }
        }

        private void miDbBackup_Click(object sender, EventArgs e)
        {
            // резервное копирование файла базы конфигурации
            string msg;
            if (ImportExport.BackupSDF(settings.AppSett.BaseSDFFile, settings.AppSett.BackupDir, out msg))
                ScadaUiUtils.ShowInfo(msg);
            else
                AppUtils.ProcError(msg);
        }

        private void miDbCompact_Click(object sender, EventArgs e)
        {
            // упаковка файла базы конфигурации
            try
            {
                if (AppData.Compact())
                    ScadaUiUtils.ShowInfo(AppPhrases.CompactCompleted);
                else
                    ScadaUiUtils.ShowError(AppPhrases.ConnectionUndefined);
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.CompactError + ":\r\n" + ex.Message);
            }
        }

        private void miDbExport_Click(object sender, EventArgs e)
        {
            // создание и отоборажение формы экспорта таблицы
            FrmExport frmExport = new FrmExport();
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null && frmTable.Table != null)
                frmExport.DefaultTableName = frmTable.Table.TableName;
            frmExport.DefaultDirectory = settings.AppSett.BaseDATDir;
            frmExport.ShowDialog();
        }

        private void miDbImport_Click(object sender, EventArgs e)
        {
            // создание и отоборажение формы импорта таблицы
            FrmImport frmImport = new FrmImport();
            FrmTable frmTable = winControl.ActiveForm as FrmTable;

            if (frmTable != null && frmTable.Table != null)
                frmImport.DefaultTableName = frmTable.Table.TableName;

            frmImport.DefaultBaseDATDir = settings.AppSett.BaseDATDir;
            frmImport.ShowDialog();
        }

        private void miDbExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miSettingsParams_Click(object sender, EventArgs e)
        {
            string oldBaseSdfFile = settings.AppSett.BaseSDFFile;

            // создание и отображение формы настроек приложения
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.ParamsToControls(settings.AppSett);

            if (frmSettings.ShowDialog() == DialogResult.OK)
            {
                frmSettings.ControlsToParams(settings.AppSett);
                lblBaseSdfFile.Text = settings.AppSett.BaseSDFFile;

                string errMsg;
                if (!settings.SaveAppSettings(out errMsg))
                    AppUtils.ProcError(errMsg);
            }

            // повторное соединение с БД, если изменился файл базы конфигурации
            if (oldBaseSdfFile != settings.AppSett.BaseSDFFile)
            {
                PrepareCloseAll(true);
                bool cancel;
                winControl.CloseAllForms(out cancel);
                if (!cancel)
                {
                    Disconnect();
                    SetItemsEnabledOnWindowAction();
                    Connect(true);
                }
            }
        }

        private void miSettingsLanguage_Click(object sender, EventArgs e)
        {
            // создание и отображение формы выбора языка
            string prevCultureName = FrmLanguage.CultureName;
            FrmLanguage frmLanguage = new FrmLanguage();
            if (frmLanguage.ShowDialog() == DialogResult.OK && prevCultureName != FrmLanguage.CultureName)
            {
                // запись культуры для выбранного языка в реестр
                string errMsg;
                if (Localization.WriteCulture(FrmLanguage.CultureName, out errMsg))
                    ScadaUiUtils.ShowInfo(AppPhrases.LanguageChanged);
                else
                    AppUtils.ProcError(errMsg);
            }
        }

        private void miEditCut_Click(object sender, EventArgs e)
        {
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null)
                frmTable.CellCut();
        }

        private void miEditCopy_Click(object sender, EventArgs e)
        {
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null)
                frmTable.CellCopy();
        }

        private void miEditPaste_Click(object sender, EventArgs e)
        {
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null)
                frmTable.CellPaste();
        }

        private void miEditReplace_Click(object sender, EventArgs e)
        {
            if (frmReplace == null || !frmReplace.Visible)
            {
                frmReplace = new FrmReplace();
                frmReplace.FrmTable = winControl.ActiveForm as FrmTable;

                // отображение формы замены по центру относительно главной формы
                // FormStartPosition = CenterParent работает только для модальных форм
                frmReplace.Left = (Left + Right - frmReplace.Width) / 2;
                frmReplace.Top = (Top + Bottom - frmReplace.Height) / 2;
                frmReplace.Show(this);
            }
            else
            {
                frmReplace.Activate();
            }
        }

        private void miViewGroupByObj_Click(object sender, EventArgs e)
        {
            miViewGroupByObj.Checked = btnGroupByObj.Checked = true;
            miViewGroupByKP.Checked = btnGroupByKP.Checked = false;
            GroupCnls();
        }

        private void miViewGroupByKP_Click(object sender, EventArgs e)
        {
            miViewGroupByObj.Checked = btnGroupByObj.Checked = false;
            miViewGroupByKP.Checked = btnGroupByKP.Checked = true;
            GroupCnls();
        }

        private void miServiceCreateCnls_Click(object sender, EventArgs e)
        {
            // создание каналов
            FrmCreateCnls.ShowDialog(settings.AppSett.CommDir);
        }

        private void miServiceCloneCnls_Click(object sender, EventArgs e)
        {
            // клонирование каналов
            FrmCloneCnls frmCloneCnl = new FrmCloneCnls();
            frmCloneCnl.ShowDialog();
        }

        private void miServiceCnlsMap_Click(object sender, EventArgs e)
        {
            // создание карты каналов
            FrmCnlMap frmCnlsMap = new FrmCnlMap();
            frmCnlsMap.ShowDialog();
        }

        private void miServiceRestart_Click(object sender, EventArgs e)
        {
            // перезапуск или запуск службы
            try
            {
                Cursor = Cursors.WaitCursor;

                // получение контроллера службы
                string serviceName = sender == miServiceRestartServer || sender == btnRestartServer ?
                    "ScadaServerService" : "ScadaCommService";
                ServiceController svcContr = new ServiceController(serviceName);

                // ожидание завершения запуска или оставновки службы
                if (svcContr.Status == ServiceControllerStatus.StartPending)
                    svcContr.WaitForStatus(ServiceControllerStatus.Running, ServiceWait);
                else if (svcContr.Status == ServiceControllerStatus.StopPending)
                    svcContr.WaitForStatus(ServiceControllerStatus.Stopped, ServiceWait);

                if (svcContr.Status == ServiceControllerStatus.Running)
                {
                    // перезапуск службы
                    svcContr.Stop();
                    svcContr.WaitForStatus(ServiceControllerStatus.Stopped, ServiceWait);
                    svcContr.Start();
                }
                else if (svcContr.Status == ServiceControllerStatus.Stopped)
                {
                    // запуск службы
                    svcContr.Start();
                }
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.ServiceRestartError + ":\r\n" + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void miRemoteDownload_Click(object sender, EventArgs e)
        {
            // открытие формы скачивания конфигурации
            FrmDownloadConfig frmDownloadConfig = new FrmDownloadConfig();
            frmDownloadConfig.ShowDialog();
        }

        private void miRemoteUpload_Click(object sender, EventArgs e)
        {
            // открытие формы передачи конфигурации
            FrmUploadConfig frmUploadConfig = new FrmUploadConfig();
            frmUploadConfig.ShowDialog();
        }

        private void miRemoteStatus_Click(object sender, EventArgs e)
        {
            // открытие формы статуса сервера
            FrmServerStatus frmServerStatus = new FrmServerStatus();
            frmServerStatus.ShowDialog();
        }

        private void miWindowCloseActive_Click(object sender, EventArgs e)
        {
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null)
            {
                frmTable.PrepareClose(true);
                bool cancel;
                winControl.CloseForm(frmTable, out cancel);
                if (!cancel)
                    SetItemsEnabledOnWindowAction();
            }
        }

        private void miWindowCloseAll_Click(object sender, EventArgs e)
        {
            PrepareCloseAll(true);
            bool cancel;
            winControl.CloseAllForms(out cancel);
            if (!cancel)
                SetItemsEnabledOnWindowAction();
        }

        private void miWindowCloseAllButActive_Click(object sender, EventArgs e)
        {
            // подготовка форм к закрытию
            List<Form> forms = winControl.Forms;
            foreach (Form form in forms)
            {
                if (form != winControl.ActiveForm)
                {
                    FrmTable frmTable = form as FrmTable;
                    if (frmTable != null)
                        frmTable.PrepareClose(true);
                }
            }

            // закрытие форм
            bool cancel;
            winControl.CloseAllButActive(out cancel);
        }

        private void miWindowPrev_Click(object sender, EventArgs e)
        {
            winControl.ActivatePrevious();
        }

        private void miWindowNext_Click(object sender, EventArgs e)
        {
            winControl.ActivateNext();
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            FrmAbout.ShowAbout();
        }


        private void miExplorerRefresh_Click(object sender, EventArgs e)
        {
            GroupCnls();
        }

        private void miInCnlProps_Click(object sender, EventArgs e)
        {
            FrmInCnlProps frmInCnlProps = new FrmInCnlProps();
            FrmTable frmTable = winControl.ActiveForm as FrmTable;
            if (frmTable != null && frmInCnlProps.ShowInCnlProps(frmTable) == DialogResult.OK)
                frmTable.UpdateTable();
        }
    }
}