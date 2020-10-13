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
 * Module   : KpHttpNotif
 * Summary  : Device driver communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.HttpNotif;
using Scada.Comm.Devices.HttpNotif.Config;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver communication logic.
    /// <para>Логика работы драйвера КП.</para>
    /// </summary>
    public class KpHttpNotifLogic : KPLogic
    {
        /// <summary>
        /// Состояния опроса КП
        /// </summary>
        private enum SessStates
        {
            /// <summary>
            /// Фатальная ошибка
            /// </summary>
            FatalError,
            /// <summary>
            /// Ожидание команд
            /// </summary>
            Waiting
        }

        /// <summary>
        /// Отправляемое уведомление
        /// </summary>
        private class Notification
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Notification()
            {
                PhoneNumbers = new List<string>();
                Emails = new List<string>();
                Text = "";
            }

            /// <summary>
            /// Получить телефонные номера
            /// </summary>
            public List<string> PhoneNumbers { get; private set; }
            /// <summary>
            /// Получить адреса эл. почты
            /// </summary>
            public List<string> Emails { get; private set; }
            /// <summary>
            /// Получить или установить текст
            /// </summary>
            public string Text { get; set; }
        }


        // Имена переменных в командной строке для формирования запроса
        private const string PhoneVarName = "phone";
        private const string EmailVarName = "email";
        private const string TextVarName = "text";
        // Разделитель адресов в запросе
        private const string ReqAddrSep = ";";
        // Разделитель адресов в команде ТУ
        private const string CmdAddrSep = ";";
        // Длина буфера ответа на запрос
        private const int RespBufLen = 100;
        // Кодировка ответа на запрос
        private static readonly Encoding RespEncoding = Encoding.UTF8;

        /// <summary>
        /// The index of the notification counter tag.
        /// </summary>
        private const int NotifCounterTagIndex = 0;
        /// <summary>
        /// The displayed lenght of a response content.
        /// </summary>
        private const int ResponseDisplayLenght = 100;

        private readonly Stopwatch stopwatch; // measures the time of operations
        private DeviceConfig deviceConfig;    // the device configuration
        private AddressBook addressBook;      // the address book shared for the communication line
        private ParamString requestUri;       // the parametrized request URI
        private ParamString requestContent;   // the parametrized request content
        private HttpClient httpClient;        // sends HTTP requests
        private bool isReady;                 // indicates that the device is ready to send requests

        private SessStates sessState;         // состояние опроса КП
        private bool writeSessState;          // вывести состояние опроса КП
        private ParamString reqTemplate;      // шаблон запроса
        private char[] respBuf;               // буфер ответа на запрос


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpHttpNotifLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;

            stopwatch = new Stopwatch();
            deviceConfig = null;
            addressBook = null;
            requestUri = null;
            requestContent = null;
            httpClient = null;
            isReady = false;

            sessState = SessStates.Waiting;
            writeSessState = true;
            reqTemplate = null;
            respBuf = new char[RespBufLen];

            InitKPTags(new List<KPTag>()
            {
                new KPTag(1, Localization.UseRussian ? "Отправлено уведомлений" : "Sent notifications")
            });
        }


        /// <summary>
        /// Создать шаблон запроса
        /// </summary>
        private void CreateReqTemplate()
        {
            try
            {
                if (ReqParams.CmdLine == "")
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Командная строка пуста." :
                        "The command line is empty.");
                }
                else
                {
                    reqTemplate = new ParamString(ReqParams.CmdLine);
                    ValidateReqTemplate();
                    sessState = SessStates.Waiting;
                }
            }
            catch (Exception ex)
            {
                sessState = SessStates.FatalError;
                reqTemplate = null;
                WriteToLog((Localization.UseRussian ?
                    "Не удалось получить HTTP-запрос из командной строки КП: " :
                    "Unable to get HTTP request from the device command line: ") + ex.Message);
            }

            writeSessState = true;
        }

        /// <summary>
        /// Проверить шаблон запроса
        /// </summary>
        private void ValidateReqTemplate()
        {
            string reqUrl = reqTemplate.ToString();
            WebRequest req = WebRequest.Create(reqUrl);

            if (!(req is HttpWebRequest))
            {
                throw new ScadaException(Localization.UseRussian ?
                    "Некорректный HTTP-запрос." :
                    "Incorrect HTTP request.");
            }
        }

        /// <summary>
        /// Преобразовать состояние опроса КП в строку
        /// </summary>
        private string SessStateToStr()
        {
            switch (sessState)
            {
                case SessStates.FatalError:
                    return Localization.UseRussian ?
                        "Отправка уведомлений невозможна" :
                        "Sending notifocations is impossible";
                case SessStates.Waiting:
                    return Localization.UseRussian ?
                        "Ожидание команд..." :
                        "Waiting for commands...";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Попытаться получить уведомление из команды ТУ
        /// </summary>
        private bool TryGetNotif(Command cmd, out Notification notif)
        {
            string cmdDataStr = cmd.GetCmdDataStr();
            int sepInd = cmdDataStr.IndexOf(CmdAddrSep);

            if (sepInd >= 0)
            {
                string recipient = cmdDataStr.Substring(0, sepInd);
                string text = cmdDataStr.Substring(sepInd + 1);

                notif = new Notification() { Text = text };

                if (addressBook == null)
                {
                    // добавление данных получателя, явно указанных в команде ТУ
                    notif.PhoneNumbers.Add(recipient);
                    notif.Emails.Add(recipient);
                }
                else
                {
                    // поиск адресов получателей в адресной книге
                    AB.AddressBook.ContactGroup contactGroup = addressBook.FindContactGroup(recipient);
                    if (contactGroup == null)
                    {
                        AB.AddressBook.Contact contact = addressBook.FindContact(recipient);
                        if (contact == null)
                        {
                            // добавление данных получателя, явно указанных в команде ТУ
                            notif.PhoneNumbers.Add(recipient);
                            notif.Emails.Add(recipient);
                        }
                        else
                        {
                            // добавление данных получателя из контакта
                            notif.PhoneNumbers.AddRange(contact.PhoneNumbers);
                            notif.Emails.AddRange(contact.Emails);
                        }
                    }
                    else
                    {
                        // добавление данных получателей из группы контактов
                        foreach (AB.AddressBook.Contact contact in contactGroup.Contacts)
                        {
                            notif.PhoneNumbers.AddRange(contact.PhoneNumbers);
                            notif.Emails.AddRange(contact.Emails);
                        }
                    }
                }

                return true;
            }
            else
            {
                notif = null;
                return false;
            }
        }

        /// <summary>
        /// Отправить уведомление
        /// </summary>
        private bool SendNotif(Notification notif)
        {
            try
            {
                // получение адреса запроса
                // использовать WebUtility.UrlEncode в .NET 4.5
                reqTemplate.SetParam(PhoneVarName, Uri.EscapeDataString(string.Join(ReqAddrSep, notif.PhoneNumbers)));
                reqTemplate.SetParam(EmailVarName, Uri.EscapeDataString(string.Join(ReqAddrSep, notif.Emails)));
                reqTemplate.SetParam(TextVarName, Uri.EscapeDataString(notif.Text));
                string reqUrl = reqTemplate.ToString();

                // создание запроса
                WebRequest req = WebRequest.Create(reqUrl);
                req.Timeout = ReqParams.Timeout;

                // отправка запроса и приём ответа
                WriteToLog(Localization.UseRussian ?
                    "Отправка уведомления. Запрос: " :
                    "Send notification. Request: ");
                WriteToLog(reqUrl);

                using (WebResponse resp = req.GetResponse())
                {
                    using (Stream respStream = resp.GetResponseStream())
                    {
                        // чтение данных из ответа
                        using (StreamReader reader = new StreamReader(respStream, RespEncoding))
                        {
                            int readCnt = reader.Read(respBuf, 0, RespBufLen);

                            if (readCnt > 0)
                            {
                                string respString = new string(respBuf, 0, readCnt);
                                WriteToLog(Localization.UseRussian ?
                                    "Полученный ответ:" :
                                    "Received response:");
                                WriteToLog(respString);
                            }
                            else
                            {
                                WriteToLog(Localization.UseRussian ?
                                    "Ответ не получен" :
                                    "No response");
                            }
                        }
                    }
                }

                IncCurData(0, 1); // увеличение счётчика уведомлений
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при отправке уведомления: " :
                    "Error sending notification: ") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0)
            {
                if (signal == 1)
                    return tagData.Val.ToString("N0");
            }

            return base.ConvertTagDataToStr(signal, tagData);
        }


        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags()
        {

        }

        /// <summary>
        /// Writes the ready flag to the log.
        /// </summary>
        private void WriteReadyFlag()
        {
            if (isReady)
            {
                WriteToLog(Localization.UseRussian ?
                    "Ожидание команд..." :
                    "Waiting for commands...");
            }
            else
            {
                WriteToLog(Localization.UseRussian ?
                    "Отправка уведомлений невозможна" :
                    "Sending notifocations is impossible");
            }
        }

        /// <summary>
        /// Sends a notification with the specified arguments.
        /// </summary>
        private bool SendNotification(List<KeyValuePair<string, string>> args)
        {
            try
            {
                // initialize the HTTP client
                if (httpClient == null)
                {
                    httpClient = new HttpClient();

                    foreach (Header header in deviceConfig.Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }
                }

                // create request
                requestUri.ResetParams();
                requestContent.ResetParams();

                foreach (var arg in args)
                {
                    requestUri.SetParam(arg.Key, arg.Value, EscapingMethod.EncodeUrl);
                    requestContent.SetParam(arg.Key, arg.Value, deviceConfig.ContentEscaping);
                }

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = deviceConfig.Method == RequestMethod.Post ? HttpMethod.Post : HttpMethod.Get,
                    RequestUri = new Uri(requestUri.ToString()),
                    Content = new StringContent(requestContent.ToString(), Encoding.UTF8, deviceConfig.ContentType)
                };

                // send request and receive response
                stopwatch.Restart();
                HttpResponseMessage response = httpClient.SendAsync(request).Result;
                HttpStatusCode responseStatus = response.StatusCode;
                string responseContent = response.Content.ReadAsStringAsync().Result;
                stopwatch.Stop();

                // output response to log
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ответ получен за {0} мс. Статус: {1} ({2})" :
                    "Response received in {0} ms. Status: {1} ({2})",
                    stopwatch.ElapsedMilliseconds, (int)responseStatus, responseStatus));

                if (responseContent.Length > 0)
                {
                    WriteToLog(Localization.UseRussian ?
                        "Содержимое ответа:" :
                        "Response content:");
                    WriteToLog(responseContent.Length <= ResponseDisplayLenght ?
                        responseContent :
                        responseContent.Substring(0, ResponseDisplayLenght));
                }

                IncCurData(NotifCounterTagIndex, 1); // increase notification counter
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при отправке уведомления: " :
                    "Error sending notification: ") + ex);
                return false;
            }
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected override string ConvertTagDataToStr(KPTag kpTag, SrezTableLight.CnlData tagData)
        {
            return base.ConvertTagDataToStr(kpTag, tagData);
        }


        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (writeSessState)
            {
                writeSessState = false;
                WriteToLog("");
                WriteToLog(SessStateToStr());
            }

            Thread.Sleep(ScadaUtils.ThreadDelay);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            lastCommSucc = false;

            if (sessState == SessStates.FatalError)
            {
                WriteToLog(SessStateToStr());
            }
            else
            {
                if (cmd.CmdNum == 1 && cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
                {
                    Notification notif;
                    if (TryGetNotif(cmd, out notif))
                    {
                        if (SendNotif(notif))
                            lastCommSucc = true;

                        // задержка позволяет ограничить скорость отправки уведомлений
                        Thread.Sleep(ReqParams.Delay);
                    }
                    else
                    {
                        WriteToLog(CommPhrases.IncorrectCmdData);
                    }
                }
                else
                {
                    WriteToLog(CommPhrases.IllegalCommand);
                }

                writeSessState = true;
            }

            CalcCmdStats();
        }

        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            // load device configuration
            isReady = false;
            deviceConfig = new DeviceConfig();
            string fileName = DeviceConfig.GetFileName(AppDirs.ConfigDir, Number);
            string errMsg;

            if (File.Exists(fileName))
            {
                if (!deviceConfig.Load(fileName, out errMsg))
                    WriteToLog(errMsg);
            }
            else
            {
                // get URI from command line for backward compatibility
                deviceConfig.Uri = ReqParams.CmdLine;
            }

            // initialize variables if the configuration is valid
            if (deviceConfig.Validate(out errMsg))
            {
                requestUri = new ParamString(deviceConfig.Uri);
                requestContent = new ParamString(deviceConfig.Content);

                addressBook = AbUtils.GetAddressBook(AppDirs.ConfigDir, CommonProps, WriteToLog);
                SetCurData(NotifCounterTagIndex, 0, 1); // reset notification counter
                isReady = true;
            }
            else
            {
                WriteToLog(errMsg);
            }

            if (!isReady)
                WriteToLog(CommPhrases.NormalKpExecImpossible);

            /*
            // получение адресной книги
            addressBook = AbUtils.GetAddressBook(AppDirs.ConfigDir, CommonProps, WriteToLog);
            // создание шаблона запроса
            CreateReqTemplate();
            // сброс счётчика уведомлений
            SetCurData(0, 0, 1);
            // установка состояния работы КП
            WorkState = sessState == SessStates.FatalError ? WorkStates.Error : WorkStates.Normal;
            */
        }
    }
}
