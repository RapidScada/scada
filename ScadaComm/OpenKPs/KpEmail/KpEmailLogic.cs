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
 * Module   : KpEmail
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 * 
 * Description
 * Sending email notifications.
 */

using Scada.Comm.Devices.AB;
using Scada.Comm.Devices.KpEmail;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public class KpEmailLogic : KPLogic
    {
        private AB.AddressBook addressBook; // адресная книга, общая для линии связи
        private Config config;              // конфигурация соединения с почтовым сервером
        private SmtpClient smtpClient;      // клиент SMTP
        private bool fatalError;            // фатальная ошибка при инициализации КП
        private string state;               // состояние КП
        private bool writeState;            // вывести состояние КП


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpEmailLogic(int number)
            : base(number)
        {
            CanSendCmd = true;
            ConnRequired = false;
            WorkState = WorkStates.Normal;

            addressBook = null;
            config = new Config();
            smtpClient = new SmtpClient();
            fatalError = false;
            state = "";
            writeState = false;

            InitKPTags(new List<KPTag>()
            {
                new KPTag(1, Localization.UseRussian ? "Отправлено писем" : "Sent emails")
            });
        }


        /// <summary>
        /// Загрузить конфигурацию соединения с почтовым сервером
        /// </summary>
        private void LoadConfig()
        {
            string errMsg;
            fatalError = !config.Load(Config.GetFileName(AppDirs.ConfigDir, Number), out errMsg);

            if (fatalError)
            {
                state = Localization.UseRussian ? 
                    "Отправка уведомлений невозможна" : 
                    "Sending notifocations is impossible";
                throw new Exception(errMsg);
            }
            else
            {
                state = Localization.UseRussian ? 
                    "Ожидание команд..." :
                    "Waiting for commands...";
            }
        }

        /// <summary>
        /// Инициализировать клиент SMTP на основе конфигурации соединения
        /// </summary>
        private void InitSnmpClient()
        {
            smtpClient.Host = config.Host;
            smtpClient.Port = config.Port;
            smtpClient.Credentials = new NetworkCredential(config.User, config.Password);
            smtpClient.Timeout = ReqParams.Timeout;
            smtpClient.EnableSsl = config.EnableSsl;
        }

        /// <summary>
        /// Попытаться получить почтовое сообщение из команды ТУ
        /// </summary>
        private bool TryGetMessage(Command cmd, out MailMessage message)
        {
            string cmdDataStr = cmd.GetCmdDataStr();
            int ind1 = cmdDataStr.IndexOf(';');
            int ind2 = ind1 >= 0 ? cmdDataStr.IndexOf(';', ind1 + 1) : -1;

            if (ind1 >= 0 && ind2 >= 0)
            {
                string recipient = cmdDataStr.Substring(0, ind1);
                string subject = cmdDataStr.Substring(ind1 + 1, ind2 - ind1 - 1);
                string text = cmdDataStr.Substring(ind2 + 1);

                List<string> addresses = new List<string>();
                if (addressBook == null)
                {
                    // добавление адреса получателя из данных команды
                    addresses.Add(recipient);
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
                            // добавление адреса получателя из данных команды
                            addresses.Add(recipient);
                        }
                        else
                        {
                            // добавление адреса получателя из контакта
                            addresses.AddRange(contact.Emails);
                        }
                    }
                    else
                    {
                        // добавление адресов получателей из группы контактов
                        foreach (AB.AddressBook.Contact contact in contactGroup.Contacts)
                            addresses.AddRange(contact.Emails);
                    }
                }

                // создание сообщения
                message = CreateMessage(addresses, subject, text);
                return message != null;
            }
            else
            {
                message = null;
                return false;
            }
        }

        /// <summary>
        /// Создать почтовое сообщение
        /// </summary>
        private MailMessage CreateMessage(List<string> addresses, string subject, string text)
        {
            MailMessage message = new MailMessage();

            try
            {
                message.From = new MailAddress(config.User, config.UserDisplayName);
            }
            catch
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Некорректный адрес отправителя {0}" :
                    "Incorrect sender address {0}", config.User));
                return null;
            }

            foreach (string address in addresses)
            {
                try
                {
                    message.To.Add(new MailAddress(address));
                }
                catch
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Некорректный адрес получателя {0}" :
                        "Incorrect recipient address {0}", address));
                }
            }

            if (message.To.Count > 0)
            {
                message.Subject = subject;
                message.Body = text;
                return message;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Отправить почтовое сообщение
        /// </summary>
        private bool SendMessage(MailMessage message)
        {
            try
            {
                smtpClient.Send(message);
                SetCurData(0, curData[0].Val + 1, 1);

                WriteToLog(string.Format(Localization.UseRussian ?
                    "Письмо отправлено на {0}" :
                    "Email has been sent to {0}", message.To.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка при отправке письма на {0}: {1}" :
                    "Error sending email to {0}: {1}", message.To.ToString(), ex.Message));
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
            if (writeState)
            {
                WriteToLog("");
                WriteToLog(state);
                writeState = false;
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

            if (fatalError)
            {
                WriteToLog(state);
            }
            else
            {
                if (cmd.CmdNum == 1 && cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
                {
                    MailMessage message;
                    if (TryGetMessage(cmd, out message))
                    {
                        if (SendMessage(message))
                            lastCommSucc = true;

                        // задержка позволяет ограничить скорость отправки писем
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

                writeState = true;
            }

            CalcCmdStats();
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            writeState = true;
            addressBook = AbUtils.GetAddressBook(AppDirs.ConfigDir, CommonProps, WriteToLog);
            LoadConfig();
            InitSnmpClient();
            SetCurData(0, 0, 1);
        }
    }
}