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
 * Summary  : Implements communication with SCADA-Server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

#undef DETAILED_LOG // enable output the detailed information to the log

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// Implements communication with SCADA-Server.
    /// <para>Реализует обмен данными со SCADA-Сервером.</para>
    /// </summary>
    public class ServerComm
    {
        /// <summary>
        /// Состояния обмена данными со SCADA-Сервером
        /// </summary>
        public enum CommStates
        {
            /// <summary>
            /// Соединение не установлено
            /// </summary>
            Disconnected,
            /// <summary>
            /// Соединение установлено
            /// </summary>
            Connected,
            /// <summary>
            /// Соединение установлено и программа авторизована
            /// </summary>
            Authorized,
            /// <summary>
            /// SCADA-Сервер не готов
            /// </summary>
            NotReady,
            /// <summary>
            /// Ошибка обмена данными
            /// </summary>
            Error,
            /// <summary>
            /// Ожидание ответа на команду или запрос
            /// </summary>
            WaitResponse
        }

        /// <summary>
        /// Директории на SCADA-Сервере
        /// </summary>
        public enum Dirs
        {
            /// <summary>
            /// Директория текущего среза
            /// </summary>
            Cur = 0x01,
            /// <summary>
            /// Директория часовых срезов
            /// </summary>
            Hour = 0x02,
            /// <summary>
            /// Директория минутных срезов
            /// </summary>
            Min = 0x03,
            /// <summary>
            /// Директория событий
            /// </summary>
            Events = 0x04,
            /// <summary>
            /// Директория базы конфигурации в формате DAT
            /// </summary>
            BaseDAT = 0x05,
            /// <summary>
            /// Директория интерфейса (таблиц, схем и т.п.)
            /// </summary>
            Itf = 0x06
        }


        /// <summary>
        /// Таймаут отправки данных по TCP, мс
        /// </summary>
        protected const int TcpSendTimeout = 1000;
        /// <summary>
        /// Таймаут приёма данных по TCP, мс
        /// </summary>
        protected const int TcpReceiveTimeout = 5000;
        /// <summary>
        /// Интервал повторных попыток соединения
        /// </summary>
        protected readonly TimeSpan ConnectSpan = TimeSpan.FromSeconds(10);
        /// <summary>
        /// Интервал проверки соединения путём запроса состояния сервера
        /// </summary>
        protected readonly TimeSpan PingSpan = TimeSpan.FromSeconds(30);


        /// <summary>
        /// Настройки соединения со SCADA-Сервером
        /// </summary>
        protected CommSettings commSettings;
        /// <summary>
        /// Журнал работы
        /// </summary>
        protected ILog log;
        /// <summary>
        /// Метод записи в журнал работы
        /// </summary>
        protected Log.WriteLineDelegate writeToLog { get; set; }
        /// <summary>
        /// TCP-клиент для обмена данными со SCADA-Сервером
        /// </summary>
        protected TcpClient tcpClient;
        /// <summary>
        /// Поток данных TCP-клиента
        /// </summary>
        protected NetworkStream netStream;
        /// <summary>
        /// Объект для синхронизации обмена данными со SCADA-Сервером
        /// </summary>
        protected object tcpLock;
        /// <summary>
        /// Состояние обмена данными со SCADA-Сервером
        /// </summary>
        protected CommStates commState;
        /// <summary>
        /// Версия SCADA-Сервера
        /// </summary>
        protected string serverVersion;
        /// <summary>
        /// Время успешного вызова метода восстановления соединения
        /// </summary>
        protected DateTime restConnSuccDT;
        /// <summary>
        /// Время неудачного вызова метода восстановления соединения
        /// </summary>
        protected DateTime restConnErrDT;
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        protected string errMsg;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ServerComm()
        {
            tcpClient = null;
            netStream = null;
            tcpLock = new object();
            commState = CommStates.Disconnected;
            serverVersion = "";
            restConnSuccDT = DateTime.MinValue;
            restConnErrDT = DateTime.MinValue;
            errMsg = "";
        }

        /// <summary>
        /// Конструктор с установкой настроек соединения со SCADA-Сервером и журнала работы
        /// </summary>
        public ServerComm(CommSettings commSettings, ILog log)
            : this()
        {
            this.commSettings = commSettings;
            this.log = log;
            this.writeToLog = null;
        }

        /// <summary>
        /// Конструктор с установкой настроек соединения со SCADA-Сервером и метода записи в журнал работы
        /// </summary>
        public ServerComm(CommSettings commSettings, Log.WriteLineDelegate writeToLog)
            : this()
        {
            this.commSettings = commSettings;
            this.log = null;
            this.writeToLog = writeToLog;
        }


        /// <summary>
        /// Получить настройки соединения со SCADA-Сервером
        /// </summary>
        public CommSettings CommSettings
        {
            get
            {
                return commSettings;
            }
        }

        /// <summary>
        /// Получить состояние обмена данными со SCADA-Сервером
        /// </summary>
        public CommStates CommState
        {
            get
            {
                return commState;
            }
        }

        /// <summary>
        /// Получить описание состояния обмена данными со SCADA-Сервером
        /// </summary>
        public string CommStateDescr
        {
            get
            {
                StringBuilder stateDescr = new StringBuilder();
                if (serverVersion != "")
                    stateDescr.Append(Localization.UseRussian ? "версия " : "version ").
                        Append(serverVersion).Append(", ");

                switch (commState)
                {
                    case CommStates.Disconnected:
                        stateDescr.Append(Localization.UseRussian ? 
                            "соединение не установлено" : 
                            "not connected");
                        break;
                    case CommStates.Connected:
                        stateDescr.Append(Localization.UseRussian ? 
                            "соединение установлено" : 
                            "connected");
                        break;
                    case CommStates.Authorized:
                        stateDescr.Append(Localization.UseRussian ? 
                            "авторизация успешна" : 
                            "authentication is successful");
                        break;
                    case CommStates.NotReady:
                        stateDescr.Append(Localization.UseRussian ? 
                            "SCADA-Сервер не готов" : 
                            "SCADA-Server isn't ready");
                        break;
                    case CommStates.Error:
                        stateDescr.Append(Localization.UseRussian ? 
                            "ошибка обмена данными" : 
                            "communication error");
                        break;
                    case CommStates.WaitResponse:
                        stateDescr.Append(Localization.UseRussian ? 
                            "ожидание ответа" : 
                            "waiting for response");
                        break;
                }

                if (errMsg != "")
                    stateDescr.Append(" - ").Append(errMsg);

                return stateDescr.ToString();
            }
        }

        /// <summary>
        /// Получить сообщение об ошибке
        /// </summary>
        public string ErrMsg
        {
            get
            {
                return errMsg;
            }
        }


        /// <summary>
        /// Проверить формат данных для заданной команды при связи со SCADA-Сервером
        /// </summary>
        protected bool CheckDataFormat(byte[] buffer, int cmdNum)
        {
            return CheckDataFormat(buffer, cmdNum, buffer.Length);
        }

        /// <summary>
        /// Проверить формат данных для заданной команды при связи со SCADA-Сервером, указав используемую длину буфера
        /// </summary>
        protected bool CheckDataFormat(byte[] buffer, int cmdNum, int bufLen)
        {
            return bufLen >= 3 && buffer[0] + 256 * buffer[1] == bufLen && buffer[2] == cmdNum;
        }

        /// <summary>
        /// Получить строковое обозначение директории на SCADA-Сервере
        /// </summary>
        protected string DirToString(Dirs directory)
        {
            switch (directory)
            {
                case Dirs.Cur:
                    return "[Srez]" + Path.DirectorySeparatorChar;
                case Dirs.Hour:
                    return "[Hr]" + Path.DirectorySeparatorChar;
                case Dirs.Min:
                    return "[Min]" + Path.DirectorySeparatorChar;
                case Dirs.Events:
                    return "[Ev]" + Path.DirectorySeparatorChar;
                case Dirs.BaseDAT:
                    return "[Base]" + Path.DirectorySeparatorChar;
                case Dirs.Itf:
                    return "[Itf]" + Path.DirectorySeparatorChar;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Записать действие в журнал работы
        /// </summary>
        protected void WriteAction(string text, Log.ActTypes actType)
        {
            if (log != null)
                log.WriteAction(text, actType);
            else if (writeToLog != null)
                writeToLog(text);
        }

        /// <summary>
        /// Установить соединение со SCADA-Сервером и произвести авторизацию
        /// </summary>
        protected bool Connect()
        {
            try
            {
                commState = CommStates.Disconnected;
                WriteAction(string.Format(Localization.UseRussian ? 
                    "Установка соединения со SCADA-Сервером \"{0}\"" : 
                    "Connect to SCADA-Server \"{0}\"", commSettings.ServerHost), Log.ActTypes.Action);

                // определение IP-адреса, если он указан в конфигурации программы
                IPAddress ipAddress = null;
                try { ipAddress = IPAddress.Parse(commSettings.ServerHost); }
                catch { }

                // создание, настройка и попытка установки соединения
                tcpClient = new TcpClient();
                tcpClient.NoDelay = true;            // sends data immediately upon calling NetworkStream.Write
                tcpClient.ReceiveBufferSize = 16384; // 16 кБ
                tcpClient.SendBufferSize = 8192;     // 8 кБ, размер по умолчанию
                tcpClient.SendTimeout = TcpSendTimeout;
                tcpClient.ReceiveTimeout = TcpReceiveTimeout;

                if (ipAddress == null)
                    tcpClient.Connect(commSettings.ServerHost, commSettings.ServerPort);
                else
                    tcpClient.Connect(ipAddress, commSettings.ServerPort);

                netStream = tcpClient.GetStream();

                // получение версии SCADA-Сервера
                byte[] buf = new byte[5];
                int bytesRead = netStream.Read(buf, 0, 5);

                // обработка считанных данных версии
                if (bytesRead == buf.Length && CheckDataFormat(buf, 0x00))
                {
                    commState = CommStates.Connected;
                    serverVersion = buf[4] + "." + buf[3]; 

                    // запрос правильности имени и пароля пользователя, его роли
                    byte userLen = (byte)commSettings.ServerUser.Length;
                    byte pwdLen = (byte)commSettings.ServerPwd.Length;
                    buf = new byte[5 + userLen + pwdLen];

                    buf[0] = (byte)(buf.Length % 256);
                    buf[1] = (byte)(buf.Length / 256);
                    buf[2] = 0x01;
                    buf[3] = userLen;
                    Array.Copy(Encoding.Default.GetBytes(commSettings.ServerUser), 0, buf, 4, userLen);
                    buf[4 + userLen] = pwdLen;
                    Array.Copy(Encoding.Default.GetBytes(commSettings.ServerPwd), 0, buf, 5 + userLen, pwdLen);

                    netStream.Write(buf, 0, buf.Length);

                    // приём результата
                    buf = new byte[4];
                    bytesRead = netStream.Read(buf, 0, 4);

                    // обработка считанных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x01))
                    {
                        int roleID = buf[3];

                        if (roleID == BaseValues.Roles.App)
                        {
                            commState = CommStates.Authorized;
                        }
                        else if (roleID < BaseValues.Roles.Err)
                        {
                            errMsg = Localization.UseRussian ? 
                                "Недостаточно прав для соединения со SCADA-Сервером" :
                                "Insufficient rights to connect to SCADA-Server";
                            WriteAction(errMsg, Log.ActTypes.Error);
                            commState = CommStates.Error;
                        }
                        else // roleID == BaseValues.Roles.Err
                        {
                            errMsg = Localization.UseRussian ? 
                                "Неверное имя пользователя или пароль" :
                                "User name or password is incorrect";
                            WriteAction(errMsg, Log.ActTypes.Error);
                            commState = CommStates.Error;
                        }
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на запрос правильности имени и пароля" :
                            "Incorrect SCADA-Server response to check user name and password request";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
                else
                {
                    errMsg = Localization.UseRussian ? 
                        "Неверный формат ответа SCADA-Сервера на запрос версии" :
                        "Incorrect SCADA-Server response to version request";
                    WriteAction(errMsg, Log.ActTypes.Error);
                    commState = CommStates.Error;
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при установке соединения со SCADA-Сервером: " : 
                    "Error connecting to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }

            // возврат результата
            if (commState == CommStates.Authorized)
            {
                return true;
            }
            else
            {
                Disconnect();
                return false;
            }
        }

        /// <summary>
        /// Разорвать соединение со SCADA-Сервером
        /// </summary>
        protected void Disconnect()
        {
            try
            {
                commState = CommStates.Disconnected;
                serverVersion = "";

                if (tcpClient != null)
                {
                    WriteAction(Localization.UseRussian ? 
                        "Разрыв соединения со SCADA-Сервером" : 
                        "Disconnect from SCADA-Server", Log.ActTypes.Action);

                    if (netStream != null)
                    {
                        // очистка (для корректного разъединения) и закрытие потока данных TCP-клиента
                        ClearNetStream();
                        netStream.Close();
                        netStream = null;
                    }

                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при разрыве соединения со SCADA-Сервером: " : 
                    "Error disconnecting from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Считать информацию из потока данных TCP-клиента
        /// </summary>
        /// <remarks>Метод используется для считывания "большого" объёма данных</remarks>
        protected int ReadNetStream(byte[] buffer, int offset, int size)
        {
            int bytesRead = 0;
            DateTime startReadDT = DateTime.Now;

            do
            {
                bytesRead += netStream.Read(buffer, bytesRead + offset, size - bytesRead);
            } while (bytesRead < size && (DateTime.Now - startReadDT).TotalMilliseconds <= TcpReceiveTimeout);

            return bytesRead;
        }

        /// <summary>
        /// Очистить поток данных TCP-клиента
        /// </summary>
        protected void ClearNetStream()
        {
            try
            {
                if (netStream != null && netStream.DataAvailable)
                {
                    // считывание оставшихся данных из потока, но не более 100 кБ
                    byte[] buf = new byte[1024];
                    int n = 0;
                    while (netStream.DataAvailable && ++n <= 100)
                        try { netStream.Read(buf, 0, 1024); }
                        catch { }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при очистке сетевого потока: " : 
                    "Error clear network stream: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Восстановить соединение со SCADA-Сервером и произвести авторизацию при необходимости
        /// </summary>
        protected bool RestoreConnection()
        {
            try
            {
                bool connectNeeded = false; // требуется повторное соединение
                DateTime now = DateTime.Now;

                if (commState >= CommStates.Authorized)
                {
                    if (now - restConnSuccDT > PingSpan)
                    {
                        // проверка соединения
                        try
                        {
                            WriteAction(Localization.UseRussian ?
                                "Запрос состояния SCADA-Сервера" :
                                "Request SCADA-Server state", Log.ActTypes.Action);
                            commState = CommStates.WaitResponse;

                            // запрос состояния SCADA-Сервера (ping)
                            byte[] buf = new byte[3];
                            buf[0] = 0x03;
                            buf[1] = 0x00;
                            buf[2] = 0x02;
                            netStream.Write(buf, 0, 3);

                            // приём результата
                            buf = new byte[4];
                            netStream.Read(buf, 0, 4);

                            // обработка результата
                            if (CheckDataFormat(buf, 0x02))
                            {
                                commState = buf[3] > 0 ? CommStates.Authorized : CommStates.NotReady;
                            }
                            else
                            {
                                errMsg = Localization.UseRussian ?
                                    "Неверный формат ответа SCADA-Сервера на запрос состояния" :
                                    "Incorrect SCADA-Server response to state request";
                                WriteAction(errMsg, Log.ActTypes.Error);
                                commState = CommStates.Error;
                                connectNeeded = true;
                            }
                        }
                        catch
                        {
                            connectNeeded = true;
                        }
                    }
                }
                else if (now - restConnErrDT > ConnectSpan)
                {
                    connectNeeded = true;
                }

                // соединение при необходимости
                if (connectNeeded)
                {
                    if (tcpClient != null)
                        Disconnect();

                    if (Connect())
                    {
                        restConnSuccDT = now;
                        restConnErrDT = DateTime.MinValue;
                        return true;
                    }
                    else
                    {
                        restConnSuccDT = DateTime.MinValue;
                        restConnErrDT = now;
                        return false;
                    }
                }
                else
                {
                    ClearNetStream(); // очистка потока данных TCP-клиента

                    if (commState >= CommStates.Authorized)
                    {
                        restConnSuccDT = now;
                        return true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ?
                            "Невозможно соединиться со SCADA-Сервером. Повторите попытку." :
                            "Unable to connect to SCADA-Server. Try again.";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при восстановлении соединения со SCADA-Сервером: " :
                    "Error restoring connection with SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                commState = CommStates.Error;
                return false;
            }
        }

        /// <summary>
        /// Восстановить значение таймаута приёма данных через TCP по умолчанию
        /// </summary>
        protected void RestoreReceiveTimeout()
        {
            try 
            {
                if (tcpClient.ReceiveTimeout != TcpReceiveTimeout)
                    tcpClient.ReceiveTimeout = TcpReceiveTimeout;
            }
            catch { }
        }

        /// <summary>
        /// Принять файл от SCADA-Сервера
        /// </summary>
        protected bool ReceiveFileToStream(Dirs dir, string fileName, Stream inStream)
        {
            bool result = false;
            string filePath = DirToString(dir) + fileName;

            try
            {
#if DETAILED_LOG
                WriteAction(string.Format(Localization.UseRussian ? 
                    "Приём файла {0} от SCADA-Сервера" : 
                    "Receive file {0} from SCADA-Server", filePath), Log.ActTypes.Action);
#endif

                commState = CommStates.WaitResponse;
                tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                const int DataSize = 50 * 1024; // размер запрашиваемых данных 50 КБ
                const byte DataSizeL = DataSize % 256;
                const byte DataSizeH = DataSize / 256;

                byte[] buf = new byte[6 + DataSize]; // буфер отправляемых и получаемых данных
                bool open = true;  // выполняется открытие файла
                bool stop = false; // признак завершения приёма данных

                while (!stop)
                {
                    if (open)
                    {
                        // отправка команды открытия файла и чтения данных
                        byte fileNameLen = (byte)fileName.Length;
                        int cmdLen = 7 + fileNameLen;
                        buf[0] = (byte)(cmdLen % 256);
                        buf[1] = (byte)(cmdLen / 256);
                        buf[2] = 0x08;
                        buf[3] = (byte)dir;
                        buf[4] = fileNameLen;
                        Array.Copy(Encoding.Default.GetBytes(fileName), 0, buf, 5, fileNameLen);
                        buf[cmdLen - 2] = DataSizeL;
                        buf[cmdLen - 1] = DataSizeH;
                        netStream.Write(buf, 0, cmdLen);
                    }
                    else
                    {
                        // отправка команды чтения данных из файла
                        buf[0] = 0x05;
                        buf[1] = 0x00;
                        buf[2] = 0x0A;
                        buf[3] = DataSizeL;
                        buf[4] = DataSizeH;
                        netStream.Write(buf, 0, 5);
                    }

                    // приём результата открытия файла и считанных данных
                    byte cmdNum = buf[2];
                    int headerLen = open ? 6 : 5;
                    int bytesRead = netStream.Read(buf, 0, headerLen);
                    int dataSizeRead = 0; // размер считанных из файла данных                    

                    if (bytesRead == headerLen)
                    {
                        dataSizeRead = buf[headerLen - 2] + 256 * buf[headerLen - 1];
                        if (0 < dataSizeRead && dataSizeRead <= DataSize)
                            bytesRead += ReadNetStream(buf, headerLen, dataSizeRead);
                    }

                    if (CheckDataFormat(buf, cmdNum, bytesRead) && bytesRead == dataSizeRead + headerLen)
                    {
                        if (open)
                        {
                            open = false;

                            if (buf[3] > 0) // файл открыт
                            {
                                inStream.Write(buf, 6, dataSizeRead);
                                commState = CommStates.Authorized;
                                stop = dataSizeRead < DataSize;
                            }
                            else
                            {
                                errMsg = string.Format(Localization.UseRussian ? 
                                    "SCADA-Серверу не удалось открыть файл {0}" : 
                                    "SCADA-Server unable to open file {0}", filePath);
                                WriteAction(errMsg, Log.ActTypes.Action);
                                commState = CommStates.NotReady;
                                stop = true;
                            }
                        }
                        else
                        {
                            inStream.Write(buf, 5, dataSizeRead);
                            commState = CommStates.Authorized;
                            stop = dataSizeRead < DataSize;
                        }
                    }
                    else
                    {
                        errMsg = string.Format(Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на команду открытия или чтения из файла {0}" :
                            "Incorrect SCADA-Server response to open file or read from file {0} command ", filePath);
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                        stop = true;
                    }
                }

                // определение результата
                if (commState == CommStates.Authorized)
                {
                    if (inStream.Length > 0)
                        inStream.Position = 0;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Localization.UseRussian ? 
                    "Ошибка при приёме файла {0} от SCADA-Сервера: " :
                    "Error receiving file {0} from SCADA-Server: ", filePath) + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
            }

            return result;
        }

        /// <summary>
        /// Отправить команду ТУ SCADA-Серверу
        /// </summary>
        protected bool SendCommand(int userID, int ctrlCnl, double cmdVal, byte[] cmdData, int kpNum, out bool result)
        {
            Monitor.Enter(tcpLock);
            bool complete = false;
            result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    WriteAction(Localization.UseRussian ? 
                        "Отправка команды ТУ SCADA-Серверу" :
                        "Send telecommand to SCADA-Server", Log.ActTypes.Action);

                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка команды
                    int cmdLen = double.IsNaN(cmdVal) ? cmdData == null ? 12 : 10 + cmdData.Length : 18;
                    byte[] buf = new byte[cmdLen];
                    buf[0] = (byte)(cmdLen % 256);
                    buf[1] = (byte)(cmdLen / 256);
                    buf[2] = 0x06;
                    buf[3] = (byte)(userID % 256);
                    buf[4] = (byte)(userID / 256);
                    buf[6] = (byte)(ctrlCnl % 256);
                    buf[7] = (byte)(ctrlCnl / 256);

                    if (!double.IsNaN(cmdVal)) // стандартная команда
                    {
                        buf[5] = 0x00;
                        buf[8] = 0x08;
                        buf[9] = 0x00;
                        byte[] bytes = BitConverter.GetBytes(cmdVal);
                        Array.Copy(bytes, 0, buf, 10, 8);
                    }
                    else if (cmdData != null) // бинарная команда
                    {
                        buf[5] = 0x01;
                        int cmdDataLen = cmdData.Length;
                        buf[8] = (byte)(cmdDataLen % 256);
                        buf[9] = (byte)(cmdDataLen / 256);
                        Array.Copy(cmdData, 0, buf, 10, cmdDataLen);
                    }
                    else // опрос КП
                    {
                        buf[5] = 0x02;
                        buf[8] = 0x02;
                        buf[9] = 0x00;
                        buf[10] = (byte)(kpNum % 256);
                        buf[11] = (byte)(kpNum / 256);
                    }

                    netStream.Write(buf, 0, cmdLen);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x06))
                    {
                        result = buf[3] > 0;
                        commState = result ? CommStates.Authorized : CommStates.NotReady;
                        complete = true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на команду ТУ" :
                            "Incorrect SCADA-Server response to telecommand";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при отправке команды ТУ SCADA-Серверу: " :
                    "Error sending telecommand to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return complete;
        }


        /// <summary>
        /// Запросить правильность имени и пароля пользователя от SCADA-Сервера, получить его роль.
        /// Возвращает успешность выполнения запроса
        /// </summary>
        public bool CheckUser(string username, string password, out int roleID)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            roleID = BaseValues.Roles.Disabled;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // запрос правильности имени и пароля пользователя, его роли
                    byte userLen = username == null ? (byte)0 : (byte)username.Length;
                    byte pwdLen = password == null ? (byte)0 : (byte)password.Length;
                    byte[] buf = new byte[5 + userLen + pwdLen];

                    buf[0] = (byte)(buf.Length % 256);
                    buf[1] = (byte)(buf.Length / 256);
                    buf[2] = 0x01;
                    buf[3] = userLen;
                    if (userLen > 0)
                        Array.Copy(Encoding.Default.GetBytes(username), 0, buf, 4, userLen);
                    buf[4 + userLen] = pwdLen;
                    if (pwdLen > 0)
                        Array.Copy(Encoding.Default.GetBytes(password), 0, buf, 5 + userLen, pwdLen);

                    netStream.Write(buf, 0, buf.Length);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x01))
                    {
                        roleID = buf[3];
                        result = true;
                        commState = CommStates.Authorized;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на запрос правильности имени и пароля" :
                            "Incorrect SCADA-Server response to check user name and password request";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при запросе правильности имени и пароля пользователя от SCADA-Сервера: " :
                    "Error requesting check user name and password to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять таблицу базы конфигурации от SCADA-Сервера
        /// </summary>
        public bool ReceiveBaseTable(string tableName, DataTable dataTable)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            if (ReceiveFileToStream(Dirs.BaseDAT, tableName, memStream))
                            {
                                BaseAdapter adapter = new BaseAdapter();
                                adapter.Stream = memStream;
                                adapter.TableName = tableName;
                                adapter.Fill(dataTable, false);
                                result = true;
                            }
                        }
                    }
                }
                finally
                {
                    // очистка таблицы, если не удалось получить новые данные
                    if (!result)
                        dataTable.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при приёме таблицы базы конфигурации от SCADA-Сервера: " : 
                    "Error receiving configuration database table from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Receives the table of the configuration database from Server.
        /// </summary>
        public bool ReceiveBaseTable(string tableName, IBaseTable baseTable)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            if (ReceiveFileToStream(Dirs.BaseDAT, tableName, memStream))
                            {
                                BaseAdapter adapter = new BaseAdapter();
                                adapter.Stream = memStream;
                                adapter.TableName = tableName;
                                adapter.Fill(baseTable, false);
                                result = true;
                            }
                        }
                    }
                }
                finally
                {
                    if (!result)
                        baseTable.ClearItems();
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при приёме таблицы базы конфигурации от Сервера: " :
                    "Error receiving configuration database table from Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять таблицу срезов от SCADA-Сервера
        /// </summary>
        public bool ReceiveSrezTable(string tableName, SrezTableLight srezTableLight)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        // определение директории таблицы
                        Dirs dir = Dirs.Cur;
                        if (tableName.Length > 0)
                        {
                            if (tableName[0] == 'h')
                                dir = Dirs.Hour;
                            else if (tableName[0] == 'm')
                                dir = Dirs.Min;
                        }

                        // приём данных
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            if (ReceiveFileToStream(dir, tableName, memStream))
                            {
                                SrezAdapter adapter = new SrezAdapter();
                                adapter.Stream = memStream;
                                adapter.TableName = tableName;
                                adapter.Fill(srezTableLight);
                                result = true;
                            }
                        }
                    }
                }
                finally
                {
                    // очистка таблицы, если не удалось получить новые данные
                    if (!result)
                    {
                        srezTableLight.Clear();
                        srezTableLight.TableName = tableName;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при приёме таблицы срезов от SCADA-Сервера: " :
                    "Error receiving data table from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять тренд входного канала от SCADA-Сервера
        /// </summary>
        public bool ReceiveTrend(string tableName, DateTime date, Trend trend)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        WriteAction(string.Format(Localization.UseRussian ? 
                            "Приём тренда входного канала {0} от SCADA-Сервера. Файл: {1}" : 
                            "Receive input channel {0} trend from SCADA-Server. File: {1}", 
                            trend.CnlNum, tableName), Log.ActTypes.Action);

                        commState = CommStates.WaitResponse;
                        tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                        byte tableType;        // тип таблицы: текущая, часовая или минутная
                        byte year, month, day; // дата запрашиваемых данных

                        if (tableName == SrezAdapter.CurTableName)
                        {
                            tableType = 0x01;
                            year = month = day = 0;
                        }
                        else
                        {
                            tableType = tableName.Length > 0 && tableName[0] == 'h' ? (byte)0x02 : (byte)0x03;
                            year = (byte)(date.Year % 100);
                            month = (byte)date.Month;
                            day = (byte)date.Day;
                        }

                        // отправка запроса тренда входного канала
                        byte[] buf = new byte[13];
                        buf[0] = 0x0D;
                        buf[1] = 0x00;
                        buf[2] = 0x0D;
                        buf[3] = tableType;
                        buf[4] = year;
                        buf[5] = month;
                        buf[6] = day;
                        buf[7] = 0x01;
                        buf[8] = 0x00;
                        byte[] bytes = BitConverter.GetBytes(trend.CnlNum);
                        Array.Copy(bytes, 0, buf, 9, 4);
                        netStream.Write(buf, 0, 13);

                        // приём данных тренда входного канала
                        buf = new byte[7];
                        int bytesRead = netStream.Read(buf, 0, 7);
                        int pointCnt = 0;

                        if (bytesRead == 7)
                        {
                            pointCnt = buf[5] + buf[6] * 256;

                            if (pointCnt > 0)
                            {
                                Array.Resize<byte>(ref buf, 7 + pointCnt * 18);
                                bytesRead += ReadNetStream(buf, 7, buf.Length - 7);
                            }
                        }

                        // заполение тренда входного канала из полученных данных
                        if (bytesRead == buf.Length && buf[4] == 0x0D)
                        {
                            for (int i = 0; i < pointCnt; i++)
                            {
                                Trend.Point point;
                                int pos = i * 18 + 7;
                                point.DateTime = ScadaUtils.DecodeDateTime(BitConverter.ToDouble(buf, pos));
                                point.Val = BitConverter.ToDouble(buf, pos + 8);
                                point.Stat = BitConverter.ToUInt16(buf, pos + 16);

                                trend.Points.Add(point);
                            }

                            trend.Sort();
                            result = true;
                            commState = CommStates.Authorized;
                        }
                        else
                        {
                            errMsg = Localization.UseRussian ? 
                                "Неверный формат ответа SCADA-Сервера на запрос тренда входного канала" :
                                "Incorrect SCADA-Server response to input channel trend request";
                            WriteAction(errMsg, Log.ActTypes.Error);
                            commState = CommStates.Error;
                        }
                    }
                }
                finally
                {
                    // очистка тренда, если не удалось получить новые данные
                    if (!result)
                    {
                        trend.Clear();
                        trend.TableName = tableName;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при приёме тренда входного канала от SCADA-Сервера: " :
                    "Error receiving input channel trend from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Receives current data from Server.
        /// </summary>
        public bool ReceiveCurData(SrezTableLight.Srez srez)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
#if DETAILED_LOG
                    WriteAction(Localization.UseRussian ?
                        "Приём текущих данных от Сервера" :
                        "Receive current data from Server", Log.ActTypes.Action);
#endif
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // send a request
                    ushort cnlCnt = (ushort)srez.CnlNums.Length;
                    int bufLen = 9 + cnlCnt * 4;
                    byte[] buf = new byte[bufLen];
                    buf[0] = (byte)(bufLen % 256);
                    buf[1] = (byte)(bufLen / 256);
                    buf[2] = 0x0D; // command
                    buf[3] = 0x01; // current data
                    buf[4] = 0x00; // year
                    buf[5] = 0x00; // month
                    buf[6] = 0x00; // day
                    buf[7] = (byte)(cnlCnt % 256);
                    buf[8] = (byte)(cnlCnt / 256);

                    for (int cnlInd = 0, arrInd = 9; cnlInd < cnlCnt; cnlInd++, arrInd += 4)
                    {
                        byte[] bytes = BitConverter.GetBytes(srez.CnlNums[cnlInd]);
                        Array.Copy(bytes, 0, buf, arrInd, 4);
                    }

                    netStream.Write(buf, 0, bufLen);

                    // receive a response
                    int bytesToRead = 15 + cnlCnt * 10;
                    buf = new byte[bytesToRead];
                    int bytesRead = ReadNetStream(buf, 0, bytesToRead);

                    if (bytesRead == bytesToRead && buf[4] == 0x0D /*command*/ && 
                        buf[5] + buf[6] * 256 == 1 /*snapshot count*/)
                    {
                        for (int cnlInd = 0, arrInd = 15; cnlInd < cnlCnt; cnlInd++, arrInd += 10)
                        {
                            srez.CnlData[cnlInd] = new SrezTableLight.CnlData(
                                BitConverter.ToDouble(buf, arrInd),
                                BitConverter.ToUInt16(buf, arrInd + 8));
                        }

                        result = true;
                        commState = CommStates.Authorized;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ?
                            "Неверный формат ответа Сервера на запрос текущих данных" :
                            "Incorrect Server response to current data request";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при приёме текущих данных от Сервера: " :
                    "Error receiving current data from Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять таблицу событий от SCADA-Сервера
        /// </summary>
        public bool ReceiveEventTable(string tableName, EventTableLight eventTableLight)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            if (ReceiveFileToStream(Dirs.Events, tableName, memStream))
                            {
                                EventAdapter adapter = new EventAdapter();
                                adapter.Stream = memStream;
                                adapter.TableName = tableName;
                                adapter.Fill(eventTableLight);
                                result = true;
                            }
                        }
                    }
                }
                finally
                {
                    // очистка таблицы, если не удалось получить новые данные
                    if (!result)
                    {
                        eventTableLight.Clear();
                        eventTableLight.TableName = tableName;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при приёме таблицы событий от SCADA-Сервера: " : 
                    "Error receiving event table from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять представление от SCADA-Сервера
        /// </summary>
        public bool ReceiveView(string fileName, BaseView view)
        {
            bool result = ReceiveUiObj(fileName, view);
            if (result && view != null)
                view.Path = fileName;
            return result;
        }

        /// <summary>
        /// Принять объект пользовательского интерфейса от SCADA-Сервера
        /// </summary>
        public bool ReceiveUiObj(string fileName, ISupportLoading uiObj)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            errMsg = "";

            try
            {
                try
                {
                    if (RestoreConnection())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            if (ReceiveFileToStream(Dirs.Itf, fileName, memStream))
                            {
                                uiObj.LoadFromStream(memStream);
                                result = true;
                            }
                        }
                    }
                }
                finally
                {
                    if (!result)
                        uiObj.Clear();
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при приёме объекта пользовательского интерфейса от SCADA-Сервера: " :
                    "Error receiving user interface object from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(tcpLock);
            }

            return result;
        }

        /// <summary>
        /// Принять файл от SCADA-Сервера
        /// </summary>
        public bool ReceiveFile(Dirs dir, string fileName, Stream stream)
        {
            lock (tcpLock)
            {
                return RestoreConnection() && ReceiveFileToStream(dir, fileName, stream);
            }
        }

        /// <summary>
        /// Принять дату и время изменения файла от SCADA-Сервера.
        /// В случае отсутствия файла возвращается минимальная дата
		/// </summary>
        public DateTime ReceiveFileAge(Dirs dir, string fileName)
        {
            Monitor.Enter(tcpLock);
            DateTime result = DateTime.MinValue;
            string filePath = DirToString(dir) + fileName;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
#if DETAILED_LOG
                    WriteAction(string.Format(Localization.UseRussian ? 
                        "Приём даты и времени изменения файла {0} от SCADA-Сервера" :
                        "Receive date and time of file {0} modification from SCADA-Server", filePath), 
                        Log.ActTypes.Action);
#endif

                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка запроса даты и времени изменения файла
                    int cmdLen = 6 + fileName.Length;
                    byte[] buf = new byte[cmdLen];
                    buf[0] = (byte)(cmdLen % 256);
                    buf[1] = (byte)(cmdLen / 256);
                    buf[2] = 0x0C;
                    buf[3] = 0x01;
                    buf[4] = (byte)dir;
                    buf[5] = (byte)fileName.Length;
                    Array.Copy(Encoding.Default.GetBytes(fileName), 0, buf, 6, fileName.Length);
                    netStream.Write(buf, 0, cmdLen);

                    // приём даты и времени изменения файла
                    buf = new byte[12];
                    netStream.Read(buf, 0, 12);

                    // обработка даты и времени изменения файла
                    if (CheckDataFormat(buf, 0x0C))
                    {
                        double dt = BitConverter.ToDouble(buf, 4);
                        result = dt == 0.0 ? DateTime.MinValue : ScadaUtils.DecodeDateTime(dt);
                        commState = CommStates.Authorized;
                    }
                    else
                    {
                        errMsg = string.Format(Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на запрос даты и времени изменения файла {0}" :
                            "Incorrect SCADA-Server response to file modification date and time request", filePath);
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Localization.UseRussian ?
                        "Ошибка при приёме даты и времени изменения файла {0} от SCADA-Сервера: " :
                        "Error receiving date and time of file {0} modification from SCADA-Server: ", filePath) + 
                        ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return result;
        }


        /// <summary>
        /// Отправить стандартную команду ТУ SCADA-Серверу
        /// </summary>
        public bool SendStandardCommand(int userID, int ctrlCnl, double cmdVal, out bool result)
        {
            return SendCommand(userID, ctrlCnl, cmdVal, null, 0, out result);
        }

        /// <summary>
        /// Отправить бинарную команду ТУ SCADA-Серверу
        /// </summary>
        public bool SendBinaryCommand(int userID, int ctrlCnl, byte[] cmdData, out bool result)
        {
            return SendCommand(userID, ctrlCnl, double.NaN, cmdData, 0, out result);
        }

        /// <summary>
        /// Отправить команду внеочередного опроса КП SCADA-Серверу
        /// </summary>
        public bool SendRequestCommand(int userID, int ctrlCnl, int kpNum, out bool result)
        {
            return SendCommand(userID, ctrlCnl, double.NaN, null, kpNum, out result);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public bool SendCommand(int userID, int ctrlCnl, Command cmd, out bool result)
        {
            switch (cmd.CmdTypeID)
            {
                case BaseValues.CmdTypes.Standard:
                    return SendCommand(userID, ctrlCnl, cmd.CmdVal, null, 0, out result);
                case BaseValues.CmdTypes.Binary:
                    return SendCommand(userID, ctrlCnl, double.NaN, cmd.CmdData, 0, out result);
                case BaseValues.CmdTypes.Request:
                    return SendCommand(userID, ctrlCnl, double.NaN, null, cmd.KPNum, out result);
                default:
                    throw new InvalidOperationException("Unknown command type.");
            }
        }

        /// <summary>
        /// Принять команду ТУ от SCADA-Сервера
        /// </summary>
        /// <remarks>
        /// Для стандартной команды возвращаемые данные команды равны null.
        /// Для бинарной команды возвращаемое значение команды равно double.NaN.
        /// Для команды опроса КП возвращаемое значение команды равно double.NaN и данные команды равны null.</remarks>
        public bool ReceiveCommand(out int kpNum, out int cmdNum, out double cmdVal, out byte[] cmdData)
        {
            Command cmd;
            bool result = ReceiveCommand(out cmd);

            if (result)
            {
                kpNum = cmd.KPNum;
                cmdNum = cmd.CmdNum;
                cmdVal = cmd.CmdTypeID == BaseValues.CmdTypes.Standard ? cmd.CmdVal : double.NaN;
                cmdData = cmd.CmdTypeID == BaseValues.CmdTypes.Binary ? cmd.CmdData : null;
            }
            else
            {
                kpNum = 0;
                cmdNum = 0;
                cmdVal = double.NaN;
                cmdData = null;
            }

            return result;
        }

        /// <summary>
        /// Принять команду от SCADA-Сервера
        /// </summary>
        public bool ReceiveCommand(out Command cmd)
        {
            Monitor.Enter(tcpLock);
            bool result = false;
            cmd = null;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // запрос команды
                    byte[] buf = new byte[3];
                    buf[0] = 0x03;
                    buf[1] = 0x00;
                    buf[2] = 0x07;
                    netStream.Write(buf, 0, 3);

                    // приём команды
                    buf = new byte[5];
                    int bytesRead = netStream.Read(buf, 0, 5);
                    int cmdDataLen = 0;

                    if (bytesRead == 5)
                    {
                        cmdDataLen = buf[3] + buf[4] * 256;

                        if (cmdDataLen > 0)
                        {
                            Array.Resize<byte>(ref buf, 10 + cmdDataLen);
                            bytesRead += netStream.Read(buf, 5, 5 + cmdDataLen);
                        }
                    }

                    // обработка полученных данных
                    if (CheckDataFormat(buf, 0x07) && bytesRead == buf.Length)
                    {
                        if (cmdDataLen > 0)
                        {
                            byte cmdType = buf[5];
                            cmd = new Command(cmdType);

                            if (cmdType == 0)
                            {
                                cmd.CmdVal = BitConverter.ToDouble(buf, 10);
                            }
                            else if (cmdType == 1)
                            {
                                byte[] cmdData = new byte[cmdDataLen];
                                Array.Copy(buf, 10, cmdData, 0, cmdDataLen);
                                cmd.CmdData = cmdData;
                            }

                            cmd.KPNum = buf[6] + buf[7] * 256;
                            cmd.CmdNum = buf[8] + buf[9] * 256;

                            commState = CommStates.Authorized;
                            result = true;
                        }
                        else // команд в очереди нет
                        {
                            commState = CommStates.Authorized;
                        }
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ?
                            "Неверный формат ответа SCADA-Сервера на запрос команды ТУ" :
                            "Incorrect SCADA-Server response to telecommand request";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ?
                    "Ошибка при приёме команды ТУ от SCADA-Сервера: " :
                    "Error requesting telecommand from SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return result;
        }


        /// <summary>
        /// Отправить текущий срез SCADA-Серверу
        /// </summary>
        public bool SendSrez(SrezTableLight.Srez curSrez, out bool result)
        {
            Monitor.Enter(tcpLock);
            bool complete = false;
            result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка команды записи текущего среза
                    int cnlCnt = curSrez.CnlNums.Length;
                    int cmdLen = cnlCnt * 14 + 5;

                    byte[] buf = new byte[cmdLen];
                    buf[0] = (byte)(cmdLen % 256);
                    buf[1] = (byte)(cmdLen / 256);
                    buf[2] = 0x03;
                    buf[3] = (byte)(cnlCnt % 256);
                    buf[4] = (byte)(cnlCnt / 256);

                    for (int i = 0; i < cnlCnt; i++)
                    {
                        byte[] bytes = BitConverter.GetBytes((UInt32)curSrez.CnlNums[i]);
                        Array.Copy(bytes, 0, buf, i * 14 + 5, 4);

                        SrezTableLight.CnlData data = curSrez.CnlData[i];
                        bytes = BitConverter.GetBytes(data.Val);
                        Array.Copy(bytes, 0, buf, i * 14 + 9, 8);

                        bytes = BitConverter.GetBytes((UInt16)data.Stat);
                        Array.Copy(bytes, 0, buf, i * 14 + 17, 2);
                    }

                    netStream.Write(buf, 0, cmdLen);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x03))
                    {
                        result = buf[3] > 0;
                        commState = result ? CommStates.Authorized : CommStates.NotReady;
                        complete = true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на команду отправки текущего среза" :
                            "Incorrect SCADA-Server response to sending current data command";
                        WriteAction(errMsg, Log.ActTypes.Exception);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при отправке текущего среза SCADA-Серверу: " : 
                    "Error sending current data to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return complete;
        }

        /// <summary>
        /// Отправить архивный срез SCADA-Серверу
        /// </summary>
        public bool SendArchive(SrezTableLight.Srez arcSrez, out bool result)
        {
            Monitor.Enter(tcpLock);
            bool complete = false;
            result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка команды записи архивного среза
                    int cnlCnt = arcSrez.CnlNums.Length;
                    int cmdLen = cnlCnt * 14 + 13;

                    byte[] buf = new byte[cmdLen];
                    buf[0] = (byte)(cmdLen % 256);
                    buf[1] = (byte)(cmdLen / 256);
                    buf[2] = 0x04;

                    double arcDT = ScadaUtils.EncodeDateTime(arcSrez.DateTime);
                    byte[] bytes = BitConverter.GetBytes(arcDT);
                    Array.Copy(bytes, 0, buf, 3, 8);

                    buf[11] = (byte)(cnlCnt % 256);
                    buf[12] = (byte)(cnlCnt / 256);

                    for (int i = 0; i < cnlCnt; i++)
                    {
                        bytes = BitConverter.GetBytes((UInt32)arcSrez.CnlNums[i]);
                        Array.Copy(bytes, 0, buf, i * 14 + 13, 4);

                        SrezTableLight.CnlData data = arcSrez.CnlData[i];
                        bytes = BitConverter.GetBytes(data.Val);
                        Array.Copy(bytes, 0, buf, i * 14 + 17, 8);

                        bytes = BitConverter.GetBytes((UInt16)data.Stat);
                        Array.Copy(bytes, 0, buf, i * 14 + 25, 2);
                    }

                    netStream.Write(buf, 0, cmdLen);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x04))
                    {
                        result = buf[3] > 0;
                        commState = result ? CommStates.Authorized : CommStates.NotReady;
                        complete = true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ?
                            "Неверный формат ответа SCADA-Сервера на команду отправки архивного среза" :
                            "Incorrect SCADA-Server response to sending archive data command";
                        WriteAction(errMsg, Log.ActTypes.Exception);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при отправке архивного среза SCADA-Серверу: " : 
                    "Error sending archive data to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return complete;
        }

        /// <summary>
        /// Отправить событие SCADA-Серверу
        /// </summary>
        public bool SendEvent(EventTableLight.Event aEvent, out bool result)
        {
            Monitor.Enter(tcpLock);
            bool complete = false;
            result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка команды записи события
                    byte descrLen = (byte)aEvent.Descr.Length;
                    byte dataLen = (byte)aEvent.Data.Length;
                    int cmdLen = 46 + descrLen + dataLen;
                    byte[] buf = new byte[cmdLen];
                    buf[0] = (byte)(cmdLen % 256);
                    buf[1] = (byte)(cmdLen / 256);
                    buf[2] = 0x05;

                    double evDT = ScadaUtils.EncodeDateTime(aEvent.DateTime);
                    byte[] bytes = BitConverter.GetBytes(evDT);
                    Array.Copy(bytes, 0, buf, 3, 8);

                    buf[11] = (byte)(aEvent.ObjNum % 256);
                    buf[12] = (byte)(aEvent.ObjNum / 256);
                    buf[13] = (byte)(aEvent.KPNum % 256);
                    buf[14] = (byte)(aEvent.KPNum / 256);
                    buf[15] = (byte)(aEvent.ParamID % 256);
                    buf[16] = (byte)(aEvent.ParamID / 256);

                    bytes = BitConverter.GetBytes(aEvent.CnlNum);
                    Array.Copy(bytes, 0, buf, 17, 4);
                    bytes = BitConverter.GetBytes(aEvent.OldCnlVal);
                    Array.Copy(bytes, 0, buf, 21, 8);
                    bytes = BitConverter.GetBytes(aEvent.OldCnlStat);
                    Array.Copy(bytes, 0, buf, 29, 2);
                    bytes = BitConverter.GetBytes(aEvent.NewCnlVal);
                    Array.Copy(bytes, 0, buf, 31, 8);
                    bytes = BitConverter.GetBytes(aEvent.NewCnlStat);
                    Array.Copy(bytes, 0, buf, 39, 2);

                    buf[41] = aEvent.Checked ? (byte)0x01 : (byte)0x00;
                    buf[42] = (byte)(aEvent.UserID % 256);
                    buf[43] = (byte)(aEvent.UserID / 256);

                    buf[44] = descrLen;
                    Array.Copy(Encoding.Default.GetBytes(aEvent.Descr), 0, buf, 45, descrLen);
                    buf[45 + descrLen] = dataLen;
                    Array.Copy(Encoding.Default.GetBytes(aEvent.Data), 0, buf, 46 + descrLen, dataLen);

                    netStream.Write(buf, 0, cmdLen);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x05))
                    {
                        result = buf[3] > 0;
                        commState = result ? CommStates.Authorized : CommStates.NotReady;
                        complete = true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ?
                            "Неверный формат ответа SCADA-Сервера на команду отправки события" :
                            "Incorrect SCADA-Server response to sending event command";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при отправке события SCADA-Серверу: " :
                    "Error sending event to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return complete;
        }

        /// <summary>
        /// Отправить команду квитирования события SCADA-Серверу
        /// </summary>
        public bool CheckEvent(int userID, DateTime date, int evNum, out bool result)
        {
            Monitor.Enter(tcpLock);
            bool complete = false;
            result = false;
            errMsg = "";

            try
            {
                if (RestoreConnection())
                {
                    WriteAction(Localization.UseRussian ? 
                        "Отправка команды квитирования события SCADA-Серверу" :
                        "Send check event command to SCADA-Server", Log.ActTypes.Action);

                    commState = CommStates.WaitResponse;
                    tcpClient.ReceiveTimeout = commSettings.ServerTimeout;

                    // отправка команды
                    byte[] buf = new byte[10];
                    buf[0] = 0x0A;
                    buf[1] = 0x00;
                    buf[2] = 0x0E;
                    buf[3] = (byte)(userID % 256);
                    buf[4] = (byte)(userID / 256);
                    buf[5] = (byte)(date.Year % 100);
                    buf[6] = (byte)date.Month;
                    buf[7] = (byte)date.Day;
                    buf[8] = (byte)(evNum % 256);
                    buf[9] = (byte)(evNum / 256);
                    netStream.Write(buf, 0, 10);

                    // приём результата
                    buf = new byte[4];
                    int bytesRead = netStream.Read(buf, 0, 4);

                    // обработка полученных данных
                    if (bytesRead == buf.Length && CheckDataFormat(buf, 0x0E))
                    {
                        result = buf[3] > 0;
                        commState = result ? CommStates.Authorized : CommStates.NotReady;
                        complete = true;
                    }
                    else
                    {
                        errMsg = Localization.UseRussian ? 
                            "Неверный формат ответа SCADA-Сервера на команду квитирования события" :
                            "Incorrect SCADA-Server response to check event command";
                        WriteAction(errMsg, Log.ActTypes.Error);
                        commState = CommStates.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? 
                    "Ошибка при отправке команды квитирования события SCADA-Серверу: " :
                    "Error sending check event command to SCADA-Server: ") + ex.Message;
                WriteAction(errMsg, Log.ActTypes.Exception);
                Disconnect();
            }
            finally
            {
                RestoreReceiveTimeout();
                Monitor.Exit(tcpLock);
            }

            return complete;
        }

        /// <summary>
        /// Завершить работу со SCADA-Сервером и освободить ресурсы
        /// </summary>
        public void Close()
        {
            Disconnect();
        }
    }
}
