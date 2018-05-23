/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : The base class for plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 */

using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utils;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// The base class for plugin specification
    /// <para>Родительский класс спецификации плагина</para>
    /// </summary>
    public abstract class PluginSpec
    {
        private AppDirs appDirs; // директории веб-приложения
        private ILog log;        // журнал


        /// <summary>
        /// Конструктор
        /// </summary>
        public PluginSpec()
        {
            appDirs = new AppDirs();
            log = new LogStub();
        }


        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Получить описание плагина
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// Получить спецификации представлений, которые реализуются плагином
        /// </summary>
        public virtual List<ViewSpec> ViewSpecs
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить спецификации отчётов, которые реализуются плагином
        /// </summary>
        public virtual List<ReportSpec> ReportSpecs
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить спецификации окон данных, которые реализуются плагином
        /// </summary>
        public virtual List<DataWndSpec> DataWndSpecs
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить пути к дополнительным скриптам, которые реализуются плагином
        /// </summary>
        public virtual ScriptPaths ScriptPaths
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить или установить директории веб-приложения
        /// </summary>
        public AppDirs AppDirs
        {
            get
            {
                return appDirs;
            }
            set
            {
                appDirs = value ?? throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// Получить или установить журнал
        /// </summary>
        public ILog Log
        {
            get
            {
                return log;
            }
            set
            {
                log = value ?? throw new ArgumentNullException("value");
            }
        }


        /// <summary>
        /// Инициализировать плагин
        /// </summary>
        /// <remarks>Метод вызывается один раз</remarks>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Выполнить действия после успешного входа пользователя в систему
        /// </summary>
        public virtual void OnUserLogin(UserData userData)
        {
        }

        /// <summary>
        /// Выполнить действия после выхода пользователя из системы
        /// </summary>
        public virtual void OnUserLogout(UserData userData)
        {
        }

        /// <summary>
        /// Получить элементы меню, доступные пользователю
        /// </summary>
        /// <remarks>Поддерживается не более 2 уровней вложенности меню</remarks>
        public virtual List<MenuItem> GetMenuItems(UserData userData)
        {
            return null;
        }


        /// <summary>
        /// Создать плагин, загрузив его из библиотеки
        /// </summary>
        public static PluginSpec CreateFromDll(string path, out string errMsg)
        {
            try
            {
                string fileName = Path.GetFileName(path);

                // загрузка сборки
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(path);
                }
                catch (Exception ex)
                {
                    errMsg = string.Format(Localization.UseRussian ?
                        "Ошибка при загрузке библиотеки плагина {0}:{1}{2}" :
                        "Error loading the plugin assembly {0}:{1}{2}", 
                        path, Environment.NewLine, ex.Message);
                    return null;
                }

                // получение типа из загруженной сборки
                Type type = null;
                string typeName = string.Format("Scada.Web.Plugins.{0}Spec",
                    Path.GetFileNameWithoutExtension(fileName));

                try
                {
                    type = assembly.GetType(typeName, true);
                }
                catch (Exception ex)
                {
                    errMsg = string.Format(Localization.UseRussian ?
                        "Не удалось получить тип плагина {0} из библиотеки {1}:{2}{3}" :
                        "Unable to get the plugin type {0} from the assembly {1}:{2}{3}",
                        typeName, path, Environment.NewLine, ex.Message);
                    return null;
                }

                try
                {
                    // создание экземпляра класса
                    PluginSpec pluginSpec = (PluginSpec)Activator.CreateInstance(type);
                    errMsg = "";
                    return pluginSpec;
                }
                catch (Exception ex)
                {
                    errMsg = string.Format(Localization.UseRussian ?
                        "Ошибка при создании экземпляра класса плагина {0} из библиотеки {1}:{2}{3}" :
                        "Error creating plugin class instance {0} from the assembly {1}:{2}{3}",
                        type, path, Environment.NewLine, ex.Message);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Localization.UseRussian ?
                    "Ошибка при создании плагина из библиотеки {0}:{1}{2}" :
                    "Error creating plugin from the assembly {0}:{1}{2}", 
                    path, Environment.NewLine, ex.Message);
                return null;
            }
        }
    }
}