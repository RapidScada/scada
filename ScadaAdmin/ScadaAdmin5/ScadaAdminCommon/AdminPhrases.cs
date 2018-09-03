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
 * Summary  : The common phrases for Administrator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
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
        // Scada.Admin.Project.ConfigBase
        public static string LoadConfigBaseError { get; private set; }

        // Scada.Admin.Project.Instance
        public static string CreateInstanceFilesError { get; private set; }
        public static string DeleteInstanceFilesError { get; private set; }

        // Scada.Admin.Project.ScadaProject
        public static string CreateProjectError { get; private set; }
        public static string LoadProjectError { get; private set; }
        public static string SaveProjectError { get; private set; }
        public static string LoadProjectDescrError { get; private set; }
        public static string LoadBaseTableError { get; private set; }
        public static string SaveBaseTableError { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Admin.Project.ConfigBase");
            LoadConfigBaseError = dict.GetPhrase("LoadConfigBaseError");

            dict = Localization.GetDictionary("Scada.Admin.Project.Instance");
            CreateInstanceFilesError = dict.GetPhrase("CreateInstanceFilesError");
            DeleteInstanceFilesError = dict.GetPhrase("DeleteInstanceFilesError");

            dict = Localization.GetDictionary("Scada.Admin.Project.ScadaProject");
            CreateProjectError = dict.GetPhrase("CreateProjectError");
            LoadProjectError = dict.GetPhrase("LoadProjectError");
            SaveProjectError = dict.GetPhrase("SaveProjectError");
            LoadProjectDescrError = dict.GetPhrase("LoadProjectDescrError");
            LoadBaseTableError = dict.GetPhrase("LoadBaseTableError");
            SaveBaseTableError = dict.GetPhrase("SaveBaseTableError");
        }
    }
}
