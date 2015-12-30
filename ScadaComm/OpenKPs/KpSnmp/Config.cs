/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : KpSnmp
 * Summary  : Device communication configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Device communication configuration
    /// <para>Конфигурация связи с КП</para>
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// Группа переменных
        /// </summary>
        public class Group
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Group()
            {
                Name = "";
                Variables = new List<Variable>();
            }

            /// <summary>
            /// Получить или установить имя группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить переменные
            /// </summary>
            public List<Variable> Variables { get; private set; }
        }

        /// <summary>
        /// Переменная
        /// </summary>
        public class Variable
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Variable()
            {
                Name = "";
                OID = "";
            }

            /// <summary>
            /// Получить или установить имя переменной
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить идентификатор переменной
            /// </summary>
            public string OID { get; set; }
        }

        /// <summary>
        /// Пароль на чтение данных по умоланию
        /// </summary>
        public const string DefReadCommunity = "public";
        /// <summary>
        /// Пароль на запись данных по умоланию
        /// </summary>
        public const string DefWriteCommunity = "private";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Config()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить пароль на чтение данных
        /// </summary>
        public string ReadCommunity { get; set; }

        /// <summary>
        /// Получить или установить пароль на запись данных
        /// </summary>
        public string WriteCommunity { get; set; }

        /// <summary>
        /// Получить граппы переменных
        /// </summary>
        public List<Group> Groups { get; private set; }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            ReadCommunity = DefReadCommunity;
            WriteCommunity = DefWriteCommunity;
            Groups = new List<Group>();
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

                // загрузка параметров
                XmlNode paramsNode = xmlDoc.DocumentElement.SelectSingleNode("Params");

                if (paramsNode != null)
                {
                    foreach (XmlElement paramElem in paramsNode.ChildNodes)
                    {
                        string name = paramElem.GetAttribute("name").ToLowerInvariant();
                        string val = paramElem.GetAttribute("value");

                        if (name == "readcommunity")
                            ReadCommunity = val;
                        else if (name == "writecommunity")
                            WriteCommunity = val;
                    }
                }

                // загрузка групп переменных
                XmlNode groupsNode = xmlDoc.DocumentElement.SelectSingleNode("Groups");

                if (groupsNode != null)
                {
                    foreach (XmlElement groupElem in groupsNode.ChildNodes)
                    {
                        Group group = new Group();
                        group.Name = groupElem.GetAttribute("name");

                        // загрузка переменных
                        XmlNodeList variableNodes = groupElem.SelectNodes("Variable");
                        foreach (XmlElement variableElem in variableNodes)
                        {
                            Variable variable = new Variable();
                            variable.Name = variableElem.GetAttribute("name");
                            variable.OID = variableElem.GetAttribute("oid");
                            group.Variables.Add(variable);
                        }

                        Groups.Add(group);
                    }
                }
                
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ":\r\n" + ex.Message;
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

                XmlElement rootElem = xmlDoc.CreateElement("KpSnmpConfig");
                xmlDoc.AppendChild(rootElem);

                // сохранение параметров
                XmlElement paramsElem = xmlDoc.CreateElement("Params");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("ReadCommunity", ReadCommunity);
                paramsElem.AppendParamElem("WriteCommunity", WriteCommunity);

                // сохранение групп переменных
                XmlElement groupsElem = xmlDoc.CreateElement("Groups");
                rootElem.AppendChild(groupsElem);

                foreach (Group group in Groups)
                {
                    XmlElement groupElem = xmlDoc.CreateElement("Group");
                    groupElem.SetAttribute("name", group.Name);
                    groupsElem.AppendChild(groupElem);

                    // сохранение переменных
                    foreach (Variable variable in group.Variables)
                    {
                        XmlElement variableElem = xmlDoc.CreateElement("Variable");
                        variableElem.SetAttribute("name", variable.Name);
                        variableElem.SetAttribute("oid", variable.OID);
                        groupElem.AppendChild(variableElem);
                    }
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ":\r\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Получить имя файла конфигурации
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return configDir + "KpSnmp_" + CommUtils.AddZeros(kpNum, 3) + ".xml";
        }
    }
}
