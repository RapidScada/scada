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
 * Module   : ScadaData
 * Summary  : Adapter for reading and writing configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2013
 * 
 * --------------------------------
 * Table file structure (version 2)
 * --------------------------------
 * 
 * Header (3 bytes):
 * FieldCnt  - field count       (Byte)
 * Reserve1  - reserve           (UInt16)
 * 
 * Field definition (107 bytes):
 * DataType  - data type          (Byte)
 * DataSize  - data size          (UInt16)
 * AllowNull - NULL allowed       (Byte)
 * NameLen   - name length <= 100 (UInt16)
 * Name      - name               (Byte[100])
 * Reserve2  - reserve            (UInt16)
 * 
 * Table row:
 * Reserve3     - reserve         (UInt16)
 * Data1..DataN - data
 * If a field allows NULL, a flag (1 byte) is written before the field data.
 * If a field value equals NULL, the appropriate data block is filled by the default.
 * 
 * Data types:
 * type 0: integer                 (Int32)
 * type 1: float                   (Double)
 * type 2: boolean                 (Byte)
 *   0 - false, otherwise true
 * type 3: date and time           (Double)
 *   Delphi time format is used
 * type 4: string with maximum length LMax <= UInt16.MaxValue - 2
 *   L   - string length           (UInt16)
 *   Str - string characters       (Byte[LMax])
 */

using System;
using System.IO;
using System.Data;
using System.Text;

namespace Scada.Data
{
    /// <summary>
    /// Adapter for reading and writing configuration database tables
    /// <para>Адаптер для чтения и записи таблиц базы конфигурации</para>
    /// </summary>
    public class BaseAdapter
    {
        /// <summary>
        /// Определение поля таблицы
        /// </summary>
        protected struct FieldDef
        {
            /// <summary>
            /// Получить или установить имя поля
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить тип данных
            /// </summary>
            public int DataType { get; set; }
            /// <summary>
            /// Получить или установить размер данных
            /// </summary>
            public int DataSize { get; set; }
            /// <summary>
            /// Получить или установить признак допустимости NULL
            /// </summary>
            public bool AllowNull { get; set; }
        }

        /// <summary>
        /// Типы данных полей таблицы
        /// </summary>
        protected struct DataTypes
        {
            /// <summary>
            /// Целый
            /// </summary>
            public const int Integer = 0;
            /// <summary>
            /// Вещественный
            /// </summary>
            public const int Double = 1;
            /// <summary>
            /// Логический
            /// </summary>
            public const int Boolean = 2;
            /// <summary>
            /// Дата и время
            /// </summary>
            public const int DateTime = 3;
            /// <summary>
            /// Строковый
            /// </summary>
            public const int String = 4;
        }

        /// <summary>
        /// Длина определения поля
        /// </summary>
        private const int FieldDefSize = 107;
        /// <summary>
        /// Максимальная длина имени поля
        /// </summary>
        private const int MaxFieldNameLen = 100;
        /// <summary>
        /// Максимальная длина строкового значения поля
        /// </summary>
        private const int MaxStringLen = ushort.MaxValue - 2;

        /// <summary>
        /// Директория базы конфигурации
        /// </summary>
        protected string directory;
        /// <summary>
        /// Входной и выходной поток
        /// </summary>
        protected Stream ioStream;
        /// <summary>
        /// Имя файла таблицы базы конфигурации
        /// </summary>
        protected string tableName;
        /// <summary>
        /// Полное имя файла таблицы базы конфигурации
        /// </summary>
        protected string fileName;
        /// <summary>
        /// Доступ к данным выполняется через файл на диске
        /// </summary>
        protected bool fileMode;


        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseAdapter()
        {
            directory = "";
            ioStream = null;
            tableName = "";
            fileName = "";
            fileMode = true;
        }


        /// <summary>
        /// Получить или установить директорию базы конфигурации
        /// </summary>
        public string Directory
        {
            get
            {
                return directory;
            }
            set
            {
                ioStream = null;
                fileMode = true;
                if (directory != value)
                {
                    directory = value;
                    fileName = directory + tableName;
                }
            }
        }

        /// <summary>
        /// Получить или установить входной и выходной поток (вместо директории)
        /// </summary>
        public Stream Stream
        {
            get
            {
                return ioStream;
            }
            set
            {
                directory = "";
                ioStream = value;
                fileName = tableName;
                fileMode = false;
            }
        }

