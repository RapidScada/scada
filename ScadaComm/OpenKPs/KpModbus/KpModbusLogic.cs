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
 * Modified : 2014
 * 
 * Description
 * Interacting with controllers via Modbus protocol.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Scada.Data;

namespace Scada.Comm.KP
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
        /// Делегат выполнения сеанса опроса
        /// </summary>
        private delegate void SessionDelegate();
        /// <summary>
        /// Делегат отправки команды
        /// </summary>
        private delegate void SendCmdDelegate(Modbus.Cmd cmd);
        /// <summary>
        /// Делегат выполнения запроса
        /// </summary>
        private delegate bool RequestDelegate(Modbus.DataUnit dataUnit);
        
        /// <summary>
        /// Периодичность попыток установки TCP-соединения, с
        /// </summary>
        private const int TcpConnectPer = 5;

        private Modbus.DeviceModel deviceModel; // модель устройства, используемая данным КП
        private Modbus.TransModes transMode;    // режим передачи данных
        private Modbus modbus;                  // объект, реализующий протокол Modbus
        private SessionDelegate session;        // метод выполнения сеанса опроса
        private SendCmdDelegate sendCmd;        // метод отправки команды
        private RequestDelegate request;        // метод выполнения запроса
        private byte devAddr;                   // адрес устройства
        private int tryNum;                     // номер попытки запроса
        private int elemGroupCnt;               // количество групп элементов
        private HashSet<int> floatSignals;      // множество сигналов, форматируемых как вещественное число

        private TcpClient tcpClient;            // TCP-клиент
        private NetworkStream netStream;        // поток данных TCP-клиента
        private DateTime connectDT;             // время установки TCP-соединения


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpModbusLogic(int number)
            : base(number)
        {
            modbus = new Modbus();
            tcpClient = null;
            netStream = null;
            connectDT = DateTime.MinValue;
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
        /// Установить TCP-соединение, если оно отсутствует
        /// </summary>
        private bool TcpConnect()
        {
            if (tcpClient == null)
            {
                // создание TCP-клиента
                tcpClient = new TcpClient();
                tcpClient.NoDelay = true;
                tcpClient.SendTimeout = KPReqParams.Timeout;
                tcpClient.ReceiveTimeout = KPReqParams.Timeout;
            }

            if (tcpClient.Connected)
            {
                return true;
            }
            else
            {
                if ((LastSessDT - connectDT).TotalSeconds >= TcpConnectPer || 
                    LastSessDT < connectDT /*время переведено назад*/)
                {
                    WriteToLog((Localization.UseRussian ? "Установка TCP-соединения с " : 
                        "Establish a TCP connection with ") + CallNum);
                    connectDT = LastSessDT;

                    try
                    {
                        // определение IP-адреса и номера порта
                        IPAddress addr;
                        int port;
                        string[] parts = CallNum.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 1)
                            addr = IPAddress.Parse(parts[0]);
                        else
                            throw new Exception(Localization.UseRussian ? 
                                "Не определён IP-адрес" : "IP address is undefined");

                        if (!(parts.Length >= 2 && int.TryParse(parts[1], out port)))
                            port = Modbus.DefTcpPort;

                        // соединение
                        tcpClient.Connect(addr, port);
                        netStream = tcpClient.GetStream();
                        modbus.NetStream = netStream;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteToLog((Localization.UseRussian ? "Ошибка при установке TCP-соединения: " :
                            "Error establishing TCP connection: ") + ex.Message);
                        return false;
                    }
                }
                else
                {
                    WriteToLog(string.Format(Localization.UseRussian ? 
                        "Попытка установки TCP-соединения может быть не ранее, чем через {0} с после предыдущей" :
                        "Attempt to establish TCP connection can not be earlier than {0} seconds after the previous", 
                        TcpConnectPer));
                    return false;
                }
            }
        }

        /// <summary>
        /// Разорвать TCP-соединение
        /// </summary>
        private void TcpDisconnect()
        {
            WriteToLog(Localization.UseRussian ? "Разрыв TCP-соединения" : "TCP disconnect");

            if (netStream != null)
            {
                netStream.Close();
                netStream = null;
                modbus.NetStream = null;
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }

        /// <summary>
        /// Получить необходимость выполнения запроса
        /// </summary>
        private bool RequestNeeded()
        {
            return !lastCommSucc && tryNum < CommLineParams.TriesCnt && !Terminated;
        }

        /// <summary>
        /// Установить значения параметров КП в соответствии со значениями элементов группы
        /// </summary>
        private void SetParamsData(Modbus.ElemGroup elemGroup)
        {
            int len = elemGroup.ElemVals.Length;
            for (int i = 0, j = elemGroup.StartParamInd + i; i < len; i++, j++)
                SetParamData(j, elemGroup.GetElemVal(i), BaseValues.ParamStat.Defined);
        }

        /// <summary>
        /// Установка неопределённого статуса параметров КП, соотвующих элементам группы
        /// </summary>
        private void InvalParamsData(Modbus.ElemGroup elemGroup)
        {
            if (elemGroup == null)
            {
                int len = KPParams == null ? 0 : KPParams.Length;
                for (int i = 0; i < len; i++)
                    SetParamData(i, CurData[i].Val, BaseValues.ParamStat.Undefined);
            }
            else
            {
                int len = elemGroup.ElemVals.Length;
                for (int i = 0, j = elemGroup.StartParamInd + i; i < len; i++, j++)
                    SetParamData(j, CurData[j].Val, BaseValues.ParamStat.Undefined);
            }
        }


        /// <summary>
        /// Выполнить сеанс опроса в режиме RTU или ASCII
        /// </summary>
        private void SerialSession()
        {
            int elemGroupInd = 0;

            while (elemGroupInd < elemGroupCnt && lastCommSucc)
            {
                Modbus.ElemGroup elemGroup = deviceModel.ElemGroups[elemGroupInd];
                lastCommSucc = false;
                tryNum = 0;

                while (RequestNeeded())
                {
                    // выполнение запроса
                    if (request(elemGroup))
                    {
                        lastCommSucc = true;
                        SetParamsData(elemGroup); // установка значений параметров
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
                    // установка неопределённого статуса параметров КП текущей и следующих групп, если запрос неудачный
                    while (elemGroupInd < elemGroupCnt)
                    {
                        InvalParamsData(elemGroup);
                        elemGroupInd++;
                    }
                }
            }
        }

        /// <summary>
        /// Выполнить сеанс опроса в режиме TCP
        /// </summary>
        private void TcpSession()
        {
            if (TcpConnect())
            {
                int elemGroupInd = 0; 

                while (elemGroupInd < elemGroupCnt && lastCommSucc)
                {
                    Modbus.ElemGroup elemGroup = deviceModel.ElemGroups[elemGroupInd];
                    lastCommSucc = false;
                    tryNum = 0;

                    while (RequestNeeded() && netStream != null)
                    {
                        // выполнение запроса
                        if (request(elemGroup))
                        {
                            lastCommSucc = true;
                            SetParamsData(elemGroup); // установка значений параметров
                        }

                        // разрыв соединения в случае ошибки работы TCP-сокета
                        if (modbus.NetStream == null)
                        {
                            netStream = null;
                            TcpDisconnect();
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
                        // установка неопределённого статуса параметров КП текущей и следующих групп, если запрос неудачный
                        while (elemGroupInd < elemGroupCnt)
                        {
                            InvalParamsData(elemGroup);
                            elemGroupInd++;
                        }
                    }
                }
            }
            else
            {
                InvalParamsData(null); // установка неопределённого статуса всех параметров КП
                Thread.Sleep(KPReqParams.Delay);
                lastCommSucc = false;
            }
        }

        /// <summary>
        /// Отправить команду в режиме RTU или ASCII
        /// </summary>
        private void SerialSendCmd(Modbus.Cmd cmd)
        {
            lastCommSucc = false;
            tryNum = 0;

            while (RequestNeeded())
            {
                // выполнение запроса
                if (request(cmd))
                    lastCommSucc = true;

                // завершение запроса
                FinishRequest();
                tryNum++;
            }
        }

        /// <summary>
        /// Отправить команду в режиме TCP
        /// </summary>
        private void TcpSendCmd(Modbus.Cmd cmd)
        {
            if (TcpConnect())
            {
                lastCommSucc = false;
                tryNum = 0;

                while (RequestNeeded() && netStream != null)
                {
                    // выполнение запроса
                    if (request(cmd))
                        lastCommSucc = true;

                    // разрыв соединения в случае ошибки работы TCP-сокета
                    if (modbus.NetStream == null)
                    {
                        netStream = null;
                        TcpDisconnect();
                    }

                    // завершение запроса
                    FinishRequest();
                    tryNum++;
                }
            }
            else
            {
                Thread.Sleep(KPReqParams.Delay);
                lastCommSucc = false;
            }
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
                Thread.Sleep(KPReqParams.Delay);
                lastCommSucc = false;
            }
            else
            {
                if (deviceModel.ElemGroups.Count > 0)
                {
                    session(); // выполнение запроса
                }
                else
                {
                    WriteToLog(Localization.UseRussian ?
                        "Отсутствуют элементы для запроса" : "No elements for request");
                    Thread.Sleep(KPReqParams.Delay);
                }
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
                    (modbusCmd.Multiple && (cmd.CmdType == CmdType.Standard || cmd.CmdType == CmdType.Binary) ||
                    !modbusCmd.Multiple && cmd.CmdType == CmdType.Standard))
                {
                    if (modbusCmd.Multiple)
                    {
                        modbusCmd.Data = cmd.CmdType == CmdType.Standard ? 
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
                    sendCmd(modbusCmd);
                }
                else
                {
                    lastCommSucc = false;
                    WriteToLog(Localization.UseRussian ? "Недопустимая команда" : "Illegal command");
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
            elemGroupCnt = 0;
            floatSignals = new HashSet<int>();
            string fileName = KPReqParams.CmdLine.Trim();

            if (fileName == "")
            {
                WriteToLog((Localization.UseRussian ? "Не задан шаблон устройства для " : 
                    "Template is undefined for the ") + Caption);
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
                        elemGroupCnt = deviceModel.ElemGroups.Count;
                    }
                }
                else
                {
                    WriteToLog((Localization.UseRussian ? "Загрузка шаблона устройства из файла " :
                        "Load device template from file ") + fileName);
                    Modbus.DeviceModel template = new Modbus.DeviceModel();
                    string errMsg;

                    if (template.LoadTemplate(ConfigDir + fileName, out errMsg))
                    {
                        deviceModel = template;
                        elemGroupCnt = deviceModel.ElemGroups.Count;
                        templates.Add(fileName, template);
                    }
                    else
                    {
                        WriteToLog(errMsg);
                        templates.Add(fileName, null);
                    }
                }
            }

            // инициализация параметров КП на основе модели устройства
            if (elemGroupCnt > 0)
            {
                ParamGroup[] paramGroups = new ParamGroup[elemGroupCnt];
                int paramInd = 0;
                int paramGroupInd = 0;

                foreach (Modbus.ElemGroup elemGroup in deviceModel.ElemGroups)
                {
                    int elemCnt = elemGroup.Elems.Count;
                    ParamGroup paramGroup = new ParamGroup(elemGroup.Name, elemCnt);
                    paramGroups[paramGroupInd++] = paramGroup;
                    elemGroup.StartParamInd = paramInd;

                    for (int i = 0; i < elemCnt; i++)
                    {
                        int signal = ++paramInd;
                        Modbus.Elem elem = elemGroup.Elems[i];

                        Param param = new Param(signal, elem.Name);
                        paramGroup.KPParams[i] = param;

                        if (elem.ElemType == Modbus.ElemTypes.Float)
                            floatSignals.Add(signal);
                    }
                }

                InitArrays(paramInd, elemGroupCnt);
                for (int i = 0; i < elemGroupCnt; i++)
                    ParamGroups[i] = paramGroups[i];
                CopyParamsFromGroups();
                CanSendCmd = deviceModel.Cmds.Count > 0;
            }
        }

        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // получение режима передачи данных
            try { transMode = (Modbus.TransModes)Enum.Parse(typeof(Modbus.TransModes), UserParams["TransMode"], true); }
            catch { transMode = Modbus.TransModes.RTU; }

            // настройка библиотеки в зависимости от режима передачи данных
            switch (transMode)
            {
                case Modbus.TransModes.RTU:
                    session += SerialSession;
                    sendCmd += SerialSendCmd;
                    request += modbus.RtuRequest;
                    break;
                case Modbus.TransModes.ASCII:
                    session += SerialSession;
                    sendCmd += SerialSendCmd;
                    request += modbus.AsciiRequest;
                    SerialPort.NewLine = Modbus.CRLF;
                    break;
                default: // Modbus.TransModes.TCP
                    session += TcpSession;
                    sendCmd += TcpSendCmd;
                    request += modbus.TcpRequest;
                    break;
            }

            // настройка объекта, реализующего протокол Modbus
            modbus.SerialPort = SerialPort;
            modbus.Timeout = KPReqParams.Timeout;
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
        /// Выполнить действия при завершении работы линии связи
        /// </summary>
        public override void OnCommLineTerminate()
        {
            // разрыв TCP-соединения
            if (tcpClient != null)
                TcpDisconnect();
        }

        /// <summary>
        /// Преобразовать данные параметра КП в строку
        /// </summary>
        public override string ParamDataToStr(int signal, ParamData paramData)
        {
            if (paramData.Stat > 0 && !floatSignals.Contains(signal))
                return CurData[signal - 1].Val.ToString("F0");
            else
                return base.ParamDataToStr(signal, paramData);
        }
    }
}
