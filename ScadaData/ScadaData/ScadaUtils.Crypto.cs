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
using System.Collections.Generic;
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
        /// Размер секретного ключа, байт
        /// </summary>
        public const int SecretKeySize = 32;
        /// <summary>
        /// Размер вектора инициализации, байт
        /// </summary>
        public const int IVSize = 16;


        /// <summary>
        /// Считать все данные из потока
        /// </summary>
        private static byte[] ReadToEnd(Stream inputStream)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                inputStream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }


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
            return BytesToHex(EncryptBytes(Encoding.UTF8.GetBytes(s), secretKey, vector));
        }

        /// <summary>
        /// Зашифровать массив байт
        /// </summary>
        public static byte[] EncryptBytes(byte[] bytes, byte[] secretKey, byte[] vector)
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
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    return memStream.ToArray();
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
            return Encoding.UTF8.GetString(DecryptBytes(HexToBytes(s), secretKey, vector));
        }

        /// <summary>
        /// Дешифровать массив байт
        /// </summary>
        public static byte[] DecryptBytes(byte[] bytes, byte[] secretKey, byte[] vector)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = vector };

                using (MemoryStream memStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateDecryptor(secretKey, vector), CryptoStreamMode.Read))
                    {
                        return ReadToEnd(cryptoStream);
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
