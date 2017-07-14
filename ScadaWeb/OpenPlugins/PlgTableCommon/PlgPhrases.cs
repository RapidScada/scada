/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : The phrases used by the plugin
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web.Plugins.Table
{
    /// <summary>
    /// The phrases used by the plugin
    /// <para>Фразы, используемые плагином</para>
    /// </summary>
    public static class PlgPhrases
    {
        static PlgPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Web.Plugins.Table.EventsRepBuilder
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

        // Словарь Scada.Web.Plugins.Table.HourDataRepBuilder
        public static string HourDataWorksheet { get; private set; }
        public static string HourDataTitle { get; private set; }
        public static string HourDataGen { get; private set; }

        // Словарь Scada.Web.Plugins.Table.EventsWndSpec
        public static string EventsTitle { get; private set; }

        // Словарь Scada.Web.Plugins.Table.TableView
        public static string LoadTableViewError { get; private set; }
        public static string SaveTableViewError { get; private set; }

        // Словарь Scada.Web.Plugins.Table.WFrmTable
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

        private static void SetToDefault()
        {
            EventsWorksheet = "EventsWorksheet"; // GetEmptyPhrase() не удовлетворяет ограничениям Excel
            AllEventsTitle = Localization.Dict.GetEmptyPhrase("AllEventsTitle");
            EventsByViewTitle = Localization.Dict.GetEmptyPhrase("EventsByViewTitle");
            EventsGen = Localization.Dict.GetEmptyPhrase("EventsGen");
            NumCol = Localization.Dict.GetEmptyPhrase("NumCol");
            TimeCol = Localization.Dict.GetEmptyPhrase("TimeCol");
            ObjCol = Localization.Dict.GetEmptyPhrase("ObjCol");
            DevCol = Localization.Dict.GetEmptyPhrase("DevCol");
            CnlCol = Localization.Dict.GetEmptyPhrase("CnlCol");
            TextCol = Localization.Dict.GetEmptyPhrase("TextCol");
            AckCol = Localization.Dict.GetEmptyPhrase("AckCol");

            HourDataWorksheet = "HourDataWorksheet";
            HourDataTitle = Localization.Dict.GetEmptyPhrase("HourDataTitle");
            HourDataGen = Localization.Dict.GetEmptyPhrase("HourDataGen");

            EventsTitle = Localization.Dict.GetEmptyPhrase("EventsTitle");

            LoadTableViewError = Localization.Dict.GetEmptyPhrase("LoadTableViewError");
            SaveTableViewError = Localization.Dict.GetEmptyPhrase("SaveTableViewError");

            SelectedDay = Localization.Dict.GetEmptyPhrase("SelectedDay");
            PreviousDay = Localization.Dict.GetEmptyPhrase("PreviousDay");
            PrevDayItem = Localization.Dict.GetEmptyPhrase("PrevDayItem");
            ItemCol = Localization.Dict.GetEmptyPhrase("ItemCol");
            CurCol = Localization.Dict.GetEmptyPhrase("CurCol");
            InCnlHint = Localization.Dict.GetEmptyPhrase("InCnlHint");
            CtrlCnlHint = Localization.Dict.GetEmptyPhrase("CtrlCnlHint");
            ObjectHint = Localization.Dict.GetEmptyPhrase("ObjectHint");
            DeviceHint = Localization.Dict.GetEmptyPhrase("DeviceHint");
            QuantityHint = Localization.Dict.GetEmptyPhrase("QuantityHint");
            UnitHint = Localization.Dict.GetEmptyPhrase("UnitHint");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.EventsRepBuilder", out dict))
            {
                EventsWorksheet = dict.GetPhrase("EventsWorksheet", EventsWorksheet);
                AllEventsTitle = dict.GetPhrase("AllEventsTitle", AllEventsTitle);
                EventsByViewTitle = dict.GetPhrase("EventsByViewTitle", EventsByViewTitle);
                EventsGen = dict.GetPhrase("EventsGen", EventsGen);
                NumCol = dict.GetPhrase("NumCol", NumCol);
                TimeCol = dict.GetPhrase("TimeCol", TimeCol);
                ObjCol = dict.GetPhrase("ObjCol", ObjCol);
                DevCol = dict.GetPhrase("DevCol", DevCol);
                CnlCol = dict.GetPhrase("CnlCol", CnlCol);
                TextCol = dict.GetPhrase("TextCol", TextCol);
                AckCol = dict.GetPhrase("AckCol", AckCol);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.HourDataRepBuilder", out dict))
            {
                HourDataWorksheet = dict.GetPhrase("HourDataWorksheet", HourDataWorksheet);
                HourDataTitle = dict.GetPhrase("HourDataTitle", HourDataTitle);
                HourDataGen = dict.GetPhrase("HourDataGen", HourDataGen);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.EventsWndSpec", out dict))
            {
                EventsTitle = dict.GetPhrase("EventsTitle", EventsTitle);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.TableView", out dict))
            {
                LoadTableViewError = dict.GetPhrase("LoadTableViewError", LoadTableViewError);
                SaveTableViewError = dict.GetPhrase("SaveTableViewError", SaveTableViewError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.WFrmTable", out dict))
            {
                SelectedDay = dict.GetPhrase("SelectedDay", SelectedDay);
                PreviousDay = dict.GetPhrase("PreviousDay", PreviousDay);
                PrevDayItem = dict.GetPhrase("PrevDayItem", PrevDayItem);
                ItemCol = dict.GetPhrase("ItemCol", ItemCol);
                CurCol = dict.GetPhrase("CurCol", CurCol);
                InCnlHint = dict.GetPhrase("InCnlHint", InCnlHint);
                CtrlCnlHint = dict.GetPhrase("CtrlCnlHint", CtrlCnlHint);
                ObjectHint = dict.GetPhrase("ObjectHint", ObjectHint);
                DeviceHint = dict.GetPhrase("DeviceHint", DeviceHint);
                QuantityHint = dict.GetPhrase("QuantityHint", QuantityHint);
                UnitHint = dict.GetPhrase("UnitHint", UnitHint);
            }
        }
    }
}
