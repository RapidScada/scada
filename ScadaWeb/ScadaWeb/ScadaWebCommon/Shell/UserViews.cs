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
 * Summary  : Views accessible to the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Views accessible to the web application user.
    /// <para>Представления, доступные пользователю веб-приложения.</para>
    /// </summary>
    public class UserViews
    {
        /// <summary>
        /// Контекст для получения данных
        /// </summary>
        protected class DataContext
        {
            /// <summary>
            /// Права пользователя
            /// </summary>
            public UserRights UserRights;
            /// <summary>
            /// Спецификации объектов пользовательского интерфейса
            /// </summary>
            public Dictionary<string, UiObjSpec> UiObjSpecs;
            /// <summary>
            /// Объект для доступа к данным кэша клиентов
            /// </summary>
            public DataAccess DataAccess;
        }

        /// <summary>
        /// Кэш представлений
        /// </summary>
        protected readonly ViewCache viewCache;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Словарь узлов дерева представлений, ключ - ид. представления
        /// </summary>
        protected readonly Dictionary<int, ViewNode> viewNodeDict;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserViews()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserViews(ViewCache viewCache, Log log)
        {
            this.viewCache = viewCache ?? throw new ArgumentNullException("viewCache");
            this.log = log ?? throw new ArgumentNullException("log");
            viewNodeDict = new Dictionary<int, ViewNode>();

            ViewNodes = new List<ViewNode>();           
        }


        /// <summary>
        /// Получить узлы дерева представлений, доступные пользователю
        /// </summary>
        public List<ViewNode> ViewNodes { get; protected set; }


        /// <summary>
        /// Рекурсивно создать узлы дерева представлений на основе элементов настроек
        /// </summary>
        protected void CreateViewNodes(List<ViewNode> destViewNodes, List<ViewSettings.ViewItem> srcViewItems, 
            int level, DataContext dataContext)
        {
            foreach (ViewSettings.ViewItem viewItem in srcViewItems)
            {
                // пропуск представления, на которое нет прав
                int viewID = viewItem.ViewID;
                if (viewID > 0 && !dataContext.UserRights.GetUiObjRights(viewID).ViewRight)
                    continue;

                // получение спецификации представления
                ViewSpec viewSpec = null;
                if (viewID > 0)
                {
                    UiObjProps viewProps = dataContext.DataAccess.GetUiObjProps(viewID);
                    if (viewProps != null &&
                        dataContext.UiObjSpecs.TryGetValue(viewProps.TypeCode, out UiObjSpec uiObjSpec))
                    {
                        viewSpec = uiObjSpec as ViewSpec;
                    }
                }

                // создание узла дерева и дочерних узлов
                ViewNode viewNode = new ViewNode(viewItem, viewSpec) { Level = level };
                CreateViewNodes(viewNode.ChildNodes, viewItem.Subitems, level + 1, dataContext);

                // добавление узла, если он соответствует представлению или имеет дочерние узлы
                if (viewID > 0 || viewNode.ChildNodes.Count > 0)
                {
                    destViewNodes.Add(viewNode);
                    viewNodeDict[viewID] = viewNode;
                }
            }
        }

        /// <summary>
        /// Рекурсивно найти узел дерева с непустым представлением
        /// </summary>
        protected ViewNode FindNonEmptyViewNode(List<ViewNode> viewNodes)
        {
            if (viewNodes != null)
            {
                foreach (ViewNode viewNode in viewNodes)
                {
                    if (viewNode.IsEmpty)
                    {
                        ViewNode node = FindNonEmptyViewNode(viewNode.ChildNodes);
                        if (node != null)
                            return node;
                    }
                    else
                    {
                        return viewNode;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получить ссылку на тип представления с заданным идентификатором
        /// </summary>
        protected Type GetViewType(int viewID)
        {
            return viewNodeDict.TryGetValue(viewID, out ViewNode viewNode) && viewNode.ViewSpec != null ?
                viewNode.ViewSpec.ViewType : null;
        }

        /// <summary>
        /// Рекурсивно слить узлы дерева в линейный список
        /// </summary>
        protected void LinearizeViewNodes(List<ViewNode> destViewNodes, List<ViewNode> addedViewNodes)
        {
            if (addedViewNodes != null)
            {
                foreach (ViewNode viewNode in addedViewNodes)
                {
                    destViewNodes.Add(viewNode);
                    LinearizeViewNodes(destViewNodes, viewNode.ChildNodes);
                }
            }
        }


        /// <summary>
        /// Инициализировать доступные представления пользователя
        /// </summary>
        public void Init(UserData userData, DataAccess dataAccess)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                viewNodeDict.Clear();
                ViewNodes.Clear();

                if (userData.UserRights != null && userData.UiObjSpecs != null)
                {
                    DataContext dataContext = new DataContext()
                    {
                        UserRights = userData.UserRights,
                        UiObjSpecs = userData.UiObjSpecs,
                        DataAccess = dataAccess
                    };

                    CreateViewNodes(ViewNodes, userData.ViewSettings.ViewItems, 0, dataContext);
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации доступных представлений пользователя" :
                    "Error initializing accessible user views");
            }
        }

        /// <summary>
        /// Получить узел дерева представлений по идентификатору
        /// </summary>
        public ViewNode GetViewNode(int viewID)
        {
            return viewNodeDict.TryGetValue(viewID, out ViewNode viewNode) ? viewNode : null;
        }

        /// <summary>
        /// Получить первый доступный узел дерева представлений
        /// </summary>
        public ViewNode GetFirstViewNode()
        {
            return FindNonEmptyViewNode(ViewNodes);
        }

        /// <summary>
        /// Получить представление по идентификатору, если тип представления заранее не известен
        /// </summary>
        /// <remarks>Если тип представления известен, эффективнее использовать ViewCache.GetView</remarks>
        public BaseView GetView(int viewID, bool throwOnError = false, bool allowUndefView = false)
        {
            if (viewID <= 0 && allowUndefView)
                return null;

            Type viewType = GetViewType(viewID);
            return viewCache.GetView(viewType, viewID, throwOnError);
        }

        /// <summary>
        /// Получить линейный список узлов дерева представлений
        /// </summary>
        public List<ViewNode> GetLinearViewNodes()
        {
            List<ViewNode> linearViewNodes = new List<ViewNode>();
            LinearizeViewNodes(linearViewNodes, ViewNodes);
            return linearViewNodes;
        }
    }
}