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
 * Module   : ScadaWebCommon
 * Summary  : Web application file storage
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.IO;

namespace Scada.Web
{
    /// <summary>
    /// Web application file storage
    /// <para>Файловое хранилище веб-приложения</para>
    /// </summary>
    public class Storage
    {
        /// <summary>
        /// Имя, обозначающее всех пользователей
        /// </summary>
        public const string AllUsersName = "AllUsers";

        /// <summary>
        /// Поддиректория приложения
        /// </summary>
        public const string AppSubdir = "App";


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected Storage()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Storage(string storageDir)
        {
            StorageDir = storageDir;
        }


        /// <summary>
        /// Получить или установить директорию хранилища приложения
        /// </summary>
        public string StorageDir { get; set; }


        /// <summary>
        /// Получить директорию хранилища всех пользователей
        /// </summary>
        public string GetAllUsersDir()
        {
            return GetUserDir(AllUsersName);
        }

        /// <summary>
        /// Получить директорию хранилища заданного пользователя
        /// </summary>
        public string GetUserDir(string username)
        {
            UserData.ValidateUserName(username);
            return StorageDir + username.ToLowerInvariant() + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Получить директорию хранилища приложения для всех пользователей
        /// </summary>
        public string GetAllUsersAppDir()
        {
            return GetUserPluginDir(AllUsersName, AppSubdir);
        }

        /// <summary>
        /// Получить директорию хранилища приложения для заданного пользователя
        /// </summary>
        public string GetUserAppDir(string username)
        {
            return GetUserPluginDir(username, AppSubdir);
        }

        /// <summary>
        /// Получить директорию хранилища плагина для всех пользователей
        /// </summary>
        public string GetAllUsersPluginDir(string pluginSubdir)
        {
            return GetUserPluginDir(AllUsersName, pluginSubdir);
        }

        /// <summary>
        /// Получить директорию хранилища плагина для заданного пользователя
        /// </summary>
        public string GetUserPluginDir(string username, string pluginSubdir)
        {
            return GetUserDir(username) + pluginSubdir + Path.DirectorySeparatorChar;
        }


        /// <summary>
        /// Создать директорию, если она не существует, и вернуть её путь
        /// </summary>
        public static string ForceDir(string path)
        {
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
