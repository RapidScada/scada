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
 * Module   : ScadaData
 * Summary  : Adapter for reading and writing event tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2019
 * 
 * --------------------------------
 * Table file structure (version 3)
 * --------------------------------
 * File consists of a sequence of events, each event has the structure:
 * Time            - event time stamp                        (Double)
 * ObjNum          - object number                           (UInt16)
 * KPNum           - device number                           (UInt16)
 * ParamID         - quantity ID                             (UInt16)
 * CnlNum          - input channel number                    (UInt16)
 * OldCnlVal       - previous channel number                 (Double)
 * OldCnlStat      - previous channel status                 (Byte)
 * NewCnlVal       - new channel value                       (Double)
 * NewCnlStat      - new channel status                      (Byte)
 * Checked         - event is checked                        (Boolean)
 * UserID          - ID of the user checked the event        (UInt16)
 * Descr           - event description                       (Byte + Byte[100])
 * Data            - auxiliary event data                    (Byte + Byte[50])
 *
 * The size of one event equals 189 bytes.
 */

using System;
using System.Data;
using System.IO;
using System.Text;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Adapter for reading and writing event tables
    /// <para>Адаптер для чтения и записи таблиц событий</para>
    /// </summary>
    public class EventAdapter : Adapter
    {
        /// <summary>
        /// Размер данных события в файле
        /// </summary>
        public const int EventDataSize = 189;
        /// <summary>
        /// Макс. длина описания события
        /// </summary>
        public const int MaxDescrLen = 100;
        /// <summary>
        /// Макс. длина дополнительных данных события
        /// </summary>
        public const int MaxDataLen = 50;


        /// <summary>
        /// Конструктор
        /// </summary>
        public EventAdapter()
            : base()
        {
        }


        /// <summary>
        /// Записать строку в буфер, в 0-й байт записывается длина данных.
        /// </summary>
        protected void StrToBytes(string s, byte[] buffer, int index, int maxStrLen)
        {
            if (string.IsNullOrEmpty(s))
            {
                buffer[index] = 0;
            }
            else
            {
                if (s.Length > maxStrLen)
                    s = s.Substring(0, maxStrLen);

                byte[] strBuf = Encoding.Default.GetBytes(s);
                buffer[index] = (byte)strBuf.Length;
                Array.Copy(strBuf, 0, buffer, index + 1, strBuf.Length);
            }
        }

        /// <summary>
        /// Преобразовать массив байт в строку, 0-й байт - длина данных.
        /// </summary>
        protected string BytesToStr(byte[] buffer, int index)
        {
            int dataLen = buffer[index++];

            if (dataLen > 0)
            {
                if (index + dataLen > buffer.Length)
                    dataLen = buffer.Length - index - 1;

                return Encoding.Default.GetString(buffer, index, dataLen);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Преобразовать объект в целое число
        /// </summary>
        protected int ConvertToInt(object obj)
        {
            try { return Convert.ToInt32(obj); }
            catch { return 0; }
        }

        /// <summary>
        /// Преобразовать объект в вещественное число
        /// </summary>
        protected double ConvertToDouble(object obj)
        {
            try { return Convert.ToDouble(obj); }
            catch { return 0.0; }
        }

        /// <summary>
        /// Преобразовать объект в дату и время
        /// </summary>
        protected DateTime ConvertToDateTime(object obj)
        {
            try { return Convert.ToDateTime(obj); }
            catch { return DateTime.MinValue; }
        }

        /// <summary>
        /// Преобразовать объект в логическое значение
        /// </summary>
        protected bool ConvertToBoolean(object obj)
        {
            try { return Convert.ToBoolean(obj); }
            catch { return false; }
        }

        /// <summary>
        /// Создать данные события на основе строки таблицы
        /// </summary>
        protected EventTableLight.Event CreateEvent(DataRowView rowView)
        {
            EventTableLight.Event ev = new EventTableLight.Event();
            ev.Number = ConvertToInt(rowView["Number"]);
            ev.DateTime = ConvertToDateTime(rowView["DateTime"]);
            ev.ObjNum = ConvertToInt(rowView["ObjNum"]);
            ev.KPNum = ConvertToInt(rowView["KPNum"]);
            ev.ParamID = ConvertToInt(rowView["ParamID"]);
            ev.CnlNum = ConvertToInt(rowView["CnlNum"]);
            ev.OldCnlVal = ConvertToDouble(rowView["OldCnlVal"]);
            ev.OldCnlStat = ConvertToInt(rowView["OldCnlStat"]);
            ev.NewCnlVal = ConvertToDouble(rowView["NewCnlVal"]);
            ev.NewCnlStat = ConvertToInt(rowView["NewCnlStat"]);
            ev.Checked = ConvertToBoolean(rowView["Checked"]);
            ev.UserID = ConvertToInt(rowView["UserID"]);
            ev.Descr = Convert.ToString(rowView["Descr"]);
            ev.Data = Convert.ToString(rowView["Data"]);
            return ev;
        }

        /// <summary>
        /// Создать буфер для записи события
        /// </summary>
        protected byte[] CreateEventBuffer(EventTableLight.Event ev)
        {
            byte[] evBuf = new byte[EventDataSize];
            Array.Copy(BitConverter.GetBytes(ScadaUtils.EncodeDateTime(ev.DateTime)), 0, evBuf, 0, 8);
            evBuf[8] = (byte)(ev.ObjNum % 256);
            evBuf[9] = (byte)(ev.ObjNum / 256);
            evBuf[10] = (byte)(ev.KPNum % 256);
            evBuf[11] = (byte)(ev.KPNum / 256);
            evBuf[12] = (byte)(ev.ParamID % 256);
            evBuf[13] = (byte)(ev.ParamID / 256);
            evBuf[14] = (byte)(ev.CnlNum % 256);
            evBuf[15] = (byte)(ev.CnlNum / 256);
            Array.Copy(BitConverter.GetBytes(ev.OldCnlVal), 0, evBuf, 16, 8);
            evBuf[24] = (byte)ev.OldCnlStat;
            Array.Copy(BitConverter.GetBytes(ev.NewCnlVal), 0, evBuf, 25, 8);
            evBuf[33] = (byte)ev.NewCnlStat;
            evBuf[34] = ev.Checked ? (byte)1 : (byte)0;
            evBuf[35] = (byte)(ev.UserID % 256);
            evBuf[36] = (byte)(ev.UserID / 256);
            StrToBytes(ev.Descr, evBuf, 37, MaxDescrLen);
            StrToBytes(ev.Data, evBuf, 138, MaxDataLen);
            return evBuf;
        }

        /// <summary>
        /// Заполнить объект dest из файла событий FileName
        /// </summary>
        protected void FillObj(object dest)
        {
            Stream stream = null;
            BinaryReader reader = null;
            DateTime fillTime = DateTime.Now;

            EventTableLight eventTableLight = null;
            DataTable dataTable = null;

            try
            {
                if (dest is EventTableLight)
                    eventTableLight = dest as EventTableLight;
                else if (dest is DataTable)
                    dataTable = dest as DataTable;
                else
                    throw new ScadaException("Destination object is invalid.");

                // определение даты событий в таблице
                DateTime date = ExtractDate(tableName);

                // подготовка объекта для хранения данных
                if (eventTableLight != null)
                {
                    eventTableLight.Clear();
                    eventTableLight.TableName = tableName;
                }
                else // dataTable != null
                {
                    // формирование структуры таблицы
                    dataTable.BeginLoadData();
                    dataTable.DefaultView.Sort = "";

                    if (dataTable.Columns.Count == 0)
                    {
                        dataTable.Columns.Add("Number", typeof(int));
                        dataTable.Columns.Add("DateTime", typeof(DateTime)).DefaultValue = date;
                        dataTable.Columns.Add("ObjNum", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("KPNum", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("ParamID", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("CnlNum", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("OldCnlVal", typeof(double)).DefaultValue = 0.0;
                        dataTable.Columns.Add("OldCnlStat", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("NewCnlVal", typeof(double)).DefaultValue = 0.0;
                        dataTable.Columns.Add("NewCnlStat", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("Checked", typeof(bool)).DefaultValue = false;
                        dataTable.Columns.Add("UserID", typeof(int)).DefaultValue = 0;
                        dataTable.Columns.Add("Descr", typeof(string));
                        dataTable.Columns.Add("Data", typeof(string));
                        dataTable.DefaultView.AllowNew = false;
                        dataTable.DefaultView.AllowEdit = false;
                        dataTable.DefaultView.AllowDelete = false;
                    }
                    else
                    {
                        DataColumn colDateTime = dataTable.Columns["DateTime"];
                        if (colDateTime != null)
                            colDateTime.DefaultValue = date;
                        dataTable.Rows.Clear();
                    }
                }

                // заполнение таблицы из файла
                stream = ioStream == null ?
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) :
                    ioStream;
                reader = new BinaryReader(stream);

                Byte[] eventBuf = new byte[EventDataSize]; // буфер данных события
                int evNum = 1; // порядковый номер события

                while (stream.Position < stream.Length)
                {
                    int readSize = reader.Read(eventBuf, 0, EventDataSize);
                    if (readSize == EventDataSize)
                    {
                        // создание события на основе считанных данных
                        EventTableLight.Event ev = new EventTableLight.Event();
                        ev.Number = evNum;
                        evNum++;

                        double time = BitConverter.ToDouble(eventBuf, 0);
                        ev.DateTime = ScadaUtils.CombineDateTime(date, time);

                        ev.ObjNum = BitConverter.ToUInt16(eventBuf, 8);
                        ev.KPNum = BitConverter.ToUInt16(eventBuf, 10);
                        ev.ParamID = BitConverter.ToUInt16(eventBuf, 12);
                        ev.CnlNum = BitConverter.ToUInt16(eventBuf, 14);
                        ev.OldCnlVal = BitConverter.ToDouble(eventBuf, 16);
                        ev.OldCnlStat = eventBuf[24];
                        ev.NewCnlVal = BitConverter.ToDouble(eventBuf, 25);
                        ev.NewCnlStat = eventBuf[33];
                        ev.Checked = eventBuf[34] > 0;
                        ev.UserID = BitConverter.ToUInt16(eventBuf, 35);
                        ev.Descr = BytesToStr(eventBuf, 37);
                        ev.Data = BytesToStr(eventBuf, 138);

                        // создание строки заполняемой таблицы
                        if (eventTableLight != null)
                        {
                            eventTableLight.AllEvents.Add(ev); // быстрее, чем eventTableLight.AddEvent(ev)
                        }
                        else // dataTable != null
                        {
                            DataRow row = dataTable.NewRow();
                            row["Number"] = ev.Number;
                            row["DateTime"] = ev.DateTime;
                            row["ObjNum"] = ev.ObjNum;
                            row["KPNum"] = ev.KPNum;
                            row["ParamID"] = ev.ParamID;
                            row["CnlNum"] = ev.CnlNum;
                            row["OldCnlVal"] = ev.OldCnlVal;
                            row["OldCnlStat"] = ev.OldCnlStat;
                            row["NewCnlVal"] = ev.NewCnlVal;
                            row["NewCnlStat"] = ev.NewCnlStat;
                            row["Checked"] = ev.Checked;
                            row["UserID"] = ev.UserID;
                            row["Descr"] = ev.Descr;
                            row["Data"] = ev.Data;
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // нормальная ситуация окончания файла
            }
            catch
            {
                fillTime = DateTime.MinValue;
                throw;
            }
            finally
            {
                if (fileMode)
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Close();
                }

                if (eventTableLight != null)
                {
                    eventTableLight.LastFillTime = fillTime;
                }
                else if (dataTable != null)
                {
                    dataTable.EndLoadData();
                    dataTable.AcceptChanges();
                    dataTable.DefaultView.Sort = "Number";
                }
            }
        }


        /// <summary>
        /// Заполнить таблицу dataTable из файла или потока
        /// </summary>
        public void Fill(DataTable dataTable)
        {
            FillObj(dataTable);
        }

        /// <summary>
        /// Заполнить таблицу eventTableLight из файла или потока
        /// </summary>
        public void Fill(EventTableLight eventTableLight)
        {
            FillObj(eventTableLight);
        }
        
        /// <summary>
        /// Записать изменения таблицы dataTable в файл или поток
        /// </summary>
        public void Update(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream == null ?
                   new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite) :
                   ioStream;
                writer = new BinaryWriter(stream);

                // запись изменённых событий
                DataView dataView = new DataView(dataTable, "", "", DataViewRowState.ModifiedCurrent);

                foreach (DataRowView rowView in dataView)
                {
                    EventTableLight.Event ev = CreateEvent(rowView);
                    
                    if (ev.Number > 0)
                    {
                        stream.Seek((ev.Number - 1) * EventDataSize, SeekOrigin.Begin);
                        writer.Write(CreateEventBuffer(ev));
                    }
                }

                // запись добавленных событий
                dataView = new DataView(dataTable, "", "", DataViewRowState.Added);

                if (dataView.Count > 0)
                {
                    // установка позиции записи кратной размеру данных события
                    stream.Seek(0, SeekOrigin.End);
                    int evInd = (int)(stream.Position / EventDataSize);
                    int evNum = evInd + 1;
                    stream.Seek(evInd * EventDataSize, SeekOrigin.Begin);

                    // запись событий и установка номеров событий
                    foreach (DataRowView rowView in dataView)
                    {
                        EventTableLight.Event ev = CreateEvent(rowView);
                        writer.Write(CreateEventBuffer(ev));
                        rowView["Number"] = evNum++;
                    }
                }

                // подтверждение успешного сохранения изменений
                dataTable.AcceptChanges();
            }
            finally
            {
                if (fileMode)
                {
                    if (writer != null)
                        writer.Close();
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        /// <summary>
        /// Добавить событие в файл или поток
        /// </summary>
        public void AppendEvent(EventTableLight.Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException("ev");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream == null ?
                   new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite) :
                   ioStream;
                writer = new BinaryWriter(stream);

                // установка позиции записи кратной размеру данных события
                stream.Seek(0, SeekOrigin.End);
                long evInd = stream.Position / EventDataSize;
                long offset = evInd * EventDataSize;
                stream.Seek(offset, SeekOrigin.Begin);

                // запись события
                writer.Write(CreateEventBuffer(ev));
                ev.Number = (int)evInd + 1;
            }
            finally
            {
                if (fileMode)
                {
                    if (writer != null)
                        writer.Close();
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        /// <summary>
        /// Квитировать событие в файле или потоке
        /// </summary>
        public void CheckEvent(int evNum, int userID)
        {
             Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream == null ?
                   new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite) :
                   ioStream;
                writer = new BinaryWriter(stream);

                stream.Seek(0, SeekOrigin.End);
                long size = stream.Position;
                long offset = (evNum - 1) * EventDataSize + 34;

                if (0 <= offset && offset + 2 < size)
                {
                    stream.Seek(offset, SeekOrigin.Begin);
                    writer.Write(userID > 0 ? (byte)1 : (byte)0);
                    writer.Write((ushort)userID);
                }
            }
            finally
            {
                if (fileMode)
                {
                    if (writer != null)
                        writer.Close();
                    if (stream != null)
                        stream.Close();
                }
            }
        }


        /// <summary>
        /// Построить имя таблицы событий на основе даты
        /// </summary>
        public static string BuildEvTableName(DateTime date)
        {
            return (new StringBuilder())
                .Append("e").Append(date.ToString("yyMMdd")).Append(".dat")
                .ToString();
        }
    }
}