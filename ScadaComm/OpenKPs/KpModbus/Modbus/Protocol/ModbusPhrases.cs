/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Summary  : Phrases used in Modbus implementation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Phrases used in Modbus implementation
    /// <para>Фразы, используемые при реализации Modbus</para>
    /// </summary>
    public static class ModbusPhrases
    {
        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ModbusPhrases()
        {
            if (Localization.UseRussian)
            {
                ConnectionRequired = "Невозможно выполнить запрос, т.к. соединение не установлено";
                IncorrectPduLength = "Некорректная длина PDU";
                IncorrectPduFuncCode = "Некорректный код функции PDU";
                IncorrectPduData = "Некорректные данные PDU";
                Request = "Запрос значений группы элементов{0}";
                Command = "Команда{0}";
                DeviceError = "Ошибка устройства";
                IllegalDataTable = "Недопустимая таблица данных для команды.";
                LoadTemplateError = "Ошибка при загрузке шаблона устройства";
                SaveTemplateError = "Ошибка при сохранении шаблона устройства";
                ClearNetStreamError = "Ошибка при очистке сетевого потока";
                OK = "OK!";
                CrcError = "Ошибка CRC!";
                LrcError = "Ошибка LRC!";
                CommErrorWithExclamation = "Ошибка связи!";
                IncorrectDevAddr = "Некорректный адрес устройства!";
                IncorrectSymbol = "Некорректный символ!";
                IncorrectAduLength = "Некорректная длина ADU!";
                IncorrectMbap = "Некорректные данные MBAP Header!";
            }
            else
            {
                ConnectionRequired = "Unable to request because connection is not established";
                IncorrectPduLength = "Incorrect PDU length";
                IncorrectPduFuncCode = "Incorrect PDU function code";
                IncorrectPduData = "Incorrect PDU data";
                Request = "Request element group{0}";
                Command = "Command{0}";
                DeviceError = "Device error";
                IllegalDataTable = "Illegal data table for the command.";
                LoadTemplateError = "Error loading device template";
                SaveTemplateError = "Error saving device template";
                ClearNetStreamError = "Error clear network stream";
                OK = "OK!";
                CrcError = "CRC error!";
                LrcError = "LRC error!";
                CommErrorWithExclamation = "Communication error!";
                IncorrectDevAddr = "Incorrect device address!";
                IncorrectSymbol = "Incorrect symbol!";
                IncorrectAduLength = "Incorrect ADU length!";
                IncorrectMbap = "Incorrect MBAP Header data!";
            }
        }

        public static string ConnectionRequired { get; set; }
        public static string IncorrectPduLength { get; set; }
        public static string IncorrectPduFuncCode { get; set; }
        public static string IncorrectPduData { get; set; }
        public static string Request { get; set; }
        public static string Command { get; set; }
        public static string DeviceError { get; set; }
        public static string IllegalDataTable { get; set; }
        public static string LoadTemplateError { get; set; }
        public static string SaveTemplateError { get; set; }
        public static string ClearNetStreamError { get; set; }
        public static string OK { get; set; }
        public static string CrcError { get; set; }
        public static string LrcError { get; set; }
        public static string CommErrorWithExclamation { get; set; }
        public static string IncorrectDevAddr { get; set; }
        public static string IncorrectSymbol { get; set; }
        public static string IncorrectAduLength { get; set; }
        public static string IncorrectMbap { get; set; }
    }
}
