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
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Text;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Common data of the web application
    /// <para>Общие данные веб-приложения</para>
    /// </summary>
    public sealed class AppData
	{
        /// <summary>
        /// Имя файла журнала приложения без директории
        /// </summary>
        public const string LogFileName = "ScadaWeb.log";

        private static readonly AppData appDataInstance; // экземпляр объекта AppData
        private readonly object appDataLock;             // объект для синхронизации доступа к данным приложения

        private bool inited;                         // признак инициализации данных приложения
        private CommSettings commSettings;           // настройки соединения с сервером
        private ServerComm serverComm;               // объект для обмена данными с сервером
        private long viewStampCntr;                  // счётчик для генерации меток представлений

        private DictUpdater scadaDataDictUpdater;    // объект для обновления словаря ScadaData
        private DictUpdater scadaWebDictUpdater;     // объект для обновления словаря ScadaWeb
        private SettingsUpdater commSettingsUpdater; // объект для обновления настроек соединения с сервером
        private SettingsUpdater webSettingsUpdater;  // объект для обновления настроек веб-приложения
        private SettingsUpdater viewSettingsUpdater; // объект для обновления настроек представлений


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static AppData()
        {
            appDataInstance = new AppData();
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта из других классов
        /// </summary>
        private AppData()
		{
            appDataLock = new object();

            inited = false;
            commSettings = new CommSettings();
            viewStampCntr = 0;

            WebSettings = new WebSettings();
            ViewSettings = new ViewSettings();
            PluginSpecs = new List<PluginSpec>();
            ViewSpecs = new Dictionary<string, ViewSpec>();
            AppDirs = new AppDirs();
            Log = new Log(Log.Formats.Full);
            Storage = new Storage(AppDirs.StorageDir);
            RememberMe = new RememberMe(Storage, Log);
            UserMonitor = new UserMonitor(Log);

            InitUpdaters();
            CreateDataObjects();
        }


        /// <summary>
        /// Получить настройки веб-приложения
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal WebSettings WebSettings { get; private set; }

        /// <summary>
        /// Получить настройки представлений
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек представлений</remarks>
        internal ViewSettings ViewSettings { get; private set; }

        /// <summary>
        /// Получить список плагинов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal List<PluginSpec> PluginSpecs { get; private set; }

        /// <summary>
        /// Получить словарь спецификаций представлений, ключ - код типа представления
        /// </summary>
        /// <remarks>Словарь заполняется на основе списка плагинов и 
        /// создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal Dictionary<string, ViewSpec> ViewSpecs { get; private set; }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; private set; }
        
        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }

        /// <summary>
        /// Получить объект для работы с хранилищем приложения
        /// </summary>
        public Storage Storage { get; private set; }

        /// <summary>
        /// Получить объект для сохранения входа пользователей в систему
        /// </summary>
        public RememberMe RememberMe { get; private set; }

        /// <summary>
        /// Получить монитор активности пользователей
        /// </summary>
        public UserMonitor UserMonitor { get; private set; }

        /// <summary>
        /// Получить объект для потокобезопасного доступа к данным кеша клиентов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public DataAccess DataAccess { get; private set; }

        /// <summary>
        /// Получить кэш представлений
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public ViewCache ViewCache { get; private set; }


        /// <summary>
        /// Инициализировать объекты для обновления словарей и настроек
        /// </summary>
        private void InitUpdaters()
        {
            scadaDataDictUpdater = new DictUpdater(AppDirs.LangDir, "ScadaData", CommonPhrases.Init, Log);
            scadaWebDictUpdater = new DictUpdater(AppDirs.LangDir, "ScadaWeb", WebPhrases.Init, Log);

            commSettingsUpdater = new SettingsUpdater(commSettings, 
                AppDirs.ConfigDir + CommSettings.DefFileName, true, Log);
            webSettingsUpdater = new SettingsUpdater(WebSettings, 
                AppDirs.ConfigDir + WebSettings.DefFileName, true, Log);
            viewSettingsUpdater = new SettingsUpdater(ViewSettings,
                AppDirs.ConfigDir + ViewSettings.DefFileName, true, Log);
        }

        /// <summary>
        /// Создать объекты доступа к данным
        /// </summary>
        private void CreateDataObjects()
        {
            serverComm = new ServerComm(commSettings, Log);
            DataCache dataCache = new DataCache(serverComm, Log);
            DataAccess = new DataAccess(dataCache, Log);
            ViewCache = new ViewCache(serverComm, DataAccess, Log);
        }

        /// <summary>
        /// Обновить словари веб-приложения
        /// </summary>
        private void RefreshDictionaries()
        {
            scadaDataDictUpdater.Update();
            scadaWebDictUpdater.Update();
        }

        /// <summary>
        /// Обновить настройки соединения с сервером
        /// </summary>
        private bool RefreshCommSettings()
        {
            bool changed;
            if (commSettingsUpdater.Update(out changed) && changed)
                commSettings = (CommSettings)commSettingsUpdater.Settings;
            return changed;
        }

        /// <summary>
        /// Обновить настройки веб-приложения
        /// </summary>
        private bool RefreshWebSettings()
        {
            bool changed;
            if (webSettingsUpdater.Update(out changed) && changed)
                WebSettings = (WebSettings)webSettingsUpdater.Settings;
            return changed;
        }

        /// <summary>
        /// Обновить настройки представлений
        /// </summary>
        private void RefreshViewSettings()
        {
            bool changed;
            if (viewSettingsUpdater.Update(out changed) && changed)
                ViewSettings = (ViewSettings)viewSettingsUpdater.Settings;
        }

        /// <summary>
        /// Загрузить спецификации плагинов
        /// </summary>
        private void LoadPlugins()
        {
            PluginSpecs = new List<PluginSpec>();

            foreach (string fileName in WebSettings.PluginFileNames)
            {
                string errMsg;
                PluginSpec pluginSpec = PluginSpec.CreateFromDll(AppDirs.BinDir + fileName, out errMsg);

                if (pluginSpec == null)
                    Log.WriteError(errMsg);
                else
                    PluginSpecs.Add(pluginSpec);
            }
        }

        /// <summary>
        /// Инициализировать плагины
        /// </summary>
        private void InitPlugins()
        {
            foreach (PluginSpec pluginSpec in PluginSpecs)
            {
                try
                {
                    pluginSpec.AppDirs = AppDirs;
                    pluginSpec.Log = Log;
                    pluginSpec.Init();
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при инициализации плагина \"{0}\"" :
                        "Error initializing plugin \"{0}\"", pluginSpec.Name);
                }
            }
        }

        /// <summary>
        /// Создать и заполнить словарь спецификаций представлений
        /// </summary>
        private void FillViewSpecs()
        {
            ViewSpecs = new Dictionary<string, ViewSpec>();

            foreach (PluginSpec pluginSpec in PluginSpecs)
            {
                try
                {
                    if (pluginSpec.ViewSpecs != null)
                    {
                        foreach (ViewSpec viewSpec in pluginSpec.ViewSpecs)
                        {
                            if (ViewSpecs.ContainsKey(viewSpec.ViewTypeCode))
                            {
                                Log.WriteError(string.Format(Localization.UseRussian ?
                                    "Спецификация представлений \"{0}\" плагина \"{1}\" игнорируется, потому что дублируется" :
                                    "View specification \"{0}\" of the plugin \"{1}\" is ignored because it is duplicated",
                                    viewSpec.ViewTypeCode, pluginSpec.Name));
                            }
                            else
                            {
                                ViewSpecs.Add(viewSpec.ViewTypeCode, viewSpec);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении спецификаций представлений, реализуемых плагином \"{0}\"" :
                        "Error getting view specifications implemented by plugin \"{0}\"", pluginSpec.Name);
                }
            }
        }


        /// <summary>
        /// Инициализировать общие данные веб-приложения
        /// </summary>
        public void Init(string webAppDir)
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

                    // настройка объекта для работы с хранилищем приложения
                    Storage.StorageDir = AppDirs.StorageDir;

                    // инициализация объектов для обновления настроек
                    InitUpdaters();
                }

                // обновление данных веб-приложения
                Refresh();
            }
        }

        /// <summary>
        /// Обновить общие данные веб-приложения
        /// </summary>
        public void Refresh()
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
                        FillViewSpecs();
                    }

                    RefreshViewSettings();
                }
            }
        }

        /// <summary>
        /// Проверить правильность имени и пароля пользователя
        /// </summary>
        public bool CheckUser(string username, string password, bool checkPassword, 
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
                        errMsg = WebPhrases.NoRights;
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

        /// <summary>
        /// Проверить, что пользователь вошёл систему
        /// </summary>
        /// <remarks>Метод используется, если сессия не доступна</remarks>
        public bool CheckLoggedOn(bool throwOnFail = true)
        {
            UserRights userRights;
            return CheckLoggedOn(out userRights, throwOnFail);
        }

        /// <summary>
        /// Проверить, что пользователь вошёл систему, и получить его права
        /// </summary>
        /// <remarks>Метод используется, если сессия не доступна</remarks>
        public bool CheckLoggedOn(out UserRights userRights, bool throwOnFail = true)
        {
            if (UserMonitor.UserIsLoggedOn(WebOperationContext.Current, out userRights))
                return true;
            else if (throwOnFail)
                throw new ScadaException(WebPhrases.NotLoggedOn);
            else
                return false;
        }

        /// <summary>
        /// Присвоить представлению уникальную в пределах приложения метку
        /// </summary>
        public long AssignStamp(BaseView view)
        {
            if (view == null)
            {
                return 0;
            }
            else lock (view.SyncRoot)
            {
                if (view.Stamp <= 0)
                    view.Stamp = ++viewStampCntr;

                return view.Stamp;
            }
        }


        /// <summary>
        /// Получить общие данные веб-приложения
        /// </summary>
        public static AppData GetAppData()
        {
            return appDataInstance;
        }
    }
}