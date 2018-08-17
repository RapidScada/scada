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
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The phrases used in the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Admin.App.Code.ExplorerBuilder
        public static string BaseNode { get; private set; }
        public static string SysTableNode { get; private set; }
        public static string DictTableNode { get; private set; }
        public static string InterfaceNode { get; private set; }
        public static string InstancesNode { get; private set; }
        public static string ServerNode { get; private set; }
        public static string CommNode { get; private set; }
        public static string WebNode { get; private set; }

        // Scada.Admin.App.Forms.FrmBaseTable
        public static string GridViewError { get; private set; }
        public static string ColumnLabel { get; private set; }

        // Scada.Admin.App.Forms.FrmMain
        public static string ProjectFileFilter { get; private set; }

        // Scada.Admin.App.Forms.FrmNewProject
        public static string ChooseProjectLocation { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string ProjectLocationNotExists { get; private set; }
        public static string ProjectAlreadyExists { get; private set; }
        public static string ProjectTemplateEmpty { get; private set; }
        public static string ProjectTemplateNotFound { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
            BaseNode = dict.GetPhrase("BaseNode");
            SysTableNode = dict.GetPhrase("SysTableNode");
            DictTableNode = dict.GetPhrase("DictTableNode");
            InterfaceNode = dict.GetPhrase("InterfaceNode");
            InstancesNode = dict.GetPhrase("InstancesNode");
            ServerNode = dict.GetPhrase("ServerNode");
            CommNode = dict.GetPhrase("CommNode");
            WebNode = dict.GetPhrase("WebNode");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmBaseTable");
            GridViewError = dict.GetPhrase("GridViewError");
            ColumnLabel = dict.GetPhrase("ColumnLabel");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmMain");
            ProjectFileFilter = dict.GetPhrase("ProjectFileFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmNewProject");
            ChooseProjectLocation = dict.GetPhrase("ChooseProjectLocation");
            ProjectNameEmpty = dict.GetPhrase("ProjectNameEmpty");
            ProjectNameInvalid = dict.GetPhrase("ProjectNameInvalid");
            ProjectLocationNotExists = dict.GetPhrase("ProjectLocationNotExists");
            ProjectAlreadyExists = dict.GetPhrase("ProjectAlreadyExists");
            ProjectTemplateEmpty = dict.GetPhrase("ProjectTemplateEmpty");
            ProjectTemplateNotFound = dict.GetPhrase("ProjectTemplateNotFound");
        }
    }
}
