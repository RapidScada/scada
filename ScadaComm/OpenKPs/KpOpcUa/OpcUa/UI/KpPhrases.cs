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
        public static string EmptyNode { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Comm.Devices.OpcUa.UI.FrmConfig");
            ConnectServerError = dict.GetPhrase("ConnectServerError");
            DisconnectServerError = dict.GetPhrase("DisconnectServerError");
            BrowseServerError = dict.GetPhrase("BrowseServerError");
            EmptyNode = dict.GetPhrase("EmptyNode");
        }
    }
}
