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
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.Comm.Devices.Modbus.Protocol;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
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
        /// Делегат выполнения запроса
        /// </summary>
        private delegate bool RequestDelegate(DataUnit dataUnit);
        
        /// <summary>
        /// Периодичность попыток установки TCP-соединения, с
        /// </summary>
        private const int TcpConnectPer = 5;

        private DeviceTemplate deviceTemplate; // шаблон устройства, используемый данным КП
        private TransModes transMode;          // режим передачи данных
        private ModbusPoll modbusPoll;         // объект для опроса устройств по протоколу Modbus
        private RequestDelegate request;       // метод выполнения запроса
        private byte devAddr;                  // адрес устройства
        private List<ElemGroup> elemGroups;    // активные запрашиваемые группы элементов
        private int elemGroupCnt;              // количество активных групп элементов
        private HashSet<int> floatSignals;     // множество сигналов, форматируемых как вещественное число


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpModbusLogic(int number)
            : base(number)
        {
            modbusPoll = new ModbusPoll();
        }


        /// <summary>
        /// Получить из общих свойств линии связи или создать словарь шаблонов устройств
        /// </summary>
        private Dictionary<string, DeviceTemplate> GetTemplates()
        {
            Dictionary<string, DeviceTemplate> templates = CommonProps.ContainsKey("Templates") ?
                CommonProps["Templates"] as Dictionary<string, DeviceTemplate> : null;

            if (templates == null)
            {
                templates = new TemplateDict();
                CommonProps.Add("Templates", templates);
            }

            return templates;
        }

        /// <summary>
        /// Установить окончание строки в соединении для режима ASCII
        /// </summary>
        private void SetNewLine()
        {
            if (Connection != null && transMode == TransModes.ASCII)
                Connection.NewLine = ModbusUtils.CRLF;
        }

        /// <summary>
        /// Установить значения тегов КП в соответствии со значениями элементов группы
        /// </summary>
        private void SetTagsData(ElemGroup elemGroup)
        {
            int len = elemGroup.ElemVals.Length;
            for (int i = 0, j = elemGroup.StartKPTagInd + i; i < len; i++, j++)
                SetCurData(j, elemGroup.GetElemVal(i), BaseValues.CnlStatuses.Defined);
        }

        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0 && !floatSignals.Contains(signal))
                return curData[signal - 1].Val.ToString("F0");
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
                            InvalidateCurData(elemGroup.StartKPTagInd, elemGroup.ElemVals.Length);
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
                        if (cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
                            modbusCmd.SetCmdData(cmd.CmdVal);
                        else
                            modbusCmd.Data = cmd.CmdData;
                    }
                    else
                    {
                        modbusCmd.Value = modbusCmd.TableType == TableTypes.HoldingRegisters ?
                            (ushort)cmd.CmdVal :
                            cmd.CmdVal > 0 ? (ushort)1 : (ushort)0;
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
            // загрузка шаблона устройства
            deviceTemplate = null;
            elemGroups = null;
            elemGroupCnt = 0;
            floatSignals = new HashSet<int>();
            string fileName = ReqParams.CmdLine.Trim();

            if (fileName == "")
            {
                WriteToLog(string.Format(Localization.UseRussian ? 
                    "{0} Ошибка: Не задан шаблон устройства для {1}" : 
                    "{0} Error: Template is undefined for the {1}", CommUtils.GetNowDT(), Caption));
            }
            else
            {
                Dictionary<string, DeviceTemplate> templates = GetTemplates();
                if (templates.ContainsKey(fileName))
                {
                    // создание шаблона устройства на основе шаблона, загруженного ранее
                    DeviceTemplate template = templates[fileName];
                    if (template != null)
                    {
                        deviceTemplate = new DeviceTemplate();
                        deviceTemplate.CopyFrom(template);
                    }
                }
                else
                {
                    WriteToLog(string.Format(Localization.UseRussian ? 
                        "{0} Загрузка шаблона устройства из файла {1}" :
                        "{0} Load device template from file {1}", CommUtils.GetNowDT(), fileName));
                    DeviceTemplate template = new DeviceTemplate();
                    string errMsg;

                    if (template.Load(AppDirs.ConfigDir + fileName, out errMsg))
                    {
                        deviceTemplate = template;
                        templates.Add(fileName, template);
                    }
                    else
                    {
                        WriteToLog(errMsg);
                        templates.Add(fileName, null);
                    }
                }
            }

            if (deviceTemplate != null)
            {
                elemGroups = deviceTemplate.GetActiveElemGroups();
                elemGroupCnt = elemGroups.Count;
            }

            // инициализация тегов КП на основе модели устройства
            if (deviceTemplate.ElemGroups.Count > 0)
            {
                List<TagGroup> tagGroups = new List<TagGroup>();
                int tagInd = 0;

                foreach (ElemGroup elemGroup in deviceTemplate.ElemGroups)
                {
                    TagGroup tagGroup = new TagGroup(elemGroup.Name);
                    tagGroups.Add(tagGroup);
                    elemGroup.StartKPTagInd = tagInd;

                    foreach (Elem elem in elemGroup.Elems)
                    {
                        int signal = ++tagInd;
                        tagGroup.KPTags.Add(new KPTag(signal, elem.Name));
                        if (elem.ElemType == ElemTypes.Float)
                            floatSignals.Add(signal);
                    }
                }

                InitKPTags(tagGroups);
                CanSendCmd = deviceTemplate.Cmds.Count > 0;
            }
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // получение режима передачи данных
            transMode = CustomParams.GetEnumParam("TransMode", false, TransModes.RTU);

            // настройка библиотеки в зависимости от режима передачи данных
            switch (transMode)
            {
                case TransModes.RTU:
                    request += modbusPoll.RtuRequest;
                    break;
                case TransModes.ASCII:
                    request += modbusPoll.AsciiRequest;
                    break;
                default: // TransModes.TCP
                    request += modbusPoll.TcpRequest;
                    break;
            }

            SetNewLine();

            // настройка объекта, реализующего протокол Modbus
            modbusPoll.Timeout = ReqParams.Timeout;
            modbusPoll.WriteToLog = WriteToLog;

            // формирование PDU и ADU
            if (deviceTemplate != null)
            {
                devAddr = (byte)Address;

                foreach (ElemGroup elemGroup in deviceTemplate.ElemGroups)
                {
                    elemGroup.InitReqPDU();
                    elemGroup.InitReqADU(devAddr, transMode);
                }
            }
        }

        /// <summary>
        /// Выполнить действия после установки соединения
        /// </summary>
        public override void OnConnectionSet()
        {
            SetNewLine();
            modbusPoll.Connection = Connection;
        }
    }
}
