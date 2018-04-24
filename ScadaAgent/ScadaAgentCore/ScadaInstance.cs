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
 * Module   : ScadaAgentCore
 * Summary  : Object for manipulating a system instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Utils;

namespace Scada.Agent
{
    /// <summary>
    /// Object for manipulating a system instance
    /// <para>Объект для манипуляций с экземпляром системы</para>
    /// </summary>
    public class ScadaInstance
    {
        private ILog log; // журнал приложения


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private ScadaInstance()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ScadaInstance(ScadaInstanceSettings settings, object syncRoot, ILog log)
        {
            Settings = settings ?? throw new ArgumentNullException("settings");
            SyncRoot = syncRoot ?? throw new ArgumentNullException("syncRoot");
            this.log = log ?? throw new ArgumentNullException("log");
            Name = settings.Name;
        }


        /// <summary>
        /// Получить наименование
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Получить настройки экземпляра системы
        /// </summary>
        public ScadaInstanceSettings Settings { get; private set; }

        /// <summary>
        /// Получить или установить объект для синхронизации доступа к экземпляру системы
        /// </summary>
        public object SyncRoot { get; private set; }


        /// <summary>
        /// ???
        /// </summary>
        private void MakeAbsolutePath(ICollection<RelPath> relPaths, out List<string> dirs, out List<string> fileNames)
        {
            dirs = null;
            fileNames = null;
        }

        /// <summary>
        /// Упаковать директорию
        /// </summary>
        private void PackDir(ZipArchive zipArchive, string srcDir, bool recursively, string entryPrefix)
        {
            srcDir = ScadaUtils.NormalDir(srcDir);
            int srcDirLen = srcDir.Length;
            DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);
            FileInfo[] fileInfoArr = srcDirInfo.GetFiles("*.*", 
                recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach (FileInfo fileInfo in fileInfoArr)
            {
                if (!fileInfo.Extension.Equals(".bak", StringComparison.OrdinalIgnoreCase))
                {
                    string entryName = fileInfo.FullName.Substring(srcDirLen).Replace('\\', '/');
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, entryPrefix + entryName,
                        CompressionLevel.Fastest);
                }
            }
        }


        /// <summary>
        /// Проверить пароль и права пользователя
        /// </summary>
        public bool ValidateUser(string username, string encryptedPassword, out string errMsg)
        {
            // TODO: реализовать и ограничить кол-во попыток
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Упаковать конфигурацию в архив
        /// </summary>
        public bool PackConfig(string destFileName, ConfigOptions configOptions)
        {
            try
            {
                using (FileStream fileStream = 
                    new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create);
                    ConfigParts configParts = configOptions.ConfigParts;

                    if (configParts.HasFlag(ConfigParts.Base))
                        PackDir(zipArchive, Settings.Directory + "BaseDAT", false, "BaseDAT/");

                    if (configParts.HasFlag(ConfigParts.Interface))
                        PackDir(zipArchive, Settings.Directory + "Interface", true, "Interface/");

                    if (configParts.HasFlag(ConfigParts.Server))
                        PackDir(zipArchive, Path.Combine(Settings.Directory, "ScadaServer", "Config"), true,
                            "ScadaServer/Config/");

                    if (configParts.HasFlag(ConfigParts.Communicator))
                        PackDir(zipArchive, Path.Combine(Settings.Directory, "ScadaComm", "Config"), true,
                            "ScadaComm/Config/");

                    if (configParts.HasFlag(ConfigParts.Webstation))
                    {
                        PackDir(zipArchive, Path.Combine(Settings.Directory, "ScadaWeb", "config"), true, 
                            "ScadaWeb/config/");
                        PackDir(zipArchive, Path.Combine(Settings.Directory, "ScadaWeb", "storage"), true,
                            "ScadaWeb/storage/");
                    }

                    return true;
                }

            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ? 
                    "Ошибка при упаковке конфигурации в архив" :
                    "Error packing configuration into archive");
                return false;
            }
        }

        /// <summary>
        /// Распаковать архив конфигурации
        /// </summary>
        public bool UnpackConfig(string srcFileName, ConfigOptions configOptions)
        {
            return false;
        }
    }
}
