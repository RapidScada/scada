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
 * Module   : ScadaWebCommon
 * Summary  : Web application settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2016
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Web
{
    /// <summary>
    /// Web application settings
    /// <para>Настройки веб-приложения</para>
    /// </summary>
    public class WebSettings : ISettings
    {
        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "WebSettings.xml";
        /// <summary>
        /// Мин. частота обновления данных, мс
        /// </summary>
        public const int MinDataRefrRate = 500;

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public WebSettings()
        {
            PluginFileNames = new List<string>();
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить частоту обновления данных, мс
        /// </summary>
        public int DataRefrRate { get; set; }

        /// <summary>
        /// Получить или установить количество отображаемых событий
        /// </summary>
        public int DispEventCnt { get; set; }

        /// <summary>
        /// Получить или установить разрешение команд управления
        /// </summary>
        public bool CmdEnabled { get; set; }

        /// <summary>
        /// Получить или установить необходимость ввода пароля при отправке команды
        /// </summary>
        public bool CmdPassword { get; set; }

        /// <summary>
        /// Получить или установить разрешение запоминать пользователя, вошедшего в систему
        /// </summary>
        public bool RemEnabled { get; set; }

        /// <summary>
        /// Получить или установить стартовую страницу после входа в систему
        /// </summary>
        public string StartPage { get; set; }

        /// <summary>
        /// Получить имена файлов библиотек подключенных плагинов
        /// </summary>
        public List<string> PluginFileNames { get; protected set; }


        /// <summary>
        /// Установить значения настроек по умолчанию
        /// </summary>
        protected void SetToDefault()
        {
            DataRefrRate = 1000;
            DispEventCnt = 100;
            CmdEnabled = true;
            CmdPassword = true; // TODO: перенести в базу конфигурации для каждого канала управления
            RemEnabled = false;
            StartPage = "";
            PluginFileNames.Clear();
        }


        /// <summary>
        /// Создать новый объект настроек
        /// </summary>
        public ISettings Create()
        {
            return new WebSettings();
        }

        /// <summary>
        /// Определить, равны ли заданные настройки текущим настройкам
        /// </summary>
        public bool Equals(ISettings settings)
        {
            return settings == this;
        }

        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            // установка значений по умолчанию
            SetToDefault();

            // загрузка настроек
            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument(); // обрабатываемый XML-документ
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                // загрузка общих параметров
                XmlNode paramsNode = rootElem.SelectSingleNode("CommonParams");
                if (paramsNode != null)
                {
                    XmlNodeList paramNodeList = paramsNode.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodeList)
                    {
                        string name = paramElem.GetAttribute("name").Trim();
                        string nameL = name.ToLowerInvariant();
                        string val = paramElem.GetAttribute("value");

                        try
                        {
                            if (nameL == "datarefrrate")
                                DataRefrRate = Math.Max(MinDataRefrRate, int.Parse(val));
                            else if (nameL == "dispeventcnt")
                                DispEventCnt = int.Parse(val);
                            else if (nameL == "cmdenabled")
                                CmdEnabled = bool.Parse(val);
                            else if (nameL == "cmdpassword")
                                CmdPassword = bool.Parse(val);
                            else if (nameL == "remenabled")
                                RemEnabled = bool.Parse(val);
                            else if (nameL == "startpage")
                                StartPage = val;
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }

                // загрузка имён файлов модулей
                XmlNode pluginsNode = rootElem.SelectSingleNode("Plugins");
                if (pluginsNode != null)
                {
                    XmlNodeList moduleNodeList = pluginsNode.SelectNodes("Plugin");
                    foreach (XmlElement moduleElem in moduleNodeList)
                        PluginFileNames.Add(moduleElem.GetAttribute("fileName"));
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.LoadWebSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить настройки в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            try
            {
                throw new NotImplementedException("Method not implemented.");
            }
            catch (Exception ex)
            {
                errMsg = WebPhrases.SaveWebSettingsError + ": " + ex.Message;
                return false;
            }
        }
    }
}
