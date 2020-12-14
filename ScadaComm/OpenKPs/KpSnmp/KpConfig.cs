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
 * Module   : KpSnmp
 * Summary  : Device communication configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Device communication configuration.
    /// <para>Конфигурация связи с КП.</para>
    /// </summary>
    internal class KpConfig : ITreeNode
    {
        /// <summary>
        /// Группа переменных
        /// </summary>
        public class VarGroup : ITreeNode
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public VarGroup()
            {
                Name = "";
                Variables = new List<Variable>();
                Parent = null;
            }

            /// <summary>
            /// Получить или установить имя группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить переменные
            /// </summary>
            public List<Variable> Variables { get; private set; }
            /// <summary>
            /// Получить или установить родительский узел
            /// </summary>
            public ITreeNode Parent { get; set; }
            /// <summary>
            /// Получить список дочерних узлов
            /// </summary>
            public IList Children
            {
                get
                {
                    return Variables;
                }
            }

            /// <summary>
            /// Определить, что параметры текущего объекта равны заданным
            /// </summary>
            public bool Equals(string name)
            {
                return string.Equals(Name, name, StringComparison.Ordinal);
            }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// Переменная
        /// </summary>
        public class Variable : ITreeNode
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Variable()
            {
                Name = "";
                OID = "";
                Parent = null;
            }

            /// <summary>
            /// Получить или установить имя переменной
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить идентификатор переменной
            /// </summary>
            public string OID { get; set; }
            /// <summary>
            /// Получить или установить признак, что переменная имеет тип BITS
            /// </summary>
            public bool IsBits { get; set; }
            /// <summary>
            /// Получить или установить родительский узел
            /// </summary>
            public ITreeNode Parent { get; set; }
            /// <summary>
            /// Получить список дочерних узлов - он всегда равен null
            /// </summary>
            public IList Children
            {
                get
                {
                    return null;
                }
            }

            /// <summary>
            /// Определить, что параметры текущего объекта равны заданным
            /// </summary>
            public bool Equals(string name, string oid, bool isBits)
            {
                return
                    string.Equals(Name, name, StringComparison.Ordinal) &&
                    string.Equals(OID, oid, StringComparison.Ordinal) &&
                    IsBits == isBits;
            }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return string.Format("{0} ({1})", Name, OID);
            }
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
        /// Версия SNMP по умоланию
        /// </summary>
        public const int DefSnmpVersion = 2;


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpConfig()
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
        /// Получить или установить версию SNMP
        /// </summary>
        public int SnmpVersion { get; set; }

        /// <summary>
        /// Получить граппы переменных
        /// </summary>
        public List<VarGroup> VarGroups { get; private set; }


        /// <summary>
        /// Получить или установить родительский узел - он всегда равен null
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                // некорректный вызов
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Получить список дочерних узлов
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return VarGroups;
            }
        }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            ReadCommunity = DefReadCommunity;
            WriteCommunity = DefWriteCommunity;
            SnmpVersion = DefSnmpVersion;
            VarGroups = new List<VarGroup>();
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
                    XmlNodeList paramNodes = paramsNode.SelectNodes("Param");
                    foreach (XmlElement paramElem in paramNodes)
                    {
                        string name = paramElem.GetAttribute("name");
                        string nameL = name.ToLowerInvariant();
                        string val = paramElem.GetAttribute("value");

                        try
                        {
                            if (nameL == "readcommunity")
                                ReadCommunity = val;
                            else if (nameL == "writecommunity")
                                WriteCommunity = val;
                            else if (nameL == "snmpversion")
                                SnmpVersion = int.Parse(val);
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }
                }

                // загрузка групп переменных
                XmlNode varGroupsNode = xmlDoc.DocumentElement.SelectSingleNode("VarGroups");

                if (varGroupsNode != null)
                {
                    XmlNodeList varGroupNodes = varGroupsNode.SelectNodes("VarGroup");
                    foreach (XmlElement varGroupElem in varGroupNodes)
                    {
                        VarGroup varGroup = new VarGroup();
                        varGroup.Name = varGroupElem.GetAttribute("name");
                        varGroup.Parent = this;

                        // загрузка переменных
                        XmlNodeList variableNodes = varGroupElem.SelectNodes("Variable");
                        foreach (XmlElement variableElem in variableNodes)
                        {
                            Variable variable = new Variable();
                            variable.Name = variableElem.GetAttribute("name");
                            variable.OID = variableElem.GetAttribute("oid");
                            variable.IsBits = variableElem.GetAttrAsBool("isBits");
                            variable.Parent = varGroup;
                            varGroup.Variables.Add(variable);
                        }

                        VarGroups.Add(varGroup);
                    }
                }
                
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

                XmlElement rootElem = xmlDoc.CreateElement("KpSnmpConfig");
                xmlDoc.AppendChild(rootElem);

                // сохранение параметров
                XmlElement paramsElem = xmlDoc.CreateElement("Params");
                rootElem.AppendChild(paramsElem);
                paramsElem.AppendParamElem("ReadCommunity", ReadCommunity);
                paramsElem.AppendParamElem("WriteCommunity", WriteCommunity);
                paramsElem.AppendParamElem("SnmpVersion", SnmpVersion);

                // сохранение групп переменных
                XmlElement varGroupsElem = xmlDoc.CreateElement("VarGroups");
                rootElem.AppendChild(varGroupsElem);

                foreach (VarGroup varGroup in VarGroups)
                {
                    XmlElement varGroupElem = xmlDoc.CreateElement("VarGroup");
                    varGroupElem.SetAttribute("name", varGroup.Name);
                    varGroupsElem.AppendChild(varGroupElem);

                    // сохранение переменных
                    foreach (Variable variable in varGroup.Variables)
                    {
                        XmlElement variableElem = xmlDoc.CreateElement("Variable");
                        variableElem.SetAttribute("name", variable.Name);
                        variableElem.SetAttribute("oid", variable.OID);
                        variableElem.SetAttribute("isBits", variable.IsBits);
                        varGroupElem.AppendChild(variableElem);
                    }
                }

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
        public static string GetFileName(string configDir, int kpNum, string cmdLine)
        {
            string shortFileName = string.IsNullOrEmpty(cmdLine) ?
                "KpSnmp_" + CommUtils.AddZeros(kpNum, 3) + ".xml" :
                cmdLine.Trim();
            return Path.Combine(configDir, shortFileName);
        }
    }
}
