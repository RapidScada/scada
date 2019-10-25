/*
 * Copyright 2019 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : KpOpcUa
 * Summary  : Represents a monitored item configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Xml;

namespace Scada.Comm.Devices.OpcUa.Config
{
    /// <summary>
    /// Represents a monitored item configuration.
    /// <para>Представляет конфигурацию отслеживаемого элемента.</para>
    /// </summary>
    public class ItemConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ItemConfig()
        {
            Active = true;
            NodeID = "";
            DisplayName = "";
            IsArray = false;
            ArrayLen = 1;
            CnlNum = 0;
            Tag = null;
        }

        
        /// <summary>
        /// Gets or sets a value indicating that the item is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the OPC UA node ID.
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the item data type is an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Get or sets the array length if the item represents an array.
        /// </summary>
        public int ArrayLen { get; set; }

        /// <summary>
        /// Gets or sets the input channel number to which the item is bound.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the object that contains data related to the item.
        /// </summary>
        public object Tag { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            Active = xmlElem.GetAttrAsBool("active");
            NodeID = xmlElem.GetAttrAsString("nodeID");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            IsArray = xmlElem.GetAttrAsBool("isArray");
            ArrayLen = xmlElem.GetAttrAsInt("arrayLen");
            CnlNum = xmlElem.GetAttrAsInt("cnlNum");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("isArray", IsArray);
            xmlElem.SetAttribute("arrayLen", ArrayLen);
            xmlElem.SetAttribute("cnlNum", CnlNum);
        }
    }
}
