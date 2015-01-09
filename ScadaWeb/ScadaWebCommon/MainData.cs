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
 * Module   : ScadaWebCommon
 * Summary  : Retrieve data from the configuration database, snapshot tables and events
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using Scada.Client;
using Scada.Data;
using Utils;

namespace Scada.Web
{
	/// <summary>
    /// Retrieve data from the configuration database, snapshot tables and events
    /// <para>Получение данных из базы конфигурации, таблиц срезов и событий</para>
	/// </summary>
	public class MainData
	{
        /// <summary>
        /// Событие в удобной для отображения форме
        /// </summary>
        public class EventView
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public EventView()
            {
                Num = "";
                Date = "";
                Time = "";
                Obj = "";
                KP = "";
                Cnl = "";
                Text = "";
                Check = false;
                User = "";
                Color = "black";
                Sound = false;
            }

            /// <summary>
            /// Получить или установить порядковый номер события в файле
            /// </summary>
            public string Num { get; set; }
            /// <summary>
            /// Получить или установить дату события
            /// </summary>
            public string Date { get; set; }
            /// <summary>
            /// Получить или установить время события
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// Получить или установить объект
            /// </summary>
            public string Obj { get; set; }
            /// <summary>
            /// Получить или установить объект
            /// </summary>
            public string KP { get; set; }
            /// <summary>
            /// Получить или установить канал
            /// </summary>
            public string Cnl { get; set; }
            /// <summary>
            /// Получить или установить текст события
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Получить или установить признак квитирования
            /// </summary>
            public bool Check { get; set; }
            /// <summary>
            /// Получить или установить имя пользователя, квитировавшего событие
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// Получить или установить цвет
            /// </summary>
            public string Color { get; set; }
            /// <summary>
            /// Получить или установить признак звука события
            /// </summary>
            public bool Sound { get; set; }
        }

        /// <summary>
        /// Права на доступ к объектам интерфейса
        /// </summary>
        public struct Right
        {
            /// <summary>
            /// Отсутствие прав
            /// </summary>
            public static readonly Right NoRights = new Right(false, false);

            /// <summary>
            /// Конструктор
            /// </summary>
            public Right(bool viewRight, bool ctrlRight)
                : this()
            {
                ViewRight = viewRight;
                CtrlRight = ctrlRight;
            }

            /// <summary>
            /// Получить или установить право на просмотр
            /// </summary>
            public bool ViewRight { get; set; }
            /// <summary>
            /// Получить или установить право на управление
            /// </summary>
            public bool CtrlRight { get; set; }
        }


		/// <summary>
        /// Время актуальности данных базы конфигурации, с
		/// </summary>
		public const int BaseValidTime = 1;
		/// <summary>
		/// Время актуальности данных текущего среза, с
		/// </summary>
		public const int CurSrezValidTime = 1;
		/// <summary>
		/// Время отображения данных текущего среза, мин
		/// </summary>
		public const int CurSrezShowTime = 15;
		/// <summary>
		/// Время актуальности данных часовых срезов, с
		/// </summary>
		public const int HourSrezValidTime = 5;
		/// <summary>
		/// Время актуальности данных минутных срезов, с
		/// </summary>
		public const int MinSrezValidTime = 5;
        /// <summary>
        /// Время актуальности данных событий, с
        /// </summary>
        public const int EventValidTime = 1;

        /// <summary>
        /// Размер списка таблиц часовых срезов для кэширования
        /// </summary>
        public const int HourCacheSize = 5;
        /// <summary>
        /// Размер списка таблиц событий для кэширования
        /// </summary>
        public const int EventCacheSize = 5;


        ServerComm serverComm;           // объект для обмена данными со SCADA-Сервером
        private string settFileName;     // полное имя файла настроек соединения со SCADA-Сервером
        private DateTime settModTime;    // время последнего изменения файла настроек

        private SrezTableLight tblCur;             // таблица текущего среза
        private SrezTableLight[] hourTableCache;   // массив таблиц часовых срезов для кэширования
        private EventTableLight[] eventTableCache; // массив таблиц событий для кэширования
        private int hourTableIndex;                // кольцевой индекс для добавления таблиц часовых срезов в кэш
        private int eventTableIndex;               // кольцевой индекс для добавления таблиц событий в кэш
        private Trend trend;                       // последний полученный минутный тренд
        private NumberFormatInfo nfi;              // формат вещественных чисел
        private string defDecSep;                  // разделитель дробной части по умолчанию
        private string defGrSep;                   // разделитель групп цифр по умолчанию

        private DateTime baseModTime;    // время последнего изменения успешно считанной базы конфигурации
        private DateTime baseFillTime;   // время последего успешного заполнения таблиц базы конфигурации
		private DataTable tblInCnl;      // таблица входных каналов
        private DataTable tblCtrlCnl;    // таблица каналов управления
        private DataTable tblObj;        // таблица объектов
        private DataTable tblKP;         // таблица КП
        private DataTable tblRole;       // таблица ролей
        private DataTable tblUser;       // таблица пользователей
        private DataTable tblInterface;  // таблица объектов интерфейса
        private DataTable tblRight;      // таблица прав на объекты интерфейса
        private DataTable tblEvType;     // таблица типов событий
        private DataTable tblParam;      // таблица параметров
		private DataTable tblUnit;       // таблица размерностей
        private DataTable tblCmdVal;     // таблица значений команд
        private DataTable tblFormat;     // таблица форматов чисел
        private DataTable[] baseTblArr;  // массив ссылок на таблицы базы конфигурации

		private CnlProps[] cnlPropsArr;  // массив свойств входных каналов
        private int maxCnlCnt;           // максимальное количество входных каналов

