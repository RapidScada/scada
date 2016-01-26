/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Modified : 2016
 * 
 * Description
 * Sending email notifications.
 */

using Scada.Comm.Devices.KpEmail;
using Scada.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using AB = Scada.Comm.Devices.AddressBook;

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

            addressBook = null;
            config = new Config();
            smtpClient = new SmtpClient();
            fatalError = false;
            state = "";
            writeState = false;
        }


        /// <summary>
        /// Загрузить адресную книгу или получить её из общих свойств линии связи
        /// </summary>
        private void LoadAddressBook()
        {
            object addrBookObj;
            if (CommonProps.TryGetValue("AddressBook", out addrBookObj))
            {
                addressBook = addrBookObj as AB.AddressBook;
            }
            else
            {
                addressBook = new AB.AddressBook();
                CommonProps.Add("AddressBook", addressBook);

                string fileName = AppDirs.ConfigDir + AB.AddressBook.DefFileName;
                if (File.Exists(fileName))
                {
                    WriteToLog(Localization.UseRussian ?
                        "Загрузка адресной книги" :
                        "Loading address book");
                    string errMsg;
                    if (!addressBook.Load(fileName, out errMsg))
                        WriteToLog(errMsg);
                }
                else
                {
                    WriteToLog(Localization.UseRussian ?
                        "Адресная книга отсутствует" :
                        "Address book is missing");
                }
            }
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
                    "Waiting for commads...";
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
        /// Преобразовать команду в список почтовых сообщений
        /// </summary>
        private bool ParseCmd(Command cmd, out List<MailMessage> messages)
        {
            messages = null;
            return false;
        }

        /// <summary>
        /// Отправить почтовые сообщения
        /// </summary>
        private bool SendMessages(List<MailMessage> messages)
        {
            try
            {
                foreach (MailMessage message in messages)
                    smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при отправке писем: " :
                    "Error sending emails: ") + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Отправить письмо
        /// </summary>
        private bool SendMail(string address, string subject, string text)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(config.User, "СП-мания"); // !!!
                message.To.Add(address);
                message.Subject = subject;
                message.Body = text;

                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ? 
                    "Ошибка при отправке письма: " :
                    "Error sending mail: ") + ex.Message);
                return false;
            }
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

            Thread.Sleep(ReqParams.Delay);
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
                    List<MailMessage> messages;
                    if (ParseCmd(cmd, out messages))
                    {
                        if (SendMessages(messages))
                            lastCommSucc = true;
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
            LoadAddressBook();
            LoadConfig();
            InitSnmpClient();
        }
    }
}