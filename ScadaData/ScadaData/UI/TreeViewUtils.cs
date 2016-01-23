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
    /// <remarks>
    /// Objects of tree node tags must implement ITreeNode interface
    /// <para>Объекты тегов узлов дерева должны поддерживать интерфейс ITreeNode</para>
    /// </remarks>
    /// </summary>
    public static class TreeViewUtils
    {
        /// <summary>
        /// Поведение при перемещении узлов дерева
        /// </summary>
        public enum MoveBehavior
        {
            /// <summary>
            /// В пределах своего родителя
            /// </summary>
            WithinParent,
            /// <summary>
            /// Через однотипных родителей
            /// </summary>
            ThroughSimilarParents
        }


        /// <summary>
        /// Получить коллекцию дочерних узлов заданного родительского узла или самого дерева
        /// </summary>
        private static TreeNodeCollection GetChildNodes(this TreeView treeView, TreeNode parentNode)
        {
            return parentNode == null ? treeView.Nodes : parentNode.Nodes;
        }


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
        /// Найти родительский узел и индекс для вставки нового узла дерева с учётом выбранного узла
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
        /// Найти индекс для вставки в список дочерних узлов заданного родительского узла с учётом выбранного узла
        /// </summary>
        public static int FindInsertIndex(this TreeView treeView, TreeNode parentNode)
        {
            TreeNode node = treeView.SelectedNode;
            while (node != null && node.Parent != parentNode)
                node = node.Parent;

            TreeNodeCollection nodes = treeView.GetChildNodes(parentNode); 
            return node == null ? nodes.Count : node.Index + 1;
        }


        /// <summary>
        /// Добавить узел в конец списка дочерних узлов заданного родительского узла или самого дерева 
        /// <remarks>Метод рекомендуется использовать, если parentNode может быть равен null</remarks>
        /// </summary>
        public static void Add(this TreeView treeView, TreeNode parentNode, ITreeNode parentObj,
            TreeNode nodeToAdd)
        {
            if (parentObj == null)
                throw new ArgumentNullException("parentObj");
            if (nodeToAdd == null)
                throw new ArgumentNullException("nodeToInsert");

            if (parentObj.Children != null && nodeToAdd.Tag is ITreeNode)
            {
                TreeNodeCollection nodes = treeView.GetChildNodes(parentNode);
                IList list = parentObj.Children;
                ITreeNode obj = (ITreeNode)nodeToAdd.Tag;
                obj.Parent = parentObj;

                nodes.Add(nodeToAdd);
                list.Add(obj);
                treeView.SelectedNode = nodeToAdd;
            }
        }

        /// <summary>
        /// Добавить узел в конец списка дочерних узлов заданного родительского узла
        /// </summary>
        /// <remarks>Аргумент parentNode не может быть равен null</remarks>
        public static void Add(this TreeView treeView, TreeNode parentNode, TreeNode nodeToAdd)
        {
            if (parentNode == null)
                throw new ArgumentNullException("parentNode");

            if (parentNode.Tag is ITreeNode)
                treeView.Add(parentNode, (ITreeNode)parentNode.Tag, nodeToAdd);
        }

        /// <summary>
        /// Вставить узел в список дочерних узлов заданного родительского узла или самого дерева 
        /// после выбранного узла дерева
        /// <remarks>Метод рекомендуется использовать, если parentNode может быть равен null</remarks>
        /// </summary>
        public static void Insert(this TreeView treeView, TreeNode parentNode, ITreeNode parentObj, 
            TreeNode nodeToInsert)
        {
            if (parentObj == null)
                throw new ArgumentNullException("parentObj");
            if (nodeToInsert == null)
                throw new ArgumentNullException("nodeToInsert");

            if (parentObj.Children != null && nodeToInsert.Tag is ITreeNode)
            {
                int index = treeView.FindInsertIndex(parentNode);
                TreeNodeCollection nodes = treeView.GetChildNodes(parentNode);
                IList list = parentObj.Children;
                ITreeNode obj = (ITreeNode)nodeToInsert.Tag;
                obj.Parent = parentObj;

                nodes.Insert(index, nodeToInsert);
                list.Insert(index, obj);
                treeView.SelectedNode = nodeToInsert;
            }
        }

        /// <summary>
        /// Вставить узел в список дочерних узлов заданного родительского узла после выбранного узла дерева
        /// </summary>
        /// <remarks>Аргумент parentNode не может быть равен null</remarks>
        public static void Insert(this TreeView treeView, TreeNode parentNode, TreeNode nodeToInsert)
        {
            if (parentNode == null)
                throw new ArgumentNullException("parentNode");

            if (parentNode.Tag is ITreeNode)
                treeView.Insert(parentNode, (ITreeNode)parentNode.Tag, nodeToInsert);
        }

        /// <summary>
        /// Переместить выбранный узел дерева и элемент соответствующего списка вверх по дереву
        /// </summary>
        public static void MoveUpSelectedNode(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            ITreeNode selectedObj = selectedNode == null ? null : selectedNode.Tag as ITreeNode;

            if (selectedObj != null)
            {
                try
                {
                    treeView.BeginUpdate();

                    TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                    IList list = selectedObj.Parent.Children;

                    int index = selectedNode.Index;
                    int newIndex = index - 1;

                    if (newIndex >= 0)
                    {
                        nodes.RemoveAt(index);
                        nodes.Insert(newIndex, selectedNode);

                        list.RemoveAt(index);
                        list.Insert(newIndex, selectedObj);

                        treeView.SelectedNode = selectedNode;
                    }
                    else if (moveBehavior == MoveBehavior.ThroughSimilarParents)
                    {
                        TreeNode parentNode = selectedNode.Parent;
                        TreeNode prevParentNode = parentNode == null ? null : parentNode.PrevNode;

                        if (parentNode != null && prevParentNode != null && 
                            parentNode.Tag is ITreeNode && prevParentNode.Tag is ITreeNode &&
                            parentNode.Tag.GetType() == prevParentNode.Tag.GetType())
                        {
                            // изменение родителя перемещаемого узла
                            nodes.RemoveAt(index);
                            prevParentNode.Nodes.Add(selectedNode);

                            ITreeNode prevParentObj = (ITreeNode)prevParentNode.Tag;
                            list.RemoveAt(index);
                            prevParentObj.Children.Add(selectedObj);
                            selectedObj.Parent = prevParentObj;

                            treeView.SelectedNode = selectedNode;
                        }
                    }
                }
                finally
                {
                    treeView.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Переместить выбранный узел дерева и элемент соответствующего списка вниз по дереву
        /// </summary>
        public static void MoveDownSelectedNode(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            ITreeNode selectedObj = selectedNode == null ? null : selectedNode.Tag as ITreeNode;

            if (selectedObj != null)
            {
                try
                {
                    treeView.BeginUpdate();

                    TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                    IList list = selectedObj.Parent.Children;

                    int index = selectedNode.Index;
                    int newIndex = index + 1;

                    if (newIndex < nodes.Count)
                    {
                        nodes.RemoveAt(index);
                        nodes.Insert(newIndex, selectedNode);

                        list.RemoveAt(index);
                        list.Insert(newIndex, selectedObj);

                        treeView.SelectedNode = selectedNode;
                    }
                    else if (moveBehavior == MoveBehavior.ThroughSimilarParents)
                    {
                        TreeNode parentNode = selectedNode.Parent;
                        TreeNode nextParentNode = parentNode == null ? null : parentNode.NextNode;

                        if (parentNode != null && nextParentNode != null &&
                            parentNode.Tag is ITreeNode && nextParentNode.Tag is ITreeNode &&
                            parentNode.Tag.GetType() == nextParentNode.Tag.GetType())
                        {
                            // изменение родителя перемещаемого узла
                            nodes.RemoveAt(index);
                            nextParentNode.Nodes.Insert(0, selectedNode);

                            ITreeNode nextParentObj = (ITreeNode)nextParentNode.Tag;
                            list.RemoveAt(index);
                            nextParentObj.Children.Insert(0, selectedObj);
                            selectedObj.Parent = nextParentObj;

                            treeView.SelectedNode = selectedNode;
                        }
                    }
                }
                finally
                {
                    treeView.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Переместить выбранный узел дерева и элемент соответствующего списка в заданную позицию
        /// </summary>
        public static void MoveSelectedNode(this TreeView treeView, int newIndex)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            ITreeNode selectedObj = selectedNode == null ? null : selectedNode.Tag as ITreeNode;

            if (selectedObj != null)
            {
                try
                {
                    treeView.BeginUpdate();

                    TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                    IList list = selectedObj.Parent.Children;

                    if (0 <= newIndex && newIndex < nodes.Count)
                    {
                        int index = selectedNode.Index;

                        nodes.RemoveAt(index);
                        nodes.Insert(newIndex, selectedNode);

                        list.RemoveAt(index);
                        list.Insert(newIndex, selectedObj);

                        treeView.SelectedNode = selectedNode;
                    }
                }
                finally
                {
                    treeView.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Удалить выбранный узел дерева и элемент из соответствующего списка
        /// </summary>
        public static void RemoveSelectedNode(this TreeView treeView)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag is ITreeNode)
            {
                TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                IList list = ((ITreeNode)selectedNode.Tag).Parent.Children;
                
                int selectedIndex = selectedNode.Index;
                nodes.RemoveAt(selectedIndex);
                list.RemoveAt(selectedIndex);
            }
        }

        /// <summary>
        /// Проверить, что перемещение выбранного узла дерева вверх возможно
        /// </summary>
        public static bool MoveUpSelectedNodeIsEnabled(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (selectedNode == null)
            {
                return false;
            }
            else if (selectedNode.PrevNode == null)
            {
                if (moveBehavior == MoveBehavior.ThroughSimilarParents)
                {
                        TreeNode parentNode = selectedNode.Parent;
                        TreeNode prevParentNode = parentNode == null ? null : parentNode.PrevNode;

                        return parentNode != null && prevParentNode != null &&
                            parentNode.Tag is ITreeNode && prevParentNode.Tag is ITreeNode &&
                            parentNode.Tag.GetType() == prevParentNode.Tag.GetType();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Проверить, что перемещение выбранного узла дерева вниз возможно
        /// </summary>
        public static bool MoveDownSelectedNodeIsEnabled(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (selectedNode == null)
            {
                return false;
            }
            else if (selectedNode.NextNode == null)
            {
                if (moveBehavior == MoveBehavior.ThroughSimilarParents)
                {
                    TreeNode parentNode = selectedNode.Parent;
                    TreeNode nextParentNode = parentNode == null ? null : parentNode.NextNode;

                    return parentNode != null && nextParentNode != null &&
                        parentNode.Tag is ITreeNode && nextParentNode.Tag is ITreeNode &&
                        parentNode.Tag.GetType() == nextParentNode.Tag.GetType();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
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

        /// <summary>
        /// Установить основное изображение узла и изображение в выбранном состоянии
        /// </summary>
        public static void SetImageKey(this TreeNode treeNode, string imageKey)
        {
            treeNode.ImageKey = treeNode.SelectedImageKey = imageKey;
        }
    }
}
