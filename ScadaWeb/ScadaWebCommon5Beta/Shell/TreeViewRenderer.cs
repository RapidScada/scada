/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaWebCommon
 * Summary  : Renders tree view HTML
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Renders tree view HTML
    /// <para>Формирует HTML код дерева</para>
    /// </summary>
    public class TreeViewRenderer
    {
        /// <summary>
        /// Генерировать HTML-код атрибутов данных
        /// </summary>
        protected string GenDataAttrsHtml(IWebTreeNode webTreeNode)
        {
            const string DataAttrTemplate = " data-{0}='{1}'";

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append(string.Format(DataAttrTemplate, "level", webTreeNode.Level));

            if (webTreeNode.DataAttrs != null)
            {
                foreach (KeyValuePair<string, string> pair in webTreeNode.DataAttrs)
                {
                    if (!string.IsNullOrWhiteSpace(pair.Key))
                        sbHtml.Append(string.Format(DataAttrTemplate, pair.Key, pair.Value));
                }
            }

            return sbHtml.ToString();
        }

        /// <summary>
        /// Рекурсивно генерировать HTML-код дерева
        /// </summary>
        protected string GenTreeViewHtml(IList treeNodes, object selObj, bool topLevel)
        {
            const string NodeTemplate = 
                "<div class='node{0}'{1}>" + 
                "<div class='indent'></div>" +
                "<div class='expander{2}'></div>" +
                "<div class='stateIcon'></div>" +
                "<div class='icon{3}'>{4}</div>" +
                "<div class='text'>{5}</div></div>";
            const string ImageTemplate = "<img src='{0}' alt='' />";
            const string LinkTemplate = "<a href='{0}'>{1}</a>";

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine(topLevel ? 
                "<div class='tree-view'>" : 
                "<div class='child-nodes'>");

            if (treeNodes != null)
            {
                foreach (object treeNode in treeNodes)
                {
                    IWebTreeNode webTreeNode = treeNode as IWebTreeNode;
                    if (webTreeNode != null)
                    {
                        string selected = webTreeNode.IsSelected(selObj) ? " selected" : "";
                        string dataAttrs = GenDataAttrsHtml(webTreeNode);

                        bool containsSubitems = webTreeNode.Children.Count > 0;
                        string expanderEmpty = containsSubitems ? "" : " empty";

                        string iconEmpty;
                        string icon;
                        if (string.IsNullOrEmpty(webTreeNode.IconUrl))
                        {
                            iconEmpty = " empty";
                            icon = "";
                        }
                        else
                        {
                            iconEmpty = "";
                            icon = string.Format(ImageTemplate, webTreeNode.IconUrl);
                        }

                        string text = HttpUtility.HtmlEncode(webTreeNode.Text);
                        string linkOrText = containsSubitems || !string.IsNullOrEmpty(webTreeNode.Url) ?
                            string.Format(LinkTemplate, webTreeNode.Url, text) : text;

                        sbHtml.AppendLine(string.Format(NodeTemplate,
                            selected, dataAttrs, expanderEmpty, iconEmpty, icon, linkOrText));

                        if (containsSubitems)
                            sbHtml.Append(GenTreeViewHtml(webTreeNode.Children, selObj, false));
                    }
                }
            }

            sbHtml.AppendLine("</div>");
            return sbHtml.ToString();
        }


        /// <summary>
        /// Генерировать HTML-код дерева для узлов, поддерживающих IWebTreeNode
        /// </summary>
        public string GenerateHtml(IList treeNodes, object selObj)
        {
            return GenTreeViewHtml(treeNodes, selObj, true);
        }
    }
}