        private Object refrLock;         // объект для синхронизации обновления данных
        private Object baseLock;         // объект для синхронизации обращения к таблицам базы конфигурации
        private Object cnlPropLock;      // объект для синхронизации получения свойств входного канала
        private Object cnlDataLock;      // объект для синхронизации получения данных входного канала
        private Object eventLock;        // объект для синхронизации получения событий
        

        /// <summary>
        /// Конструктор
        /// </summary>
		public MainData()
		{
            serverComm = null;
            settFileName = "";
            settModTime = DateTime.MinValue;

            tblCur = new SrezTableLight();
            hourTableCache = new SrezTableLight[HourCacheSize];
            eventTableCache = new EventTableLight[EventCacheSize];
            for (int i = 0; i < HourCacheSize; i++)
                hourTableCache[i] = null;
            for (int i = 0; i < EventCacheSize; i++)
                eventTableCache[i] = null;
            hourTableIndex = 0;
            eventTableIndex = 0;
            trend = null;
            nfi = new NumberFormatInfo();
            defDecSep = Localization.Culture.NumberFormat.NumberDecimalSeparator;
            defGrSep = Localization.Culture.NumberFormat.NumberGroupSeparator;

            baseModTime = DateTime.MinValue;
            baseFillTime = DateTime.MinValue;
            tblInCnl = new DataTable("InCnl");
            tblCtrlCnl = new DataTable("CtrlCnl");
            tblObj = new DataTable("Obj");
            tblKP = new DataTable("KP");
            tblRole = new DataTable("Role");
            tblUser = new DataTable("User");
            tblInterface = new DataTable("Interface");
            tblRight = new DataTable("Right");
            tblEvType = new DataTable("EvType");
            tblParam = new DataTable("Param");
            tblUnit = new DataTable("Unit");
            tblCmdVal = new DataTable("CmdVal");
            tblFormat = new DataTable("Format");

            baseTblArr = new DataTable[13];
            baseTblArr[0] = tblInCnl;
            baseTblArr[1] = tblCtrlCnl;
            baseTblArr[2] = tblObj;
            baseTblArr[3] = tblKP;
            baseTblArr[4] = tblRole;
            baseTblArr[5] = tblUser;
            baseTblArr[6] = tblInterface;
            baseTblArr[7] = tblRight;
            baseTblArr[8] = tblEvType;
            baseTblArr[9] = tblParam;
            baseTblArr[10] = tblUnit;
            baseTblArr[11] = tblCmdVal;
            baseTblArr[12] = tblFormat;

			cnlPropsArr = null;
            maxCnlCnt = 0;

            refrLock = new Object();
            baseLock = new Object();
            cnlPropLock = new Object();
            cnlDataLock = new Object();
            eventLock = new Object();
        }


        /// <summary>
        /// Получить объект для обмена данными со SCADA-Сервером
        /// </summary>
        public ServerComm ServerComm
        {
            get
            {
                RefrServerComm();
                return serverComm;
            }
        }

        /// <summary>
        /// Имя файла настроек соединения со SCADA-Сервером
        /// </summary>
        public string SettingsFileName
        {
            get
            {
                return settFileName;
            }
            set
            {
                settFileName = value;
            }
        }

		/// <summary>
		/// Массив свойств входных каналов
		/// </summary>
		public CnlProps[] CnlPropsArr
		{
			get
			{
				return cnlPropsArr;
			}
		}


