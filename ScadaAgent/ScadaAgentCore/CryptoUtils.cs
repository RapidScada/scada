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
using System.Security.Cryptography;

namespace Scada.Agent
{
    /// <summary>
    /// The class contains utility cryptographic methods
    /// <para>Класс, содержащий вспомогательные криптографические методы</para>
    /// </summary>
    public static class CryptoUtils
    {
        /// <summary>
        /// Генератор криптографически защищённых случайных чисел
        /// </summary>
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();


        /// <summary>
        /// Получить случайное 64-битное целое
        /// </summary>
        public static long GetRandomLong()
        {
            byte[] randomArr = new byte[8];
            Rng.GetBytes(randomArr);
            return BitConverter.ToInt64(randomArr, 0);
        }

        /// <summary>
        /// Зашифровать пароль
        /// </summary>
        public static string EncryptPassword(string password, long sessionID)
        {
            // TODO
            return password;
        }

        /// <summary>
        /// Дешифровать пароль
        /// </summary>
        public static string DecryptPassword(string encryptedPassword, long sessionID)
        {
            // TODO
            return encryptedPassword;
        }
    }
}
