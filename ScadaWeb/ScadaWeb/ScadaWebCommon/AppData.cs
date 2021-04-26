/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
 */

using Scada.Client;
using Scada.Data.Configuration;
using Scada.Web.Plugins;
using Scada.Web.Shell;
using System;
using System.Collections.Generic;
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
        private string cultureName;                  // имя используемой культуры
        private long viewStampCntr;                  // счётчик для генерации меток представлений

        private DictUpdater scadaDataDictUpdater;    // объект для обновления словаря ScadaData
        private DictUpdater scadaWebDictUpdater;     // объект для обновления словаря ScadaWeb
        private SettingsUpdater webSettingsUpdater;  // объект для обновления настроек веб-приложения
        private SettingsUpdater viewSettingsUpdater; // объект для обновления настроек представлений
        private DateTime viewSettingsBaseAge;        // время изменения базы конфигурации для настроек представлений


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
            cultureName = Localization.Culture.Name;
            viewStampCntr = 0;

            scadaDataDictUpdater = null;
            scadaWebDictUpdater = null;
            webSettingsUpdater = null;
            viewSettingsUpdater = null;
            viewSettingsBaseAge = DateTime.MinValue;

            WebSettings = new WebSettings();
            ViewSettings = new ViewSettings();
            PluginSpecs = new List<PluginSpec>();
            UiObjSpecs = new Dictionary<string, UiObjSpec>();
            AppDirs = new AppDirs();
            Log = new Log(Log.Formats.Full);
            Storage = new Storage(AppDirs.StorageDir);
            RememberMe = new RememberMe(Storage, Log);
            Stats = new Stats(Storage, Log);
            UserMonitor = new UserMonitor(Log);

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
        /// Получить список спецификаций плагинов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal List<PluginSpec> PluginSpecs { get; private set; }

        /// <summary>
        /// Получить словарь спецификаций объектов пользовательского интерфейса, ключ - код типа объекта
        /// </summary>
        /// <remarks>Словарь заполняется на основе списка плагинов и 
        /// создаётся заново при изменении файла настроек веб-приложения</remarks>
        internal Dictionary<string, UiObjSpec> UiObjSpecs { get; private set; }


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
        /// Получить объект для передачи обезличенной статистики команде разработчиков
        /// </summary>
        public Stats Stats { get; private set; }

        /// <summary>
        /// Получить монитор активности пользователей
        /// </summary>
        public UserMonitor UserMonitor { get; private set; }

        /// <summary>
        /// Получить объект для обмена данными с сервером
        /// </summary>
        public ServerComm ServerComm { get; private set; }

        /// <summary>
        /// Получить объект для потокобезопасного доступа к данным кэша клиентов
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public DataAccess DataAccess { get; private set; }

        /// <summary>
        /// Получить кэш представлений
        /// </summary>
        /// <remarks>Объект создаётся заново при изменении файла настройки соединения с сервером</remarks>
        public ViewCache ViewCache { get; private set; }


        /// <summary>
        /// Инициализировать объекты для обновления словарей
        /// </summary>
        private void InitDictUpdaters()
        {
            scadaDataDictUpdater = new DictUpdater(AppDirs.LangDir, "ScadaData", CommonPhrases.Init, Log);
            scadaWebDictUpdater = new DictUpdater(AppDirs.LangDir, "ScadaWeb", WebPhrases.Init, Log);
        }

        /// <summary>
        /// Инициализировать объекты для обновления настроек
        /// </summary>
        private void InitSettingsUpdaters()
        {
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
            ServerComm = new ServerComm(WebSettings.CommSettings, Log);
            DataCache dataCache = new DataCache(ServerComm, Log);
            DataAccess = new DataAccess(dataCache, Log);
            ViewCache = new ViewCache(ServerComm, DataAccess, Log);
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
        /// Обновить настройки веб-приложения
        /// </summary>
        private void RefreshWebSettings(out bool webSettingsChanged, out bool commSettingsChanged)
        {
            CommSettings oldCommSettings = WebSettings.CommSettings;

            if (webSettingsUpdater.Update(out webSettingsChanged) && webSettingsChanged)
            {
                WebSettings = (WebSettings)webSettingsUpdater.Settings;
                commSettingsChanged = !oldCommSettings.Equals(WebSettings.CommSettings);
            }
            else
            {
                commSettingsChanged = false;
            }
        }

        /// <summary>
        /// Обновить настройки представлений
        /// </summary>
        private void RefreshViewSettings()
        {
            if (WebSettings.ViewsFromBase)
            {
                // обновление настроек представлений из базы конфигурации
                DataAccess.DataCache.RefreshBaseTables();
                DateTime baseAge = DataAccess.DataCache.BaseTables.BaseAge;

                if (baseAge > DateTime.MinValue && viewSettingsBaseAge != baseAge)
                {
                    ViewSettings newViewSettings = new ViewSettings();
                    string errMsg;

                    if (newViewSettings.LoadFromBase(DataAccess, out errMsg))
                    {
                        if (!ViewSettings.Equals(newViewSettings))
                        {
                            ViewSettings = newViewSettings;
                            viewSettingsBaseAge = baseAge;
                            viewSettingsUpdater.ResetFileAge();
                        }
                    }
                    else
                    {
                        Log.WriteError(errMsg);
                    }
                }
            }
            else
            {
                // обновление настроек представлений из файла
                bool changed;
                if (viewSettingsUpdater.Update(out changed) && changed)
                {
                    ViewSettings = (ViewSettings)viewSettingsUpdater.Settings;
                    viewSettingsBaseAge = DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Обработать изменение культуры веб-приложения
        /// </summary>
        private void ProcCultureChange()
        {
            if (cultureName != WebSettings.Culture)
            {
                cultureName = WebSettings.Culture;
                Localization.ChangeCulture(cultureName);
                ClientApiSvc.RefreshDataFormatter();
                InitDictUpdaters();
                RefreshDictionaries();
            }
        }

        /// <summary>
        /// Загрузить спецификации плагинов
        /// </summary>
        private void LoadPlugins()
        {
            PluginSpecs = new List<PluginSpec>();
            HashSet<string> procFileNames = new HashSet<string>();

            foreach (string fileName in WebSettings.PluginFileNames)
            {
                if (!procFileNames.Contains(fileName))
                {
                    string errMsg;
                    PluginSpec pluginSpec = PluginSpec.CreateFromDll(AppDirs.BinDir + fileName, out errMsg);

                    if (pluginSpec == null)
                        Log.WriteError(errMsg);
                    else
                        PluginSpecs.Add(pluginSpec);

                    procFileNames.Add(fileName);
                }
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
        /// Создать и заполнить словарь спецификаций объектов пользовательского интерфейса
        /// </summary>
        private void FillUiObjSpecs()
        {
            UiObjSpecs = new Dictionary<string, UiObjSpec>();

            foreach (PluginSpec pluginSpec in PluginSpecs)
            {
                try
                {
                    // функция добавления спецификации с сохранением уникальности по коду типа объекта
                    void addSpecFunc(UiObjSpec spec)
                    {
                        if (UiObjSpecs.ContainsKey(spec.TypeCode))
                        {
                            Log.WriteError(string.Format(Localization.UseRussian ?
                                "Спецификация \"{0}\" плагина \"{1}\" игнорируется, потому что дублируется" :
                                "The specification \"{0}\" of the plugin \"{1}\" is ignored because it is duplicated",
                                spec.TypeCode, pluginSpec.Name));
                        }
                        else
                        {
                            UiObjSpecs.Add(spec.TypeCode, spec);
                        }
                    }

                    // добавление спецификаций представлений
                    if (pluginSpec.ViewSpecs != null)
                    {
                        foreach (ViewSpec viewSpec in pluginSpec.ViewSpecs)
                        {
                            addSpecFunc(viewSpec);
                        }
                    }

                    // добавление спецификаций отчётов
                    if (pluginSpec.ReportSpecs != null)
                    {
                        foreach (ReportSpec reportSpec in pluginSpec.ReportSpecs)
                        {
                            addSpecFunc(reportSpec);
                        }
                    }

                    // добавление спецификаций окон данных
                    if (pluginSpec.DataWndSpecs != null)
                    {
                        foreach (DataWndSpec dataWndSpec in pluginSpec.DataWndSpecs)
                        {
                            addSpecFunc(dataWndSpec);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении спецификаций, реализуемых плагином \"{0}\"" :
                        "Error getting specifications implemented by the plugin \"{0}\"", 
                        pluginSpec.Name);
                }
            }
        }

        /// <summary>
        /// Добавить дополнительные скрипты к настройкам веб-приложения
        /// </summary>
        private void AppendExtraScripts()
        {
            List<string> extraScripts = new List<string>();

            foreach (PluginSpec pluginSpec in PluginSpecs)
            {
                try
                {
                    if (pluginSpec.ScriptPaths != null && pluginSpec.ScriptPaths.ExtraScripts != null)
                        extraScripts.AddRange(pluginSpec.ScriptPaths.ExtraScripts);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении скриптов плагина \"{0}\"" :
                        "Error getting scripts of the plugin \"{0}\"", pluginSpec.Name);
                }
            }

            WebSettings.ScriptPaths.ExtraScripts = extraScripts;
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
                    InitDictUpdaters();
                    InitSettingsUpdaters();
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
                    RefreshWebSettings(out bool webSettingsChanged, out bool commSettingsChanged);

                    if (commSettingsChanged)
                        CreateDataObjects();

                    if (webSettingsChanged)
                    {
                        ProcCultureChange();
                        LoadPlugins();
                        InitPlugins();
                        FillUiObjSpecs();
                        AppendExtraScripts();
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
                if (ServerComm.CheckUser(username, checkPassword ? password : null, out roleID))
                {
                    if (roleID == BaseValues.Roles.Disabled)
                    {
                        errMsg = WebPhrases.UserDisabled;
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
            return CheckLoggedOn(out UserRights userRights, throwOnFail);
        }

        /// <summary>
        /// Проверить, что пользователь вошёл систему, и получить его права
        /// </summary>
        /// <remarks>Метод используется, если сессия не доступна</remarks>
        public bool CheckLoggedOn(out UserRights userRights, bool throwOnFail = true)
        {
            return UserMonitor.CheckLoggedOn(out userRights, throwOnFail);
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