        /// <summary>
		/// Дата и время последней записи в файл, при отсутствии файла - минимальная дата
		/// </summary>
		private DateTime GetLastWriteTime(string path)
		{
            if (!File.Exists(path))
                return DateTime.MinValue;

			try
			{
				return File.GetLastWriteTime(path);
			}
			catch
			{
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Заполнить свойства входных каналов
		/// </summary>
		private void FillCnlProps()
		{
            Monitor.Enter(baseLock);
            AppData.Log.WriteAction(Localization.UseRussian ? "Заполнение свойств входных каналов" : 
                "Fill input channels properties", Log.ActTypes.Action);

            try
            {
                int inCnlCnt = tblInCnl.Rows.Count; // количество входных каналов

                if (inCnlCnt == 0)
                {
                    cnlPropsArr = null;
                }
                else
                {
                    if (0 < maxCnlCnt && maxCnlCnt < inCnlCnt)
                        inCnlCnt = maxCnlCnt;
                    CnlProps[] newCnlPropsArr = new CnlProps[inCnlCnt];

                    for (int i = 0; i < inCnlCnt; i++)
                    {
                        DataRowView rowView = tblInCnl.DefaultView[i];
                        int cnlNum = (int)rowView["CnlNum"];
                        CnlProps cnlProps = GetCnlProps(cnlNum);
                        if (cnlProps == null) 
                            cnlProps = new CnlProps(cnlNum);

                        // определение свойств, не использующих внешних ключей
                        cnlProps.CnlName = (string)rowView["Name"];
                        cnlProps.CtrlCnlNum = (int)rowView["CtrlCnlNum"];
                        cnlProps.EvSound = (bool)rowView["EvSound"];

                        // определение номера и наименования объекта
                        cnlProps.ObjNum = (int)rowView["ObjNum"];
                        tblObj.DefaultView.RowFilter = "ObjNum = " + cnlProps.ObjNum;
                        cnlProps.ObjName = tblObj.DefaultView.Count > 0 ? (string)tblObj.DefaultView[0]["Name"] : "";

                        // определение номера и наименования КП
                        cnlProps.KPNum = (int)rowView["KPNum"];
                        tblKP.DefaultView.RowFilter = "KPNum = " + cnlProps.KPNum;
                        cnlProps.KPName = tblKP.DefaultView.Count > 0 ? (string)tblKP.DefaultView[0]["Name"] : "";

                        // определение наименования параметра и имени файла значка
                        tblParam.DefaultView.RowFilter = "ParamID = " + rowView["ParamID"];
                        if (tblParam.DefaultView.Count > 0)
                        {
                            DataRowView paramRowView = tblParam.DefaultView[0];
                            cnlProps.ParamName = (string)paramRowView["Name"];
                            object iconFileName = paramRowView["IconFileName"];
                            cnlProps.IconFileName = iconFileName == DBNull.Value ? "" : iconFileName.ToString();
                        }
                        else
                        {
                            cnlProps.ParamName = "";
                            cnlProps.IconFileName = "";
                        }

                        // определение формата вывода
                        tblFormat.DefaultView.RowFilter = "FormatID = " + rowView["FormatID"];
                        if (tblFormat.DefaultView.Count > 0)
                        {
                            DataRowView formatRowView = tblFormat.DefaultView[0];
                            cnlProps.ShowNumber = (bool)formatRowView["ShowNumber"];
                            cnlProps.DecDigits = (int)formatRowView["DecDigits"];
                        }

                        // определение размерностей
                        tblUnit.DefaultView.RowFilter = "UnitID = " + rowView["UnitID"];
                        if (tblUnit.DefaultView.Count > 0)
                        {
                            string sign = (string)tblUnit.DefaultView[0]["Sign"];
                            cnlProps.UnitArr = sign.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < cnlProps.UnitArr.Length; j++)
                                cnlProps.UnitArr[j] = cnlProps.UnitArr[j].Trim();
                            if (cnlProps.UnitArr.Length == 1 && cnlProps.UnitArr[0] == "")
                                cnlProps.UnitArr = null;
                        }
                        else
                        {
                            cnlProps.UnitArr = null;
                        }

                        newCnlPropsArr[i] = cnlProps;
                    }

                    cnlPropsArr = newCnlPropsArr;
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при заполнении свойств входных каналов: " :
                    "Error filling input channels properties: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(baseLock);
            }
        }

        /// <summary>
        /// Обновить (пересоздать) объект для обмена данными со SCADA-Сервером
        /// при изменении файла настроек соединения со SCADA-Сервером
        /// </summary>
        private void RefrServerComm()
        {
            if (settFileName != "")
            {
                DateTime dateTime = GetLastWriteTime(settFileName);
                if (dateTime > DateTime.MinValue && dateTime != settModTime)
                {
                    settModTime = dateTime;
                    CommSettings commSettings = new CommSettings();
                    commSettings.LoadFromFile(settFileName, AppData.Log);
                    if (serverComm == null || !serverComm.CommSettings.Equals(commSettings))
                    {
                        if (serverComm != null)
                        {
                            serverComm.Close();

                            tblCur = new SrezTableLight();
                            for (int i = 0; i < HourCacheSize; i++)
                                hourTableCache[i] = null;
                            for (int i = 0; i < EventCacheSize; i++)
                                eventTableCache[i] = null;
                            hourTableIndex = 0;
                            eventTableIndex = 0;
                            trend = null;

                            baseModTime = DateTime.MinValue;
                            baseFillTime = DateTime.MinValue;
                        }
                        serverComm = new ServerComm(commSettings);
                    }
                }
            }
        }

        /// <summary>
        /// Преобразовать событие в удобную для отображения форму
        /// </summary>
        private EventView ConvEvent(EventTableLight.Event ev)
        {
            EventView eventView = new EventView();

            eventView.Num = ev.Number.ToString();
            eventView.Date = ev.DateTime.ToString("d", Localization.Culture);
            eventView.Time = ev.DateTime.ToString("T", Localization.Culture);
            eventView.Text = ev.Descr;

            // получение свойств канала события
            CnlProps cnlProps = GetCnlProps(ev.CnlNum);

            // определение наименования объекта
            if (cnlProps == null || cnlProps.ObjNum != ev.ObjNum)
            {
                tblObj.DefaultView.RowFilter = "ObjNum = " + ev.ObjNum;
                if (tblObj.DefaultView.Count > 0)
                    eventView.Obj = (string)tblObj.DefaultView[0]["Name"];
            }
            else
            {
                eventView.Obj = cnlProps.ObjName;
            }

            // определение наименования КП
            if (cnlProps == null || cnlProps.KPNum != ev.KPNum)
            {
                tblKP.DefaultView.RowFilter = "KPNum = " + ev.KPNum;
                if (tblKP.DefaultView.Count > 0)
                    eventView.KP = (string)tblKP.DefaultView[0]["Name"];
            }
            else
            {
                eventView.KP = cnlProps.KPName;
            }

            if (cnlProps != null)
            {
                // определение наименования канала и признака звука
                eventView.Cnl = cnlProps.CnlName;
                eventView.Sound = cnlProps.EvSound;

                // проверка нового статуса канала
                int newCnlStat = ev.NewCnlStat;
                bool newValIsUndef = newCnlStat <= BaseValues.ParamStat.Undefined ||
                    newCnlStat == BaseValues.ParamStat.FormulaError || newCnlStat == BaseValues.ParamStat.Unreliable;

                // определение цвета
                if (!cnlProps.ShowNumber && cnlProps.UnitArr != null && cnlProps.UnitArr.Length == 2)
                {
                    if (!newValIsUndef)
                        eventView.Color = ev.NewCnlVal > 0 ? "green" : "red";
                }
                else
                {
                    string color;
                    if (GetColorByStat(newCnlStat, out color))
                        eventView.Color = color;
                }

                // определение текста события, если не задано его описание
                if (eventView.Text == "")
                {
                    // получение типа события
                    tblEvType.DefaultView.RowFilter = "CnlStatus = " + newCnlStat;
                    string evTypeName = tblEvType.DefaultView.Count > 0 ? 
                        (string)tblEvType.DefaultView[0]["Name"] : "";

                    if (newValIsUndef)
                    {
                        eventView.Text = evTypeName;
                    }
                    else if (cnlProps.ShowNumber)
                    {
                        // добавление типа события
                        if (evTypeName != "")
                            eventView.Text = evTypeName + ": ";
                        // добавление значения канала
                        nfi.NumberDecimalDigits = cnlProps.DecDigits;
                        nfi.NumberDecimalSeparator = defDecSep;
                        nfi.NumberGroupSeparator = defGrSep;
                        eventView.Text += ev.NewCnlVal.ToString("N", nfi);
                        // добавление размерности
                        if (cnlProps.UnitArr != null)
                            eventView.Text += " " + cnlProps.UnitArr[0];
                    }
                    else if (cnlProps.UnitArr != null)
                    {
                        int unitInd = (int)ev.NewCnlVal;
                        if (unitInd < 0) 
                            unitInd = 0;
                        else if (unitInd >= cnlProps.UnitArr.Length) 
                            unitInd = cnlProps.UnitArr.Length - 1;
                        eventView.Text = cnlProps.UnitArr[unitInd];
                    }
                }
            }

            // определение свойств квитирования
            eventView.Check = ev.Checked;

            if (ev.Checked)
            {
                tblUser.DefaultView.RowFilter = "UserID = " + ev.UserID;
                eventView.User = tblUser.DefaultView.Count > 0 ? (string)tblUser.DefaultView[0]["Name"] : WebPhrases.EventChecked;
            }
            else
            {
                eventView.User = WebPhrases.EventUnchecked;
            }

            return eventView;
        }

        /// <summary>
        /// Получить цвет, соответствующий статусу
        /// </summary>
        private bool GetColorByStat(int stat, out string color)
        {
            if (tblEvType.Columns.Count > 0) // таблица загружена
            {
                tblEvType.DefaultView.RowFilter = "CnlStatus = " + stat;
                if (tblEvType.DefaultView.Count > 0)
                {
                    object colorObj = tblEvType.DefaultView[0]["Color"];
                    if (colorObj != DBNull.Value)
                    {
                        color = colorObj.ToString();
                        return true;
                    }
                }
            }

            color = "";
            return false;
        }
		

		/// <summary>
        /// Обновить таблицы базы конфигурации, если они изменились
		/// </summary>
		public void RefreshBase()
		{
            Monitor.Enter(refrLock);
            DateTime nowDT = DateTime.Now;

            try
            {
                // обновление объекта для обмена данными со SCADA-Сервером
                RefrServerComm();

                // обновление базы конфигурации
                DateTime inCnlsModTime = serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT, tblInCnl.TableName + ".dat");

                if ((nowDT - baseFillTime).TotalSeconds > BaseValidTime /*данные устарели*/ &&
                    inCnlsModTime != baseModTime /*файл таблицы входных каналов изменён*/ &&
                    inCnlsModTime > DateTime.MinValue)
                {
                    AppData.Log.WriteAction(Localization.UseRussian ? "Обновление таблиц базы конфигурации" :
                        "Refresh tables of the configuration database", Log.ActTypes.Action);
                    baseModTime = inCnlsModTime;
                    baseFillTime = nowDT;

                    // проверка блокировки таблиц SCADA-Сервером
                    try
                    {
                        DateTime t0 = nowDT;
                        TimeSpan waitSpan = new TimeSpan(0, 0, 5); // 5 секунд
                        while (serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT, "baselock") > DateTime.MinValue &&
                            DateTime.Now - t0 < waitSpan)
                            Thread.Sleep(500);
                    }
                    catch
                    {
                    }

                    // заполнение таблиц
                    Monitor.Enter(baseLock);
                    try
                    {
                        for (int i = 0; i < baseTblArr.Length; i++)
                        {
                            DataTable dataTable = baseTblArr[i];
                            if (!serverComm.ReceiveBaseTable(dataTable.TableName + ".dat", dataTable))
                                baseModTime = DateTime.MinValue;
                        }
                    }
                    finally
                    {
                        Monitor.Exit(baseLock);
                    }

                    // заполнение свойств входных каналов
                    FillCnlProps();
                }
            }
            catch (Exception ex)
            {
                baseModTime = DateTime.MinValue;
                baseFillTime = DateTime.MinValue;

                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при обновлении таблиц базы конфигурации: " :
                    "Error refreshing tables of the configuration database: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(refrLock);
            }
		}
        
