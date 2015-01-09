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
 * Module   : SCADA-Server Service
 * Summary  : Main server logic implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Scada.Data;
using Scada.Server.Module;
using Utils;

namespace Scada.Server.Svc
{
    /// <summary>
    /// Main server logic implementation
    /// <para>Реализация основной логики сервера</para>
    /// </summary>
    sealed partial class MainLogic
    {
        /// <summary>
        /// Наименования состояний работы
        /// </summary>
        private static class WorkStateNames
        {
            /// <summary>
            /// Статический конструктор
            /// </summary>
            static WorkStateNames()
            {
                if (Localization.UseRussian)
                {
                    Normal = "норма";
                    Stopped = "остановлен";
                    Error = "ошибка";
                }
                else
                {
                    Normal = "normal";
                    Stopped = "stopped";
                    Error = "error";
                }
            }

            /// <summary>
            /// Норма
            /// </summary>
            public static readonly string Normal;
            /// <summary>
            /// Остановлен
            /// </summary>
            public static readonly string Stopped;
            /// <summary>
            /// Ошибка
            /// </summary>
            public static readonly string Error;
        }

        /// <summary>
        /// Старший байт номера версии приложения
        /// </summary>
        public const byte AppVersionHi = 4;
        /// <summary>
        /// Младший байт номера версии приложения
        /// </summary>
        public const byte AppVersionLo = 4;
        /// <summary>
        /// Имя файла журнала приложения
        /// </summary>
        private const string LogFileName = "ScadaServerSvc.log";
        /// <summary>
        /// Имя файла информации о работе приложения
        /// </summary>
        private const string InfoFileName = "ScadaServerSvc.txt";
        /// <summary>
        /// Время ожидания остановки потока, мс
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// Вместимость кэша таблиц минутных срезов
        /// </summary>
        private const int MinCacheCapacity = 5;
        /// <summary>
        /// Вместимость кэша таблиц часовых срезов
        /// </summary>
        private const int HourCacheCapacity = 10;
        /// <summary>
        /// Период хранения кэша таблиц минутных срезов
        /// </summary>
        private static readonly TimeSpan MinCacheStorePer = TimeSpan.FromMinutes(10);
        /// <summary>
        /// Период хранения кэша таблиц часовых срезов
        /// </summary>
        private static readonly TimeSpan HourCacheStorePer = TimeSpan.FromMinutes(10);
        /// <summary>
        /// Интервал очистки кэша таблиц срезов
        /// </summary>
        private static readonly TimeSpan CacheClearSpan = TimeSpan.FromMinutes(1);
        /// <summary>
        /// Имена файлов базы конфигурации в формате DAT
        /// </summary>
        private static readonly string[] BaseFiles = 
        { 
            "cmdtype.dat", "cmdval.dat", "cnltype.dat", "commline.dat", "ctrlcnl.dat", "evtype.dat", 
            "format.dat", "formula.dat", "incnl.dat", "interface.dat", "kp.dat", "kptype.dat", "obj.dat", 
            "param.dat", "right.dat", "role.dat", "unit.dat", "user.dat"
        };
        /// <summary>
        /// Формат текста информации о работе приложения для вывода в файл
        /// </summary>
        private static readonly string AppInfoFormat = Localization.UseRussian ?
            "SCADA-Сервер\r\n------------\r\n" +
            "Запуск       : {0}\r\nВремя работы : {1}\r\nСостояние    : {2}\r\nВерсия       : {3}" :
            "SCADA-Server\r\n------------\r\n" +
            "Started        : {0}\r\nExecution time : {1}\r\nState          : {2}\r\nVersion        : {3}";
        /// <summary>
        /// Формат описания события на команду ТУ
        /// </summary>
        private static readonly string EventOnCmdFormat = Localization.UseRussian ?
            "Команда ТУ: канал упр. = {0}, значение = {1}, ид. польз. = {2}" :
            "Command: out channel = {0}, value = {1}, user ID = {2}";

        private string infoFileName;               // полное имя файла информации
        private Thread thread;                     // поток работы сервера
        private volatile bool terminated;          // работа потока прервана
        private volatile bool serverIsReady;       // сервер готов к работе
        private DateTime startDT;                  // дата и время запуска работы
        private string workState;                  // состояние работы
        private Comm comm;                         // взаимодействие с клиентами
        private Calculator calculator;             // калькулятор для вычисления данных входных каналов
        private SortedList<int, InCnl> inCnls;     // активные входные каналы
        private List<InCnl> drCnls;                // список каналов типа дорасчётный ТС и ТИ, количество переключений
        private List<InCnl> drmCnls;               // список каналов типа минутный ТС и ТИ
        private List<InCnl> drhCnls;               // список каналов типа часовой ТС и ТИ
        private int[] drCnlNums;                   // номера каналов drCnls
        private int[] drmCnlNums;                  // номера каналов drmCnls
        private int[] drhCnlNums;                  // номера каналов drhCnls
        private List<int> avgCnlInds;              // индексы усредняемых каналов типа ТИ
        private SortedList<int, CtrlCnl> ctrlCnls; // активные каналы управления
        private SortedList<string, User> users;    // пользователи
        private List<string> formulas;             // формулы
        private SrezTable.Srez curSrez;            // текущий срез, предназначенный для формироваться данных сервера
        private bool curSrezMod;                   // признак изменения текущего среза (для записи по изменению)
        private SrezTableLight.Srez procSrez;      // срез, обрабатываемый в настоящий момент
        private SrezTable.SrezDescr srezDescr;     // описание создаваемых срезов
        private AvgData[] minAvgData;              // минутные данные для усреднения
        private AvgData[] hrAvgData;               // часовые данные для усреднения
        private DateTime[] activeDTs;              // дата и время активности каналов
        private SrezAdapter curSrezAdapter;        // адаптер таблицы текущего среза
        private SrezAdapter curSrezCopyAdapter;    // адаптер таблицы копии текущего среза
        private EventAdapter eventAdapter;         // адаптер таблицы событий
        private EventAdapter eventCopyAdapter;     // адаптер таблицы копий событий
        private SortedList<DateTime, SrezTableCache> minSrezTableCache; // кэш таблиц минутных срезов
        private SortedList<DateTime, SrezTableCache> hrSrezTableCache;  // кэш таблиц часовых срезов
        private List<ModLogic> modules;            // список модулей


        /// <summary>
        /// Конструктор
        /// </summary>
        public MainLogic()
        {
            ExeDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            ConfigDir = "";
            LangDir = "";
            LogDir = "";
            ModDir = "";
            AppLog = new Log(Log.Formats.Full);
            AppLog.Encoding = Encoding.UTF8;
            Settings = new Settings();

            infoFileName = "";
            thread = null;
            terminated = false;
            serverIsReady = false;
            startDT = DateTime.MinValue;
            workState = "";
            comm = new Comm(this);
            calculator = new Calculator(this);
            inCnls = new SortedList<int, InCnl>();
            drCnls = null;
            drmCnls = null;
            drhCnls = null;
            drCnlNums = null;
            drmCnlNums = null;
            drhCnlNums = null;
            avgCnlInds = null;
            ctrlCnls = new SortedList<int, CtrlCnl>();
            users = new SortedList<string, User>();
            formulas = new List<string>();
            curSrez = null;
            curSrezMod = false;
            srezDescr = null;
            minAvgData = null;
            hrAvgData = null;
            activeDTs = null;
            curSrezAdapter = null;
            curSrezCopyAdapter = null;
            eventAdapter = null;
            eventCopyAdapter = null;
            minSrezTableCache = null;
            hrSrezTableCache = null;
            modules = new List<ModLogic>();
        }


        /// <summary>
        /// Получить директорию исполняемого файла приложения
        /// </summary>
        public string ExeDir { get; private set; }

        /// <summary>
        /// Получить директорию конфигурации приложения
        /// </summary>
        public string ConfigDir { get; private set; }

        /// <summary>
        /// Получить директорию языковых файлов приложения
        /// </summary>
        public string LangDir { get; private set; }

        /// <summary>
        /// Получить директорию журналов приложения
        /// </summary>
        public string LogDir { get; private set; }

        /// <summary>
        /// Получить директорию подключаемых модулей
        /// </summary>
        public string ModDir { get; private set; }

        /// <summary>
        /// Получить журнал приложения
        /// </summary>
        public Log AppLog { get; private set; }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Получить признак, что сервер готов к работе
        /// </summary>
        public bool ServerIsReady
        {
            get
            {
                return serverIsReady;
            }
        }