        /// <summary>
        /// Получить или установить имя файла таблицы базы конфигурации
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    fileName = directory + tableName;
                }
            }
        }

        /// <summary>
        /// Получить или установить полное имя файла таблицы базы конфигурации
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (fileName != value)
                {
                    directory = Path.GetDirectoryName(value);
                    ioStream = null;
                    tableName = Path.GetFileName(value);
                    fileName = value;
                    fileMode = true;
                }
            }
        }


        /// <summary>
        /// Преобразовать массив байт в значение, тип которого задан dataType
        /// </summary>
        protected static object BytesToObj(byte[] bytes, int index, int dataType)
        {
            switch (dataType)
            {
                case DataTypes.Integer:
                    return BitConverter.ToInt32(bytes, index);
                case DataTypes.Double:
                    return BitConverter.ToDouble(bytes, index);
                case DataTypes.Boolean:
                    return bytes[index] > 0;
                case DataTypes.DateTime:
                    return Arithmetic.DecodeDateTime(BitConverter.ToDouble(bytes, index));
                case DataTypes.String:
                    int len = BitConverter.ToUInt16(bytes, index);
                    index += 2;
                    if (index + len > bytes.Length)
                        len = bytes.Length - index;
                    return len > 0 ? Encoding.Default.GetString(bytes, index, len) : "";
                default:
                    return DBNull.Value;
            }
        }

        /// <summary>
        /// Преобразовать строку и записать в массив байт
        /// </summary>
        protected static void ConvertStr(string s, int maxLen, byte[] buffer, int index)
        {
            int len;
            if (string.IsNullOrEmpty(s))
            {
                len = 0;
            }
            else
            {
                if (s.Length > maxLen)
                    s = s.Substring(0, maxLen);
                len = s.Length;
            }

            Array.Copy(BitConverter.GetBytes((ushort)len), 0, buffer, index, 2);
            Array.Copy(Encoding.Default.GetBytes(s), 0, buffer, index + 2, len);
            for (int i = len; i < maxLen; i++)
                buffer[index + i + 2] = 0;
        }


        /// <summary>
        /// Заполнить таблицу dataTable из файла или потока
        /// </summary>
        public void Fill(DataTable dataTable, bool allowNulls)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                stream = ioStream == null ?
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) :
                    ioStream;
                reader = new BinaryReader(stream);

                // считывание заголовка
                byte fieldCnt = reader.ReadByte(); // количество полей
                stream.Seek(2, SeekOrigin.Current);

                if (fieldCnt > 0)
                {
                    // считывание определений полей
                    FieldDef[] fieldDefs = new FieldDef[fieldCnt];
                    int recSize = 2; // размер строки в файле
                    byte[] buf = new byte[FieldDefSize];

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        // загрузка данных определения поля в буфер для увеличения скорости работы
                        int readSize = reader.Read(buf, 0, FieldDefSize);

                        // заполение определения поля из буфера
                        if (readSize == FieldDefSize)
                        {
                            FieldDef fieldDef = new FieldDef();
                            fieldDef.DataType = buf[0];
                            fieldDef.DataSize = BitConverter.ToUInt16(buf, 1);
                            fieldDef.AllowNull = buf[3] > 0;
                            fieldDef.Name = (string)BytesToObj(buf, 4, DataTypes.String);
                            if (string.IsNullOrEmpty(fieldDef.Name))
                                throw new Exception("Field name can't be empty.");
                            fieldDefs[i] = fieldDef;

                            recSize += fieldDef.DataSize;
                            if (fieldDef.AllowNull)
                                recSize++;
                        }
                    }

                    // формирование структуры таблицы
                    dataTable.BeginLoadData();
                    dataTable.DefaultView.Sort = "";

                    if (dataTable.Columns.Count == 0)
                    {
                        foreach (FieldDef fieldDef in fieldDefs)
                        {
                            DataColumn column = new DataColumn(fieldDef.Name);
                            column.AllowDBNull = fieldDef.AllowNull;

                            switch (fieldDef.DataType)
                            {
                                case DataTypes.Integer:
                                    column.DataType = typeof(int);
                                    break;
                                case DataTypes.Double:
                                    column.DataType = typeof(double);
                                    break;
                                case DataTypes.Boolean:
                                    column.DataType = typeof(bool);
                                    break;
                                case DataTypes.DateTime:
                                    column.DataType = typeof(DateTime);
                                    break;
                                default:
                                    column.DataType = typeof(string);
                                    column.MaxLength = fieldDef.DataSize - 2;
                                    break;
                            }

                            dataTable.Columns.Add(column);
                        }

                        dataTable.DefaultView.AllowNew = false;
                        dataTable.DefaultView.AllowEdit = false;
                        dataTable.DefaultView.AllowDelete = false;
                    }
                    else
                    {
                        dataTable.Rows.Clear();
                    }

                    // считывание строк
                    buf = new byte[recSize];
                    while (stream.Position < stream.Length)
                    {
                        // загрузка данных строки таблицы в буфер для увеличения скорости работы
                        int readSize = reader.Read(buf, 0, recSize);

                        // заполение строки таблицы из буфера
                        if (readSize == recSize)
                        {
                            DataRow row = dataTable.NewRow();
                            int bufInd = 2;
                            foreach (FieldDef fieldDef in fieldDefs)
                            {
                                bool isNull = fieldDef.AllowNull ? buf[bufInd++] > 0 : false;
                                int colInd = dataTable.Columns.IndexOf(fieldDef.Name);
                                if (colInd >= 0)
                                    row[colInd] = allowNulls && isNull ?
                                        DBNull.Value : BytesToObj(buf, bufInd, fieldDef.DataType);
                                bufInd += fieldDef.DataSize;
                            }
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // нормальная ситуация окончания файла
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

                dataTable.EndLoadData();
                dataTable.AcceptChanges();

                if (dataTable.Columns.Count > 0)
                    dataTable.DefaultView.Sort = dataTable.Columns[0].ColumnName;
            }
        }

        /// <summary>
        /// Записать таблицу dataTable в файл или поток
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
                    new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite) :
                    ioStream;
                writer = new BinaryWriter(stream, Encoding.Default);

                // запись заголовка
                byte fieldCnt = dataTable.Columns.Count > byte.MaxValue ? 
                    byte.MaxValue : (byte)dataTable.Columns.Count;
                writer.Write(fieldCnt);
                writer.Write((ushort)0); // резерв

                if (fieldCnt > 0)
                {
                    // формирование и запись определений полей
                    FieldDef[] fieldDefs = new FieldDef[fieldCnt];
                    int recSize = 2; // размер строки в файле
                    byte[] buf = new byte[FieldDefSize];
                    buf[FieldDefSize - 1] = buf[FieldDefSize - 2] = 0; // резерв

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        FieldDef fieldDef = new FieldDef();
                        DataColumn col = dataTable.Columns[i];
                        Type type = col.DataType;

                        if (type == typeof(int))
                        {
                            fieldDef.DataType = DataTypes.Integer;
                            fieldDef.DataSize = sizeof(int);
                        }
                        else if (type == typeof(double))
                        {
                            fieldDef.DataType = DataTypes.Double;
                            fieldDef.DataSize = sizeof(double);
                        }
                        else if (type == typeof(bool))
                        {
                            fieldDef.DataType = DataTypes.Boolean;
                            fieldDef.DataSize = 1;
                        }
                        else if (type == typeof(DateTime))
                        {
                            fieldDef.DataType = DataTypes.DateTime;
                            fieldDef.DataSize = sizeof(double);
                        }
                        else
                        {
                            fieldDef.DataType = DataTypes.String;
                            int maxLen = col.MaxLength;
                            fieldDef.DataSize = 0 < maxLen && maxLen <= MaxStringLen ? 
                                (ushort)maxLen + 2 : (ushort)MaxStringLen + 2;
                        }

                        fieldDef.AllowNull = col.AllowDBNull;
                        fieldDef.Name = col.ColumnName;

                        recSize += fieldDef.DataSize;
                        if (fieldDef.AllowNull)
                            recSize++;
                        fieldDefs[i] = fieldDef;

                        buf[0] = (byte)fieldDef.DataType;
                        Array.Copy(BitConverter.GetBytes((ushort)fieldDef.DataSize), 0, buf, 1, 2);
                        buf[3] = fieldDef.AllowNull ? (byte)1 : (byte)0;
                        ConvertStr(fieldDef.Name, MaxFieldNameLen, buf, 4);

                        writer.Write(buf);
                    }

                    // запись строк
                    buf = new byte[recSize];
                    buf[0] = buf[1] = 0; // резерв
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int bufInd = 2;

                        foreach (FieldDef fieldDef in fieldDefs)
                        {
                            int colInd = dataTable.Columns.IndexOf(fieldDef.Name);
                            object val = colInd >= 0 ? row[colInd] : null;
                            bool isNull = val == null || val == DBNull.Value;

                            if (fieldDef.AllowNull)
                                buf[bufInd++] = isNull ? (byte)1 : (byte)0;

                            switch (fieldDef.DataType)
                            {
                                case DataTypes.Integer:
                                    int intVal = isNull ? 0 : (int)val;
                                    Array.Copy(BitConverter.GetBytes(intVal), 0, buf, bufInd, fieldDef.DataSize);
                                    break;
                                case DataTypes.Double:
                                    double dblVal = isNull ? 0.0 : (double)val;
                                    Array.Copy(BitConverter.GetBytes(dblVal), 0, buf, bufInd, fieldDef.DataSize);
                                    break;
                                case DataTypes.Boolean:
                                    buf[bufInd] = (byte)(isNull ? 0 : (bool)val ? 1 : 0);
                                    break;
                                case DataTypes.DateTime:
                                    double dtVal = isNull ? 0.0 : Arithmetic.EncodeDateTime((DateTime)val);
                                    Array.Copy(BitConverter.GetBytes(dtVal), 0, buf, bufInd, fieldDef.DataSize);
                                    break;
                                default:
                                    string strVal = isNull ? "" : val.ToString();
                                    ConvertStr(strVal, fieldDef.DataSize - 2, buf, bufInd);
                                    break;
                            }

                            bufInd += fieldDef.DataSize;
                        }

                        writer.Write(buf);
                    }
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
    }
}
