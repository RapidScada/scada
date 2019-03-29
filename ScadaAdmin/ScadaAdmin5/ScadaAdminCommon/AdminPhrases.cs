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
 * Module   : ScadaAdminCommon
 * Summary  : The common phrases for Administrator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Admin
{
    /// <summary>
    /// The common phrases for Administrator.
    /// <para>Общие фразы Администратора.</para>
    /// </summary>
    public static class AdminPhrases
    {
        // Scada.Admin
        public static string IndexNotFound { get; private set; }

        // Scada.Admin.Deployment.DeploymentSettings
        public static string LoadDeploymentSettingsError { get; private set; }
        public static string SaveDeploymentSettingsError { get; private set; }

        // Scada.Admin.Project.CommApp
        public static string CreateCommFilesError { get; private set; }
        public static string DeleteCommFilesError { get; private set; }

        // Scada.Admin.Project.ConfigBase
        public static string LoadConfigBaseError { get; private set; }
        public static string SaveConfigBaseError { get; private set; }

        // Scada.Admin.Project.Instance
        public static string CreateInstanceFilesError { get; private set; }
        public static string DeleteInstanceFilesError { get; private set; }
        public static string RenameInstanceError { get; private set; }

        // Scada.Admin.Project.ScadaProject
        public static string CreateProjectError { get; private set; }
        public static string LoadProjectError { get; private set; }
        public static string SaveProjectError { get; private set; }
        public static string LoadProjectDescrError { get; private set; }
        public static string LoadBaseTableError { get; private set; }
        public static string SaveBaseTableError { get; private set; }
        public static string IncorrectProjectName{ get; private set; }
        public static string RenameProjectError { get; private set; }
        public static string ProjectDirectoryExists { get; private set; }

        // Scada.Admin.Project.ServerApp
        public static string CreateServerFilesError { get; private set; }
        public static string DeleteServerFilesError { get; private set; }

        // Scada.Admin.Project.WebApp
        public static string CreateWebFilesError { get; private set; }
        public static string DeleteWebFilesError { get; private set; }

        // Scada.Admin.ImportExport
        public static string ImportBaseTableError { get; private set; }
        public static string ImportArchiveError { get; private set; }
        public static string ExportToArchiveError { get; private set; }
        public static string ExportBaseTableError { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Admin");
            IndexNotFound = dict.GetPhrase("IndexNotFound");

            dict = Localization.GetDictionary("Scada.Admin.Deployment.DeploymentSettings");
            LoadDeploymentSettingsError = dict.GetPhrase("LoadDeploymentSettingsError");
            SaveDeploymentSettingsError = dict.GetPhrase("SaveDeploymentSettingsError");

            dict = Localization.GetDictionary("Scada.Admin.Project.CommApp");
            CreateCommFilesError = dict.GetPhrase("CreateCommFilesError");
            DeleteCommFilesError = dict.GetPhrase("DeleteCommFilesError");

            dict = Localization.GetDictionary("Scada.Admin.Project.ConfigBase");
            LoadConfigBaseError = dict.GetPhrase("LoadConfigBaseError");
            SaveConfigBaseError = dict.GetPhrase("SaveConfigBaseError");

            dict = Localization.GetDictionary("Scada.Admin.Project.Instance");
            CreateInstanceFilesError = dict.GetPhrase("CreateInstanceFilesError");
            DeleteInstanceFilesError = dict.GetPhrase("DeleteInstanceFilesError");
            RenameInstanceError = dict.GetPhrase("RenameInstanceError");

            dict = Localization.GetDictionary("Scada.Admin.Project.ScadaProject");
            CreateProjectError = dict.GetPhrase("CreateProjectError");
            LoadProjectError = dict.GetPhrase("LoadProjectError");
            SaveProjectError = dict.GetPhrase("SaveProjectError");
            LoadProjectDescrError = dict.GetPhrase("LoadProjectDescrError");
            LoadBaseTableError = dict.GetPhrase("LoadBaseTableError");
            SaveBaseTableError = dict.GetPhrase("SaveBaseTableError");
            IncorrectProjectName = dict.GetPhrase("IncorrectProjectName");
            RenameProjectError = dict.GetPhrase("RenameProjectError");
            ProjectDirectoryExists = dict.GetPhrase("ProjectDirectoryExists");

            dict = Localization.GetDictionary("Scada.Admin.Project.ServerApp");
            CreateServerFilesError = dict.GetPhrase("CreateServerFilesError");
            DeleteServerFilesError = dict.GetPhrase("DeleteServerFilesError");

            dict = Localization.GetDictionary("Scada.Admin.Project.WebApp");
            CreateWebFilesError = dict.GetPhrase("CreateWebFilesError");
            DeleteWebFilesError = dict.GetPhrase("DeleteWebFilesError");

            dict = Localization.GetDictionary("Scada.Admin.ImportExport");
            ImportBaseTableError = dict.GetPhrase("ImportBaseTableError");
            ImportArchiveError = dict.GetPhrase("ImportArchiveError");
            ExportToArchiveError = dict.GetPhrase("ExportToArchiveError");
            ExportBaseTableError = dict.GetPhrase("ExportBaseTableError");
        }
    }
}
