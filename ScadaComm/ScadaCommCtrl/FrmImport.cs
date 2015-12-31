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
 * Module   : SCADA-Communicator Control
 * Summary  : Import communicatoin lines and devices form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2014
 */

using Scada.UI;
using System;
using System.Data;
using System.Windows.Forms;
using Utils;

namespace Scada.Comm.Ctrl
{
    /// <summary>
    /// Import communicatoin lines and devices form
    /// <para>Форма импорта линий связи и КП</para>
    /// </summary>
    public partial class FrmImport : Form
    {
        private bool checking;   // происходит программное переключение узла дерева
        private int commLineNum; // номер линии связи, для которой выполняется импорт


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmImport()
        {
            InitializeComponent();
            checking = false;
            commLineNum = -1;
        }


        /// <summary>
        /// Получить дерево линий связи и КП
        /// </summary>
        public TreeView TreeView
        {
            get
            {
                return treeView;
            }
        }


        /// <summary>
        /// Импорт линий связи и КП
        /// </summary>
        public static DialogResult Import(DataTable tblCommLine, DataTable tblKP, Log errLog, out FrmImport frmImport)
        {
            frmImport = null;

            try
            {
                if (tblCommLine.DefaultView.Count == 0)
                {
                    ScadaUiUtils.ShowInfo(AppPhrases.NoImportData);
                    return DialogResult.Cancel;
                }
                else
                {
                    // заполнение дерева импортируемых данных: линий связи и КП
                    frmImport = new FrmImport();
                    TreeView treeView = frmImport.TreeView;

                    try
                    {
                        treeView.BeginUpdate();
                        treeView.Nodes.Clear();

                        tblCommLine.DefaultView.Sort = "CommLineNum";
                        tblKP.DefaultView.Sort = "CommLineNum, KPNum";
                        int kpCnt = tblKP.DefaultView.Count;
                        int kpInd = 0;

                        foreach (DataRowView rowLine in tblCommLine.DefaultView)
                        {
                            int lineNum = (int)rowLine["CommLineNum"];
                            TreeNode nodeLine = new TreeNode(Settings.CommLine.GetCaption(lineNum, rowLine["Name"]));
                            nodeLine.Tag = rowLine;
                            treeView.Nodes.Add(nodeLine);

                            DataRowView rowKP = kpInd < kpCnt ? tblKP.DefaultView[kpInd] : null;
                            int kpLineNum = rowKP == null ? -1 : (int)rowKP["CommLineNum"];

                            while (0 <= kpLineNum && kpLineNum <= lineNum)
                            {
                                if (kpLineNum == lineNum)
                                {
                                    TreeNode nodeKP = 
                                        new TreeNode(Settings.KP.GetCaption((int)rowKP["KPNum"], rowKP["Name"]));
                                    nodeKP.Tag = rowKP;
                                    nodeLine.Nodes.Add(nodeKP);
                                }

                                kpInd++;
                                rowKP = kpInd < kpCnt ? tblKP.DefaultView[kpInd] : null;
                                kpLineNum = rowKP == null ? -1 : (int)rowKP["CommLineNum"];
                            }
                        }
                    }
                    finally
                    {
                        treeView.EndUpdate();
                    }

                    // отображение формы импорта
                    return frmImport.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.PrepareImportFormError1 + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);

                return DialogResult.Abort;
            }
        }

        /// <summary>
        /// Импорт КП
        /// </summary>
        public static DialogResult Import(DataTable tblKP, int commLineNum, Log errLog, out FrmImport frmImport)
        {
            frmImport = null;

            try
            {
                tblKP.DefaultView.RowFilter = "CommLineNum = " + commLineNum;
                tblKP.DefaultView.Sort = "KPNum";

                if (tblKP.DefaultView.Count == 0)
                {
                    ScadaUiUtils.ShowInfo(AppPhrases.NoImportData);
                    return DialogResult.Cancel;
                }
                else
                {
                    // заполнение дерева импортируемых данных: только КП
                    frmImport = new FrmImport();
                    frmImport.commLineNum = commLineNum;
                    TreeView treeView = frmImport.TreeView;
                    treeView.ShowRootLines = false;

                    try
                    {
                        treeView.BeginUpdate();
                        treeView.Nodes.Clear();

                        foreach (DataRowView rowKP in tblKP.DefaultView)
                        {
                            TreeNode nodeKP = new TreeNode(Settings.KP.GetCaption((int)rowKP["KPNum"], rowKP["Name"]));
                            nodeKP.Tag = rowKP;
                            treeView.Nodes.Add(nodeKP);
                        }
                    }
                    finally
                    {
                        treeView.EndUpdate();
                    }

                    // отображение формы импорта
                    return frmImport.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.PrepareImportFormError2 + ":\r\n" + ex.Message;
                errLog.WriteAction(errMsg);
                ScadaUiUtils.ShowError(errMsg);

                return DialogResult.Abort;
            }
            finally
            {
                try { tblKP.DefaultView.RowFilter = ""; }
                catch { }
            }
        }


        private void FrmImport_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Comm.Ctrl.FrmImport");
            if (commLineNum > 0)
                Text = string.Format(AppPhrases.ImportAlternateTitle, commLineNum);
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // управление выбором узлов дерева при импорте линий связи и КП
            if (!checking)
            {
                checking = true;
                TreeNode node = e.Node;

                if (node.Parent == null)
                {
                    if (node.Nodes.Count > 0)
                    {
                        bool nodeChecked = node.Checked;
                        foreach (TreeNode childNode in node.Nodes)
                            childNode.Checked = nodeChecked;
                    }
                }
                else
                {
                    if (node.Checked)
                        node.Parent.Checked = true;
                }

                checking = false;
            }
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView.Nodes)
                node.Checked = true;
        }

        private void miDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView.Nodes)
                node.Checked = false;
        }
    }
}