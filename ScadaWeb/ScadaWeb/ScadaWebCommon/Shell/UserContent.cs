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
 * Module   : ScadaWebCommon
 * Summary  : Content accessible to the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
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
    /// Content accessible to the web application user.
    /// <para>Контент, доступный пользователю веб-приложения.</para>
    /// </summary>
    public class UserContent
    {
        /// <summary>
        /// Журнал.
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Словарь элементов отчётов, ключ - ид. отчёта.
        /// </summary>
        protected readonly Dictionary<int, ReportItem> reportItemDict;
        /// <summary>
        /// Словарь элементов окон данных, ключ - ид. окна данных.
        /// </summary>
        protected readonly Dictionary<int, DataWndItem> dataWndItemDict;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров.
        /// </summary>
        protected UserContent()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserContent(Log log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            reportItemDict = new Dictionary<int, ReportItem>();
            dataWndItemDict = new Dictionary<int, DataWndItem>();

            ReportItems = new List<ReportItem>();
            DataWndItems = new List<DataWndItem>();
        }


        /// <summary>
        /// Получить элементы отчётов, доступные пользователю.
        /// </summary>
        public List<ReportItem> ReportItems { get; protected set; }

        /// <summary>
        /// Получить элементы окон данных, доступные пользователю.
        /// </summary>
        public List<DataWndItem> DataWndItems { get; protected set; }


        /// <summary>
        /// Добавление контента, прописанного в базе конфигурации.
        /// </summary>
        protected void AddContentFromBase(UserRights userRights, Dictionary<string, UiObjSpec> uiObjSpecs, 
            DataAccess dataAccess)
        {
            if (userRights != null && uiObjSpecs != null)
            {
                List<UiObjProps> uiObjPropsList = dataAccess.GetUiObjPropsList(
                    UiObjProps.BaseUiTypes.Report | UiObjProps.BaseUiTypes.DataWnd);

                foreach (UiObjProps uiObjProps in uiObjPropsList)
                {
                    int uiObjID = uiObjProps.UiObjID;

                    if (!uiObjProps.Hidden && userRights.GetUiObjRights(uiObjID).ViewRight)
                    {
                        uiObjSpecs.TryGetValue(uiObjProps.TypeCode, out UiObjSpec uiObjSpec);

                        if (uiObjProps.BaseUiType == UiObjProps.BaseUiTypes.Report)
                        {
                            // добавление элемента отчёта
                            ReportItem reportItem = new ReportItem()
                            {
                                UiObjID = uiObjID,
                                Text = uiObjProps.Title,
                                Path = uiObjProps.Path
                            };

                            if (uiObjSpec is ReportSpec reportSpec)
                            {
                                if (string.IsNullOrEmpty(reportItem.Text))
                                    reportItem.Text = reportSpec.Name;
                                reportItem.Url = uiObjSpec.GetUrl(uiObjID);
                                reportItem.ReportSpec = reportSpec;
                            }

                            if (!string.IsNullOrEmpty(reportItem.Text))
                            {
                                ReportItems.Add(reportItem);
                                reportItemDict[uiObjID] = reportItem;
                            }
                        }
                        else if (uiObjProps.BaseUiType == UiObjProps.BaseUiTypes.DataWnd)
                        {
                            // добавление элемента окна данных
                            DataWndItem dataWndItem = new DataWndItem()
                            {
                                UiObjID = uiObjID,
                                Text = uiObjProps.Title,
                                Path = uiObjProps.Path
                            };

                            if (uiObjSpec is DataWndSpec dataWndSpec)
                            {
                                if (string.IsNullOrEmpty(dataWndItem.Text))
                                    dataWndItem.Text = dataWndSpec.Name;
                                dataWndItem.Url = uiObjSpec.GetUrl(uiObjID);
                                dataWndItem.DataWndSpec = dataWndSpec;
                            }

                            if (!string.IsNullOrEmpty(dataWndItem.Text))
                            {
                                DataWndItems.Add(dataWndItem);
                                dataWndItemDict[uiObjID] = dataWndItem;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавление контента, доступного всем, который задаётся спецификациями плагинов.
        /// </summary>
        protected void AddContentFromPlugins(List<PluginSpec> pluginSpecs)
        {
            if (pluginSpecs != null)
            {
                foreach (PluginSpec pluginSpec in pluginSpecs)
                {
                    // добавление общедоступных элементов отчётов
                    if (pluginSpec.ReportSpecs != null)
                    {
                        foreach (ReportSpec reportSpec in pluginSpec.ReportSpecs)
                        {
                            if (reportSpec.ForEveryone)
                            {
                                ReportItems.Add(new ReportItem()
                                {
                                    Text = reportSpec.Name,
                                    Url = reportSpec.Url,
                                    ReportSpec = reportSpec
                                });
                            }
                        }
                    }

                    // добавление общедоступных элементов окон данных
                    if (pluginSpec.DataWndSpecs != null)
                    {
                        foreach (DataWndSpec dataWndSpec in pluginSpec.DataWndSpecs)
                        {
                            if (dataWndSpec.ForEveryone)
                            {
                                DataWndItems.Add(new DataWndItem()
                                {
                                    Text = dataWndSpec.Name,
                                    Url = dataWndSpec.Url,
                                    DataWndSpec = dataWndSpec
                                });
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Инициализировать доступный контент пользователя.
        /// </summary>
        public void Init(UserData userData, DataAccess dataAccess)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                reportItemDict.Clear();
                dataWndItemDict.Clear();
                ReportItems.Clear();
                DataWndItems.Clear();

                AddContentFromBase(userData.UserRights, userData.UiObjSpecs, dataAccess);
                AddContentFromPlugins(userData.PluginSpecs);

                ReportItems.Sort();
                DataWndItems.Sort();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации доступного контента пользователя" :
                    "Error initializing accessible user content");
            }
        }

        /// <summary>
        /// Получить элемент отчёта по идентификатору.
        /// </summary>
        public ReportItem GetReportItem(int reportID)
        {
            return reportItemDict.TryGetValue(reportID, out ReportItem reportItem) ? reportItem : null;
        }

        /// <summary>
        /// Получить элемент отчёта, имеющий спецификацию заданного типа, по идентификатору.
        /// </summary>
        public ReportItem GetReportItem(int reportID, Type specType, bool throwOnFail = false)
        {
            ReportItem reportItem = GetReportItem(reportID);

            if (reportItem == null)
            {
                if (throwOnFail)
                    throw new ScadaException(string.Format(WebPhrases.ReportNotFound, reportID));
                else
                    return null;
            }
            else if (reportItem.ReportSpec == null || reportItem.ReportSpec.GetType() != specType)
            {
                if (throwOnFail)
                    throw new ScadaException(string.Format(WebPhrases.UnexpectedReportSpec, reportID));
                else
                    return null;
            }
            else
            {
                return reportItem;
            }
        }

        /// <summary>
        /// Получить элемент окна данных по идентификатору.
        /// </summary>
        public DataWndItem GetDataWndItem(int dataWndID)
        {
            return dataWndItemDict.TryGetValue(dataWndID, out DataWndItem dataWndItem) ? dataWndItem : null;
        }

        /// <summary>
        /// Получить элемент окна данных, имеющий спецификацию заданного типа, по идентификатору.
        /// </summary>
        public DataWndItem GetDataWndItem(int dataWndID, Type specType, bool throwOnFail = false)
        {
            DataWndItem dataWndItem = GetDataWndItem(dataWndID);

            if (dataWndItem == null)
            {
                if (throwOnFail)
                    throw new ScadaException(string.Format(WebPhrases.DataWndNotFound, dataWndID));
                else
                    return null;
            }
            else if (dataWndItem.DataWndSpec == null || dataWndItem.DataWndSpec.GetType() != specType)
            {
                if (throwOnFail)
                    throw new ScadaException(string.Format(WebPhrases.UnexpectedDataWndSpec, dataWndID));
                else
                    return null;
            }
            else
            {
                return dataWndItem;
            }
        }
    }
}
