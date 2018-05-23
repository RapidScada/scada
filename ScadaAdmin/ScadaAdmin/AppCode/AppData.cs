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
 * Module   : SCADA-Administrator
 * Summary  : The common application data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Scada;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// The common application data
    /// <para>Общие данные приложения</para>
    /// </summary>
    internal static class AppData
    {
        /// <summary>
        /// Имя файла журнала ошибок приложения
        /// </summary>
        private const string ErrFileName = "ScadaAdmin.err";


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static AppData()
        {
            AppDirs = new AppDirs();
            AppDirs.Init(Path.GetDirectoryName(Application.ExecutablePath));

            ErrLog = new Log(Log.Formats.Full)
            {
                FileName = AppDirs.LogDir + ErrFileName,
                Encoding = Encoding.UTF8
            };

            Settings = new Settings();
            Conn =  new SqlCeConnection();
        }


        /// <summary>
        /// Получить директории приложения
        /// </summary>
        public static AppDirs AppDirs { get; private set; }

        /// <summary>
        /// Получить журнал ошибок приложения
        /// </summary>
        public static Log ErrLog { get; private set; }


        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public static Settings Settings { get; private set; }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        public static SqlCeConnection Conn { get; private set; }

        /// <summary>
        /// Получить признак, установлено ли соединение с БД
        /// </summary>
        public static bool Connected
        {
            get
            {
                return Conn.State == ConnectionState.Open;
            }
        }


        /// <summary>
        /// Соединиться с БД, используя заданную в файле Web.Config строку связи
        /// </summary>
        public static void Connect()
        {
            if (Conn.State != ConnectionState.Closed)
                Disconnect();

            string baseSdfFileName = Settings.AppSett.BaseSDFFile;

            if (!File.Exists(baseSdfFileName))
                throw new FileNotFoundException(string.Format(AppPhrases.BaseSDFFileNotFound, baseSdfFileName));

            try
            {
                string connStr = "Data Source=" + baseSdfFileName;
                if (Conn.ConnectionString != connStr)
                    Conn.ConnectionString = connStr;
                Conn.Open();
            }
            catch
            {
                Conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Разъединиться с БД
        /// </summary>
        public static void Disconnect()
        {
            Conn.Close();
        }

        /// <summary>
        /// Упаковать БД
        /// </summary>
        public static bool Compact()
        {
            if (string.IsNullOrEmpty(Conn.ConnectionString))
            {
                return false;
            }
            else
            {
                bool wasConnected = Connected;

                try
                {
                    if (wasConnected)
                        Conn.Close();

                    SqlCeEngine engine = new SqlCeEngine(Conn.ConnectionString);
                    engine.Compact(string.Empty);
                }
                finally
                {
                    if (wasConnected)
                        Conn.Open();
                }

                return true;
            }
        }
    }
}
