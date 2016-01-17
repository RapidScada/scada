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
 * Summary  : Adapter for reading and writing configuration database tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2016
 * 
 * --------------------------------
 * Table file structure (version 3)
 * --------------------------------
 * 
 * Header (3 bytes):
 * FieldCnt  - field count         (Byte)
 * Reserve1  - reserve             (UInt16)
 * 
 * Field definition (110 bytes):
 * DataType  - data type           (Byte)
 * DataSize  - data size           (UInt16)
 * MaxStrLen - max. string length  (UInt16)
 * AllowNull - NULL allowed        (Byte)
 * NameLen   - name length <= 100  (UInt16)
 * Name      - name, ASCII string  (Byte[100])
 * Reserve2  - reserve             (UInt16)
 * 
 * Table row:
 * Reserve3     - reserve          (UInt16)
 * Data1..DataN - row data
 * If a field allows NULL, a flag (1 byte) is written before the field data.
 * If a field value equals NULL, the appropriate data block is filled by the default.
 * 
 * Data types and data representation:
 * type 0: integer                 (Int32)
 * type 1: float                   (Double)
 * type 2: boolean                 (Byte)
 *   0 - false, otherwise true
 * type 3: date and time           (Double)
 *   Delphi data format is used
 * type 4: UTF-8 string with maximum data size UInt16.MaxValue - 2
 *   String data size              (UInt16)
 *   String data                   (Byte[])
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
            /// Получить или установить максимальную длину строки для строкового типа
            /// </summary>
            public int MaxStrLen { get; set; }
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
        protected const int FieldDefSize = 110;
        /// <summary>
        /// Максимальная длина имени поля
        /// </summary>
        protected const int MaxFieldNameLen = 100;
        /// <summary>
        /// Максимально допустимая длина данных для сохранения строкового значения поля
        /// </summary>
        protected const int MaxStringDataSize = ushort.MaxValue - 2;
        /// <summary>
        /// Максимально допустимая длина (количество символов) строкового значения поля
        /// </summary>
        protected static readonly int MaxStringLen = Encoding.UTF8.GetMaxCharCount(MaxStringDataSize);

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
                    int strDataSize = BitConverter.ToUInt16(bytes, index);
                    index += 2;
                    if (index + strDataSize > bytes.Length)
                        strDataSize = bytes.Length - index;
                    // для данных в кодировке ASCII метод Encoding.UTF8.GetString() тоже будет корректно работать
                    return strDataSize > 0 ? Encoding.UTF8.GetString(bytes, index, strDataSize) : "";
                default:
                    return DBNull.Value;
            }
        }

        /// <summary>
        /// Преобразовать и записать в массив байт строку в заданной кодировке
        /// </summary>
        protected static void ConvertStr(string s, int maxLen, byte[] buffer, int index, Encoding encoding)
        {
            byte[] strData;
            int strDataSize;

            if (string.IsNullOrEmpty(s))
            {
                strData = null;
                strDataSize = 0;
            }
            else
            {
                if (s.Length > maxLen)
                    s = s.Substring(0, maxLen);

                strData = encoding.GetBytes(s);
                strDataSize = strData.Length;
            }

            Array.Copy(BitConverter.GetBytes((ushort)strDataSize), 0, buffer, index, 2);
            if (strData != null)
                Array.Copy(strData, 0, buffer, index + 2, strDataSize);
            Array.Clear(buffer, index + 2 + strDataSize, maxLen - strDataSize);
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
                    byte[] fieldDefBuf = new byte[FieldDefSize];

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        // загрузка данных определения поля в буфер для увеличения скорости работы
                        int readSize = reader.Read(fieldDefBuf, 0, FieldDefSize);

                        // заполение определения поля из буфера
                        if (readSize == FieldDefSize)
                        {
                            FieldDef fieldDef = new FieldDef();
                            fieldDef.DataType = fieldDefBuf[0];
                            fieldDef.DataSize = BitConverter.ToUInt16(fieldDefBuf, 1);
                            fieldDef.MaxStrLen = BitConverter.ToUInt16(fieldDefBuf, 3);
                            fieldDef.AllowNull = fieldDefBuf[5] > 0;
                            fieldDef.Name = (string)BytesToObj(fieldDefBuf, 6, DataTypes.String);
                            if (string.IsNullOrEmpty(fieldDef.Name))
                                throw new Exception("Field name must not be empty.");
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
                                    column.MaxLength = fieldDef.MaxStrLen;
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
                    byte[] rowBuf = new byte[recSize];
                    while (stream.Position < stream.Length)
                    {
                        // загрузка данных строки таблицы в буфер для увеличения скорости работы
                        int readSize = reader.Read(rowBuf, 0, recSize);

                        // заполение строки таблицы из буфера
                        if (readSize == recSize)
                        {
                            DataRow row = dataTable.NewRow();
                            int bufInd = 2;
                            foreach (FieldDef fieldDef in fieldDefs)
                            {
                                bool isNull = fieldDef.AllowNull ? rowBuf[bufInd++] > 0 : false;
                                int colInd = dataTable.Columns.IndexOf(fieldDef.Name);
                                if (colInd >= 0)
                                    row[colInd] = allowNulls && isNull ?
                                        DBNull.Value : BytesToObj(rowBuf, bufInd, fieldDef.DataType);
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
                    byte[] fieldDefBuf = new byte[FieldDefSize];
                    fieldDefBuf[FieldDefSize - 1] = fieldDefBuf[FieldDefSize - 2] = 0; // резерв

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        FieldDef fieldDef = new FieldDef();
                        DataColumn col = dataTable.Columns[i];
                        Type type = col.DataType;

                        if (type == typeof(int))
                        {
                            fieldDef.DataType = DataTypes.Integer;
                            fieldDef.DataSize = sizeof(int);
                            fieldDef.MaxStrLen = 0;
                        }
                        else if (type == typeof(double))
                        {
                            fieldDef.DataType = DataTypes.Double;
                            fieldDef.DataSize = sizeof(double);
                            fieldDef.MaxStrLen = 0;
                        }
                        else if (type == typeof(bool))
                        {
                            fieldDef.DataType = DataTypes.Boolean;
                            fieldDef.DataSize = 1;
                            fieldDef.MaxStrLen = 0;
                        }
                        else if (type == typeof(DateTime))
                        {
                            fieldDef.DataType = DataTypes.DateTime;
                            fieldDef.DataSize = sizeof(double);
                            fieldDef.MaxStrLen = 0;
                        }
                        else // String
                        {
                            fieldDef.DataType = DataTypes.String;
                            int maxLen = Math.Min(col.MaxLength, MaxStringLen);
                            fieldDef.DataSize = Encoding.UTF8.GetMaxByteCount(maxLen);
                            fieldDef.MaxStrLen = maxLen;
                        }

                        fieldDef.Name = col.ColumnName;
                        fieldDef.AllowNull = col.AllowDBNull;

                        recSize += fieldDef.DataSize;
                        if (fieldDef.AllowNull)
                            recSize++;
                        fieldDefs[i] = fieldDef;

                        fieldDefBuf[0] = (byte)fieldDef.DataType;
                        Array.Copy(BitConverter.GetBytes((ushort)fieldDef.DataSize), 0, fieldDefBuf, 1, 2);
                        Array.Copy(BitConverter.GetBytes((ushort)fieldDef.MaxStrLen), 0, fieldDefBuf, 3, 2);
                        fieldDefBuf[5] = fieldDef.AllowNull ? (byte)1 : (byte)0;
                        ConvertStr(fieldDef.Name, MaxFieldNameLen, fieldDefBuf, 6, Encoding.ASCII);

                        writer.Write(fieldDefBuf);
                    }

                    // запись строк
                    byte[] rowBuf = new byte[recSize];
                    rowBuf[0] = rowBuf[1] = 0; // резерв
                    foreach (DataRowView rowView in dataTable.DefaultView)
                    {
                        DataRow row = rowView.Row;
                        int bufInd = 2;

                        foreach (FieldDef fieldDef in fieldDefs)
                        {
                            int colInd = dataTable.Columns.IndexOf(fieldDef.Name);
                            object val = colInd >= 0 ? row[colInd] : null;
                            bool isNull = val == null || val == DBNull.Value;

                            if (fieldDef.AllowNull)
                                rowBuf[bufInd++] = isNull ? (byte)1 : (byte)0;

                            switch (fieldDef.DataType)
                            {
                                case DataTypes.Integer:
                                    int intVal = isNull ? 0 : (int)val;
                                    Array.Copy(BitConverter.GetBytes(intVal), 0, rowBuf, bufInd, fieldDef.DataSize);
                                    break;
                                case DataTypes.Double:
                                    double dblVal = isNull ? 0.0 : (double)val;
                                    Array.Copy(BitConverter.GetBytes(dblVal), 0, rowBuf, bufInd, fieldDef.DataSize);
                                    break;
                                case DataTypes.Boolean:
                                    rowBuf[bufInd] = (byte)(isNull ? 0 : (bool)val ? 1 : 0);
                                    break;
                                case DataTypes.DateTime:
                                    double dtVal = isNull ? 0.0 : Arithmetic.EncodeDateTime((DateTime)val);
                                    Array.Copy(BitConverter.GetBytes(dtVal), 0, rowBuf, bufInd, fieldDef.DataSize);
                                    break;
                                default:
                                    string strVal = isNull ? "" : val.ToString();
                                    ConvertStr(strVal, fieldDef.MaxStrLen, rowBuf, bufInd, Encoding.UTF8);
                                    break;
                            }

                            bufInd += fieldDef.DataSize;
                        }

                        writer.Write(rowBuf);
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
