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
 * Module   : SCADA-Server Service
 * Summary  : Communication with clients
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2021
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Server.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Communication with clients
    /// <para>Взаимодействие с клиентами</para>
    /// </summary>
    sealed class Comm : IServerCommands
    {
        /// <summary>
        /// Директории на SCADA-Сервере
        /// </summary>
        private enum Dirs : byte
        {
            /// <summary>
            /// Директория текущего среза
            /// </summary>
            Cur = 0x01,
            /// <summary>
            /// Директория часовых срезов
            /// </summary>
            Hr = 0x02,
            /// <summary>
            /// Директория минутных срезов
            /// </summary>
            Min = 0x03,
            /// <summary>
            /// Директория событий
            /// </summary>
            Ev = 0x04,
            /// <summary>
            /// Директория базы конфигурации в формате DAT
            /// </summary>
            BaseDAT = 0x05,
            /// <summary>
            /// Директория интерфейса (таблиц, схем и т.п.)
            /// </summary>
            Itf = 0x06,
            /// <summary>
            /// Директория копии текущего среза
            /// </summary>
            CurCopy = 0x81,
            /// <summary>
            /// Директория копий часовых срезов
            /// </summary>
            HrCopy = 0x82,
            /// <summary>
            /// Директория копий минутных срезов
            /// </summary>
            MinCopy = 0x83,
            /// <summary>
            /// Директория копий событий
            /// </summary>
            EvCopy = 0x84
        }

        /// <summary>
        /// Наименования директорий
        /// </summary>
        private static readonly Dictionary<Dirs, string> DirNames = new Dictionary<Dirs, string>()
        {
            { Dirs.Cur, "[Cur]" + Path.DirectorySeparatorChar },
            { Dirs.Hr, "[Hr]" + Path.DirectorySeparatorChar },
            { Dirs.Min, "[Min]" + Path.DirectorySeparatorChar },
            { Dirs.Ev, "[Ev]" + Path.DirectorySeparatorChar },
            { Dirs.BaseDAT, "[Base]" + Path.DirectorySeparatorChar },
            { Dirs.Itf, "[Itf]" + Path.DirectorySeparatorChar },
            { Dirs.CurCopy, "[CurCopy]" + Path.DirectorySeparatorChar },
            { Dirs.HrCopy, "[HrCopy]" + Path.DirectorySeparatorChar },
            { Dirs.MinCopy, "[MinCopy]" + Path.DirectorySeparatorChar },
            { Dirs.EvCopy, "[EvCopy]" + Path.DirectorySeparatorChar }
        };

        /// <summary>
        /// Информация о подключенном клиенте
        /// </summary>
        private class ClientInfo
        {
            private static readonly string ActivityStr = 
                Localization.UseRussian ? "; активность: " : "; activity: ";
            private TcpClient tcpClient; // клиент TCP-соединения

            /// <summary>
            /// Конструктор
            /// </summary>
            private ClientInfo()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ClientInfo(TcpClient tcpClient)
            {
                TcpClient = tcpClient;
                Authenticated = false;
                UserName = "";
                UserRoleID = BaseValues.Roles.Disabled;
                ActivityDT = DateTime.Now;
                CmdList = new List<Command>();
                Dir = Dirs.Cur;
                FileName = "";
                FileStream = null;
            }

            /// <summary>
            /// Получить клиента TCP-соединения
            /// </summary>
            public TcpClient TcpClient
            {
                get
                {
                    return tcpClient;
                }
                private set
                {
                    tcpClient = value;
                    if (tcpClient == null)
                    {
                        NetStream = null;
                        Address = "";
                    }
                    else
                    {
                        NetStream = tcpClient.GetStream();
                        Address = ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString();
                    }
                }
            }
            /// <summary>
            /// Получить поток данных TCP-соединения
            /// </summary>
            public NetworkStream NetStream { get; private set; }
            /// <summary>
            /// Получить IP-адрес компьютера клиента
            /// </summary>
            public string Address { get; private set; }
            /// <summary>
            /// Получить или установить признак успешной аутентификации
            /// </summary>
            public bool Authenticated { get; set; }
            /// <summary>
            /// Получить или установить имя пользователя
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// Получить или установить идентификатор роли пользователя
            /// </summary>
            public int UserRoleID { get; set; }
            /// <summary>
            /// Получить или установить дату и время последней активности
            /// </summary>
            public DateTime ActivityDT { get; set; }
            /// <summary>
            /// Получить список команд ТУ
            /// </summary>
            public List<Command> CmdList { get; private set; }
            /// <summary>
            /// Получить или установить директорию запрашиваемого файла
            /// </summary>
            public Dirs Dir { get; set; }
            /// <summary>
            /// Получить или установить имя запрашиваемого файла
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// Получить информацию о полном имени запрашиваемого файла
            /// </summary>
            public string FullFileNameInfo
            {
                get
                {
                    return FileName == "" ? "" : DirNames[Dir] + FileName;
                }
            }
            /// <summary>
            /// Получить или установить поток запрашиваемого файла
            /// </summary>
            public FileStream FileStream { get; set; }

            /// <summary>
            /// Закрыть запрашиваемый файл
            /// </summary>
            public void CloseFile()
            {
                Dir = Dirs.Cur;
                FileName = "";

                if (FileStream != null)
                {
                    FileStream.Close();
                    FileStream = null;
                }
            }
            /// <summary>
            /// Вернуть строковое представление соединения с клиентом
            /// </summary>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(Address))
                    sb.Append(Address);

                if (Authenticated)
                    sb.Append("; ").Append(UserName).Append(" (").
                        Append(BaseValues.Roles.GetRoleName(UserRoleID)).Append(")");

                sb.Append(ActivityStr).Append(ActivityDT.ToString("T", Localization.Culture));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Команда ТУ и связанная с ней информация для последующей обработки.
        /// </summary>
        private class CmdItem
        {
            public int CtrlCnlNum { get; set; } 
            public Command Cmd { get; set; }
            public int UserID { get; set; }
        }


        /// <summary>
        /// Таймаут отправки данных по TCP, мс
        /// </summary>
        private const int TcpSendTimeout = 1000;
        /// <summary>
        /// Таймаут приёма данных по TCP, мс
        /// </summary>
        private const int TcpReceiveTimeout = 5000;
        /// <summary>
        /// Время неактивности клиента до его отключения, с
        /// </summary>
        private const int InactiveTime = 60;
        /// <summary>
        /// Время хранения команды ТУ в списке команд клиента, с
        /// </summary>
        private const int CmdLifeTime = 60;
        /// <summary>
        /// Время ожидания остановки потока, мс
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// Длина буфера принимаемых данных
        /// </summary>
        private const int InBufLenght = 1000000;
        /// <summary>
        /// Длина буфера передаваемых данных
        /// </summary>
        private const int OutBufLenght = 100000;
        /// <summary>
        /// Буфер для передачи номера версии приложения
        /// </summary>
        private readonly byte[] AppVersionBuf = { 0x05, 0x00, 0x00, ServerUtils.AppVersionLo, ServerUtils.AppVersionHi };
        /// <summary>
        /// Описания команд
        /// </summary>
        private readonly string[] CmdDescrs;

        private readonly MainLogic mainLogic;      // ссылка на объект основной логики приложения
        private readonly Log appLog;               // журнал приложения
        private readonly Settings settings;        // настройки приложения
        private readonly byte[] inBuf;             // буфер принимаемых данных
        private readonly byte[] outBuf;            // буфер передаваемых данных
        private readonly List<Command> cmdBuf;     // буфер команд ТУ для передачи подключенным клиентам
        private readonly Queue<CmdItem> cmdQue;    // очередь команд для обработки
        private readonly List<ClientInfo> clients; // информация о подключенных клиентах

        private Thread thread;            // поток взаимодействия с клиентами
        private volatile bool terminated; // работа потока прервана
        private TcpListener tcpListener;  // прослушиватель TCP-соединений


        /// <summary>
        /// Конструктор
        /// </summary>
        public Comm(MainLogic mainLogic)
        {
            // заполнение массива описаний команд
            CmdDescrs = new string[byte.MaxValue];
            for (byte code = 0; code < byte.MaxValue; code++)
            {
                CmdDescrs[code] = GetCmdDescr(code);
            }

            // инициализация полей
            this.mainLogic = mainLogic ?? throw new ArgumentNullException("mainLogic");
            appLog = mainLogic.AppLog ?? throw new ArgumentNullException("mainLogic.AppLog");
            settings = mainLogic.Settings ?? throw new ArgumentNullException("mainLogic.Settings");
            inBuf = new byte[InBufLenght];
            outBuf = new byte[OutBufLenght];
            cmdBuf = new List<Command>();
            cmdQue = new Queue<CmdItem>();
            clients = new List<ClientInfo>();

            thread = null;
            terminated = false;
            tcpListener = null;
        }


        /// <summary>
        /// Считать информацию из потока данных TCP-соединения
        /// </summary>
        /// <remarks>Метод используется для считывания "большого" объёма данных</remarks>
        private static int ReadNetStream(NetworkStream netStream, byte[] buffer, int offset, int size)
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
        /// Разъединаться
        /// </summary>
        private static void Disconnect(TcpClient tcpClient, NetworkStream netStream)
        {
            if (netStream != null)
                try { netStream.Close(); }
                catch { }

            if (tcpClient != null)
                try { tcpClient.Close(); }
                catch { }
        }

        /// <summary>
        /// Получить описание команды
        /// </summary>
        private static string GetCmdDescr(byte cmdCode)
        {
            string descr;

            switch (cmdCode)
            {
                case 0x01:
                    descr = Localization.UseRussian ? "проверка имени и пароля" : "check user name and password";
                    break;
                case 0x02:
                    descr = Localization.UseRussian ? "запрос состояния сервера - ping" : 
                        "request server state - ping";
                    break;
                case 0x03:
                    descr = Localization.UseRussian ? "запись текущего среза" : "write current data";
                    break;
                case 0x04:
                    descr = Localization.UseRussian ? "запись архивного среза" : "write archive data";
                    break;
                case 0x05:
                    descr = Localization.UseRussian ? "запись события" : "write event";
                    break;
                case 0x06:
                    descr = Localization.UseRussian ? "команда ТУ" : "command";
                    break;
                case 0x07:
                    descr = Localization.UseRussian ? "запрос команды ТУ" : "request command";
                    break;
                case 0x08:
                    descr = Localization.UseRussian ? "открытие и чтение из файла" : "open and read file";
                    break;
                case 0x09:
                    descr = Localization.UseRussian ? "перемещение позиции чтения из файла" : "file seek";
                    break;
                case 0x0A:
                    descr = Localization.UseRussian ? "чтение из файла" : "read file";
                    break;
                case 0x0B:
                    descr = Localization.UseRussian ? "закрытие файла" : "close file";
                    break;
                case 0x0C:
                    descr = Localization.UseRussian ? "запрос времени изменения файлов" : 
                        "request files modification time";
                    break;
                case 0x0D:
                    descr = Localization.UseRussian ? "запрос данных из таблицы среза" : 
                        "request data from snapshot table";
                    break;
                case 0x0E:
                    descr = Localization.UseRussian ? "квитирование события" : "check event";
                    break;
                default:
                    descr = "";
                    break;
            }

            return "0x" + cmdCode.ToString("X2") + (descr == "" ? "" : " (" + descr + ")");
        }


        /// <summary>
        /// Цикл взаимодействия с клиентами (метод вызывается в отдельном потоке)
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {
                Monitor.Enter(clients);
                ClientInfo client = null;

                try
                {
                    // открытие запрашиваемых соединений с клиентами
                    while (tcpListener.Pending() && !terminated)
                    {
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        tcpClient.NoDelay = true;
                        tcpClient.SendTimeout = TcpSendTimeout;
                        tcpClient.ReceiveTimeout = TcpReceiveTimeout;

                        client = new ClientInfo(tcpClient);
                        appLog.WriteAction(string.Format(Localization.UseRussian ? "Соединение с клиентом {0}" : 
                            "Connect to client {0}", client.Address), Log.ActTypes.Action);
                        clients.Add(client);

                        // подтверждение соединения - отправка версии сервера
                        client.NetStream.Write(AppVersionBuf, 0, AppVersionBuf.Length);
                    }

                    DateTime nowDT = DateTime.Now;
                    int clientInd = 0;

                    while (clientInd < clients.Count && !terminated)
                    {
                        client = clients[clientInd];

                        // приём и обработка данных от клиента
                        if (client.TcpClient.Available > 0)
                        {
                            client.ActivityDT = nowDT;
                            ReceiveData(client);
                        }

                        if ((nowDT - client.ActivityDT).TotalSeconds > InactiveTime)
                        {
                            // отключение клиента, если он не активен
                            appLog.WriteAction(string.Format(Localization.UseRussian ? "Отключение клиента {0}" :
                                "Disconnect client {0}", client.Address), Log.ActTypes.Action);
                            Disconnect(client.TcpClient, client.NetStream);
                            client.CloseFile();
                            clients.RemoveAt(clientInd);
                        }
                        else
                        {
                            // удаление устаревших команд ТУ из списка команд клиента
                            int cmdInd = 0;
                            while (cmdInd < client.CmdList.Count)
                            {
                                if ((nowDT - client.CmdList[cmdInd].CreateDT).TotalSeconds > CmdLifeTime)
                                    client.CmdList.RemoveAt(cmdInd);
                                else
                                    cmdInd++;
                            }

                            // переход к следующему клиенту
                            clientInd++;
                        }
                    }

                    // передача команд ТУ на обработку
                    ProcCommands();

                    // передача команд ТУ подключенным клиентам
                    lock (cmdBuf)
                    {
                        if (cmdBuf.Count > 0)
                        {
                            foreach (ClientInfo cl in clients)
                                if (cl.UserRoleID == BaseValues.Roles.App)
                                    cl.CmdList.AddRange(cmdBuf);
                            cmdBuf.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = client == null ? 
                        (Localization.UseRussian ? 
                            "Ошибка при взаимодействии с клиентами" : "Error communicating with clients") :
                        string.Format(Localization.UseRussian ?
                            "Ошибка при взаимодействии с клиентом {0}" : "Error communicating with the client {0}", 
                            client.Address);
                    appLog.WriteAction(s + ": " + ex.Message, Log.ActTypes.Exception);
                }
                finally
                {
                    Monitor.Exit(clients);
                }

                Thread.Sleep(10);
            }
        }
        
        /// <summary>
        /// Принять и обработать данные от клиента
        /// </summary>
        private void ReceiveData(ClientInfo client)
        {
            try
            {
                // приём длины данных и номера команды
                NetworkStream netStream = client.NetStream;
                bool formatError = true;
                int bytesRead = netStream.Read(inBuf, 0, 3);

                if (bytesRead == 3)
                {
                    int cmdLen = (int)BitConverter.ToUInt16(inBuf, 0);
                    byte cmd = inBuf[2];

                    if (cmdLen <= InBufLenght)
                    {
                        // приём оставшейся части данных
                        int dataLen = cmdLen - 3;
                        bytesRead = dataLen > 0 ? ReadNetStream(netStream, inBuf, 3, dataLen) : 0;

                        if (bytesRead == dataLen)
                        {
                            formatError = false;
                            
                            if (settings.DetailedLog || 
                                cmd == 0x06 /*команда ТУ*/ || cmd == 0x0E /*квитирование события*/)
                                appLog.WriteAction(string.Format(Localization.UseRussian ? 
                                    "Получена команда {0} от клиента {1}" : 
                                    "Command {0} is received from the client {1}",
                                    CmdDescrs[cmd], client.Address), Log.ActTypes.Action);

                            // обработка команды
                            ProcCommCommand(client, cmd, cmdLen);
                        }
                    }
                }

                if (formatError)
                {
                    appLog.WriteAction(string.Format(Localization.UseRussian ? 
                        "Некорректный формат полученных данных от клиента {0}" : 
                        "Incorrect format of the data received from the client {0}", 
                        client.Address), Log.ActTypes.Error);

                    // очистка потока: приём имеющихся данных, но не более InBufLenght
                    if (netStream.DataAvailable)
                        netStream.Read(inBuf, 0, inBuf.Length);
                }
            }
            catch (Exception ex)
            {
                appLog.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при приёме и обработке данных от клиента {0}: {1}" :
                    "Error receiving and processing data from the client {0}: {1}",
                    client.Address, ex.Message), Log.ActTypes.Exception);
            }
        }
        
        /// <summary>
        /// Обработать команду протокола обмена данными
        /// </summary>
        private void ProcCommCommand(ClientInfo client, byte cmd, int cmdLen)
        {
            bool sendResp = true;    // отправить ответ на команду
            int respDataLen = 0;     // длина данных ответа на команду
            byte[] extraData = null; // дополнительные данные ответа

            switch (cmd)
            {
                case 0x01: // проверка имени и пароля
                    int userNameLen = inBuf[3];
                    string userName = Encoding.Default.GetString(inBuf, 4, userNameLen);
                    string password = Encoding.Default.GetString(inBuf, 5 + userNameLen, inBuf[4 + userNameLen]);
                    bool pwdIsEmpty = string.IsNullOrEmpty(password);
                    int roleID;
                    bool checkOk = mainLogic.CheckUser(userName, password, out roleID);

                    if (client.Authenticated)
                    {
                        if (pwdIsEmpty)
                        {
                            string checkOkStr = checkOk ?
                                (Localization.UseRussian ? "успешно" : "success") :
                                (Localization.UseRussian ? "ошибка" : "error");
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Получение роли пользователя {0}. Результат: {1}" : 
                                "Get user {0} role. Result: {1}", userName, checkOkStr));
                        }
                        else
                        {
                            string checkOkStr = checkOk ?
                                (Localization.UseRussian ? "верно" : "passed") :
                                (Localization.UseRussian ? "неверно" : "failed");
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Проверка имени и пароля пользователя {0}. Результат: {1}" : 
                                "Check username and password for {0}. Result: {1}", userName, checkOkStr));
                        }
                    }
                    else
                    {
                        if (checkOk && roleID != BaseValues.Roles.Disabled && !pwdIsEmpty)
                        {
                            client.Authenticated = true;
                            client.UserName = userName;
                            client.UserRoleID = roleID;
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Пользователь {0} успешно аутентифицирован" : 
                                "The user {0} is successfully authenticated", userName));
                        }
                        else
                        {
                            client.ActivityDT = DateTime.MinValue; // для отключения клиента после отправки ответа
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Неудачная попытка аутентификации пользователя {0}" : 
                                "Unsuccessful attempt to authenticate the user {0}", userName));
                        }
                    }

                    respDataLen = 1;
                    outBuf[3] = (byte)roleID;
                    break;
                case 0x02: // запрос состояния сервера (ping)
                    respDataLen = 1;
                    outBuf[3] = mainLogic.ServerIsReady ? (byte)1 : (byte)0;
                    break;
                case 0x03: // запись текущего среза
                    if (client.UserRoleID == BaseValues.Roles.App)
                    {
                        int cnlCnt = BitConverter.ToUInt16(inBuf, 3);
                        SrezTableLight.Srez srez = new SrezTableLight.Srez(DateTime.MinValue, cnlCnt);

                        for (int i = 0, j = 5; i < cnlCnt; i++, j += 14)
                        {
                            srez.CnlNums[i] = (int)BitConverter.ToUInt32(inBuf, j);
                            srez.CnlData[i] = new SrezTableLight.CnlData(
                                BitConverter.ToDouble(inBuf, j + 4),
                                BitConverter.ToUInt16(inBuf, j + 12));
                        }

                        outBuf[3] = mainLogic.ProcCurData(srez) ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        outBuf[3] = 0;
                    }

                    respDataLen = 1;
                    break;
                case 0x04: // запись архивного среза
                    if (client.UserRoleID == BaseValues.Roles.App)
                    {
                        DateTime dateTime = ScadaUtils.DecodeDateTime(BitConverter.ToDouble(inBuf, 3));
                        int cnlCnt = BitConverter.ToUInt16(inBuf, 11);
                        SrezTableLight.Srez srez = new SrezTableLight.Srez(dateTime, cnlCnt);

                        for (int i = 0, j = 13; i < cnlCnt; i++, j += 14)
                        {
                            srez.CnlNums[i] = (int)BitConverter.ToUInt32(inBuf, j);
                            srez.CnlData[i] = new SrezTableLight.CnlData(
                                BitConverter.ToDouble(inBuf, j + 4),
                                BitConverter.ToUInt16(inBuf, j + 12));
                        }

                        outBuf[3] = mainLogic.ProcArcData(srez) ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        outBuf[3] = 0;
                    }

                    respDataLen = 1;
                    break;
                case 0x05: // запись события
                    if (client.UserRoleID == BaseValues.Roles.App)
                    {
                        EventTableLight.Event ev = new EventTableLight.Event();
                        ev.DateTime = ScadaUtils.DecodeDateTime(BitConverter.ToDouble(inBuf, 3));
                        ev.ObjNum = BitConverter.ToUInt16(inBuf, 11);
                        ev.KPNum = BitConverter.ToUInt16(inBuf, 13);
                        ev.ParamID = BitConverter.ToUInt16(inBuf, 15);
                        ev.CnlNum = (int)BitConverter.ToUInt32(inBuf, 17);
                        ev.OldCnlVal = BitConverter.ToDouble(inBuf, 21);
                        ev.OldCnlStat = BitConverter.ToUInt16(inBuf, 29);
                        ev.NewCnlVal = BitConverter.ToDouble(inBuf, 31);
                        ev.NewCnlStat = BitConverter.ToUInt16(inBuf, 39);
                        ev.Checked = BitConverter.ToBoolean(inBuf, 41);
                        ev.UserID = BitConverter.ToUInt16(inBuf, 42);
                        int evDescrLen = inBuf[44];
                        int evDataLen = inBuf[45 + evDescrLen];
                        ev.Descr = Encoding.Default.GetString(inBuf, 45, evDescrLen);
                        ev.Data = Encoding.Default.GetString(inBuf, 46 + evDescrLen, evDataLen);

                        outBuf[3] = mainLogic.ProcEvent(ev) ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        outBuf[3] = 0;
                    }

                    respDataLen = 1;
                    break;
                case 0x06: // команда ТУ
                    bool cmdProcOk = false; // команда обработана успешно

                    if (client.UserRoleID == BaseValues.Roles.Admin || 
                        client.UserRoleID == BaseValues.Roles.Dispatcher || client.UserRoleID == BaseValues.Roles.App)
                    {
                        int cmdUserID = BitConverter.ToUInt16(inBuf, 3);
                        byte cmdTypeID = inBuf[5];
                        int ctrlCnlNum = BitConverter.ToUInt16(inBuf, 6);
                        MainLogic.CtrlCnl ctrlCnl = mainLogic.GetCtrlCnl(ctrlCnlNum);

                        if (ctrlCnl == null)
                        {
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Команда ТУ на несуществующий канал упр. {0}, ид. польз. = {1}" :
                                "Command to nonexistent out channel {0}, user ID = {1}",
                                ctrlCnlNum, cmdUserID));
                        }
                        else
                        {
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Команда ТУ: канал упр. = {0}, ид. польз. = {1}" :
                                "Command: out channel = {0}, user ID = {1}",
                                ctrlCnlNum, cmdUserID));

                            // создание команды ТУ
                            Command ctrlCmd = new Command(cmdTypeID);
                            ctrlCmd.CmdData = new byte[BitConverter.ToUInt16(inBuf, 8)];
                            Array.Copy(inBuf, 10, ctrlCmd.CmdData, 0, ctrlCmd.CmdData.Length);
                            FillCommandProps(ctrlCmd, ctrlCnl);

                            // обработка команды ТУ
                            bool passToClients;
                            mainLogic.ProcCommand(ctrlCnl, ctrlCmd, cmdUserID, out passToClients);

                            if (passToClients)
                            {
                                // передача команды ТУ подключенным клиентам
                                ctrlCmd.PrepareCmdData();
                                foreach (ClientInfo cl in clients)
                                    if (cl.UserRoleID == BaseValues.Roles.App)
                                        cl.CmdList.Add(ctrlCmd);
                            }
                            else if (ctrlCmd.CmdNum > 0)
                            {
                                appLog.WriteAction(Localization.UseRussian ? 
                                    "Команда ТУ отменена" : 
                                    "Command is canceled");
                            }

                            cmdProcOk = true;
                        }
                    }

                    respDataLen = 1;
                    outBuf[3] = cmdProcOk ? (byte)1 : (byte)0;
                    break;
                case 0x07: // запрос команды ТУ
                    if (client.UserRoleID == BaseValues.Roles.App && client.CmdList.Count > 0)
                    {
                        Command ctrlCmd = client.CmdList[0];
                        int cmdDataLen = ctrlCmd.CmdData == null ? 0 : ctrlCmd.CmdData.Length;
                        respDataLen = 7 + cmdDataLen;
                        outBuf[3] = (byte)(cmdDataLen % 256);
                        outBuf[4] = (byte)(cmdDataLen / 256);
                        outBuf[5] = (byte)ctrlCmd.CmdTypeID;
                        outBuf[6] = (byte)(ctrlCmd.KPNum % 256);
                        outBuf[7] = (byte)(ctrlCmd.KPNum / 256);
                        outBuf[8] = (byte)(ctrlCmd.CmdNum % 256);
                        outBuf[9] = (byte)(ctrlCmd.CmdNum / 256);
                        if (cmdDataLen > 0)
                            Array.Copy(ctrlCmd.CmdData, 0, outBuf, 10, cmdDataLen);

                        // удаление команды ТУ из списка команд клиента
                        client.CmdList.RemoveAt(0);
                    }
                    else
                    {
                        respDataLen = 2;
                        outBuf[3] = 0;
                        outBuf[4] = 0;
                    }
                    break;
                case 0x08: // открытие и чтение из файла
                    int readCnt = 0;
                    bool readOk = false;

                    if (client.Authenticated)
                    {
                        client.CloseFile();

                        try { client.Dir = (Dirs)inBuf[3]; }
                        catch { client.Dir = Dirs.Cur; }

                        int fileNameLen = inBuf[4];
                        client.FileName = Encoding.Default.GetString(inBuf, 5, fileNameLen);
                        string fullFileName = GetFullFileName(client.Dir, client.FileName);
                        int count = BitConverter.ToUInt16(inBuf, 5 + fileNameLen);

                        if (settings.DetailedLog)
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Открытие файла {0}" : 
                                "Opening file {0}", fullFileName));

                        try
                        {
                            if (File.Exists(fullFileName))
                            {
                                client.FileStream = new FileStream(fullFileName,
                                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                readCnt = client.FileStream.Read(outBuf, 6, count);
                                readOk = true;
                            }
                            else
                            {
                                appLog.WriteError(string.Format(Localization.UseRussian ?
                                    "Файл {0} не найден." : 
                                    "File {0} not found.", client.FullFileNameInfo));
                            }
                        }
                        catch (Exception ex)
                        {
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при работе с файлом {0}: {1}" :
                                "Error working with the file {0}: {1}",
                                client.FullFileNameInfo, ex.Message), Log.ActTypes.Exception);
                        }
                        finally
                        {
                            if (readCnt < count)
                                client.CloseFile();
                        }
                    }

                    respDataLen = 3 + readCnt;
                    outBuf[3] = readOk ? (byte)1 : (byte)0;
                    outBuf[4] = (byte)(readCnt % 256);
                    outBuf[5] = (byte)(readCnt / 256);
                    break;
                case 0x09: // перемещение позиции чтения из файла
                    long pos = 0;
                    bool seekOk = false;

                    if (client.Authenticated && client.FileStream != null)
                    {
                        SeekOrigin origin;
                        try { origin = (SeekOrigin)inBuf[3]; }
                        catch { origin = SeekOrigin.Begin; }
                        long offset = BitConverter.ToUInt32(inBuf, 4);

                        try
                        {
                            pos = client.FileStream.Seek(offset, origin);
                            seekOk = true;
                        }
                        catch (Exception ex)
                        {
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при работе с файлом {0}: {1}" :
                                "Error working with the file {0}: {1}",
                                client.FullFileNameInfo, ex.Message), Log.ActTypes.Exception);
                        }
                    }

                    respDataLen = 5;
                    outBuf[3] = seekOk ? (byte)1 : (byte)0;
                    Array.Copy(BitConverter.GetBytes((uint)pos), 0, outBuf, 4, 4);
                    break;
                case 0x0A: // чтение из файла
                    readCnt = 0;

                    if (client.Authenticated && client.FileStream != null)
                    {
                        int count = BitConverter.ToUInt16(inBuf, 3);

                        try
                        {
                            readCnt = client.FileStream.Read(outBuf, 5, count);
                        }
                        catch (Exception ex)
                        {
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при работе с файлом {0}: {1}" : 
                                "Error working with the file {0}: {1}",
                                client.FullFileNameInfo, ex.Message), Log.ActTypes.Exception);
                        }
                        finally
                        {
                            if (readCnt < count)
                                client.CloseFile();
                        }
                    }

                    respDataLen = 2 + readCnt;
                    outBuf[3] = (byte)(readCnt % 256);
                    outBuf[4] = (byte)(readCnt / 256);
                    break;
                case 0x0B: // закрытие файла
                    if (client.Authenticated && client.FileStream != null)
                    {
                        client.CloseFile();
                        outBuf[3] = 1;
                    }
                    else
                    {
                        outBuf[3] = 0;
                    }

                    respDataLen = 1;
                    break;
                case 0x0C: // запрос времени изменения файлов
                    int fileCnt = inBuf[3];
                    outBuf[3] = inBuf[3];

                    for (int i = 0, j = 4, k = 4; i < fileCnt; i++, k += 8)
                    {
                        Dirs dir;
                        try { dir = (Dirs)inBuf[j++]; }
                        catch { dir = Dirs.Cur; }

                        int fileNameLen = inBuf[j++];
                        string fileName = Encoding.Default.GetString(inBuf, j, fileNameLen);
                        string fullFileName = GetFullFileName(dir, fileName);
                        j += fileNameLen;

                        if (settings.DetailedLog)
                            appLog.WriteAction(string.Format(Localization.UseRussian ?
                                "Получение времени изменения файла {0}" : 
                                "Obtaining the modification time of the file {0}", fullFileName));

                        double fileModTime;
                        try 
                        {
                            fileModTime = File.Exists(fullFileName) ?
                                ScadaUtils.EncodeDateTime(File.GetLastWriteTime(fullFileName)) : 0; 
                        }
                        catch
                        {
                            fileModTime = 0;
                        }

                        Array.Copy(BitConverter.GetBytes(fileModTime), 0, outBuf, k, 8);
                    }

                    respDataLen = 1 + 8 * fileCnt;
                    break;
                case 0x0D: // запрос данных из таблицы среза
                    byte srezTypeNum = inBuf[3];
                    SnapshotType srezType;
                    DateTime srezDate;

                    if (srezTypeNum == 0x01)
                    {
                        srezType = SnapshotType.Cur;
                        srezDate = DateTime.MinValue;
                    }
                    else
                    {
                        srezType = srezTypeNum == 0x02 ? SnapshotType.Hour : SnapshotType.Min;
                        srezDate = new DateTime(inBuf[4] + 2000, inBuf[5], inBuf[6]);
                    }

                    int cnlNumCnt = BitConverter.ToUInt16(inBuf, 7);
                    int[] cnlNums = new int[cnlNumCnt];

                    for (int i = 0, j = 9; i < cnlNumCnt; i++, j += 4)
                        cnlNums[i] = (int)BitConverter.ToUInt32(inBuf, j);

                    if (settings.DetailedLog)
                    {
                        string srezTypeStr;
                        if (srezType == SnapshotType.Cur)
                            srezTypeStr = Localization.UseRussian ? "текущие" : "current";
                        else if (srezType == SnapshotType.Min)
                            srezTypeStr = Localization.UseRussian ? "минутные" : "minute";
                        else
                            srezTypeStr = Localization.UseRussian ? "часовые" : "hourly";

                        appLog.WriteAction(string.Format(Localization.UseRussian ? 
                            "Запрос данных. Тип: {0}. Дата: {1}. Каналы: {2}" : 
                            "Data request. Type: {0}. Date: {1}. Channels: {2}",
                            srezTypeStr, srezDate.ToString("d", Localization.Culture), string.Join(", ", cnlNums)));
                    }

                    SrezTableLight srezTable = mainLogic.GetSnapshotTable(srezDate, srezType, cnlNums);
                    int srezCnt = srezTable == null ? 0 : srezTable.SrezList.Count;
                    outBuf[5] = (byte)(srezCnt % 256);
                    outBuf[6] = (byte)(srezCnt / 256);
                    extraData = new byte[srezCnt * (10 * cnlNumCnt + 8)];

                    for (int i = 0, j = 0; i < srezCnt; i++)
                    {
                        SrezTableLight.Srez srez = srezTable.SrezList.Values[i];
                        Array.Copy(BitConverter.GetBytes(ScadaUtils.EncodeDateTime(srez.DateTime)), 0, extraData, j, 8);
                        j += 8;

                        for (int k = 0; k < cnlNumCnt; k++)
                        {
                            SrezTable.CnlData cnlData = srez.CnlData[k];
                            Array.Copy(BitConverter.GetBytes(cnlData.Val), 0, extraData, j, 8);
                            j += 8;
                            extraData[j++] = (byte)(cnlData.Stat % 256);
                            extraData[j++] = (byte)(cnlData.Stat / 256);
                        }
                    }

                    respDataLen = 2 + extraData.Length;
                    break;
                case 0x0E: // квитирование события
                    if (client.Authenticated)
                    {
                        int evUserID = BitConverter.ToUInt16(inBuf, 3);
                        DateTime evDate = new DateTime(inBuf[5] + 2000, inBuf[6], inBuf[7]);
                        int evNum = BitConverter.ToUInt16(inBuf, 8);
                        outBuf[3] = mainLogic.CheckEvent(evDate, evNum, evUserID) ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        outBuf[3] = 0;
                    }

                    respDataLen = 1;
                    break;
            }

            // передача ответа на команду
            if (sendResp)
            {
                if (cmd == 0x0D)
                {
                    int respLen = 5 + respDataLen;
                    Array.Copy(BitConverter.GetBytes((uint)respLen), 0, outBuf, 0, 4);
                    outBuf[4] = cmd;
                    client.NetStream.Write(outBuf, 0, 7);
                }
                else
                {
                    int respLen = 3 + respDataLen;
                    Array.Copy(BitConverter.GetBytes((ushort)respLen), 0, outBuf, 0, 2);
                    outBuf[2] = cmd;
                    client.NetStream.Write(outBuf, 0, respLen);
                }

                if (extraData != null && extraData.Length > 0)
                    client.NetStream.Write(extraData, 0, extraData.Length);
            }
        }

        /// <summary>
        /// Заполнить свойства команды ТУ на основе канала управления
        /// </summary>
        private void FillCommandProps(Command cmd, MainLogic.CtrlCnl ctrlCnl)
        {
            int cmdTypeID = cmd.CmdTypeID;

            if (cmdTypeID == BaseValues.CmdTypes.Standard || cmdTypeID == BaseValues.CmdTypes.Binary)
            {
                cmd.KPNum = (ushort)ctrlCnl.KPNum;
                cmd.CmdNum = (ushort)ctrlCnl.CmdNum;

                if (cmdTypeID == BaseValues.CmdTypes.Standard && cmd.CmdData != null && cmd.CmdData.Length == 8)
                    cmd.CmdVal = BitConverter.ToDouble(cmd.CmdData, 0);
            }
            else if (cmdTypeID == BaseValues.CmdTypes.Request)
            {
                cmd.KPNum = (ushort)ctrlCnl.KPNum;
            }
        }

        /// <summary>
        /// Получить полное имя файла
        /// </summary>
        private string GetFullFileName(Dirs dir, string fileName)
        {
            fileName = ScadaUtils.CorrectDirectorySeparator(fileName);
            string sepPlusFileName = Path.DirectorySeparatorChar + fileName;

            switch (dir)
            {
                case Dirs.Cur:
                    return settings.ArcDir + "Cur" + sepPlusFileName;
                case Dirs.Hr:
                    return settings.ArcDir + "Hour" + sepPlusFileName;
                case Dirs.Min:
                    return settings.ArcDir + "Min" + sepPlusFileName;
                case Dirs.Ev:
                    return settings.ArcDir + "Events" + sepPlusFileName;
                case Dirs.BaseDAT:
                    return settings.BaseDATDir + fileName;
                case Dirs.Itf:
                    return settings.ItfDir + fileName;
                case Dirs.CurCopy:
                    return settings.ArcCopyDir + "Cur" + sepPlusFileName;
                case Dirs.HrCopy:
                    return settings.ArcCopyDir + "Hour" + sepPlusFileName;
                case Dirs.MinCopy:
                    return settings.ArcCopyDir + "Min" + sepPlusFileName;
                default: // Dirs.EvCopy
                    return settings.ArcCopyDir + "Events" + sepPlusFileName;
            }
        }

        /// <summary>
        /// Отправить команды ТУ из очереди на обработку.
        /// </summary>
        private void ProcCommands()
        {
            while (true)
            {
                CmdItem cmdItem;

                lock (cmdQue)
                {
                    if (cmdQue.Count > 0)
                        cmdItem = cmdQue.Dequeue();
                    else
                        break;
                }

                DoSendCommand(cmdItem.CtrlCnlNum, cmdItem.Cmd, cmdItem.UserID);
            }
        }

        /// <summary>
        /// Непосредственно отправить команду ТУ на обработку.
        /// </summary>
        private void DoSendCommand(int ctrlCnlNum, Command cmd, int userID)
        {
            MainLogic.CtrlCnl ctrlCnl = mainLogic.GetCtrlCnl(ctrlCnlNum);

            if (ctrlCnl == null)
            {
                appLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Команда ТУ на несуществующий канал упр. {0}, ид. польз. = {1}" :
                    "Command to nonexistent out channel {0}, user ID = {1}",
                    ctrlCnlNum, userID));
            }
            else
            {
                appLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Команда ТУ: канал упр. = {0}, ид. польз. = {1}" :
                    "Command: out channel = {0}, user ID = {1}",
                    ctrlCnlNum, userID));

                // заполнение свойств команды ТУ
                FillCommandProps(cmd, ctrlCnl);

                // обработка команды ТУ
                mainLogic.ProcCommand(ctrlCnl, cmd, userID, out bool passToClients);

                if (passToClients)
                {
                    // передача команды ТУ подключенным клиентам
                    PassCommand(cmd);
                }
                else if (cmd.CmdNum > 0)
                {
                    appLog.WriteAction(Localization.UseRussian ?
                        "Команда ТУ отменена" :
                        "Command is canceled");
                }
            }
        }


        /// <summary>
        /// Запустить взаимодействие с клиентами
        /// </summary>
        public bool Start()
        {
            try
            {
                // остановка взаимодействия
                Stop();

                // запуск прослушивателя соединений
                tcpListener = new TcpListener(IPAddress.Any, settings.TcpPort);
                tcpListener.Start();
                appLog.WriteAction(Localization.UseRussian ? "Прослушиватель соединений запущен" : 
                    "Connection listener is started", Log.ActTypes.Action);

                // запуск потока взаимодействия с клиентами
                terminated = false;
                thread = new Thread(new ThreadStart(Execute));
                thread.Start();

                return true;
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? "Ошибка при запуске взаимодействия с клиентами: " : 
                    "Error start communication with clients: ") + ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Остановить взаимодействие с клиентами
        /// </summary>
        public void Stop()
        {
            // остановка потока взаимодействия с клиентами
            try
            {
                if (thread != null)
                {
                    terminated = true;
                    if (!thread.Join(WaitForStop))
                        thread.Abort();
                    thread = null;
                }
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при остановке потока взаимодействия с клиентами: " :
                    "Error stop the thread for communication with clients: ") + ex.Message, Log.ActTypes.Exception);
            }

            // остановка прослушивателя соединений
            try
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                    appLog.WriteAction(Localization.UseRussian ? "Прослушиватель соединений остановлен" :
                        "Connection listener is stopped", Log.ActTypes.Action);
                }
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? "Ошибка при остановке прослушивателя соединений: " :
                    "Error stop connection listener: ") + ex.Message, Log.ActTypes.Exception);
            }

            // отключение всех клиентов
            try
            {
                foreach (ClientInfo client in clients)
                {
                    client.NetStream.Close();
                    client.TcpClient.Close();
                    client.CloseFile();
                }
                clients.Clear();
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? "Ошибка при отключении всех клиентов: " : 
                    "Error disconnecting all the clients: ") + ex.Message, Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Получить информацию о подключенных клиентах
        /// </summary>
        public string GetClientsInfo()
        {
            Monitor.Enter(clients);
            StringBuilder clientsInfo = new StringBuilder();

            try
            {
                string header = Localization.UseRussian ? "Подключенные клиенты" : "Connected Clients";
                clientsInfo.Append(header);
                int cnt = clients.Count;

                if (cnt > 0)
                {
                    clientsInfo.Append(" (").Append(cnt).Append(")");
                    string br = new string('-', clientsInfo.Length);
                    clientsInfo.AppendLine().AppendLine(br);

                    for (int i = 0; i < cnt; i++)
                        clientsInfo.Append((i + 1).ToString()).Append(". ").
                            AppendLine(clients[i].ToString());
                }
                else
                {
                    clientsInfo.AppendLine().
                        AppendLine(new string('-', header.Length)).
                        AppendLine(Localization.UseRussian ? "Нет" : "No");
                }
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? "Ошибка при получении информации о клиентах: " : 
                    "Error getting information about the clients: ") + ex.Message, Log.ActTypes.Exception);
            }
            finally
            {
                Monitor.Exit(clients);
            }

            return clientsInfo.ToString();
        }
        
        /// <summary>
        /// Отправить команду ТУ по заданному каналу управления
        /// </summary>
        /// <remarks>Метод вызывается серверными модулями</remarks>
        public void SendCommand(int ctrlCnlNum, Command cmd, int userID)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            lock (cmdQue)
            {
                cmdQue.Enqueue(new CmdItem
                {
                    CtrlCnlNum = ctrlCnlNum,
                    Cmd = cmd,
                    UserID = userID
                });
            }
        }

        /// <summary>
        /// Передать команду ТУ подключенным клиентам
        /// </summary>
        /// <remarks>Метод вызывается серверными модулями</remarks>
        public void PassCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            lock (cmdBuf)
            {
                cmd.PrepareCmdData();
                cmdBuf.Add(cmd);
            }
        }
    }
}
