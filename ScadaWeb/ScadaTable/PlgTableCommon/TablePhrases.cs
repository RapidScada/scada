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
 * Module   : PlgTableCommon
 * Summary  : The phrases used by the table plugin and editor
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2019
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Table
{
    /// <summary>
    /// The phrases used by the table plugin and editor.
    /// <para>Фразы, используемые плагином и редактором таблиц.</para>
    /// </summary>
    public static class TablePhrases
    {
        // Scada.Table.TableView
        public static string LoadTableViewError { get; private set; }
        public static string SaveTableViewError { get; private set; }

        // Scada.Table.Editor.Forms.FrmMain
        public static string EditorTitle { get; private set; }
        public static string DefaultTableTitle { get; private set; }
        public static string TableFileFilter { get; private set; }
        public static string SaveTableConfirm { get; private set; }
        public static string BaseNotFound { get; private set; }
        public static string EmptyDeviceNode { get; private set; }
        public static string LoadConfigBaseError { get; private set; }
        public static string FillCnlTreeError { get; private set; }
        public static string LoadFormStateError { get; private set; }
        public static string SaveFormStateError { get; private set; }

        // Scada.Web.Plugins.Table.EventsRepBuilder
        public static string EventsWorksheet { get; private set; }
        public static string AllEventsTitle { get; private set; }
        public static string EventsByViewTitle { get; private set; }
        public static string EventsGen { get; private set; }
        public static string NumCol { get; private set; }
        public static string TimeCol { get; private set; }
        public static string ObjCol { get; private set; }
        public static string DevCol { get; private set; }
        public static string CnlCol { get; private set; }
        public static string TextCol { get; private set; }
        public static string AckCol { get; private set; }

        // Scada.Web.Plugins.Table.HourDataRepBuilder
        public static string HourDataWorksheet { get; private set; }
        public static string HourDataTitle { get; private set; }
        public static string HourDataGen { get; private set; }

        // Scada.Web.Plugins.Table.EventsWndSpec
        public static string EventsTitle { get; private set; }

        // Scada.Web.Plugins.Table.WFrmTable
        public static string SelectedDay { get; private set; }
        public static string PreviousDay { get; private set; }
        public static string PrevDayItem { get; private set; }
        public static string ItemCol { get; private set; }
        public static string CurCol { get; private set; }
        public static string InCnlHint { get; private set; }
        public static string CtrlCnlHint { get; private set; }
        public static string ObjectHint { get; private set; }
        public static string DeviceHint { get; private set; }
        public static string QuantityHint { get; private set; }
        public static string UnitHint { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Table.TableView");
            LoadTableViewError = dict.GetPhrase("LoadTableViewError");
            SaveTableViewError = dict.GetPhrase("SaveTableViewError");

            dict = Localization.GetDictionary("Scada.Table.Editor.Forms.FrmMain");
            EditorTitle = dict.GetPhrase("EditorTitle");
            DefaultTableTitle = dict.GetPhrase("DefaultTableTitle");
            TableFileFilter = dict.GetPhrase("TableFileFilter");
            SaveTableConfirm = dict.GetPhrase("SaveTableConfirm");
            BaseNotFound = dict.GetPhrase("BaseNotFound");
            EmptyDeviceNode = dict.GetPhrase("EmptyDeviceNode");
            LoadConfigBaseError = dict.GetPhrase("LoadConfigBaseError");
            FillCnlTreeError = dict.GetPhrase("FillCnlTreeError");
            LoadFormStateError = dict.GetPhrase("LoadFormStateError");
            SaveFormStateError = dict.GetPhrase("SaveFormStateError");

            dict = Localization.GetDictionary("Scada.Web.Plugins.Table.EventsRepBuilder");
            EventsWorksheet = dict.GetPhrase("EventsWorksheet", "EventsWorksheet"); // the default phrase must comply with Excel restrictions
            AllEventsTitle = dict.GetPhrase("AllEventsTitle");
            EventsByViewTitle = dict.GetPhrase("EventsByViewTitle");
            EventsGen = dict.GetPhrase("EventsGen");
            NumCol = dict.GetPhrase("NumCol");
            TimeCol = dict.GetPhrase("TimeCol");
            ObjCol = dict.GetPhrase("ObjCol");
            DevCol = dict.GetPhrase("DevCol");
            CnlCol = dict.GetPhrase("CnlCol");
            TextCol = dict.GetPhrase("TextCol");
            AckCol = dict.GetPhrase("AckCol");

            dict = Localization.GetDictionary("Scada.Web.Plugins.Table.HourDataRepBuilder");
            HourDataWorksheet = dict.GetPhrase("HourDataWorksheet");
            HourDataTitle = dict.GetPhrase("HourDataTitle");
            HourDataGen = dict.GetPhrase("HourDataGen");

            dict = Localization.GetDictionary("Scada.Web.Plugins.Table.EventsWndSpec");
            EventsTitle = dict.GetPhrase("EventsTitle");

            dict = Localization.GetDictionary("Scada.Web.Plugins.Table.WFrmTable");
            SelectedDay = dict.GetPhrase("SelectedDay");
            PreviousDay = dict.GetPhrase("PreviousDay");
            PrevDayItem = dict.GetPhrase("PrevDayItem");
            ItemCol = dict.GetPhrase("ItemCol");
            CurCol = dict.GetPhrase("CurCol");
            InCnlHint = dict.GetPhrase("InCnlHint");
            CtrlCnlHint = dict.GetPhrase("CtrlCnlHint");
            ObjectHint = dict.GetPhrase("ObjectHint");
            DeviceHint = dict.GetPhrase("DeviceHint");
            QuantityHint = dict.GetPhrase("QuantityHint");
            UnitHint = dict.GetPhrase("UnitHint");
        }
    }
}
