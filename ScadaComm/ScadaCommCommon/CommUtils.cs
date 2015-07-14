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
 * Summary  : The class contains utility methods for SCADA-Communicator and its libraries
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Comm.Devices;
using Scada.Data;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Scada.Comm
{
    /// <summary>
    /// The class contains utility methods for SCADA-Communicator and its libraries
    /// <para>Класс, содержащий вспомогательные методы для SCADA-Коммуникатора и его библиотек</para>
    /// </summary>
    public static class CommUtils
    {
        /// <summary>
        /// Форматы вывода протокола обмена данными в журнал
        /// </summary>
        public enum ProtocolLogFormats
        {
            /// <summary>
            /// 16-ричный формат
            /// </summary>
            Hex,
            /// <summary>
            /// Строковый формат
            /// </summary>
            String
        }

        /// <summary>
        /// Формат даты и времени для вывода в журнал линии связи
        /// </summary>
        public const string CommLineDTFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        
        /// <summary>
        /// Время актуальности команды управления, с
        /// </summary>
        private const int CmdLifeTime = 60;
        /// <summary>
        /// Максимальный номер имени файла команды управления
        /// </summary>
        private const int MaxCmdFileNum = 999;

        private static int cmdFileNum = 1;            // текущий номер имени файла команды управления
        private static object cmdLock = new object(); // объект для синхронизации записи команд управления


        /// <summary>
        /// Преобразовать массив байт в строку 16-ричных чисел, разделённых пробелами
        /// </summary>
        public static string BytesToHex(byte[] buffer, int index, int length)
        {
            StringBuilder sbResult = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                    sbResult.Append(" ");
                sbResult.Append(buffer[index + i].ToString("X2"));
            }

            return sbResult.ToString();
        }

        /// <summary>
        /// Преобразовать массив байт в строку 16-ричных чисел, разделённых пробелами
        /// </summary>
        public static string BytesToHex(byte[] buffer)
        {
            return BytesToHex(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Преобразовать массив байт в текстовую строку
        /// </summary>
        public static string BytesToString(byte[] buffer, int index, int length, bool skipNonPrinting = false)
        {
            StringBuilder sbResult = new StringBuilder();
            bool notSkip = !skipNonPrinting;
            int n = index + length;

            for (int i = index; i < n; i++)
            {
                byte b = buffer[i];

                if (b >= 32)
                {
                    sbResult.Append(Encoding.Default.GetString(buffer, i, 1));
                }
                else if (notSkip)
                {
                    sbResult.Append("<");
                    sbResult.Append(b.ToString("X2"));
                    sbResult.Append(">");
                }
            }

            return sbResult.ToString();
        }

        /// <summary>
        /// Преобразовать массив байт в текстовую строку
        /// </summary>
        public static string BytesToString(byte[] buffer)
        {
            return BytesToString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Добавить к числу value спереди нули до достжения длины length
        /// </summary>
        public static string AddZeros(int value, int length)
        {
            StringBuilder sbResult = new StringBuilder(value.ToString());
            while (sbResult.Length < length)
                sbResult.Insert(0, "0");
            return sbResult.ToString();
        }

        /// <summary>
        /// Получить текущую дату и время для вывода в журнал линии связи
        /// </summary>
        public static string GetNowDT()
        {
            return DateTime.Now.ToString(CommLineDTFormat);
        }


        /// <summary>
        /// Записать команду службе SCADA-Коммуникатора в файл
        /// </summary>
        public static bool SaveCmd(string cmdDir, string sender, string cmdType, string[] cmdParams, out string msg)
        {
            try
            {
                if (cmdDir == "")
                {
                    msg = Localization.UseRussian ?
                        "Невозможно записать команду, т.к. директория команд неопределена." :
                        "Unable to write a command because the command directory is undefined.";
                    return false;
                }

                // формирование имени файла команды
                string fileName = ""; // имя файла команды

                lock (cmdLock)
                {
                    int num = cmdFileNum;

                    do
                    {
                        string name = cmdDir + "cmd" + AddZeros(num, 3) + ".dat";
                        if (!File.Exists(name))
                            fileName = name;
                        else if (++num > MaxCmdFileNum)
                            num = 1;
                    } while (fileName == "" && num != cmdFileNum);

                    cmdFileNum = num < MaxCmdFileNum ? num + 1 : 1;
                }

                if (fileName == "")
                {
                    msg = Localization.UseRussian ?
                        "Невозможно записать команду, т.к. отсутствуют доступные имена файлов команд." :
                        "Unable to write a command because available command file names are missing.";
                    return false;
                }

                // запись команды
                FileStream fileStream = null;
                StreamWriter streamWriter = null;

                try
                {
                    fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
                    streamWriter = new StreamWriter(fileStream, Encoding.Default);

                    streamWriter.WriteLine("[Command]");
                    streamWriter.WriteLine("Target=ScadaCommSvc");
                    streamWriter.WriteLine("Sender=" + sender);
                    streamWriter.WriteLine("User=" + Environment.UserName);
                    streamWriter.WriteLine("DateTime=" + DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo));
                    streamWriter.WriteLine("LifeTime=" + CmdLifeTime);
                    streamWriter.WriteLine("CmdType=" + cmdType);
                    foreach (string param in cmdParams)
                        streamWriter.WriteLine(param);
                    streamWriter.WriteLine("End=");
                }
                finally
                {
                    if (streamWriter != null)
                        streamWriter.Close();
                    if (fileStream != null)
                        fileStream.Close();
                }

                msg = Localization.UseRussian ?
                    "Команда отправлена успешно." : "The command has been sent successfully";
                return true;
            }
            catch (Exception ex)
            {
                msg = (Localization.UseRussian ?
                    "Ошибка при записи команды: " : "Error saving command: ") + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Записать команду КП в файл
        /// </summary>
        public static bool SaveCmd(string cmdDir, string sender, Command cmd, out string msg)
        {
            string[] cmdParams = new string[] {
                "KPNum=" + cmd.KPNum,
                "CmdNum=" + cmd.CmdNum,
                "CmdVal=" + cmd.CmdVal.ToString(),
                "CmdData=" + cmd.CmdData == null ? "" : ScadaUtils.BytesToHex(cmd.CmdData)
            };

            return SaveCmd(cmdDir, sender, BaseValues.CmdTypes.GetCmdTypeCode(cmd.CmdTypeID), cmdParams, out msg);
        }
    }
}
