/*
 * Copyright 2021 Elena Shiryaeva
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModDbExport
 * Summary  : The phrases used by the module
 * 
 * Author   : Elena Shiryaeva
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Server.Modules.DbExport.UI
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    internal static class LibPhrases
    {
        // Scada.Server.Modules.DbExport.UI.FrmDbExportConfig
        public static string ConnectionOptionsNode { get; private set; }
        public static string ArcUploadOptionsNode { get; private set; }
        public static string TriggerGrNode { get; private set; }
        public static string CurDataTrigger { get; private set; }
        public static string ArcDataTrigger { get; private set; }
        public static string EventTrigger { get; private set; }
        public static string CurDataType { get; private set; }
        public static string ArcDataType { get; private set; }
        public static string EventType { get; private set; }
        public static string TargetName{ get; private set; }
        public static string TargetNameNotUnique { get; private set; }
        public static string NameEmpty { get; private set; }

        // Scada.Server.Modules.DbExport.UI.FrmRangeEdit
        public static string RangeNotValid { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Server.Modules.DbExport.UI.FrmDbExportConfig");
           
            ConnectionOptionsNode = dict.GetPhrase("ConnectionOptionsNode");
            ArcUploadOptionsNode = dict.GetPhrase("ArcUploadOptionsNode");
            TriggerGrNode = dict.GetPhrase("TriggerGrNode");

            CurDataType = dict.GetPhrase("CurDataType");
            ArcDataType = dict.GetPhrase("ArcDataType");
            EventType = dict.GetPhrase("EventType");

            CurDataTrigger = dict.GetPhrase("CurDataTrigger");
            ArcDataTrigger = dict.GetPhrase("ArcDataTrigger");
            EventTrigger = dict.GetPhrase("EventTrigger");
            TargetName = dict.GetPhrase("TargetName");

            NameEmpty = dict.GetPhrase("NameEmpty");
            TargetNameNotUnique = dict.GetPhrase("TargetNameNotUnique");

            dict = Localization.GetDictionary("Scada.Server.Modules.DbExport.UI.FrmRangeEdit");
            RangeNotValid = dict.GetPhrase("RangeNotValid");
        }
    }
}
