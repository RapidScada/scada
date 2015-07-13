/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : The phrases used by SCADA-Communicator and its libraries
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Comm
{
    /// <summary>
    /// The phrases used by SCADA-Communicator and its libraries
    /// <para>Фразы, используемые SCADA-Коммуникатором и его библиотеками</para>
    /// </summary>
    public static class CommPhrases
    {
        static CommPhrases()
        {
            SetToDefault();
            InitOnLocalization();
        }

        // Словарь Scada.Comm.Settings
        public static string LineCaption { get; private set; }
        public static string KPCaption { get; private set; }
        public static string IncorrectKPSettings { get; private set; }
        public static string IncorrectLineSettings { get; private set; }

        // Фразы, устанавливаемые в зависимости от локализации, не загружая из словаря
        public static string SendNotation;
        public static string ReceiveNotation;
        public static string ReadDataError;
        public static string ReadDataWithStopCondError;
        public static string ReadLinesError;
        public static string ReadAvailableError;
        public static string WriteDataError;
        public static string WriteLineError;
        public static string ClearDataStreamError;
        public static string IllegalCommand { get; private set; }
        public static string IncorrectCmdData { get; private set; }
        public static string NoCmdData { get; private set; }


        private static void SetToDefault()
        {
            LineCaption = "Линия";
            KPCaption = "КП";
            IncorrectKPSettings = "Некорректные параметры КП {0}";
            IncorrectLineSettings = "Некорректная конфигурация линии связи {0}";
        }

        private static void InitOnLocalization()
        {
            if (Localization.UseRussian)
            {
                SendNotation = "Отправка";
                ReceiveNotation = "Приём";
                ReadDataError = "Ошибка при считывании данных";
                ReadDataWithStopCondError = "Ошибка при считывании данных с условием остановки";
                ReadLinesError = "Ошибка при считывании строк";
                ReadAvailableError = "Ошибка при считывании доступных данных";
                WriteDataError = "Ошибка при записи данных";
                WriteLineError = "Ошибка при записи строки";
                ClearDataStreamError = "Ошибка при очистке потока данных";
                IllegalCommand = "Недопустимая команда";
                IncorrectCmdData = "Некорректные данные команды";
                NoCmdData = "Отсутствуют данные команды";
            }
            else
            {
                SendNotation = "Send";
                ReceiveNotation = "Receive";
                ReadDataError = "Error reading data";
                ReadDataWithStopCondError = "Error reading data with stop condition";
                ReadLinesError = "Error reading lines";
                ReadAvailableError = "Error reading available data";
                WriteDataError = "Error writing data";
                WriteLineError = "Error writing line";
                ClearDataStreamError = "Error clearing data stream";
                IllegalCommand = "Illegal command";
                IncorrectCmdData = "Incorrect command data";
                NoCmdData = "No command data";
            }
        }

        public static void InitFromDictionaries()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Settings", out dict))
            {
                LineCaption = dict.GetPhrase("LineCaption", LineCaption);
                KPCaption = dict.GetPhrase("KPCaption", KPCaption);
                IncorrectKPSettings = dict.GetPhrase("IncorrectKPSettings", IncorrectKPSettings);
                IncorrectLineSettings = dict.GetPhrase("IncorrectLineSettings", IncorrectLineSettings);
            }
        }
    }
}
