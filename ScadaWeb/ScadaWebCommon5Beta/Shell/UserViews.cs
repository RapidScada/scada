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
 * Summary  : Views accessible to the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data;
using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Views accessible to the web application user
    /// <para>Представления, доступные пользователю веб-приложения</para>
    /// </summary>
    public class UserViews
    {
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;

        /// <summary>
        /// Права пользователя
        /// </summary>
        protected UserRights userRights;
        /// <summary>
        /// Спецификации представлений
        /// </summary>
        protected Dictionary<string, ViewSpec> viewSpecs;
        /// <summary>
        /// Объект для доступа к данным кеша клиентов
        /// </summary>
        protected DataAccess dataAccess;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserViews()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserViews(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
            userRights = null;
            viewSpecs = null;
            dataAccess = null;

            ViewNodes = new List<ViewNode>();
        }


        /// <summary>
        /// Получить узлы дерева представлений
        /// </summary>
        public List<ViewNode> ViewNodes { get; protected set; }


        /// <summary>
        /// Рекурсивно создать узлы дерева представлений на основе элементов настроек
        /// </summary>
        protected void CreateViewNodes(List<ViewNode> destViewNodes, List<ViewSettings.ViewItem> srcViewItems, int level)
        {
            foreach (ViewSettings.ViewItem viewItem in srcViewItems)
            {
                int viewID = viewItem.ViewID;

                // пропуск представления, на которое нет прав
                if (viewID > 0 && !userRights.GetViewRights(viewItem.ViewID).ViewRight)
                    continue;

                // получение спецификации представления
                ViewSpec viewSpec = null;
                if (viewID > 0)
                {
                    ViewProps viewProps = dataAccess.GetViewProps(viewID);
                    if (viewProps != null)
                        viewSpecs.TryGetValue(viewProps.ViewTypeCode, out viewSpec);
                }

                // создание узла дерева и дочерних узлов
                ViewNode viewNode = new ViewNode(viewItem, viewSpec) { Level = level };
                CreateViewNodes(viewNode.ChildNodes, viewItem.Subitems, level + 1);

                // добавление узла, если он соответствует представлению или имеет дочерние узлы
                if (viewID > 0 || viewNode.ChildNodes.Count > 0)
                    destViewNodes.Add(viewNode);
            }
        }

        /// <summary>
        /// Инициализировать представления пользователя
        /// </summary>
        public void Init(UserData userData, DataAccess dataAccess)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                userRights = userData.UserRights;
                viewSpecs = userData.ViewSpecs;
                this.dataAccess = dataAccess;

                if (userRights != null && viewSpecs != null)
                    CreateViewNodes(ViewNodes, userData.ViewSettings.ViewItems, 0);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации представлений пользователя" :
                    "Error initializing user views");
            }
        }
    }
}