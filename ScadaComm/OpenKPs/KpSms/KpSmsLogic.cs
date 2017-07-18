/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : KpSms
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2009
 * Modified : 2016
 * 
 * Description
 * Sending and receiving SMS messages using AT commands.
 */

using Scada.Comm.Channels;
using Scada.Comm.Devices.AB;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic
    /// <para>Логика работы КП</para>
    /// </summary>
    public sealed class KpSmsLogic : KPLogic
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        private class Message
        {
            public int Index;          // индекс
            public int Status;         // статус: 0 - не прочитано, 1 - прочитано, 2 - не отправлено, 3 - отправлено
            public int Length;         // длина PDU без учёта номера центра сообщений

            public string Phone;       // телефонный номер отправителя
            public DateTime TimeStamp; // временная метка центра сообщений
            public string Text;        // текст сообщения
            public object[] Reference; // ссылка на сообщение в списке общих свойств линии связи

            public Message()
            {
                Index = 0;
                Status = 0;
                Length = 0;

                Phone = "";
                TimeStamp = DateTime.MinValue;
                Text = "";
                Reference = null;
            }
        }

        /// <summary>
        /// Список сообщений для общих свойств линии связи
        /// </summary>
        private class MessageObjList : List<object[]>
        {
            public override string ToString()
            {
                return "List of " + Count + " messages";
            }
        }

        // Максимальное значение счётчика событий КП
        private const int MaxEventCount = 999999;
        // Окончание строки при обмене данными с модемом
        private const string NewLine = "\x0D\x0A"; // 0D0A
        // Условие остановки считывания данных при получении OK
        private readonly Connection.TextStopCondition OkStopCond = new Connection.TextStopCondition("OK");
        // Условие остановки считывания данных при получении OK или ERROR
        private readonly Connection.TextStopCondition OkErrStopCond = new Connection.TextStopCondition("OK", "ERROR");

        private bool primary;               // основной КП на линии связи, обмен данными с GSM-терминалом
        private AB.AddressBook addressBook; // адресная книга
        List<Message> messageList;          // список сообщений, полученных GSM-терминалом


        /// <summary>
        /// Конструктор
        /// </summary>
        public KpSmsLogic(int number)
            : base(number)
        {
            primary = false;
            addressBook = null;
            messageList = new List<Message>();            
            CanSendCmd = true;

            List<KPTag> kpTags = new List<KPTag>();
            if (Localization.UseRussian)
            {
                kpTags.Add(new KPTag(1, "Связь"));
                kpTags.Add(new KPTag(2, "Кол-во событий"));
            }
            else
            {
                kpTags.Add(new KPTag(1, "Connection"));
                kpTags.Add(new KPTag(2, "Event count"));
            }
            InitKPTags(kpTags);
        }


        /// <summary>
        /// Декодировать телефонный номер
        /// </summary>
        private static string DecodePhone(string phoneNumber)
        {
            StringBuilder result = new StringBuilder();
            if (phoneNumber.StartsWith("91"))
                result.Append("+");

            for (int i = 2; i < phoneNumber.Length; i += 2)
            {
                if (i + 1 < phoneNumber.Length)
                {
                    char c = phoneNumber[i + 1];
                    if ('0' <= c && c <= '9') 
                        result.Append(c);

                    c = phoneNumber[i];
                    if ('0' <= c && c <= '9') 
                        result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Закодировать телефонный номер
        /// </summary>
        private static string EncodePhone(string phoneNumber)
        {
            StringBuilder result = new StringBuilder();
            int phoneLen = phoneNumber.Length;

            if (phoneLen > 0)
            {
                if (phoneNumber[0] == '+')
                {
                    phoneNumber = phoneNumber.Substring(1);
                    result.Append("91");
                    phoneLen--;
                }
                else
                    result.Append("81");

                int i = 1;
                while (i < phoneLen)
                {
                    result.Append(phoneNumber[i]);
                    result.Append(phoneNumber[i - 1]);
                    i += 2;
                }
                if (i == phoneLen)
                {
                    result.Append('F');
                    result.Append(phoneNumber[i - 1]);
                }
            }

            return phoneLen.ToString("X2") + result.ToString();
        }

        /// <summary>
        /// Преобразовать символьную запись 16-ричного числа в байт
        /// </summary>
        private static byte HexToByte(string s)
        {
            try { return (byte)int.Parse(s, NumberStyles.HexNumber);}
            catch { return 0; }
        }

        /// <summary>
        /// Декодировать текст в 7-битной кодировке
        /// </summary>
        private static string Decode7bitText(string text)
        {
            byte[] MaskL = new byte[] {0xFE, 0xFC, 0xF8, 0xF0, 0xE0, 0xC0, 0x80};
            byte[] MaskR = new byte[] {0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F};

            // замена пар символов на их 16-ричные значения и запись в буфер
            byte[] buf = new byte[text.Length / 2];
            int bufPos = 0;
            for (int i = 0; i < text.Length - 1; i += 2)
                buf[bufPos++] = HexToByte(text.Substring(i, 2));

            // расшифровка
            byte[] result = new byte[text.Length];
            int resPos = 0;
            byte part = 0;
            byte bit = 7;

            for (int i = 0; i < bufPos; i++)
            {
                byte b = buf[i];
                byte sym = (byte)((part >> (bit + 1)) | ((b & MaskR[bit - 1]) << (7 - bit)));
                part = (byte)(b & MaskL[bit - 1]);
                result[resPos++] = sym;

                if (--bit == 0)
                {
                    sym = (byte)((b & 0xFE) >> 1);
                    part = 0;

                    result[resPos++] = sym;
                    bit = 7;
                }
            }

            return resPos > 0 ? new string(Encoding.Default.GetChars(result, 0, resPos)) : "";
        }

        /// <summary>
        /// Закодировать текст в 7-битную кодировку
        /// </summary>
        private static List<byte> Encode7bitText(string text)
        {
            List<byte> result = new List<byte>();
            byte[] bytes = Encoding.Default.GetBytes(text);

            byte bit = 7; // от 7 до 1
            int i = 0;
            int len = bytes.Length;
            while (i < len)
            {
                byte sym = (byte)(bytes[i] & 0x7F);
                byte nextSym = i < len - 1 ? (byte)(bytes[i + 1] & 0x7F) : (byte)0;
                byte code = (byte)((sym >> (7 - bit)) | (nextSym << bit));

                if (bit == 1)
                {
                    i++;
                    bit = 7;
                }
                else
                    bit--;

                result.Add(code);
                i++;
            }

            return result;
        }

        /// <summary>
        /// Декодировать текст в 8-битной кодировке
        /// </summary>
        private static string Decode8bitText(string text)
        {
            byte[] buf = new byte[text.Length / 2];
            int bufPos = 0;

            for (int i = 0; i < text.Length - 1; i += 2)
                buf[bufPos++] = HexToByte(text.Substring(i, 2));

            return bufPos > 0 ? new string(Encoding.Default.GetChars(buf, 0, bufPos)) : "";
        }

        /// <summary>
        /// Декодировать текст в кодировке Unicode
        /// </summary>
        private static string DecodeUnicodeText(string text)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length - 3; i += 4)
                try
                {
                    int val = int.Parse(text.Substring(i, 4), NumberStyles.HexNumber);
                    result.Append(char.ConvertFromUtf32(val));
                }
                catch { }

            return result.ToString();
        }

        /// <summary>
        /// Закодировать текст в кодировку Unicode
        /// </summary>
        private static List<byte> EncodeUnicodeText(string text)
        {
            List<byte> result = new List<byte>();

            for (int i = 0; i < text.Length; i++)
            {
                int val = char.ConvertToUtf32(text, i);
                result.Add((byte)(val >> 8 & 0xFF));
                result.Add((byte)(val & 0xFF));
            }

            return result;
        }

        /// <summary>
        /// Создать Protocol Data Unit для передачи сообщения
        /// </summary>
        private static string MakePDU(string phoneNumber, string msgText, out int pduLen)
        {
            // выбор кодировки
            bool sevenBit = true;
            for (int i = 0; i < msgText.Length && sevenBit; i++)
            {
                char c = msgText[i];
                if ((c < ' ' || c > 'z') && c != '\n')
                    sevenBit = false;
            }

            // установка длины текста, допустимой для передачи
            const int Max7bitMsgLen = 160;
            const int MaxUnicodeMsgLen = 70;
            int maxMsgLen = sevenBit ? Max7bitMsgLen : MaxUnicodeMsgLen;

            if (msgText.Length > maxMsgLen)
                msgText = msgText.Substring(0, maxMsgLen);

            // формирование PDU
            StringBuilder pdu = new StringBuilder();
            pdu.Append("00");                          // Service Center Adress (SCA)
            pdu.Append("01");                          // PDU-type
            pdu.Append("00");                          // Message Reference (MR)
            pdu.Append(EncodePhone(phoneNumber));      // Destination Adress (DA)
            pdu.Append("00");                          // Protocol Identifier (PID)

            byte dcs;                                  // Data Coding Scheme (DCS)
            List<byte> ud;                             // User Data (UD)
            byte udl;                                  // User Data Length (UDL)

            if (sevenBit)
            {
                dcs = 0x00;
                ud = Encode7bitText(msgText);
                udl = (byte)msgText.Length;
            }
            else
            {
                dcs = 0x08;
                ud = EncodeUnicodeText(msgText);
                udl = (byte)ud.Count;
            }

            pdu.Append(dcs.ToString("X2"));
            pdu.Append(udl.ToString("X2"));

            foreach (byte b in ud)
                pdu.Append(b.ToString("X2"));

            pduLen = (pdu.Length - 2) / 2;
            return pdu.ToString();
        }


        /// <summary>
        /// Заполнить список сообщений по входным данным, полученным от GSM-терминала
        /// </summary>
        private bool FillMessageList(List<string> inData, out string logMsg)
        {
            bool result = true;
            StringBuilder logMsgSB = new StringBuilder();
            messageList.Clear();

            int i = 1;
            int lineCnt = inData.Count;
            while (i <= lineCnt)
            {
                string line = inData[i - 1].Trim();
                if (line.StartsWith("+CMGL: ") && line.Length > 7)
                {
                    // получение индекса, статуса и длины сообщения
                    Message msg = new Message();
                    bool paramsOK = false;
                    string[] parts = line.Substring(7).Split(new char[] { ',' }, StringSplitOptions.None);
                    if (parts.Length >= 3)
                    {
                        int val1, val2, val3;
                        if (int.TryParse(parts[0], out val1) && int.TryParse(parts[1], out val2) &&
                            int.TryParse(parts[parts.Length - 1], out val3))
                        {
                            paramsOK = true;
                            msg.Index = val1;
                            msg.Status = val2;
                            msg.Length = val3;
                            i++;
                        }
                    }

                    // расшифровка PDU
                    if (paramsOK)
                    {
                        if (i <= lineCnt)
                        {
                            line = inData[i - 1].Trim(); // PDU
                            try
                            {
                                int scaLen = int.Parse(line.Substring(0, 2));         // длина номера центра сообщений
                                int oaPos = scaLen * 2 + 4;                           // позиция номера отправителя

                                if (msg.Length == (line.Length - oaPos + 2) / 2)
                                {
                                    int oaLen = int.Parse(line.Substring(oaPos, 2),
                                        NumberStyles.HexNumber);                      // длина номера отправителя
                                    if (oaLen % 2 > 0) oaLen++;
                                    msg.Phone = DecodePhone(line.Substring(oaPos + 2, oaLen + 2));

                                    int sctsPos = oaPos + oaLen + 8;                  // позиция временной метки
                                    msg.TimeStamp = new DateTime(int.Parse("20" + line[sctsPos + 1] + line[sctsPos]),
                                        int.Parse(line[sctsPos + 3].ToString() + line[sctsPos + 2]),
                                        int.Parse(line[sctsPos + 5].ToString() + line[sctsPos + 4]),
                                        int.Parse(line[sctsPos + 7].ToString() + line[sctsPos + 6]),
                                        int.Parse(line[sctsPos + 9].ToString() + line[sctsPos + 8]),
                                        int.Parse(line[sctsPos + 11].ToString() + line[sctsPos + 10]));

                                    string dcs = line.Substring(sctsPos - 2, 2);      // кодировка
                                    int udPos = sctsPos + 16;                         // позиция текста сообщения
                                    int udl = int.Parse(line.Substring(udPos - 2, 2),
                                        NumberStyles.HexNumber);                      // длина текста сообщения
                                    string ud = line.Substring(udPos);                // текст сообщения

                                    // проверка длины текста сообщения, разные модемы вычиляют UDL по-разному
                                    if (!(dcs == "00" && ud.Length * 4 / 7 == udl || 
                                        dcs != "00" && ud.Length == udl * 2))
                                    {
                                        logMsgSB.AppendLine(string.Format(Localization.UseRussian ?
                                            "Предупреждение в строке {0}: некорректная длина текста сообщения" :
                                            "Warning in line {0}: incorrect message length", i));
                                    }

                                    // декодирование сообщения
                                    if (dcs == "00")
                                        msg.Text = Decode7bitText(ud);
                                    else if (dcs == "F6")
                                        msg.Text = Decode8bitText(ud);
                                    else if (dcs == "08")
                                        msg.Text = DecodeUnicodeText(ud);

                                    messageList.Add(msg);
                                }
                                else
                                {
                                    result = false;
                                    logMsgSB.AppendLine(string.Format(Localization.UseRussian ?
                                        "Ошибка в строке {0}: некорректная длина PDU" :
                                        "Error in line {0}: incorrect PDU length", i));
                                }
                            }
                            catch
                            {
                                result = false;
                                logMsgSB.AppendLine(string.Format(Localization.UseRussian ?
                                    "Ошибка в строке {0}: невозможно расшифровать PDU" :
                                    "Error in line {0}: unable to decode PDU", i));
                            }
                        }
                        else
                        {
                            result = false;
                            logMsgSB.AppendLine(Localization.UseRussian ?
                                "Ошибка: некорректное завершение входных данных" :
                                "Error: incorrect termination of the input data");
                        }
                    }
                    else
                    {
                        result = false;
                        logMsgSB.AppendLine(string.Format(Localization.UseRussian ?
                            "Ошибка в строке {0}: некорректные параметры сообщения" :
                            "Error in line {0}: incorrect message parameters", i));
                    }
                }

                i++;
            }

            logMsg = logMsgSB.ToString();
            return result;
        }

        /// <summary>
        /// Получить из общих свойств линии связи или создать список сообщений, представленных в виде object[]
        /// </summary>
        private List<object[]> GetMessageObjList()
        {
            List<object[]> messages = CommonProps.ContainsKey("Messages") ? 
                CommonProps["Messages"] as List<object[]> : null;

            if (messages == null)
            {
                messages = new MessageObjList();
                CommonProps.Add("Messages", messages);
            }

            return messages;
        }

        /// <summary>
        /// Преобразовать сообщение в object[]
        /// </summary>
        /// <remarks>Вид результата: object[] {индекс, статус, телефон, время, текст, обработано}</remarks>
        private object[] ConvertMessage(Message message)
        {
            object[] msgObjArr = new object[6];

            if (message == null)
            {
                msgObjArr[0] = 0;
                msgObjArr[1] = 0;
                msgObjArr[2] = "";
                msgObjArr[3] = DateTime.MinValue;
                msgObjArr[4] = "";
                msgObjArr[5] = true;
            }
            else
            {
                msgObjArr[0] = message.Index;
                msgObjArr[1] = message.Status;
                msgObjArr[2] = message.Phone.Clone();
                msgObjArr[3] = message.TimeStamp;
                msgObjArr[4] = message.Text.Clone();
                msgObjArr[5] = false;
            }

            return msgObjArr;
        }

        /// <summary>
        /// Записать событие в список событий КП
        /// </summary>
        private void WriteEvent(DateTime timeStamp, string phone, string text, ref int eventCnt)
        {
            eventCnt++;
            KPEvent ev = new KPEvent(timeStamp, Number, KPTags[1]);
            ev.NewData = new SrezTableLight.CnlData(curData[1].Val + eventCnt, 1);
            ev.Descr = phone == "" ? text : phone + "; " + text;
            AddEvent(ev);
        }

        /// <summary>
        /// Увеличение параметра КП "Кол-во событий"
        /// </summary>
        private void IncEventCount(int eventCnt)
        {
            double newVal = curData[1].Val + eventCnt;
            if (newVal > MaxEventCount) 
                newVal = 0;
            SetCurData(1, newVal, 1);
        }


        /// <summary>
        /// Сеанс опроса основного КП
        /// </summary>
        private void PrimarySession()
        {
            Connection.WriteToLog = WriteToLog;
            Connection.NewLine = NewLine;
            int tryNum;

            // отключение эхо
            if (WorkState != WorkStates.Normal)
            {
                lastCommSucc = false;
                tryNum = 0;
                while (RequestNeeded(ref tryNum))
                {
                    WriteToLog(Localization.UseRussian ? "Отключение эхо" : "Set echo off");
                    Connection.WriteLine("ATE0");
                    Connection.ReadLines(ReqParams.Timeout, OkStopCond, out lastCommSucc);

                    FinishRequest();
                    tryNum++;
                }
            }

            // сброс вызова
            if (lastCommSucc)
            {
                lastCommSucc = false;
                tryNum = 0;
                while (RequestNeeded(ref tryNum))
                {
                    WriteToLog(Localization.UseRussian ? "Сброс вызова" : "Drop call");
                    Connection.WriteLine("ATH" /*"AT+CHUP"*/);
                    Connection.ReadLines(ReqParams.Timeout, OkStopCond, out lastCommSucc);

                    FinishRequest();
                    tryNum++;
                }
            }

            // обработка и удаление сообщений, полученных ранее
            int eventCnt = 0; // количество созданных событий

            if (lastCommSucc)
            {
                foreach (Message msg in messageList)
                {
                    // обработка сообщения, если оно не обработано другими КП
                    try
                    {
                        object[] msgObjArr = msg.Reference;
                        if (!(bool)msgObjArr[5] /*сообщение не обработано*/ && 
                            (int)msgObjArr[1] <= 1 /*принятое сообщение*/)
                        {
                            // запись события
                            WriteEvent(msg.TimeStamp, msg.Phone, msg.Text, ref eventCnt);
                            msgObjArr[5] = true;
                        }
                    }
                    catch
                    {
                        WriteToLog((Localization.UseRussian ? 
                            "Ошибка при обработке сообщения " : 
                            "Error processing message ") + msg.Index);
                    }

                    // удаление сообщений из памяти GSM-терминала
                    bool deleteComplete = false;
                    tryNum = 0;
                    while (tryNum < ReqTriesCnt && !deleteComplete && !Terminated)
                    {
                        WriteToLog((Localization.UseRussian ? "Удаление сообщения " : "Delete message ") + msg.Index);
                        Connection.WriteLine("AT+CMGD=" + msg.Index);
                        Connection.ReadLines(ReqParams.Timeout, OkStopCond, out deleteComplete);

                        FinishRequest();
                        tryNum++;
                    }
                    lastCommSucc = lastCommSucc && deleteComplete;
                }

                messageList.Clear();
                GetMessageObjList().Clear();
            }

            IncEventCount(eventCnt);
            if (lastCommSucc)
                WriteToLog((Localization.UseRussian ? 
                    "Количество полученных сообщений: " : 
                    "Received message count: ") + eventCnt);

            // запрос списка сообщений
            if (lastCommSucc)
            {
                lastCommSucc = false;
                tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    WriteToLog(Localization.UseRussian ? "Запрос списка сообщений" : "Request message list");
                    Connection.WriteLine("AT+CMGL=4");
                    List<string> inData = Connection.ReadLines(ReqParams.Timeout, OkStopCond, out lastCommSucc);

                    // расшифровка сообщений
                    if (lastCommSucc)
                    {
                        string logMsg;
                        if (!FillMessageList(inData, out logMsg))
                            lastCommSucc = false;

                        if (logMsg != "")
                            WriteToLog(logMsg);
                    }

                    FinishRequest();
                    tryNum++;
                }

                // запись сообщений в общие свойства линии связи
                List<object[]> msgObjList = GetMessageObjList();
                foreach (Message msg in messageList)
                {
                    object[] msgObjArr = ConvertMessage(msg);
                    msg.Reference = msgObjArr;
                    msgObjList.Add(msgObjArr);
                }
            }

            // определение наличия связи
            double newVal = lastCommSucc ? 1.0 : -1.0;
            SetCurData(0, newVal, 1);
        }

        /// <summary>
        /// Сеанс опроса не основного КП
        /// </summary>
        private void SecondarySession()
        {
            // обработка сообщений из общих свойств линии связи
            List<object[]> msgObjList = GetMessageObjList();
            int eventCnt = 0; // количество созданных событий

            foreach (object[] msgObjArr in msgObjList)
            {
                try
                {
                    if (!(bool)msgObjArr[5]) // сообщение не обработано
                    {
                        if ((string)msgObjArr[2] == CallNum /*совпадение телефонного номера*/ &&
                            (int)msgObjArr[1] <= 1 /*принятое сообщение*/)
                        {
                            // запись события
                            WriteEvent((DateTime)msgObjArr[3], "", (string)msgObjArr[4], ref eventCnt);
                            msgObjArr[5] = true;
                        }
                    }
                }
                catch
                {
                    int index;
                    try { index = (int)msgObjArr[0]; }
                    catch { index = 0; }
                    WriteToLog((Localization.UseRussian ? 
                        "Ошибка при обработке сообщения" : 
                        "Error processing message") + (index > 0 ? " " + index : ""));
                }
            }

            IncEventCount(eventCnt);
            WriteToLog((Localization.UseRussian ? 
                "Количество полученных сообщений: " :
                "Received message count: ") + eventCnt);
        }

        /// <summary>
        /// Получить список телефонных номеров получателя, используя адресную книгу
        /// </summary>
        private List<string> GetPhoneNumbers(string recipient)
        {
            List<string> phoneNumbers = new List<string>();

            if (addressBook == null)
            {
                // добавление номера получателя напрямую
                phoneNumbers.Add(recipient);
            }
            else
            {
                // поиск телефонных номеров получателей в адресной книге
                AB.AddressBook.ContactGroup contactGroup = addressBook.FindContactGroup(recipient);
                if (contactGroup == null)
                {
                    AB.AddressBook.Contact contact = addressBook.FindContact(recipient);
                    if (contact == null)
                    {
                        // добавление номера получателя напрямую
                        phoneNumbers.Add(recipient);
                    }
                    else
                    {
                        // добавление номеров получателей из контакта
                        phoneNumbers.AddRange(contact.PhoneNumbers);
                    }
                }
                else
                {
                    // добавление номеров получателей из группы контактов
                    foreach (AB.AddressBook.Contact contact in contactGroup.Contacts)
                        phoneNumbers.AddRange(contact.PhoneNumbers);
                }
            }

            return phoneNumbers;
        }

        /// <summary>
        /// Отправить SMS по заданным номерам
        /// </summary>
        private bool SendMessages(string msgText, List<string> phoneNumbers)
        {
            bool responseOK = true;
            Connection.WriteToLog = WriteToLog;
            Connection.NewLine = NewLine;

            foreach (string phoneNumber in phoneNumbers)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Отправка SMS на номер {0}" :
                    "Send message to {0}", phoneNumber));

                int pduLen;
                string pdu = MakePDU(phoneNumber, msgText, out pduLen);
                Connection.WriteLine("AT+CMGS=" + pduLen);
                Thread.Sleep(100);

                try
                {
                    Connection.NewLine = "\x1A";
                    Connection.WriteLine(pdu);
                }
                finally
                {
                    Connection.NewLine = NewLine;
                }

                bool stopReceived;
                Connection.ReadLines(ReqParams.Timeout, OkStopCond, out stopReceived);
                if (!stopReceived)
                    responseOK = false;
                Thread.Sleep(ReqParams.Delay);
            }

            return responseOK;
        }

        /// <summary>
        /// Преобразовать данные тега КП в строку
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0)
            {
                if (signal == 1)
                    return tagData.Val > 0 ?
                        (Localization.UseRussian ? "Есть" : "Yes") :
                        (Localization.UseRussian ? "Нет" : "No");
                else if (signal == 2)
                    return ((int)tagData.Val).ToString();
            }

            return base.ConvertTagDataToStr(signal, tagData);
        }


        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (primary)
                PrimarySession();
            else
                SecondarySession();

            CalcSessStats();
        }

        /// <summary>
        /// Отправить команду ТУ
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);
            lastCommSucc = false;

            if (cmd.CmdTypeID == BaseValues.CmdTypes.Binary && (cmd.CmdNum == 1 || cmd.CmdNum == 2))
            {
                string cmdDataStr = cmd.GetCmdDataStr();
                if (cmdDataStr != "")
                {
                    Connection.WriteToLog = WriteToLog;
                    if (cmd.CmdNum == 1)
                    {
                        // извлечение получателя и текста сообщения из данных команды
                        // для основного КП: <группа, контакт или телефон>;<текст сообщения> 
                        // для остальных КП: <текст сообщения> 
                        string recipient;
                        string msgText;

                        if (primary)
                        {
                            int scPos = cmdDataStr.IndexOf(';');
                            recipient = scPos >= 0 ? cmdDataStr.Substring(0, scPos).Trim() : "";
                            msgText = scPos >= 0 ? cmdDataStr.Substring(scPos + 1) : cmdDataStr;
                        }
                        else
                        {
                            recipient = CallNum.Trim();
                            msgText = cmdDataStr;
                        }

                        if (recipient == "" || msgText == "")
                        {
                            WriteToLog(Localization.UseRussian ?
                                "Получатель или текст сообщения отсутствует" :
                                "Recipient or message text is missing");
                        }
                        else
                        {
                            // формирование списка телефонных номеров для отправки сообщений
                            List<string> phoneNumbers = GetPhoneNumbers(recipient);

                            // отправка сообщений
                            if (phoneNumbers.Count > 0)
                                lastCommSucc = SendMessages(msgText, phoneNumbers);
                            else
                                WriteToLog(string.Format(Localization.UseRussian ?
                                    "\"{0}\" не содержит телефонных номеров" :
                                    "\"{0}\" does not contain phone numbers", recipient));
                        }
                    }
                    else
                    {
                        // произвольная AT-команда
                        Connection.WriteLine(cmdDataStr);
                        Connection.ReadLines(ReqParams.Timeout, OkErrStopCond, out lastCommSucc);
                        Thread.Sleep(ReqParams.Delay);
                    }
                }
                else
                {
                    WriteToLog(CommPhrases.NoCmdData);
                }
            }
            else
            {
                WriteToLog(CommPhrases.IllegalCommand);
            }

            CalcCmdStats();
        }
        
        /// <summary>
        /// Выполнить действия при запуске линии связи
        /// </summary>
        public override void OnCommLineStart()
        {
            // определение, является ли КП основным на линии связи
            // основным автоматически считается первый КП на линии связи
            object primaryObj;
            if (CommonProps.TryGetValue("KpSmsPrimary", out primaryObj))
            {
                primary = false;
                addressBook = null;
            }
            else
            {
                primary = true;
                CommonProps.Add("KpSmsPrimary", Caption);

                // загрузка адресной книги
                if (!AbUtils.LoadAddressBook(AppDirs.ConfigDir, WriteToLog, out addressBook))
                    addressBook = null;
            }
        }

        /// <summary>
        /// Выполнить действия после установки соединения
        /// </summary>
        public override void OnConnectionSet()
        {
            // установка символа окончания строки
            if (Connection != null)
                Connection.NewLine = "\x0D";
        }
    }
}