        /// <summary>
        /// Обновить таблицы базы конфигурации и текущего среза, если они изменились
        /// </summary>
        public void RefreshData()
        {
            SrezTableLight hourTable;
            RefreshData(DateTime.MinValue, out hourTable);
        }

        /// <summary>
        /// Обновить таблицы базы конфигурации, текущего и часовых срезов, если они изменились
        /// </summary>
        /// <param name="reqDate">Дата запрашиваемых часовых данных</param>
        /// <param name="hourTable">Таблица часовых срезов</param>
		public void RefreshData(DateTime reqDate, out SrezTableLight hourTable)
		{
			RefreshBase();
            Monitor.Enter(refrLock);

            try
            {
                // обновление текущего среза
                DateTime now = DateTime.Now;

                if ((now - tblCur.LastFillTime).TotalSeconds > CurSrezValidTime) // данные устарели
                {
                    DateTime curModTime = serverComm.ReceiveFileAge(ServerComm.Dirs.Cur, "current.dat");
                    if (curModTime != tblCur.FileModTime) // файл среза изменён
                        tblCur.FileModTime = serverComm.ReceiveSrezTable("current.dat", tblCur) ?
                            curModTime : DateTime.MinValue;
                }

                // обновление таблицы часовых срезов
                hourTable = null;

                if (reqDate > DateTime.MinValue)
                {
                    string hourTableName = "h" + reqDate.ToString("yyMMdd") + ".dat";

                    // поиск индекса таблицы часовых срезов в кэше
                    int tableIndex = -1;
                    for (int i = 0; i < HourCacheSize; i++)
                    {
                        hourTable = hourTableCache[i];
                        if (hourTable != null && hourTable.TableName == hourTableName)
                        {
                            tableIndex = i;
                            break;
                        }
                    }

                    if (tableIndex < 0 || (now - hourTable.LastFillTime).TotalSeconds > HourSrezValidTime /*данные устарели*/)
                    {
                        DateTime fileModTime = serverComm.ReceiveFileAge(ServerComm.Dirs.Hour, hourTableName);

                        if (tableIndex < 0)
                        {
                            hourTable = null;

                            // определение места в кэше для новой таблицы часовых срезов
                            tableIndex = hourTableIndex;
                            if (++hourTableIndex == HourCacheSize)
                                hourTableIndex = 0;
                        }

                        if (hourTable == null || fileModTime != hourTable.FileModTime /*файл срезов изменён*/)
                        {
                            // создание новой таблицы часовых срезов
                            hourTable = new SrezTableLight();
                            hourTableCache[tableIndex] = hourTable;

                            // загрузка таблицы часовых срезов
                            if (serverComm.ReceiveSrezTable(hourTableName, hourTable))
                                hourTable.FileModTime = fileModTime;
                        }
                    }
                }
            }
            finally
            {
                Monitor.Exit(refrLock);
            }
		}

