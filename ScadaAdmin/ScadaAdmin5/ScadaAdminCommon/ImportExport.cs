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
 * Module   : ScadaAdminCommon
 * Summary  : Import and export configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;
using Scada.Agent;
using Scada.Data.Tables;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;

namespace Scada.Admin
{
    /// <summary>
    /// Import and export configuration.
    /// <para>Импорт и экспорт конфигурации.</para>
    /// </summary>
    public class ImportExport
    {
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
                        string destFileName = Path.Combine(destDir, zipEntry.FullName);
                        Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                        zipEntry.ExtractToFile(destFileName, true);
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
            if (srcTable.Columns.Contains(destTable.PrimaryKey))
            {
                PropertyDescriptorCollection destProps = TypeDescriptor.GetProperties(destTable.ItemType);

                foreach (DataRowView srcRowView in srcTable.DefaultView)
                {
                    object destItem = TableConverter.CreateItem(destTable.ItemType, srcRowView.Row, destProps);
                    destTable.AddObject(destItem);
                }
            }
        }


        /// <summary>
        /// Imports the instance configuration from the specified archive.
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
                // TODO: uncomment
                // delete the extracted files
                //if (Directory.Exists(extractDir))
                //    Directory.Delete(extractDir, true);
            }
        }
    }
}
