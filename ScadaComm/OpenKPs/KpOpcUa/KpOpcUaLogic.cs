/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Device driver communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Scada.Comm.Devices.OpcUa;
using Scada.Comm.Devices.OpcUa.Config;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver communication logic.
    /// <para>Логика работы драйвера КП.</para>
    /// </summary>
    public class KpOpcUaLogic : KPLogic
    {
        /// <summary>
        /// Represents metadata about a subscription.
        /// </summary>
        private class SubscriptionTag
        {
            public SubscriptionTag(Subscription subscription)
            {
                Subscription = subscription;
                ItemsByNodeID = new Dictionary<string, ItemTag>();
            }

            public Subscription Subscription { get; private set; }
            public Dictionary<string, ItemTag> ItemsByNodeID { get; private set; }
        }

        /// <summary>
        /// Represents metadata about a monitored item.
        /// </summary>
        public class ItemTag
        {
            public ItemConfig ItemConfig { get; set; }
            public KPTag KPTag { get; set; }
        }

        /// <summary>
        /// Supported tag types.
        /// </summary>
        private enum TagType { Number, String, DateTime };


        /// <summary>
        /// The period of reconnecting to OPC server if a connection lost, ms
        /// </summary>
        private const int ReconnectPeriod = 10000;
        /// <summary>
        /// The delay before reconnect.
        /// </summary>
        private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(5);

        private readonly object opcLock;                      // synchronizes communication with OPC server
        private DeviceConfig deviceConfig;                    // the device driver configuration
        private bool autoAccept;                              // auto accept OPC server certificate
        private bool connected;                               // connection with OPC server is established
        private DateTime connAttemptDT;                       // the time stamp of a connection attempt
        private Session opcSession;                           // the OPC session
        private SessionReconnectHandler reconnectHandler;     // the object needed to reconnect
        private Dictionary<int, KPTag> tagsByCnlNum;          // the device tags accessed by channel number
        private Dictionary<uint, SubscriptionTag> subscrByID; // the subscription tags accessed by IDs
        private Dictionary<int, CommandConfig> cmdByNum;      // the commands accessed by their numbers


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpOpcUaLogic(int number)
            : base(number)
        {
            opcLock = new object();
            deviceConfig = null;
            autoAccept = true;
            connected = false;
            connAttemptDT = DateTime.MinValue;
            opcSession = null;
            reconnectHandler = null;
            tagsByCnlNum = null;
            subscrByID = null;
            cmdByNum = null;

            CanSendCmd = true;
            ConnRequired = false;
        }


        /// <summary>
        /// Connects to the OPC server.
        /// </summary>
        private void ConnectToOpcServer()
        {
            try
            {
                OpcUaHelper helper = new OpcUaHelper(AppDirs, Number, OpcUaHelper.RuntimeKind.Logic)
                {
                    CertificateValidation = CertificateValidator_CertificateValidation,
                    WriteToLog = WriteToLog
                };

                connected = helper.ConnectAsync(deviceConfig.ConnectionOptions, ReqParams.Timeout).Result;
                autoAccept = autoAccept || helper.AutoAccept; // TODO: autoAccept always true?
                opcSession = helper.OpcSession;
                opcSession.KeepAlive += OpcSession_KeepAlive;
                opcSession.Notification += OpcSession_Notification;
            }
            catch (Exception ex)
            {
                connected = false;
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при соединении с OPC-сервером: " :
                    "Error connecting OPC server: ") + ex);
            }
        }

        /// <summary>
        /// Creates subscriptions according to the configuration.
        /// </summary>
        private bool CreateSubscriptions()
        {
            try
            {
                if (opcSession == null)
                    throw new InvalidOperationException("OPC session must not be null.");

                subscrByID = new Dictionary<uint, SubscriptionTag>();

                foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
                {
                    if (!subscriptionConfig.Active)
                        continue;

                    Subscription subscription = new Subscription(opcSession.DefaultSubscription)
                    {
                        DisplayName = subscriptionConfig.DisplayName,
                        PublishingInterval = subscriptionConfig.PublishingInterval
                    };

                    SubscriptionTag subscriptionTag = new SubscriptionTag(subscription);

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        if (!itemConfig.Active)
                            continue;

                        subscription.AddItem(new MonitoredItem(subscription.DefaultItem)
                        {
                            StartNodeId = itemConfig.NodeID,
                            DisplayName = itemConfig.DisplayName
                        });

                        if (itemConfig.Tag is KPTag kpTag)
                        {
                            subscriptionTag.ItemsByNodeID[itemConfig.NodeID] = new ItemTag
                            {
                                ItemConfig = itemConfig,
                                KPTag = kpTag
                            };
                        }
                    }

                    opcSession.AddSubscription(subscription);
                    subscription.Create();
                    subscrByID[subscription.Id] = subscriptionTag;
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при создании подписок: " :
                    "Error creating subscriptions: ") + ex);
                return false;
            }
        }

        /// <summary>
        /// Clears all subscriptions of the OPC session.
        /// </summary>
        private void ClearSubscriptions()
        {
            try
            {
                subscrByID = null;
                opcSession.RemoveSubscriptions(new List<Subscription>(opcSession.Subscriptions));
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при очистке подписок: " :
                    "Error clearing subscriptions: ") + ex);
            }
        }

        /// <summary>
        /// Validates the certificate.
        /// </summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator validator, 
            CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = autoAccept;

                if (autoAccept)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Принятый сертификат: {0}" :
                        "Accepted certificate: {0}", e.Certificate.Subject));
                }
                else
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Отклоненный сертификат: {0}" :
                        "Rejected certificate: {0}", e.Certificate.Subject));
                }
            }
        }

        /// <summary>
        /// Reconnects if needed.
        /// </summary>
        private void OpcSession_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            if (e.Status != null && ServiceResult.IsNotGood(e.Status))
            {
                WriteToLog(string.Format("{0} {1}/{2}", 
                    e.Status, sender.OutstandingRequestCount, sender.DefunctRequestCount));

                if (reconnectHandler == null)
                {
                    InvalidateCurData();
                    WorkState = WorkStates.Error;
                    WriteToLog(Localization.UseRussian ?
                        "Переподключение к OPC-серверу" :
                        "Reconnecting to OPC server");
                    reconnectHandler = new SessionReconnectHandler();
                    reconnectHandler.BeginReconnect(sender, ReconnectPeriod, OpcSession_ReconnectComplete);
                }
            }
        }

        /// <summary>
        /// Processes the reconnect procedure.
        /// </summary>
        private void OpcSession_ReconnectComplete(object sender, EventArgs e)
        {
            // ignore callbacks from discarded objects
            if (!ReferenceEquals(sender, reconnectHandler))
            {
                return;
            }

            opcSession = reconnectHandler.Session;
            reconnectHandler.Dispose();
            reconnectHandler = null;

            // after reconnecting, the subscriptions are automatically recreated, but with the wrong IDs and names,
            // so it's needed to clear them and create again
            ClearSubscriptions();
            WorkState = CreateSubscriptions() ? WorkStates.Normal : WorkStates.Error;
            WriteToLog(Localization.UseRussian ?
                "Переподключено" :
                "Reconnected");
        }

        /// <summary>
        /// Processes new data received from OPC server.
        /// </summary>
        private void OpcSession_Notification(Session session, NotificationEventArgs e)
        {
            try
            {
                Monitor.Enter(opcLock);
                WriteToLog("");
                LastSessDT = DateTime.Now;

                if (subscrByID != null && 
                    subscrByID.TryGetValue(e.Subscription.Id, out SubscriptionTag subscriptionTag))
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0} КП {1}. Обработка новых данных. Подписка: {2}" :
                        "{0} Device {1}. Process new data. Subscription: {2}", 
                        LastSessDT.ToLocalizedString(), Number, e.Subscription.DisplayName));
                    ProcessDataChanges(subscriptionTag, e.NotificationMessage);
                    ProcessEvents(e.NotificationMessage);
                    lastCommSucc = true;
                }
                else
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Ошибка: подписка [{0}] \"{1}\" не найдена" :
                        "Error: subscription [{0}] \"{1}\" not found",
                        e.Subscription.Id, e.Subscription.DisplayName));
                    lastCommSucc = false;
                }
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при обработке новых данных: " :
                    "Error processing new data: ") + ex);
                lastCommSucc = false;
            }
            finally
            {
                CalcSessStats();
                Monitor.Exit(opcLock);
            }
        }

        /// <summary>
        /// Processes new data.
        /// </summary>
        private void ProcessDataChanges(SubscriptionTag subscriptionTag, NotificationMessage notificationMessage)
        {
            foreach (MonitoredItemNotification change in notificationMessage.GetDataChanges(false))
            {
                MonitoredItem monitoredItem = subscriptionTag.Subscription.FindItemByClientHandle(change.ClientHandle);

                if (monitoredItem != null)
                {
                    if (subscriptionTag.ItemsByNodeID.TryGetValue(monitoredItem.StartNodeId.ToString(), 
                        out ItemTag itemTag))
                    {
                        WriteToLog((Localization.UseRussian ? "Приём " : "Receive ") +
                            monitoredItem.DisplayName + " = " + change.Value + " (" + change.Value.StatusCode + ")");

                        int tagIndex = itemTag.KPTag.Index;
                        int tagStatus = StatusCode.IsGood(change.Value.StatusCode) ?
                            BaseValues.CnlStatuses.Defined :
                            BaseValues.CnlStatuses.Undefined;

                        if (itemTag.ItemConfig.IsArray)
                        {
                            int arrayLen = itemTag.ItemConfig.ArrayLen;
                            double[] vals = DecodeArray(change.Value.Value, arrayLen, out TagType tagType);

                            for (int i = 0; i < arrayLen; i++)
                            {
                                SetCurData(tagIndex, vals[i], tagStatus);
                                KPTags[tagIndex].Aux = tagType;
                                tagIndex++;
                            }
                        }
                        else
                        {
                            SetCurData(tagIndex, DecodeItemVal(change.Value.Value, out TagType tagType), tagStatus);
                            itemTag.KPTag.Aux = tagType;
                        }
                    }
                    else
                    {
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "Ошибка: тег \"{0}\" не найден" :
                            "Error: tag \"{0}\" not found", monitoredItem.StartNodeId));
                    }
                }
            }
        }

        /// <summary>
        /// Processes new events.
        /// </summary>
        private void ProcessEvents(NotificationMessage notificationMessage)
        {
            foreach (EventFieldList eventFields in notificationMessage.GetEvents(true))
            {
                // events are not really implemented
                WriteToLog((Localization.UseRussian ?
                    "Новое событие " :
                    "New event ") + eventFields);
            }
        }
        
        /// <summary>
        /// Decodes the received tag value and returns the tag type.
        /// </summary>
        private double DecodeItemVal(object val, out TagType tagType)
        {
            try
            {
                if (val is string)
                {
                    tagType = TagType.String;
                    return ScadaUtils.EncodeAscii((string)val);
                }
                else if (val is DateTime)
                {
                    tagType = TagType.DateTime;
                    return ScadaUtils.EncodeDateTime((DateTime)val);
                }
                else
                {
                    tagType = TagType.Number;
                    return Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при декодировании элемента: " :
                    "Error decoding item: ") + ex);

                tagType = TagType.Number;
                return 0.0;
            }
        }

        /// <summary>
        /// Decodes the received array and returns the tag type.
        /// </summary>
        private double[] DecodeArray(object val, int len, out TagType tagType)
        {
            double[] outArr = new double[len];
            tagType = TagType.Number;

            try
            {
                // get the type of the 1st element
                Array inArr = (Array)val;

                if (inArr.Length > 0)
                {
                    object firstItem = inArr.GetValue(0);
                    if (firstItem is string)
                        tagType = TagType.String;
                    else if (firstItem is DateTime)
                        tagType = TagType.DateTime;
                }

                // decode array elements
                for (int i = 0, n = Math.Min(inArr.Length, len); i < n; i++)
                {
                    object inVal = inArr.GetValue(i);
                    double outVal;

                    switch (tagType)
                    {
                        case TagType.String:
                            outVal = ScadaUtils.EncodeAscii((string)inVal);
                            break;
                        case TagType.DateTime:
                            outVal = ScadaUtils.EncodeDateTime((DateTime)inVal);
                            break;
                        default:
                            outVal = Convert.ToDouble(inVal);
                            break;
                    }

                    outArr[i] = outVal;
                }
            }
            catch (Exception ex)
            {
                WriteToLog((Localization.UseRussian ?
                    "Ошибка при декодировании массива: " :
                    "Error decoding array: ") + ex);
            }

            return outArr;
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags()
        {
            tagsByCnlNum = new Dictionary<int, KPTag>();
            List<TagGroup> tagGroups = new List<TagGroup>(deviceConfig.Subscriptions.Count);
            int signal = 1;

            foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
            {
                TagGroup tagGroup = new TagGroup(subscriptionConfig.DisplayName);
                tagGroups.Add(tagGroup);

                foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                {
                    bool cnlNumSpecified = itemConfig.CnlNum > 0;

                    if (itemConfig.IsArray)
                    {
                        for (int i = 0, n = itemConfig.ArrayLen; i < n; i++)
                        {
                            KPTag kpTag = tagGroup.AddNewTag(signal++, itemConfig.DisplayName + "[" + i + "]");
                            itemConfig.Tag = itemConfig.Tag ?? kpTag; // store a reference to the 1st tag

                            if (cnlNumSpecified)
                                tagsByCnlNum[itemConfig.CnlNum + i] = kpTag;
                        }
                    }
                    else
                    {
                        KPTag kpTag = tagGroup.AddNewTag(signal++, itemConfig.DisplayName);
                        itemConfig.Tag = kpTag;

                        if (cnlNumSpecified)
                            tagsByCnlNum[itemConfig.CnlNum] = kpTag;
                    }
                }
            }

            InitKPTags(tagGroups);
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected override string ConvertTagDataToStr(KPTag kpTag, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0 && kpTag.Aux is TagType tagType)
            {
                switch (tagType)
                {
                    case TagType.String:
                        return ScadaUtils.DecodeAscii(tagData.Val);
                    case TagType.DateTime:
                        return ScadaUtils.DecodeDateTime(tagData.Val).ToLocalizedString();
                }
            }

            return base.ConvertTagDataToStr(kpTag, tagData);
        }



        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            if (deviceConfig == null)
            {
                lastCommSucc = false;
                WorkState = WorkStates.Error;
            }
            else if (!connected)
            {
                base.Session();

                // delay before connection
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan connectionDelay = ReconnectDelay - (utcNow - connAttemptDT);

                if (connectionDelay > TimeSpan.Zero)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "Задержка перед соединением {0} с" :
                        "Delay before connection {0} sec", connectionDelay.TotalSeconds.ToString("N1")));
                    Thread.Sleep(connectionDelay);
                }

                // connect to OPC server and create subscriptions
                connAttemptDT = DateTime.UtcNow;
                ConnectToOpcServer();
                WorkState = connected && CreateSubscriptions() ?
                    WorkStates.Normal : WorkStates.Error;
            }

            Thread.Sleep(ReqParams.Delay);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            try
            {
                Monitor.Enter(opcLock);
                base.SendCmd(cmd);
                lastCommSucc = false;

                if (connected)
                {
                    if (cmdByNum.TryGetValue(cmd.CmdNum, out CommandConfig commandConfig) &&
                        cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
                    {
                        // prepare value to write
                        string dataTypeName = commandConfig.DataTypeName;
                        Type itemType = Type.GetType(dataTypeName, false, true);
                        object itemVal;

                        if (itemType == null)
                        {
                            throw new ScadaException(string.Format(Localization.UseRussian ?
                                "Не удалось получить тип данных {0}" :
                                "Unable to get data type {0}", dataTypeName));
                        }

                        if (itemType.IsArray)
                        {
                            throw new ScadaException(string.Format(Localization.UseRussian ?
                                "Тип данных {0} не поддерживается" :
                                "Data type {0} not supported", dataTypeName));
                        }

                        try
                        {
                            itemVal = Convert.ChangeType(cmd.CmdVal, itemType);
                        }
                        catch
                        {
                            throw new ScadaException(string.Format(Localization.UseRussian ?
                                "Не удалось привести значение команды к типу {0}" :
                                "Unable to convert command value to the type {0}", itemType.FullName));
                        }

                        // write value
                        WriteToLog(string.Format(Localization.UseRussian ?
                            "Отправка значения OPC-серверу: {0} = {1}" :
                            "Send value to the OPC server: {0} = {1}", commandConfig.DisplayName, itemVal));

                        WriteValue valueToWrite = new WriteValue
                        {
                            NodeId = commandConfig.NodeID,
                            AttributeId = Attributes.Value,
                            Value = new DataValue(new Variant(itemVal))
                        };

                        opcSession.Write(null, new WriteValueCollection { valueToWrite }, 
                            out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);

                        if (StatusCode.IsGood(results[0]))
                        {
                            WriteToLog(CommPhrases.ResponseOK);
                            lastCommSucc = true;
                        }
                        else
                        {
                            WriteToLog(CommPhrases.ResponseError);
                        }
                    }
                    else
                    {
                        WriteToLog(CommPhrases.IllegalCommand);
                    }
                }
                else
                {
                    WriteToLog(Localization.UseRussian ?
                        "Невозможно отправить команду ТУ, т.к. соединение с OPC-сервером не установлено" :
                        "Unable to send command because connection with the OPC server is not established");
                }
            }
            finally
            {
                CalcCmdStats();
                Monitor.Exit(opcLock);
            }
        }

        /// <summary>
        /// Performs actions after adding the device to a communication line.
        /// </summary>
        public override void OnAddedToCommLine()
        {
            deviceConfig = new DeviceConfig();

            if (deviceConfig.Load(DeviceConfig.GetFileName(AppDirs.ConfigDir, Number), out string errMsg))
            {
                InitDeviceTags();

                // fill the command dictionary
                cmdByNum = new Dictionary<int, CommandConfig>();
                deviceConfig.Commands.ForEach((CommandConfig c) => cmdByNum[c.CmdNum] = c);
            }
            else
            {
                deviceConfig = null;
                WriteToLog(errMsg);
                WriteToLog(Localization.UseRussian ?
                    "Взаимодействие с OPC-сервером невозможно, т.к. конфигурация КП не загружена" :
                    "Interaction with OPC server is impossible because device configuration is not loaded");
            }
        }

        /// <summary>
        /// Performs actions when terminating a communication line.
        /// </summary>
        public override void OnCommLineTerminate()
        {
            if (opcSession != null)
                opcSession.Close();
        }

        /// <summary>
        /// Binds the device tag to the input channel.
        /// </summary>
        public override void BindTag(int signal, int cnlNum, int objNum, int paramID)
        {
            // the signal here is the signal specified for a channel in the configuration database
            if (signal > 0)
            {
                base.BindTag(signal, cnlNum, objNum, paramID);
            }
            else if (tagsByCnlNum.TryGetValue(cnlNum, out KPTag kpTag))
            {
                kpTag.CnlNum = cnlNum;
                kpTag.ObjNum = objNum;
                kpTag.ParamID = paramID;
            }
        }
    }
}
