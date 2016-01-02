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
 * Module   : ScadaData
 * Summary  : Utility methods for TreeView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// Utility methods for TreeView control
    /// <para>Вспомогательные методы для работы с элементом управления TreeView</para>
    /// </summary>
    public class TreeViewUtils
    {
        /// <summary>
        /// Создать узел дерева
        /// </summary>
        public static TreeNode CreateNode(string text, string imageKey, bool expand = false)
        {
            TreeNode node = new TreeNode(text)
            {
                ImageKey = imageKey,
                SelectedImageKey = imageKey
            };

            if (expand)
                node.Expand();

            return node;
        }

        /// <summary>
        /// Создать узел дерева на основе заданного объекта
        /// </summary>
        public static TreeNode CreateNode(object tag, string imageKey, bool expand = false)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            TreeNode node = CreateNode(tag.ToString(), imageKey, expand);
            node.Tag = tag;
            return node;
        }
    }
}
