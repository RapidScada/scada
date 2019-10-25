/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Specifies the connection security modes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Comm.Devices.OpcUa.Config
{
    /// <summary>
    /// Specifies the connection security policies.
    /// <para>Задает политики безопасности соединения.</para>
    /// </summary>
    public enum SecurityPolicy
    {
        None,
        Basic128Rsa15,
        Basic256,
        Basic256Sha256,
        Aes128_Sha256_RsaOaep,
        Aes256_Sha256_RsaPss,
        Https
    }
}
