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
using System.IO;
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
        /// <summary>
        /// Маска для поиска файлов библиотек компонентов
        /// </summary>
        private const string CompLibMask = "plgsch*comp.dll";

        private static readonly CompManager instance; // экземпляр объекта менеджера

        private Web.AppDirs webAppDirs; // директории веб-приложения
        private ILog log;               // журнал приложения
        private Dictionary<string, CompFactory> compFactories; // фабрики компонентов, ключ - XML-префикс


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
        /// Добавить фабрику компонентов плагина в словарь
        /// </summary>
        private void AddCompFactory(ISchemeComp schemeComp)
        {
            CompLibSpec compLibSpec = schemeComp.CompLibSpec;

            if (!string.IsNullOrEmpty(compLibSpec.XmlPrefix))
                compFactories[compLibSpec.XmlPrefix] = compLibSpec.CompFactory;
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
                compFactories.Clear();

                DirectoryInfo dirInfo = new DirectoryInfo(webAppDirs.BinDir);
                FileInfo[] fileInfoArr = dirInfo.GetFiles(CompLibMask, SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    string errMsg;
                    PluginSpec pluginSpec = PluginSpec.CreateFromDll(fileInfo.FullName, out errMsg);

                    if (pluginSpec == null)
                    {
                        log.WriteError(errMsg);
                    }
                    else if (pluginSpec is ISchemeComp)
                    {
                        AddCompFactory((ISchemeComp)pluginSpec);
                    }
                    else
                    {
                        log.WriteError(string.Format(Localization.UseRussian ? 
                            "Библиотека {0} не предоставляет компоненты схем" : 
                            "The assembly {0} does not provide scheme components", fileInfo.FullName));
                    }
                }
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
                compFactories.Clear();

                if (pluginSpecs != null)
                {
                    foreach (PluginSpec pluginSpec in pluginSpecs)
                    {
                        if (pluginSpec is ISchemeComp)
                            AddCompFactory((ISchemeComp)pluginSpec);
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

            try
            {
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
            }
            catch (Exception ex)
            {
                log.WriteException(ex, string.Format(Localization.UseRussian ?
                    "Ошибка при создании компонента на основе XML-узла \"{0}\"" :
                    "Error creating component based on the XML node \"{0}\"", compNode.Name));
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
