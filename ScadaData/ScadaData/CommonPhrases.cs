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
 * Module   : ScadaData
 * Summary  : The common phrases used in the system
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2019
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada
{
    /// <summary>
    /// The common phrases used in the system
    /// <para>Общие фразы, используемые в системе</para>
    /// </summary>
    public static class CommonPhrases
    {
        static CommonPhrases()
        {
            SetToDefault();
        }

        public static string ProductName { get; private set; }
        public static string InfoCaption { get; private set; }
        public static string QuestionCaption { get; private set; }
        public static string ErrorCaption { get; private set; }
        public static string WarningCaption { get; private set; }
        public static string Error { get; private set; }
        public static string ErrorWithColon { get; private set; }
        public static string UnhandledException { get; private set; }
        public static string SaveSettingsConfirm { get; private set; }
        public static string FileNotFound { get; private set; }
        public static string DirNotExists { get; private set; }
        public static string NamedFileNotFound { get; private set; }
        public static string NamedDirNotExists { get; private set; }
        public static string BaseDATDir { get; private set; }
        public static string BaseDATDirNotExists { get; private set; }
        public static string ChooseBaseDATDir { get; private set; }
        public static string LoadAppSettingsError { get; private set; }
        public static string SaveAppSettingsError { get; private set; }
        public static string LoadCommSettingsError { get; private set; }
        public static string SaveCommSettingsError { get; private set; }
        public static string GridDataError { get; private set; }
        public static string IntegerRequired { get; private set; }
        public static string IntegerRangingRequired { get; private set; }
        public static string RealRequired { get; private set; }
        public static string NonemptyRequired { get; private set; }
        public static string DateTimeRequired { get; private set; }
        public static string LineLengthLimit { get; private set; }
        public static string NotNumber { get; private set; }
        public static string NotHexadecimal { get; private set; }
        public static string LoadImageError { get; private set; }
        public static string LoadHyperlinkError { get; private set; }
        public static string IncorrectFileFormat { get; private set; }
        public static string NoData { get; private set; }
        public static string NoRights { get; private set; }
        public static string IncorrectXmlNodeVal { get; private set; }
        public static string IncorrectXmlAttrVal { get; private set; }
        public static string IncorrectXmlParamVal { get; private set; }
        public static string XmlNodeNotFound { get; private set; }
        public static string EventAck { get; private set; }
        public static string EventNotAck { get; private set; }
        public static string IncorrectCmdVal { get; private set; }
        public static string IncorrectCmdData { get; private set; }

        public static string CmdTypeTable { get; private set; }
        public static string CmdValTable { get; private set; }
        public static string CnlTypeTable { get; private set; }
        public static string CommLineTable { get; private set; }
        public static string CtrlCnlTable { get; private set; }
        public static string EvTypeTable { get; private set; }
        public static string FormatTable { get; private set; }
        public static string FormulaTable { get; private set; }
        public static string InCnlTable { get; private set; }
        public static string InterfaceTable { get; private set; }
        public static string KPTable { get; private set; }
        public static string KPTypeTable { get; private set; }
        public static string ObjTable { get; private set; }
        public static string ParamTable { get; private set; }
        public static string RightTable { get; private set; }
        public static string RoleTable { get; private set; }
        public static string RoleRefTable { get; private set; }
        public static string UnitTable { get; private set; }
        public static string UserTable { get; private set; }

        public static string ContinuePendingSvcState { get; private set; }
        public static string PausedSvcState { get; private set; }
        public static string PausePendingSvcState { get; private set; }
        public static string RunningSvcState { get; private set; }
        public static string StartPendingSvcState { get; private set; }
        public static string StoppedSvcState { get; private set; }
        public static string StopPendingSvcState { get; private set; }
        public static string NotInstalledSvcState { get; private set; }

        private static void SetToDefault()
        {
            ProductName = Localization.Dict.GetEmptyPhrase("ProductName");
            InfoCaption = Localization.Dict.GetEmptyPhrase("InfoCaption");
            QuestionCaption = Localization.Dict.GetEmptyPhrase("QuestionCaption");
            ErrorCaption = Localization.Dict.GetEmptyPhrase("ErrorCaption");
            WarningCaption = Localization.Dict.GetEmptyPhrase("WarningCaption");
            Error = Localization.Dict.GetEmptyPhrase("Error");
            ErrorWithColon = Localization.Dict.GetEmptyPhrase("ErrorWithColon");
            UnhandledException = Localization.Dict.GetEmptyPhrase("UnhandledException");
            SaveSettingsConfirm = Localization.Dict.GetEmptyPhrase("SaveSettingsConfirm");
            FileNotFound = Localization.Dict.GetEmptyPhrase("FileNotFound");
            DirNotExists = Localization.Dict.GetEmptyPhrase("DirNotExists");
            NamedFileNotFound = Localization.Dict.GetEmptyPhrase("NamedFileNotFound");
            NamedDirNotExists = Localization.Dict.GetEmptyPhrase("NamedDirNotExists");
            BaseDATDir = Localization.Dict.GetEmptyPhrase("BaseDATDir");
            BaseDATDirNotExists = Localization.Dict.GetEmptyPhrase("BaseDATDirNotExists");
            ChooseBaseDATDir = Localization.Dict.GetEmptyPhrase("ChooseBaseDATDir");
            LoadAppSettingsError = Localization.Dict.GetEmptyPhrase("LoadAppSettingsError");
            SaveAppSettingsError = Localization.Dict.GetEmptyPhrase("SaveAppSettingsError");
            LoadCommSettingsError = Localization.Dict.GetEmptyPhrase("LoadCommSettingsError");
            SaveCommSettingsError = Localization.Dict.GetEmptyPhrase("SaveCommSettingsError");
            GridDataError = Localization.Dict.GetEmptyPhrase("GridDataError");
            IntegerRequired = Localization.Dict.GetEmptyPhrase("IntegerRequired");
            IntegerRangingRequired = Localization.Dict.GetEmptyPhrase("IntegerRangingRequired");
            RealRequired = Localization.Dict.GetEmptyPhrase("RealRequired");
            NonemptyRequired = Localization.Dict.GetEmptyPhrase("NonemptyRequired");
            DateTimeRequired = Localization.Dict.GetEmptyPhrase("DateTimeRequired");
            LineLengthLimit = Localization.Dict.GetEmptyPhrase("LineLengthLimit");
            NotNumber = Localization.Dict.GetEmptyPhrase("NotNumber");
            NotHexadecimal = Localization.Dict.GetEmptyPhrase("NotHexadecimal");
            LoadImageError = Localization.Dict.GetEmptyPhrase("LoadImageError");
            LoadHyperlinkError = Localization.Dict.GetEmptyPhrase("LoadHyperlinkError");
            IncorrectFileFormat = Localization.Dict.GetEmptyPhrase("IncorrectFileFormat");
            NoData = Localization.Dict.GetEmptyPhrase("NoData");
            NoRights = Localization.Dict.GetEmptyPhrase("NoRights");
            IncorrectXmlNodeVal = Localization.Dict.GetEmptyPhrase("IncorrectXmlNodeVal");
            IncorrectXmlAttrVal = Localization.Dict.GetEmptyPhrase("IncorrectXmlAttrVal");
            IncorrectXmlParamVal = Localization.Dict.GetEmptyPhrase("IncorrectXmlParamVal");
            XmlNodeNotFound = Localization.Dict.GetEmptyPhrase("XmlNodeNotFound");
            EventAck = Localization.Dict.GetEmptyPhrase("EventAck");
            EventNotAck = Localization.Dict.GetEmptyPhrase("EventNotAck");
            IncorrectCmdVal = Localization.Dict.GetEmptyPhrase("IncorrectCmdVal");
            IncorrectCmdData = Localization.Dict.GetEmptyPhrase("IncorrectCmdData");

            CmdTypeTable = Localization.Dict.GetEmptyPhrase("CmdTypeTable");
            CmdValTable = Localization.Dict.GetEmptyPhrase("CmdValTable");
            CnlTypeTable = Localization.Dict.GetEmptyPhrase("CnlTypeTable");
            CommLineTable = Localization.Dict.GetEmptyPhrase("CommLineTable");
            CtrlCnlTable = Localization.Dict.GetEmptyPhrase("CtrlCnlTable");
            EvTypeTable = Localization.Dict.GetEmptyPhrase("EvTypeTable");
            FormatTable = Localization.Dict.GetEmptyPhrase("FormatTable");
            FormulaTable = Localization.Dict.GetEmptyPhrase("FormulaTable");
            InCnlTable = Localization.Dict.GetEmptyPhrase("InCnlTable");
            InterfaceTable = Localization.Dict.GetEmptyPhrase("InterfaceTable");
            KPTable = Localization.Dict.GetEmptyPhrase("KPTable");
            KPTypeTable = Localization.Dict.GetEmptyPhrase("KPTypeTable");
            ObjTable = Localization.Dict.GetEmptyPhrase("ObjTable");
            ParamTable = Localization.Dict.GetEmptyPhrase("ParamTable");
            RightTable = Localization.Dict.GetEmptyPhrase("RightTable");
            RoleTable = Localization.Dict.GetEmptyPhrase("RoleTable");
            UnitTable = Localization.Dict.GetEmptyPhrase("UnitTable");
            UserTable = Localization.Dict.GetEmptyPhrase("UserTable");

            ContinuePendingSvcState = Localization.Dict.GetEmptyPhrase("ContinuePendingSvcState");
            PausedSvcState = Localization.Dict.GetEmptyPhrase("PausedSvcState");
            PausePendingSvcState = Localization.Dict.GetEmptyPhrase("PausePendingSvcState");
            RunningSvcState = Localization.Dict.GetEmptyPhrase("RunningSvcState");
            StartPendingSvcState = Localization.Dict.GetEmptyPhrase("StartPendingSvcState");
            StoppedSvcState = Localization.Dict.GetEmptyPhrase("StoppedSvcState");
            StopPendingSvcState = Localization.Dict.GetEmptyPhrase("StopPendingSvcState");
            NotInstalledSvcState = Localization.Dict.GetEmptyPhrase("NotInstalledSvcState");
        }

        public static void Init()
        {
            if (Localization.Dictionaries.TryGetValue("Common", out Localization.Dict dict))
            {
                ProductName = dict.GetPhrase("ProductName", ProductName);
                InfoCaption = dict.GetPhrase("InfoCaption", InfoCaption);
                QuestionCaption = dict.GetPhrase("QuestionCaption", QuestionCaption);
                ErrorCaption = dict.GetPhrase("ErrorCaption", ErrorCaption);
                WarningCaption = dict.GetPhrase("WarningCaption", WarningCaption);
                Error = dict.GetPhrase("Error", Error);
                ErrorWithColon = dict.GetPhrase("ErrorWithColon", ErrorWithColon);
                UnhandledException = dict.GetPhrase("UnhandledException", UnhandledException);
                SaveSettingsConfirm = dict.GetPhrase("SaveSettingsConfirm", SaveSettingsConfirm);
                FileNotFound = dict.GetPhrase("FileNotFound", FileNotFound);
                DirNotExists = dict.GetPhrase("DirNotExists", DirNotExists);
                NamedFileNotFound = dict.GetPhrase("NamedFileNotFound", NamedFileNotFound);
                NamedDirNotExists = dict.GetPhrase("NamedDirNotExists", NamedDirNotExists);
                BaseDATDir = dict.GetPhrase("BaseDATDir", BaseDATDir);
                BaseDATDirNotExists = dict.GetPhrase("BaseDATDirNotExists", BaseDATDirNotExists);
                ChooseBaseDATDir = dict.GetPhrase("ChooseBaseDATDir", ChooseBaseDATDir);
                LoadAppSettingsError = dict.GetPhrase("LoadAppSettingsError", LoadAppSettingsError);
                SaveAppSettingsError = dict.GetPhrase("SaveAppSettingsError", SaveAppSettingsError);
                LoadCommSettingsError = dict.GetPhrase("LoadCommSettingsError", LoadCommSettingsError);
                SaveCommSettingsError = dict.GetPhrase("SaveCommSettingsError", SaveCommSettingsError);
                GridDataError = dict.GetPhrase("GridDataError", GridDataError);
                IntegerRequired = dict.GetPhrase("IntegerRequired", IntegerRequired);
                IntegerRangingRequired = dict.GetPhrase("IntegerRangingRequired", IntegerRangingRequired);
                RealRequired = dict.GetPhrase("RealRequired", RealRequired);
                NonemptyRequired = dict.GetPhrase("NonemptyRequired", NonemptyRequired);
                DateTimeRequired = dict.GetPhrase("DateTimeRequired", DateTimeRequired);
                LineLengthLimit = dict.GetPhrase("LineLengthLimit", LineLengthLimit);
                NotNumber = dict.GetPhrase("NotNumber", NotNumber);
                NotHexadecimal = dict.GetPhrase("NotHexadecimal", NotHexadecimal);
                LoadImageError = dict.GetPhrase("LoadImageError", LoadImageError);
                LoadHyperlinkError = dict.GetPhrase("LoadHyperlinkError", LoadHyperlinkError);
                NoData = dict.GetPhrase("NoData", NoData);
                NoRights = dict.GetPhrase("NoRights", NoRights);
                IncorrectXmlNodeVal = dict.GetPhrase("IncorrectXmlNodeVal", IncorrectXmlNodeVal);
                IncorrectXmlAttrVal = dict.GetPhrase("IncorrectXmlAttrVal", IncorrectXmlAttrVal);
                IncorrectXmlParamVal = dict.GetPhrase("IncorrectXmlParamVal", IncorrectXmlParamVal);
                XmlNodeNotFound = dict.GetPhrase("XmlNodeNotFound", XmlNodeNotFound);
                EventAck = dict.GetPhrase("EventAck", EventAck);
                EventNotAck = dict.GetPhrase("EventNotAck", EventNotAck);
                IncorrectCmdVal = dict.GetPhrase("IncorrectCmdVal", IncorrectCmdVal);
                IncorrectCmdData = dict.GetPhrase("IncorrectCmdData", IncorrectCmdData);

                CmdTypeTable = dict.GetPhrase("CmdTypeTable", CmdTypeTable);
                CmdValTable = dict.GetPhrase("CmdValTable", CmdValTable);
                CnlTypeTable = dict.GetPhrase("CnlTypeTable", CnlTypeTable);
                CommLineTable = dict.GetPhrase("CommLineTable", CommLineTable);
                CtrlCnlTable = dict.GetPhrase("CtrlCnlTable", CtrlCnlTable);
                EvTypeTable = dict.GetPhrase("EvTypeTable", EvTypeTable);
                FormatTable = dict.GetPhrase("FormatTable", FormatTable);
                FormulaTable = dict.GetPhrase("FormulaTable", FormulaTable);
                InCnlTable = dict.GetPhrase("InCnlTable", InCnlTable);
                InterfaceTable = dict.GetPhrase("InterfaceTable", InterfaceTable);
                KPTable = dict.GetPhrase("KPTable", KPTable);
                KPTypeTable = dict.GetPhrase("KPTypeTable", KPTypeTable);
                ObjTable = dict.GetPhrase("ObjTable", ObjTable);
                ParamTable = dict.GetPhrase("ParamTable", ParamTable);
                RightTable = dict.GetPhrase("RightTable", RightTable);
                RoleTable = dict.GetPhrase("RoleTable", RoleTable);
                RoleRefTable = dict.GetPhrase("RoleRefTable", RoleRefTable);
                UnitTable = dict.GetPhrase("UnitTable", UnitTable);
                UserTable = dict.GetPhrase("UserTable", UserTable);

                ContinuePendingSvcState = dict.GetPhrase("ContinuePendingSvcState", ContinuePendingSvcState);
                PausedSvcState = dict.GetPhrase("PausedSvcState", PausedSvcState);
                PausePendingSvcState = dict.GetPhrase("PausePendingSvcState", PausePendingSvcState);
                RunningSvcState = dict.GetPhrase("RunningSvcState", RunningSvcState);
                StartPendingSvcState = dict.GetPhrase("StartPendingSvcState", StartPendingSvcState);
                StoppedSvcState = dict.GetPhrase("StoppedSvcState", StoppedSvcState);
                StopPendingSvcState = dict.GetPhrase("StopPendingSvcState", StopPendingSvcState);
                NotInstalledSvcState = dict.GetPhrase("NotInstalledSvcState", NotInstalledSvcState);
            }
        }
    }
}
