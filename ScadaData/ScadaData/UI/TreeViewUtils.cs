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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// Utility methods for TreeView control
    /// <para>Вспомогательные методы для работы с элементом управления TreeView</para>
    /// </summary>
    public static class TreeViewUtils
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


        /// <summary>
        /// Найти ближайший узел дерева заданного типа по отношению к выбранному узлу вверх по дереву
        /// </summary>
        public static TreeNode FindClosest(this TreeView treeView, Type tagType)
        {
            if (tagType == null)
                throw new ArgumentNullException("tagType");

            TreeNode node = treeView.SelectedNode;
            while (node != null && !(tagType.IsInstanceOfType(node.Tag)))
                node = node.Parent;
            return node;
        }

        /// <summary>
        /// Найти индекс для вставки в список дочерних узлов заданного родительского узла
        /// </summary>
        public static int FindInsertIndex(this TreeView treeView, TreeNode parentNode)
        {
            TreeNode node = treeView.SelectedNode;
            while (node != null && node.Parent != parentNode)
                node = node.Parent;

            TreeNodeCollection nodes = parentNode == null ? treeView.Nodes : parentNode.Nodes;
            return node == null ? nodes.Count : node.Index + 1;
        }

        /// <summary>
        /// Найти родительский узел и индекс для вставки нового узла дерева
        /// </summary>
        public static bool FindInsertLocation(this TreeView treeView, Type tagType, 
            out TreeNode parentNode, out int index)
        {
            TreeNode node = treeView.FindClosest(tagType);

            if (node == null)
            {
                parentNode = null;
                index = -1;
                return false;
            }
            else
            {
                parentNode = node.Parent;
                index = node.Index + 1;
                return true;
            }
        }


        /// <summary>
        /// Вставить узел в дерево и объект из тега вставляемого узла в соответствующий список
        /// </summary>
        public static void Insert(this TreeView treeView, TreeNode parentNode, TreeNode nodeToInsert, 
            IList associatedList, int index)
        {
            if (nodeToInsert == null)
                throw new ArgumentNullException("nodeToInsert");
            if (associatedList == null)
                throw new ArgumentNullException("associatedList");

            TreeNodeCollection nodes = parentNode == null ? treeView.Nodes : parentNode.Nodes;
            nodes.Insert(index, nodeToInsert);
            associatedList.Insert(index, nodeToInsert.Tag);
            treeView.SelectedNode = nodeToInsert;
        }

        /// <summary>
        /// Вставить узел в дерево и объект из тега вставляемого узла в соответствующий список
        /// </summary>
        public static void Insert(this TreeView treeView, TreeNode parentNode, TreeNode nodeToInsert, 
            IList associatedList)
        {
            int index = treeView.FindInsertIndex(parentNode);
            treeView.Insert(parentNode, nodeToInsert, associatedList, index);
        }

        /// <summary>
        /// Переместить узел дерева и элемент соответствующего списка вверх
        /// </summary>
        public static void MoveUp(this TreeView treeView, TreeNode parentNode, IList associatedList, int index)
        {
            try
            {
                treeView.BeginUpdate();

                TreeNodeCollection nodes = parentNode == null ? treeView.Nodes : parentNode.Nodes;
                TreeNode node = nodes[index];
                int newIndex = index - 1;

                if (newIndex >= 0)
                {
                    nodes.RemoveAt(index);
                    nodes.Insert(newIndex, node);
                    associatedList.RemoveAt(index);
                    associatedList.Insert(newIndex, node.Tag);
                    treeView.SelectedNode = node;
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Переместить узел дерева и элемент соответствующего списка вниз
        /// </summary>
        public static void MoveDown(this TreeView treeView, TreeNode parentNode, IList associatedList, int index)
        {
            try
            {
                treeView.BeginUpdate();

                TreeNodeCollection nodes = parentNode == null ? treeView.Nodes : parentNode.Nodes;
                TreeNode node = nodes[index];
                int newIndex = index + 1;

                if (newIndex < nodes.Count)
                {
                    nodes.RemoveAt(index);
                    nodes.Insert(newIndex, node);
                    associatedList.RemoveAt(index);
                    associatedList.Insert(newIndex, node.Tag);
                    treeView.SelectedNode = node;
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Удалить узел из дерева и объект из соответствующего списка по индексу
        /// </summary>
        public static void Remove(this TreeView treeView, TreeNode parentNode, IList associatedList, int index)
        {
            if (associatedList == null)
                throw new ArgumentNullException("associatedList");

            TreeNodeCollection nodes = parentNode == null ? treeView.Nodes : parentNode.Nodes;
            nodes.RemoveAt(index);
            associatedList.RemoveAt(index);
        }
        
        /// <summary>
        /// Удалить выбранный узел из дерева и объект из соответствующего списка
        /// </summary>
        public static void RemoveSelectedNode(this TreeView treeView, IList associatedList)
        {
            if (treeView.SelectedNode != null)
                treeView.Remove(treeView.SelectedNode.Parent, associatedList, treeView.SelectedNode.Index);
        }


        /// <summary>
        /// Получить выбранный в дереве объект справочника
        /// </summary>
        public static object GetSelectedObject(this TreeView treeView)
        {
            return treeView.SelectedNode == null ? null : treeView.SelectedNode.Tag;
        }

        /// <summary>
        /// Получить выбранный в дереве объект справочника
        /// </summary>
        public static void UpdateSelectedNodeText(this TreeView treeView)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag != null)
                selectedNode.Text = selectedNode.Tag.ToString();
        }
    }
}
