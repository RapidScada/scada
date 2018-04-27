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
 * Module   : ScadaData
 * Summary  : The class contains utility methods for the whole system. Cryptographic utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Scada
{
    partial class ScadaUtils
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
        /// Вычислить хеш-функцию MD5 по массиву байт
        /// </summary>
        public static string ComputeHash(byte[] bytes)
        {
            return BytesToHex(MD5.Create().ComputeHash(bytes));
        }

        /// <summary>
        /// Вычислить хеш-функцию MD5 по строке
        /// </summary>
        public static string ComputeHash(string s)
        {
            return ComputeHash(Encoding.UTF8.GetBytes(s));
        }

        /// <summary>
        /// Зашифровать строку
        /// </summary>
        public static string Encrypt(string s, byte[] secretKey, byte[] vector)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = vector };

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateEncryptor(secretKey, vector), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                            writer.Write(s);
                    }

                    return BytesToHex(memStream.ToArray());
                }
            }
            finally
            {
                alg?.Clear();
            }
        }

        /// <summary>
        /// Дешифровать строку
        /// </summary>
        public static string Decrypt(string s, byte[] secretKey, byte[] vector)
        {
            if (!HexToBytes(s, out byte[] encryptedData))
                throw new ArgumentException("String is not hexadecimal.", "s");

            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = vector };

                using (MemoryStream memStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateDecryptor(secretKey, vector), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                            return reader.ReadToEnd();
                    }
                }
            }
            finally
            {
                alg?.Clear();
            }
        }
    }
}
