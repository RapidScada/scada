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
 * Module   : Administrator
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The phrases used in the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Admin.App.Code.AppState
        public static string LoadAppStateError { get; private set; }
        public static string SaveAppStateError { get; private set; }

        // Scada.Admin.App.Code.CnlMap
        public static string CnlMapByDevice { get; private set; }
        public static string CnlMapByObject { get; private set; }
        public static string InCnlsCaption { get; private set; }
        public static string OutCnlsCaption { get; private set; }
        public static string DeviceCaption { get; private set; }
        public static string ObjectCaption { get; private set; }
        public static string EmptyDevice { get; private set; }
        public static string EmptyObject { get; private set; }
        public static string NoChannels { get; private set; }
        public static string GenerateCnlMapError { get; private set; }

        // Scada.Admin.App.Code.ExplorerBuilder
        public static string BaseNode { get; private set; }
        public static string SysTableNode { get; private set; }
        public static string DictTableNode { get; private set; }
        public static string TableByDeviceNode { get; private set; }
        public static string EmptyDeviceNode { get; private set; }
        public static string InterfaceNode { get; private set; }
        public static string InstancesNode { get; private set; }
        public static string ServerNode { get; private set; }
        public static string CommNode { get; private set; }
        public static string WebNode { get; private set; }
        public static string EmptyNode { get; private set; }
        public static string DeviceFilter { get; private set; }
        public static string EmptyDeviceFilter { get; private set; }

        // Scada.Admin.App.Code.IntegrityCheck
        public static string IntegrityCheckTitle { get; private set; }
        public static string TableCorrect { get; private set; }
        public static string TableHasErrors { get; private set; }
        public static string LostPrimaryKeys { get; private set; }
        public static string BaseCorrect { get; private set; }
        public static string BaseHasErrors { get; private set; }
        public static string IntegrityCheckError { get; private set; }

        // Scada.Admin.App.Controls.Deployment.CtrlProfileSelector
        public static string ProfileNotSet { get; private set; }
        public static string ConfirmDeleteProfile { get; private set; }

        // Scada.Admin.App.Controls.Deployment.CtrlTransferSettings
        public static string ConfigNotSelected { get; private set; }
        public static string IncorrectObjFilter { get; private set; }

        // Scada.Admin.App.Forms
        public static string CorrectErrors { get; private set; }
        public static string AllCommLines { get; private set; }
        public static string SelectedColumn { get; private set; }

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
        public static string ServiceCommandComplete { get; private set; }
        public static string UnableControlService { get; private set; }
        public static string ControlServiceError { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmProfileEdit
        public static string ValidUrlRequired { get; private set; }
        public static string ProfileNameDuplicated { get; private set; }
        public static string IncorrectSecretKey { get; private set; }

        // Scada.Admin.App.Forms.Deployment.FrmUploadConfig
        public static string UploadConfigComplete { get; private set; }
        public static string UploadConfigError { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmBaseTable
        public static string GridViewError { get; private set; }
        public static string ColumnLabel { get; private set; }
        public static string DeleteRowConfirm { get; private set; }
        public static string DeleteRowsConfirm { get; private set; }
        public static string ClearTableConfirm { get; private set; }
        public static string RowsNotDeleted { get; private set; }
        public static string ColumnNotNull { get; private set; }
        public static string UniqueRequired { get; private set; }
        public static string KeyReferenced { get; private set; }
        public static string DataNotExist { get; private set; }
        public static string DataChangeError { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmFilter
        public static string IncorrectTableFilter { get; private set; }

        // Scada.Admin.App.Forms.Tables.FrmFind
        public static string ValueNotFound { get; private set; }
        public static string SearchComplete { get; private set; }
        public static string ReplaceCount { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCnlClone
        public static string KeepUnchanged { get; private set; }
        public static string CloneCnlsComplete { get; private set; }
        public static string CloneInCnlsError { get; private set; }
        public static string CloneCtrlCnlsError { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCnlCreate
        public static string CreateCnlsStep1 { get; private set; }
        public static string CreateCnlsStep2 { get; private set; }
        public static string CreateCnlsStep3 { get; private set; }
        public static string DeviceInfo { get; private set; }
        public static string DeviceNotFound { get; private set; }
        public static string NoDeviceSelected { get; private set; }
        public static string CreateCnlsComplete { get; private set; }
        public static string CreateCnlsError { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCommImport
        public static string NoDataSelected { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmCulture
        public static string LoadCulturesError { get; private set; }
        public static string CultureRequired { get; private set; }
        public static string CultureNotFound { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmDeviceAdd
        public static string DeviceAlreadyExists { get; private set; }
        public static string CommLineNotFound { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmFileAssociation
        public static string ExecutableFileFilter { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmLineAdd
        public static string LineAlreadyExists { get; private set; }

        // Scada.Admin.App.Forms.Tools.FrmSettings
        public static string ChooseServerDir { get; private set; }
        public static string ChooseCommDir { get; private set; }

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
        public static string SelectItemMessage { get; private set; }
        public static string ProjectFileFilter { get; private set; }
        public static string ConfirmDeleteDirectory { get; private set; }
        public static string ConfirmDeleteFile { get; private set; }
        public static string ConfirmDeleteInstance { get; private set; }
        public static string ConfirmDeleteCommLine { get; private set; }
        public static string FileOperationError { get; private set; }
        public static string DirectoryAlreadyExists { get; private set; }
        public static string FileAlreadyExists { get; private set; }
        public static string InstanceAlreadyExists { get; private set; }
        public static string SaveConfigBaseConfirm { get; private set; }
        public static string DeviceNotFoundInComm { get; private set; }
        public static string WebUrlNotSet { get; private set; }
        public static string ReopenProject { get; private set; }

        // Scada.Admin.App.Forms.FrmProjectNew
        public static string ChooseProjectLocation { get; private set; }
        public static string ProjectNameEmpty { get; private set; }
        public static string ProjectNameInvalid { get; private set; }
        public static string ProjectLocationNotExists { get; private set; }
        public static string ProjectAlreadyExists { get; private set; }
        public static string ProjectTemplateEmpty { get; private set; }
        public static string ProjectTemplateNotFound { get; private set; }

        // Scada.Admin.App.Forms.FrmTableExport
        public static string ExportTableFilter { get; private set; }

        // Scada.Admin.App.Forms.FrmTableImport
        public static string ImportTableFilter { get; private set; }
        public static string ImportTableComplete { get; private set; }

        // Scada.Admin.App.Forms.FrmTextEditor
        public static string OpenTextFileError { get; private set; }
        public static string SaveTextFileError { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Admin.App.Code.AppState");
            LoadAppStateError = dict.GetPhrase("LoadAppStateError");
            SaveAppStateError = dict.GetPhrase("SaveAppStateError");

            dict = Localization.GetDictionary("Scada.Admin.App.Code.CnlMap");
            CnlMapByDevice = dict.GetPhrase("CnlMapByDevice");
            CnlMapByObject = dict.GetPhrase("CnlMapByObject");
            InCnlsCaption = dict.GetPhrase("InCnlsCaption");
            OutCnlsCaption = dict.GetPhrase("OutCnlsCaption");
            DeviceCaption = dict.GetPhrase("DeviceCaption");
            ObjectCaption = dict.GetPhrase("ObjectCaption");
            EmptyDevice = dict.GetPhrase("EmptyDevice");
            EmptyObject = dict.GetPhrase("EmptyObject");
            NoChannels = dict.GetPhrase("NoChannels");
            GenerateCnlMapError = dict.GetPhrase("GenerateCnlMapError");

            dict = Localization.GetDictionary("Scada.Admin.App.Code.ExplorerBuilder");
            BaseNode = dict.GetPhrase("BaseNode");
            SysTableNode = dict.GetPhrase("SysTableNode");
            DictTableNode = dict.GetPhrase("DictTableNode");
            TableByDeviceNode = dict.GetPhrase("TableByDeviceNode");
            EmptyDeviceNode = dict.GetPhrase("EmptyDeviceNode");
            InterfaceNode = dict.GetPhrase("InterfaceNode");
            InstancesNode = dict.GetPhrase("InstancesNode");
            ServerNode = dict.GetPhrase("ServerNode");
            CommNode = dict.GetPhrase("CommNode");
            WebNode = dict.GetPhrase("WebNode");
            EmptyNode = dict.GetPhrase("EmptyNode");
            DeviceFilter = dict.GetPhrase("DeviceFilter");
            EmptyDeviceFilter = dict.GetPhrase("EmptyDeviceFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Code.IntegrityCheck");
            IntegrityCheckTitle = dict.GetPhrase("IntegrityCheckTitle");
            TableCorrect = dict.GetPhrase("TableCorrect");
            TableHasErrors = dict.GetPhrase("TableHasErrors");
            LostPrimaryKeys = dict.GetPhrase("LostPrimaryKeys");
            BaseCorrect = dict.GetPhrase("BaseCorrect");
            BaseHasErrors = dict.GetPhrase("BaseHasErrors");
            IntegrityCheckError = dict.GetPhrase("IntegrityCheckError");

            dict = Localization.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlProfileSelector");
            ProfileNotSet = dict.GetPhrase("ProfileNotSet");
            ConfirmDeleteProfile = dict.GetPhrase("ConfirmDeleteProfile");

            dict = Localization.GetDictionary("Scada.Admin.App.Controls.Deployment.CtrlTransferSettings");
            ConfigNotSelected = dict.GetPhrase("ConfigNotSelected");
            IncorrectObjFilter = dict.GetPhrase("IncorrectObjFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms");
            CorrectErrors = dict.GetPhrase("CorrectErrors");
            AllCommLines = dict.GetPhrase("AllCommLines");
            SelectedColumn = dict.GetPhrase("SelectedColumn");

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
            ServiceCommandComplete = dict.GetPhrase("ServiceCommandComplete");
            UnableControlService = dict.GetPhrase("UnableControlService");
            ControlServiceError = dict.GetPhrase("ControlServiceError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmProfileEdit");
            ValidUrlRequired = dict.GetPhrase("ValidUrlRequired");
            ProfileNameDuplicated = dict.GetPhrase("ProfileNameDuplicated");
            IncorrectSecretKey = dict.GetPhrase("IncorrectSecretKey");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Deployment.FrmUploadConfig");
            UploadConfigComplete = dict.GetPhrase("UploadConfigComplete");
            UploadConfigError = dict.GetPhrase("UploadConfigError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tables.FrmBaseTable");
            GridViewError = dict.GetPhrase("GridViewError");
            ColumnLabel = dict.GetPhrase("ColumnLabel");
            DeleteRowConfirm = dict.GetPhrase("DeleteRowConfirm");
            DeleteRowsConfirm = dict.GetPhrase("DeleteRowsConfirm");
            ClearTableConfirm = dict.GetPhrase("ClearTableConfirm");
            RowsNotDeleted = dict.GetPhrase("RowsNotDeleted");
            ColumnNotNull = dict.GetPhrase("ColumnNotNull");
            UniqueRequired = dict.GetPhrase("UniqueRequired");
            KeyReferenced = dict.GetPhrase("KeyReferenced");
            DataNotExist = dict.GetPhrase("DataNotExist");
            DataChangeError = dict.GetPhrase("DataChangeError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFilter");
            IncorrectTableFilter = dict.GetPhrase("IncorrectTableFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tables.FrmFind");
            ValueNotFound = dict.GetPhrase("ValueNotFound");
            SearchComplete = dict.GetPhrase("SearchComplete");
            ReplaceCount = dict.GetPhrase("ReplaceCount");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCnlClone");
            KeepUnchanged = dict.GetPhrase("KeepUnchanged");
            CloneCnlsComplete = dict.GetPhrase("CloneCnlsComplete");
            CloneInCnlsError = dict.GetPhrase("CloneInCnlsError");
            CloneCtrlCnlsError = dict.GetPhrase("CloneCtrlCnlsError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCnlCreate");
            CreateCnlsStep1 = dict.GetPhrase("CreateCnlsStep1");
            CreateCnlsStep2 = dict.GetPhrase("CreateCnlsStep2");
            CreateCnlsStep3 = dict.GetPhrase("CreateCnlsStep3");
            DeviceInfo = dict.GetPhrase("DeviceInfo");
            DeviceNotFound = dict.GetPhrase("DeviceNotFound");
            NoDeviceSelected = dict.GetPhrase("NoDeviceSelected");
            CreateCnlsComplete = dict.GetPhrase("CreateCnlsComplete");
            CreateCnlsError = dict.GetPhrase("CreateCnlsError");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCommImport");
            NoDataSelected = dict.GetPhrase("NoDataSelected");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmCulture");
            LoadCulturesError = dict.GetPhrase("LoadCulturesError");
            CultureRequired = dict.GetPhrase("CultureRequired");
            CultureNotFound = dict.GetPhrase("CultureNotFound");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmDeviceAdd");
            DeviceAlreadyExists = dict.GetPhrase("DeviceAlreadyExists");
            CommLineNotFound = dict.GetPhrase("CommLineNotFound");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmFileAssociation");
            ExecutableFileFilter = dict.GetPhrase("ExecutableFileFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmLineAdd");
            LineAlreadyExists = dict.GetPhrase("LineAlreadyExists");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.Tools.FrmSettings");
            ChooseServerDir = dict.GetPhrase("ChooseServerDir");
            ChooseCommDir = dict.GetPhrase("ChooseCommDir");

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
            SelectItemMessage = dict.GetPhrase("SelectItemMessage");
            ProjectFileFilter = dict.GetPhrase("ProjectFileFilter");
            ConfirmDeleteDirectory = dict.GetPhrase("ConfirmDeleteDirectory");
            ConfirmDeleteFile = dict.GetPhrase("ConfirmDeleteFile");
            ConfirmDeleteInstance = dict.GetPhrase("ConfirmDeleteInstance");
            ConfirmDeleteCommLine = dict.GetPhrase("ConfirmDeleteCommLine");
            FileOperationError = dict.GetPhrase("FileOperationError");
            DirectoryAlreadyExists = dict.GetPhrase("DirectoryAlreadyExists");
            FileAlreadyExists = dict.GetPhrase("FileAlreadyExists");
            InstanceAlreadyExists = dict.GetPhrase("InstanceAlreadyExists");
            SaveConfigBaseConfirm = dict.GetPhrase("SaveConfigBaseConfirm");
            DeviceNotFoundInComm = dict.GetPhrase("DeviceNotFoundInComm");
            WebUrlNotSet = dict.GetPhrase("WebUrlNotSet");
            ReopenProject = dict.GetPhrase("ReopenProject");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmProjectNew");
            ChooseProjectLocation = dict.GetPhrase("ChooseProjectLocation");
            ProjectNameEmpty = dict.GetPhrase("ProjectNameEmpty");
            ProjectNameInvalid = dict.GetPhrase("ProjectNameInvalid");
            ProjectLocationNotExists = dict.GetPhrase("ProjectLocationNotExists");
            ProjectAlreadyExists = dict.GetPhrase("ProjectAlreadyExists");
            ProjectTemplateEmpty = dict.GetPhrase("ProjectTemplateEmpty");
            ProjectTemplateNotFound = dict.GetPhrase("ProjectTemplateNotFound");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmTableExport");
            ExportTableFilter = dict.GetPhrase("ExportTableFilter");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmTableImport");
            ImportTableFilter = dict.GetPhrase("ImportTableFilter");
            ImportTableComplete = dict.GetPhrase("ImportTableComplete");

            dict = Localization.GetDictionary("Scada.Admin.App.Forms.FrmTextEditor");
            OpenTextFileError = dict.GetPhrase("OpenTextFileError");
            SaveTextFileError = dict.GetPhrase("SaveTextFileError");
        }
    }
}
