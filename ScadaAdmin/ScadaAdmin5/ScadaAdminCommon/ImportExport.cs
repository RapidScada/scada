/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ScadaAdminCommon
 * Summary  : Import and export configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.Deployment;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using Interface = Scada.Data.Entities.Interface;

namespace Scada.Admin
{
    /// <summary>
    /// Import and export configuration.
    /// <para>Импорт и экспорт конфигурации.</para>
    /// </summary>
    public class ImportExport
    {
        /// <summary>
        /// The name of the archive entry that contains project information.
        /// </summary>
        private const string ProjectInfoEntryName = "Project.txt";


        /// <summary>
        /// Extracts the specified archive.
        /// </summary>
        private void ExtractArchive(string srcFileName, string destDir)
        {
            using (FileStream fileStream =
                new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry zipEntry in zipArchive.Entries)
                    {
                        string entryName = zipEntry.FullName.Replace('/', Path.DirectorySeparatorChar);
                        string absPath = Path.Combine(destDir, entryName);
                        Directory.CreateDirectory(Path.GetDirectoryName(absPath));

                        if (entryName[entryName.Length - 1] != Path.DirectorySeparatorChar)
                            zipEntry.ExtractToFile(absPath, true);
                    }
                }
            }
        }

        /// <summary>
        /// Recursively moves the files overwriting the existing files.
        /// </summary>
        private void MergeDirectory(DirectoryInfo srcDirInfo, DirectoryInfo destDirInfo)
        {
            // create necessary directories
            if (!destDirInfo.Exists)
                destDirInfo.Create();

            foreach (DirectoryInfo srcSubdirInfo in srcDirInfo.GetDirectories())
            {
                DirectoryInfo destSubdirInfo = new DirectoryInfo(
                    Path.Combine(destDirInfo.FullName, srcSubdirInfo.Name));

                MergeDirectory(srcSubdirInfo, destSubdirInfo);
            }

            // move files
            foreach (FileInfo srcFileInfo in srcDirInfo.GetFiles())
            {
                string destFileName = Path.Combine(destDirInfo.FullName, srcFileInfo.Name);

                if (File.Exists(destFileName))
                    File.Delete(destFileName);

                srcFileInfo.MoveTo(destFileName);
            }
        }

        /// <summary>
        /// Moves the files overwriting the existing files.
        /// </summary>
        private void MergeDirectory(string srcDirName, string destDirName)
        {
            MergeDirectory(new DirectoryInfo(srcDirName), new DirectoryInfo(destDirName));
        }

        /// <summary>
        /// Imports the configuration database table.
        /// </summary>
        private void ImportBaseTable(DataTable srcTable, IBaseTable destTable)
        {
            // add primary keys if needed
            if (!srcTable.Columns.Contains(destTable.PrimaryKey))
            {
                srcTable.Columns.Add(destTable.PrimaryKey, typeof(int));
                srcTable.BeginLoadData();
                int colInd = srcTable.Columns.Count - 1;
                int id = 1;

                foreach (DataRow row in srcTable.Rows)
                {
                    row[colInd] = id++;
                }

                srcTable.EndLoadData();
                srcTable.AcceptChanges();
            }

            // merge data
            destTable.Modified = true;
            PropertyDescriptorCollection destProps = TypeDescriptor.GetProperties(destTable.ItemType);

            foreach (DataRowView srcRowView in srcTable.DefaultView)
            {
                object destItem = TableConverter.CreateItem(destTable.ItemType, srcRowView.Row, destProps);
                destTable.AddObject(destItem);
            }
        }

        /// <summary>
        /// Adds the directory content to the archive.
        /// </summary>
        private void PackDirectory(ZipArchive zipArchive, string srcDir, string entryPrefix, bool ignoreRegKeys)
        {
            DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);
            int srcDirLen = srcDir.Length;

            foreach (FileInfo fileInfo in srcDirInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                if (!(fileInfo.Extension.Equals(".bak", StringComparison.OrdinalIgnoreCase) ||
                    ignoreRegKeys && (fileInfo.Name.EndsWith("_Reg.xml", StringComparison.OrdinalIgnoreCase) ||
                    fileInfo.Name.Equals("CompCode.txt", StringComparison.OrdinalIgnoreCase))))
                {
                    string entryName = entryPrefix + fileInfo.FullName.Substring(srcDirLen).Replace('\\', '/');
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, entryName, CompressionLevel.Fastest);
                }
            }
        }

        /// <summary>
        /// Adds the specified files to the archive.
        /// </summary>
        private void PackFiles(ZipArchive zipArchive, string srcDir, List<string> srcFileNames, string entryPrefix)
        {
            int srcDirLen = srcDir.Length;

            foreach (string srcFileName in srcFileNames)
            {
                string entryName = entryPrefix + srcFileName.Substring(srcDirLen).Replace('\\', '/');
                zipArchive.CreateEntryFromFile(srcFileName, entryName, CompressionLevel.Fastest);
            }
        }

        /// <summary>
        /// Gets a new table from the source table filtered by objects.
        /// </summary>
        private IBaseTable GetFilteredTable<T>(IBaseTable srcTable, List<int> objNums)
        {
            IBaseTable destTable = new BaseTable<T>(srcTable.Name, srcTable.PrimaryKey, srcTable.Title);

            if (srcTable.TryGetIndex("ObjNum", out TableIndex index))
            {
                foreach (int objNum in objNums)
                {
                    if (index.ItemGroups.TryGetValue(objNum, out SortedDictionary<int, object> itemGroup))
                    {
                        foreach (object item in itemGroup.Values)
                        {
                            destTable.AddObject(item);
                        }
                    }
                }
            }
            else
            {
                throw new ScadaException(AdminPhrases.IndexNotFound);
            }

            return destTable;
        }

        /// <summary>
        /// Gets the existing interface files filtered by objects.
        /// </summary>
        private List<string> GetInterfaceFiles(BaseTable<Interface> interfaceTable, 
            string interfaceDir, List<int> objNums)
        {
            if (interfaceTable.TryGetIndex("ObjNum", out TableIndex index))
            {
                List<string> fileNames = new List<string>();

                foreach (int objNum in objNums)
                {
                    if (index.ItemGroups.TryGetValue(objNum, out SortedDictionary<int, object> itemGroup))
                    {
                        foreach (Interface item in itemGroup.Values)
                        {
                            string fileName = Path.Combine(interfaceDir, item.Name /*path*/);
                            if (File.Exists(fileName))
                                fileNames.Add(fileName);
                        }
                    }
                }

                return fileNames;
            }
            else
            {
                throw new ScadaException(AdminPhrases.IndexNotFound);
            }
        }


        /// <summary>
        /// Imports the configuration from the specified archive.
        /// </summary>
        public void ImportArchive(string srcFileName, ScadaProject project, Instance instance, 
            out ConfigParts foundConfigParts)
        {
            if (srcFileName == null)
                throw new ArgumentNullException("srcFileName");
            if (project == null)
                throw new ArgumentNullException("project");
            if (instance == null)
                throw new ArgumentNullException("instance");

            foundConfigParts = ConfigParts.None;
            string extractDir = Path.Combine(Path.GetDirectoryName(srcFileName),
                Path.GetFileNameWithoutExtension(srcFileName));

            try
            {
                // extract the configuration
                ExtractArchive(srcFileName, extractDir);

                // import the configuration database
                string srcBaseDir = Path.Combine(extractDir, DirectoryBuilder.GetDirectory(ConfigParts.Base));

                if (Directory.Exists(srcBaseDir))
                {
                    foundConfigParts |= ConfigParts.Base;

                    foreach (IBaseTable destTable in project.ConfigBase.AllTables)
                    {
                        string datFileName = Path.Combine(srcBaseDir, destTable.Name.ToLowerInvariant() + ".dat");

                        if (File.Exists(datFileName))
                        {
                            try
                            {
                                BaseAdapter baseAdapter = new BaseAdapter() { FileName = datFileName };
                                DataTable srcTable = new DataTable();
                                baseAdapter.Fill(srcTable, true);
                                ImportBaseTable(srcTable, destTable);
                            }
                            catch (Exception ex)
                            {
                                throw new ScadaException(string.Format(
                                    AdminPhrases.ImportBaseTableError, destTable.Name), ex);
                            }
                        }
                    }
                }

                // import the interface files
                string srcInterfaceDir = Path.Combine(extractDir, DirectoryBuilder.GetDirectory(ConfigParts.Interface));

                if (Directory.Exists(srcInterfaceDir))
                {
                    foundConfigParts |= ConfigParts.Interface;
                    MergeDirectory(srcInterfaceDir, project.Interface.InterfaceDir);
                }

                // import the Server settings
                if (instance.ServerApp.Enabled)
                {
                    string srcServerDir = Path.Combine(extractDir, DirectoryBuilder.GetDirectory(ConfigParts.Server));

                    if (Directory.Exists(srcServerDir))
                    {
                        foundConfigParts |= ConfigParts.Server;
                        MergeDirectory(srcServerDir, instance.ServerApp.AppDir);

                        if (!instance.ServerApp.LoadSettings(out string errMsg))
                            throw new ScadaException(errMsg);
                    }
                }

                // import the Communicator settings
                if (instance.CommApp.Enabled)
                {
                    string srcCommDir = Path.Combine(extractDir, DirectoryBuilder.GetDirectory(ConfigParts.Comm));

                    if (Directory.Exists(srcCommDir))
                    {
                        foundConfigParts |= ConfigParts.Comm;
                        MergeDirectory(srcCommDir, instance.CommApp.AppDir);

                        if (!instance.CommApp.LoadSettings(out string errMsg))
                            throw new ScadaException(errMsg);
                    }
                }

                // import the Webstation settings
                if (instance.WebApp.Enabled)
                {
                    string srcWebDir = Path.Combine(extractDir, DirectoryBuilder.GetDirectory(ConfigParts.Web));

                    if (Directory.Exists(srcWebDir))
                    {
                        foundConfigParts |= ConfigParts.Web;
                        MergeDirectory(srcWebDir, instance.WebApp.AppDir);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ScadaException(AdminPhrases.ImportArchiveError, ex);
            }
            finally
            {
                // delete the extracted files
                if (Directory.Exists(extractDir))
                    Directory.Delete(extractDir, true);
            }
        }

        /// <summary>
        /// Exports the configuration to the specified archive.
        /// </summary>
        public void ExportToArchive(string destFileName, ScadaProject project, Instance instance, 
            UploadSettings uploadSettings)
        {
            if (destFileName == null)
                throw new ArgumentNullException("destFileName");
            if (project == null)
                throw new ArgumentNullException("project");
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (uploadSettings == null)
                throw new ArgumentNullException("transferSettings");

            FileStream fileStream = null;
            ZipArchive zipArchive = null;

            try
            {
                fileStream = new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create);

                List<int> objNums = uploadSettings.ObjNums;
                bool filterByObj = objNums.Count > 0;
                bool ignoreRegKeys = uploadSettings.IgnoreRegKeys;

                // add the configuration database to the archive
                if (uploadSettings.IncludeBase)
                {
                    foreach (IBaseTable srcTable in project.ConfigBase.AllTables)
                    {
                        string entryName = "BaseDAT/" + srcTable.Name.ToLowerInvariant() + ".dat";
                        ZipArchiveEntry tableEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Fastest);

                        using (Stream entryStream = tableEntry.Open())
                        {
                            // filter the source table by objects if needed
                            IBaseTable baseTable = srcTable;

                            if (filterByObj)
                            {
                                if (srcTable.ItemType == typeof(InCnl))
                                    baseTable = GetFilteredTable<InCnl>(srcTable, objNums);
                                else if (srcTable.ItemType == typeof(CtrlCnl))
                                    baseTable = GetFilteredTable<CtrlCnl>(srcTable, objNums);
                                else if (srcTable.ItemType == typeof(Interface))
                                    baseTable = GetFilteredTable<Interface>(srcTable, objNums);
                            }

                            // convert the table to DAT format
                            BaseAdapter baseAdapter = new BaseAdapter() { Stream = entryStream };
                            baseAdapter.Update(baseTable);
                        }
                    }
                }

                // add the interface files to the archive
                if (uploadSettings.IncludeInterface)
                {
                    string interfaceDir = project.Interface.InterfaceDir;
                    string entryPrefix = DirectoryBuilder.GetDirectory(ConfigParts.Interface, '/');

                    if (filterByObj)
                    {
                        PackFiles(zipArchive, interfaceDir, 
                            GetInterfaceFiles(project.ConfigBase.InterfaceTable, interfaceDir, objNums), 
                            entryPrefix);
                    }
                    else
                    {
                        PackDirectory(zipArchive, interfaceDir, entryPrefix, ignoreRegKeys);
                    }
                }

                // add the Server settings to the archive
                if (uploadSettings.IncludeServer && instance.ServerApp.Enabled)
                {
                    PackDirectory(zipArchive, instance.ServerApp.AppDir,
                        DirectoryBuilder.GetDirectory(ConfigParts.Server, '/'), ignoreRegKeys);
                }

                // add the Communicator settings to the archive
                if (uploadSettings.IncludeComm && instance.CommApp.Enabled)
                {
                    PackDirectory(zipArchive, instance.CommApp.AppDir,
                        DirectoryBuilder.GetDirectory(ConfigParts.Comm, '/'), ignoreRegKeys);
                }

                // add the Webstation settings to the archive
                if (uploadSettings.IncludeWeb && instance.WebApp.Enabled)
                {
                    PackDirectory(zipArchive, Path.Combine(instance.WebApp.AppDir, "config"),
                        DirectoryBuilder.GetDirectory(ConfigParts.Web, AppFolder.Config, '/'), ignoreRegKeys);

                    if (!uploadSettings.IgnoreWebStorage)
                    {
                        PackDirectory(zipArchive, Path.Combine(instance.WebApp.AppDir, "storage"),
                            DirectoryBuilder.GetDirectory(ConfigParts.Web, AppFolder.Storage, '/'), ignoreRegKeys);
                    }
                }

                // add an information entry to the archive
                using (Stream entryStream = 
                    zipArchive.CreateEntry(ProjectInfoEntryName, CompressionLevel.Fastest).Open())
                {
                    using (StreamWriter writer = new StreamWriter(entryStream, Encoding.UTF8))
                    {
                        writer.Write(project.GetInfo());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ScadaException(AdminPhrases.ExportToArchiveError, ex);
            }
            finally
            {
                zipArchive?.Dispose();
                fileStream?.Dispose();
            }
        }

        /// <summary>
        /// Exports the configuration database table to the file.
        /// </summary>
        public void ImportBaseTable(string srcFileName, BaseTableFormat format, IBaseTable baseTable,
            int srcStartID, int srcEndID, int destStartID, out int affectedRows)
        {
            if (srcFileName == null)
                throw new ArgumentNullException("destFileName");
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            affectedRows = 0;
            if (srcStartID > srcEndID)
                return;

            // open the source table
            IBaseTable srcTable = BaseTableFactory.GetBaseTable(baseTable);

            switch (format)
            {
                case BaseTableFormat.DAT:
                    new BaseAdapter() { FileName = srcFileName }.Fill(srcTable, true);
                    break;
                case BaseTableFormat.XML:
                    srcTable.Load(srcFileName);
                    break;
                default: // BaseTableFormat.CSV
                    throw new ScadaException("Format is not supported.");
            }

            // copy data from the source table to the destination
            int shiftID = destStartID - srcStartID;

            foreach (object item in srcTable.EnumerateItems())
            {
                int itemID = srcTable.GetPkValue(item);
                if (srcStartID <= itemID && itemID <= srcEndID)
                {
                    if (shiftID == 0)
                    {
                        baseTable.AddObject(item);
                        affectedRows++;
                    }
                    else
                    {
                        int newItemID = itemID + shiftID;
                        if (1 <= newItemID && newItemID <= AdminUtils.MaxCnlNum)
                        {
                            srcTable.SetPkValue(item, newItemID);
                            baseTable.AddObject(item);
                            affectedRows++;
                        }
                    }
                }
                else if (itemID > srcEndID)
                {
                    break;
                }
            }

            if (affectedRows > 0)
                baseTable.Modified = true;
        }

        /// <summary>
        /// Exports the configuration database table to the file.
        /// </summary>
        public void ExportBaseTable(string destFileName, BaseTableFormat format, IBaseTable baseTable, 
            int startID, int endID)
        {
            if (destFileName == null)
                throw new ArgumentNullException("destFileName");
            if (baseTable == null)
                throw new ArgumentNullException("baseTable");

            IBaseTable destTable;
            if (0 < startID || endID < int.MaxValue)
            {
                // filter data
                destTable = BaseTableFactory.GetBaseTable(baseTable);

                if (startID <= endID)
                {
                    foreach (object item in baseTable.EnumerateItems())
                    {
                        int itemID = baseTable.GetPkValue(item);
                        if (startID <= itemID && itemID <= endID)
                            destTable.AddObject(item);
                        else if (itemID > endID)
                            break;
                    }
                }
            }
            else
            {
                destTable = baseTable;
            }

            switch (format)
            {
                case BaseTableFormat.DAT:
                    new BaseAdapter() { FileName = destFileName }.Update(destTable);
                    break;
                case BaseTableFormat.XML:
                    destTable.Save(destFileName);
                    break;
                default: // BaseTableFormat.CSV
                    new CsvConverter(destFileName).ConvertToCsv(destTable);
                    break;
            }
        }
    }
}
