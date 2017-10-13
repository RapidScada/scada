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
 * Module   : KpModbus
 * Summary  : Device template
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Device template
    /// <para>Шаблон устройства</para>
    /// </summary>
    public class DeviceTemplate
    {
        /// <summary>
        /// Настройки шаблона
        /// </summary>
        public class Settings
        {
            // Порядок байт по умолчанию для значений длиной 2, 4 и 8 байт
            private int[] defByteOrder2;
            private int[] defByteOrder4;
            private int[] defByteOrder8;

            /// <summary>
            /// Конструктор
            /// </summary>
            public Settings()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить признак отображения адресов, начиная с 0
            /// </summary>
            public bool ZeroAddr { get; set; }
            /// <summary>
            /// Получить или установить признак отображения адресов в 10-тичной системе
            /// </summary>
            public bool DecAddr { get; set; }
            /// <summary>
            /// Получить или установить строковую запись порядка байт по умолчанию для значений длиной 2 байта
            /// </summary>
            public string DefByteOrder2 { get; set; }
            /// <summary>
            /// Получить или установить строковую запись порядка байт по умолчанию для значений длиной 4 байта
            /// </summary>
            public string DefByteOrder4 { get; set; }
            /// <summary>
            /// Получить или установить строковую запись порядка байт по умолчанию для значений длиной 8 байт
            /// </summary>
            public string DefByteOrder8 { get; set; }

            /// <summary>
            /// Установить настройки шаблона по умолчанию
            /// </summary>
            public void SetToDefault()
            {
                defByteOrder2 = null;
                defByteOrder4 = null;
                defByteOrder8 = null;

                ZeroAddr = false;
                DecAddr = true;
                DefByteOrder2 = "";
                DefByteOrder4 = "";
                DefByteOrder8 = "";
            }
            /// <summary>
            /// Загрузить настройки шаблона из XML-узла
            /// </summary>
            public void LoadFromXml(XmlElement settingsElem)
            {
                if (settingsElem == null)
                    throw new ArgumentNullException("settingsElem");

                ZeroAddr = settingsElem.GetChildAsBool("ZeroAddr", false);
                DecAddr = settingsElem.GetChildAsBool("DecAddr", true);
                DefByteOrder2 = settingsElem.GetChildAsString("DefByteOrder2");
                DefByteOrder4 = settingsElem.GetChildAsString("DefByteOrder4");
                DefByteOrder8 = settingsElem.GetChildAsString("DefByteOrder8");

                defByteOrder2 = ModbusUtils.ParseByteOrder(DefByteOrder2);
                defByteOrder4 = ModbusUtils.ParseByteOrder(DefByteOrder4);
                defByteOrder8 = ModbusUtils.ParseByteOrder(DefByteOrder8);
            }
            /// <summary>
            /// Сохранить настройки шаблона в XML-узле
            /// </summary>
            public void SaveToXml(XmlElement settingsElem)
            {
                if (settingsElem == null)
                    throw new ArgumentNullException("settingsElem");

                settingsElem.AppendElem("ZeroAddr", ZeroAddr);
                settingsElem.AppendElem("DecAddr", DecAddr);
                settingsElem.AppendElem("DefByteOrder2", DefByteOrder2);
                settingsElem.AppendElem("DefByteOrder4", DefByteOrder4);
                settingsElem.AppendElem("DefByteOrder8", DefByteOrder8);
            }
            /// <summary>
            /// Получить подходящий порядок байт по умолчанию
            /// </summary>
            public int[] GetDefByteOrder(int elemCnt)
            {
                switch (elemCnt)
                {
                    case 1:
                        return defByteOrder2;
                    case 2:
                        return defByteOrder4;
                    case 4:
                        return defByteOrder8;
                    default:
                        return null;
                }
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public DeviceTemplate()
        {
            Sett = new Settings();
            ElemGroups = new List<ElemGroup>();
            Cmds = new List<ModbusCmd>();
        }


        /// <summary>
        /// Получить настройки шаблона
        /// </summary>
        public Settings Sett { get; private set; }

        /// <summary>
        /// Получить список групп элементов
        /// </summary>
        public List<ElemGroup> ElemGroups { get; private set; }

        /// <summary>
        /// Получить список команд
        /// </summary>
        public List<ModbusCmd> Cmds { get; private set; }


        /// <summary>
        /// Установить свойства шаблона по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            Sett.SetToDefault();
            ElemGroups.Clear();
            Cmds.Clear();
        }

        /// <summary>
        /// Найти команду по номеру
        /// </summary>
        public ModbusCmd FindCmd(int cmdNum)
        {
            foreach (ModbusCmd cmd in Cmds)
            {
                if (cmd.CmdNum == cmdNum)
                    return cmd;
            }

            return null;
        }

        /// <summary>
        /// Получить активные группы элементов
        /// </summary>
        public List<ElemGroup> GetActiveElemGroups()
        {
            List<ElemGroup> activeElemGroups = new List<ElemGroup>();

            foreach (ElemGroup elemGroup in ElemGroups)
            {
                if (elemGroup.Active)
                    activeElemGroups.Add(elemGroup);
            }

            return activeElemGroups;
        }

        /// <summary>
        /// Загрузить шаблон устройства
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            SetToDefault();

            try
            {
                // загрузка шаблона устройства
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                // загрузка настроек шаблона
                XmlNode settingsNode = xmlDoc.DocumentElement.SelectSingleNode("Settings");
                if (settingsNode != null)
                    Sett.LoadFromXml((XmlElement)settingsNode);

                // загрузка групп элементов
                XmlNode elemGroupsNode = xmlDoc.DocumentElement.SelectSingleNode("ElemGroups");
                if (elemGroupsNode != null)
                {
                    int kpTagInd = 0;

                    foreach (XmlElement elemGroupElem in elemGroupsNode.ChildNodes)
                    {
                        TableTypes tableType = elemGroupElem.GetAttrAsEnum<TableTypes>("tableType");
                        ElemGroup elemGroup = new ElemGroup(tableType);
                        elemGroup.Name = elemGroupElem.GetAttribute("name");
                        elemGroup.Address = (ushort)elemGroupElem.GetAttrAsInt("address");
                        elemGroup.Active = elemGroupElem.GetAttrAsBool("active", true);
                        elemGroup.StartKPTagInd = kpTagInd;
                        elemGroup.StartSignal = kpTagInd + 1;

                        XmlNodeList elemNodes = elemGroupElem.SelectNodes("Elem");
                        ElemTypes defElemType = elemGroup.DefElemType;

                        foreach (XmlElement elemElem in elemNodes)
                        {
                            Elem elem = new Elem();
                            elem.Name = elemElem.GetAttribute("name");
                            elem.ElemType = elemElem.GetAttrAsEnum("type", defElemType);

                            if (elemGroup.ByteOrderEnabled)
                            {
                                elem.ByteOrderStr = elemElem.GetAttribute("byteOrder");
                                elem.ByteOrder = ModbusUtils.ParseByteOrder(elem.ByteOrderStr);
                                if (elem.ByteOrder == null)
                                    elem.ByteOrder = Sett.GetDefByteOrder(elem.Length);
                            }

                            elemGroup.Elems.Add(elem);
                        }

                        if (0 < elemGroup.Elems.Count && elemGroup.Elems.Count <= DataUnit.GetMaxElemCnt(tableType))
                        {
                            ElemGroups.Add(elemGroup);
                            kpTagInd += elemGroup.Elems.Count;
                        }
                    }
                }

                // загрузка команд
                XmlNode cmdsNode = xmlDoc.DocumentElement.SelectSingleNode("Cmds");
                if (cmdsNode != null)
                {
                    foreach (XmlElement cmdElem in cmdsNode.ChildNodes)
                    {
                        ModbusCmd cmd = new ModbusCmd(
                            cmdElem.GetAttrAsEnum<TableTypes>("tableType"), 
                            cmdElem.GetAttrAsBool("multiple"), 
                            cmdElem.GetAttrAsInt("elemCnt", 1));
                        cmd.ElemType = cmdElem.GetAttrAsEnum("elemType", cmd.DefElemType);
                        cmd.Address = (ushort)cmdElem.GetAttrAsInt("address");
                        cmd.Name = cmdElem.GetAttribute("name");
                        cmd.CmdNum = cmdElem.GetAttrAsInt("cmdNum");

                        if (cmd.ByteOrderEnabled)
                        {
                            cmd.ByteOrderStr = cmdElem.GetAttribute("byteOrder");
                            cmd.ByteOrder = ModbusUtils.ParseByteOrder(cmd.ByteOrderStr);
                            if (cmd.ByteOrder == null)
                                cmd.ByteOrder = Sett.GetDefByteOrder(cmd.ElemCnt);
                        }

                        if (0 < cmd.CmdNum && cmd.CmdNum <= ushort.MaxValue)
                            Cmds.Add(cmd);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ModbusPhrases.LoadTemplateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить шаблон устройства
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("DevTemplate");
                xmlDoc.AppendChild(rootElem);

                // сохранение настроек шаблона
                Sett.SaveToXml(rootElem.AppendElem("Settings"));

                // сохранение групп элементов
                XmlElement elemGroupsElem = xmlDoc.CreateElement("ElemGroups");
                rootElem.AppendChild(elemGroupsElem);

                foreach (ElemGroup elemGroup in ElemGroups)
                {
                    XmlElement elemGroupElem = xmlDoc.CreateElement("ElemGroup");
                    elemGroupElem.SetAttribute("active", elemGroup.Active);
                    elemGroupElem.SetAttribute("tableType", elemGroup.TableType);
                    elemGroupElem.SetAttribute("address", elemGroup.Address);
                    elemGroupElem.SetAttribute("name", elemGroup.Name);
                    elemGroupsElem.AppendChild(elemGroupElem);

                    foreach (Elem elem in elemGroup.Elems)
                    {
                        XmlElement elemElem = xmlDoc.CreateElement("Elem");
                        elemElem.SetAttribute("name", elem.Name);

                        if (elemGroup.ElemTypeEnabled)
                            elemElem.SetAttribute("type", elem.ElemType.ToString().ToLowerInvariant());

                        if (elemGroup.ByteOrderEnabled)
                            elemElem.SetAttribute("byteOrder", elem.ByteOrderStr);

                        elemGroupElem.AppendChild(elemElem);
                    }
                }

                // сохранение команд
                XmlElement cmdsElem = xmlDoc.CreateElement("Cmds");
                rootElem.AppendChild(cmdsElem);

                foreach (ModbusCmd cmd in Cmds)
                {
                    XmlElement cmdElem = xmlDoc.CreateElement("Cmd");
                    cmdsElem.AppendChild(cmdElem);

                    cmdElem.SetAttribute("tableType", cmd.TableType);
                    cmdElem.SetAttribute("multiple", cmd.Multiple);
                    cmdElem.SetAttribute("address", cmd.Address);

                    if (cmd.ElemTypeEnabled)
                        cmdElem.SetAttribute("elemType", cmd.ElemType.ToString().ToLowerInvariant());

                    if (cmd.Multiple)
                        cmdElem.SetAttribute("elemCnt", cmd.ElemCnt);

                    if (cmd.ByteOrderEnabled)
                        cmdElem.SetAttribute("byteOrder", cmd.ByteOrderStr);

                    cmdElem.SetAttribute("cmdNum", cmd.CmdNum);
                    cmdElem.SetAttribute("name", cmd.Name);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ModbusPhrases.SaveTemplateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Копировать модель устройства из заданной
        /// </summary>
        public void CopyFrom(DeviceTemplate srcTemplate)
        {
            if (srcTemplate == null)
                throw new ArgumentNullException("srcTemplate");

            // очистка списков групп элементов и команд
            ElemGroups.Clear();
            Cmds.Clear();

            // копирование групп элементов
            foreach (ElemGroup srcGroup in srcTemplate.ElemGroups)
            {
                ElemGroup elemGroup = new ElemGroup(srcGroup.TableType)
                {
                    Name = srcGroup.Name,
                    Address = srcGroup.Address,
                    Active = srcGroup.Active,
                    StartKPTagInd = srcGroup.StartKPTagInd,
                    StartSignal = srcGroup.StartSignal,
                };

                foreach (Elem srcElem in srcGroup.Elems)
                {
                    elemGroup.Elems.Add(new Elem()
                    {
                        Name = srcElem.Name,
                        ElemType = srcElem.ElemType,
                        ByteOrder = srcElem.ByteOrder, // копируется ссылка на массив
                        ByteOrderStr = srcElem.ByteOrderStr
                    });
                }

                ElemGroups.Add(elemGroup);
            }

            // копирование команд
            foreach (ModbusCmd srcCmd in srcTemplate.Cmds)
            {
                Cmds.Add(new ModbusCmd(srcCmd.TableType)
                {
                    Multiple = srcCmd.Multiple,
                    ElemCnt = srcCmd.ElemCnt,
                    Address = srcCmd.Address,
                    Name = srcCmd.Name,
                    CmdNum = srcCmd.CmdNum
                });
            }
        }
    }
}
