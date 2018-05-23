/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Import and export the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2017
 */

using Ionic.Zip;
using Scada;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Text;
using System.Threading;

namespace ScadaAdmin
{
    /// <summary>
    /// Import and export the configuration database
    /// <para>Импорт и экспорт базы конфигурации</para>
    /// </summary>
    internal static class ImportExport
    {
        /// <summary>
        /// Задержка перед созданием архива для надежного закрытия соединения с БД, мс
        /// </summary>
        private const int ZipDelay = 200;


        /// <summary>
        /// Вывести в журнал столбцы таблицы
        /// </summary>
        private static void WriteColumns(StreamWriter writer, DataTable dataTable, string subTitle)
        {
            AppUtils.WriteTitle(writer, subTitle);
            if (dataTable.Columns.Count > 0)
            {
                foreach (DataColumn column in dataTable.Columns)
                    writer.WriteLine(column.ColumnName + " (" + column.DataType + ")");
            }
            else
            {
                writer.WriteLine(AppPhrases.NoColumns);
            }
            writer.WriteLine();
        }

        /// <summary>
        /// Импортировать таблицу
        /// </summary>
        private static void ImportTable(DataTable srcTable, Tables.TableInfo destTableInfo, int shiftID, 
            StreamWriter writer, out int updRowCnt, out int errRowCnt, out string msg)
        {
            // определение режима импорта: только добавление строк или добавление/обновление
            string idColName = destTableInfo.IDColName;
            bool tryToUpdate = idColName != "" && srcTable.Columns.Contains(idColName);

            // получение таблицы, в которую производится импорт
            DataTable destTable;
            if (tryToUpdate)
            {
                // заполение столбцов и данных таблицы
                destTable = destTableInfo.GetTable();
            }
            else
            {
                // заполение столбцов таблицы
                destTable = new DataTable(destTableInfo.Name);
                Tables.FillTableSchema(destTable);
            }

            // вывод заголовка и стобцов в журнал импорта
            if (writer != null)
            {
                writer.WriteLine();
                AppUtils.WriteTitle(writer, string.Format(AppPhrases.ImportTableTitle, 
                    destTableInfo.Name + " (" + destTableInfo.Header + ")"));
                writer.WriteLine();
                WriteColumns(writer, srcTable, AppPhrases.SrcTableColumns);
                WriteColumns(writer, destTable, AppPhrases.DestTableColumns);
            }

            // заполнение таблицы в формате SDF
            foreach (DataRowView srcRowView in srcTable.DefaultView)
            {
                DataRow srcRow = srcRowView.Row;
                DataRow destRow = null;

                if (tryToUpdate)
                {
                    object curID = srcRow[idColName];
                    object newID = curID is int ? (int)curID + shiftID : curID;
                    int rowInd = destTable.DefaultView.Find(newID); // таблица отсортирована по ключу
                    if (rowInd >= 0)
                        destRow = destTable.DefaultView[rowInd].Row;
                }

                if (destRow == null)
                {
                    destRow = destTable.NewRow();
                    destTable.Rows.Add(destRow);
                }

                foreach (DataColumn destColumn in destTable.Columns)
                {
                    int ind = srcTable.Columns.IndexOf(destColumn.ColumnName);
                    if (ind >= 0 && destColumn.DataType == srcTable.Columns[ind].DataType)
                    {
                        object val = srcRow[ind];
                        destRow[destColumn] = destColumn.ColumnName == idColName && shiftID > 0 ?
                            (int)val /*ID*/ + shiftID : val;
                    }
                }
            }

            // сохранение информации в базе конфигурации в формате SDF
            updRowCnt = 0;
            errRowCnt = 0;
            DataRow[] errRows = null;

            try
            {
                SqlCeDataAdapter sqlAdapter = destTable.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
                updRowCnt = sqlAdapter.Update(destTable);
            }
            catch (Exception ex)
            {
                throw new Exception(AppPhrases.WriteDBError + ":\r\n" + ex.Message);
            }

            // обработка ошибок
            if (destTable.HasErrors)
            {
                errRows = destTable.GetErrors();
                errRowCnt = errRows.Length;
            }

            msg = errRowCnt == 0 ? string.Format(AppPhrases.ImportTableCompleted, updRowCnt) :
                string.Format(AppPhrases.ImportTableCompletedWithErr, updRowCnt, errRowCnt);

            // вывод результата и ошибок в журнал импорта
            if (writer != null)
            {
                AppUtils.WriteTitle(writer, AppPhrases.ImportTableResult);
                writer.WriteLine(msg);

                if (errRowCnt > 0)
                {
                    writer.WriteLine();
                    AppUtils.WriteTitle(writer, AppPhrases.ImportTableErrors);

                    foreach (DataRow row in errRows)
                    {
                        if (idColName != "")
                            writer.Write(idColName + " = " + row[idColName] + " : ");
                        writer.WriteLine(row.RowError);
                    }
                }
            }
        }


