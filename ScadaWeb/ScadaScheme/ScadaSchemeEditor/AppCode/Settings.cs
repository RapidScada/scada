/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : Scheme Editor
 * Summary  : Application settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using System;
using System.IO;
using System.Xml;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Application settings.
    /// <para>Настройки приложения.</para>
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// Браузеры для редактирования схемы.
        /// </summary>
        public enum Browsers
        {
            Default,
            Chrome,
            Firefox
        }

        /// <summary>
        /// Имя файла настроек по умолчанию.
        /// </summary>
        public const string DefFileName = "ScadaSchemeEditorConfig.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Settings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить директорию веб-приложения.
        /// </summary>
        public string WebDir { get; set; }

        /// <summary>
        /// Получить или установить браузер.
        /// </summary>
        public Browsers Browser { get; set; }


        /// <summary>
        /// Установить настройки приложения по умолчанию.
        /// </summary>
        private void SetToDefault()
        {
            WebDir = ScadaUtils.IsRunningOnWin ? 
                @"C:\SCADA\ScadaWeb\" : 
                "/opt/scada/ScadaWeb/";
            Browser = Browsers.Default;
        }

        /// <summary>
        /// Загрузить настройки приложения из файла.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNodeList paramNodes = xmlDoc.DocumentElement.SelectNodes("Param");

                foreach (XmlElement paramElem in paramNodes)
                {
                    string name = paramElem.GetAttribute("name");
                    string nameL = name.ToLowerInvariant();
                    string val = paramElem.GetAttribute("value");

                    try
                    {
                        if (nameL == "webdir")
                            WebDir = ScadaUtils.NormalDir(val);
                        else if (nameL == "browser")
                            Browser = (Browsers)Enum.Parse(typeof(Browsers), val, true);
                    }
                    catch
                    {
                        throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
        
        /// <summary>
        /// Сохранить настройки приложения в файле.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                // формирование XML-документа
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaSchemeEditorConfig");
                xmlDoc.AppendChild(rootElem);
                rootElem.AppendParamElem("WebDir", WebDir, 
                    "Директория веб-приложения", "Web application directory");
                rootElem.AppendParamElem("Browser", Browser,
                    "Браузер для редактирования схемы", "Browser for scheme editing");

                // сохранение в файле
                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
    }
}
