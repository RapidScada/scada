/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Modified : 2015
 * 
 * Description
 * Interacting with controllers via Modbus protocol.
 */

using Scada.Comm.Devices.KpModbus;
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
        private class TemplateDict : Dictionary<string, Modbus.DeviceModel>
        {
            public override string ToString()
            {
                return "Dictionary of " + Count + " templates";
            }
        }

        /// <summary>
        /// Делегат выполнения запроса
        /// </summary>
        private delegate bool RequestDelegate(Modbus.DataUnit dataUnit);
        
        /// <summary>
        /// Периодичность попыток установки TCP-соединения, с
        /// </summary>
        private const int TcpConnectPer = 5;

        private Modbus.DeviceModel deviceModel;    // модель устройства, используемая данным КП
        private Modbus.TransModes transMode;       // режим передачи данных
        private Modbus modbus;                     // объект, реализующий протокол Modbus
        private RequestDelegate request;           // метод выполнения запроса
        private byte devAddr;                      // адрес устройства
        private List<Modbus.ElemGroup> elemGroups; // активные запрашиваемые группы элементов
        private int elemGroupCnt;                  // количество активных групп элементов
        private HashSet<int> floatSignals;         // множество сигналов, форматируемых как вещественное число


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpModbusLogic(int number)
            : base(number)
        {
            modbus = new Modbus();
        }


        /// <summary>
        /// Получить из общих свойств линии связи или создать словарь шаблонов устройств
        /// </summary>
        private Dictionary<string, Modbus.DeviceModel> GetTemplates()
        {
            Dictionary<string, Modbus.DeviceModel> templates = CommonProps.ContainsKey("Templates") ?
                CommonProps["Templates"] as Dictionary<string, Modbus.DeviceModel> : null;

            if (templates == null)
            {
                templates = new TemplateDict();
                CommonProps.Add("Templates", templates);
            }

            return templates;
        }

        /// <summary>
        /// Установить значения тегов КП в соответствии со значениями элементов группы
        /// </summary>
        private void SetTagsData(Modbus.ElemGroup elemGroup)
        {
            int len = elemGroup.ElemVals.Length;
            for (int i = 0, j = elemGroup.StartKPTagInd + i; i < len; i++, j++)
                SetCurData(j, elemGroup.GetElemVal(i), BaseValues.CnlStatuses.Defined);
        }

        /// <summary>
        /// Установка неопределённого статуса тегов КП, соотвующих элементам группы
        /// </summary>
        private void InvalTagsData(Modbus.ElemGroup elemGroup)
        {
            if (elemGroup == null)
            {
                int len = KPTags == null ? 0 : KPTags.Length;
                for (int i = 0; i < len; i++)
                    SetCurData(i, curData[i].Val, BaseValues.CnlStatuses.Undefined);
            }
            else
            {
                int len = elemGroup.ElemVals.Length;
                for (int i = 0, j = elemGroup.StartKPTagInd + i; i < len; i++, j++)
                    SetCurData(j, curData[j].Val, BaseValues.CnlStatuses.Undefined);
            }
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

            if (deviceModel == null)
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
                    Modbus.ElemGroup elemGroup = elemGroups[elemGroupInd];
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
                            InvalTagsData(elemGroup);
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
                Modbus.Cmd modbusCmd = deviceModel.FindCmd(cmd.CmdNum);

                if (modbusCmd != null &&
                    (modbusCmd.Multiple && (cmd.CmdTypeID == BaseValues.CmdTypes.Standard || 
                    cmd.CmdTypeID == BaseValues.CmdTypes.Binary) ||
                    !modbusCmd.Multiple && cmd.CmdTypeID == BaseValues.CmdTypes.Standard))
                {
                    // формирование команды Modbus
                    if (modbusCmd.Multiple)
                    {
                        modbusCmd.Data = cmd.CmdTypeID == BaseValues.CmdTypes.Standard ? 
                            BitConverter.GetBytes(cmd.CmdVal) : cmd.CmdData;
                    }
                    else
                    {
                        modbusCmd.Value = modbusCmd.TableType == Modbus.TableTypes.HoldingRegisters ?
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
            deviceModel = null;
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
                Dictionary<string, Modbus.DeviceModel> templates = GetTemplates();
                if (templates.ContainsKey(fileName))
                {
                    // создание модели устройства на основе модели, загруженной ранее
                    Modbus.DeviceModel template = templates[fileName];
                    if (template != null)
                    {
                        deviceModel = new Modbus.DeviceModel();
                        deviceModel.CopyFrom(template);
                    }
                }
                else
                {
                    WriteToLog(string.Format(Localization.UseRussian ? 
                        "{0} Загрузка шаблона устройства из файла {1}" :
                        "{0} Load device template from file {1}", CommUtils.GetNowDT(), fileName));
                    Modbus.DeviceModel template = new Modbus.DeviceModel();
                    string errMsg;

                    if (template.LoadTemplate(AppDirs.ConfigDir + fileName, out errMsg))
                    {
                        deviceModel = template;
                        templates.Add(fileName, template);
                    }
                    else
                    {
                        WriteToLog(errMsg);
                        templates.Add(fileName, null);
                    }
                }
            }

            if (deviceModel != null)
            {
                elemGroups = deviceModel.GetActiveElemGroups();
                elemGroupCnt = elemGroups.Count;
            }

            // инициализация тегов КП на основе модели устройства
            if (deviceModel.ElemGroups.Count > 0)
            {
                List<TagGroup> tagGroups = new List<TagGroup>();
                int tagInd = 0;

                foreach (Modbus.ElemGroup elemGroup in deviceModel.ElemGroups)
                {
                    TagGroup tagGroup = new TagGroup(elemGroup.Name);
                    tagGroups.Add(tagGroup);
                    elemGroup.StartKPTagInd = tagInd;

                    foreach (Modbus.Elem elem in elemGroup.Elems)
                    {
                        int signal = ++tagInd;
                        tagGroup.KPTags.Add(new KPTag(signal, elem.Name));
                        if (elem.ElemType == Modbus.ElemTypes.Float)
                            floatSignals.Add(signal);
                    }
                }

                InitKPTags(tagGroups);
                CanSendCmd = deviceModel.Cmds.Count > 0;
            }
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // получение режима передачи данных
            transMode = CustomParams.GetEnumParam("TransMode", false, Modbus.TransModes.RTU);

            // настройка библиотеки в зависимости от режима передачи данных
            switch (transMode)
            {
                case Modbus.TransModes.RTU:
                    request += modbus.RtuRequest;
                    break;
                case Modbus.TransModes.ASCII:
                    request += modbus.AsciiRequest;
                    break;
                default: // Modbus.TransModes.TCP
                    request += modbus.TcpRequest;
                    break;
            }

            // настройка объекта, реализующего протокол Modbus
            modbus.Timeout = ReqParams.Timeout;
            modbus.WriteToLog = WriteToLog;

            // формирование PDU и ADU
            if (deviceModel != null)
            {
                devAddr = (byte)Address;

                foreach (Modbus.ElemGroup elemGroup in deviceModel.ElemGroups)
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
            if (transMode == Modbus.TransModes.ASCII)
                Connection.NewLine = Modbus.CRLF;
            modbus.Connection = Connection;
        }
    }
}