        /// <summary>
        /// Обновить таблицы базы конфигурации и событий, если они изменились
        /// </summary>
        /// <param name="reqDate">Дата запрашиваемых событий</param>
        /// <param name="eventTable">Таблица событий</param>
        public void RefreshEvents(DateTime reqDate, out EventTableLight eventTable)
        {
            RefreshBase();
            Monitor.Enter(refrLock);

            try
            {
                // обновление таблицы событий
                string eventTableName = "e" + reqDate.ToString("yyMMdd") + ".dat";
                eventTable = null;

                // поиск индекса таблицы событий в кэше
                int tableIndex = -1;
                for (int i = 0; i < EventCacheSize; i++)
                {
                    eventTable = eventTableCache[i];
                    if (eventTable != null && eventTable.TableName == eventTableName)
                    {
                        tableIndex = i;
                        break;
                    }
                }

                if (tableIndex < 0 || (DateTime.Now - eventTable.LastFillTime).TotalSeconds > EventValidTime /*данные устарели*/)
                {
                    DateTime fileModTime = serverComm.ReceiveFileAge(ServerComm.Dirs.Events, eventTableName);

                    if (tableIndex < 0)
                    {
                        eventTable = null;

                        // определение места в кэше для новой таблицы событий
                        tableIndex = eventTableIndex;
                        if (++eventTableIndex == EventCacheSize)
                            eventTableIndex = 0;
                    }

                    if (eventTable == null || fileModTime != eventTable.FileModTime /*файл событий изменён*/)
                    {
                        // создание новой таблицы событий
                        eventTable = new EventTableLight();
                        eventTableCache[tableIndex] = eventTable;

                        // загрузка таблицы часовых срезов
                        if (serverComm.ReceiveEventTable(eventTableName, eventTable))
                            eventTable.FileModTime = fileModTime;
                    }
                }
            }
            finally
            {
                Monitor.Exit(refrLock);
            }
        }

        /// <summary>
        /// Ограничить количество используемых входных каналов
        /// </summary>
        /// <param name="maxCnlCnt">Максимальное количество входных каналов или 0, если количество неограничено</param>
        public void RestrictCnlCnt(int maxCnlCnt)
        {
            Monitor.Enter(cnlPropLock);
            try
            {
                this.maxCnlCnt = maxCnlCnt;
                baseModTime = DateTime.MinValue;
                baseFillTime = DateTime.MinValue;
            }
            finally
            {
                Monitor.Exit(cnlPropLock);
            }
        }


		/// <summary>
		/// Получить свойства входного канала по его номеру
		/// </summary>
		public CnlProps GetCnlProps(int cnlNum)
		{
            Monitor.Enter(cnlPropLock);
            CnlProps cnlProps = null;

            try
            {                
                if (cnlPropsArr != null)
                {
                    int ind = Array.BinarySearch(cnlPropsArr, (object)cnlNum);
                    if (ind >= 0)
                        cnlProps = cnlPropsArr[ind];
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при получении свойств входного канала {0}: {1}" : 
                    "Error getting input channel {0} properties: {1}", cnlNum, ex.Message), Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(cnlPropLock);
            }

            return cnlProps;
		}

