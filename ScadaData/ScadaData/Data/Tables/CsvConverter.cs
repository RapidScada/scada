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
 * Summary  : Provides conversion of various data sources to CSV format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Provides conversion of various data sources to CSV format.
    /// <para>Обеспечивает преобразование различных источников в формат CSV.</para>
    /// </summary>
    public class CsvConverter
    {
        /// <summary>
        /// Indicates that the converter uses a particular file.
        /// </summary>
        protected bool fileMode;

        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CsvConverter(string fileName)
        {
            fileMode = true;
            FileName = fileName;
            Stream = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CsvConverter(Stream stream)
        {
            fileMode = false;
            Stream = stream ?? throw new ArgumentNullException("stream");
            FileName = "";
        }


        /// <summary>
        /// Gets the destination file name.
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Gets the destination stream.
        /// </summary>
        public Stream Stream { get; protected set; }


        /// <summary>
        /// Gets the appropriate stream.
        /// </summary>
        protected Stream GetStream()
        {
            return fileMode ?
                new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite) :
                Stream;
        }

        /// <summary>
        /// Writes the specified value in CSV format.
        /// </summary>
        protected void WriteValue(StreamWriter writer, object val)
        {
            if (val != null && val != DBNull.Value)
            {
                if (val is bool)
                {
                    writer.Write(val.ToString().ToLowerInvariant());
                }
                else if (val is double dbl)
                {
                    writer.Write(dbl.ToString(Localization.Culture));
                }
                else if (val is DateTime dt)
                {
                    writer.Write(dt.ToString(Localization.Culture));
                }
                else if (val is string str)
                {
                    writer.Write("\"");
                    writer.Write(str.Replace("\"", "\"\""));
                    writer.Write("\"");
                }
                else
                {
                    writer.Write(val);
                }
            }
        }

        /// <summary>
        /// Converts the specified data table to CSV.
        /// </summary>
        public void ConvertToCsv(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");

            Stream stream = GetStream();
            StreamWriter writer = null;

            try
            {
                if (dataTable.DefaultView.Count > 0)
                {
                    // prepare a writer
                    writer = new StreamWriter(stream, Encoding.Default);

                    // output the table header
                    int colCnt = dataTable.Columns.Count;
                    for (int i = 0; i < colCnt; i++)
                    {
                        if (i > 0)
                            writer.Write(";");
                        writer.Write("[" + dataTable.Columns[i].ColumnName + "]");
                    }
                    writer.WriteLine();

                    // output the table rows
                    foreach (DataRowView rowView in dataTable.DefaultView)
                    {
                        for (int i = 0; i < colCnt; i++)
                        {
                            if (i > 0)
                                writer.Write(";");
                            WriteValue(writer, rowView[i]);
                        }
                        writer.WriteLine();
                    }
                }
            }
            finally
            {
                if (fileMode)
                {
                    writer?.Dispose();
                    stream?.Dispose();
                }
            }
        }

        /// <summary>
        /// Converts the specified configuration database table to CSV.
        /// </summary>
        public void ConvertToCsv(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            Stream stream = GetStream();
            StreamWriter writer = null;

            try
            {
                if (baseTable.ItemCount > 0)
                {
                    // prepare a writer
                    writer = new StreamWriter(stream, Encoding.Default);

                    // output the table header
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(baseTable.ItemType);
                    int propCnt = props.Count;
                    for (int i = 0; i < propCnt; i++)
                    {
                        if (i > 0)
                            writer.Write(";");
                        writer.Write("[" + props[i].Name + "]");
                    }
                    writer.WriteLine();

                    // output the table rows
                    foreach (object item in baseTable.EnumerateItems())
                    {
                        for (int i = 0; i < propCnt; i++)
                        {
                            if (i > 0)
                                writer.Write(";");
                            WriteValue(writer, props[i].GetValue(item));
                        }
                        writer.WriteLine();
                    }
                }
            }
            finally
            {
                if (fileMode)
                {
                    writer?.Dispose();
                    stream?.Dispose();
                }
            }
        }

        /// <summary>
        /// Converts the specified snapshot table to CSV.
        /// </summary>
        public void ConvertToCsv(SrezTable srezTable)
        {
            if (srezTable == null)
                throw new ArgumentNullException("srezTable");

            Stream stream = GetStream();
            StreamWriter writer = null;

            try
            {
                int srezCnt = srezTable.SrezList.Count;

                if (srezCnt > 0)
                {
                    // output the channels present in the last snapshot
                    int[] cnlNums = srezTable.SrezList.Values[srezCnt - 1].CnlNums;

                    // prepare a writer
                    writer = new StreamWriter(stream);

                    // output the table header
                    writer.Write("[DateTime]");
                    foreach (int cnlNum in cnlNums)
                    {
                        writer.Write(";[" + cnlNum + "]");
                    }
                    writer.WriteLine();

                    // output the data
                    foreach (SrezTableLight.Srez srez in srezTable.SrezList.Values)
                    {
                        writer.Write(srez.DateTime.ToString(Localization.Culture));

                        foreach (int cnlNum in cnlNums)
                        {
                            writer.Write(";");
                            SrezTableLight.CnlData cnlData = srez.GetCnlData(cnlNum);
                            if (cnlData.Stat > 0)
                                writer.Write(cnlData.Val.ToString(Localization.Culture));
                        }

                        writer.WriteLine();
                    }
                }
            }
            finally
            {
                if (fileMode)
                {
                    writer?.Dispose();
                    stream?.Dispose();
                }
            }
        }
    }
}
