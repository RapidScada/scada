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
 * Summary  : Adapter for reading and writing data tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2019
 * 
 * --------------------------------
 * Table file structure (version 2)
 * --------------------------------
 * File consists of snapshots, each snapshot has the structure:
 * N               - channel number list length          (UInt16)
 * n0..nN-1        - channel number list                 (N * UInt16)
 * CS              - check sum of the previous data      (UInt16)
 * Time            - snapshot time stamp                 (Double)
 * {Value, Status} - snapshot data                       (N * (Double + Byte))
 * 
 * Channel numbers in the list are unique and sorted in ascending order.
 * If the channel number list equals the previous list, it is skipped and N is set to 0.
 * CS = (UInt16)(N + n0 + ... + nN-1 + 1)
 */

using System;
using System.Data;
using System.IO;
using System.Text;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Adapter for reading and writing data tables
    /// <para>Адаптер для чтения и записи таблиц срезов</para>
    /// </summary>
    public class SrezAdapter : Adapter
    {
        /// <summary>
        /// Имя таблицы текущего среза
        /// </summary>
        public const string CurTableName = "current.dat";
        /// <summary>
        /// Буфер пустого списка номеров каналов в формате сохранения
        /// </summary>
        protected static readonly byte[] EmptyCnlNumsBuf = new byte[] { 0x00, 0x00, 0x01, 0x00 };


        /// <summary>
        /// Конструктор
        /// </summary>
        public SrezAdapter()
            : base()
        {
        }


        /// <summary>
        /// Извлечь данные входного канала из бинарного буфера
        /// </summary>
        protected void ExtractCnlData(byte[] buf, ref int bufInd, out double cnlVal, out byte cnlStat)
        {
            cnlVal = BitConverter.ToDouble(buf, bufInd);
            cnlStat = buf[bufInd + 8];
            bufInd += 9;
        }

        /// <summary>
        /// Получить буфер описания структуры среза в формате сохранения
        /// </summary>
        protected byte[] GetSrezDescrBuf(SrezTable.SrezDescr srezDescr)
        {
            ushort cnlNumsLen = (ushort)srezDescr.CnlNums.Length;
            byte[] cnlNumsBuf = new byte[cnlNumsLen * 2 + 4];
            cnlNumsBuf[0] = (byte)(cnlNumsLen % 256);
            cnlNumsBuf[1] = (byte)(cnlNumsLen / 256);
            int bufPos = 2;

            for (int i = 0; i < cnlNumsLen; i++)
            {
                ushort cnlNum = (ushort)srezDescr.CnlNums[i];
                cnlNumsBuf[bufPos++] = (byte)(cnlNum % 256);
                cnlNumsBuf[bufPos++] = (byte)(cnlNum / 256);
            }

            cnlNumsBuf[bufPos++] = (byte)(srezDescr.CS % 256);
            cnlNumsBuf[bufPos++] = (byte)(srezDescr.CS / 256);

            return cnlNumsBuf;
        }

        /// <summary>
        /// Получить буфер данных среза в формате сохранения
        /// </summary>
        protected byte[] GetCnlDataBuf(SrezTable.CnlData[] cnlData)
        {
            int cnlCnt = cnlData.Length;
            byte[] srezDataBuf = new byte[cnlCnt * 9];

            for (int i = 0, k = 0; i < cnlCnt; i++)
            {
                SrezTable.CnlData data = cnlData[i];
                BitConverter.GetBytes(data.Val).CopyTo(srezDataBuf, k);
                srezDataBuf[k + 8] = (byte)data.Stat;
                k += 9;
            }

            return srezDataBuf;
        }

        /// <summary>
        /// Заполнить объект dest из файла срезов FileName
        /// </summary>
        protected void FillObj(object dest)
        {
            Stream stream = null;
            BinaryReader reader = null;
            DateTime fillTime = DateTime.Now;

            SrezTableLight srezTableLight = dest as SrezTableLight;
            DataTable dataTable = dest as DataTable;
            Trend trend = dest as Trend;

            SrezTable srezTable = srezTableLight as SrezTable;
            SrezTableLight.Srez lastStoredSrez = null;

            try
            {
                if (srezTableLight == null && dataTable == null && trend == null)
                    throw new ScadaException("Destination object is invalid.");

                // подготовка объекта для хранения данных
                if (srezTableLight != null)
                {
                    srezTableLight.Clear();
                    srezTableLight.TableName = tableName;

                    if (srezTable != null)
                        srezTable.BeginLoadData();
                }
                else if (dataTable != null)
                {
                    // формирование структуры таблицы
                    dataTable.BeginLoadData();
                    dataTable.DefaultView.Sort = "";

                    if (dataTable.Columns.Count == 0)
                    {
                        dataTable.Columns.Add("DateTime", typeof(DateTime));
                        dataTable.Columns.Add("CnlNum", typeof(int));
                        dataTable.Columns.Add("Val", typeof(double));
                        dataTable.Columns.Add("Stat", typeof(int));
                        dataTable.DefaultView.AllowNew = false;
                        dataTable.DefaultView.AllowEdit = false;
                        dataTable.DefaultView.AllowDelete = false;
                    }
                    else
                    {
                        dataTable.Rows.Clear();
                    }
                }
                else // trend != null
                {
                    trend.Clear();
                    trend.TableName = tableName;
                }

                // заполнение объекта данными
                stream = ioStream ?? new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream);

                DateTime date = ExtractDate(tableName); // определение даты срезов
                SrezTable.SrezDescr srezDescr = null;   // описание среза
                int[] cnlNums = null; // ссылка на номера входных каналов из описания среза
                while (stream.Position < stream.Length)
                {
                    // считывание списка номеров каналов и КС
                    int cnlNumCnt = reader.ReadUInt16();
                    if (cnlNumCnt > 0)
                    {
                        // загрузка номеров каналов в буфер для увеличения скорости работы
                        int cnlNumSize = cnlNumCnt * 2;
                        byte[] buf = new byte[cnlNumSize];
                        int readSize = reader.Read(buf, 0, cnlNumSize);

                        // создание описания среза и заполнение номеров каналов из буфера 
                        // с проверкой их уникальности и упорядоченности
                        if (readSize == cnlNumSize)
                        {
                            int prevCnlNum = -1;
                            srezDescr = new SrezTable.SrezDescr(cnlNumCnt);
                            cnlNums = srezDescr.CnlNums;
                            for (int i = 0; i < cnlNumCnt; i++)
                            {
                                int cnlNum = BitConverter.ToUInt16(buf, i * 2);
                                if (prevCnlNum >= cnlNum)
                                    throw new ScadaException("Table is incorrect.");
                                cnlNums[i] = prevCnlNum = cnlNum;
                            }
                            srezDescr.CalcCS();
                        }
                    }
                    else if (srezDescr == null)
                    {
                        throw new Exception("Table is incorrect.");
                    }

                    // считывание и проверка КС
                    ushort cs = reader.ReadUInt16();
                    bool csOk = cnlNumCnt > 0 ? srezDescr.CS == cs : cs == 1;

                    // считывание данных среза
                    int cnlCnt = cnlNums.Length;   // количество каналов в срезе
                    int srezDataSize = cnlCnt * 9; // размер данных среза
                    if (csOk)
                    {
                        long srezPos = stream.Position;
                        double time = reader.ReadDouble();
                        DateTime srezDT = ScadaUtils.CombineDateTime(date, time);

                        // инициализация нового среза
                        SrezTableLight.Srez srez;
                        if (srezTable != null)
                        {
                            srez = new SrezTable.Srez(srezDT, srezDescr) 
                            { 
                                State = DataRowState.Unchanged,
                                Position = srezPos
                            };
                        }
                        else if (srezTableLight != null)
                        {
                            srez = new SrezTableLight.Srez(srezDT, cnlCnt);
                            cnlNums.CopyTo(srez.CnlNums, 0);
                        }
                        else // srezTableLight == null
                        {
                            srez = null;
                        }

                        // считывание данных входных каналов
                        int bufInd = 0;
                        double val;
                        byte stat;
                        if (trend != null)
                        {
                            // выбор данных требуемого канала для тренда
                            int index = Array.BinarySearch<int>(cnlNums, trend.CnlNum);
                            if (index >= 0)
                            {
                                stream.Seek(index * 9, SeekOrigin.Current);
                                byte[] buf = new byte[9];
                                int readSize = reader.Read(buf, 0, 9);
                                if (readSize == 9)
                                {
                                    ExtractCnlData(buf, ref bufInd, out val, out stat);
                                    Trend.Point point = new Trend.Point(srezDT, val, stat);
                                    trend.Points.Add(point);
                                    stream.Seek(srezDataSize - (index + 1) * 9, SeekOrigin.Current);
                                }
                            }
                            else
                            {
                                stream.Seek(srezDataSize, SeekOrigin.Current);
                            }
                        }
                        else
                        {
                            // загрузка данных среза в буфер для увеличения скорости работы
                            byte[] buf = new byte[srezDataSize];
                            int readSize = reader.Read(buf, 0, srezDataSize);

                            // заполение таблицы срезов из буфера
                            if (srezTableLight != null)
                            {
                                for (int i = 0; i < cnlCnt; i++)
                                {
                                    ExtractCnlData(buf, ref bufInd, out val, out stat);

                                    srez.CnlNums[i] = cnlNums[i];
                                    srez.CnlData[i].Val = val;
                                    srez.CnlData[i].Stat = stat;

                                    if (bufInd >= readSize)
                                        break;
                                }

                                srezTableLight.AddSrez(srez);
                                lastStoredSrez = srez;
                            }
                            else // dataTable != null
                            {
                                for (int i = 0; i < cnlCnt; i++)
                                {
                                    ExtractCnlData(buf, ref bufInd, out val, out stat);

                                    DataRow row = dataTable.NewRow();
                                    row["DateTime"] = srezDT;
                                    row["CnlNum"] = cnlNums[i];
                                    row["Val"] = val;
                                    row["Stat"] = stat;
                                    dataTable.Rows.Add(row);

                                    if (bufInd >= readSize)
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // пропустить срез, считая его размер так, как при повторяющемся списке номеров каналов
                        stream.Seek(srezDataSize + 8, SeekOrigin.Current);
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

                if (srezTableLight != null)
                {
                    srezTableLight.LastFillTime = fillTime;
                    if (srezTable != null)
                    {
                        srezTable.LastStoredSrez = (SrezTable.Srez)lastStoredSrez;
                        srezTable.EndLoadData();
                    }
                }
                else if (dataTable != null)
                {
                    dataTable.EndLoadData();
                    dataTable.AcceptChanges();
                    dataTable.DefaultView.Sort = "DateTime, CnlNum";
                }
                else if (trend != null)
                {
                    trend.LastFillTime = fillTime;
                    trend.Sort();
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
        /// Заполнить таблицу srezTableLight из файла или потока
        /// </summary>
        public void Fill(SrezTableLight srezTableLight)
        {
            FillObj(srezTableLight);
        }

        /// <summary>
        /// Заполнить тренд trend канала cnlNum из файла или потока
        /// </summary>
        public void Fill(Trend trend)
        {
            FillObj(trend);
        }

        /// <summary>
        /// Создать таблицу, состоящую из одного среза, в файле или потоке
        /// </summary>
        /// <remarks>Для записи таблицы текущего среза</remarks>
        public void Create(SrezTable.Srez srez, DateTime srezDT)
        {
            if (srez == null)
                throw new ArgumentNullException("srez");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream ?? 
                    new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream);

                writer.Write(GetSrezDescrBuf(srez.SrezDescr));
                writer.Write(ScadaUtils.EncodeDateTime(srezDT));
                writer.Write(GetCnlDataBuf(srez.CnlData));
                stream.SetLength(stream.Position);
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
        /// Записать изменения таблицы срезов в файл или поток
        /// </summary>
        public void Update(SrezTable srezTable)
        {
            if (srezTable == null)
                throw new ArgumentNullException("srezTable");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream ??
                    new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream);

                // запись изменённых срезов
                foreach (SrezTable.Srez srez in srezTable.ModifiedSrezList)
                {
                    stream.Seek(srez.Position + 8, SeekOrigin.Begin);
                    writer.Write(GetCnlDataBuf(srez.CnlData));
                }

                // установка позиции записи добавленных срезов в поток, 
                // восстановление таблицы срезов в случае необходимости
                SrezTable.Srez lastSrez = srezTable.LastStoredSrez;

                if (lastSrez == null)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    stream.Seek(0, SeekOrigin.End);
                    long offset = lastSrez.Position + lastSrez.CnlNums.Length * 9 + 8;

                    if (stream.Position < offset)
                    {
                        byte[] buf = new byte[offset - stream.Position];
                        stream.Write(buf, 0, buf.Length);
                    }
                    else
                    {
                        stream.Seek(offset, SeekOrigin.Begin);
                    }
                }

                // запись добавленных срезов
                SrezTable.SrezDescr prevSrezDescr = lastSrez?.SrezDescr;

                foreach (SrezTable.Srez srez in srezTable.AddedSrezList)
                {
                    // запись номеров каналов среза
                    if (srez.SrezDescr.Equals(prevSrezDescr))
                        writer.Write(EmptyCnlNumsBuf);
                    else
                        writer.Write(GetSrezDescrBuf(srez.SrezDescr));

                    prevSrezDescr = srez.SrezDescr;

                    // запись данных среза
                    srez.Position = stream.Position;
                    writer.Write(ScadaUtils.EncodeDateTime(srez.DateTime));
                    writer.Write(GetCnlDataBuf(srez.CnlData));
                    lastSrez = srez;
                }

                // подтверждение успешного сохранения изменений
                srezTable.AcceptChanges();
                srezTable.LastStoredSrez = lastSrez;
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
        /// Построить имя таблицы минутных срезов на основе даты
        /// </summary>
        public static string BuildMinTableName(DateTime date)
        {
            return (new StringBuilder())
                .Append("m").Append(date.ToString("yyMMdd")).Append(".dat")
                .ToString();
        }

        /// <summary>
        /// Построить имя таблицы часовых срезов на основе даты
        /// </summary>
        public static string BuildHourTableName(DateTime date)
        {
            return (new StringBuilder())
                .Append("h").Append(date.ToString("yyMMdd")).Append(".dat")
                .ToString();
        }
    }
}