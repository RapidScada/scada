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
 * Module   : SCADA-Administrator
 * Summary  : Export and import the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada;
using Scada.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ScadaAdmin
{
    /// <summary>
    /// Export and import the configuration database
    /// <para>Экспорт и импорт базы конфигурации</para>
    /// </summary>
    public static class ExportImport
    {
        /// <summary>
        /// Экспортировать таблицу базы конфигурации
        /// </summary>
        public static bool ExportTable(Tables.TableInfo srcTableInfo, string destFileName, out string msg)
        {
            return ExportTable(srcTableInfo, destFileName, 0, int.MaxValue, out msg);
        }

        /// <summary>
        /// Экспортировать таблицу базы конфигурации, ограничив диапазон идентификаторов
        /// </summary>
        public static bool ExportTable(Tables.TableInfo srcTableInfo, string destFileName, int minID, int maxID, out string msg)
        {
            try
            {
                // проверка аргументов метода
                if (srcTableInfo == null)
                    throw new ArgumentNullException("srcTableInfo");

                if (string.IsNullOrEmpty(destFileName))
                    throw new ArgumentException("???", "destFileName");  // !!! todo

                string dir = Path.GetDirectoryName(destFileName);
                if (string.IsNullOrEmpty(dir))
                    throw new DirectoryNotFoundException(AppPhrases.ExportDirUndefied);

                if (!Directory.Exists(dir))
                    throw new DirectoryNotFoundException(AppPhrases.ExportDirNotExists);

                // получение таблицы
                DataTable srcTable = srcTableInfo.GetTable();

                // ограничение диапазона идентификаторов
                if ((0 < minID || maxID < int.MaxValue) && srcTableInfo.IdColName != "")
                    srcTable.DefaultView.RowFilter = string.Format("{0} <= {2} and {2} <= {1}", minID, maxID, srcTableInfo.IdColName);

                // сохранение таблицы в формате DAT
                BaseAdapter adapter = new BaseAdapter();
                adapter.FileName = destFileName;
                adapter.Update(srcTable);
                msg = AppPhrases.ExportCompleted;
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.ExportError + ":\r\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Передать базу конфигурации серверу, т.е конвертировать из формата SDF в DAT
        /// </summary>
        public static bool PassBase(List<Tables.TableInfo> tableInfoList, string baseDATDir, out string msg)
        {
            try
            {
                // проверка аргументов метода
                if (tableInfoList == null)
                    throw new ArgumentNullException("tableInfoList");

                if (!Directory.Exists(baseDATDir))
                    throw new DirectoryNotFoundException(CommonPhrases.BaseDATDirNotExists);

                // создание файла блокировки базы конфигурации
                string baseLockPath = baseDATDir + "baselock";
                FileStream baseLockStream = new FileStream(baseLockPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                baseLockStream.Close();

                try
                {
                    // сохранение таблиц в формате DAT
                    BaseAdapter adapter = new BaseAdapter();
                    foreach (Tables.TableInfo tableInfo in Tables.TableInfoList)
                    {
                        DataTable table = tableInfo.GetTable();
                        adapter.FileName = baseDATDir + tableInfo.FileName;
                        adapter.Update(table);
                    }
                }
                finally
                {
                    // удаление файла блокировки базы конфигурации
                    File.Delete(baseLockPath);
                }

                msg = AppPhrases.DbPassCompleted;
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.DbPassError + ":\r\n" + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Импортировать таблицу базы конфигурации
        /// </summary>
        public static bool ImportTable()
        {
            return false;
        }

        /// <summary>
        /// Импортировать патч базы конфигурации
        /// </summary>
        public static bool ImportBasePatch(string srcFileName, Tables.TableInfo destTableInfo, out string msg)
        {
            msg = "";
            return false;
        }
    }
}
