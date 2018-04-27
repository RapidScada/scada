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
 * Module   : ScadaAgentCore
 * Summary  : The class contains utility cryptographic methods
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Text;

namespace Scada.Agent
{
    /// <summary>
    /// The class contains utility cryptographic methods
    /// <para>Класс, содержащий вспомогательные криптографические методы</para>
    /// </summary>
    public static class CryptoUtils
    {
        /// <summary>
        /// Зашифровать пароль
        /// </summary>
        public static string EncryptPassword(string password, long sessionID, byte[] secretKey, byte[] vector)
        {
            byte[] pwdBuf = Encoding.UTF8.GetBytes(password);
            byte[] sessBuf = BitConverter.GetBytes(sessionID);

            for (int i = 0, cnt = Math.Min(pwdBuf.Length, sessBuf.Length); i < cnt; i++)
            {
                pwdBuf[i] ^= sessBuf[i];
            }

            return ScadaUtils.BytesToHex(ScadaUtils.EncryptBytes(pwdBuf, secretKey, vector));
        }

        /// <summary>
        /// Дешифровать пароль
        /// </summary>
        public static string DecryptPassword(string encryptedPassword, long sessionID, byte[] secretKey, byte[] vector)
        {
            byte[] pwdBuf = ScadaUtils.HexToBytes(encryptedPassword);
            byte[] sessBuf = BitConverter.GetBytes(sessionID);

            for (int i = 0, cnt = Math.Min(pwdBuf.Length, sessBuf.Length); i < cnt; i++)
            {
                pwdBuf[i] ^= sessBuf[i];
            }

            return Encoding.UTF8.GetString(ScadaUtils.DecryptBytes(pwdBuf, secretKey, vector));
        }

        /// <summary>
        /// Дешифровать пароль, не вызывая исключений в случае ошибки
        /// </summary>
        public static string SafelyDecryptPassword(string encryptedPassword, long sessionID, 
            byte[] secretKey, byte[] vector)
        {
            try
            {
                return DecryptPassword(encryptedPassword, sessionID, secretKey, vector);
            }
            catch
            {
                return "";
            }
        }
    }
}
