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
 * Module   : KpEmail
 * Summary  : Mail server connection configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System;
using System.Xml;

namespace Scada.Comm.Devices.KpEmail
{
    /// <summary>
    /// Mail server connection configuration.
    /// <para>Конфигурация соединения с почтовым сервером.</para>
    /// </summary>
    internal class KpConfig
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public KpConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить имя или IP-адрес сервера
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Получить или установить порт
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Получить или установить пользователя
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Получить или установить пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Получить или установить признак использования SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// Gets or sets the display name of the sender.
        /// </summary>
        public string SenderDisplayName { get; set; }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            // Gmail: smtp.gmail.com, 587
            // Yandex: smtp.yandex.ru, 25
            Host = "smtp.gmail.com";
            Port = 587;
            User = "example@gmail.com";
            Password = "";
            EnableSsl = true;
            SenderAddress = "example@gmail.com";
            SenderDisplayName = "Rapid SCADA";
        }


        /// <summary>
        /// Загрузить конфигурацию из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            SetToDefault();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlElement rootElem = xmlDoc.DocumentElement;
                Host = rootElem.GetChildAsString("Host");
                Port = rootElem.GetChildAsInt("Port");
                User = rootElem.GetChildAsString("User");
                Password = ScadaUtils.Decrypt(rootElem.GetChildAsString("Password"));
                EnableSsl = rootElem.GetChildAsBool("EnableSsl");
                SenderAddress = rootElem.GetChildAsString("SenderAddress", User);
                string userDisplayName = rootElem.GetChildAsString("UserDisplayName"); // for backward compatibility
                SenderDisplayName = rootElem.GetChildAsString("SenderDisplayName", userDisplayName);

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить конфигурацию в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("KpEmailConfig");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("Host", Host);
                rootElem.AppendElem("Port", Port);
                rootElem.AppendElem("User", User);
                rootElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
                rootElem.AppendElem("EnableSsl", EnableSsl);
                rootElem.AppendElem("SenderAddress", SenderAddress);
                rootElem.AppendElem("SenderDisplayName", SenderDisplayName);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Получить имя файла конфигурации
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return configDir + "KpEmail_" + CommUtils.AddZeros(kpNum, 3) + ".xml";
        }
    }
}
