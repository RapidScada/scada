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
 * Summary  : Represents the Communicator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.IO;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents the Communicator application.
    /// <para>Представляет приложение Коммуникатор.</para>
    /// </summary>
    public class CommApp : ScadaApp
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommApp()
            : base()
        {
            ClearSettings();
        }


        /// <summary>
        /// Gets the settings of the application.
        /// </summary>
        public Comm.Settings Settings { get; protected set; }


        /// <summary>
        /// Gets the full file name of the application settings.
        /// </summary>
        private string GetSettingsPath()
        {
            return Path.Combine(GetConfigDir(), Comm.Settings.DefFileName);
        }

        /// <summary>
        /// Gets the directory of the application configuration.
        /// </summary>
        public string GetConfigDir()
        {
            return Path.Combine(AppDir, "Config");
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public bool LoadSettings(out string errMsg)
        {
            return Settings.Load(GetSettingsPath(), out errMsg);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public bool SaveSettings(out string errMsg)
        {
            return Settings.Save(GetSettingsPath(), out errMsg);
        }

        /// <summary>
        /// Creates project files required for the application.
        /// </summary>
        public override bool CreateAppFiles(out string errMsg)
        {
            try
            {
                Directory.CreateDirectory(GetConfigDir());
                return SaveSettings(out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.CreateCommFilesError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Delete project files of the application.
        /// </summary>
        public override bool DeleteAppFiles(out string errMsg)
        {
            try
            {
                Directory.Delete(AppDir, true);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AdminPhrases.DeleteCommFilesError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Clears the application settings.
        /// </summary>
        public override void ClearSettings()
        {
            Settings = new Comm.Settings { CreateBakFile = false };
        }

        /// <summary>
        /// Gets the directory of the application.
        /// </summary>
        public static string GetAppDir(string parentDir)
        {
            return Path.Combine(parentDir, "ScadaComm");
        }
    }
}
