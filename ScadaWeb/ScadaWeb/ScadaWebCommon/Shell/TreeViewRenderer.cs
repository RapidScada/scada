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
 * Module   : ScadaWebCommon
 * Summary  : Renders tree view HTML
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Renders tree view HTML.
    /// <para>Формирует HTML код дерева.</para>
    /// </summary>
    public class TreeViewRenderer
    {
        /// <summary>
        /// Параметры отображения дерева
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Получить или установить 
            /// </summary>
            public bool ShowIcons { get; set; }
            /// <summary>
            /// Получить или установить ссылку на иконку папки
            /// </summary>
            public string FolderImageUrl { get; set; }
            /// <summary>
            /// Получить или установить ссылку на иконку документа, если иконка узла пустая
            /// </summary>
            public string DocumentImageUrl { get; set; }
        }


        /// <summary>
        /// Генерировать HTML-код атрибутов данных
        /// </summary>
        protected string GenDataAttrsHtml(IWebTreeNode webTreeNode)
        {
            const string DataAttrTemplate = " data-{0}='{1}'";

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append(string.Format(DataAttrTemplate, "script", webTreeNode.Script));
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
        protected string GenTreeViewHtml(IList treeNodes, object selObj, Options options, bool topLevel)
        {
            const string NodeTemplate =
                "<a class='node{0}' href='{1}' {2}>" +
                "<div class='node-items'>" +
                "<div class='indent'></div>" +
                "<div class='expander left{3}'></div>" +
                "<div class='stateIcon'></div>" +
                "<div class='icon'>{4}</div>" +
                "<div class='text'>{5}</div>" +
                "<div class='expander right{3}'></div>" +
                "</div></a>";
            const string IconTemplate = "<img src='{0}' alt='' />";

            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine(topLevel ? 
                "<div class='tree-view'>" : 
                "<div class='child-nodes'>");

            if (treeNodes != null)
            {
                foreach (object treeNode in treeNodes)
                {
                    if (treeNode is IWebTreeNode webTreeNode &&
                        !webTreeNode.Hidden)
                    {
                        bool containsSubitems = webTreeNode.Children.Count > 0;
                        bool urlIsEmpty = string.IsNullOrEmpty(webTreeNode.Url);
                        string nodeCssClass = (webTreeNode.IsSelected(selObj) ? " selected" : "") + 
                            (!containsSubitems && urlIsEmpty ? " disabled" : "");
                        string dataAttrs = GenDataAttrsHtml(webTreeNode);
                        string expanderCssClass = containsSubitems ? "" : " empty";
                        string icon = "";

                        if (options.ShowIcons)
                        {
                            string iconUrl = string.IsNullOrEmpty(webTreeNode.IconUrl) ? 
                                (containsSubitems ? options.FolderImageUrl : options.DocumentImageUrl) : 
                                webTreeNode.IconUrl;
                            icon = string.Format(IconTemplate, iconUrl);
                        }

                        sbHtml.AppendLine(string.Format(NodeTemplate,
                            nodeCssClass, webTreeNode.Url, dataAttrs, expanderCssClass, icon, HttpUtility.HtmlEncode(webTreeNode.Text)));

                        if (containsSubitems)
                            sbHtml.Append(GenTreeViewHtml(webTreeNode.Children, selObj, options, false));
                    }
                }
            }

            sbHtml.AppendLine("</div>");
            return sbHtml.ToString();
        }


        /// <summary>
        /// Генерировать HTML-код дерева для узлов, поддерживающих IWebTreeNode
        /// </summary>
        public string GenerateHtml(IList treeNodes, object selObj, Options options)
        {
            if (options == null)
                options = new Options();

            return GenTreeViewHtml(treeNodes, selObj, options, true);
        }
    }
}
