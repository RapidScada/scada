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
 * Module   : Administrator
 * Summary  : Common data of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Config;
using Scada.UI;
using System;
using System.IO;
using System.Text;
using Utils;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Common data of the application.
    /// <para>Общие данные приложения.</para>
    /// </summary>
    public sealed class AppData
    {
        /// <summary>
        /// Short name of the application error log file.
        /// </summary>
        private const string ErrFileName = "ScadaAdmin.err";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppData()
        {
            AppDirs = new AdminDirs();
            ErrLog = new Log(Log.Formats.Full);
            AppSettings = new AdminSettings();
            AppState = new AppState();
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AdminDirs AppDirs { get; }

        /// <summary>
        /// Gets the application error log.
        /// </summary>
        public Log ErrLog { get; }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public AdminSettings AppSettings { get; }

        /// <summary>
        /// Gets the state of application controls.
        /// </summary>
        public AppState AppState { get; }


        /// <summary>
        /// Clears the temporary directory.
        /// </summary>
        private void ClearTempDir()
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(AppDirs.TempDir);

                if (directoryInfo.Exists)
                {
                    foreach (DirectoryInfo subdirInfo in directoryInfo.GetDirectories())
                    {
                        subdirInfo.Delete(true);
                    }

                    foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrLog.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при очистке директории временных файлов" :
                    "Error cleaning the directory of temporary files");
            }
        }

        /// <summary>
        /// Initializes the common data.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);

            ErrLog.FileName = AppDirs.LogDir + ErrFileName;
            ErrLog.Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Makes finalization steps.
        /// </summary>
        public void FinalizeApp()
        {
            ClearTempDir();
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public void ProcError(string message)
        {
            ErrLog.WriteError(message);
            ScadaUiUtils.ShowError(message);
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public void ProcError(Exception ex, string message = null)
        {
            ErrLog.WriteException(ex, message);
            ScadaUiUtils.ShowError(string.IsNullOrEmpty(message) ? 
                ex.Message : 
                message + ":" + Environment.NewLine + ex.Message);
        }
    }
}