        /// <summary>
        /// Импортировать таблицу базы конфигурации из файла формата DAT
        /// </summary>
        public static bool ImportTable(string srcFileName, Tables.TableInfo destTableInfo, 
            int minID, int maxID, int newMinID, string logFileName, out bool logCreated, out string msg)
        {
            // проверка аргументов метода
            if (srcFileName == null)
                throw new ArgumentNullException("srcFileName");

            if (destTableInfo == null)
                throw new ArgumentNullException("destTableInfo");

            if (logFileName == null)
                throw new ArgumentNullException("logFileName");

            logCreated = false;
            StreamWriter writer = null;

            try
            {
                // создание журнала импорта и вывод параметров импорта
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;
                AppUtils.WriteTitle(writer, DateTime.Now.ToString("G", Localization.Culture) + " " + 
                    AppPhrases.ImportTitle);
                writer.WriteLine(AppPhrases.ImportSource + srcFileName);

                // проверка существования импортируемого файла
                if (!File.Exists(srcFileName))
                    throw new FileNotFoundException(AppPhrases.ImportFileNotExist);

                // загрузка импортируемой таблицы в формате DAT
                BaseAdapter baseAdapter = new BaseAdapter();
                DataTable srcTable = new DataTable();
                baseAdapter.FileName = srcFileName;

                try
                {
                    baseAdapter.Fill(srcTable, true);
                }
                catch (Exception ex)
                {
                    throw new Exception(AppPhrases.LoadTableError + ":\r\n" + ex.Message);
                }

                // ограничение диапазона идентификаторов
                string idColName = destTableInfo.IDColName;
                if ((0 < minID || maxID < int.MaxValue) && idColName != "")
                    srcTable.DefaultView.RowFilter = string.Format("{0} <= {2} and {2} <= {1}",
                        minID, maxID, idColName);
                int shiftID = newMinID > 0 ? newMinID - minID : 0;

                // импорт таблицы
                int updRowCnt;
                int errRowCnt;
                ImportTable(srcTable, destTableInfo, shiftID, writer, out updRowCnt, out errRowCnt, out msg);

                if (updRowCnt > 0)
                    msg += AppPhrases.RefreshRequired;
                return errRowCnt == 0;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.ImportTableError + ":\r\n" + ex.Message;
                try { if (logCreated) writer.WriteLine(msg); }
                catch { }
                return false;
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }
        }

