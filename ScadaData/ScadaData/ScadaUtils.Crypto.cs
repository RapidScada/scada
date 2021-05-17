/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
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
        /// Секретный ключ для использования по умолчанию
        /// </summary>
        private static readonly byte[] DefaultSecretKey = new byte[SecretKeySize] {
            0x0A, 0xBA, 0x06, 0xBC, 0x1A, 0x5D, 0x44, 0x3E, 0x5A, 0xE8, 0x46, 0x7F, 0xB8, 0x85, 0x49, 0xF6,
            0xE9, 0xCC, 0x90, 0xF0, 0x80, 0x45, 0x33, 0xFC, 0x2A, 0x67, 0xD9, 0xBA, 0x00, 0xCE, 0xC7, 0x8A };
        /// <summary>
        /// Вектор инициализации для использования по умолчанию
        /// </summary>
        private static readonly byte[] DefaultIV = new byte[IVSize] {
            0xA5, 0x5C, 0x5A, 0x7B, 0x40, 0xD4, 0x2D, 0x33, 0xA4, 0x6F, 0xF7, 0x84, 0x94, 0x1C, 0x47, 0x85 };
        /// <summary>
        /// The password hash salt.
        /// </summary>
        private const string PasswordSalt = "aEGnwn3CCSFdth7kNXc3";

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
        /// Получить случайный массив байт
        /// </summary>
        public static byte[] GetRandomBytes(int count)
        {
            byte[] randomArr = new byte[count];
            Rng.GetBytes(randomArr);
            return randomArr;
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
        public static string Encrypt(string s, byte[] secretKey, byte[] iv)
        {
            return BytesToHex(EncryptBytes(Encoding.UTF8.GetBytes(s), secretKey, iv));
        }
        
        /// <summary>
        /// Зашифровать строку
        /// </summary>
        public static string Encrypt(string s)
        {
            return Encrypt(s, DefaultSecretKey, DefaultIV);
        }

        /// <summary>
        /// Зашифровать массив байт
        /// </summary>
        public static byte[] EncryptBytes(byte[] bytes, byte[] secretKey, byte[] iv)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = iv };

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateEncryptor(secretKey, iv), CryptoStreamMode.Write))
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
        /// Дешифровать строку, используя ключ по умолчанию
        /// </summary>
        public static string Decrypt(string s, byte[] secretKey, byte[] iv)
        {
            return string.IsNullOrEmpty(s) ? "" : Encoding.UTF8.GetString(DecryptBytes(HexToBytes(s), secretKey, iv));
        }

        /// <summary>
        /// Дешифровать строку, используя ключ по умолчанию
        /// </summary>
        public static string Decrypt(string s)
        {
            return Decrypt(s, DefaultSecretKey, DefaultIV);
        }

        /// <summary>
        /// Дешифровать массив байт
        /// </summary>
        public static byte[] DecryptBytes(byte[] bytes, byte[] secretKey, byte[] iv)
        {
            RijndaelManaged alg = null;

            try
            {
                alg = new RijndaelManaged() { Key = secretKey, IV = iv };

                using (MemoryStream memStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memStream, alg.CreateDecryptor(secretKey, iv), CryptoStreamMode.Read))
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

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public static string GetPasswordHash(int itemKey, string password)
        {
            if (string.IsNullOrEmpty(password))
                return "";

            string hash1 = ComputeHash(password);
            string hash2 = ComputeHash(BitConverter.GetBytes(itemKey));
            return ComputeHash(hash1 + hash2 + PasswordSalt);
        }
    }
}