        /// <summary>
        /// Загрузить модули
        /// </summary>
        private void LoadModules()
        {
            lock (modules)
            {
                // очистка списка модулей
                modules.Clear();

                foreach (string fileName in Settings.ModuleFileNames)
                {
                    string fullFileName = ModDir + fileName;

                    try
                    {
                        if (!File.Exists(fullFileName))
                            throw new Exception(Localization.UseRussian ? "Файл не найден." : "File not found.");

                        // создание экземпляра класса модуля
                        Assembly asm = Assembly.LoadFile(fullFileName);
                        Type type = asm.GetType("Scada.Server.Module." + 
                            Path.GetFileNameWithoutExtension(fileName) + "Logic", true);
                        ModLogic modLogic = Activator.CreateInstance(type) as ModLogic;
                        modLogic.ConfigDir = ConfigDir;
                        modLogic.LangDir = LangDir;
                        modLogic.LogDir = LogDir;
                        modLogic.Settings = Settings;
                        modLogic.WriteToLog = AppLog.WriteAction;
                        modLogic.PassCommand = comm.PassCommand;
                        modules.Add(modLogic);
                        AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                            "Загружен модуль из файла {0}" : "Module is loaded from the file {0}", 
                            fullFileName), Log.ActTypes.Action);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                            "Ошибка при загрузке модуля из файла {0}: {1}" :
                            "Error loading module from the file {0}: {1}", 
                            fullFileName, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Проверить существование директорий данных
        /// </summary>
        private bool CheckDataDirs()
        {
            // проверка существования директорий
            string dirNotExistStr = Localization.UseRussian ? 
                "Директория {0}{1} не существует." :
                "The {0}{1} directory does not exist.";
            string datFormatStr = Localization.UseRussian ? " в формате DAT" : " in DAT format";
            List<string> errors = new List<string>();

            if (!Directory.Exists(Settings.BaseDATDir))
                errors.Add(string.Format(dirNotExistStr, Localization.UseRussian ? 
                    "базы конфигурации" : "configuration database", datFormatStr));
            if (!Directory.Exists(Settings.ItfDir))
                errors.Add(string.Format(dirNotExistStr, Localization.UseRussian ? 
                    "интерфейса" : "interface", ""));
            if (!Directory.Exists(Settings.ArcDir))
                errors.Add(string.Format(dirNotExistStr, Localization.UseRussian ?
                    "архива" : "archive", datFormatStr));
            if (!Directory.Exists(Settings.ArcCopyDir))
                errors.Add(string.Format(dirNotExistStr, Localization.UseRussian ?
                    "копии архива" : "archive copy", datFormatStr));
            if (Settings.ArcDir == Settings.ArcCopyDir)
                errors.Add(Localization.UseRussian ?
                    "Директория архива в формате DAT и директория его копии совпадают." :
                    "The archive in DAT format directory and its copy directory are equal.");

            if (errors.Count > 0)
            {
                AppLog.WriteAction(string.Join("\n", errors), Log.ActTypes.Error);
                return false;
            }
            else
            {
                // создание поддиректорий архива, если они не существуют
                try
                {
                    Directory.CreateDirectory(Settings.ArcDir + @"Cur\");
                    Directory.CreateDirectory(Settings.ArcDir + @"Min\");
                    Directory.CreateDirectory(Settings.ArcDir + @"Hour\");
                    Directory.CreateDirectory(Settings.ArcDir + @"Events\");
                    Directory.CreateDirectory(Settings.ArcCopyDir + @"Cur\");
                    Directory.CreateDirectory(Settings.ArcCopyDir + @"Min\");
                    Directory.CreateDirectory(Settings.ArcCopyDir + @"Hour\");
                    Directory.CreateDirectory(Settings.ArcCopyDir + @"Events\");

                    AppLog.WriteAction(Localization.UseRussian ? 
                        "Проверка существования директорий данных выполнена успешно" :
                        "Check the existence of the data directories is completed successfully", Log.ActTypes.Action);
                    return true;
                }
                catch (Exception ex)
                {
                    AppLog.WriteAction((Localization.UseRussian ? "Ошибка при создании поддиректорий архива: " : 
                        "Error creating subdirectories of the archive: ") + ex.Message, Log.ActTypes.Exception);
                    return false;
                }
            }
        }

        /// <summary>
        /// Проверить существование файлов базы конфигурации
        /// </summary>
        private bool CheckBaseFiles()
        {
            List<string> requiredFiles = new List<string>();

            foreach (string fileName in BaseFiles)
            {
                string path = Settings.BaseDATDir + fileName;
                if (!File.Exists(path))
                    requiredFiles.Add(string.Format(Localization.UseRussian ? 
                        "Не существует файл базы конфигурации {0}" : 
                        "The configuration database file {0} not found", path));
            }


            if (requiredFiles.Count > 0)
            {
                AppLog.WriteAction(string.Join("\n", requiredFiles), Log.ActTypes.Error);
                return false;
            }
            else
            {
                AppLog.WriteAction(Localization.UseRussian ? 
                    "Проверка существования файлов базы конфигурации выполнена успешно" :
                    "Check the existence of the configuration database files is completed successfully", 
                    Log.ActTypes.Action);
                return true;
            }
        }

        /// <summary>
        /// Считать входные каналы из базы конфигурации
        /// </summary>
        private bool ReadInCnls()
        {
            try
            {
                lock (inCnls)
                {
                    // очистка информации о каналах
                    inCnls.Clear();
                    drCnls = new List<InCnl>();
                    drmCnls = new List<InCnl>();
                    drhCnls = new List<InCnl>();
                    drCnlNums = null;
                    drmCnlNums = null;
                    drhCnlNums = null;
                    avgCnlInds = new List<int>();

                    // заполнение информации о каналах
                    DataTable tblInCnl = new DataTable();
                    BaseAdapter adapter = new BaseAdapter();
                    adapter.FileName = Settings.BaseDATDir + @"\incnl.dat";
                    adapter.Fill(tblInCnl, false);

                    foreach (DataRow dataRow in tblInCnl.Rows)
                    {
                        if ((bool)dataRow["Active"])
                        {
                            InCnl inCnl = new InCnl();
                            inCnl.CnlNum = (int)dataRow["CnlNum"];
                            inCnl.CnlTypeID = (int)dataRow["CnlTypeID"];
                            inCnl.ObjNum = (int)dataRow["ObjNum"];
                            inCnl.KPNum = (int)dataRow["KPNum"];
                            inCnl.FormulaUsed = (bool)dataRow["FormulaUsed"];
                            inCnl.Formula = (string)dataRow["Formula"];
                            inCnl.Averaging = (bool)dataRow["Averaging"];
                            inCnl.ParamID = (int)dataRow["ParamID"];
                            inCnl.EvEnabled = (bool)dataRow["EvEnabled"];
                            inCnl.EvOnChange = (bool)dataRow["EvOnChange"];
                            inCnl.EvOnUndef = (bool)dataRow["EvOnUndef"];
                            inCnl.LimLowCrash = (double)dataRow["LimLowCrash"];
                            inCnl.LimLow = (double)dataRow["LimLow"];
                            inCnl.LimHigh = (double)dataRow["LimHigh"];
                            inCnl.LimHighCrash = (double)dataRow["LimHighCrash"];

                            int cnlTypeID = inCnl.CnlTypeID;
                            if (BaseValues.CnlTypes.MinCnlTypeID <= cnlTypeID && 
                                cnlTypeID <= BaseValues.CnlTypes.MaxCnlTypeID)
                                inCnls.Add(inCnl.CnlNum, inCnl);
                            
                            if (cnlTypeID == BaseValues.CnlTypes.TSDR || cnlTypeID == BaseValues.CnlTypes.TIDR ||
                                cnlTypeID == BaseValues.CnlTypes.SWCNT)
                                drCnls.Add(inCnl);
                            else if (cnlTypeID == BaseValues.CnlTypes.TSDRM || cnlTypeID == BaseValues.CnlTypes.TIDRM)
                                drmCnls.Add(inCnl);
                            else if (cnlTypeID == BaseValues.CnlTypes.TSDRH || cnlTypeID == BaseValues.CnlTypes.TIDRH)
                                drhCnls.Add(inCnl);

                            if (inCnl.Averaging && cnlTypeID == BaseValues.CnlTypes.TI)
                                avgCnlInds.Add(inCnls.Count - 1);
                        }
                    }

                    // заполнение номеров дорасчётных каналов
                    int cnt = drCnls.Count;
                    drCnlNums = new int[cnt];
                    for (int i = 0; i < cnt; i++)
                        drCnlNums[i] = drCnls[i].CnlNum;
                    
                    cnt = drmCnls.Count;
                    drmCnlNums = new int[cnt];
                    for (int i = 0; i < cnt; i++)
                        drmCnlNums[i] = drmCnls[i].CnlNum;

                    cnt = drhCnls.Count;
                    drhCnlNums = new int[cnt];
                    for (int i = 0; i < cnt; i++)
                        drhCnlNums[i] = drhCnls[i].CnlNum;

                    // определение результата
                    if (inCnls.Count > 0)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                            "Входные каналы считаны из базы конфигурации. Количество активных каналов: {0}" : 
                            "Input channels are read from the configuration database. Active channel count: {0}",
                            inCnls.Count), Log.ActTypes.Action);
                        return true;
                    }
                    else
                    {
                        AppLog.WriteAction(Localization.UseRussian ? 
                            "В базе конфигурации отсутствуют активные входные каналы" :
                            "No active input channels in the configuration database", Log.ActTypes.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при считывании входных каналов из базы конфигурации: " :
                    "Error reading input channels from the configuration database: ") + 
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Считать каналы управления из базы конфигурации
        /// </summary>
        private bool ReadCtrlCnls()
        {
            try
            {
                lock (ctrlCnls)
                {
                    ctrlCnls.Clear();
                    DataTable tblCtrlCnl = new DataTable();
                    BaseAdapter adapter = new BaseAdapter();
                    adapter.FileName = Settings.BaseDATDir + @"\ctrlcnl.dat";
                    adapter.Fill(tblCtrlCnl, false);

                    foreach (DataRow dataRow in tblCtrlCnl.Rows)
                    {
                        if ((bool)dataRow["Active"])
                        {
                            CtrlCnl ctrlCnl = new CtrlCnl();
                            ctrlCnl.CtrlCnlNum = (int)dataRow["CtrlCnlNum"];
                            ctrlCnl.CmdTypeID = (int)dataRow["CmdTypeID"];
                            ctrlCnl.ObjNum = (int)dataRow["ObjNum"];
                            ctrlCnl.KPNum = (int)dataRow["KPNum"];
                            ctrlCnl.CmdNum = (int)dataRow["CmdNum"];
                            ctrlCnl.FormulaUsed = (bool)dataRow["FormulaUsed"];
                            ctrlCnl.Formula = (string)dataRow["Formula"];
                            ctrlCnl.EvEnabled = (bool)dataRow["EvEnabled"];
                            ctrlCnls.Add(ctrlCnl.CtrlCnlNum, ctrlCnl);
                        }
                    }
                }

                AppLog.WriteAction(Localization.UseRussian ? "Каналы управления считаны из базы конфигурации" :
                    "Ouput channels are read from the configuration database", Log.ActTypes.Action);
                return true;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при считывании каналов управления из базы конфигурации: " :
                    "Error reading ouput channels from the configuration database: ") +
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Считать пользователей из базы конфигурации
        /// </summary>
        private bool ReadUsers()
        {
            try
            {
                lock (users)
                {
                    users.Clear();
                    DataTable tblUser = new DataTable();
                    BaseAdapter adapter = new BaseAdapter();
                    adapter.FileName = Settings.BaseDATDir + @"\user.dat";
                    adapter.Fill(tblUser, false);

                    foreach (DataRow dataRow in tblUser.Rows)
                    {
                        User user = new User();
                        user.Name = (string)dataRow["Name"];
                        user.Password = (string)dataRow["Password"];
                        user.RoleID = (int)dataRow["RoleID"];
                        users[user.Name.Trim().ToLower()] = user;
                    }
                }

                AppLog.WriteAction(Localization.UseRussian ? "Пользователи считаны из базы конфигурации" :
                    "Users are read from the configuration database", Log.ActTypes.Action);
                return true;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при считывании пользователей из базы конфигурации: " :
                    "Error reading users from the configuration database: ") + 
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Считать формулы из базы конфигурации
        /// </summary>
        private bool ReadFormulas()
        {
            try
            {
                formulas.Clear();
                DataTable tblFormula = new DataTable();
                BaseAdapter adapter = new BaseAdapter();
                adapter.FileName = Settings.BaseDATDir + @"\formula.dat";
                adapter.Fill(tblFormula, false);

                foreach (DataRow dataRow in tblFormula.Rows)
                    formulas.Add((string)dataRow["Source"]);

                AppLog.WriteAction(Localization.UseRussian ? "Формулы считаны из базы конфигурации" :
                    "Formulas are read from the configuration database", Log.ActTypes.Action);
                return true;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? "Ошибка при считывании формул из базы конфигурации: " :
                    "Error reading formulas from the configuration database: ") + ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Считать необходимые данные из базы конфигурации
        /// </summary>
        private bool ReadBase()
        {
            return ReadInCnls() && ReadCtrlCnls() && ReadUsers() && ReadFormulas();
        }

        /// <summary>
        /// Инициализировать калькулятор для вычисления данных входных каналов
        /// </summary>
        private bool InitCalculator()
        {
            // очистка и добавление формул в калькулятор
            calculator.ClearFormulas();

            foreach (string formula in formulas)
                calculator.AddAuxFormulaSource(formula);

            foreach (InCnl inCnl in inCnls.Values)
            {
                if (inCnl.FormulaUsed)
                    calculator.AddCnlFormulaSource(inCnl.CnlNum, inCnl.Formula);
            }

            foreach (CtrlCnl ctrlCnl in ctrlCnls.Values)
            {
                if (ctrlCnl.FormulaUsed && ctrlCnl.CmdTypeID == 0 /*стандартная команда*/)
                    calculator.AddCtrlCnlFormulaSource(ctrlCnl.CtrlCnlNum, ctrlCnl.Formula);
            }

            // компиляция формул и получение методов вычисления каналов
            if (calculator.CompileSource())
            {
                foreach (InCnl inCnl in inCnls.Values)
                {
                    if (inCnl.FormulaUsed)
                    {
                        inCnl.CalcCnlData = calculator.GetCalcCnlData(inCnl.CnlNum);
                        if (inCnl.CalcCnlData == null)
                            return false;
                    }
                    else
                    {
                        inCnl.CalcCnlData = null;
                    }
                }

                foreach (CtrlCnl ctrlCnl in ctrlCnls.Values)
                {
                    if (ctrlCnl.FormulaUsed && ctrlCnl.CmdTypeID == 0 /*стандартная команда*/)
                    {
                        ctrlCnl.CalcCmdVal = calculator.GetCalcCmdVal(ctrlCnl.CtrlCnlNum);
                        if (ctrlCnl.CalcCmdVal == null)
                            return false;
                    }
                    else
                    {
                        ctrlCnl.CalcCmdVal = null;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Цикл работы менеждера (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            try
            {
                // запись информации о работе приложения
                workState = WorkStateNames.Normal;
                WriteInfo();

                // выполнение действий модулей
                RaiseOnServerStart();

                // инициализация адаптеров таблиц текущего среза и событий
                curSrezAdapter = new SrezAdapter();
                curSrezCopyAdapter = new SrezAdapter();
                eventAdapter = new EventAdapter();
                eventCopyAdapter = new EventAdapter();
                curSrezAdapter.FileName = Settings.ArcDir + @"Cur\current.dat";
                curSrezCopyAdapter.FileName = Settings.ArcCopyDir + @"Cur\current.dat";
                eventAdapter.Directory = Settings.ArcDir + @"Events\";
                eventCopyAdapter.Directory = Settings.ArcCopyDir + @"Events\";

                // инициализация кэша таблиц минутных и часовых срезов
                minSrezTableCache = new SortedList<DateTime, SrezTableCache>();
                hrSrezTableCache = new SortedList<DateTime, SrezTableCache>();

                // инициализация описания создаваемых срезов
                int cnlCnt = inCnls.Count;
                srezDescr = new SrezTable.SrezDescr(cnlCnt);
                for (int i = 0; i < cnlCnt; i++)
                    srezDescr.CnlNums[i] = inCnls.Values[i].CnlNum;                
                srezDescr.CalcCS();

                // загрузка исходного текущего среза из файла
                SrezTableLight.Srez curSrezSrc = null;
                SrezTableLight tblCurSrezScr = new SrezTableLight();

                try
                {
                    if (File.Exists(curSrezAdapter.FileName))
                    {
                        curSrezAdapter.Fill(tblCurSrezScr);
                        if (tblCurSrezScr.SrezList.Count > 0)
                            curSrezSrc = tblCurSrezScr.SrezList.Values[0];
                    }

                    if (curSrezSrc == null)
                        AppLog.WriteAction(Localization.UseRussian ? "Текущий срез не загружен" :
                            "Current data are not loaded", Log.ActTypes.Action);
                    else
                        AppLog.WriteAction(Localization.UseRussian ? "Текущий срез загружен" :
                            "Current data are loaded", Log.ActTypes.Action);
                }
                catch (Exception ex)
                {
                    AppLog.WriteAction((Localization.UseRussian ? "Ошибка при загрузке текущего среза: " : 
                        "Error loading current data: ") + ex.Message, Log.ActTypes.Exception);
                }

                // инициализация текущего среза, предназначенного для формироваться данных сервера
                curSrez = new SrezTable.Srez(DateTime.MinValue, srezDescr, curSrezSrc);

                // инициализация данных для усреднения и времени активности каналов
                minAvgData = new AvgData[cnlCnt];
                hrAvgData = new AvgData[cnlCnt];
                activeDTs = new DateTime[cnlCnt];
                DateTime nowDT = DateTime.Now;

                for (int i = 0; i < cnlCnt; i++)
                {
                    minAvgData[i] = new AvgData() { Sum = 0.0, Cnt = 0 };
                    hrAvgData[i] = new AvgData() { Sum = 0.0, Cnt = 0 };
                    activeDTs[i] = nowDT;
                }

                // цикл работы сервера
                nowDT = DateTime.MaxValue;
                DateTime today = nowDT.Date;
                DateTime prevDT;
                DateTime writeCurSrezDT = DateTime.MinValue;
                DateTime writeMinSrezDT = DateTime.MinValue;
                DateTime writeHrSrezDT = DateTime.MinValue;
                DateTime calcMinDT = DateTime.MinValue;
                DateTime calcHrDT = DateTime.MinValue;
                DateTime clearCacheDT = nowDT;

                bool writeCur = Settings.WriteCur || Settings.WriteCurCopy;
                bool writeCurOnMod = Settings.WriteCurPer <= 0;
                bool writeMin = (Settings.WriteMin || Settings.WriteMinCopy) && Settings.WriteMinPer > 0;
                bool writeHr = (Settings.WriteHr || Settings.WriteHrCopy) && Settings.WriteHrPer > 0;

                curSrezMod = false;
                serverIsReady = true;

                while (!terminated)
                {
                    prevDT = nowDT;
                    nowDT = DateTime.Now;
                    today = nowDT.Date;

                    // расчёт времени записи срезов и вычисления значений дорасчётных каналов
                    // при переводе времени назад или при первом проходе цикла
                    if (prevDT > nowDT)
                    {
                        writeCurSrezDT = nowDT;
                        writeMinSrezDT = CalcNextTime(nowDT, Settings.WriteMinPer);
                        writeHrSrezDT = CalcNextTime(nowDT, Settings.WriteHrPer);
                        calcMinDT = drmCnls.Count > 0 ? CalcNextTime(nowDT, 60) : DateTime.MaxValue;
                        calcHrDT = drhCnls.Count > 0 ? CalcNextTime(nowDT, 3600) : DateTime.MaxValue;
                    }

                    // удаление устаревших файлов срезов и событий при изменении даты или при первом проходе цикла
                    if (prevDT.Date != today)
                    {
                        ClearArchive(Settings.ArcDir + @"Min\", "m*.dat", today.AddDays(-Settings.StoreMinPer));
                        ClearArchive(Settings.ArcDir + @"Hour\", "h*.dat", today.AddDays(-Settings.StoreHrPer));
                        ClearArchive(Settings.ArcDir + @"Events\", "e*.dat", today.AddDays(-Settings.StoreEvPer));
                        ClearArchive(Settings.ArcCopyDir + @"Min\", "m*.dat", today.AddDays(-Settings.StoreMinPer));
                        ClearArchive(Settings.ArcCopyDir + @"Hour\", "h*.dat", today.AddDays(-Settings.StoreHrPer));
                        ClearArchive(Settings.ArcCopyDir + @"Events\", "e*.dat", today.AddDays(-Settings.StoreEvPer));
                    }

                    lock (curSrez)
                    {
                        // установка недостоверности неактивных каналов
                        SetUnreliable();

                        // вычисление дорасчётных каналов и выполнение действий модулей
                        CalcDRCnls(drCnls, curSrez, true);
                        RaiseOnCurDataCalculated(drCnlNums, curSrez);

                        // вычисление минутных каналов и выполнение действий модулей
                        if (calcMinDT <= nowDT)
                        {
                            CalcDRCnls(drmCnls, curSrez, true);
                            RaiseOnCurDataCalculated(drmCnlNums, curSrez);
                            calcMinDT = CalcNextTime(nowDT, 60);
                            curSrezMod = true;
                        }

                        // вычисление часовых каналов и выполнение действий модулей
                        if (calcHrDT <= nowDT)
                        {
                            CalcDRCnls(drhCnls, curSrez, true);
                            RaiseOnCurDataCalculated(drhCnlNums, curSrez);
                            calcHrDT = CalcNextTime(nowDT, 3600);
                            curSrezMod = true;
                        }

                        // запись текущего среза
                        if ((writeCurSrezDT <= nowDT || writeCurOnMod && curSrezMod) && writeCur)
                        {
                            WriteSrez(SrezTypes.Cur, writeCurSrezDT);
                            curSrezMod = false;
                            writeCurSrezDT = writeCurOnMod ? 
                                DateTime.MaxValue : CalcNextTime(nowDT, Settings.WriteCurPer);
                        }

                        // запись минутного среза
                        if (writeMinSrezDT <= nowDT && writeMin)
                        {
                            WriteSrez(SrezTypes.Min, writeMinSrezDT);
                            writeMinSrezDT = CalcNextTime(nowDT, Settings.WriteMinPer);
                        }

                        // запись часового среза
                        if (writeHrSrezDT <= nowDT && writeHr)
                        {
                            WriteSrez(SrezTypes.Hour, writeHrSrezDT);
                            writeHrSrezDT = CalcNextTime(nowDT, Settings.WriteHrPer);
                        }
                    }

                    // очистка устаревших данных кэша
                    if (nowDT - clearCacheDT > CacheClearSpan || nowDT < clearCacheDT /*время переведено назад*/)
                    {
                        clearCacheDT = nowDT;
                        ClearSrezTableCache(minSrezTableCache, MinCacheStorePer, MinCacheCapacity);
                        ClearSrezTableCache(hrSrezTableCache, HourCacheStorePer, HourCacheCapacity);
                    }

                    // запись информации о работе приложения
                    WriteInfo();

                    // задержка для экономиии ресурсов процессора
                    Thread.Sleep(100);
                }
            }
            finally
            {
                // выполнение действий модулей
                RaiseOnServerStop();

                // запись информации о работе приложения
                workState = WorkStateNames.Stopped;
                WriteInfo();
            }
        }

        /// <summary>
        /// Вычислить следующее время записи срезов
        /// </summary>
        /// <remarks>Период задаётся в секундах</remarks>
        private DateTime CalcNextTime(DateTime nowDT, int period)
        {
            return period > 0 ? 
                nowDT.Date.AddSeconds(((int)nowDT.TimeOfDay.TotalSeconds / period + 1) * period) : 
                nowDT;
        }

        /// <summary>
        /// Вычислить ближайшее время записи срезов
        /// </summary>
        /// <remarks>Период задаётся в секундах</remarks>
        private DateTime CalcNearestTime(DateTime dateTime, int period)
        {
            if (period > 0)
            {
                DateTime dt1 = dateTime.Date.AddSeconds((int)dateTime.TimeOfDay.TotalSeconds / period * period);
                DateTime dt2 = dt1.AddSeconds(period);
                double delta1 = Math.Abs((dateTime - dt1).TotalSeconds);
                double delta2 = Math.Abs((dateTime - dt2).TotalSeconds);
                return delta1 <= delta2 ? dt1 : dt2;
            }
            else
            {
                return dateTime;
            }
        }

        /// <summary>
        /// Получить кэш таблицы срезов, создав его при необходимости
        /// </summary>
        private SrezTableCache GetSrezTableCache(DateTime date, SrezTypes srezType)
        {
            SortedList<DateTime, SrezTableCache> srezTableCacheList;
            SrezTableCache srezTableCache;

            if (srezType == SrezTypes.Min)
                srezTableCacheList = minSrezTableCache;
            else if (srezType == SrezTypes.Hour)
                srezTableCacheList = hrSrezTableCache;
            else
                throw new ArgumentException(Localization.UseRussian ? 
                    "Недопустимый тип срезов." : "Illegal snapshot type.");

            lock (srezTableCacheList)
            {
                if (srezTableCacheList.TryGetValue(date, out srezTableCache))
                {
                    srezTableCache.AccessDT = DateTime.Now;
                }
                else
                {
                    // создание кэша таблицы срезов
                    srezTableCache = new SrezTableCache(date);
                    srezTableCacheList.Add(date, srezTableCache);
                    string path;

                    if (srezType == SrezTypes.Min)
                    {
                        path = @"Min\m" + date.ToString("yyMMdd") + ".dat";
                        if (Localization.UseRussian)
                        {
                            srezTableCache.SrezTable.Descr = "минутных срезов";
                            srezTableCache.SrezTableCopy.Descr = "копий минутных срезов";
                        }
                        else
                        {
                            srezTableCache.SrezTable.Descr = "minute data";
                            srezTableCache.SrezTableCopy.Descr = "minute data copy";
                        }
                    }
                    else
                    {
                        path = @"Hour\h" + date.ToString("yyMMdd") + ".dat";
                        if (Localization.UseRussian)
                        {
                            srezTableCache.SrezTable.Descr = "часовых срезов";
                            srezTableCache.SrezTableCopy.Descr = "копий часовых срезов";
                        }
                        else
                        {
                            {
                                srezTableCache.SrezTable.Descr = "hourly data";
                                srezTableCache.SrezTableCopy.Descr = "hourly data copy";
                            }
                        }
                    }

                    srezTableCache.SrezAdapter.FileName = Settings.ArcDir + path;
                    srezTableCache.SrezCopyAdapter.FileName = Settings.ArcCopyDir + path;
                }
            }

            return srezTableCache;
        }

        /// <summary>
        /// Очистить устаревшие данные кэша
        /// </summary>
        private void ClearSrezTableCache(SortedList<DateTime, SrezTableCache> srezTableCacheList, 
            TimeSpan storePer, int capacity)
        {
            lock (srezTableCacheList)
            {
                // удаление устаревших данных
                DateTime nowDT = DateTime.Now;
                DateTime today = nowDT.Date;
                int i = 0;

                while (i < srezTableCacheList.Count)
                {
                    SrezTableCache srezTableCache = srezTableCacheList.Values[i];
                    if (nowDT - srezTableCache.AccessDT > storePer && srezTableCache.Date != today)
                        srezTableCacheList.RemoveAt(i);
                    else
                        i++;
                }

                // удаление данных с наименьшим временем доступа, если превышена вместимость
                if (srezTableCacheList.Count > capacity)
                {
                    int cnt = srezTableCacheList.Count;
                    DateTime[] accDTs = new DateTime[cnt];
                    DateTime[] keyDates = new DateTime[cnt];
                    
                    for (int j = 0; j < cnt; j++)
                    {
                        SrezTableCache srezTableCache = srezTableCacheList.Values[j];
                        accDTs[j] = srezTableCache.AccessDT;
                        keyDates[j] = srezTableCache.Date;
                    }

                    Array.Sort(accDTs, keyDates);
                    int delCnt = cnt - capacity;

                    for (int j = 0, k = 0; j < cnt && k < delCnt; j++)
                    {
                        DateTime keyDate = keyDates[j];
                        if (keyDate != today)
                        {
                            srezTableCacheList.Remove(keyDate);
                            k++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Очистить устаревшие архивные данные
        /// </summary>
        private void ClearArchive(string dir, string pattern, DateTime arcBegDate)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);

                if (dirInfo.Exists)
                {
                    FileInfo[] files = dirInfo.GetFiles(pattern, SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in files)
                    {
                        string fileName = fileInfo.Name;
                        int year, month, day;

                        if (fileName.Length >= 7 &&
                            int.TryParse(fileName.Substring(1, 2), out year) &&
                            int.TryParse(fileName.Substring(3, 2), out month) &&
                            int.TryParse(fileName.Substring(5, 2), out day))
                        {
                            DateTime fileDate;
                            try { fileDate = new DateTime(2000 + year, month, day); }
                            catch { fileDate = DateTime.MaxValue; }

                            if (fileDate < arcBegDate)
                                fileInfo.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при очистке устаревших архивных данных: {0},\r\nДиректория: {1}" : 
                    "Error clearing outdated archive data: {0}\r\nDirectory: {1}", 
                    ex.Message, dir), Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Записать срез в таблицы срезов, выбрав нужные таблицы
        /// </summary>
        private void WriteSrez(SrezTypes srezType, DateTime srezDT)
        {
            if (srezType == SrezTypes.Cur)
            {
                // запись нового текущего среза
                if (Settings.WriteCur)
                    WriteCurSrez(curSrezAdapter, srezDT);

                if (Settings.WriteCurCopy)
                    WriteCurSrez(curSrezCopyAdapter, srezDT);
            }
            else
            {
                // определение параметров записи среза
                bool writeMain;
                bool writeCopy;
                AvgData[] avgData;

                if (srezType == SrezTypes.Min)
                {
                    writeMain = Settings.WriteMin;
                    writeCopy = Settings.WriteMinCopy;
                    avgData = minAvgData;
                }
                else // srezType == SrezTypes.Hour
                {
                    writeMain = Settings.WriteHr;
                    writeCopy = Settings.WriteHrCopy;
                    avgData = hrAvgData;
                }

                // получение кэша таблицы срезов
                SrezTableCache srezTableCache = GetSrezTableCache(srezDT.Date, srezType);

                // запись нового минутного или часового среза
                lock (srezTableCache)
                {
                    if (writeMain)
                        WriteArcSrez(srezTableCache.SrezTable, srezTableCache.SrezAdapter, srezDT, avgData);

                    if (writeCopy)
                        WriteArcSrez(srezTableCache.SrezTableCopy, srezTableCache.SrezCopyAdapter, srezDT, avgData);
                }
            }
        }

        /// <summary>
        /// Записать срез в таблицу текущего среза
        /// </summary>
        private void WriteCurSrez(SrezAdapter srezAdapter, DateTime srezDT)
        {
            string fileName = "";

            try
            {
                fileName = srezAdapter.FileName;
                srezAdapter.Create(curSrez, srezDT);

                if (Settings.DetailedLog)
                {
                    if (srezAdapter == curSrezAdapter)
                        AppLog.WriteAction(Localization.UseRussian ? 
                            "Запись среза в таблицу текущего среза завершена" :
                            "Writing snapshot in the current data table is completed", 
                            Log.ActTypes.Action);
                    else
                        AppLog.WriteAction(Localization.UseRussian ? 
                            "Запись среза в таблицу копии текущего среза завершена" :
                            "Writing snapshot in the current data copy table is completed", 
                            Log.ActTypes.Action);
                }
            }
            catch (Exception ex)
            {
                string fileNameStr = string.IsNullOrEmpty(fileName) ? "" :
                    (Localization.UseRussian ? "\r\nИмя файла: " : "\r\nFilename: ") + fileName;
                AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при записи среза в таблицу текущего среза: {0}{1}" :
                    "Error writing snapshot in the current data table: {0}{1}", 
                    ex.Message, fileNameStr), Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Записать срез в таблицу архивных (минутных или часовых) срезов
        /// </summary>
        private void WriteArcSrez(SrezTable srezTable, SrezAdapter srezAdapter, DateTime srezDT, AvgData[] avgData)
        {
            string fileName = "";

            try
            {
                // заполнение таблицы срезов, если файл изменился
                fileName = srezAdapter.FileName;
                SrezTableCache.FillSrezTable(srezTable, srezAdapter);

                // добавление копии среза в таблицу
                SrezTable.Srez newSrez = srezTable.AddSrezCopy(curSrez, srezDT);

                // запись усредняемых данных
                bool changed = false;

                foreach (int cnlInd in avgCnlInds)
                {
                    AvgData ad = avgData[cnlInd];

                    if (ad.Cnt > 0)
                    {
                        newSrez.CnlData[cnlInd] = 
                            new SrezTableLight.CnlData(ad.Sum / ad.Cnt, BaseValues.ParamStat.Defined);
                        avgData[cnlInd] = new AvgData() { Sum = 0.0, Cnt = 0 }; // сброс
                        changed = true;
                    }
                }

                // вычисление дорасчётных каналов, если добавленый срез изменился
                if (changed)
                    CalcDRCnls(drCnls, newSrez, false);

                // запись изменений таблицы срезов
                srezAdapter.Update(srezTable);
                srezTable.FileModTime = File.GetLastWriteTime(fileName);

                if (Settings.DetailedLog)
                    AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                        "Запись среза в таблицу {0} завершена" : "Writing snapshot in the {0} table is completed", 
                        srezTable.Descr), Log.ActTypes.Action);
            }
            catch (Exception ex)
            {
                string fileNameStr = string.IsNullOrEmpty(fileName) ? "" :
                    (Localization.UseRussian ? "\r\nИмя файла: " : "\r\nFilename: ") + fileName;
                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при записи среза в таблицу архивных срезов: {0}{1}" :
                    "Error writing snapshot in the archive data table: {0}{1}",
                    ex.Message, fileNameStr), Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Записать принятый срез в таблицу архивных срезов
        /// </summary>
        private bool WriteReceivedSrez(SrezTable srezTable, SrezAdapter srezAdapter,
            SrezTableLight.Srez receivedSrez, DateTime srezDT, ref SrezTableLight.Srez arcSrez)
        {
            string fileName = "";

            try
            {
                // получение существующего или создание нового архивного среза
                fileName = srezAdapter.FileName;
                SrezTableCache.FillSrezTable(srezTable, srezAdapter);
                SrezTable.Srez srez = srezTable.GetSrez(srezDT);
                bool addSrez;

                if (srez == null)
                {
                    srez = new SrezTable.Srez(srezDT, srezDescr, receivedSrez);
                    addSrez = true;
                }
                else
                {
                    addSrez = false;
                }

                if (arcSrez != null)
                    arcSrez = srez;

                // изменение архивного среза
                procSrez = srez;
                int cntCnt = receivedSrez.CnlNums.Length;

                for (int i = 0; i < cntCnt; i++)
                {
                    int cnlNum = receivedSrez.CnlNums[i];
                    int cnlInd = srez.GetCnlIndex(cnlNum);
                    InCnl inCnl;

                    if (inCnls.TryGetValue(cnlNum, out inCnl) && cnlInd >= 0 &&
                        (inCnl.CnlTypeID == BaseValues.CnlTypes.TS || inCnl.CnlTypeID == BaseValues.CnlTypes.TI))
                    {
                        // вычисление новых данных входного канала
                        SrezTableLight.CnlData oldCnlData = srez.CnlData[cnlInd];
                        SrezTableLight.CnlData newCnlData = receivedSrez.CnlData[i];
                        if (newCnlData.Stat == BaseValues.ParamStat.Defined)
                            newCnlData.Stat = BaseValues.ParamStat.Archival;
                        CalcCnlData(inCnl, oldCnlData, ref newCnlData);

                        // запись новых данных в архивный срез
                        srez.CnlData[cnlInd] = newCnlData;
                    }
                }

                // вычисление дорасчётных каналов
                CalcDRCnls(drCnls, srez, false);

                if (addSrez)
                    srezTable.AddSrez(srez);
                else
                    srezTable.MarkSrezAsModified(srez);

                // запись изменений таблицы срезов
                srezAdapter.Update(srezTable);
                srezTable.FileModTime = File.GetLastWriteTime(fileName);
                return true;
            }
            catch (Exception ex)
            {
                string fileNameStr = string.IsNullOrEmpty(fileName) ? "" :
                    (Localization.UseRussian ? "\r\nИмя файла: " : "\r\nFilename: ") + fileName;
                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при записи принятого срезы в таблицу архивных срезов: {0}{1}" :
                    "Error writing received snapshot in the archive data table: {0}{1}",
                    ex.Message, fileNameStr), Log.ActTypes.Exception);
                return false;
            }
            finally
            {
                procSrez = null;
            }
        }

        /// <summary>
        /// Записать событие в таблицу событий
        /// </summary>
        private bool WriteEvent(string tableName, EventAdapter eventAdapter, EventTableLight.Event ev)
        {
            string fileName = "";

            try
            {
                lock (eventAdapter)
                {
                    eventAdapter.TableName = tableName;
                    fileName = eventAdapter.FileName;
                    eventAdapter.AppendEvent(ev);

                    if (Settings.DetailedLog)
                    {
                        string tableDescr = eventAdapter == this.eventAdapter ?
                            (Localization.UseRussian ? "событий" : "event") :
                            (Localization.UseRussian ? "копий событий" : "event copy");
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Запись события в таблицу {0} завершена" : "Writing event in the {0} table is completed",
                            tableDescr), Log.ActTypes.Action);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                string fileNameStr = string.IsNullOrEmpty(fileName) ? "" :
                    (Localization.UseRussian ? "\r\nИмя файла: " : "\r\nFilename: ") + fileName;
                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при записи события в таблицу событий: {0}{1}" :
                    "Error writing event in the event table: {0}{1}",
                    ex.Message, fileNameStr), Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Записать событие в таблицы событий в соответствии с настройками, выполнить действия модулей
        /// </summary>
        private bool WriteEvent(EventTableLight.Event ev)
        {
            // выполнение действий модулей до записи
            RaiseOnEventCreating(ev);

            // запись событий
            string tableName = "e" + ev.DateTime.ToString("yyMMdd") + ".dat";
            bool writeOk1 = Settings.WriteEv ? WriteEvent(tableName, eventAdapter, ev) : true;
            bool writeOk2 = Settings.WriteEvCopy ? WriteEvent(tableName, eventCopyAdapter, ev) : true;
            
            // выполнение действий модулей после записи
            RaiseOnEventCreated(ev);

            return writeOk1 && writeOk2;
        }

        /// <summary>
        /// Записать квитирование события в таблицу событий
        /// </summary>
        private bool WriteEventCheck(string tableName, EventAdapter eventAdapter, int evNum, int userID)
        {
            string fileName = "";

            try
            {
                lock (eventAdapter)
                {
                    eventAdapter.TableName = tableName;
                    fileName = eventAdapter.FileName;
                    eventAdapter.CheckEvent(evNum, userID);

                    if (Settings.DetailedLog)
                    {
                        string tableDescr = eventAdapter == this.eventAdapter ?
                            (Localization.UseRussian ? "событий" : "event") :
                            (Localization.UseRussian ? "копий событий" : "event copy");
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Запись квитирования события в таблицу {0} завершена" : 
                            "Writing event check in the {0} table is completed", tableDescr), Log.ActTypes.Action);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                string fileNameStr = string.IsNullOrEmpty(fileName) ? "" :
                    (Localization.UseRussian ? "\r\nИмя файла: " : "\r\nFilename: ") + fileName;
                AppLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при записи квитирования события в таблицу событий: {0}{1}" :
                    "Error writing event check in the event table: {0}{1}",
                    ex.Message, fileNameStr), Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Вычислить данные входного канала
        /// </summary>
        private void CalcCnlData(InCnl inCnl, SrezTableLight.CnlData oldCnlData, ref SrezTableLight.CnlData newCnlData)
        {
            if (inCnl != null)
            {
                try
                {
                    // вычисление новых данных
                    if (inCnl.CalcCnlData != null)
                    {
                        lock (calculator)
                            inCnl.CalcCnlData(ref newCnlData);
                    }

                    // увеличение счётчика количества переключений
                    if (inCnl.CnlTypeID == BaseValues.CnlTypes.SWCNT && 
                        newCnlData.Stat > BaseValues.ParamStat.Undefined)
                    {
                        bool even = (int)oldCnlData.Val % 2 == 0; // старое значение чётное
                        newCnlData.Val = newCnlData.Val < 0 && even || newCnlData.Val >= 0 && !even ? 
                            Math.Truncate(oldCnlData.Val) + 1 : Math.Truncate(oldCnlData.Val);
                    }

                    // корректировка нового статуса, если задана проверка границ значения
                    if (newCnlData.Stat == BaseValues.ParamStat.Defined &&
                        (inCnl.LimLow < inCnl.LimHigh || inCnl.LimLowCrash < inCnl.LimHighCrash))
                    {
                        newCnlData.Stat = BaseValues.ParamStat.Normal;

                        if (inCnl.LimLow < inCnl.LimHigh)
                        {
                            if (newCnlData.Val < inCnl.LimLow)
                                newCnlData.Stat = BaseValues.ParamStat.Low;
                            else if (newCnlData.Val > inCnl.LimHigh)
                                newCnlData.Stat = BaseValues.ParamStat.High;
                        }

                        if (inCnl.LimLowCrash < inCnl.LimHighCrash)
                        {
                            if (newCnlData.Val < inCnl.LimLowCrash)
                                newCnlData.Stat = BaseValues.ParamStat.LowCrash;
                            else if (newCnlData.Val > inCnl.LimHighCrash)
                                newCnlData.Stat = BaseValues.ParamStat.HighCrash;
                        }
                    }
                }
                catch
                {
                    newCnlData.Stat = BaseValues.ParamStat.FormulaError;
                }
            }
        }

        /// <summary>
        /// Генерировать событие в соответствии со свойствами и данными входного канала
        /// </summary>
        private void GenEvent(InCnl inCnl, SrezTableLight.CnlData oldCnlData, SrezTableLight.CnlData newCnlData)
        {
            if (inCnl.EvEnabled)
            {
                double oldVal = oldCnlData.Val;
                double newVal = newCnlData.Val;
                int oldStat = oldCnlData.Stat;
                int newStat = newCnlData.Stat;

                bool dataHasChanged = 
                    oldStat > BaseValues.ParamStat.Undefined && newStat > BaseValues.ParamStat.Undefined &&
                    (oldVal != newVal || oldStat != newStat);

                if (// события по изменению
                    inCnl.EvOnChange && dataHasChanged || 
                    // события по неопределённому состоянию и выходу из него
                    inCnl.EvOnUndef && 
                    (oldStat > BaseValues.ParamStat.Undefined && newStat == BaseValues.ParamStat.Undefined || 
                    oldStat == BaseValues.ParamStat.Undefined && newStat > BaseValues.ParamStat.Undefined) ||
                    // события нормализации, занижения и завышения
                    (newStat == BaseValues.ParamStat.Normal || newStat == BaseValues.ParamStat.LowCrash || 
                    newStat == BaseValues.ParamStat.Low || newStat == BaseValues.ParamStat.High || 
                    newStat == BaseValues.ParamStat.HighCrash) && oldStat != newStat)
                {
                    // создание события
                    EventTableLight.Event ev = new EventTableLight.Event();
                    ev.DateTime = DateTime.Now;
                    ev.ObjNum = inCnl.ObjNum;
                    ev.KPNum = inCnl.KPNum;
                    ev.ParamID = inCnl.ParamID;
                    ev.CnlNum = inCnl.CnlNum;
                    ev.OldCnlVal = oldCnlData.Val;
                    ev.OldCnlStat = oldStat;
                    ev.NewCnlVal = newCnlData.Val;
                    ev.NewCnlStat = dataHasChanged && oldStat == BaseValues.ParamStat.Defined && 
                        newStat == BaseValues.ParamStat.Defined ? BaseValues.ParamStat.Changed : newStat;

                    // запись события и выполнение действий модулей
                    WriteEvent(ev);
                }
            }
        }

        /// <summary>
        /// Вычислить дорасчётные каналы
        /// </summary>
        private void CalcDRCnls(List<InCnl> inCnls, SrezTableLight.Srez srez, bool genEvents)
        {
            try
            {
                procSrez = srez;

                foreach (InCnl inCnl in inCnls)
                {
                    int cnlInd = srez.GetCnlIndex(inCnl.CnlNum);

                    if (cnlInd >= 0)
                    {
                        // вычисление новых данных входного канала
                        SrezTableLight.CnlData oldCnlData = srez.CnlData[cnlInd];
                        SrezTableLight.CnlData newCnlData =
                            new SrezTableLight.CnlData(oldCnlData.Val, BaseValues.ParamStat.Defined);
                        CalcCnlData(inCnl, oldCnlData, ref newCnlData);

                        // запись новых данных в срез
                        srez.CnlData[cnlInd] = newCnlData;

                        // генерация события
                        if (genEvents)
                            GenEvent(inCnl, oldCnlData, newCnlData);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? "Ошибка при вычислении дорасчётных каналов: " :
                    "Error calculating channels: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                procSrez = null;
            }
        }

        /// <summary>
        /// Установить недостоверность неактивных каналов
        /// </summary>
        private void SetUnreliable()
        {
            if (Settings.InactUnrelTime > 0)
            {
                TimeSpan inactUnrelSpan = TimeSpan.FromMinutes(Settings.InactUnrelTime);
                DateTime nowDT = DateTime.Now;
                int cnlCnt = srezDescr.CnlNums.Length;

                for (int i = 0; i < cnlCnt; i++)
                {
                    InCnl inCnl = inCnls.Values[i];
                    int cnlTypeID = inCnl.CnlTypeID;

                    if ((cnlTypeID == BaseValues.CnlTypes.TS || cnlTypeID == BaseValues.CnlTypes.TI) &&
                        curSrez.CnlData[i].Stat > BaseValues.ParamStat.Undefined && 
                        nowDT - activeDTs[i] > inactUnrelSpan)
                    {
                        // установка недостоверного статуса
                        SrezTableLight.CnlData oldCnlData = curSrez.CnlData[i];
                        SrezTableLight.CnlData newCnlData = 
                            new SrezTableLight.CnlData(oldCnlData.Val, BaseValues.ParamStat.Unreliable);
                        curSrez.CnlData[i] = newCnlData;
                        curSrezMod = true;

                        // генерация события
                        GenEvent(inCnl, oldCnlData, newCnlData);
                    }
                }
            }
        }

        /// <summary>
        /// Записать в файл информацию о работе приложения
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                {
                    TimeSpan workSpan = DateTime.Now - startDT;
                    writer.WriteLine(AppInfoFormat,
                        startDT.ToLocalizedString(),
                        workSpan.Days > 0 ? workSpan.ToString(@"d\.hh\:mm\:ss") : workSpan.ToString(@"hh\:mm\:ss"),
                        workState,
                        AppVersionHi + "." + AppVersionLo);
                    writer.WriteLine();
                    writer.Write(comm.GetClientsInfo());
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при записи в файл информации о работе приложения: " :
                    "Error writing application information to the file: ") + ex.Message, Log.ActTypes.Exception);
            }
        }


        /// <summary>
        /// Вызвать событие OnServerStart для модулей
        /// </summary>
        private void RaiseOnServerStart()
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnServerStart();
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий при запуске работы сервера в модуле {0}: {1}" : 
                            "Error executing actions on server start in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnServerStop для модулей
        /// </summary>
        private void RaiseOnServerStop()
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnServerStop();
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий при остановке работы сервера в модуле {0}: {1}" :
                            "Error executing actions on server stop in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnCurDataProcessed для модулей
        /// </summary>
        private void RaiseOnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnCurDataProcessed(cnlNums, curSrez);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий после обработки новых текущих данных в модуле {0}: {1}" :
                            "Error executing actions on current data processed in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnCurDataCalculated для модулей
        /// </summary>
        private void RaiseOnCurDataCalculated(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnCurDataCalculated(cnlNums, curSrez);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при выполнении действий " + 
                            "после вычисления дорасчётных каналов текущего среза в модуле {0}: {1}" :
                            "Error executing actions on current data calculated in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnArcDataProcessed для модулей
        /// </summary>
        private void RaiseOnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnArcDataProcessed(cnlNums, arcSrez);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий после обработки новых архивных данных в модуле {0}: {1}" :
                            "Error executing actions on archive data processed in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnEventCreating для модулей
        /// </summary>
        private void RaiseOnEventCreating(EventTableLight.Event ev)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnEventCreating(ev);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий при создании события в модуле {0}: {1}" :
                            "Error executing actions on event creating in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnEventCreated для модулей
        /// </summary>
        private void RaiseOnEventCreated(EventTableLight.Event ev)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnEventCreated(ev);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий после создания события в модуле {0}: {1}" :
                            "Error executing actions on event created in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnEventChecked для модулей
        /// </summary>
        private void RaiseOnEventChecked(DateTime date, int evNum, int userID)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnEventChecked(date, evNum, userID);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий после квитирования события в модуле {0}: {1}" :
                            "Error executing actions on event checked in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Вызвать событие OnCommandReceived для модулей
        /// </summary>
        private void RaiseOnCommandReceived(int ctrlCnlNum, ModLogic.Command cmd, int userID, ref bool passToClients)
        {
            lock (modules)
            {
                foreach (ModLogic modLogic in modules)
                {
                    try
                    {
                        modLogic.OnCommandReceived(ctrlCnlNum, cmd, userID, ref passToClients);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ?
                            "Ошибка при выполнении действий после приёма команды ТУ в модуле {0}: {1}" :
                            "Error executing actions on command received in module {0}: {1}",
                            modLogic.Name, ex.Message), Log.ActTypes.Exception);
                    }
                }
            }
        }


        /// <summary>
        /// Инициализировать директории приложения
        /// </summary>
        public void InitAppDirs(out bool dirsExist, out bool logDirExists)
        {
            ConfigDir = ExeDir + "Config\\";
            LangDir = ExeDir + "Lang\\";
            LogDir = ExeDir + "Log\\";
            ModDir = ExeDir + "Mod\\";

            AppLog.FileName = LogDir + LogFileName;
            infoFileName = LogDir + InfoFileName;
            logDirExists = Directory.Exists(LogDir);
            dirsExist = Directory.Exists(ConfigDir) && Directory.Exists(LangDir) && 
                logDirExists && Directory.Exists(ModDir);
        }

        /// <summary>
        /// Запустить работу сервера
        /// </summary>
        public bool Start()
        {
            try
            {
                // остановка работы
                Stop();

                // запуск работы
                startDT = DateTime.Now;
                string errMsg;

                if (Settings.Load(ConfigDir + Settings.DefFileName, out errMsg))
                {
                    LoadModules();

                    if (CheckDataDirs() && CheckBaseFiles() && ReadBase() && InitCalculator() && comm.Start())
                    {
                        AppLog.WriteAction(Localization.UseRussian ? "Запуск работы сервера" : "Start server", 
                            Log.ActTypes.Action);
                        terminated = false;
                        serverIsReady = false;
                        thread = new Thread(new ThreadStart(Execute));
                        thread.Start();
                    }
                }
                else
                {
                    AppLog.WriteAction(errMsg, Log.ActTypes.Error);
                }

                return thread != null;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при запуске работы сервера: " : "Error start server: ") +
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
            finally
            {
                if (thread == null)
                {
                    workState = WorkStateNames.Error;
                    WriteInfo();
                }
            }
        }

        /// <summary>
        /// Остановить работу сервера
        /// </summary>
        public void Stop()
        {
            try
            {
                // остановка взаимодействия с клиентами
                comm.Stop();

                // остановка потока работы сервера
                if (thread != null)
                {
                    serverIsReady = false;
                    terminated = true;

                    if (thread.Join(WaitForStop))
                    {
                        AppLog.WriteAction(Localization.UseRussian ? "Работа сервера остановлена" :
                            "Server is stopped", Log.ActTypes.Action);
                    }
                    else
                    {
                        thread.Abort();
                        AppLog.WriteAction(Localization.UseRussian ? "Работа сервера прервана" : "Server is aborted", 
                            Log.ActTypes.Action);
                    }

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                workState = WorkStateNames.Error;
                WriteInfo();
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при остановке работы сервера: " : "Error stop server: ") + 
                    ex.Message, Log.ActTypes.Exception);
            }
        }


        /// <summary>
        /// Получить канал управления по идентификатору
        /// </summary>
        public CtrlCnl GetCtrlCnl(int ctrlCnlNum)
        {
            lock (ctrlCnls)
            {
                CtrlCnl ctrlCnl;
                return ctrlCnls.TryGetValue(ctrlCnlNum, out ctrlCnl) ? ctrlCnl.Clone() : null;
            }
        }

        /// <summary>
        /// Получить пользователя по имени
        /// </summary>
        public User GetUser(string userName)
        {
            lock (users)
            {
                User user;
                return users.TryGetValue(userName.Trim().ToLower(), out user) ? user.Clone() : null;
            }
        }

        /// <summary>
        /// Получить таблицу срезов, содержащую данные заданных каналов
        /// </summary>
        public SrezTableLight GetSrezTable(DateTime date, SrezTypes srezType, int[] cnlNums)
        {
            try
            {
                SrezTableLight destSrezTable;
                int cnlNumsLen = cnlNums == null ? 0 : cnlNums.Length;

                if (serverIsReady && cnlNumsLen > 0)
                {
                    // получение кэша таблицы срезов
                    SrezTableCache srezTableCache = GetSrezTableCache(date.Date, srezType);

                    lock (srezTableCache)
                    {
                        // заполнение таблицы срезов в кэше
                        srezTableCache.FillSrezTable();

                        // создание новой таблицы срезов и копирование в неё данных заданных каналов
                        SrezTable srcSrezTable = srezTableCache.SrezTable;
                        SrezTable.SrezDescr prevSrezDescr = null;
                        int[] cnlNumIndexes = new int[cnlNumsLen];
                        destSrezTable = new SrezTableLight();

                        foreach (SrezTable.Srez srcSrez in srcSrezTable.SrezList.Values)
                        {
                            // определение индексов каналов
                            if (!srcSrez.SrezDescr.Equals(prevSrezDescr))
                            {
                                for (int i = 0; i < cnlNumsLen; i++)
                                    cnlNumIndexes[i] = Array.BinarySearch<int>(srcSrez.CnlNums, cnlNums[i]);
                            }

                            // создание и заполнение среза, содержащего заданные каналы
                            SrezTableLight.Srez destSrez = new SrezTableLight.Srez(srcSrez.DateTime, cnlNumsLen);

                            for (int i = 0; i < cnlNumsLen; i++)
                            {
                                destSrez.CnlNums[i] = cnlNums[i];
                                int cnlNumInd = cnlNumIndexes[i];
                                destSrez.CnlData[i] = cnlNumInd < 0 ?
                                    SrezTableLight.CnlData.Empty : srcSrez.CnlData[cnlNumInd];
                            }

                            destSrezTable.SrezList.Add(destSrez.DateTime, destSrez);
                            prevSrezDescr = srcSrez.SrezDescr;
                        }
                    }
                }
                else
                {
                    destSrezTable = null;
                }

                return destSrezTable;
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при получении таблицы срезов: " : "Error getting snapshot table: ") + 
                    ex.Message, Log.ActTypes.Exception);
                return null;
            }
        }

        /// <summary>
        /// Обработать новые текущие данные
        /// </summary>
        public bool ProcCurData(SrezTableLight.Srez receivedSrez)
        {
            try
            {
                if (serverIsReady)
                {
                    lock (curSrez)
                    {
                        procSrez = curSrez;
                        int cnlCnt = receivedSrez == null ? 0 : receivedSrez.CnlNums.Length;

                        for (int i = 0; i < cnlCnt; i++)
                        {
                            int cnlNum = receivedSrez.CnlNums[i];
                            int cnlInd = curSrez.GetCnlIndex(cnlNum);
                            InCnl inCnl;

                            if (inCnls.TryGetValue(cnlNum, out inCnl) && cnlInd >= 0) // входной канал существует
                            {
                                if (inCnl.CnlTypeID == BaseValues.CnlTypes.TS || 
                                    inCnl.CnlTypeID == BaseValues.CnlTypes.TI)
                                {
                                    // вычисление новых данных входного канала
                                    SrezTableLight.CnlData oldCnlData = curSrez.CnlData[cnlInd];
                                    SrezTableLight.CnlData newCnlData = receivedSrez.CnlData[i];
                                    CalcCnlData(inCnl, oldCnlData, ref newCnlData);

                                    // расчёт данных для усреднения
                                    if (inCnl.Averaging && newCnlData.Stat > BaseValues.ParamStat.Undefined &&
                                        newCnlData.Stat != BaseValues.ParamStat.FormulaError &&
                                        newCnlData.Stat != BaseValues.ParamStat.Unreliable)
                                    {
                                        minAvgData[cnlInd].Sum += newCnlData.Val;
                                        minAvgData[cnlInd].Cnt++;
                                        hrAvgData[cnlInd].Sum += newCnlData.Val;
                                        hrAvgData[cnlInd].Cnt++;
                                    }

                                    // запись новых данных в текущий срез
                                    curSrez.CnlData[cnlInd] = newCnlData;

                                    // генерация события
                                    GenEvent(inCnl, oldCnlData, newCnlData);

                                    // обновление информации об активности канала
                                    activeDTs[cnlInd] = DateTime.Now;
                                }
                                else if (inCnl.CnlTypeID != BaseValues.CnlTypes.TSDR && 
                                    inCnl.CnlTypeID != BaseValues.CnlTypes.TIDR)
                                {
                                    // запись новых данных минутных и часовых каналов, а также
                                    // количества переключений в текущий срез без вычислений
                                    curSrez.CnlData[cnlInd] = receivedSrez.CnlData[i];
                                }
                            }
                        }

                        // выполнение действий модулей
                        if (cnlCnt > 0)
                            RaiseOnCurDataProcessed(receivedSrez.CnlNums, curSrez);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при обработке новых текущих данных: " : "Error processing the new current data: ") + 
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
            finally
            {
                procSrez = null;
                curSrezMod = true;
            }
        }

        /// <summary>
        /// Обработать новые архивные данные
        /// </summary>
        public bool ProcArcData(SrezTableLight.Srez receivedSrez)
        {
            try
            {
                if (serverIsReady)
                {
                    bool result = true;
                    int cnlCnt = receivedSrez == null ? 0 : receivedSrez.CnlNums.Length;

                    if (cnlCnt > 0)
                    {
                        // определение времени, на которое записывются архивные данные
                        DateTime paramSrezDT = receivedSrez.DateTime;
                        DateTime paramSrezDate = paramSrezDT.Date;
                        DateTime nearestMinDT = CalcNearestTime(paramSrezDT, Settings.WriteMinPer);
                        DateTime nearestHrDT = CalcNearestTime(paramSrezDT, Settings.WriteHrPer);

                        // получение кэша таблиц срезов
                        SrezTableCache minCache = Settings.WriteMin || Settings.WriteMinCopy ?
                            GetSrezTableCache(paramSrezDate, SrezTypes.Min) : null;
                        SrezTableCache hrCache = 
                            nearestHrDT == paramSrezDT && (Settings.WriteHr || Settings.WriteHrCopy) ?
                                GetSrezTableCache(paramSrezDate, SrezTypes.Hour) : null;
                        SrezTableLight.Srez arcSrez = null;

                        // запись минутных данных
                        if (minCache != null)
                        {
                            lock (minCache)
                            {
                                if (Settings.WriteMin && !WriteReceivedSrez(minCache.SrezTable, 
                                    minCache.SrezAdapter, receivedSrez, nearestMinDT, ref arcSrez))
                                    result = false;

                                if (Settings.WriteMinCopy && !WriteReceivedSrez(minCache.SrezTableCopy, 
                                    minCache.SrezCopyAdapter, receivedSrez, nearestMinDT, ref arcSrez))
                                    result = false;
                            }
                        }

                        // запись часовых данных
                        if (hrCache != null)
                        {
                            lock (hrCache)
                            {
                                if (Settings.WriteHr && !WriteReceivedSrez(hrCache.SrezTable, 
                                    hrCache.SrezAdapter, receivedSrez, nearestHrDT, ref arcSrez))
                                    result = false;

                                if (Settings.WriteHrCopy && !WriteReceivedSrez(hrCache.SrezTableCopy, 
                                    hrCache.SrezCopyAdapter, receivedSrez, nearestHrDT, ref arcSrez))
                                    result = false;
                            }
                        }

                        // выполнение действий модулей
                        RaiseOnArcDataProcessed(receivedSrez.CnlNums, arcSrez);
                    }

                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при обработке новых архивных данных: " : "Error processing the new archive data: ") +
                    ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Получить данные входного канала среза, обрабатываемого в настоящий момент
        /// </summary>
        public SrezTableLight.CnlData GetCnlData(int cnlNum)
        {
            SrezTableLight.CnlData cnlData = SrezTableLight.CnlData.Empty;

            try
            {
                if (procSrez != null)
                    procSrez.GetCnlData(cnlNum, out cnlData);
            }
            catch (Exception ex)
            {
                AppLog.WriteAction((Localization.UseRussian ?
                    "Ошибка при получении данных входного канала среза, обрабатываемого в настоящий момент: " :
                    "Error getting the input channel data of the snapshot being processed at the moment: ") + 
                    ex.Message, Log.ActTypes.Exception);
            }

            return cnlData;
        }

        /// <summary>
        /// Обработать новое событие
        /// </summary>
        public bool ProcEvent(EventTableLight.Event ev)
        {
            if (serverIsReady)
                return ev == null ? true : WriteEvent(ev);
            else
                return false;
        }

        /// <summary>
        /// Квитировать событие
        /// </summary>
        public bool CheckEvent(DateTime date, int evNum, int userID)
        {
            if (serverIsReady)
            {
                // запись квитирования события
                string tableName = "e" + date.ToString("yyMMdd") + ".dat";
                bool writeOk1 = Settings.WriteEv ? 
                    WriteEventCheck(tableName, eventAdapter, evNum, userID) : true;
                bool writeOk2 = Settings.WriteEvCopy ? 
                    WriteEventCheck(tableName, eventCopyAdapter, evNum, userID) : true;

                // выполнение действий модулей
                RaiseOnEventChecked(date, evNum, userID);

                return writeOk1 && writeOk2;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Обработать команду ТУ
        /// </summary>
        public void ProcCommand(CtrlCnl ctrlCnl, ModLogic.Command cmd, int userID, out bool passToClients)
        {
            if (!serverIsReady || ctrlCnl == null)
            {
                passToClients = false;
            }
            else
            {
                int ctrlCnlNum = ctrlCnl.CtrlCnlNum;

                // вычисление значения команды
                if (ctrlCnl.CalcCmdVal != null)
                {
                    try
                    {
                        double cmdVal = cmd.CmdVal;
                        lock (calculator)
                            ctrlCnl.CalcCmdVal(ref cmdVal);
                        cmd.CmdVal = cmdVal;
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteAction(string.Format(Localization.UseRussian ? 
                            "Ошибка при вычислении значения команды для канала управления {0}: {1}" : 
                            "Error calculating command value for the output channel {0}: {1}", 
                            ctrlCnlNum, ex.Message), Log.ActTypes.Error);
                        cmd.CmdVal = double.NaN;
                    }
                }

                // выполнение действий модулей после приёма команды
                passToClients = !double.IsNaN(cmd.CmdVal);
                RaiseOnCommandReceived(ctrlCnlNum, cmd, userID, ref passToClients);

                // создание события
                if (passToClients && ctrlCnl.EvEnabled)
                {
                    EventTableLight.Event ev = new EventTableLight.Event();
                    ev.DateTime = DateTime.Now;
                    ev.ObjNum = ctrlCnl.ObjNum;
                    ev.KPNum = ctrlCnl.KPNum;
                    ev.Descr = string.Format(EventOnCmdFormat, ctrlCnlNum, cmd.CmdVal, userID);

                    // запись события и выполнение действий модулей
                    WriteEvent(ev);
                }
            }
        }
    }
}