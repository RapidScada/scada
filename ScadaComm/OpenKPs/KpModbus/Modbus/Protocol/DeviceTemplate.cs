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
 * Module   : KpModbus
 * Summary  : Device template
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2019
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

                defByteOrder2 = null;
                defByteOrder4 = null;
                defByteOrder8 = null;

                ZeroAddr = settingsElem.GetChildAsBool("ZeroAddr", false);
                DecAddr = settingsElem.GetChildAsBool("DecAddr", true);
                DefByteOrder2 = settingsElem.GetChildAsString("DefByteOrder2");
                DefByteOrder4 = settingsElem.GetChildAsString("DefByteOrder4");
                DefByteOrder8 = settingsElem.GetChildAsString("DefByteOrder8");
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
            /// Копировать настройки из заданных
            /// </summary>
            public void CopyFrom(Settings srcSettings)
            {
                if (srcSettings == null)
                    throw new ArgumentNullException("srcSettings");

                defByteOrder2 = null;
                defByteOrder4 = null;
                defByteOrder8 = null;

                ZeroAddr = srcSettings.ZeroAddr;
                DecAddr = srcSettings.DecAddr;
                DefByteOrder2 = srcSettings.DefByteOrder2;
                DefByteOrder4 = srcSettings.DefByteOrder4;
                DefByteOrder8 = srcSettings.DefByteOrder8;
            }
            /// <summary>
            /// Получить подходящий порядок байт по умолчанию
            /// </summary>
            public int[] GetDefByteOrder(int elemCnt)
            {
                switch (elemCnt)
                {
                    case 1:
                        if (defByteOrder2 == null)
                            defByteOrder2 = ModbusUtils.ParseByteOrder(DefByteOrder2);
                        return defByteOrder2;

                    case 2:
                        if (defByteOrder4 == null)
                            defByteOrder4 = ModbusUtils.ParseByteOrder(DefByteOrder4);
                        return defByteOrder4;

                    case 4:
                        if (defByteOrder8 == null)
                            defByteOrder8 = ModbusUtils.ParseByteOrder(DefByteOrder8);
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
        /// Sets the default values.
        /// </summary>
        protected virtual void SetToDefault()
        {
            Sett.SetToDefault();
            ElemGroups.Clear();
            Cmds.Clear();
        }

        /// <summary>
        /// Loads the template from the XML node.
        /// </summary>
        protected virtual void LoadFromXml(XmlNode rootNode)
        {
            // загрузка настроек шаблона
            XmlNode settingsNode = rootNode.SelectSingleNode("Settings");
            if (settingsNode != null)
                Sett.LoadFromXml((XmlElement)settingsNode);

            // загрузка групп элементов
            XmlNode elemGroupsNode = rootNode.SelectSingleNode("ElemGroups");
            if (elemGroupsNode != null)
            {
                XmlNodeList elemGroupNodes = elemGroupsNode.SelectNodes("ElemGroup");
                int kpTagInd = 0;

                foreach (XmlElement elemGroupElem in elemGroupNodes)
                {
                    ElemGroup elemGroup = CreateElemGroup(elemGroupElem.GetAttrAsEnum<TableType>("tableType"));
                    elemGroup.StartKPTagInd = kpTagInd;
                    elemGroup.StartSignal = kpTagInd + 1;
                    elemGroup.LoadFromXml(elemGroupElem);

                    if (elemGroup.Elems.Count > 0)
                    {
                        if (elemGroup.ByteOrderEnabled)
                        {
                            foreach (Elem elem in elemGroup.Elems)
                            {
                                if (elem.ByteOrder == null)
                                    elem.ByteOrder = Sett.GetDefByteOrder(elem.Quantity);
                            }
                        }

                        ElemGroups.Add(elemGroup);
                        kpTagInd += elemGroup.Elems.Count;
                    }
                }
            }

            // загрузка команд
            XmlNode cmdsNode = rootNode.SelectSingleNode("Cmds");
            if (cmdsNode != null)
            {
                XmlNodeList cmdNodes = cmdsNode.SelectNodes("Cmd");

                foreach (XmlElement cmdElem in cmdNodes)
                {
                    ModbusCmd cmd = CreateModbusCmd(
                        cmdElem.GetAttrAsEnum<TableType>("tableType"),
                        cmdElem.GetAttrAsBool("multiple"));
                    cmd.LoadFromXml(cmdElem);

                    if (cmd.ByteOrderEnabled && cmd.ByteOrder == null)
                        cmd.ByteOrder = Sett.GetDefByteOrder(cmd.ElemCnt);

                    if (cmd.CmdNum > 0)
                        Cmds.Add(cmd);
                }
            }
        }

        /// <summary>
        /// Saves the template into the XML node.
        /// </summary>
        protected virtual void SaveToXml(XmlElement rootElem)
        {
            // сохранение настроек шаблона
            Sett.SaveToXml(rootElem.AppendElem("Settings"));

            // сохранение групп элементов
            XmlElement elemGroupsElem = rootElem.AppendElem("ElemGroups");
            foreach (ElemGroup elemGroup in ElemGroups)
            {
                elemGroup.SaveToXml(elemGroupsElem.AppendElem("ElemGroup"));
            }

            // сохранение команд
            XmlElement cmdsElem = rootElem.AppendElem("Cmds");
            foreach (ModbusCmd cmd in Cmds)
            {
                cmd.SaveToXml(cmdsElem.AppendElem("Cmd"));
            }
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
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                LoadFromXml(xmlDoc.DocumentElement);
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
                SaveToXml(rootElem);

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
        /// Copies the template properties from the source template.
        /// </summary>
        public virtual void CopyFrom(DeviceTemplate srcTemplate)
        {
            if (srcTemplate == null)
                throw new ArgumentNullException("srcTemplate");

            // копирование настроек шаблона
            Sett.CopyFrom(srcTemplate.Sett);

            // копирование групп элементов
            ElemGroups.Clear();
            foreach (ElemGroup srcGroup in srcTemplate.ElemGroups)
            {
                ElemGroup destGroup = CreateElemGroup(srcGroup.TableType);
                destGroup.CopyFrom(srcGroup);
                ElemGroups.Add(destGroup);
            }

            // копирование команд
            Cmds.Clear();
            foreach (ModbusCmd srcCmd in srcTemplate.Cmds)
            {
                ModbusCmd destCmd = CreateModbusCmd(srcCmd.TableType, srcCmd.Multiple);
                destCmd.CopyFrom(srcCmd);
                Cmds.Add(destCmd);
            }
        }

        /// <summary>
        /// Creates a new group of Modbus elements.
        /// </summary>
        public virtual ElemGroup CreateElemGroup(TableType tableType)
        {
            return new ElemGroup(tableType);
        }

        /// <summary>
        /// Creates a new Modbus command.
        /// </summary>
        public virtual ModbusCmd CreateModbusCmd(TableType tableType, bool multiple)
        {
            return new ModbusCmd(tableType, multiple);
        }
    }
}
