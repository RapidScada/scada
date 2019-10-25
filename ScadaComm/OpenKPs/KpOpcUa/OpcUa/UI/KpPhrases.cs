/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : The phrases used by the library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// The phrases used by the library.
    /// <para>Фразы, используемые библиотекой.</para>
    /// </summary>
    public static class KpPhrases
    {
        // Scada.Comm.Devices.OpcUa.UI.FrmConfig
        public static string ConnectServerError { get; private set; }
        public static string DisconnectServerError { get; private set; }
        public static string BrowseServerError { get; private set; }
        public static string GetDataTypeError { get; private set; }
        public static string ServerUrlRequired { get; private set; }
        public static string EmptyNode { get; private set; }
        public static string SubscriptionsNode { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string EmptySubscription { get; private set; }
        public static string EmptyItem { get; private set; }
        public static string EmptyCommand { get; private set; }
        public static string UnknownDataType { get; private set; }

        // Scada.Comm.Devices.OpcUa.UI.FrmNodeAttr
        public static string ReadAttrError { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Comm.Devices.OpcUa.UI.FrmConfig");
            ConnectServerError = dict.GetPhrase("ConnectServerError");
            DisconnectServerError = dict.GetPhrase("DisconnectServerError");
            BrowseServerError = dict.GetPhrase("BrowseServerError");
            GetDataTypeError = dict.GetPhrase("GetDataTypeError");
            ServerUrlRequired = dict.GetPhrase("ServerUrlRequired");
            EmptyNode = dict.GetPhrase("EmptyNode");
            SubscriptionsNode = dict.GetPhrase("SubscriptionsNode");
            CommandsNode = dict.GetPhrase("CommandsNode");
            EmptySubscription = dict.GetPhrase("EmptySubscription");
            EmptyItem = dict.GetPhrase("EmptyItem");
            EmptyCommand = dict.GetPhrase("EmptyCommand");
            UnknownDataType = dict.GetPhrase("UnknownDataType");

            dict = Localization.GetDictionary("Scada.Comm.Devices.OpcUa.UI.FrmNodeAttr");
            ReadAttrError = dict.GetPhrase("ReadAttrError");
        }
    }
}
