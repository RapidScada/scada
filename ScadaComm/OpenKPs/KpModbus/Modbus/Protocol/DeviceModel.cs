using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Модель устройства
    /// </summary>
    public class DeviceModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DeviceModel()
        {
            ElemGroups = new List<ElemGroup>();
            Cmds = new List<ModbusCmd>();
        }


        /// <summary>
        /// Получить список групп элементов
        /// </summary>
        public List<ElemGroup> ElemGroups { get; private set; }

        /// <summary>
        /// Получить список команд
        /// </summary>
        public List<ModbusCmd> Cmds { get; private set; }


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
        public bool LoadTemplate(string fileName, out string errMsg)
        {
            try
            {
                // очистка списков групп элементов и команд
                ElemGroups.Clear();
                Cmds.Clear();

                // загрузка шаблона устройства
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                // загрузка групп элементов
                XmlNode elemGroupsNode = xmlDoc.DocumentElement.SelectSingleNode("ElemGroups");

                if (elemGroupsNode != null)
                {
                    int kpTagInd = 0;

                    foreach (XmlElement elemGroupElem in elemGroupsNode.ChildNodes)
                    {
                        TableTypes tableType =
                            (TableTypes)(Enum.Parse(typeof(TableTypes),
                            elemGroupElem.GetAttribute("tableType"), true));
                        ElemGroup elemGroup = new ElemGroup(tableType);
                        elemGroup.Name = elemGroupElem.GetAttribute("name");
                        elemGroup.Address = (ushort)elemGroupElem.GetAttrAsInt("address");
                        string active = elemGroupElem.GetAttribute("active");
                        elemGroup.Active = active == "" ? true : bool.Parse(active);
                        elemGroup.StartKPTagInd = kpTagInd;
                        elemGroup.StartSignal = kpTagInd + 1;

                        XmlNodeList elemNodes = elemGroupElem.SelectNodes("Elem");
                        foreach (XmlElement elemElem in elemNodes)
                        {
                            Elem elem = new Elem();
                            elem.Name = elemElem.GetAttribute("name");
                            string elemTypeStr = elemElem.GetAttribute("type");
                            elem.ElemType = elemTypeStr == "" ? elemGroup.DefElemType :
                                (ElemTypes)(Enum.Parse(typeof(ElemTypes), elemTypeStr, true));
                            elem.InitByteOrder(elemElem.GetAttribute("byteOrder"));
                            elemGroup.Elems.Add(elem);
                        }

                        if (0 < elemGroup.Elems.Count && elemGroup.Elems.Count <= ElemGroup.GetMaxElemCnt(tableType))
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
                        TableTypes tableType =
                            (TableTypes)(Enum.Parse(typeof(TableTypes), cmdElem.GetAttribute("tableType"), true));
                        string multiple = cmdElem.GetAttribute("multiple");
                        string elemCnt = cmdElem.GetAttribute("elemCnt");
                        ModbusCmd cmd = multiple == "" || elemCnt == "" ?
                            new ModbusCmd(tableType) :
                            new ModbusCmd(tableType, bool.Parse(multiple), int.Parse(elemCnt));
                        cmd.Address = (ushort)cmdElem.GetAttrAsInt("address");
                        cmd.Name = cmdElem.GetAttribute("name");
                        cmd.CmdNum = cmdElem.GetAttrAsInt("cmdNum");

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
        public bool SaveTemplate(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("DevTemplate");
                xmlDoc.AppendChild(rootElem);

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

                    bool writeElemType = elemGroup.TableType == TableTypes.InputRegisters ||
                        elemGroup.TableType == TableTypes.HoldingRegisters;

                    foreach (Elem elem in elemGroup.Elems)
                    {
                        XmlElement elemElem = xmlDoc.CreateElement("Elem");
                        elemElem.SetAttribute("name", elem.Name);
                        if (writeElemType)
                        {
                            elemElem.SetAttribute("type", elem.ElemType.ToString().ToLowerInvariant());
                            elemElem.SetAttribute("byteOrder", elem.ByteOrderStr);
                        }
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
                    cmdElem.SetAttribute("elemCnt", cmd.ElemCnt);
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
        public void CopyFrom(DeviceModel srcDeviceModel)
        {
            if (srcDeviceModel == null)
                throw new ArgumentNullException("srcDeviceModel");

            // очистка списков групп элементов и команд
            ElemGroups.Clear();
            Cmds.Clear();

            // копирование групп элементов
            foreach (ElemGroup srcGroup in srcDeviceModel.ElemGroups)
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
            foreach (ModbusCmd srcCmd in srcDeviceModel.Cmds)
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
