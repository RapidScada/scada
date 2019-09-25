/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Represents a driver configuration for a device
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Devices.OpcUa.Config
{
    /// <summary>
    /// Represents a driver configuration for a device.
    /// <para>Представляет конфигурацию драйвера для устройства.</para>
    /// </summary>
    public class DeviceConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        public List<SubscriptionConfig> Subscriptions { get; private set; }

        /// <summary>
        /// Gets the commands.
        /// </summary>
        public List<CommandConfig> Commands { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            ConnectionOptions = new ConnectionOptions();
            Subscriptions = new List<SubscriptionConfig>();
            Commands = new List<CommandConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                    ConnectionOptions.LoadFromXml(connectionOptionsNode);

                if (rootElem.SelectSingleNode("Subscriptions") is XmlNode subscriptionsNode)
                {
                    foreach (XmlElement subscriptionElem in subscriptionsNode.SelectNodes("Subscription"))
                    {
                        SubscriptionConfig subscriptionConfig = new SubscriptionConfig();
                        subscriptionConfig.LoadFromXml(subscriptionElem);
                        Subscriptions.Add(subscriptionConfig);
                    }
                }

                if (rootElem.SelectSingleNode("Commands") is XmlNode commandsNode)
                {
                    foreach (XmlElement commandElem in commandsNode.SelectNodes("Command"))
                    {
                        CommandConfig commandConfig = new CommandConfig();
                        commandConfig.LoadFromXml(commandElem);
                        Commands.Add(commandConfig);
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the configuration to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("KpOpcUa");
                xmlDoc.AppendChild(rootElem);

                ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));
                XmlElement subscriptionsElem = rootElem.AppendElem("Subscriptions");
                XmlElement commandsElem = rootElem.AppendElem("Commands");

                foreach (SubscriptionConfig subscriptionConfig in Subscriptions)
                {
                    subscriptionConfig.SaveToXml(subscriptionsElem.AppendElem("Subscription"));
                }

                foreach (CommandConfig commandConfig in Commands)
                {
                    commandConfig.SaveToXml(commandsElem.AppendElem("Command"));
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Gets the configuration file name.
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return Path.Combine(configDir, "KpOpcUa_" + CommUtils.AddZeros(kpNum, 3) + ".xml");
        }
    }
}
