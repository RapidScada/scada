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
 * Module   : ScadaData
 * Summary  : Cache of the data received from SCADA-Server for clients usage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Data;
using System;
using System.Data;
using System.Threading;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// Cache of the data received from SCADA-Server for clients usage
    /// <para>Кэш данных, полученных от SCADA-Сервера, для использования клиентами</para>
    /// </summary>
    /// <remarks>All the returned data are not thread safe
    /// <para>Все возвращаемые данные не являются потокобезопасными</para></remarks>
    public class DataCache
    {
        /// <summary>
        /// Время актуальности архивных данных в кэше
        /// </summary>
        protected static readonly TimeSpan BaseValidSpan = TimeSpan.FromSeconds(1);
        /// <summary>
        /// Время актуальности архивных данных в кэше
        /// </summary>
        protected static readonly TimeSpan DataValidSpan = TimeSpan.FromMilliseconds(500);
        /// <summary>
        /// Время ожидания снятия блокировки базы конфигурации
        /// </summary>
        protected static readonly TimeSpan WaitBaseLock = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Разделитель значений внутри поля таблицы
        /// </summary>
        protected static readonly char[] FieldSeparator = new char[] { ';' };


        /// <summary>
        /// Объект для обмена данными со SCADA-Сервером
        /// </summary>
        protected readonly ServerComm serverComm;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;

        /// <summary>
        /// Объект для синхронизации доступа к таблицам базы конфигурации
        /// </summary>
        protected readonly object baseLock;
        /// <summary>
        /// Объект для синхронизации достапа к текущим даным
        /// </summary>
        protected readonly object curDataLock;

        /// <summary>
        /// Время последего успешного обновления таблиц базы конфигурации
        /// </summary>
        protected DateTime baseRefrDT;
        /// <summary>
        /// Таблица текущего среза
        /// </summary>
        protected SrezTableLight tblCur;
        /// <summary>
        /// Время последего успешного обновления таблицы текущего среза
        /// </summary>
        protected DateTime curRefrDT;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected DataCache()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataCache(ServerComm serverComm, Log log)
        {
            if (serverComm == null)
                throw new ArgumentNullException("serverComm");
            if (log == null)
                throw new ArgumentNullException("log");

            this.serverComm = serverComm;
            this.log = log;

            baseLock = new object();
            curDataLock = new object();

            baseRefrDT = DateTime.MinValue;
            tblCur = new SrezTableLight();
            curRefrDT = DateTime.MinValue;

            BaseAge = DateTime.MinValue;
            BaseTables = new BaseTables();
            CnlProps = new InCnlProps[0];
            CtrlCnlProps = new CtrlCnlProps[0];
        }


        /// <summary>
        /// Получить время последнего изменения успешно считанной базы конфигурации
        /// </summary>
        public DateTime BaseAge { get; protected set; }

        /// <summary>
        /// Получить таблицы базы конфигурации
        /// </summary>
        /// <remarks>Таблицы после загрузки не изменяются экземпляром данного класса.
        /// При обновлении таблиц объект таблиц пересоздаётся, обеспечивая целостность</remarks>
        public BaseTables BaseTables { get; protected set; }

        /// <summary>
        /// Получить свойства входных каналов
        /// </summary>
        /// <remarks>Свойства каналов автоматически создаются после обновления таблиц базы конфигурации</remarks>
        public InCnlProps[] CnlProps { get; protected set; }

        /// <summary>
        /// Получить свойства входных каналов
        /// </summary>
        /// <remarks>Свойства каналов автоматически создаются после обновления таблиц базы конфигурации</remarks>
        public CtrlCnlProps[] CtrlCnlProps { get; protected set; }


        /// <summary>
        /// Заполнить свойства входных каналов
        /// </summary>
        protected void FillCnlProps()
        {
            try
            {
                log.WriteAction(Localization.UseRussian ?
                    "Заполнение свойств входных каналов" :
                    "Fill input channels properties");

                DataTable tblInCnl = BaseTables.InCnlTable;
                int inCnlCnt = tblInCnl.Rows.Count; // количество входных каналов
                InCnlProps[] newCnlProps = new InCnlProps[inCnlCnt];

                for (int i = 0; i < inCnlCnt; i++)
                {
                    DataRowView rowView = tblInCnl.DefaultView[i];
                    InCnlProps cnlProps = new InCnlProps();

                    // определение свойств, не использующих внешних ключей
                    cnlProps.CnlNum = (int)rowView["CnlNum"];
                    cnlProps.CnlName = (string)rowView["Name"];
                    cnlProps.CnlTypeID = (int)rowView["CnlTypeID"];
                    cnlProps.ObjNum = (int)rowView["ObjNum"];
                    cnlProps.KPNum = (int)rowView["KPNum"];
                    cnlProps.Signal = (int)rowView["Signal"];
                    cnlProps.FormulaUsed = (bool)rowView["FormulaUsed"];
                    cnlProps.Formula = (string)rowView["Formula"];
                    cnlProps.Averaging = (bool)rowView["Averaging"];
                    cnlProps.ParamID = (int)rowView["ParamID"];
                    cnlProps.UnitID = (int)rowView["UnitID"];
                    cnlProps.CtrlCnlNum = (int)rowView["CtrlCnlNum"];
                    cnlProps.EvEnabled = (bool)rowView["EvEnabled"];
                    cnlProps.EvSound = (bool)rowView["EvSound"];
                    cnlProps.EvOnChange = (bool)rowView["EvOnChange"];
                    cnlProps.EvOnUndef = (bool)rowView["EvOnUndef"];
                    cnlProps.LimLowCrash = (double)rowView["LimLowCrash"];
                    cnlProps.LimLow = (double)rowView["LimLow"];
                    cnlProps.LimHigh = (double)rowView["LimHigh"];
                    cnlProps.LimHighCrash = (double)rowView["LimHighCrash"];

                    // определение наименования объекта
                    DataTable tblObj = BaseTables.ObjTable;
                    tblObj.DefaultView.RowFilter = "ObjNum = " + cnlProps.ObjNum;
                    cnlProps.ObjName = tblObj.DefaultView.Count > 0 ? (string)tblObj.DefaultView[0]["Name"] : "";

                    // определение наименования КП
                    DataTable tblKP = BaseTables.KPTable;
                    tblKP.DefaultView.RowFilter = "KPNum = " + cnlProps.KPNum;
                    cnlProps.KPName = tblKP.DefaultView.Count > 0 ? (string)tblKP.DefaultView[0]["Name"] : "";

                    // определение наименования параметра и имени файла значка
                    DataTable tblParam = BaseTables.ParamTable;
                    tblParam.DefaultView.RowFilter = "ParamID = " + cnlProps.ParamID;
                    if (tblParam.DefaultView.Count > 0)
                    {
                        DataRowView paramRowView = tblParam.DefaultView[0];
                        cnlProps.ParamName = (string)paramRowView["Name"];
                        cnlProps.IconFileName = (string)paramRowView["IconFileName"];
                    }

                    // определение формата вывода
                    DataTable tblFormat = BaseTables.FormatTable;
                    tblFormat.DefaultView.RowFilter = "FormatID = " + rowView["FormatID"];
                    if (tblFormat.DefaultView.Count > 0)
                    {
                        DataRowView formatRowView = tblFormat.DefaultView[0];
                        cnlProps.ShowNumber = (bool)formatRowView["ShowNumber"];
                        cnlProps.DecDigits = (int)formatRowView["DecDigits"];
                    }

                    // определение размерностей
                    DataTable tblUnit = BaseTables.UnitTable;
                    tblUnit.DefaultView.RowFilter = "UnitID = " + cnlProps.UnitID;
                    if (tblUnit.DefaultView.Count > 0)
                    {
                        cnlProps.UnitSign = (string)tblUnit.DefaultView[0]["Sign"];
                        string[] unitArr = cnlProps.UnitArr = 
                            cnlProps.UnitSign.Split(FieldSeparator, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < unitArr.Length; j++)
                            unitArr[j] = unitArr[j].Trim();
                        if (unitArr.Length == 1 && unitArr[0] == "")
                            cnlProps.UnitArr = null;
                    }

                    newCnlProps[i] = cnlProps;
                }

                CnlProps = newCnlProps;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, (Localization.UseRussian ? 
                    "Ошибка при заполнении свойств входных каналов: " :
                    "Error filling input channels properties"));
            }
        }

        /// <summary>
        /// Заполнить свойства каналов управления
        /// </summary>
        protected void FillCtrlCnlProps()
        {
            try
            {
                log.WriteAction(Localization.UseRussian ?
                    "Заполнение свойств каналов управления" :
                    "Fill output channels properties");

                DataTable tblCtrlCnl = BaseTables.CtrlCnlTable;
                int ctrlCnlCnt = tblCtrlCnl.Rows.Count;
                CtrlCnlProps[] newCtrlCnlProps = new CtrlCnlProps[ctrlCnlCnt];

                for (int i = 0; i < ctrlCnlCnt; i++)
                {
                    DataRowView rowView = tblCtrlCnl.DefaultView[i];
                    CtrlCnlProps ctrlCnlProps = new CtrlCnlProps();

                    // определение свойств, не использующих внешних ключей
                    ctrlCnlProps.CtrlCnlNum = (int)rowView["CtrlCnlNum"];
                    ctrlCnlProps.CtrlCnlName = (string)rowView["Name"];
                    ctrlCnlProps.CmdTypeID = (int)rowView["CmdTypeID"];
                    ctrlCnlProps.ObjNum = (int)rowView["ObjNum"];
                    ctrlCnlProps.KPNum = (int)rowView["KPNum"];
                    ctrlCnlProps.CmdNum = (int)rowView["CmdNum"];
                    ctrlCnlProps.CmdValID = (int)rowView["CmdValID"];
                    ctrlCnlProps.FormulaUsed = (bool)rowView["FormulaUsed"];
                    ctrlCnlProps.Formula = (string)rowView["Formula"];
                    ctrlCnlProps.EvEnabled = (bool)rowView["EvEnabled"];

                    // определение наименования объекта
                    DataTable tblObj = BaseTables.ObjTable;
                    tblObj.DefaultView.RowFilter = "ObjNum = " + ctrlCnlProps.ObjNum;
                    ctrlCnlProps.ObjName = tblObj.DefaultView.Count > 0 ? (string)tblObj.DefaultView[0]["Name"] : "";

                    // определение наименования КП
                    DataTable tblKP = BaseTables.KPTable;
                    tblKP.DefaultView.RowFilter = "KPNum = " + ctrlCnlProps.KPNum;
                    ctrlCnlProps.KPName = tblKP.DefaultView.Count > 0 ? (string)tblKP.DefaultView[0]["Name"] : "";

                    // определение размерностей
                    DataTable tblCmdVal = BaseTables.CmdValTable;
                    tblCmdVal.DefaultView.RowFilter = "CmdValID = " + ctrlCnlProps.CmdValID;
                    if (tblCmdVal.DefaultView.Count > 0)
                    {
                        ctrlCnlProps.CmdVal = (string)tblCmdVal.DefaultView[0]["Val"];
                        string[] cmdValArr = ctrlCnlProps.CmdValArr = 
                            ctrlCnlProps.CmdVal.Split(FieldSeparator, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < cmdValArr.Length; j++)
                            cmdValArr[j] = cmdValArr[j].Trim();
                        if (cmdValArr.Length == 1 && cmdValArr[0] == "")
                            ctrlCnlProps.CmdValArr = null;
                    }

                    newCtrlCnlProps[i] = ctrlCnlProps;
                }

                CtrlCnlProps = newCtrlCnlProps;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, (Localization.UseRussian ?
                    "Ошибка при заполнении свойств каналов управления: " :
                    "Error filling output channels properties"));
            }
        }

        /// <summary>
        /// Обновить текущие данные
        /// </summary>
        protected void RefreshCurData()
        {
            try
            {
                DateTime utcNowDT = DateTime.UtcNow;
                if (utcNowDT - curRefrDT > DataValidSpan) // данные устарели
                {
                    curRefrDT = utcNowDT;
                    DateTime newCurAge = serverComm.ReceiveFileAge(ServerComm.Dirs.Cur, SrezAdapter.CurTableName);

                    if (newCurAge == DateTime.MinValue)
                    {
                        throw new ScadaException(Localization.UseRussian ?
                            "Не удалось принять время изменения файла текущих данных." :
                            "Unable to receive the current data file modification time.");
                    }
                    else if (tblCur.FileModTime != newCurAge) // файл среза изменён
                    {
                        if (serverComm.ReceiveSrezTable(SrezAdapter.CurTableName, tblCur))
                        {
                            tblCur.FileModTime = newCurAge;
                            tblCur.LastFillTime = utcNowDT;
                        }
                        else
                        {
                            curRefrDT = DateTime.MinValue;
                            tblCur.FileModTime = DateTime.MinValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                curRefrDT = DateTime.MinValue;
                tblCur.FileModTime = DateTime.MinValue;

                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при обновлении текущих данных" :
                    "Error refreshing the current data");
            }
        }


        /// <summary>
        /// Обновить таблицы базы конфигурации и свойства каналов
        /// </summary>
        public void RefreshBaseTables()
        {
            lock (baseLock)
            {
                try
                {
                    DateTime utcNowDT = DateTime.UtcNow;

                    if (utcNowDT - baseRefrDT > BaseValidSpan) // данные устарели
                    {
                        baseRefrDT = utcNowDT;
                        DateTime newBaseAge = serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT,
                            BaseTables.GetFileName(BaseTables.InCnlTable));

                        if (newBaseAge == DateTime.MinValue)
                        {
                            throw new ScadaException(Localization.UseRussian ?
                                "Не удалось принять время изменения базы конфигурации." :
                                "Unable to receive the configuration database modification time.");
                        }
                        else if (BaseAge != newBaseAge) // база конфигурации изменена
                        {
                            BaseAge = newBaseAge;
                            log.WriteAction(Localization.UseRussian ? 
                                "Обновление таблиц базы конфигурации" :
                                "Refresh the tables of the configuration database");

                            // ожидание снятия возможной блокировки базы конфигурации
                            DateTime t0 = utcNowDT;
                            while (serverComm.ReceiveFileAge(ServerComm.Dirs.BaseDAT, "baselock") > DateTime.MinValue &&
                                DateTime.UtcNow - t0 <= WaitBaseLock)
                            {
                                Thread.Sleep(ScadaUtils.ThreadDelay);
                            }

                            // получение данных таблиц
                            foreach (DataTable dataTable in BaseTables.AllTables)
                            {
                                string tableName = BaseTables.GetFileName(dataTable);

                                if (!serverComm.ReceiveBaseTable(tableName, dataTable))
                                {
                                    log.WriteError(string.Format(Localization.UseRussian ?
                                        "Не удалось принять таблицу {0}" :
                                        "Unable to receive the table {0}", tableName));

                                    baseRefrDT = DateTime.MinValue;
                                    BaseAge = DateTime.MinValue;
                                }
                            }

                            // заполнение свойств каналов
                            FillCnlProps();
                            FillCtrlCnlProps();
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseRefrDT = DateTime.MinValue;
                    BaseAge = DateTime.MinValue;

                    log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при обновлении таблиц базы конфигурации" :
                        "Error refreshing the tables of the configuration database");
                }
            }
        }

        /// <summary>
        /// Получить текущий срез из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемый срез после загрузки не изменяется экземпляром данного класса,
        /// таким образом, чтение его данных является потокобезопасным</remarks>
        public SrezTableLight.Srez GetCurSnapshot(out DateTime dataAge)
        {
            lock (curDataLock)
            {
                try
                {
                    RefreshCurData();
                    dataAge = tblCur.FileModTime;
                    return tblCur.SrezList.Count > 0 ? tblCur.SrezList.Values[0] : null;
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, Localization.UseRussian ?
                        "Ошибка при получении текущего среза из кеша или от сервера" :
                        "Error getting the current snapshot the cache or from the server");
                    dataAge = DateTime.MinValue;
                    return null;
                }
            }
        }

        /// <summary>
        /// Получить таблицу часового среза за сутки из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемая таблица после загрузки не изменяется экземпляром данного класса,
        /// таким образом, чтение её данных является потокобезопасным. 
        /// Метод всегда возвращает объект, не равный null</remarks>
        public SrezTableLight GetHourTable(DateTime date)
        {
            return new SrezTableLight();
        }

        /// <summary>
        /// Получить таблицу событий за сутки из кеша или от сервера
        /// </summary>
        /// <remarks>Возвращаемая таблица после загрузки не изменяется экземпляром данного класса,
        /// таким образом, чтение её данных является потокобезопасным.
        /// Метод всегда возвращает объект, не равный null</remarks>
        public EventTableLight GetEventTable(DateTime date)
        {
            return new EventTableLight();
        }

        /// <summary>
        /// Получить тренд минутных данных заданного канала за сутки
        /// </summary>
        /// <remarks>Возвращаемый тренд после загрузки не изменяется экземпляром данного класса,
        /// таким образом, чтение его данных является потокобезопасным.
        /// Метод всегда возвращает объект, не равный null</remarks>
        public Trend GetMinTrend(int cnlNum, DateTime date)
        {
            return new Trend(cnlNum);
        }
    }
}