        /// <summary>
        /// Получить свойства канала управления по его номеру
        /// </summary>
        public CtrlCnlProps GetCtrlCnlProps(int ctrlCnlNum)
        {
            Monitor.Enter(baseLock);
            CtrlCnlProps ctrlCnlProps = null;

            try
            {
                tblCtrlCnl.DefaultView.RowFilter = "CtrlCnlNum = " + ctrlCnlNum;
                if (tblCtrlCnl.DefaultView.Count > 0)
                {
                    DataRowView rowView = tblCtrlCnl.DefaultView[0];
                    ctrlCnlProps = new CtrlCnlProps(ctrlCnlNum);
                    ctrlCnlProps.CtrlCnlName = (string)rowView["Name"];
                    ctrlCnlProps.CmdTypeID = (int)rowView["CmdTypeID"];

                    // определение номера и наименования объекта
                    ctrlCnlProps.ObjNum = (int)rowView["ObjNum"];
                    tblObj.DefaultView.RowFilter = "ObjNum = " + ctrlCnlProps.ObjNum;
                    ctrlCnlProps.ObjName = tblObj.DefaultView.Count > 0 ? (string)tblObj.DefaultView[0]["Name"] : "";

                    // определение номера и наименования КП
                    ctrlCnlProps.KPNum = (int)rowView["KPNum"];
                    tblKP.DefaultView.RowFilter = "KPNum = " + ctrlCnlProps.KPNum;
                    ctrlCnlProps.KPName = tblKP.DefaultView.Count > 0 ? (string)tblKP.DefaultView[0]["Name"] : "";

                    // определение значений команды
                    tblCmdVal.DefaultView.RowFilter = "CmdValID = " + rowView["CmdValID"];
                    if (tblCmdVal.DefaultView.Count > 0)
                    {
                        string val = (string)tblCmdVal.DefaultView[0]["Val"];
                        ctrlCnlProps.CmdValArr = val.Split(';'); // включая пустые элементы
                        for (int i = 0; i < ctrlCnlProps.CmdValArr.Length; i++)
                            ctrlCnlProps.CmdValArr[i] = ctrlCnlProps.CmdValArr[i].Trim();
                        if (ctrlCnlProps.CmdValArr.Length == 1 && ctrlCnlProps.CmdValArr[0] == "")
                            ctrlCnlProps.CmdValArr = null;
                    }
                    else
                    {
                        ctrlCnlProps.CmdValArr = null;
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при получении свойств канала управления {0}: {1}" :
                    "Error getting output channel {0} properties: {1}", ctrlCnlNum, ex.Message), 
                    Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(baseLock);
            }

            return ctrlCnlProps;
        }


		/// <summary>
		/// Получить данные канала текущего среза
		/// </summary>
		public void GetCurData(int cnlNum, out double val, out int stat)
		{
            Monitor.Enter(cnlDataLock);
			val = 0.0;
			stat = 0;

            try
            {
                if (tblCur.SrezList.Count > 0)
                {
                    SrezTableLight.Srez srez = tblCur.SrezList.Values[0];
                    SrezTableLight.CnlData cnlData;
                    bool found = srez.GetCnlData(cnlNum, out cnlData);
                    if (found)
                    {
                        val = cnlData.Val;
                        stat = cnlData.Stat;
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при получении данных канала {0} текущего среза: {1}" :
                    "Error getting channel {0} current data: {1}", cnlNum, ex.Message), Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(cnlDataLock);
            }
        }

		/// <summary>
		/// Получить данные канала часового среза из указанной таблицы срезов
		/// </summary>
        public void GetHourData(SrezTableLight hourTable, int cnlNum, DateTime dateTime, 
            out double val, out int stat)
		{
            Monitor.Enter(cnlDataLock);
            val = 0.0;
			stat = 0;

            try
            {
                SrezTableLight.Srez srez;
                if (hourTable != null && hourTable.SrezList.TryGetValue(dateTime, out srez))
                {
                    SrezTableLight.CnlData cnlData;
                    bool found = srez.GetCnlData(cnlNum, out cnlData);
                    if (found)
                    {
                        val = cnlData.Val;
                        stat = cnlData.Stat;
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при получении данных канала {0} часового среза: {1}" :
                    "Error getting channel {0} hour data: {1}", cnlNum, ex.Message), Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(cnlDataLock);
            }
        }
        
        /// <summary>
        /// Получить события, соответствующие фильтру, из таблицы событий
        /// </summary>
        /// <remarks>Если cnlsFilter равен null, то фильтрация не производится</remarks>
        public List<EventTableLight.Event> GetEvents(EventTableLight eventTable, List<int> cnlsFilter)
        {
            Monitor.Enter(eventLock);
            List<EventTableLight.Event> eventList = null;

            try
            {
                if (eventTable != null)
                {
                    if (cnlsFilter == null)
                    {
                        eventList = eventTable.AllEvents;
                    }
                    else
                    {
                        eventTable.Filters = EventTableLight.EventFilters.Cnls;
                        eventTable.CnlsFilter = cnlsFilter;
                        eventList = eventTable.FilteredEvents;
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при получении событий: " : 
                    "Error getting events: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(eventLock);
            }

            return eventList;
        }

        /// <summary>
        /// Получить события, соответствующие фильтру, из таблицы событий, начиная с заданного номера события
        /// </summary>
        /// <remarks>Если cnlsFilter равен null, то фильтрация не производится</remarks>
        public List<EventTableLight.Event> GetEvents(EventTableLight eventTable, List<int> cnlsFilter, 
            int startEvNum)
        {
            Monitor.Enter(eventLock);
            List<EventTableLight.Event> eventList = null;

            try
            {
                if (eventTable != null)
                {
                    eventTable.Filters = cnlsFilter == null ?
                        EventTableLight.EventFilters.None : EventTableLight.EventFilters.Cnls;
                    eventTable.CnlsFilter = cnlsFilter;
                    eventList = eventTable.GetEvents(startEvNum);
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при получении событий, начиная с заданного номера: " :
                    "Error getting events starting from the specified number: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(eventLock);
            }

            return eventList;
        }

        /// <summary>
        /// Получить заданное количество последних событий, соответствующих фильтру
        /// </summary>
        /// <remarks>Если cnlsFilter равен null, то фильтрация не производится</remarks>
        public List<EventTableLight.Event> GetLastEvents(EventTableLight eventTable, List<int> cnlsFilter, 
            int count)
        {
            Monitor.Enter(eventLock);
            List<EventTableLight.Event> eventList = null;

            try
            {
                if (eventTable != null)
                {
                    eventTable.Filters = cnlsFilter == null ?
                        EventTableLight.EventFilters.None : EventTableLight.EventFilters.Cnls;
                    eventTable.CnlsFilter = cnlsFilter;
                    eventList = eventTable.GetLastEvents(count);
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при получении последних событий: " :
                    "Error getting last events: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(eventLock);
            }

            return eventList;
        }

        /// <summary>
        /// Получить событие из таблицы событий по номеру
        /// </summary>
        public EventTableLight.Event GetEventByNum(EventTableLight eventTable, int evNum)
        {
            Monitor.Enter(eventLock);
            EventTableLight.Event ev = null;

            try
            {
                if (1 <= evNum && evNum <= eventTable.AllEvents.Count &&
                    eventTable.AllEvents[evNum - 1].Number == evNum)
                    ev = eventTable.AllEvents[evNum - 1];
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при получении события по номеру: " :
                    "Error getting event by number: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(eventLock);
            }

            return ev;
        }

        /// <summary>
        /// Преобразовать событие в удобную для отображения форму
        /// </summary>
        public EventView ConvertEvent(EventTableLight.Event ev)
        {
            Monitor.Enter(baseLock);
            EventView eventView = null;

            try 
            {
                if (ev != null)
                    eventView = ConvEvent(ev);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при преобразовании события в удобную для отображения форму: " :
                    "Error converting event to a suitable view") + ex.Message, Log.ActTypes.Exception);
            }
            finally 
            {
                Monitor.Exit(baseLock); 
            }

            return eventView;
        }

        /// <summary>
        /// Преобразовать список событий в удобную для отображения форму
        /// </summary>
        public List<EventView> ConvertEvents(List<EventTableLight.Event> eventList)
        {
            Monitor.Enter(baseLock);
            List<EventView> eventViewList = new List<EventView>();

            try
            {
                if (eventList != null)
                    foreach (EventTableLight.Event ev in eventList)
                        eventViewList.Add(ConvEvent(ev));                
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ?
                    "Ошибка при преобразовании списка событий в удобную для отображения форму: " :
                    "Error converting events list to a suitable view") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(baseLock);
            }

            return eventViewList;
        }

		/// <summary>
		/// Получить данные канала минутного среза за сутки
		/// </summary>
		public Trend GetMinData(int cnlNum, DateTime date)
		{
            Monitor.Enter(cnlDataLock);
            try
            {
                string minTableName = "m" + date.ToString("yyMMdd") + ".dat";
                if (trend == null || trend.CnlNum != cnlNum || trend.TableName != minTableName)
                {
                    trend = new Trend(cnlNum);
                    trend.FileModTime = serverComm.ReceiveTrend(minTableName, date, trend) ?
                        serverComm.ReceiveFileAge(ServerComm.Dirs.Min, minTableName) : DateTime.MinValue;
                }
                else
                {
                    DateTime minModTime = serverComm.ReceiveFileAge(ServerComm.Dirs.Min, minTableName);
                    if ((DateTime.Now - minModTime).TotalSeconds > MinSrezValidTime /*данные устарели*/ &&
                        minModTime != trend.FileModTime /*файл срезов изменён*/)
                    {
                        trend = new Trend(cnlNum);
                        trend.FileModTime = serverComm.ReceiveTrend(minTableName, date, trend) ?
                            minModTime : DateTime.MinValue;
                    }
                }
                return trend;
            }
            finally
            {
                Monitor.Exit(cnlDataLock);
            }
        }


		/// <summary>
		/// Получить отформатированное текущее значение канала
		/// </summary>
		public string GetCnlVal(int cnlNum, bool showUnit, out string color)
		{
            return GetCnlVal(null, cnlNum, DateTime.MinValue, showUnit, out color);
		}

		/// <summary>
        /// Получить отформатированное значение канала из текущего или часового среза за указанное время
		/// </summary>
        /// <remarks>Для текущего значения hourTable равно null или dataDT равно DateTime.MinValue</remarks>
        public string GetCnlVal(SrezTableLight hourTable, int cnlNum, DateTime dateTime, bool showUnit, 
            out string color)
		{
            // получение значения и статуса канала
            double val;
            int stat;

            if (dateTime == DateTime.MinValue || hourTable == null)
                GetCurData(cnlNum, out val, out stat);
            else
                GetHourData(hourTable, cnlNum, dateTime, out val, out stat);

            // форматирование значения канала
            bool isNumber;
            return FormatCnlVal(val, stat, GetCnlProps(cnlNum), showUnit, true, dateTime, DateTime.Now, 
                out isNumber, out color);
		}

        /// <summary>
        /// Форматировать значение входного канала
        /// </summary>
        /// <remarks>Для текущего значения dataDT равно DateTime.MinValue</remarks>
        public string FormatCnlVal(double val, int stat, CnlProps cnlProps, bool showUnit, bool getColor, 
            DateTime dataDT, DateTime nowDT, out bool isNumber, out string color,
            string decSep = null, string grSep = null)
        {
            string result = "";
            isNumber = false;
            color = "black";

            try
            {
                // определение длины массива размерностей канала
                int unitArrLen = cnlProps == null || cnlProps.UnitArr == null ? 0 : cnlProps.UnitArr.Length;

                // определение цвета
                if (cnlProps != null && getColor)
                {
                    if (!cnlProps.ShowNumber && unitArrLen == 2 && stat > 0 && 
                        stat != BaseValues.ParamStat.FormulaError && stat != BaseValues.ParamStat.Unreliable)
                    {
                        color = val > 0 ? "green" : "red";
                    }
                    else
                    {
                        Monitor.Enter(baseLock);
                        try
                        {
                            string colorByStat;
                            if (GetColorByStat(stat, out colorByStat))
                                color = colorByStat;
                        }
                        finally
                        {
                            Monitor.Exit(baseLock);
                        }
                    }
                }

                // определение результата метода
                if (cnlProps == null || cnlProps.ShowNumber)
                {
                    string unit = showUnit && unitArrLen > 0 ? " " + cnlProps.UnitArr[0] : "";
                    isNumber = unit == "";

                    nfi.NumberDecimalDigits = cnlProps == null ? 3 : cnlProps.DecDigits;
                    nfi.NumberDecimalSeparator = decSep == null ? defDecSep : decSep;
                    nfi.NumberGroupSeparator = grSep == null ? defGrSep : grSep;
                    result = val.ToString("N", nfi) + unit;
                }
                else if (unitArrLen > 0)
                {
                    int unitInd = (int)val;
                    if (unitInd < 0)
                        unitInd = 0;
                    else if (unitInd >= unitArrLen)
                        unitInd = unitArrLen - 1;
                    result = cnlProps.UnitArr[unitInd];
                }

                // изменение результата метода, если значение канала не определено			
                if (dataDT == DateTime.MinValue)
                {
                    if ((nowDT - tblCur.FileModTime).TotalMinutes > CurSrezShowTime) // текущий срез устарел
                    {
                        result = "";
                        isNumber = false;
                    }
                    else if (stat == 0)
                    {
                        result = "---";
                        isNumber = false;
                    }
                }
                else if (stat == 0)
                {
                    result = "---";
                    isNumber = false;

                    if (dataDT.Date > nowDT.Date)
                    {
                        result = "";
                    }
                    else if (dataDT.Date == nowDT.Date)
                    {
                        if (dataDT.Hour > nowDT.Hour + 1)
                            result = "";
                        else if (dataDT.Hour == nowDT.Hour + 1)
                        {
                            result = "***";
                            color = "green";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string cnlNumStr = cnlProps == null ? "" : " " + cnlProps.CnlNum;
                AppData.Log.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при форматировании значения входного канала{0}: {1}" : 
                    "Error formatting input channel{0} value: {1}", cnlNumStr, ex.Message), Log.ActTypes.Exception);
            }

            return result;
        }


		/// <summary>
        /// Проверить правильность имени и пароля пользователя, получить его роль
		/// </summary>
        public bool CheckUser(string login, string password, bool checkPassword, out int roleID, out string errMsg)
		{
            // обновление объекта для обмена данными со SCADA-Сервером при необходимости
            RefrServerComm();

            if (serverComm == null)
            {
                roleID = (int)ServerComm.Roles.Disabled;
                errMsg = WebPhrases.CommSettingsNotLoaded;
                return false;
            }
            else
            {
                if (checkPassword && string.IsNullOrEmpty(password))
                {
                    roleID = (int)ServerComm.Roles.Err;
                    errMsg = WebPhrases.WrongPassword;
                    return false;
                }
                else
                {
                    // проверка пользователя
                    if (serverComm.CheckUser(login, checkPassword ? password : null, out roleID))
                    {
                        if (roleID == (int)ServerComm.Roles.Disabled)
                            errMsg = WebPhrases.NoRightsL;
                        else if (roleID == (int)ServerComm.Roles.App)
                            errMsg = WebPhrases.IllegalRole;
                        else if (roleID == (int)ServerComm.Roles.Err)
                            errMsg = WebPhrases.WrongPassword;
                        else
                            errMsg = "";

                        return errMsg == "";
                    }
                    else
                    {
                        errMsg = WebPhrases.ServerUnavailable;
                        return false;
                    }
                }
            }
		}

        /// <summary>
        /// Получить идентификатор пользователя по имени
        /// </summary>
        public int GetUserID(string login)
        {
            // обновление таблиц базы конфигурации при необходимости
            RefreshBase();

            Monitor.Enter(baseLock);
            int userID = 0;

            try
            {
                tblUser.DefaultView.RowFilter = "Name = '" + login + "'";
                if (tblUser.DefaultView.Count > 0)
                    userID = (int)tblUser.DefaultView[0]["UserID"];
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при получении идентификатора пользователя по имени: " : 
                    "Error getting user ID by name: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(baseLock);
            }

            return userID;
        }

        /// <summary>
        /// Получить список прав на объекты интерфейса для заданного идентификатора роли
        /// </summary>
        public SortedList<string, Right> GetRightList(int roleID)
        {
            // обновление таблиц базы конфигурации при необходимости
            RefreshBase();

            Monitor.Enter(baseLock);
            SortedList<string, Right> rightList = new SortedList<string, Right>();

            try
            {
                tblRight.DefaultView.RowFilter = "RoleID = " + roleID;
                int rowCnt = tblRight.DefaultView.Count;

                for (int i = 0; i < rowCnt; i++)
                {
                    DataRowView rowView = tblRight.DefaultView[i];
                    tblInterface.DefaultView.RowFilter = "ItfID = " + rowView["ItfID"];

                    if (tblInterface.DefaultView.Count > 0)
                    {
                        Right right = new Right();
                        right.ViewRight = (bool)rowView["ViewRight"];
                        right.CtrlRight = (bool)rowView["CtrlRight"];

                        string name = (string)tblInterface.DefaultView[0]["Name"];
                        if (!rightList.ContainsKey(name))
                            rightList.Add(name, right);
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? 
                    "Ошибка при получении списка прав на объекты интерфейса: " : 
                    "Error getting interface objects rights list: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(baseLock);
            }

            return rightList;
        }

        /// <summary>
        /// Получить наименование роли по идентификатору
        /// </summary>
        public string GetRoleName(int roleID)
        {
            string roleName = ServerComm.GetRoleName(roleID);

            if ((int)ServerComm.Roles.Custom <= roleID && roleID < (int)ServerComm.Roles.Err)
            {
                // обновление таблиц базы конфигурации при необходимости
                RefreshBase();

                // получение наименования пользовательской роли из базы конфигурации
                Monitor.Enter(baseLock);
                try
                {
                    tblRole.DefaultView.RowFilter = "RoleID = " + roleID;
                    if (tblRole.DefaultView.Count > 0)
                        roleName = (string)tblRole.DefaultView[0]["Name"];
                }
                catch (Exception ex)
                {
                    AppData.Log.WriteAction((Localization.UseRussian ? 
                        "Ошибка при получении наименования роли по идентификатору: " :
                        "Error getting role name by ID: ") +  ex.Message, Log.ActTypes.Exception);
                }
                finally
                {
                    Monitor.Exit(baseLock);
                }
            }

            return roleName;
        }
	}
}