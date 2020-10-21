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
        /// Specifies the tag indices.
        /// </summary>
        private static class TagIndex
        {
            public const int NotifCounter = 0;
            public const int ResponseStatus = 1;
        }

        /// <summary>
        /// Specifies the predefined parameter names.
        /// </summary>
        private static class ParamName
        {
            public const string Address = "address";
            public const string Phone = "phone";
            public const string Email = "email";
            public const string Text = "text";
        }


        /// <summary>
        /// The parameter separator of the 1st command.
        /// </summary>
        private const char CmdSep = ';';
        /// <summary>
        /// The separator for parts of an address.
        /// </summary>
        private const string AddrSep = ";";
        /// <summary>
        /// The displayed lenght of a response content.
        /// </summary>
        private const int ResponseDisplayLenght = 100;

        private readonly Stopwatch stopwatch; // measures the time of operations
        private DeviceConfig deviceConfig;    // the device configuration
        private AddressBook addressBook;      // the address book shared for the communication line
        private ParamString paramUri;         // the parametrized request URI
        private ParamString paramContent;     // the parametrized request content
        private HttpClient httpClient;        // sends HTTP requests
        private bool isReady;                 // indicates that the device is ready to send requests
        private bool flagLoggingRequired;     // logging of the ready flag is required


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
            paramUri = null;
            paramContent = null;
            httpClient = null;
            isReady = false;
            flagLoggingRequired = false;

            InitDeviceTags();
        }


        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags()
        {
            if (Localization.UseRussian)
            {
                InitKPTags(new List<KPTag>()
                {
                    new KPTag(1, "Отправлено уведомлений"),
                    new KPTag(2, "Статус ответа")
                });
            }
            else
            {
                InitKPTags(new List<KPTag>()
                {
                    new KPTag(1, "Notifications sent"),
                    new KPTag(2, "Response status")
                });
            }
        }

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        private bool ValidateConfig(DeviceConfig deviceConfig, out string errMsg)
        {
            if (string.IsNullOrEmpty(deviceConfig.Uri))
            {
                errMsg = string.Format(Localization.UseRussian ? 
                    "Ошибка: {0}: URI не может быть пустым." :
                    "Error: {0}: URI must not be empty.", Caption);
                return false;
            }
            else
            {
                try
                {
                    Uri uri = new Uri(deviceConfig.Uri);
                }
                catch
                {
                    errMsg = string.Format(Localization.UseRussian ?
                        "Ошибка: {0}: некорректный URI" :
                        "Error: {0}: invalid URI.", Caption);
                    return false;
                }
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Writes the ready flag to the log.
        /// </summary>
        private void WriteReadyFlag()
        {
            if (isReady)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} ожидает команд..." :
                    "{0} is waiting for commands...", Caption));
            }
            else
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка: {0} не может отправлять уведомления" :
                    "Error: {0} unable to send notifications", Caption));
            }
        }

        /// <summary>
        /// Gets notification arguments from the command.
        /// </summary>
        private bool GetArguments(Command cmd, out Dictionary<string, string> args)
        {
            if (cmd.CmdNum == 1)
            {
                string cmdDataStr = cmd.GetCmdDataStr();
                int sepInd = cmdDataStr.IndexOf(CmdSep);

                if (sepInd >= 0)
                {
                    args = new Dictionary<string, string>
                    {
                        { ParamName.Address, cmdDataStr.Substring(0, sepInd) },
                        { ParamName.Text, cmdDataStr.Substring(sepInd + 1) }
                    };
                }
                else
                {
                    args = null;
                }
            }
            else
            {
                args = cmd.GetCmdDataArgs();
            }

            if (args == null)
            {
                return false;
            }
            else
            {
                AddContactDetails(args);
                return true;
            }
        }

        /// <summary>
        /// Adds the contact phones and emails.
        /// </summary>
        private void AddContactDetails(Dictionary<string, string> args)
        {
            if (args.TryGetValue(ParamName.Address, out string address) &&
                !(args.ContainsKey(ParamName.Phone) && args.ContainsKey(ParamName.Email)))
            {
                List<string> phoneNumbers = new List<string>();
                List<string> emails = new List<string>();

                if (addressBook == null)
                {
                    // add the known address as phone number and email
                    phoneNumbers.Add(address);
                    emails.Add(address);
                }
                else
                {
                    // search in the address book
                    if (addressBook.FindContactGroup(address) is AddressBook.ContactGroup contactGroup)
                    {
                        // add all contacts from the group
                        foreach (AddressBook.Contact contact in contactGroup.Contacts)
                        {
                            phoneNumbers.AddRange(contact.PhoneNumbers);
                            emails.AddRange(contact.Emails);
                        }
                    }
                    else if (addressBook.FindContact(address) is AddressBook.Contact contact)
                    {
                        // add the contact phone numbers and emails
                        phoneNumbers.AddRange(contact.PhoneNumbers);
                        emails.AddRange(contact.Emails);
                    }
                    else
                    {
                        // add the known address as phone number and email
                        phoneNumbers.Add(address);
                        emails.Add(address);
                    }
                }

                if (!args.ContainsKey(ParamName.Phone))
                    args.Add(ParamName.Phone, string.Join(AddrSep, phoneNumbers));

                if (!args.ContainsKey(ParamName.Email))
                    args.Add(ParamName.Email, string.Join(AddrSep, emails));
            }
        }

        /// <summary>
        /// Sends a notification with the specified arguments.
        /// </summary>
        private bool SendNotification(Dictionary<string, string> args)
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
                paramUri?.ResetParams(args, EscapingMethod.EncodeUrl);
                paramContent?.ResetParams(args, deviceConfig.ContentEscaping);

                string uri = paramUri == null ? deviceConfig.Uri : paramUri.ToString();
                string content = paramContent == null ? deviceConfig.Content : paramContent.ToString();

                HttpRequestMessage request = new HttpRequestMessage(
                    deviceConfig.Method == RequestMethod.Post ? HttpMethod.Post : HttpMethod.Get, 
                    uri);

                if (deviceConfig.Method == RequestMethod.Post)
                {
                    request.Content = string.IsNullOrEmpty(deviceConfig.ContentType) ?
                        new StringContent(content, Encoding.UTF8) :
                        new StringContent(content, Encoding.UTF8, deviceConfig.ContentType);
                }

                // send request and receive response
                WriteToLog(Localization.UseRussian ?
                    "Отправка запроса:" :
                    "Send request:");
                WriteToLog(request.RequestUri.ToString());

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

                    if (responseContent.Length <= ResponseDisplayLenght)
                    {
                        WriteToLog(responseContent);
                    }
                    else
                    {
                        WriteToLog(responseContent.Substring(0, ResponseDisplayLenght));
                        WriteToLog("...");
                    }
                }

                // update tag values
                IncCurData(TagIndex.NotifCounter, 1);
                SetCurData(TagIndex.ResponseStatus, (int)responseStatus, 1);

                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при отправке уведомления: " :
                    "Error sending notification: ") + ex);
                InvalidateCurData(TagIndex.ResponseStatus, 1);
                return false;
            }
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected override string ConvertTagDataToStr(KPTag kpTag, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0)
            {
                if (kpTag.Index == TagIndex.NotifCounter)
                    return tagData.Val.ToString("N0");
                else if (kpTag.Index == TagIndex.ResponseStatus)
                    return tagData.Val.ToString("N0") + " (" + (HttpStatusCode)tagData.Val + ")";
            }

            return base.ConvertTagDataToStr(kpTag, tagData);
        }


        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (flagLoggingRequired)
            {
                flagLoggingRequired = false;
                WriteToLog("");
                WriteReadyFlag();
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

            if (isReady)
            {
                if ((cmd.CmdNum == 1 || cmd.CmdNum == 2) && cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
                {
                    if (GetArguments(cmd, out Dictionary<string, string> args))
                    {
                        int tryNum = 0;

                        while (RequestNeeded(ref tryNum))
                        {
                            lastCommSucc = SendNotification(args);
                            FinishRequest();
                            tryNum++;
                        }
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

                flagLoggingRequired = true;
            }
            else
            {
                WriteReadyFlag();
            }

            CalcCmdStats();
        }

        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            isReady = false;
            flagLoggingRequired = false;

            // load device configuration
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
            if (ValidateConfig(deviceConfig, out errMsg))
            {
                if (deviceConfig.ParamEnabled)
                {
                    paramUri = new ParamString(deviceConfig.Uri, deviceConfig.ParamBegin, deviceConfig.ParamEnd);
                    paramContent = new ParamString(deviceConfig.Content, deviceConfig.ParamBegin, deviceConfig.ParamEnd);
                }
                else
                {
                    paramUri = null;
                    paramContent = null;
                }

                addressBook = AbUtils.GetAddressBook(AppDirs.ConfigDir, CommonProps, WriteToLog);
                SetCurData(TagIndex.NotifCounter, 0, 1); // reset notification counter
                isReady = true;
                flagLoggingRequired = true;
            }
            else
            {
                WriteToLog(errMsg);
            }
        }
    }
}
