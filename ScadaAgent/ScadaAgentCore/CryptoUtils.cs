using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Scada.Agent
{
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
    }
}
