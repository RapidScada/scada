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
 * Module   : Scheme Editor
 * Summary  : Common data of Scheme Editor
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Client;
using System;
using System.IO;
using System.ServiceModel;
using System.Text;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Common data of Scheme Editor
    /// <para>Общие данные Редактора схем</para>
    /// </summary>
    internal sealed class AppData
    {
        /// <summary>
        /// Имя файла журнала приложения без директории
        /// </summary>
        public const string LogFileName = "ScadaSchemeEditor.log";

        private static readonly AppData appDataInstance; // экземпляр объекта AppData

        private ServiceHost schemeEditorSvcHost; // хост WCF-службы для взаимодействия с веб-интерфейсом
        private long viewStampCntr;              // счётчик для генерации меток представлений


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
            schemeEditorSvcHost = null;
            viewStampCntr = 0;

            AppDirs = new AppDirs();
            Settings = new Settings();
            CompManager = CompManager.GetInstance();
            Log = new Log(Log.Formats.Full);
            Editor = new Editor(CompManager, Log);
            MainForm = null;
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Получить менеджер компонентов схемы
        /// </summary>
        public CompManager CompManager { get; private set; }

        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log Log { get; private set; }

        /// <summary>
        /// Получить объект, содержащий данные и реализующий логику редактора
        /// </summary>
        public Editor Editor { get; private set; }

        /// <summary>
        /// Получить или установить интерфейс главной формы
        /// </summary>
        public IMainForm MainForm { get; private set; }


        /// <summary>
        /// Запустить WCF-службу для взаимодействия с веб-интерфейсом
        /// </summary>
        private bool StartWcfService(out string serviceUrl)
        {
            try
            {
                schemeEditorSvcHost = new ServiceHost(typeof(SchemeEditorSvc));
                ServiceBehaviorAttribute behavior =
                    schemeEditorSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeEditorSvcHost.Open();
                serviceUrl = schemeEditorSvcHost.BaseAddresses.Count > 0 ?
                    schemeEditorSvcHost.BaseAddresses[0].AbsoluteUri : "";

                Log.WriteAction(Localization.UseRussian ?
                    "WCF-служба запущена" :
                    "WCF service is started");

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при запуске WCF-службы" :
                    "Error starting WCF service");
                serviceUrl = "";
                return false;
            }
        }

        /// <summary>
        /// Остановить WCF-службу, взаимодействующую с веб-интерфейсом
        /// </summary>
        private void StopWcfService()
        {
            if (schemeEditorSvcHost != null)
            {
                try
                {
                    schemeEditorSvcHost.Close();
                    Log.WriteAction(Localization.UseRussian ?
                        "WCF-служба остановлена" :
                        "WCF service is stopped");
                }
                catch
                {
                    schemeEditorSvcHost.Abort();
                    Log.WriteAction(Localization.UseRussian ?
                        "WCF-служба прервана" :
                        "WCF service is aborted");
                }

                schemeEditorSvcHost = null;
            }
        }


        /// <summary>
        /// Инициализировать общие данные Редактора схем
        /// </summary>
        public void Init(string exeDir, IMainForm mainForm)
        {
            // инициализация директорий приложения
            AppDirs.Init(exeDir);

            // установка интерфейса главной формы
            MainForm = mainForm;

            // настройка журнала приложения
            Log.FileName = AppDirs.LogDir + LogFileName;
            Log.Encoding = Encoding.UTF8;
            Log.WriteBreak();
            Log.WriteAction(Localization.UseRussian ?
                "Инициализация общих данных Редактора схем" :
                "Initialize common data of Scheme Editor");
        }

        /// <summary>
        /// Загрузить компоненты
        /// </summary>
        public void LoadComponents()
        {
            if (Directory.Exists(Settings.WebDir))
            {
                AppDirs.WebDir = Settings.WebDir;
            }
            else
            {
                Log.WriteError(Localization.UseRussian ?
                    "Не существует директория веб-интерфейса, указанная в настройках" :
                    "Web interface directory, specified in the settings, does not exist");
            }

            Web.AppDirs webAppDirs = new Web.AppDirs();
            webAppDirs.Init(AppDirs.WebDir);
            CompManager.Init(webAppDirs, Log);
            CompManager.LoadCompFromFiles();
            SchemeContext.GetInstance().Init(webAppDirs);
        }

        /// <summary>
        /// Запустить механизм редактора схем
        /// </summary>
        public bool StartEditor()
        {
            return StartWcfService(out string serviceUrl) && Editor.CreateWebPage(AppDirs.WebDir, serviceUrl);
        }

        /// <summary>
        /// Завершить работу приложения
        /// </summary>
        public void FinalizeApp()
        {
            StopWcfService();

            Log.WriteAction(Localization.UseRussian ?
                "Работа Редактора схем завершена" :
                "Scheme Editor is shutdown", Log.ActTypes.Action);
            Log.WriteBreak();
        }

        /// <summary>
        /// Присвоить представлению уникальную в пределах приложения метку
        /// </summary>
        public long AssignViewStamp(BaseView view)
        {
            if (view == null)
            {
                return 0;
            }
            else
            {
                lock (view.SyncRoot)
                {
                    if (view.Stamp <= 0)
                        view.Stamp = ++viewStampCntr;

                    return view.Stamp;
                }
            }
        }


        /// <summary>
        /// Получить общие данные приложения
        /// </summary>
        public static AppData GetAppData()
        {
            return appDataInstance;
        }
    }
}
