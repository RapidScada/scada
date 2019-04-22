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
 * Module   : ScadaSchemeCommon
 * Summary  : Manages scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Scheme.Model;
using Scada.Scheme.Model.PropertyGrid;
using Scada.Web;
using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Utils;

namespace Scada.Scheme
{
    /// <summary>
    /// Manages scheme components.
    /// <para>Менеджер, управляющий компонентами схемы.</para>
    /// </summary>
    public sealed class CompManager
    {
        /// <summary>
        /// Маска для поиска файлов библиотек компонентов.
        /// </summary>
        private const string CompLibMask = "PlgSch*Comp.dll";
        /// <summary>
        /// Стандартные типы компонентов, ключ - полное имя типа.
        /// </summary>
        private static readonly Dictionary<string, Type> StandardCompTypes;
        /// <summary>
        /// Экземпляр объекта менеджера.
        /// </summary>
        private static readonly CompManager instance;

        private readonly object syncLock;   // объект для синхронизации доступа
        private AppDirs appDirs;            // директории веб-приложения
        private ILog log;                   // журнал приложения
        private List<CompLibSpec> allSpecs; // все спецификации библиотек
        private Dictionary<string, CompFactory> factsByPrefix; // фабрики компонентов, ключ - XML-префикс
        private Dictionary<string, CompLibSpec> specsByType;   // спецификации библиотек, ключ - имя типа компонента


        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static CompManager()
        {
            StandardCompTypes = new Dictionary<string, Type>()
            {
                { typeof(StaticText).FullName, typeof(StaticText) },
                { typeof(DynamicText).FullName, typeof(DynamicText) },
                { typeof(StaticPicture).FullName, typeof(StaticPicture) },
                { typeof(DynamicPicture).FullName, typeof(DynamicPicture) }
            };

            instance = new CompManager();
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта из других классов.
        /// </summary>
        private CompManager()
        {
            syncLock = new object();
            appDirs = null;
            log = new LogStub();
            allSpecs = new List<CompLibSpec>();
            factsByPrefix = new Dictionary<string, CompFactory>();
            specsByType = new Dictionary<string, CompLibSpec>();
            LoadErrors = new List<string>();
        }


        /// <summary>
        /// Получить ошибки при загрузке библиотек.
        /// </summary>
        public List<string> LoadErrors { get; private set; }


        /// <summary>
        /// Очистить словари
        /// </summary>
        private void ClearDicts()
        {
            allSpecs.Clear();
            factsByPrefix.Clear();
            specsByType.Clear();
            LoadErrors.Clear();
        }

        /// <summary>
        /// Добавить компоненты в словари.
        /// </summary>
        private bool AddComponents(ISchemeComp schemeComp, AttrTranslator attrTranslator)
        {
            CompLibSpec compLibSpec = schemeComp.CompLibSpec;

            if (compLibSpec != null)
            {
                if (compLibSpec.Validate(out string errMsg) && compLibSpec.CompFactory != null)
                {
                    allSpecs.Add(compLibSpec);
                    factsByPrefix[compLibSpec.XmlPrefix] = compLibSpec.CompFactory;

                    foreach (CompItem compItem in compLibSpec.CompItems)
                    {
                        if (compItem != null && compItem.CompType != null)
                        {
                            specsByType[compItem.CompType.FullName] = compLibSpec;
                            if (attrTranslator != null)
                                attrTranslator.TranslateAttrs(compItem.CompType);
                        }
                    }

                    return true;
                }
                else if (!string.IsNullOrEmpty(errMsg))
                {
                    LoadErrors.Add(errMsg);
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить, что тип компонента относится к стандартным типам.
        /// </summary>
        private bool TypeIsStrandard(Type compType)
        {
            return compType == typeof(StaticText) || compType == typeof(DynamicText) || 
                compType == typeof(StaticPicture) || compType == typeof(DynamicPicture);
        }


        /// <summary>
        /// Инициализировать менеджер компонентов.
        /// </summary>
        public void Init(AppDirs appDirs, ILog log)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.log = log ?? throw new ArgumentNullException("log");
        }

        /// <summary>
        /// Загрузить компоненты из файлов.
        /// </summary>
        public void LoadCompFromFiles()
        {
            try
            {
                lock (syncLock)
                {
                    log.WriteAction(Localization.UseRussian ?
                    "Загрузка компонентов из файлов" :
                    "Load components from files");

                    ClearDicts();
                    AttrTranslator attrTranslator = new AttrTranslator();
                    DirectoryInfo dirInfo = new DirectoryInfo(appDirs.BinDir);
                    FileInfo[] fileInfoArr = dirInfo.GetFiles(CompLibMask, SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in fileInfoArr)
                    {
                        string fileName = fileInfo.FullName;
                        PluginSpec pluginSpec = PluginSpec.CreateFromDll(fileName, out string errMsg);

                        if (pluginSpec == null)
                        {
                            log.WriteError(errMsg);
                        }
                        else if (pluginSpec is ISchemeComp)
                        {
                            pluginSpec.AppDirs = appDirs;
                            pluginSpec.Log = log;
                            pluginSpec.Init();

                            if (AddComponents((ISchemeComp)pluginSpec, attrTranslator))
                            {
                                log.WriteAction(string.Format(Localization.UseRussian ?
                                    "Загружены компоненты из файла {0}" :
                                    "Components are loaded from the file {0}", fileName));
                            }
                            else
                            {
                                log.WriteAction(string.Format(Localization.UseRussian ?
                                    "Не удалось загрузить компоненты из файла {0}" :
                                    "Unable to load components from the file {0}", fileName));
                            }
                        }
                        else
                        {
                            log.WriteError(string.Format(Localization.UseRussian ?
                                "Библиотека {0} не предоставляет компоненты схем" :
                                "The assembly {0} does not provide scheme components", fileName));
                        }
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
        /// Извлечь компоненты из загруженных плагинов.
        /// </summary>
        public void RetrieveCompFromPlugins(List<PluginSpec> pluginSpecs)
        {
            try
            {
                lock (syncLock)
                {
                    log.WriteAction(Localization.UseRussian ?
                    "Извлечение компонентов из установленных плагинов" :
                    "Retrieve components from the installed plugins");
                    ClearDicts();

                    if (pluginSpecs != null)
                    {
                        foreach (PluginSpec pluginSpec in pluginSpecs)
                        {
                            if (pluginSpec is ISchemeComp)
                                AddComponents((ISchemeComp)pluginSpec, null);
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
        /// Создать компонент на основе XML-узла.
        /// </summary>
        public BaseComponent CreateComponent(XmlNode compNode, out string errMsg)
        {
            if (compNode == null)
                throw new ArgumentNullException("compNode");

            errMsg = "";
            string nodeName = compNode.Name;

            try
            {
                lock (syncLock)
                {
                    string xmlPrefix = compNode.Prefix;
                    string localName = compNode.LocalName.ToLowerInvariant();

                    if (string.IsNullOrEmpty(xmlPrefix))
                    {
                        if (localName == "statictext")
                            return new StaticText();
                        else if (localName == "dynamictext")
                            return new DynamicText();
                        else if (localName == "staticpicture")
                            return new StaticPicture();
                        else if (localName == "dynamicpicture")
                            return new DynamicPicture();
                        else
                            errMsg = string.Format(SchemePhrases.UnknownComponent, nodeName);
                    }
                    else if (factsByPrefix.TryGetValue(xmlPrefix, out CompFactory compFactory))
                    {
                        BaseComponent comp = compFactory.CreateComponent(localName, true);
                        if (comp == null)
                            errMsg = string.Format(SchemePhrases.UnableCreateComponent, nodeName);
                        return comp;
                    }
                    else
                    {
                        errMsg = string.Format(SchemePhrases.CompLibraryNotFound, nodeName);
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format(SchemePhrases.ErrorCreatingComponent, nodeName);
                log.WriteException(ex, string.Format(Localization.UseRussian ?
                    "Ошибка при создании компонента на основе XML-узла \"{0}\"" :
                    "Error creating component based on the XML node \"{0}\"", compNode.Name));
            }

            return null;
        }

        /// <summary>
        /// Создать компонент заданного типа.
        /// </summary>
        public BaseComponent CreateComponent(string compTypeName)
        {
            try
            {
                lock (syncLock)
                {
                    if (StandardCompTypes.TryGetValue(compTypeName, out Type compType))
                    {
                        return (BaseComponent)Activator.CreateInstance(compType);
                    }
                    else if (specsByType.TryGetValue(compTypeName, out CompLibSpec compLibSpec))
                    {
                        BaseComponent comp = compLibSpec.CompFactory.CreateComponent(compTypeName, false);

                        if (comp == null)
                        {
                            throw new ScadaException(string.Format(Localization.UseRussian ?
                                "Фабрика компонентов вернула пустой результат." :
                                "Component factory returned an empty result."));
                        }

                        return comp;
                    }
                    else
                    {
                        throw new ScadaException(string.Format(Localization.UseRussian ?
                            "Неизвестный тип компонента." :
                            "Unknown component type."));
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при создании компонента типа \"{0}\"" :
                    "Error creating component of the type \"{0}\"", compTypeName);

                if (ex is ScadaException)
                    log.WriteError(errMsg + ": " + ex.Message);
                else 
                    log.WriteException(ex, errMsg);

                return null;
            }
        }

        /// <summary>
        /// Получить спецификацию библиотеки по типу компонента.
        /// </summary>
        public CompLibSpec GetSpecByType(Type compType)
        {
            if (compType == null)
                throw new ArgumentNullException("compType");

            if (TypeIsStrandard(compType))
            {
                return null;
            }
            else
            {
                lock (syncLock)
                {
                    specsByType.TryGetValue(compType.FullName, out CompLibSpec compLibSpec);
                    return compLibSpec;
                }
            }
        }

        /// <summary>
        /// Получить спецификации библиотек, отсортированные по заголовкам групп.
        /// </summary>
        public CompLibSpec[] GetSortedSpecs()
        {
            lock (syncLock)
            {
                int specCnt = allSpecs.Count;
                string[] headers = new string[specCnt];
                CompLibSpec[] specs = new CompLibSpec[specCnt];

                for (int i = 0; i < specCnt; i++)
                {
                    CompLibSpec spec = allSpecs[i];
                    headers[i] = spec.GroupHeader;
                    specs[i] = spec;
                }

                Array.Sort(headers, specs);
                return specs;
            }
        }

        /// <summary>
        /// Получить объединённый список стилей компонентов.
        /// </summary>
        public List<string> GetAllStyles()
        {
            lock (syncLock)
            {
                List<string> allStyles = new List<string>();

                foreach (CompLibSpec spec in allSpecs)
                {
                    allStyles.AddRange(spec.Styles);
                }

                return allStyles;
            }
        }

        /// <summary>
        /// Получить объединённый список скриптов компонентов.
        /// </summary>
        public List<string> GetAllScripts()
        {
            lock (syncLock)
            {
                List<string> allScripts = new List<string>();

                foreach (CompLibSpec spec in allSpecs)
                {
                    allScripts.AddRange(spec.Scripts);
                }

                return allScripts;
            }
        }


        /// <summary>
        /// Получить единственный экземпляр менеджера.
        /// </summary>
        public static CompManager GetInstance()
        {
            return instance;
        }
    }
}
