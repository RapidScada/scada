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
        /// <summary>
        /// Список путей
        /// </summary>
        private class PathList
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PathList()
            {
                Dirs = new List<string>();
                Files = new List<string>();
            }

            /// <summary>
            /// Получить абсолютные пути директорий
            /// </summary>
            public List<string> Dirs { get; private set; }
            /// <summary>
            /// Получить абсолютные пути файлов
            /// </summary>
            public List<string> Files { get; private set; }
        }

        /// <summary>
        /// Справочник путей, сгруппированных по частям конфигурации и папкам приложения
        /// </summary>
        private class PathDict : Dictionary<ConfigParts, Dictionary<AppFolder, PathList>>
        {
            /// <summary>
            /// Получить или создать список путей
            /// </summary>
            public PathList GetOrAdd(ConfigParts configPart, AppFolder appFolder)
            {
                Dictionary<AppFolder, PathList> subDict;
                PathList pathList;

                if (TryGetValue(configPart, out subDict))
                {
                    if (subDict.TryGetValue(appFolder, out pathList))
                        return pathList;
                }
                else
                {
                    subDict = new Dictionary<AppFolder, PathList>();
                    this[configPart] = subDict;
                }

                pathList = new PathList();
                subDict[appFolder] = pathList;
                return pathList;
            }
        }

        /// <summary>
        /// Все части конфигурации в виде массива
        /// </summary>
        private static ConfigParts[] AllConfigParts = { ConfigParts.Base, ConfigParts.Interface,
            ConfigParts.Server, ConfigParts.Communicator, ConfigParts.Webstation };

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
        /// Получить относительные пути конфигурации, соответствующие заданным частям
        /// </summary>
        private List<RelPath> GetConfigPaths(ConfigParts configParts)
        {
            List<RelPath> configPaths = new List<RelPath>();

            if (configParts.HasFlag(ConfigParts.Base))
                configPaths.Add(new RelPath(ConfigParts.Base, AppFolder.Root));

            if (configParts.HasFlag(ConfigParts.Interface))
                configPaths.Add(new RelPath(ConfigParts.Interface, AppFolder.Root));

            if (configParts.HasFlag(ConfigParts.Server))
                configPaths.Add(new RelPath(ConfigParts.Server, AppFolder.Config));

            if (configParts.HasFlag(ConfigParts.Communicator))
                configPaths.Add(new RelPath(ConfigParts.Communicator, AppFolder.Config));

            if (configParts.HasFlag(ConfigParts.Webstation))
            {
                configPaths.Add(new RelPath(ConfigParts.Webstation, AppFolder.Config));
                configPaths.Add(new RelPath(ConfigParts.Webstation, AppFolder.Storage));
            }

            return configPaths;
        }

        /// <summary>
        /// Получить директорию части конфигурации
        /// </summary>
        private string GetConfigPartDir(ConfigParts configPart, char? directorySeparator)
        {
            switch (configPart)
            {
                case ConfigParts.Base:
                    return "BaseDAT" + directorySeparator;
                case ConfigParts.Interface:
                    return "Interface" + directorySeparator;
                case ConfigParts.Server:
                    return "ScadaServer" + directorySeparator;
                case ConfigParts.Communicator:
                    return "ScadaComm" + directorySeparator;
                case ConfigParts.Webstation:
                    return "ScadaWeb" + directorySeparator;
                default:
                    throw new ArgumentException("Incorrect configuration part.");
            }
        }

        /// <summary>
        /// Получить директорию папки приложения
        /// </summary>
        private string GetAppFolderDir(AppFolder appFolder, char? directorySeparator, bool lowerCase = false)
        {
            string dir;

            switch (appFolder)
            {
                case AppFolder.Config:
                    dir = "Config" + directorySeparator;
                    break;
                case AppFolder.Log:
                    dir = "Log" + directorySeparator;
                    break;
                case AppFolder.Storage:
                    dir = "Storage" + directorySeparator;
                    break;
                default:
                    dir = "";
                    break;
            }

            return lowerCase ? dir.ToLowerInvariant() : dir;
        }

        /// <summary>
        /// Получить директорию папки приложения
        /// </summary>
        private string GetAppFolderDir(ConfigParts configPart, AppFolder appFolder, char directorySeparator)
        {
            return GetConfigPartDir(configPart, directorySeparator) +
                GetAppFolderDir(appFolder, directorySeparator, configPart == ConfigParts.Webstation);
        }

        /// <summary>
        /// Получить абсолютный путь из относительного
        /// </summary>
        private string GetAbsPath(RelPath relPath)
        {
            return Path.Combine(Settings.Directory, 
                GetConfigPartDir(relPath.ConfigPart, null), 
                GetAppFolderDir(relPath.AppFolder, null, relPath.ConfigPart == ConfigParts.Webstation), 
                relPath.Path);
        }

        /// <summary>
        /// Разделить исключаемые пути по группам
        /// </summary>
        private PathDict SeparatePaths(ICollection<RelPath> relPaths)
        {
            PathDict pathDict = new PathDict();

            foreach (RelPath relPath in relPaths)
            {
                PathList pathList = pathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder);
                string absPath = GetAbsPath(relPath);
                char lastPathSym = absPath[absPath.Length - 1];

                if (lastPathSym == Path.DirectorySeparatorChar || lastPathSym == Path.AltDirectorySeparatorChar)
                    pathList.Dirs.Add(absPath);
                else
                    pathList.Files.Add(absPath);
            }

            return pathDict;
        }

        /// <summary>
        /// Упаковать директорию
        /// </summary>
        private void PackDir(ZipArchive zipArchive, string srcDir, string entryPrefix, PathList excludedPaths)
        {
            srcDir = ScadaUtils.NormalDir(srcDir);

            if (!excludedPaths.Dirs.Contains(srcDir))
            {
                DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);

                // упаковка поддиректорий
                DirectoryInfo[] dirInfoArr = srcDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo dirInfo in dirInfoArr)
                {
                    PackDir(zipArchive, dirInfo.FullName, entryPrefix + dirInfo.Name + "/", excludedPaths);
                }

                // упаковка файлов
                FileInfo[] fileInfoArr = srcDirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                int srcDirLen = srcDir.Length;

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (!excludedPaths.Files.Contains(fileInfo.FullName) &&
                        !fileInfo.Extension.Equals(".bak", StringComparison.OrdinalIgnoreCase))
                    {
                        string entryName = fileInfo.FullName.Substring(srcDirLen).Replace('\\', '/');
                        zipArchive.CreateEntryFromFile(fileInfo.FullName, entryPrefix + entryName,
                            CompressionLevel.Fastest);
                    }
                }
            }
        }

        /// <summary>
        /// Упаковать директорию
        /// </summary>
        private void PackDir(ZipArchive zipArchive, RelPath relPath, PathDict excludedPathDict)
        {
            PackDir(zipArchive, 
                GetAbsPath(relPath),
                GetAppFolderDir(relPath.ConfigPart, relPath.AppFolder, '/'), 
                excludedPathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder));
        }

        /// <summary>
        /// Очистить директорию
        /// </summary>
        private void ClearDir(string dir, PathList excludedPaths, out bool dirEmpty)
        {
            if (excludedPaths.Dirs.Contains(dir))
            {
                dirEmpty = false;
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);

                // очистка поддиректорий
                DirectoryInfo[] subdirInfoArr = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo subdirInfo in subdirInfoArr)
                {
                    ClearDir(subdirInfo.FullName, excludedPaths, out bool subdirEmpty);
                    if (subdirEmpty)
                        subdirInfo.Delete();
                }

                // удаление файлов
                FileInfo[] fileInfoArr = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                dirEmpty = true;

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (excludedPaths.Files.Contains(fileInfo.FullName))
                        dirEmpty = false;
                    else
                        fileInfo.Delete();
                }
            }
        }

        /// <summary>
        /// Очистить директорию
        /// </summary>
        private void ClearDir(RelPath relPath, PathDict excludedPathDict)
        {
            ClearDir(GetAbsPath(relPath), excludedPathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder), 
                out bool dirEmpty);
        }

        /// <summary>
        /// Проверить, что строка начинается хотя бы с одного из заданных значений
        /// </summary>
        private bool StartsWith(string s, ICollection<string> values, StringComparison comparisonType)
        {
            foreach (string val in values)
            {
                if (s.StartsWith(val, comparisonType))
                    return true;
            }

            return false;
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
                List<RelPath> configPaths = GetConfigPaths(configOptions.ConfigParts);
                PathDict pathDict = SeparatePaths(configOptions.ExcludedPaths);

                using (FileStream fileStream = 
                    new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                    {
                        foreach (RelPath relPath in configPaths)
                        {
                            PackDir(zipArchive, relPath, pathDict);
                        }

                        return true;
                    }
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
            try
            {
                // удаление существующей конфигурации
                List<RelPath> configPaths = GetConfigPaths(configOptions.ConfigParts);
                PathDict pathDict = SeparatePaths(configOptions.ExcludedPaths);

                foreach (RelPath relPath in configPaths)
                {
                    ClearDir(relPath, pathDict);
                }

                // определение допустимых директорий для распаковки
                ConfigParts configParts = configOptions.ConfigParts;
                List<string> allowedEntries = new List<string>(AllConfigParts.Length);

                foreach (ConfigParts configPart in AllConfigParts)
                {
                    if (configParts.HasFlag(configPart))
                        allowedEntries.Add(GetConfigPartDir(configPart, '/'));
                }

                // распаковка новой конфигурации
                using (FileStream fileStream =
                    new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                    {
                        string instanceDir = Settings.Directory;

                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            if (StartsWith(entry.FullName, allowedEntries, StringComparison.Ordinal))
                            {
                                string relPath = entry.FullName.Replace('/', Path.DirectorySeparatorChar);
                                string destFileName = instanceDir + relPath;
                                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                                entry.ExtractToFile(destFileName, true);
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при распаковке конфигурации из архива" :
                    "Error unpacking configuration from archive");
                return false;
            }
        }
    }
}
