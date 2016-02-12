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
 * Summary  : Common data of the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data;
using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Common data of the web application
    /// <para>Общие данные веб-приложения</para>
    /// </summary>
    public static class AppData
	{
        /// <summary>
        /// Имя файла журнала приложения без директории
        /// </summary>
        public const string LogFileName = "ScadaWeb.log";

        private static readonly object appDataLock; // объект для синхронизации доступа к данным приложения

        private static bool inited;                 // признак инициализации общих данных веб-приложения
        private static CommSettings commSettings;   // настройки соединения с сервером
        private static ServerComm serverComm;       // объект для обмена данными с сервером

        private static DateTime scadaDataDictAge;   // время изменения файла словаря ScadaData
        private static DateTime scadaWebDictAge;    // время изменения файла словаря ScadaWeb
        private static DateTime commSettingsAge;    // время изменения файла настроек соединения с сервером
        private static DateTime webSettingsAge;     // время изменения файла настроек веб-приложения


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static AppData()
		{
            appDataLock = new object();

            inited = false;
            commSettings = new CommSettings();
            serverComm = null; // создаётся в CreateDataObjects()

            scadaDataDictAge = DateTime.MinValue;
            scadaWebDictAge = DateTime.MinValue;
            commSettingsAge = DateTime.MinValue;
            webSettingsAge = DateTime.MinValue;

            WebSettings = new WebSettings();
            PluginSpecs = new List<PluginSpec>();
            AppDirs = new AppDirs();
            Log = new Log(Log.Formats.Full);

            CreateDataObjects();
        }


        /// <summary>
        /// Получить настройки веб-приложения
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal static WebSettings WebSettings { get; private set; }

        /// <summary>
        /// Получить список плагинов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal static List<PluginSpec> PluginSpecs { get; private set; }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public static AppDirs AppDirs { get; private set; }
        
        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public static Log Log { get; private set; }

        /// <summary>
        /// Получить объект для потокобезопасного доступа к данным кеша клиентов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public static DataAccess DataAccess { get; private set; }

        /// <summary>
        /// Получить кэш представлений
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public static ViewCache ViewCache { get; private set; }


        /// <summary>
        /// Обновить словари веб-приложения
        /// </summary>
        private static void RefreshDictionaries()
        {
            // обновление словаря ScadaData
            bool reloaded;
            string msg;
            bool refreshOK = Localization.RefreshDictionary(AppDirs.LangDir, "ScadaData", 
                ref scadaDataDictAge, out reloaded, out msg);
            Log.WriteAction(msg, refreshOK ? Log.ActTypes.Action : Log.ActTypes.Error);

            if (reloaded)
                CommonPhrases.Init();

            // обновление словаря ScadaWeb
            refreshOK = Localization.RefreshDictionary(AppDirs.LangDir, "ScadaWeb", 
                ref scadaWebDictAge, out reloaded, out msg);
            Log.WriteAction(msg, refreshOK ? Log.ActTypes.Action : Log.ActTypes.Error);

            if (reloaded)
                WebPhrases.Init();
        }

        /// <summary>
        /// Обновить настройки соединения с сервером
        /// </summary>
        private static bool RefreshCommSettings()
        {
            return false;
        }

        /// <summary>
        /// Обновить настройки веб-приложения
        /// </summary>
        private static bool RefreshWebSettings()
        {
            return false;
        }

        /// <summary>
        /// Создать объекты доступа к данным
        /// </summary>
        private static void CreateDataObjects()
        {
            serverComm = new ServerComm(commSettings, Log);
            DataCache dataCache = new DataCache(serverComm, Log);
            DataAccess = new DataAccess(dataCache, Log);
            ViewCache = new ViewCache(serverComm, Log);
        }

        /// <summary>
        /// Загрузить информацию о плагинах
        /// </summary>
        private static void LoadPlugins()
        {
        }

        /// <summary>
        /// Инициализировать плагины
        /// </summary>
        private static void InitPlugins()
        {
        }


        /// <summary>
        /// Инициализировать общие данные веб-приложения
        /// </summary>
        public static void Init(string webAppDir)
        {
            lock (appDataLock)
            {
                if (!inited)
                {
                    inited = true;

                    // инициализация директорий приложения
                    AppDirs.Init(webAppDir);

                    // настройка журнала приложения
                    Log.FileName = AppDirs.LogDir + LogFileName;
                    Log.Encoding = Encoding.UTF8;
                    Log.WriteBreak();
                    Log.WriteAction(Localization.UseRussian ?
                        "Инициализация общих данных веб-приложения" :
                        "Initialize common web application data");
                }

                // обновление данных веб-приложения
                Refresh();
            }
        }

        /// <summary>
        /// Обновить общие данные веб-приложения
        /// </summary>
        public static void Refresh()
        {
            lock (appDataLock)
            {
                if (inited)
                {
                    RefreshDictionaries();

                    if (RefreshCommSettings())
                        CreateDataObjects();

                    if (RefreshWebSettings())
                    {
                        LoadPlugins();
                        InitPlugins();
                    }
                }
            }
        }

        /// <summary>
        /// Проверить правильность имени и пароля пользователя
        /// </summary>
        public static bool CheckUser(string username, string password, bool checkPassword, 
            out int roleID, out string errMsg)
        {
            if (checkPassword && string.IsNullOrEmpty(password))
            {
                roleID = BaseValues.Roles.Err;
                errMsg = WebPhrases.WrongPassword;
                return false;
            }
            else
            {
                if (serverComm.CheckUser(username, checkPassword ? password : null, out roleID))
                {
                    if (roleID == BaseValues.Roles.Disabled)
                    {
                        errMsg = WebPhrases.NoRightsL;
                        return false;
                    }
                    else if (roleID == BaseValues.Roles.App)
                    {
                        errMsg = WebPhrases.IllegalRole;
                        return false;
                    }
                    else if (roleID == BaseValues.Roles.Err)
                    {
                        errMsg = WebPhrases.WrongPassword;
                        return false;
                    }
                    else
                    {
                        errMsg = "";
                        return true;
                    }
                }
                else
                {
                    errMsg = WebPhrases.ServerUnavailable;
                    return false;
                }
            }
        }
    }
}