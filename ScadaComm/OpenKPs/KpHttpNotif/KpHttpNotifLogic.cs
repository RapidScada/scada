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
 * Module   : KpHttpNotif
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.HttpNotif;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
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

        private AB.AddressBook addressBook; // адресная книга, общая для линии связи
        private SessStates sessState;       // состояние опроса КП
        private bool writeSessState;        // вывести состояние опроса КП
        private ParamString reqTemplate;    // шаблон запроса
        private char[] respBuf;             // буфер ответа на запрос


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpHttpNotifLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;

            addressBook = null;
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
        /// Выполнить сеанс опроса КП
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
        /// Отправить команду ТУ
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
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // получение адресной книги
            addressBook = AbUtils.GetAddressBook(AppDirs.ConfigDir, CommonProps, WriteToLog);
            // создание шаблона запроса
            CreateReqTemplate();
            // сброс счётчика уведомлений
            SetCurData(0, 0, 1);
            // установка состояния работы КП
            WorkState = sessState == SessStates.FatalError ? WorkStates.Error : WorkStates.Normal;
        }
    }
}
