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
 * Summary  : The base class for adapter
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Globalization;
using System.IO;

namespace Scada.Data.Tables
{
    /// <summary>
    /// The base class for adapter
    /// <para>Базовый класс адаптера</para>
    /// </summary>
    public abstract class Adapter
    {
        /// <summary>
        /// Директория таблицы срезов
        /// </summary>
        protected string directory;
        /// <summary>
        /// Входной и выходной поток
        /// </summary>
        protected Stream ioStream;
        /// <summary>
        /// Имя файла таблицы срезов
        /// </summary>
        protected string tableName;
        /// <summary>
        /// Полное имя файла таблицы срезов
        /// </summary>
        protected string fileName;
        /// <summary>
        /// Доступ к данным выполняется через файл на диске
        /// </summary>
        protected bool fileMode;


        /// <summary>
        /// Конструктор
        /// </summary>
        public Adapter()
        {
            directory = "";
            ioStream = null;
            tableName = "";
            fileName = "";
            fileMode = true;
        }


        /// <summary>
        /// Получить или установить директорию таблицы срезов
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
        /// Получить или установить имя файла таблицы срезов
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
        /// Получить или установить полное имя файла таблицы срезов
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
        /// Извлечь дату из имени файла таблицы срезов или событий (без директории)
        /// </summary>
        protected DateTime ExtractDate(string tableName)
        {
            try
            {
                return DateTime.ParseExact(tableName.Substring(1, 6), "yyMMdd", CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
