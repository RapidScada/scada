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
 * Module   : ScadaAgentEngine
 * Summary  : Object for manipulating a system instance
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using Scada.Data.Configuration;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using Utils;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Object for manipulating a system instance.
    /// <para>Объект для манипуляций с экземпляром системы.</para>
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
            /// Получить или добавить новый список путей
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
        /// Макс. количество попыток проверки пользователя
        /// </summary>
        private const int MaxValidateUserAttempts = 3;
        /// <summary>
        /// The name of the archive entry that contains project information.
        /// </summary>
        private const string ProjectInfoEntryName = "Project.txt";
        /// <summary>
        /// Все части конфигурации в виде массива
        /// </summary>
        private static readonly ConfigParts[] AllConfigParts = { ConfigParts.Base,
            ConfigParts.Interface, ConfigParts.Server, ConfigParts.Comm, ConfigParts.Web };

        private ILog log; // журнал приложения
        private int validateUserAttemptNum; // номер попытки проверки пользователя


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
            validateUserAttemptNum = 0;
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
        /// Получить имя файла статуса сервиса
        /// </summary>
        private string GetServiceStatusFile(ServiceApp serviceApp)
        {
            switch (serviceApp)
            {
                case ServiceApp.Server:
                    return "ScadaServerSvc.txt";
                case ServiceApp.Comm:
                    return "ScadaCommSvc.txt";
                default:
                    throw new ArgumentException("Unknown service.");
            }
        }

        /// <summary>
        /// Получить имя файла команды сервиса
        /// </summary>
        private string GetServiceBatchFile(ServiceCommand command)
        {
            string ext = ScadaUtils.IsRunningOnWin ? ".bat" : ".sh";

            switch (command)
            {
                case ServiceCommand.Start:
                    return "svc_start" + ext;
                case ServiceCommand.Stop:
                    return "svc_stop" + ext;
                default: // ServiceCommand.Restart
                    return "svc_restart" + ext;
            }
        }

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

            if (configParts.HasFlag(ConfigParts.Comm))
                configPaths.Add(new RelPath(ConfigParts.Comm, AppFolder.Config));

            if (configParts.HasFlag(ConfigParts.Web))
            {
                configPaths.Add(new RelPath(ConfigParts.Web, AppFolder.Config));
                configPaths.Add(new RelPath(ConfigParts.Web, AppFolder.Storage));
            }

            return configPaths;
        }

        /// <summary>
        /// Подготовить игнорируемые пути: разделить по группам, применить поиск файлов по маске
        /// </summary>
        private PathDict PrepareIgnoredPaths(ICollection<RelPath> relPaths)
        {
            PathDict pathDict = new PathDict();

            if (relPaths != null)
            {
                foreach (RelPath relPath in relPaths)
                {
                    PathList pathList = pathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder);
                    string[] absPathArr;

                    if (relPath.IsMask)
                    {
                        string dir = GetAbsPath(relPath.ConfigPart, relPath.AppFolder, "");
                        absPathArr = Directory.Exists(dir) ? 
                            Directory.GetFiles(dir, relPath.Path) : new string[0];
                    }
                    else
                    {
                        absPathArr = new string[] { GetAbsPath(relPath) };
                    }

                    foreach (string absPath in absPathArr)
                    {
                        char lastSym = absPath[absPath.Length - 1];

                        if (lastSym == Path.DirectorySeparatorChar || lastSym == Path.AltDirectorySeparatorChar)
                            pathList.Dirs.Add(absPath);
                        else
                            pathList.Files.Add(absPath);
                    }
                }
            }

            return pathDict;
        }

        /// <summary>
        /// Упаковать директорию
        /// </summary>
        private void PackDir(ZipArchive zipArchive, string srcDir, string entryPrefix, PathList ignoredPaths)
        {
            srcDir = ScadaUtils.NormalDir(srcDir);

            if (!ignoredPaths.Dirs.Contains(srcDir) && Directory.Exists(srcDir))
            {
                DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);

                // упаковка поддиректорий
                DirectoryInfo[] dirInfoArr = srcDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo dirInfo in dirInfoArr)
                {
                    PackDir(zipArchive, dirInfo.FullName, entryPrefix + dirInfo.Name + "/", ignoredPaths);
                }

                // упаковка файлов
                FileInfo[] fileInfoArr = srcDirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                int srcDirLen = srcDir.Length;

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (!ignoredPaths.Files.Contains(fileInfo.FullName) &&
                        !fileInfo.Extension.Equals(".bak", StringComparison.OrdinalIgnoreCase))
                    {
                        string entryName = entryPrefix + fileInfo.FullName.Substring(srcDirLen).Replace('\\', '/');
                        zipArchive.CreateEntryFromFile(fileInfo.FullName, entryName, CompressionLevel.Fastest);
                    }
                }
            }
        }

        /// <summary>
        /// Упаковать директорию
        /// </summary>
        private void PackDir(ZipArchive zipArchive, RelPath relPath, PathDict ignoredPathDict)
        {
            PackDir(zipArchive, 
                GetAbsPath(relPath),
                DirectoryBuilder.GetDirectory(relPath.ConfigPart, relPath.AppFolder, '/'), 
                ignoredPathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder));
        }

        /// <summary>
        /// Очистить директорию
        /// </summary>
        private void ClearDir(DirectoryInfo dirInfo, PathList ignoredPaths, out bool dirEmpty)
        {
            if (ignoredPaths.Dirs.Contains(dirInfo.FullName))
            {
                dirEmpty = false;
            }
            else
            {
                // очистка поддиректорий
                DirectoryInfo[] subdirInfoArr = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo subdirInfo in subdirInfoArr)
                {
                    ClearDir(subdirInfo, ignoredPaths, out bool subdirEmpty);
                    if (subdirEmpty)
                        subdirInfo.Delete();
                }

                // удаление файлов
                FileInfo[] fileInfoArr = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                dirEmpty = true;

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    if (ignoredPaths.Files.Contains(fileInfo.FullName))
                        dirEmpty = false;
                    else
                        fileInfo.Delete();
                }
            }
        }

        /// <summary>
        /// Очистить директорию
        /// </summary>
        private void ClearDir(RelPath relPath, PathDict ignoredPathDict)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(GetAbsPath(relPath));

            if (dirInfo.Exists)
            {
                ClearDir(dirInfo, ignoredPathDict.GetOrAdd(relPath.ConfigPart, relPath.AppFolder),
                    out bool dirEmpty);
            }
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
        /// Проверить пользователя
        /// </summary>
        /// <remarks>Проверяется имя пользователя, пароль и роль</remarks>
        public bool ValidateUser(string username, string password, out string errMsg)
        {
            try
            {
                // проверка количества попыток
                if (validateUserAttemptNum > MaxValidateUserAttempts)
                {
                    errMsg = Localization.UseRussian ?
                        "Превышено количество попыток входа" :
                        "Number of login attempts exceeded";
                    return false;
                }
                else
                {
                    validateUserAttemptNum++;
                }

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    // открытие таблицы пользователей
                    BaseAdapter baseAdapter = new BaseAdapter();
                    DataTable userTable = new DataTable();
                    baseAdapter.FileName = Path.Combine(Settings.Directory,
                        DirectoryBuilder.GetDirectory(ConfigParts.Base), "user.dat");
                    baseAdapter.Fill(userTable, false);

                    // поиск и проверка информации о пользователе
                    userTable.CaseSensitive = false;
                    DataRow[] rows = userTable.Select(string.Format("Name = '{0}'", username));

                    if (rows.Length > 0)
                    {
                        DataRow row = rows[0];
                        if ((string)row["Password"] == password)
                        {
                            if ((int)row["RoleID"] == BaseValues.Roles.App)
                            {
                                validateUserAttemptNum = 0;
                                errMsg = "";
                                return true;
                            }
                            else
                            {
                                errMsg = Localization.UseRussian ?
                                    "Недостаточно прав" :
                                    "Insufficient rights";
                                return false;
                            }
                        }
                    }
                }

                errMsg = Localization.UseRussian ?
                    "Неверное имя пользователя или пароль" :
                    "Invalid username or password";
                return false;
            }
            catch (Exception ex)
            {
                errMsg = Localization.UseRussian ?
                   "Ошибка при проверке пользователя" :
                   "Error validating user";
                log.WriteException(ex, errMsg);
                return false;
            }
        }

        /// <summary>
        /// Управлять службой
        /// </summary>
        public bool ControlService(ServiceApp serviceApp, ServiceCommand command)
        {
            try
            {
                string batchFileName = Path.Combine(Settings.Directory,
                    DirectoryBuilder.GetDirectory(serviceApp), GetServiceBatchFile(command));

                if (File.Exists(batchFileName))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = batchFileName,
                        UseShellExecute = false
                    });
                    return true;
                }
                else
                {
                    log.WriteError(string.Format(Localization.UseRussian ?
                        "Не найден файл для управления службой {0}" :
                        "File {0} for service control not found", batchFileName));
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                   "Ошибка при управлении службой" :
                   "Error controlling service");
                return false;
            }
        }

        /// <summary>
        /// Получить статус службы
        /// </summary>
        public bool GetServiceStatus(ServiceApp serviceApp, out ServiceStatus status)
        {
            try
            {
                status = ServiceStatus.Undefined;
                string statusFileName = Path.Combine(Settings.Directory, 
                    DirectoryBuilder.GetDirectory(serviceApp), 
                    DirectoryBuilder.GetDirectory(AppFolder.Log),
                    GetServiceStatusFile(serviceApp));

                if (File.Exists(statusFileName))
                {
                    string[] lines = File.ReadAllLines(statusFileName, Encoding.UTF8);

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("State", StringComparison.Ordinal) ||
                            line.StartsWith("Состояние", StringComparison.Ordinal))
                        {
                            int colonInd = line.IndexOf(':');

                            if (colonInd > 0)
                            {
                                string statusStr = line.Substring(colonInd + 1).Trim();

                                if (statusStr.Equals("normal", StringComparison.OrdinalIgnoreCase) ||
                                    statusStr.Equals("норма", StringComparison.OrdinalIgnoreCase))
                                {
                                    status = ServiceStatus.Normal;
                                }
                                else if (statusStr.Equals("stopped", StringComparison.OrdinalIgnoreCase) ||
                                    statusStr.Equals("остановлен", StringComparison.OrdinalIgnoreCase))
                                {
                                    status = ServiceStatus.Stopped;
                                }
                                else if (statusStr.Equals("error", StringComparison.OrdinalIgnoreCase) ||
                                    statusStr.Equals("ошибка", StringComparison.OrdinalIgnoreCase))
                                {
                                    status = ServiceStatus.Error;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                   "Ошибка при получении статуса службы" :
                   "Error getting service status");
                status = ServiceStatus.Undefined;
                return false;
            }
        }

        /// <summary>
        /// Получить абсолютный путь из относительного
        /// </summary>
        public string GetAbsPath(RelPath relPath)
        {
            return GetAbsPath(relPath.ConfigPart, relPath.AppFolder, relPath.Path);
        }

        /// <summary>
        /// Получить абсолютный путь из относительного
        /// </summary>
        public string GetAbsPath(ConfigParts configPart, AppFolder appFolder, string path)
        {
            return Path.Combine(Settings.Directory, 
                DirectoryBuilder.GetDirectory(configPart, appFolder, Path.DirectorySeparatorChar), path);
        }

        /// <summary>
        /// Получить доступные части конфигурации
        /// </summary>
        public bool GetAvailableConfig(out ConfigParts configParts)
        {
            try
            {
                configParts = ConfigParts.None;

                foreach (ConfigParts configPart in AllConfigParts)
                {
                    if (Directory.Exists(Path.Combine(Settings.Directory, DirectoryBuilder.GetDirectory(configPart))))
                        configParts |= configPart;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении доступных частей конфигурации" :
                    "Error getting available parts of the configuration");
                configParts = ConfigParts.None;
                return false;
            }
        }

        /// <summary>
        /// Упаковать конфигурацию в архив
        /// </summary>
        public bool PackConfig(string destFileName, ConfigOptions configOptions)
        {
            try
            {
                List<RelPath> configPaths = GetConfigPaths(configOptions.ConfigParts);
                PathDict ignoredPathDict = PrepareIgnoredPaths(configOptions.IgnoredPaths);

                using (FileStream fileStream = 
                    new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                    {
                        foreach (RelPath relPath in configPaths)
                        {
                            PackDir(zipArchive, relPath, ignoredPathDict);
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
                // delete the existing configuration
                List<RelPath> configPaths = GetConfigPaths(configOptions.ConfigParts);
                PathDict pathDict = PrepareIgnoredPaths(configOptions.IgnoredPaths);

                foreach (RelPath relPath in configPaths)
                {
                    ClearDir(relPath, pathDict);
                }

                // delete a project information file
                string instanceDir = Settings.Directory;
                string projectInfoFileName = Path.Combine(instanceDir, ProjectInfoEntryName);
                File.Delete(projectInfoFileName);

                // define allowed directories to unpack
                ConfigParts configParts = configOptions.ConfigParts;
                List<string> allowedEntries = new List<string> { ProjectInfoEntryName };

                foreach (ConfigParts configPart in AllConfigParts)
                {
                    if (configParts.HasFlag(configPart))
                        allowedEntries.Add(DirectoryBuilder.GetDirectory(configPart, '/'));
                }

                // unpack the new configuration
                using (FileStream fileStream =
                    new FileStream(srcFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                    {
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

        /// <summary>
        /// Обзор директории
        /// </summary>
        public bool Browse(RelPath relPath, out ICollection<string> directories, out ICollection<string> files)
        {
            try
            {
                string absPath = GetAbsPath(relPath);
                DirectoryInfo dirInfo = new DirectoryInfo(absPath);

                // получение поддиректорий
                directories = new List<string>();
                DirectoryInfo[] subdirInfoArr = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (DirectoryInfo subdirInfo in subdirInfoArr)
                {
                    directories.Add(subdirInfo.Name);
                }

                // получение файлов
                files = new List<string>();
                FileInfo[] fileInfoArr = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoArr)
                {
                    files.Add(fileInfo.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                   "Ошибка при обзоре директории" :
                   "Error browsing directory");
                directories = null;
                files = null;
                return false;
            }
        }
    }
}
