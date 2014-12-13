/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Application level data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Scada.Web;
using Utils;

namespace Scada.Scheme
{
    /// <summary>
    /// Application level data
    /// <para>Данные уровня приложения</para>
    /// </summary>
    public class SchemeApp
    {
        /// <summary>
        /// Режимы работы приложения
        /// </summary>
        public enum WorkModes
        {
            /// <summary>
            /// Мониторинг
            /// </summary>
            Monitor,
            /// <summary>
            /// Редактирование
            /// </summary>
            Edit
        }


        private static SchemeApp schemeAppObj; // объект для хранения данных уровня приложения


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static SchemeApp()
        {
            schemeAppObj = null;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        private SchemeApp()
        {
            WorkMode = WorkModes.Monitor;
            SchemeSettings = new SchemeSettings();
            MainData = null;
            EditorData = null;
            Log = new Log(Log.Formats.Full);
        }


        /// <summary>
        /// Получить режим работы приложения
        /// </summary>
        public WorkModes WorkMode { get; private set; }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public SchemeSettings SchemeSettings { get; private set; }

        /// <summary>
        /// Получить объект для работы с данными системы
        /// </summary>
        public MainData MainData { get; private set; }

        /// <summary>
        /// Получить или установить данные редактора схем
        /// </summary>
        public EditorData EditorData { get; set; }

        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }

        
        /// <summary>
        /// Инициализировать данные уровня приложения
        /// </summary>
        public static SchemeApp InitSchemeApp(WorkModes workMode)
        {
            SchemeApp schemeApp;

            if (schemeAppObj == null)
            {
                schemeApp = new SchemeApp();
                schemeAppObj = schemeApp;
                schemeApp.WorkMode = workMode;

                if (workMode == WorkModes.Monitor)
                {
                    // инициализация общих данных приложения SCADA-Web
                    AppData.InitAppData();
                    schemeApp.MainData = AppData.MainData;

                    // настройка журнала приложения
                    schemeApp.Log.FileName = AppData.LogDir + "ScadaScheme.log";
                    schemeApp.Log.Encoding = Encoding.UTF8;
                    schemeApp.Log.WriteBreak();
                    schemeApp.Log.WriteAction(Localization.UseRussian ? "Запуск SCADA-Схемы" : 
                        "Start SCADA-Scheme", Log.ActTypes.Action);
                }
                else // WorkModes.Edit
                {
                    // инициализация данных редактора схем
                    schemeApp.EditorData = new EditorData();

                    // настройка журнала приложения
                    schemeApp.Log.FileName = ScadaUtils.NormalDir(Path.GetDirectoryName(Application.ExecutablePath)) + 
                        "log\\ScadaSchemeEditor.log";
                    schemeApp.Log.Encoding = Encoding.UTF8;
                    schemeApp.Log.WriteBreak();
                    schemeApp.Log.WriteAction(Localization.UseRussian ? "Запуск SCADA-Редактора схем" :
                        "Start SCADA-Scheme Editor", Log.ActTypes.Action);
                }
            }
            else if (schemeAppObj.WorkMode != workMode)
            {
                throw new ArgumentException(Localization.UseRussian ?
                    "Ошибка при инициализации данных приложения SCADA-Схема: некорректный режим работы." : 
                    "Error initializing SCADA-Scheme application data: incorrect work mode.", "workMode");
            }
            else
            {
                schemeApp = schemeAppObj;
            }

            if (workMode == WorkModes.Monitor)
            {
                // получение настроек Silverlight-приложения из настроек SCADA-Web
                schemeApp.SchemeSettings.RefrFreq = AppData.WebSettings.SrezRefrFreq;
                schemeApp.SchemeSettings.CmdEnabled = AppData.WebSettings.CmdEnabled;
            }

            // инициализировать используемые фразы
            schemeApp.SchemeSettings.SchemePhrases.Init();
            SchemePhrases.InitStatic();

            return schemeApp;
        }

        /// <summary>
        /// Получить данные уровня приложения, создав их при необходимости
        /// </summary>
        public static SchemeApp GetSchemeApp()
        {
            if (schemeAppObj == null)
                throw new Exception(Localization.UseRussian ? 
                    "Ошибка при получении данных приложения SCADA-Схема: данные не инициализированы." :
                    "Error getting SCADA-Scheme application data: data is not initialized.");
            else
                return schemeAppObj;
        }
    }
}
