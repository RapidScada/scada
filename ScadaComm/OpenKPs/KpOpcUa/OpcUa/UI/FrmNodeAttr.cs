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
 * Module   : KpOpcUa
 * Summary  : Form for viewing OPC node attributes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Opc.Ua;
using Opc.Ua.Client;
using Scada.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Devices.OpcUa.UI
{
    /// <summary>
    /// Form for viewing OPC node attributes.
    /// <para>Форма просмотра атрибутов OPC-узла.</para>
    /// </summary>
    public partial class FrmNodeAttr : Form
    {
        private Session opcSession; // the OPC session
        private NodeId nodeId;      // the node whose attributes are shown


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmNodeAttr()
        {
            InitializeComponent();
            colName.Name = "colName";
            colValue.Name = "colValue";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmNodeAttr(Session opcSession, NodeId nodeId)
            : this()
        {
            this.opcSession = opcSession ?? throw new ArgumentNullException("opcSession");
            this.nodeId = nodeId ?? throw new ArgumentNullException("nodeId");
        }


        /// <summary>
        /// Reads available attributes from OPC server.
        /// </summary>
        private void ReadAttributes()
        {
            try
            {
                // request attributes
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

                foreach (uint attributeId in Attributes.GetIdentifiers())
                {
                    nodesToRead.Add(new ReadValueId
                    {
                        NodeId = nodeId,
                        AttributeId = attributeId
                    });
                }

                opcSession.Read(null, 0, TimestampsToReturn.Neither, nodesToRead,
                    out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                // display attributes
                for (int i = 0; i < nodesToRead.Count; i++)
                {
                    ReadValueId readValueId = nodesToRead[i];
                    DataValue dataValue = results[i];

                    if (dataValue.StatusCode != StatusCodes.BadAttributeIdInvalid)
                    {
                        listView.Items.Add(new ListViewItem(new string[] {
                        Attributes.GetBrowseName(readValueId.AttributeId),
                        FormatAttribute(readValueId.AttributeId, dataValue.Value)
                    }));
                    }
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(KpPhrases.ReadAttrError + ":" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Formats the attribute value.
        /// </summary>
        private string FormatAttribute(uint attributeId, object value)
        {
            switch (attributeId)
            {
                case Attributes.NodeClass:
                    return value == null ?
                        "(null)" :
                        Enum.ToObject(typeof(NodeClass), value).ToString();

                case Attributes.DataType:
                    if (value is NodeId dataTypeId)
                    {
                        INode dataType = opcSession.NodeCache.Find(dataTypeId);
                        if (dataType != null)
                            return dataType.DisplayName.Text;
                    }
                    return string.Format("{0}", value);

                case Attributes.ValueRank:
                    if (value is int valueRank)
                    {
                        switch (valueRank)
                        {
                            case ValueRanks.Scalar: return "Scalar";
                            case ValueRanks.OneDimension: return "OneDimension";
                            case ValueRanks.OneOrMoreDimensions: return "OneOrMoreDimensions";
                            case ValueRanks.Any: return "Any";
                            default: return string.Format("{0}", valueRank);
                        }
                    }
                    return string.Format("{0}", value);

                case Attributes.MinimumSamplingInterval:
                    if (value is double minimumSamplingInterval)
                    {
                        if (minimumSamplingInterval == MinimumSamplingIntervals.Indeterminate)
                            return "Indeterminate";
                        else if (minimumSamplingInterval == MinimumSamplingIntervals.Continuous)
                            return "Continuous";
                        else
                            return string.Format("{0}", minimumSamplingInterval);
                    }
                    return string.Format("{0}", value);

                case Attributes.AccessLevel:
                case Attributes.UserAccessLevel:
                    byte accessLevel = Convert.ToByte(value);
                    List<string> accessList = new List<string>();

                    if ((accessLevel & AccessLevels.CurrentRead) != 0)
                        accessList.Add("Readable");

                    if ((accessLevel & AccessLevels.CurrentWrite) != 0)
                        accessList.Add("Writeable");

                    if ((accessLevel & AccessLevels.HistoryRead) != 0)
                        accessList.Add("History");

                    if ((accessLevel & AccessLevels.HistoryWrite) != 0)
                        accessList.Add("History Update");

                    if (accessList.Count == 0)
                        accessList.Add("No Access");

                    return string.Join(" | ", accessList);

                case Attributes.EventNotifier:
                    byte notifier = Convert.ToByte(value);
                    List<string> bits = new List<string>();

                    if ((notifier & EventNotifiers.SubscribeToEvents) != 0)
                        bits.Add("Subscribe");

                    if ((notifier & EventNotifiers.HistoryRead) != 0)
                        bits.Add("History");

                    if ((notifier & EventNotifiers.HistoryWrite) != 0)
                        bits.Add("History Update");

                    if (bits.Count == 0)
                        bits.Add("No Access");

                    return string.Join(" | ", notifier);

                default:
                    return string.Format("{0}", value);
            }
        }


        private void FrmNodeAttr_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
        }

        private async void FrmNodeAttr_Shown(object sender, EventArgs e)
        {
            await Task.Run(() => ReadAttributes());
        }
    }
}
