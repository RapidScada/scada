/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : The class contains utility methods for the whole system
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2020
 */

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Scada
{
    /// <summary>
    /// The class contains utility methods for the whole system.
    /// <para>Класс, содержащий вспомогательные методы для всей системы.</para>
    /// </summary>
    public static partial class ScadaUtils
    {
        /// <summary>
        /// Версия данной библиотеки
        /// </summary>
        internal const string LibVersion = "5.1.5.0";
        /// <summary>
        /// Формат вещественных чисел с разделителем точкой
        /// </summary>
        private static readonly NumberFormatInfo NfiDot;
        /// <summary>
        /// Формат вещественных чисел с разделителем запятой
        /// </summary>
        private static readonly NumberFormatInfo NfiComma;

        /// <summary>
        /// Задержка потока для экономии ресурсов, мс
        /// </summary>
        public const int ThreadDelay = 100;
        /// <summary>
        /// Начало отчёта времени, которое используется приложениями Rapid SCADA
        /// </summary>
        /// <remarks>Совпадает с началом отсчёта времени в OLE Automation и Delphi</remarks>
        public static readonly DateTime ScadaEpoch = new DateTime(1899, 12, 30, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// Determines that the application is running on Windows.
        /// </summary>
        public static readonly bool IsRunningOnWin = IsWindows(Environment.OSVersion);
        /// <summary>
        /// Determines that the application is running on Mono Framework.
        /// </summary>
        public static readonly bool IsRunningOnMono = Type.GetType("Mono.Runtime") != null;


        /// <summary>
        /// Конструктор
        /// </summary>
        static ScadaUtils()
        {
            NfiDot = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            NfiComma = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            NfiComma.NumberDecimalSeparator = ",";
        }


        /// <summary>
        /// Удалить пробелы из строки
        /// </summary>
        private static string RemoveWhiteSpace(string s)
        {
            StringBuilder sb = new StringBuilder();

            if (s != null)
            {
                foreach (char c in s)
                {
                    if (!char.IsWhiteSpace(c))
                        sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Check whether the application is running on Windows.
        /// </summary>
        private static bool IsWindows(OperatingSystem os)
        {
            // since .NET 4.7.1 change to RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            PlatformID pid = os.Platform;
            return pid == PlatformID.Win32NT || pid == PlatformID.Win32S ||
                pid == PlatformID.Win32Windows || pid == PlatformID.WinCE;
        }


        /// <summary>
        /// Добавить слэш к имени директории, если необходимо
        /// </summary>
        public static string NormalDir(string dir)
        {
            dir = dir == null ? "" : dir.Trim();
            int lastIndex = dir.Length - 1;

            if (lastIndex >= 0 && !(dir[lastIndex] == Path.DirectorySeparatorChar ||
                dir[lastIndex] == Path.AltDirectorySeparatorChar))
            {
                dir += Path.DirectorySeparatorChar;
            }

            return dir;
        }

        /// <summary>
        /// Скорректировать разделитель директории
        /// </summary>
        public static string CorrectDirectorySeparator(string path)
        {
            // Path.AltDirectorySeparatorChar == '/' для Mono на Linux, что некорректно 
            return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Закодировать дату и время в вещественное значение времени
        /// </summary>
        /// <remarks>Совместим с методом DateTime.ToOADate()</remarks>
        public static double EncodeDateTime(DateTime dateTime)
        {
            return (dateTime - ScadaEpoch).TotalDays;
        }

        /// <summary>
        /// Декодировать вещественное значение времени, преобразовав его в формат DateTime
        /// </summary>
        /// <remarks>Совместим с методом DateTime.FromOADate()</remarks>
        public static DateTime DecodeDateTime(double dateTime)
        {
            return ScadaEpoch.AddDays(dateTime);
        }

        /// <summary>
        /// Комбинировать заданные дату и время в единое значение
        /// </summary>
        public static DateTime CombineDateTime(DateTime date, double time)
        {
            return date.AddDays(time - Math.Truncate(time));
        }

        /// <summary>
        /// Закодировать первые 8 символов строки ASCII в вещественное число
        /// </summary>
        public static double EncodeAscii(string s)
        {
            byte[] buf = new byte[8];
            int len = Math.Min(8, s.Length);
            Encoding.ASCII.GetBytes(s, 0, len, buf, 0);
            return BitConverter.ToDouble(buf, 0);
        }

        /// <summary>
        /// Декодировать вещественное число, преобразовав его в строку ASCII
        /// </summary>
        public static string DecodeAscii(double val)
        {
            byte[] buf = BitConverter.GetBytes(val);
            return Encoding.ASCII.GetString(buf).TrimEnd((char)0);
        }

        /// <summary>
        /// Закодировать первые 4 символа строки Unicode в вещественное число
        /// </summary>
        public static double EncodeUnicode(string s)
        {
            byte[] buf = new byte[8];
            int len = Math.Min(4, s.Length);
            Encoding.Unicode.GetBytes(s, 0, len, buf, 0);
            return BitConverter.ToDouble(buf, 0);
        }

        /// <summary>
        /// Декодировать вещественное число, преобразовав его в строку Unicode
        /// </summary>
        public static string DecodeUnicode(double val)
        {
            byte[] buf = BitConverter.GetBytes(val);
            return Encoding.Unicode.GetString(buf).TrimEnd((char)0);
        }

        /// <summary>
        /// Определить, является ли заданная строка записью даты, используя Localization.Culture
        /// </summary>
        public static bool StrIsDate(string s)
        {
            DateTime dateTime;
            return DateTime.TryParse(s, Localization.Culture, DateTimeStyles.None, out dateTime) ?
                dateTime.TimeOfDay.TotalMilliseconds == 0 : false;
        }

        /// <summary>
        /// Преобразовать строку в дату, используя Localization.Culture
        /// </summary>
        public static DateTime StrToDate(string s)
        {
            DateTime dateTime;
            if (DateTime.TryParse(s, Localization.Culture, DateTimeStyles.None, out dateTime))
                return dateTime.Date;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Попытаться преобразовать строку в дату и время, используя Localization.Culture
        /// </summary>
        public static bool TryParseDateTime(string s, out DateTime result)
        {
            return DateTime.TryParse(s, Localization.Culture, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// Преобразовать строку в вещественное число
        /// </summary>
        /// <remarks>Метод работает с разделителями целой части '.' и ','.
        /// Если преобразование невозможно, возвращается double.NaN</remarks>
        public static double StrToDouble(string s)
        {
            try { return ParseDouble(s); }
            catch { return double.NaN; }
        }

        /// <summary>
        /// Преобразовать строку в вещественное число
        /// </summary>
        /// <remarks>Метод работает с разделителями целой части '.' и ','.
        /// Если преобразование невозможно, вызывается исключение FormatException</remarks>
        public static double StrToDoubleExc(string s)
        {
            try { return ParseDouble(s); }
            catch { throw new FormatException(string.Format(CommonPhrases.NotNumber, s)); }
        }

        /// <summary>
        /// Преобразовать строку в вещественное число
        /// </summary>
        /// <remarks>Метод работает с разделителями целой части '.' и ','</remarks>
        public static double ParseDouble(string s)
        {
            return double.Parse(s, s.Contains(".") ? NfiDot : NfiComma);
        }

        /// <summary>
        /// Попытаться преобразовать строку в вещественное число
        /// </summary>
        /// <remarks>Метод работает с разделителями целой части '.' и ','</remarks>
        public static bool TryParseDouble(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Float, s.Contains(".") ? NfiDot : NfiComma, out result);
        }

        /// <summary>
        /// Преобразовать массив байт в строку на основе 16-ричного представления
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            return BytesToHex(bytes, 0, bytes == null ? 0 : bytes.Length);
        }

        /// <summary>
        /// Преобразовать заданный диапазон массива байт в строку на основе 16-ричного представления
        /// </summary>
        public static string BytesToHex(byte[] bytes, int index, int count)
        {
            StringBuilder sb = new StringBuilder();
            int last = index + count;
            for (int i = index; i < last; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }
        
        /// <summary>
        /// Преобразовать строку 16-ричных чисел в массив байт, используя существующий массив
        /// </summary>
        public static bool HexToBytes(string s, int strIndex, byte[] buf, int bufIndex, int byteCount)
        {
            int strLen = s == null ? 0 : s.Length;
            int convBytes = 0;

            while (strIndex < strLen && convBytes < byteCount)
            {
                try
                {
                    buf[bufIndex] = byte.Parse(s.Substring(strIndex, 2), NumberStyles.AllowHexSpecifier);
                    bufIndex++;
                    convBytes++;
                    strIndex += 2;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return convBytes > 0;
        }

        /// <summary>
        /// Преобразовать строку 16-ричных чисел в массив байт, создав новый массив
        /// </summary>
        public static bool HexToBytes(string s, out byte[] bytes, bool skipWhiteSpace = false)
        {
            if (skipWhiteSpace)
                s = RemoveWhiteSpace(s);

            int strLen = s == null ? 0 : s.Length;
            int bufLen = strLen / 2;
            bytes = new byte[bufLen];
            return HexToBytes(s, 0, bytes, 0, bufLen);
        }

        /// <summary>
        /// Преобразовать строку 16-ричных чисел в массив байт
        /// </summary>
        public static byte[] HexToBytes(string s, bool skipWhiteSpace = false)
        {
            if (HexToBytes(s, out byte[] bytes, skipWhiteSpace))
                return bytes;
            else
                throw new FormatException(CommonPhrases.NotHexadecimal);
        }

        /// <summary>
        /// Глубокое (полное) клонирование объекта
        /// </summary>
        /// <remarks>Все клонируемые объекты должны иметь атрибут Serializable</remarks>
        public static object DeepClone(object obj, SerializationBinder binder = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (binder != null)
                    bf.Binder = binder;
                bf.Serialize(ms, obj);

                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }
        
        /// <summary>
        /// Глубокое (полное) клонирование объекта
        /// </summary>
        public static T DeepClone<T>(T obj, SerializationBinder binder = null)
        {
            return (T)DeepClone((object)obj, binder);
        }

        /// <summary>
        /// Скорректировать имя типа для работы DeepClone
        /// </summary>
        public static void CorrectTypeName(ref string typeName)
        {
            if (typeName.Contains("System.Collections.Generic.List"))
            {
                // удаление информации о сборке
                int ind1 = typeName.IndexOf(",");
                int ind2 = typeName.IndexOf("]");
                if (ind1 < ind2)
                    typeName = typeName.Remove(ind1, ind2 - ind1);
            }
        }

        /// <summary>
        /// Получить время последней записи в файл
        /// </summary>
        public static DateTime GetLastWriteTime(string fileName)
        {
            try
            {
                return File.Exists(fileName) ? File.GetLastWriteTime(fileName) : DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Identifies a nullable type.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Indicates whether the string is correct URL.
        /// </summary>
        public static bool IsValidUrl(string s)
        {
            return !string.IsNullOrEmpty(s) && 
                Uri.TryCreate(s, UriKind.Absolute, out Uri uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Determines whether two arrays are equal.
        /// </summary>
        public static bool ArraysEqual<T>(T[] a, T[] b)
        {
            if (a == b)
                return true;
            else if (a == null || b == null)
                return false;
            else
                return Enumerable.SequenceEqual(a, b);
        }
    }
}