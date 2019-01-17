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
        public static string EmptyNode { get; private set; }

        // Scada.Admin.App.Controls.Deployment.CtrlProfileSelector
        public static string ProfileNotSet { get; private set; }
        public static string ConfirmDeleteProfile { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmConnSettings
        public static string EmptyFieldsNotAllowed { get; private set; }
        public static string ProfileNameDuplicated { get; private set; }
        public static string IncorrectSecretKey { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmDownloadConfig
        public static string NothingToDownload { get; private set; }
        public static string DownloadConfigComplete { get; private set; }
        public static string DownloadConfigError { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmInstanceProfile
        public static string ConnectionOK { get; private set; }
        public static string TestConnectionError { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmInstanceStatus
        public static string NormalSvcStatus { get; private set; }
        public static string StoppedSvcStatus { get; private set; }
        public static string ErrorSvcStatus { get; private set; }
        public static string UndefinedSvcStatus { get; private set; }
        public static string ServiceRestarted { get; private set; }
        public static string UnableRestartService { get; private set; }
        public static string ServiceRestartError { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmUploadConfig
        public static string NothingToUpload { get; private set; }
        public static string UploadConfigComplete { get; private set; }
        public static string UploadConfigError { get; private set; }

        // Scada.Admin.App.Forms.FrmBaseTable
        public static string GridViewError { get; private set; }
        public static string ColumnLabel { get; private set; }

        // Scada.Admin.App.Forms.FrmFileNew
        public static string FileNameEmpty { get; private set; }
        public static string FileNameInvalid { get; private set; }

        // Scada.Admin.App.Forms.FrmInstanceEdit
        public static string NewInstanceTitle { get; private set; }
        public static string EditInstanceTitle { get; private set; }
        public static string InstanceNameEmpty { get; private set; }
        public static string InstanceNameInvalid { get; private set; }
        public static string InstanceSelectApps { get; private set; }

        // Scada.Admin.App.Forms.FrmItemName
        public static string ItemNameEmpty { get; private set; }
        public static string ItemNameInvalid { get; private set; }
        public static string ItemNameDuplicated { get; private set; }

        // Scada.Admin.App.Forms.FrmMain
        public static string EmptyTitle { get; private set; }
        public static string ProjectTitle { get; private set; }
        public static string WelcomeMessage { get; private set; }
        public static string ProjectFileFilter { get; private set; }
        public static string ConfirmDeleteDirectory { get; private set; }
        public static string ConfirmDeleteFile { get; private set; }
        public static string ConfirmDeleteInstance { get; private set; }
        public static string ConfirmDeleteCommLine { get; private set; }
        public static string FileOperationError { get; private set; }
        public static string DirectoryAlreadyExists { get; private set; }
        public static string FileAlreadyExists { get; private set; }
        public static string InstanceAlreadyExists { get; private set; }

        // Scada.Admin.App.Forms.FrmProjectNew
        public static string ChooseProjectLocation { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string ProjectLocationNotExists { get; private set; }
        public static string ProjectAlreadyExists { get; private set; }
        public static string ProjectTemplateEmpty { get; private set; }
        public static string ProjectTemplateNotFound { get; private set; }

        // Scada.Admin.App.Forms.FrmTextEditor
        public static string OpenTextFileError { get; private set; }
        public static string SaveTextFileError { get; private set; }

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
            EmptyNode = dict.GetPhrase("EmptyNode");

            dict = Localization.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            ProfileNotSet = dict.GetPhrase("ProfileNotSet");
            ConfirmDeleteProfile = dict.GetPhrase("ConfirmDeleteProfile");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmConnSettings");
            EmptyFieldsNotAllowed = dict.GetPhrase("EmptyFieldsNotAllowed");
            ProfileNameDuplicated = dict.GetPhrase("ProfileNameDuplicated");
            IncorrectSecretKey = dict.GetPhrase("IncorrectSecretKey");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmDownloadConfig");
            NothingToDownload = dict.GetPhrase("NothingToDownload");
            DownloadConfigComplete = dict.GetPhrase("DownloadConfigComplete");
            DownloadConfigError = dict.GetPhrase("DownloadConfigError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceProfile");
            ConnectionOK = dict.GetPhrase("ConnectionOK");
            TestConnectionError = dict.GetPhrase("TestConnectionError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmInstanceStatus");
            NormalSvcStatus = dict.GetPhrase("NormalSvcStatus");
            StoppedSvcStatus = dict.GetPhrase("StoppedSvcStatus");
            ErrorSvcStatus = dict.GetPhrase("ErrorSvcStatus");
            UndefinedSvcStatus = dict.GetPhrase("UndefinedSvcStatus");
            ServiceRestarted = dict.GetPhrase("ServiceRestarted");
            UnableRestartService = dict.GetPhrase("UnableRestartService");
            ServiceRestartError = dict.GetPhrase("ServiceRestartError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmUploadConfig");
            NothingToUpload = dict.GetPhrase("NothingToUpload");
            UploadConfigComplete = dict.GetPhrase("UploadConfigComplete");
            UploadConfigError = dict.GetPhrase("UploadConfigError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmBaseTable");
            GridViewError = dict.GetPhrase("GridViewError");
            ColumnLabel = dict.GetPhrase("ColumnLabel");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmFileNew");
            FileNameEmpty = dict.GetPhrase("FileNameEmpty");
            FileNameInvalid = dict.GetPhrase("FileNameInvalid");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmInstanceEdit");
            NewInstanceTitle = dict.GetPhrase("NewInstanceTitle");
            EditInstanceTitle = dict.GetPhrase("EditInstanceTitle");
            InstanceNameEmpty = dict.GetPhrase("InstanceNameEmpty");
            InstanceNameInvalid = dict.GetPhrase("InstanceNameInvalid");
            InstanceSelectApps = dict.GetPhrase("InstanceSelectApps");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmItemName");
            ItemNameEmpty = dict.GetPhrase("ItemNameEmpty");
            ItemNameInvalid = dict.GetPhrase("ItemNameInvalid");
            ItemNameDuplicated = dict.GetPhrase("ItemNameDuplicated");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmMain");
            EmptyTitle = dict.GetPhrase("EmptyTitle");
            ProjectTitle = dict.GetPhrase("ProjectTitle");
            WelcomeMessage = dict.GetPhrase("WelcomeMessage");
            ProjectFileFilter = dict.GetPhrase("ProjectFileFilter");
            ConfirmDeleteDirectory = dict.GetPhrase("ConfirmDeleteDirectory");
            ConfirmDeleteFile = dict.GetPhrase("ConfirmDeleteFile");
            ConfirmDeleteInstance = dict.GetPhrase("ConfirmDeleteInstance");
            ConfirmDeleteCommLine = dict.GetPhrase("ConfirmDeleteCommLine");
            FileOperationError = dict.GetPhrase("FileOperationError");
            DirectoryAlreadyExists = dict.GetPhrase("DirectoryAlreadyExists");
            FileAlreadyExists = dict.GetPhrase("FileAlreadyExists");
            InstanceAlreadyExists = dict.GetPhrase("InstanceAlreadyExists");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmProjectNew");
            ChooseProjectLocation = dict.GetPhrase("ChooseProjectLocation");
            ProjectNameEmpty = dict.GetPhrase("ProjectNameEmpty");
            ProjectNameInvalid = dict.GetPhrase("ProjectNameInvalid");
            ProjectLocationNotExists = dict.GetPhrase("ProjectLocationNotExists");
            ProjectAlreadyExists = dict.GetPhrase("ProjectAlreadyExists");
            ProjectTemplateEmpty = dict.GetPhrase("ProjectTemplateEmpty");
            ProjectTemplateNotFound = dict.GetPhrase("ProjectTemplateNotFound");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmTextEditor");
            OpenTextFileError = dict.GetPhrase("OpenTextFileError");
            SaveTextFileError = dict.GetPhrase("SaveTextFileError");
        }
    }
}
