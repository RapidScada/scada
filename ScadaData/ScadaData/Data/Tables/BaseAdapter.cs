/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Modified : 2018
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
using System.ComponentModel;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Adapter for reading and writing configuration database tables
    /// <para>Адаптер для чтения и записи таблиц базы конфигурации</para>
    /// </summary>
    public class BaseAdapter : Adapter
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
        /// Максимально допустимая длина (количество символов) имени поля
        /// </summary>
        protected const int MaxFieldNameLen = 100;
        /// <summary>
        /// Максимально допустимая длина данных для сохранения имени поля
        /// </summary>
        protected const int MaxFieldNameDataSize = MaxFieldNameLen + 2 /*запись длины*/;
        /// <summary>
        /// Максимально допустимая длина данных для сохранения строкового значения поля
        /// </summary>
        protected const int MaxStringDataSize = ushort.MaxValue - 2 /*запись длины*/;
        /// <summary>
        /// Максимально допустимая длина (количество символов) строкового значения поля
        /// </summary>
        protected static readonly int MaxStringLen = Encoding.UTF8.GetMaxCharCount(MaxStringDataSize);


        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseAdapter()
            : base()
        {
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
                    return ScadaUtils.DecodeDateTime(BitConverter.ToDouble(bytes, index));
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
        protected static void ConvertStr(string s, int maxLen, int maxDataSize, 
            byte[] buffer, int index, Encoding encoding)
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
            int totalStrDataSize = strDataSize + 2; // размер данных строки с учётом записи длины
            Array.Clear(buffer, index + totalStrDataSize, maxDataSize - totalStrDataSize);
        }

        /// <summary>
        /// Считать определения полей
        /// </summary>
        protected FieldDef[] ReadFieldDefs(Stream stream, BinaryReader reader, out int recSize)
        {
            // считывание заголовка
            byte fieldCnt = reader.ReadByte(); // количество полей
            stream.Seek(2, SeekOrigin.Current);

            FieldDef[] fieldDefs = new FieldDef[fieldCnt];
            recSize = 2; // размер строки в файле

            if (fieldCnt > 0)
            {
                // считывание определений полей
                byte[] fieldDefBuf = new byte[FieldDefSize];

                for (int i = 0; i < fieldCnt; i++)
                {
                    // загрузка данных определения поля в буфер для увеличения скорости работы
                    int readSize = reader.Read(fieldDefBuf, 0, FieldDefSize);

                    // заполение определения поля из буфера
                    if (readSize == FieldDefSize)
                    {
                        FieldDef fieldDef = new FieldDef
                        {
                            DataType = fieldDefBuf[0],
                            DataSize = BitConverter.ToUInt16(fieldDefBuf, 1),
                            MaxStrLen = BitConverter.ToUInt16(fieldDefBuf, 3),
                            AllowNull = fieldDefBuf[5] > 0,
                            Name = (string)BytesToObj(fieldDefBuf, 6, DataTypes.String)
                        };

                        if (string.IsNullOrEmpty(fieldDef.Name))
                            throw new ScadaException("Field name must not be empty.");

                        fieldDefs[i] = fieldDef;
                        recSize += fieldDef.DataSize;

                        if (fieldDef.AllowNull)
                            recSize++;
                    }
                }
            }

            return fieldDefs;
        }

        /// <summary>
        /// Создать определение поля
        /// </summary>
        protected FieldDef CreateFieldDef(string name, Type type, int maxLength, bool allowNull, ref int recSize)
        {
            FieldDef fieldDef = new FieldDef();

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
                int maxLen = Math.Min(maxLength, MaxStringLen);
                fieldDef.DataSize = 2 /*запись длины*/ + Encoding.UTF8.GetMaxByteCount(maxLen);
                fieldDef.MaxStrLen = maxLen;
            }

            fieldDef.Name = name;
            fieldDef.AllowNull = allowNull;

            recSize += fieldDef.DataSize;
            if (fieldDef.AllowNull)
                recSize++;

            return fieldDef;
        }

        /// <summary>
        /// Записать определение поля
        /// </summary>
        protected void WriteFieldDef(FieldDef fieldDef, BinaryWriter writer)
        {
            byte[] fieldDefBuf = new byte[FieldDefSize];
            fieldDefBuf[0] = (byte)fieldDef.DataType;
            Array.Copy(BitConverter.GetBytes((ushort)fieldDef.DataSize), 0, fieldDefBuf, 1, 2);
            Array.Copy(BitConverter.GetBytes((ushort)fieldDef.MaxStrLen), 0, fieldDefBuf, 3, 2);
            fieldDefBuf[5] = fieldDef.AllowNull ? (byte)1 : (byte)0;
            ConvertStr(fieldDef.Name, MaxFieldNameLen, MaxFieldNameDataSize, fieldDefBuf, 6, Encoding.ASCII);
            fieldDefBuf[FieldDefSize - 2] = fieldDefBuf[FieldDefSize - 1] = 0; // резерв

            writer.Write(fieldDefBuf);
        }

        /// <summary>
        /// Записать значение в буффер строки таблицы
        /// </summary>
        protected void WriteValueToRowBuffer(FieldDef fieldDef, object val, byte[] rowBuf, ref int bufInd)
        {
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
                    double dtVal = isNull ? 0.0 : ScadaUtils.EncodeDateTime((DateTime)val);
                    Array.Copy(BitConverter.GetBytes(dtVal), 0, rowBuf, bufInd, fieldDef.DataSize);
                    break;
                default:
                    string strVal = isNull ? "" : val.ToString();
                    ConvertStr(strVal, fieldDef.MaxStrLen, fieldDef.DataSize,
                        rowBuf, bufInd, Encoding.UTF8);
                    break;
            }

            bufInd += fieldDef.DataSize;
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
            dataTable.Rows.Clear();

            try
            {
                stream = ioStream ?? new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream);
                FieldDef[] fieldDefs = ReadFieldDefs(stream, reader, out int recSize);

                if (fieldDefs.Length > 0)
                {
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
                                {
                                    row[colInd] = allowNulls && isNull ?
                                        DBNull.Value : BytesToObj(rowBuf, bufInd, fieldDef.DataType);
                                }

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
                    reader?.Close();
                    stream?.Close();
                }

                dataTable.EndLoadData();
                dataTable.AcceptChanges();

                if (dataTable.Columns.Count > 0)
                    dataTable.DefaultView.Sort = dataTable.Columns[0].ColumnName;
            }
        }

        /// <summary>
        /// Заполнить таблицу baseTable из файла или потока
        /// </summary>
        public void Fill(IBaseTable baseTable, bool allowNulls)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = null;
            BinaryReader reader = null;

            baseTable.ClearItems();
            baseTable.IndexesEnabled = false;

            try
            {
                stream = ioStream ?? new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream);
                FieldDef[] fieldDefs = ReadFieldDefs(stream, reader, out int recSize);
                int fieldCnt = fieldDefs.Length;

                if (fieldCnt > 0)
                {
                    // получение свойств, соответствующих определениям полей
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                    PropertyDescriptor[] propArr = new PropertyDescriptor[fieldCnt];

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        propArr[i] = props[fieldDefs[i].Name];
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
                            object item = Activator.CreateInstance(baseTable.ItemType);
                            int bufInd = 2;

                            for (int fieldInd = 0; fieldInd < fieldCnt; fieldInd++)
                            {
                                FieldDef fieldDef = fieldDefs[fieldInd];
                                PropertyDescriptor prop = propArr[fieldInd];
                                bool isNull = fieldDef.AllowNull ? rowBuf[bufInd++] > 0 : false;

                                if (prop != null)
                                {
                                    object val = allowNulls && isNull ?
                                        null : BytesToObj(rowBuf, bufInd, fieldDef.DataType);
                                    prop.SetValue(item, val);
                                }

                                bufInd += fieldDef.DataSize;
                            }

                            baseTable.AddObject(item);
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
                    reader?.Close();
                    stream?.Close();
                }

                baseTable.IndexesEnabled = true;
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
                stream = ioStream ?? new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.Default);

                // запись заголовка
                byte fieldCnt = (byte)Math.Min(dataTable.Columns.Count, byte.MaxValue);
                writer.Write(fieldCnt);
                writer.Write((ushort)0); // резерв

                if (fieldCnt > 0)
                {
                    // формирование и запись определений полей
                    FieldDef[] fieldDefs = new FieldDef[fieldCnt];
                    int recSize = 2; // размер строки в файле

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        DataColumn col = dataTable.Columns[i];
                        FieldDef fieldDef = CreateFieldDef(col.ColumnName, col.DataType, 
                            col.MaxLength, col.AllowDBNull, ref recSize);
                        fieldDefs[i] = fieldDef;
                        WriteFieldDef(fieldDef, writer);
                    }

                    // запись строк
                    byte[] rowBuf = new byte[recSize];
                    rowBuf[0] = rowBuf[1] = 0; // резерв

                    foreach (DataRowView rowView in dataTable.DefaultView)
                    {
                        int bufInd = 2;

                        for (int i = 0; i < fieldCnt; i++)
                        {
                            WriteValueToRowBuffer(fieldDefs[i], rowView[i], rowBuf, ref bufInd);
                        }

                        writer.Write(rowBuf);
                    }
                }
            }
            finally
            {
                if (fileMode)
                {
                    writer?.Close();
                    stream?.Close();
                }
            }
        }

        /// <summary>
        /// Записать таблицу baseTable в файл или поток
        /// </summary>
        public void Update(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = null;
            BinaryWriter writer = null;

            try
            {
                stream = ioStream ?? new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.Default);

                // запись заголовка
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                byte fieldCnt = (byte)Math.Min(props.Count, byte.MaxValue);
                writer.Write(fieldCnt);
                writer.Write((ushort)0);

                if (fieldCnt > 0)
                {
                    // получение макс. длин строковых полей
                    int[] maxStrLenArr = new int[fieldCnt];

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        maxStrLenArr[i] = 0;
                        PropertyDescriptor prop = props[i];

                        if (prop.PropertyType == typeof(string))
                        {
                            foreach (object item in baseTable.EnumerateItems())
                            {
                                object val = prop.GetValue(item);
                                maxStrLenArr[i] = Math.Max(maxStrLenArr[i], val == null ? 0 : val.ToString().Length);
                            }
                        }
                    }

                    // формирование и запись определений полей
                    FieldDef[] fieldDefs = new FieldDef[fieldCnt];
                    int recSize = 2; // размер строки в файле

                    for (int i = 0; i < fieldCnt; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        bool isNullable = prop.PropertyType.IsNullable();
                        FieldDef fieldDef = CreateFieldDef(prop.Name, 
                            isNullable ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType,
                            maxStrLenArr[i], isNullable || prop.PropertyType.IsClass, ref recSize);
                        fieldDefs[i] = fieldDef;
                        WriteFieldDef(fieldDef, writer);
                    }

                    // запись строк
                    byte[] rowBuf = new byte[recSize];
                    rowBuf[0] = rowBuf[1] = 0;

                    foreach (object item in baseTable.EnumerateItems())
                    {
                        int bufInd = 2;

                        for (int i = 0; i < fieldCnt; i++)
                        {
                            WriteValueToRowBuffer(fieldDefs[i], props[i].GetValue(item), rowBuf, ref bufInd);
                        }

                        writer.Write(rowBuf);
                    }
                }
            }
            finally
            {
                if (fileMode)
                {
                    writer?.Close();
                    stream?.Close();
                }
            }
        }
    }
}
