/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Specifies the authentication modes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Comm.Devices.OpcUa.Config
{
    /// <summary>
    /// Specifies the authentication modes.
    /// <para>Задает режимы аутентификации.</para>
    /// </summary>
    public enum AuthenticationMode
    {
        Anonymous,
        Username
    }
}
