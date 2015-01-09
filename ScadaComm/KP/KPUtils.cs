/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : KP
 * Summary  : The utilities for device libraries
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Comm.KP
{
    /// <summary>
    /// The utilities for device libraries
    /// <para>Вспомогательные методы для библиотек КП</para>
    /// </summary>
    public static class KPUtils
    {
        /// <summary>
        /// Время актуальности команды управления, с
        /// </summary>
        private const int CmdLifeTime = 60;
        /// <summary>
        /// Максимальный номер имени файла команды управления
        /// </summary>
        private const int MaxCmdFileNum = 999;

        /// <summary>
        /// Текущий номер имени файла команды управления
        /// </summary>
        private static int cmdFileNum = 1;
        /// <summary>
        /// Объект для синхронизации записи команд управления
        /// </summary>
        private static object cmdLock = new object();


        /// <summary>
        /// Сообщение о невозможности отправки данных
        /// </summary>
        public static readonly string WriteDataImpossible;
        /// <summary>
        /// Сообщение о невозможности отправки строк
        /// </summary>
        public static readonly string WriteLineImpossible;
        /// <summary>
        /// Сообщение о невозможности приёма данных
        /// </summary>
        public static readonly string ReadDataImpossible;
        /// <summary>
        /// Сообщение о невозможности приёма строк
        /// </summary>
        public static readonly string ReadLinesImpossible;
        /// <summary>
        /// Обозначение отправки данных
        /// </summary>
        public static readonly string SendNotation;
        /// <summary>
        /// Обозначение приёма данных
        /// </summary>
        public static readonly string ReceiveNotation;


        /// <summary>
        /// Статический конструктор
        /// </summary>
        static KPUtils()
        {
            // инициализция фраз
            if (Localization.UseRussian)
            {
                WriteDataImpossible = "Отправка данных невозможна, т.к. порт не инициализирован";
                WriteLineImpossible = "Отправка строки невозможна, т.к. порт не инициализирован";
                ReadDataImpossible = "Приём данных невозможен, т.к. порт не инициализирован";
                ReadLinesImpossible = "Приём строки невозможен, т.к. порт не инициализирован";
                SendNotation = "Отправка";
                ReceiveNotation = "Приём";
            }
            else
            {
                WriteDataImpossible = "Sending data is impossible because the port is not initialized";
                WriteLineImpossible = "Sending string is impossible because the port is not initialized";
                ReadDataImpossible = "Receiving data is impossible because the port is not initialized";
                ReadLinesImpossible = "Receiving string is impossible because the port is not initialized";
                SendNotation = "Send";
                ReceiveNotation = "Receive";
            }
        }


        /// <summary>
        /// Преобразовать массив байт в строку 16-ричных чисел
        /// </summary>
        /// <param name="buffer">Массив байт</param>
        /// <param name="index">Начальный индекс в массиве байт</param>
        /// <param name="length">Длина преобразуемых данных</param>
        /// <returns>Строка 16-ричных чисел, разделённых пробелами</returns>
        public static string BytesToHex(byte[] buffer, int index,  int length)
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
        /// Преобразовать массив байт в строку 16-ричных чисел
        /// </summary>
        /// <param name="buffer">Массив байт</param>
        /// <returns>Строка 16-ричных чисел, разделённых пробелами</returns>
        public static string BytesToHex(byte[] buffer)
        {
            return BytesToHex(buffer, 0, buffer.Length);
        }
        
        /// <summary>
        /// Преобразовать массив байт в строку
        /// </summary>
        /// <param name="buffer">Массив байт</param>
        /// <param name="index">Начальный индекс в массиве байт</param>
        /// <param name="length">Длина преобразуемых данных</param>
        /// <param name="skipNonPrinting">Пропустить непечатные символы</param>
        /// <returns>Строка, символы которой закодированы в массиве</returns>
        public static string BytesToString(byte[] buffer, int index, int length, bool skipNonPrinting)
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
        /// Преобразовать массив байт в строку
        /// </summary>
        /// <param name="buffer">Массив байт</param>
        /// <returns>Строка, символы которой закодированы в массиве</returns>
        public static string BytesToString(byte[] buffer)
        {
            return BytesToString(buffer, 0, buffer.Length, true);
        }

        /// <summary>
        /// Добавить при необходимости к числу value нули спереди до достжения длины length
        /// </summary>
        /// <param name="value">Дополняемое число</param>
        /// <param name="length">Длина строки результата</param>
        /// <returns>Строковую запись числа с добавленными в начало нулями</returns>
        /// 
        public static string AddZeros(int value, int length)
        {
            StringBuilder sbResult = new StringBuilder(value.ToString());
            while (sbResult.Length < length)
                sbResult.Insert(0, "0");
            return sbResult.ToString();
        }


        /// <summary>
        /// Записать данные в последовательный порт
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер передаваемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество передаваемых байт</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        public static void WriteToSerialPort(SerialPort serialPort, byte[] buffer, int index, int count, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = WriteDataImpossible;
                }
                else
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Write(buffer, index, count);
                    logText = SendNotation + " (" + count + "): " + KPUtils.BytesToHex(buffer, index, count);
                }
            }
            catch (Exception ex)
            {
                logText = (Localization.UseRussian ? 
                    "Ошибка при отправке данных: " : "Error sending data: ") + ex.Message;
            }
        }

        /// <summary>
        /// Записать строку в последовательный порт
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="text">Записываемая в порт строка</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        public static void WriteLineToSerialPort(SerialPort serialPort, string text, out string logText)
        {
            try
            {
                if (serialPort == null)
                {
                    logText = WriteLineImpossible;
                }
                else
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.WriteLine(text);
                    logText = SendNotation + ": " + text;
                }
            }
            catch (Exception ex)
            {
                logText = (Localization.UseRussian ? 
                    "Ошибка при отправке строки: " : "Error sending string: ") + ex.Message;
            }
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="count">Количество принимаемых байт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int count,
            int timeout, bool wait, out string logText)
        {
            int readCnt = 0;

            if (serialPort == null)
            {
                logText = ReadDataImpossible;
            }
            else
            {
                // данный способ чтения данных необходим для избежания исключения 
                // System.ObjectDisposedException при прерывании потока линии связи
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                serialPort.ReadTimeout = 0;

                while (readCnt < count && startDT <= nowDT && nowDT <= stopDT)
                {
                    try { readCnt += serialPort.Read(buffer, index + readCnt, count - readCnt); }
                    catch { /*The operation has timed out*/ }

                    if (readCnt < count)
                        Thread.Sleep(100); // накопление входных данных в буфере порта

                    nowDT = DateTime.Now;
                }

                logText = ReceiveNotation + " (" + readCnt + "/" + count + "): " + 
                    KPUtils.BytesToHex(buffer, index, readCnt);

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return readCnt;
        }

        /// <summary>
        /// Считать данные из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="buffer">Буфер принимаемых данных</param>
        /// <param name="index">Начальный индекс в буфере</param>
        /// <param name="maxCount">Максимальное количество принимаемых байт</param>
        /// <param name="stopCode">Байт, означающий окончание считывания данных</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Количество считанных байт</returns>
        public static int ReadFromSerialPort(SerialPort serialPort, byte[] buffer, int index, int maxCount, 
            byte stopCode, int timeout, bool wait, out string logText)
        {
            int readCnt = 0;

            if (serialPort == null)
            {
                logText = KPUtils.ReadDataImpossible;
            }
            else
            {
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);

                bool stop = false;
                int curInd = index;
                serialPort.ReadTimeout = 0;

                while (readCnt <= maxCount && !stop && startDT <= nowDT && nowDT <= stopDT)
                {
                    bool readOk;
                    try { readOk = serialPort.Read(buffer, curInd, 1) > 0; }
                    catch { readOk = false; }

                    if (readOk)
                    {
                        stop = buffer[curInd] == stopCode;
                        curInd++;
                        readCnt++;
                    }
                    else
                    {
                        Thread.Sleep(100); // накопление входных данных в буфере порта
                    }

                    nowDT = DateTime.Now;
                }

                logText = ReceiveNotation + " (" + readCnt + "): " + KPUtils.BytesToHex(buffer, index, readCnt);

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return readCnt;
        }

        /// <summary>
        /// Считать строки из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="endLine">Строка, при получении которой завершить приём данных</param>
        /// <param name="endFound">Призкак получения завершающей строки</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Считанные из последовательного порта строки</returns>
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, bool wait, 
            string endLine, out bool endFound, out string logText)
        {
            return ReadLinesFromSerialPort(serialPort, timeout, wait, 
                string.IsNullOrEmpty(endLine) ? null : new string[] {endLine}, out endFound, out logText);
        }
        
        /// <summary>
        /// Считать строки из последовательного порта
        /// </summary>
        /// <param name="serialPort">Последовательный порт</param>
        /// <param name="timeout">Таймаут чтения данных, мс</param>
        /// <param name="wait">Ожидать завершения таймаута после окончания чтения</param>
        /// <param name="endLines">Массив строк, при получении которых завершить приём данных</param>
        /// <param name="endFound">Призкак получения завершающей строки</param>
        /// <param name="logText">Строка для вывода в журнал</param>
        /// <returns>Считанные из последовательного порта строки</returns>
        public static List<string> ReadLinesFromSerialPort(SerialPort serialPort, int timeout, bool wait,
            string[] endLines, out bool endFound, out string logText)
        {
            List<string> inDataList = new List<string>(); // входные данные
            StringBuilder inDataSB = new StringBuilder(); // строковое представление входных данных
            int endLinesLen = endLines == null ? 0 : endLines.Length;
            endFound = false;

            if (serialPort == null)
            {
                logText = ReadLinesImpossible;
            }
            else
            {
                DateTime nowDT = DateTime.Now;
                DateTime startDT = nowDT;
                DateTime stopDT = startDT.AddMilliseconds(timeout);
                serialPort.ReadTimeout = 0;

                while (!endFound && startDT <= nowDT && nowDT <= stopDT)
                {
                    string line;
                    try { line = serialPort.ReadLine().Trim(); }
                    catch { line = ""; /*The operation has timed out*/ }

                    if (line != "")
                    {
                        inDataList.Add(line);
                        inDataSB.AppendLine(line);

                        for (int i = 0; i < endLinesLen && !endFound; i++)
                            endFound = line.EndsWith(endLines[i], StringComparison.OrdinalIgnoreCase);
                    }

                    if (!endFound)
                        Thread.Sleep(100); // накопление входных данных в буфере serialPort

                    nowDT = DateTime.Now;
                }

                logText = ReceiveNotation + ": " + 
                    (inDataList.Count > 0 ? 
                        inDataSB.ToString() : 
                        (Localization.UseRussian ? "нет данных" : "no data"));

                if (wait && startDT <= nowDT)
                {
                    int delay = (int)(stopDT - nowDT).TotalMilliseconds;
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return inDataList;
        }


        /// <summary>
        /// Записать команду управления в файл
        /// </summary>
        /// <param name="cmdDir">Директория команд</param>
        /// <param name="sender">Имя приложения, отправившего команду</param>
        /// <param name="cmdType">Тип команды</param>
        /// <param name="cmdParams">Параметры команды</param>
        /// <param name="msg">Сообщение</param>
        /// <returns>Успешно ли произведена запись команды</returns>
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
                    DateTime nowDT = DateTime.Now;
                    streamWriter.WriteLine("Date=" + nowDT.ToString("dd.MM.yyyy"));
                    streamWriter.WriteLine("Time=" + nowDT.ToString("HH:mm:ss"));
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
        /// Записать команду управления в файл
        /// </summary>
        /// <param name="cmdDir">Директория команд</param>
        /// <param name="sender">Имя приложения, отправившего команду</param>
        /// <param name="cmd">Команда КП</param>
        /// <param name="msg">Сообщение</param>
        /// <returns>Успешно ли произведена запись команды</returns>
        public static bool SaveCmd(string cmdDir, string sender, KPLogic.Command cmd, out string msg)
        {
            StringBuilder sbCmdData = new StringBuilder();
            if (cmd.CmdData != null)
            {
                int cmdDataLen = cmd.CmdData.Length;
                for (int i = 0; i < cmdDataLen; i++)
                    sbCmdData.Append(cmd.CmdData[i].ToString("X2"));
            }

            string[] cmdParams = new string[] {
                "KPNum=" + cmd.KPNum,
                "CmdNum=" + cmd.CmdNum,
                "CmdVal=" + cmd.CmdVal.ToString(),
                "CmdData=" + sbCmdData
            };

            return SaveCmd(cmdDir, sender, cmd.CmdType.ToString(), cmdParams, out msg);
        }
    }
}