        /// <summary>
        /// Импортировать все таблицы базы конфигурации из заданной директории
        /// </summary>
        public static bool ImportAllTables(string srcDir, List<Tables.TableInfo> destTableInfoList,
            string logFileName, out bool logCreated, out string msg)
        {
            // проверка аргументов метода
            if (srcDir == null)
                throw new ArgumentNullException("srcDir");

            if (destTableInfoList == null)
                throw new ArgumentNullException("destTableInfoList");

            if (logFileName == null)
                throw new ArgumentNullException("logFileName");

            logCreated = false;
            StreamWriter writer = null;

            try
            {
                // создание журнала импорта и вывод параметров импорта
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;
                AppUtils.WriteTitle(writer, DateTime.Now.ToString("G", Localization.Culture) + " " +
                    AppPhrases.ImportTitle);
                writer.WriteLine(AppPhrases.ImportSource + srcDir);

                // проверка существования импортируемой директории
                if (!Directory.Exists(srcDir))
                    throw new DirectoryNotFoundException(AppPhrases.ImportDirNotExist);

                // импорт таблиц, файлы которых существуют в заданной директории
                int totalUpdRowCnt = 0;
                int totalErrRowCnt = 0;
                int updRowCnt;
                int errRowCnt;
                srcDir = ScadaUtils.NormalDir(srcDir);

                foreach (Tables.TableInfo destTableInfo in destTableInfoList)
                {
                    string srcFileName = srcDir + destTableInfo.FileName;

                    if (File.Exists(srcFileName))
                    {
                        // загрузка импортируемой таблицы в формате DAT
                        BaseAdapter baseAdapter = new BaseAdapter();
                        DataTable srcTable = new DataTable();
                        baseAdapter.FileName = srcFileName;

                        try
                        {
                            baseAdapter.Fill(srcTable, true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(AppPhrases.LoadTableError + ":\r\n" + ex.Message);
                        }

                        // импорт таблицы
                        string s;
                        ImportTable(srcTable, destTableInfo, 0, writer, out updRowCnt, out errRowCnt, out s);
                        totalUpdRowCnt += updRowCnt;
                        totalErrRowCnt += errRowCnt;
                    }
                }

                msg = totalErrRowCnt == 0 ? string.Format(AppPhrases.ImportCompleted, totalUpdRowCnt) :
                    string.Format(AppPhrases.ImportCompletedWithErr, totalUpdRowCnt, totalErrRowCnt);

                // вывод результата в журнал импорта
                writer.WriteLine();
                AppUtils.WriteTitle(writer, AppPhrases.ImportResult);
                writer.WriteLine(msg);

                if (totalUpdRowCnt > 0)
                    msg += AppPhrases.RefreshRequired;
                return totalErrRowCnt == 0;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.ImportAllTablesError + ":\r\n" + ex.Message;
                try { if (logCreated) writer.WriteLine(msg); }
                catch { }
                return false;
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }
        }

        /// <summary>
        /// Импортировать архив базы конфигурации (патч)
        /// </summary>
        public static bool ImportArchive(string srcFileName, List<Tables.TableInfo> destTableInfoList,
            string logFileName, out bool logCreated, out string msg)
        {
            // проверка аргументов метода
            if (srcFileName == null)
                throw new ArgumentNullException("srcFileName");

            if (destTableInfoList == null)
                throw new ArgumentNullException("destTableInfoList");

            if (logFileName == null)
                throw new ArgumentNullException("logFileName");

            logCreated = false;
            StreamWriter writer = null;

            try
            {
                // создание журнала импорта и вывод параметров импорта
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;
                AppUtils.WriteTitle(writer, DateTime.Now.ToString("G", Localization.Culture) + " " + 
                    AppPhrases.ImportTitle);
                writer.WriteLine(AppPhrases.ImportSource + srcFileName);

                // проверка существования импортируемого файла
                if (!File.Exists(srcFileName))
                    throw new FileNotFoundException(AppPhrases.ImportFileNotExist);

                using (ZipFile zipFile = ZipFile.Read(srcFileName))
                {
                    // получение словаря всех файлов архива с именами в нижнем регистре
                    Dictionary<string, ZipEntry> zipEntries = new Dictionary<string, ZipEntry>(zipFile.Count);
                    foreach (ZipEntry zipEntry in zipFile)
                    {
                        string fileName = zipEntry.FileName.ToLowerInvariant();
                        if (fileName.StartsWith("basedat/", StringComparison.Ordinal) ||
                            fileName.StartsWith("basedat\\", StringComparison.Ordinal))
                        {
                            fileName = fileName.Substring("basedat/".Length);
                            zipEntries.Add(fileName, zipEntry);
                        }
                    }

                    // импорт таблиц из тех, которые содержатся в архиве
                    int totalUpdRowCnt = 0;
                    int totalErrRowCnt = 0;
                    int updRowCnt;
                    int errRowCnt;

                    foreach (Tables.TableInfo destTableInfo in destTableInfoList)
                    {
                        if (zipEntries.ContainsKey(destTableInfo.FileName))
                        {
                            using (MemoryStream zipStream = new MemoryStream())
                            {
                                // распаковка архива в поток в памяти
                                ZipEntry zipEntry = zipEntries[destTableInfo.FileName];
                                zipEntry.Extract(zipStream);

                                // загрузка импортируемой таблицы в формате DAT из потока
                                BaseAdapter baseAdapter = new BaseAdapter();
                                DataTable srcTable = new DataTable();
                                baseAdapter.Stream = zipStream;
                                baseAdapter.Stream.Position = 0;

                                try
                                {
                                    baseAdapter.Fill(srcTable, true);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(AppPhrases.LoadTableError + ":\r\n" + ex.Message);
                                }

                                // импорт таблицы
                                string s;
                                ImportTable(srcTable, destTableInfo, 0, writer, out updRowCnt, out errRowCnt, out s);
                                totalUpdRowCnt += updRowCnt;
                                totalErrRowCnt += errRowCnt;
                            }
                        }
                    }

                    msg = totalErrRowCnt == 0 ? string.Format(AppPhrases.ImportCompleted, totalUpdRowCnt) :
                        string.Format(AppPhrases.ImportCompletedWithErr, totalUpdRowCnt, totalErrRowCnt);

                    // вывод результата в журнал импорта
                    writer.WriteLine();
                    AppUtils.WriteTitle(writer, AppPhrases.ImportResult);
                    writer.WriteLine(msg);

                    if (totalUpdRowCnt > 0)
                        msg += AppPhrases.RefreshRequired;
                    return totalErrRowCnt == 0;
                }
            }
            catch (Exception ex)
            {
                msg = AppPhrases.ImportArchiveError + ":\r\n" + ex.Message;
                try { if (logCreated) writer.WriteLine(msg); }
                catch { }
                return false;
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }
        }
        
        /// <summary>
        /// Экспортировать таблицу базы конфигурации в файл формата DAT
        /// </summary>
        public static bool ExportTable(Tables.TableInfo srcTableInfo, string destFileName, 
            int minID, int maxID, out string msg)
        {
            try
            {
                // проверка аргументов метода
                if (srcTableInfo == null)
                    throw new ArgumentNullException("srcTableInfo");

                if (string.IsNullOrWhiteSpace(destFileName))
                    throw new ArgumentException(AppPhrases.ExportFileUndefied);

                string dir = Path.GetDirectoryName(destFileName);
                if (string.IsNullOrWhiteSpace(dir))
                    throw new DirectoryNotFoundException(AppPhrases.ExportDirUndefied);

                if (!Directory.Exists(dir))
                    throw new DirectoryNotFoundException(AppPhrases.ExportDirNotExists);

                // получение таблицы
                DataTable srcTable = srcTableInfo.GetTable();

                // ограничение диапазона идентификаторов
                if ((0 < minID || maxID < int.MaxValue) && srcTableInfo.IDColName != "")
                    srcTable.DefaultView.RowFilter = string.Format("{0} <= {2} and {2} <= {1}", 
                        minID, maxID, srcTableInfo.IDColName);

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
        public static bool PassBase(List<Tables.TableInfo> srcTableInfoList, string baseDATDir, out string msg)
        {
            try
            {
                // проверка аргументов метода
                if (srcTableInfoList == null)
                    throw new ArgumentNullException("srcTableInfoList");

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
        /// Создать резервную копию файла базы конфигурации в формате SDF
        /// </summary>
        public static bool BackupSDF(string baseSDFFileName, string backupDir, out string msg)
        {
            try
            {
                // проверка аргументов метода
                if (!File.Exists(baseSDFFileName))
                    throw new FileNotFoundException(AppPhrases.BaseSDFFileNotExists);

                if (!Directory.Exists(backupDir))
                    throw new DirectoryNotFoundException(AppPhrases.BackupDirNotExists);

                // резервирование
                bool wasConnected = AppData.Connected;
                string backupFileName = backupDir + Path.GetFileNameWithoutExtension(baseSDFFileName) +
                    DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".zip";

                try
                {
                    if (wasConnected)
                    {
                        AppData.Conn.Close(); // для сохранения изменений
                        Thread.Sleep(ZipDelay);
                    }

                    // создание временной копии базы конфигурации
                    string tempFileName = backupDir + Path.GetFileName(baseSDFFileName);
                    File.Copy(baseSDFFileName, tempFileName, true);

                    // создание архива
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddFile(tempFileName, "");
                        zip.Save(backupFileName);
                    }

                    // удаление временного файла
                    File.Delete(tempFileName);
                }
                finally
                {
                    if (wasConnected)
                        AppData.Conn.Open();
                }

                msg = string.Format(AppPhrases.BackupCompleted, backupFileName);
                return true;
            }
            catch (Exception ex)
            {
                msg = AppPhrases.BackupError + ":\r\n" + ex.Message;
                return false;
            }
        }
    }
}