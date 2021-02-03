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
 * Module   : KpModbus
 * Summary  : The class contains utility methods for the Modbus protocol implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2021
 */

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// The class contains utility methods for the Modbus protocol implementation.
    /// <para>Класс, содержащий вспомогательные константы и методы для реализации протокола Modbus.</para>
    /// </summary>
    public static class ModbusUtils
    {
        /// <summary>
        /// Символ начала сообщения в режиме ASCII.
        /// </summary>
        public const string Colon = ":";

        /// <summary>
        /// Окончание сообщения в режиме ASCII.
        /// </summary>
        public const string CRLF = "\x0D\x0A";

        /// <summary>
        /// Номер порта, используемого в режиме TCP по умолчанию.
        /// </summary>
        public const int DefTcpPort = 502;

        #region Таблицы для расчёта CRC-16
        /* Table of CRC values for high–order byte */
        private static readonly byte[] CRCHiTable = new byte[] {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40
        };

        /* Table of CRC values for low–order byte */
        private static readonly byte[] CRCLoTable = new byte[] {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
            0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
            0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
            0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
            0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
            0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
            0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
            0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
            0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
            0x40
        };
        #endregion


        /// <summary>
        /// Рассчитать CRC-16.
        /// </summary>
        /// <remarks>Метод взят из официального описания протокола Modbus.</remarks>
        public static ushort CalcCRC16(byte[] buffer, int offset, int length)
        {
            byte crcHi = 0xFF;   // high byte of CRC initialized
            byte crcLo = 0xFF;   // low byte of CRC initialized
            int index;           // will index into CRC lookup table

            while (length-- > 0) // pass through message buffer
            {
                index = crcLo ^ buffer[offset++]; // calculate the CRC
                crcLo = (byte)(crcHi ^ CRCHiTable[index]);
                crcHi = CRCLoTable[index];
            }

            return (ushort)((crcHi << 8) | crcLo);
        }

        /// <summary>
        /// Рассчитать LRC.
        /// </summary>
        /// <remarks>Метод взят из официального описания протокола Modbus.</remarks>
        public static byte CalcLRC(byte[] buffer, int offset, int length)
        {
            byte uchLRC = 0;                // LRC char initialized
            while (length-- > 0)            // pass through message buffer
                uchLRC += buffer[offset++]; // add buffer byte without carry
            return (byte)(-(sbyte)uchLRC);  // return twos complement
        }

        /// <summary>
        /// Получить наименование типа таблицы данных.
        /// </summary>
        public static string GetTableTypeName(TableType tableType)
        {
            switch (tableType)
            {
                case TableType.DiscreteInputs:
                    return "Discrete Inputs";
                case TableType.Coils:
                    return "Coils";
                case TableType.InputRegisters:
                    return "Input Registers";
                default: // TableTypes.HoldingRegisters
                    return "Holding Registers";
            }
        }

        /// <summary>
        /// Получить описание исключения, полученного от устройства.
        /// </summary>
        public static string GetExcDescr(byte excCode)
        {
            string descr = "[" + excCode.ToString("X2") + "] ";

            switch (excCode)
            {
                case 0x01:
                    descr += "ILLEGAL FUNCTION";
                    break;
                case 0x02:
                    descr += "ILLEGAL DATA ADDRESS";
                    break;
                case 0x03:
                    descr += "ILLEGAL DATA VALUE";
                    break;
                case 0x04:
                    descr += "SLAVE DEVICE FAILURE";
                    break;
                case 0x05:
                    descr += "ACKNOWLEDGE";
                    break;
                case 0x06:
                    descr += "SLAVE DEVICE BUSY";
                    break;
                case 0x08:
                    descr += "MEMORY PARITY ERROR";
                    break;
                case 0x0A:
                    descr += "GATEWAY PATH UNAVAILABLE";
                    break;
                case 0x0B:
                    descr += "GATEWAY TARGET DEVICE FAILED TO RESPOND";
                    break;
                default:
                    descr += "Unknown";
                    break;
            }

            return descr;
        }

        /// <summary>
        /// Получить строковую запись диапазона адресов элемента.
        /// </summary>
        public static string GetAddressRange(int address, int count, bool zeroAddr, bool decAddr)
        {
            if (!zeroAddr)
                address++;

            string format = decAddr ? "G" : "X";
            string suffix = decAddr ? "" : "H";
            return address.ToString(format) + suffix + 
                (count <= 1 ? "" : " - " + (address + count - 1).ToString(format) + suffix);
        }
        
        /// <summary>
        /// Разобрать массив, определяющий порядок байт, из строковой записи вида '01234567'.
        /// </summary>
        public static int[] ParseByteOrder(string byteOrderStr)
        {
            if (string.IsNullOrEmpty(byteOrderStr))
            {
                return null;
            }
            else
            {
                int len = byteOrderStr.Length;
                int[] byteOrder = new int[len];

                for (int i = 0; i < len; i++)
                {
                    byteOrder[i] = int.TryParse(byteOrderStr[i].ToString(), out int n) ? n : 0;
                }

                return byteOrder;
            }
        }

        /// <summary>
        /// Копировать элементы массива с заданным порядоком байт.
        /// </summary>
        public static void ApplyByteOrder(byte[] src, int srcOffset, byte[] dest, int destOffset, int count, 
            int[] byteOrder, bool reverse)
        {
            int srcLen = src == null ? 0 : src.Length;
            int endSrcInd = srcOffset + count - 1;
            int ordLen = byteOrder == null ? 0 : byteOrder.Length;

            if (byteOrder == null)
            {
                // копирование данных без учёта порядка байт
                for (int i = 0; i < count; i++)
                {
                    int srcInd = reverse ? endSrcInd - i : srcOffset + i;
                    dest[destOffset++] = 0 <= srcInd && srcInd < srcLen ? src[srcInd] : (byte)0;
                }
            }
            else
            {
                // копирование данных с учётом порядка байт
                for (int i = 0; i < count; i++)
                {
                    int srcInd = i < ordLen ? (reverse ? endSrcInd - byteOrder[i] : srcOffset + byteOrder[i]) : -1;
                    dest[destOffset++] = 0 <= srcInd && srcInd < srcLen ? src[srcInd] : (byte)0;
                }
            }
        }

        /// <summary>
        /// Копировать элементы массива с заданным порядоком байт.
        /// </summary>
        public static void ApplyByteOrder(byte[] src, byte[] dest, int[] byteOrder)
        {
            ApplyByteOrder(src, 0, dest, 0, dest.Length, byteOrder, false);
        }

        /// <summary>
        /// Gets the required quantity of addresses depending on the element type.
        /// </summary>
        public static int GetQuantity(ElemType elemType)
        {
            switch (elemType)
            {
                case ElemType.ULong:
                case ElemType.Long:
                case ElemType.Double:
                    return 4;
                case ElemType.UInt:
                case ElemType.Int:
                case ElemType.Float:
                    return 2;
                default: // Undefined, Bool
                    return 1;
            }
        }

        /// <summary>
        /// Gets the length required to store element data depending on the element type.
        /// </summary>
        public static int GetDataLength(ElemType elemType)
        {
            return elemType == ElemType.Bool ? 1 : GetQuantity(elemType) * 2;
        }

        /// <summary>
        /// Determines whether the specified function is a read function.
        /// </summary>
        public static bool IsReadFunction(byte funcCode)
        {
            return funcCode <= 0x04;
        }
    }
}
