/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Manages scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using System.Xml;
using Utils;

namespace Scada.Scheme
{
    /// <summary>
    /// Manages scheme components
    /// <para>Менеджер, управляющий компонентами схемы</para>
    /// </summary>
    public sealed class CompManager
    {
        private static readonly CompManager instance; // экземпляр объекта менеджера

        private Web.AppDirs webAppDirs; // директории веб-приложения
        private ILog log;               // журнал приложения
        private Dictionary<string, CompFactory> compFactories; // фабрики компонентов


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static CompManager()
        {
            instance = new CompManager();
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта из других классов
        /// </summary>
        private CompManager()
        {
            webAppDirs = new Web.AppDirs();
            log = new LogStub();
            compFactories = new Dictionary<string, CompFactory>();
        }


        /// <summary>
        /// Инициализировать менеджер компонентов
        /// </summary>
        public void Init(string webAppDir, Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            webAppDirs.Init(webAppDir);
            this.log = log;
        }

        /// <summary>
        /// Инициализировать менеджер компонентов
        /// </summary>
        public void Init(Web.AppDirs webAppDirs, Log log)
        {
            if (webAppDirs == null)
                throw new ArgumentNullException("webAppDirs");
            if (log == null)
                throw new ArgumentNullException("log");

            this.webAppDirs = webAppDirs;
            this.log = log;
        }

        /// <summary>
        /// Загрузить компоненты из файлов
        /// </summary>
        public void LoadCompFromFiles()
        {
            try
            {
                log.WriteAction(Localization.UseRussian ?
                    "Загрузка компонентов из файлов" :
                    "Load components from files");
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при загрузке компонентов из файлов" :
                    "Error loading components from files");
            }
        }

        /// <summary>
        /// Извлечь компоненты из загруженных плагинов
        /// </summary>
        public void RetrieveCompFromPlugins(List<PluginSpec> pluginSpecs)
        {
            try
            {
                log.WriteAction(Localization.UseRussian ?
                    "Извлечение компонентов из установленных плагинов" :
                    "Retrieve components from the installed plugins");

                if (pluginSpecs != null)
                {
                    foreach (PluginSpec pluginSpec in pluginSpecs)
                    {
                        if (pluginSpec is ISchemeComp)
                        {
                            ISchemeComp schemeComp = (ISchemeComp)pluginSpec;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при извлечении компонентов из установленных плагинов" :
                    "Error retrieving components from the installed plugins");
            }
        }

        /// <summary>
        /// Создать компонент на основе XML-узла
        /// </summary>
        public BaseComponent CreateComponent(XmlNode compNode)
        {
            if (compNode == null)
                throw new ArgumentNullException("compNode");

            string xmlPrefix = compNode.Prefix;
            string nodeName = compNode.Name.ToLowerInvariant();
            CompFactory compFactory;

            if (string.IsNullOrEmpty(xmlPrefix))
            {
                if (nodeName == "statictext")
                    return new StaticText();
                else if (nodeName == "dynamictext")
                    return new DynamicText();
                else if (nodeName == "staticpicture")
                    return new StaticPicture();
                else if (nodeName == "dynamicpicture")
                    return new DynamicPicture();
            }
            else if (compFactories.TryGetValue(xmlPrefix, out compFactory))
            {
                return compFactory.CreateComponent(nodeName);
            }

            return null;
        }

        /// <summary>
        /// Получить единственный экземпляр менеджера
        /// </summary>
        public static CompManager GetInstance()
        {
            return instance;
        }
    }
}
