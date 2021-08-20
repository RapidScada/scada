/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Device driver communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus;
using Scada.Comm.Devices.Modbus.Protocol;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver communication logic.
    /// <para>Логика работы драйвера КП.</para>
    /// </summary>
    public class KpModbusLogic : KPLogic
    {
        /// <summary>
        /// Словарь шаблонов устройств для общих свойств линии связи
        /// </summary>
        private class TemplateDict : Dictionary<string, DeviceTemplate>
        {
            public override string ToString()
            {
                return "Dictionary of " + Count + " templates";
            }
        }
        
        /// <summary>
        /// Периодичность попыток установки TCP-соединения, с
        /// </summary>
        private const int TcpConnectPer = 5;

        private TransMode transMode;                // режим передачи данных
        private ModbusPoll modbusPoll;              // объект для опроса КП
        private ModbusPoll.RequestDelegate request; // метод выполнения запроса
        private byte devAddr;                       // адрес устройства
        private List<ElemGroup> elemGroups;         // активные запрашиваемые группы элементов
        private int elemGroupCnt;                   // количество активных групп элементов
        private HashSet<int> floatSignals;          // множество сигналов, форматируемых как вещественное число

        /// <summary>
        /// Шаблон устройства, используемый данным КП
        /// </summary>
        protected DeviceTemplate deviceTemplate;


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpModbusLogic(int number)
            : base(number)
        {
        }


        /// <summary>
        /// Gets the key of the template dictionary.
        /// </summary>
        protected virtual string TemplateDictKey
        {
            get
            {
                return "Modbus.Templates";
            }
        }


        /// <summary>
        /// Gets or creates the template dictionary from the common line properties.
        /// </summary>
        private TemplateDict GetTemplateDictionary()
        {
            TemplateDict templateDict = CommonProps.ContainsKey(TemplateDictKey) ? 
                CommonProps[TemplateDictKey] as TemplateDict : null;

            if (templateDict == null)
            {
                templateDict = new TemplateDict();
                CommonProps.Add(TemplateDictKey, templateDict);
            }

            return templateDict;
        }

        /// <summary>
        /// Gets existing or create a new device template.
        /// </summary>
        private void PrepareTemplate(string fileName)
        {
            deviceTemplate = null;

            if (string.IsNullOrEmpty(fileName))
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0} Ошибка: Не задан шаблон устройства для {1}" :
                    "{0} Error: Template is undefined for the {1}", CommUtils.GetNowDT(), Caption));
            }
            else
            {
                TemplateDict templateDict = GetTemplateDictionary();

                if (templateDict.TryGetValue(fileName, out DeviceTemplate existingTemplate))
                {
                    if (existingTemplate != null)
                    {
                        deviceTemplate = GetTemplateFactory().CreateDeviceTemplate();
                        deviceTemplate.CopyFrom(existingTemplate);
                    }
                }
                else
                {
                    DeviceTemplate newTemplate = GetTemplateFactory().CreateDeviceTemplate();
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} Загрузка шаблона устройства из файла {1}" :
                        "{0} Load device template from file {1}", CommUtils.GetNowDT(), fileName));
                    string filePath = Path.IsPathRooted(fileName) ?
                        fileName : Path.Combine(AppDirs.ConfigDir, fileName);

                    if (newTemplate.Load(filePath, out string errMsg))
                        deviceTemplate = newTemplate;
                    else
                        WriteToLog(errMsg);

                    templateDict.Add(fileName, deviceTemplate);
                }
            }
        }

        /// <summary>
        /// Initializes an object for polling data.
        /// </summary>
        private void InitModbusPoll()
        {
            if (deviceTemplate != null)
            {
                // find the required size of the input buffer
                int inBufSize = 0;
                foreach (ElemGroup elemGroup in elemGroups)
                {
                    if (inBufSize < elemGroup.RespAduLen)
                        inBufSize = elemGroup.RespAduLen;
                }

                foreach (ModbusCmd cmd in deviceTemplate.Cmds)
                {
                    if (inBufSize < cmd.RespAduLen)
                        inBufSize = cmd.RespAduLen;
                }

                // create an object for polling data
                modbusPoll = new ModbusPoll(inBufSize)
                {
                    Timeout = ReqParams.Timeout,
                    Connection = Connection,
                    WriteToLog = WriteToLog
                };
            }
        }

        /// <summary>
        /// Установить окончание строки в соединении для режима ASCII
        /// </summary>
        private void SetNewLine()
        {
            if (Connection != null && transMode == TransMode.ASCII)
                Connection.NewLine = ModbusUtils.CRLF;
        }

        /// <summary>
        /// Установить значения тегов КП в соответствии со значениями элементов группы
        /// </summary>
        private void SetTagsData(ElemGroup elemGroup)
        {
            for (int i = 0, j = elemGroup.StartKPTagInd + i, cnt = elemGroup.Elems.Count; i < cnt; i++, j++)
            {
                SetCurData(j, elemGroup.GetElemVal(i), BaseValues.CnlStatuses.Defined);
            }
        }


        /// <summary>
        /// Gets a device template factory.
        /// </summary>
        protected virtual DeviceTemplateFactory GetTemplateFactory()
        {
            return KpUtils.TemplateFactory;
        }

        /// <summary>
        /// Creates tag groups according to the specified template.
        /// </summary>
        protected virtual List<TagGroup> CreateTagGroups(DeviceTemplate deviceTemplate, ref int tagInd)
        {
            List<TagGroup> tagGroups = new List<TagGroup>();

            if (deviceTemplate != null)
            {
                foreach (ElemGroup elemGroup in deviceTemplate.ElemGroups)
                {
                    TagGroup tagGroup = new TagGroup(elemGroup.Name);
                    tagGroups.Add(tagGroup);
                    elemGroup.StartKPTagInd = tagInd;

                    foreach (Elem elem in elemGroup.Elems)
                    {
                        int signal = ++tagInd;
                        tagGroup.AddNewTag(signal, elem.Name);

                        if (elem.ElemType == ElemType.Float || elem.ElemType == ElemType.Double)
                            floatSignals.Add(signal);
                    }
                }
            }

            return tagGroups;
        }

        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0 && !floatSignals.Contains(signal))
                return tagData.Val.ToString("F0");
            else
                return base.ConvertTagDataToStr(signal, tagData);
        }


        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (deviceTemplate == null)
            {
                WriteToLog(Localization.UseRussian ? 
                    "Нормальное взаимодействие с КП невозможно, т.к. шаблон устройства не загружен" :
                    "Normal device communication is impossible because device template has not been loaded");
                Thread.Sleep(ReqParams.Delay);
                lastCommSucc = false;
            }
            else if (elemGroupCnt > 0)
            {
                // выполнение запросов по группам элементов
                int elemGroupInd = 0;
                while (elemGroupInd < elemGroupCnt && lastCommSucc)
                {
                    ElemGroup elemGroup = elemGroups[elemGroupInd];
                    lastCommSucc = false;
                    int tryNum = 0;

                    while (RequestNeeded(ref tryNum))
                    {
                        // выполнение запроса
                        if (request(elemGroup))
                        {
                            lastCommSucc = true;
                            SetTagsData(elemGroup); // установка значений тегов КП
                        }

                        // завершение запроса
                        FinishRequest();
                        tryNum++;
                    }

                    if (lastCommSucc)
                    {
                        // переход к следующей группе элементов
                        elemGroupInd++;
                    }
                    else if (tryNum > 0)
                    {
                        // установка неопределённого статуса тегов КП текущей и следующих групп, если запрос неудачный
                        while (elemGroupInd < elemGroupCnt)
                        {
                            elemGroup = elemGroups[elemGroupInd];
                            InvalidateCurData(elemGroup.StartKPTagInd, elemGroup.Elems.Count);
                            elemGroupInd++;
                        }
                    }
                }
            }
            else
            {
                WriteToLog(Localization.UseRussian ?
                    "Отсутствуют элементы для запроса" : 
                    "No elements for request");
                Thread.Sleep(ReqParams.Delay);
            }

            // расчёт статистики
            CalcSessStats();
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);

            if (CanSendCmd)
            {
                ModbusCmd modbusCmd = deviceTemplate.FindCmd(cmd.CmdNum);

                if (modbusCmd != null &&
                    (modbusCmd.Multiple && (cmd.CmdTypeID == BaseValues.CmdTypes.Standard || 
                    cmd.CmdTypeID == BaseValues.CmdTypes.Binary) ||
                    !modbusCmd.Multiple && cmd.CmdTypeID == BaseValues.CmdTypes.Standard))
                {
                    // формирование команды Modbus
                    if (modbusCmd.Multiple)
                    {
                        modbusCmd.Value = 0;

                        if (cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
                            modbusCmd.SetCmdData(cmd.CmdVal);
                        else
                            modbusCmd.Data = cmd.CmdData;
                    }
                    else
                    {
                        modbusCmd.Value = modbusCmd.TableType == TableType.HoldingRegisters ?
                            (ushort)cmd.CmdVal :
                            cmd.CmdVal > 0 ? (ushort)1 : (ushort)0;
                        modbusCmd.SetCmdData(cmd.CmdVal);
                    }

                    modbusCmd.InitReqPDU();
                    modbusCmd.InitReqADU(devAddr, transMode);

                    // отправка команды устройству
                    lastCommSucc = false;
                    int tryNum = 0;

                    while (RequestNeeded(ref tryNum))
                    {
                        // выполнение запроса
                        if (request(modbusCmd))
                            lastCommSucc = true;

                        // завершение запроса
                        FinishRequest();
                        tryNum++;
                    }
                }
                else
                {
                    lastCommSucc = false;
                    WriteToLog(CommPhrases.IllegalCommand);
                }
            }

            // расчёт статистики
            CalcCmdStats();
        }
        
        /// <summary>
        /// Выполнить действия после добавления КП на линию связи
        /// </summary>
        public override void OnAddedToCommLine()
        {
            // получение или загрузка шаблона устройства
            string fileName = ReqParams.CmdLine.Trim();
            PrepareTemplate(fileName);

            // инициализация тегов КП на основе шаблона устройства
            floatSignals = new HashSet<int>();
            int tagInd = 0;
            List<TagGroup> tagGroups = CreateTagGroups(deviceTemplate, ref tagInd);
            InitKPTags(tagGroups);

            // определение режима передачи данных
            transMode = CustomParams.GetEnumParam("TransMode", false, TransMode.RTU);

            // определение возможности отправки команд
            CanSendCmd = deviceTemplate != null && deviceTemplate.Cmds.Count > 0;
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // инициализация запрашиваемых элементов и команд
            // располагается в OnCommLineStart, т.к. здесь Address окончательно определён
            if (deviceTemplate == null)
            {
                elemGroups = null;
                elemGroupCnt = 0;
            }
            else
            {
                // получение активных групп элементов
                elemGroups = deviceTemplate.GetActiveElemGroups();
                elemGroupCnt = elemGroups.Count;

                // формирование PDU и ADU
                devAddr = (byte)Address;
                foreach (ElemGroup elemGroup in elemGroups)
                {
                    elemGroup.InitReqPDU();
                    elemGroup.InitReqADU(devAddr, transMode);
                }

                foreach (ModbusCmd cmd in deviceTemplate.Cmds)
                {
                    cmd.InitReqPDU();
                    cmd.InitReqADU(devAddr, transMode);
                }
            }

            // инициализация объекта для опроса КП
            InitModbusPoll();

            // выбор метода запроса
            request = modbusPoll.GetRequestMethod(transMode);
        }

        /// <summary>
        /// Выполнить действия после установки соединения
        /// </summary>
        public override void OnConnectionSet()
        {
            SetNewLine();

            if (modbusPoll != null)
                modbusPoll.Connection = Connection;
        }
    }
}
