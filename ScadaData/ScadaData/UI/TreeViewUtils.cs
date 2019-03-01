/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Modified : 2019
 */

using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Получить объект, связанный с узлом дерева
        /// </summary>
        private static object GetRelatedObject(TreeNode treeNode)
        {
            return treeNode?.Tag is TreeNodeTag treeNodeTag ?
                treeNodeTag.RelatedObject : treeNode?.Tag;
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
        /// Найти первый узел дерева заданного типа среди всех дочерних узлов, включая заданный узел
        /// </summary>
        public static TreeNode FindFirst(this TreeNode treeNode, Type tagType)
        {
            foreach (TreeNode childNode in IterateNodes(treeNode))
            {
                if (tagType.IsInstanceOfType(childNode.Tag))
                    return childNode;
            }

            return null;
        }

        /// <summary>
        /// Найти первый узел дерева заданного типа среди всех дочерних узлов, включая заданный узел
        /// </summary>
        public static TreeNode FindFirst(this TreeNode treeNode, string nodeType)
        {
            foreach (TreeNode childNode in IterateNodes(treeNode))
            {
                if (childNode.Tag is TreeNodeTag tag && tag.NodeType == nodeType)
                    return childNode;
            }

            return null;
        }

        /// <summary>
        /// Найти ближайший узел дерева заданного типа по отношению к заданному узлу вверх по дереву
        /// </summary>
        public static TreeNode FindClosest(this TreeNode treeNode, Type tagType)
        {
            if (tagType == null)
                throw new ArgumentNullException("tagType");

            while (treeNode != null && !tagType.IsInstanceOfType(treeNode.Tag))
            {
                treeNode = treeNode.Parent;
            }

            return treeNode;
        }

        /// <summary>
        /// Найти ближайший узел дерева заданного типа по отношению к заданному узлу вверх по дереву
        /// </summary>
        public static TreeNode FindClosest(this TreeNode treeNode, string nodeType)
        {
            while (treeNode != null && !(treeNode.Tag is TreeNodeTag tag && tag.NodeType == nodeType))
            {
                treeNode = treeNode.Parent;
            }

            return treeNode;
        }

        /// <summary>
        /// Найти узел дерева заданного типа среди узлов одного уровня
        /// </summary>
        public static TreeNode FindSibling(this TreeNode treeNode, Type tagType)
        {
            if (tagType == null)
                throw new ArgumentNullException("tagType");

            if (treeNode != null && treeNode.Parent != null)
            {
                foreach (TreeNode childNode in treeNode.Parent.Nodes)
                {
                    if (tagType.IsInstanceOfType(childNode.Tag))
                        return childNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Найти узел дерева заданного типа среди узлов одного уровня
        /// </summary>
        public static TreeNode FindSibling(this TreeNode treeNode, string nodeType)
        {
            if (treeNode != null && treeNode.Parent != null)
            {
                foreach (TreeNode childNode in treeNode.Parent.Nodes)
                {
                    if (childNode.Tag is TreeNodeTag tag && tag.NodeType == nodeType)
                        return childNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Проверить, имеет ли тег узла дерева указанный тип
        /// </summary>
        public static bool TagIs(this TreeNode treeNode, string nodeType)
        {
            return treeNode.Tag is TreeNodeTag tag && tag.NodeType == nodeType;
        }

        /// <summary>
        /// Найти родительский узел и индекс для вставки нового узла дерева с учётом выбранного узла
        /// </summary>
        public static bool FindInsertLocation(this TreeView treeView, Type tagType,
            out TreeNode parentNode, out int index)
        {
            TreeNode node = treeView.SelectedNode?.FindClosest(tagType);

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
            {
                node = node.Parent;
            }

            return node == null ? treeView.GetChildNodes(parentNode).Count : node.Index + 1;
        }


        /// <summary>
        /// Добавить узел в конец списка дочерних узлов заданного родительского узла или самого дерева 
        /// <remarks>Метод рекомендуется использовать, если объекты не поддерживают ITreeNode</remarks>
        /// </summary>
        public static void Add(this TreeView treeView, TreeNode parentNode, TreeNode nodeToAdd, 
            IList destList, object objToAdd)
        {
            if (nodeToAdd == null)
                throw new ArgumentNullException("nodeToAdd");
            if (destList == null)
                throw new ArgumentNullException("destList");
            if (objToAdd == null)
                throw new ArgumentNullException("objToAdd");

            TreeNodeCollection nodes = treeView.GetChildNodes(parentNode);
            nodes.Add(nodeToAdd);
            destList.Add(objToAdd);
            treeView.SelectedNode = nodeToAdd;
        }

        /// <summary>
        /// Добавить узел в конец списка дочерних узлов заданного родительского узла
        /// </summary>
        /// <remarks>Аргумент parentNode не может быть равен null</remarks>
        public static void Add(this TreeView treeView, TreeNode parentNode, TreeNode nodeToAdd)
        {
            if (parentNode == null)
                throw new ArgumentNullException("parentNode");

            if (GetRelatedObject(parentNode) is ITreeNode parentObj &&
                GetRelatedObject(nodeToAdd) is ITreeNode objToAdd)
            {
                objToAdd.Parent = parentObj;
                treeView.Add(parentNode, nodeToAdd, parentObj.Children, objToAdd);
            }
        }

        /// <summary>
        /// Вставить узел в список дочерних узлов заданного родительского узла или самого дерева 
        /// после выбранного узла дерева
        /// </summary>
        /// <remarks>
        /// Метод рекомендуется использовать, если parentNode равен null или объекты не поддерживают ITreeNode
        /// </remarks>
        public static void Insert(this TreeView treeView, TreeNode parentNode, TreeNode nodeToInsert, 
            IList destList, object objToInsert)
        {
            if (nodeToInsert == null)
                throw new ArgumentNullException("nodeToInsert");
            if (destList == null)
                throw new ArgumentNullException("destList");
            if (objToInsert == null)
                throw new ArgumentNullException("objToInsert");

            int index = treeView.FindInsertIndex(parentNode);
            TreeNodeCollection nodes = treeView.GetChildNodes(parentNode);

            nodes.Insert(index, nodeToInsert);
            destList.Insert(index, objToInsert);
            treeView.SelectedNode = nodeToInsert;
        }

        /// <summary>
        /// Вставить узел в список дочерних узлов заданного родительского узла после выбранного узла дерева
        /// </summary>
        /// <remarks>Аргумент parentNode не может быть равен null</remarks>
        public static void Insert(this TreeView treeView, TreeNode parentNode, TreeNode nodeToInsert)
        {
            if (parentNode == null)
                throw new ArgumentNullException("parentNode");

            if (GetRelatedObject(parentNode) is ITreeNode parentObj &&
                GetRelatedObject(nodeToInsert) is ITreeNode objToInsert)
            {
                objToInsert.Parent = parentObj;
                treeView.Insert(parentNode, nodeToInsert, parentObj.Children, objToInsert);
            }
        }

        /// <summary>
        /// Переместить выбранный узел дерева и элемент заданного списка вверх по дереву
        /// </summary>
        /// <remarks>Метод рекомендуется использовать, если объекты не поддерживают ITreeNode</remarks>
        public static void MoveUpSelectedNode(this TreeView treeView, IList sourceList)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (selectedNode != null)
            {
                try
                {
                    treeView.BeginUpdate();

                    TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                    int index = selectedNode.Index;
                    int newIndex = index - 1;

                    if (newIndex >= 0)
                    {
                        nodes.RemoveAt(index);
                        nodes.Insert(newIndex, selectedNode);

                        object selectedObj = sourceList[index];
                        sourceList.RemoveAt(index);
                        sourceList.Insert(newIndex, selectedObj);

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
        /// Переместить выбранный узел дерева и элемент соответствующего списка вверх по дереву
        /// </summary>
        public static void MoveUpSelectedNode(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (GetRelatedObject(selectedNode) is ITreeNode selectedObj)
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
        /// Переместить выбранный узел дерева и элемент заданного списка вниз по дереву
        /// </summary>
        /// <remarks>Метод рекомендуется использовать, если объекты не поддерживают ITreeNode</remarks>
        public static void MoveDownSelectedNode(this TreeView treeView, IList sourceList)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (selectedNode != null)
            {
                try
                {
                    treeView.BeginUpdate();

                    TreeNodeCollection nodes = treeView.GetChildNodes(selectedNode.Parent);
                    int index = selectedNode.Index;
                    int newIndex = index + 1;

                    if (newIndex < nodes.Count)
                    {
                        nodes.RemoveAt(index);
                        nodes.Insert(newIndex, selectedNode);

                        object selectedObj = sourceList[index];
                        sourceList.RemoveAt(index);
                        sourceList.Insert(newIndex, selectedObj);

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
        /// Переместить выбранный узел дерева и элемент соответствующего списка вниз по дереву
        /// </summary>
        public static void MoveDownSelectedNode(this TreeView treeView, MoveBehavior moveBehavior)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            if (GetRelatedObject(selectedNode) is ITreeNode selectedObj)
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

            if (GetRelatedObject(selectedNode) is ITreeNode selectedObj)
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
        /// Удалить заданный узел дерева и элемент списка
        /// </summary>
        /// <remarks>Метод рекомендуется использовать, если объекты не поддерживают ITreeNode</remarks>
        public static void RemoveNode(this TreeView treeView, TreeNode nodeToRemove, IList sourceList)
        {
            if (nodeToRemove != null)
            {
                TreeNodeCollection nodes = treeView.GetChildNodes(nodeToRemove.Parent);
                int index = nodeToRemove.Index;
                nodes.RemoveAt(index);
                sourceList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Удалить выбранный узел дерева и элемент из соответствующего списка
        /// </summary>
        public static void RemoveSelectedNode(this TreeView treeView)
        {
            if (GetRelatedObject(treeView.SelectedNode) is ITreeNode treeNodeObj)
                RemoveNode(treeView, treeView.SelectedNode, treeNodeObj.Parent.Children);
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
                        TreeNode prevParentNode = parentNode?.PrevNode;

                        return parentNode != null && prevParentNode != null &&
                            (parentNode.Tag is ITreeNode && prevParentNode.Tag is ITreeNode &&
                            parentNode.Tag.GetType() == prevParentNode.Tag.GetType() ||
                            parentNode.Tag is TreeNodeTag tag1 && prevParentNode.Tag is TreeNodeTag tag2 &&
                            tag1.NodeType == tag2.NodeType);
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
                    TreeNode nextParentNode = parentNode?.NextNode;

                    return parentNode != null && nextParentNode != null &&
                        (parentNode.Tag is ITreeNode && nextParentNode.Tag is ITreeNode &&
                        parentNode.Tag.GetType() == nextParentNode.Tag.GetType() ||
                        parentNode.Tag is TreeNodeTag tag1 && nextParentNode.Tag is TreeNodeTag tag2 &&
                            tag1.NodeType == tag2.NodeType);
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
        /// Получить объект выбранного узла дерева
        /// </summary>
        public static object GetSelectedObject(this TreeView treeView)
        {
            return GetRelatedObject(treeView.SelectedNode);
        }

        /// <summary>
        /// Обновить текст выбранного узла дерева в соответствии со строковым представленем его объекта
        /// </summary>
        public static void UpdateSelectedNodeText(this TreeView treeView)
        {
            object relatedObj = GetRelatedObject(treeView.SelectedNode);
            if (relatedObj != null)
                treeView.SelectedNode.Text = relatedObj.ToString();
        }

        /// <summary>
        /// Установить основное изображение узла и изображение в выбранном состоянии
        /// </summary>
        public static void SetImageKey(this TreeNode treeNode, string imageKey)
        {
            treeNode.ImageKey = treeNode.SelectedImageKey = imageKey;
        }


        /// <summary>
        /// Рекурсивный обход узлов дерева
        /// </summary>
        public static IEnumerable<TreeNode> IterateNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (TreeNode childNode in IterateNodes(node.Nodes))
                {
                    yield return childNode;
                }
            }
        }

        /// <summary>
        /// Рекурсивный обход узлов дерева
        /// </summary>
        public static IEnumerable<TreeNode> IterateNodes(TreeNode treeNode)
        {
            yield return treeNode;

            foreach (TreeNode childNode in IterateNodes(treeNode.Nodes))
            {
                yield return childNode;
            }
        }
    }
}
