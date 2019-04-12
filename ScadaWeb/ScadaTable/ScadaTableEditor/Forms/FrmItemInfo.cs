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
 * Module   : Table Editor
 * Summary  : Form for displaying the table view item properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2019
 */

using Scada.Data.Entities;
using Scada.Table.Editor.Code;
using Scada.UI;
using System;
using System.Windows.Forms;

namespace Scada.Table.Editor.Forms
{
    /// <summary>
    /// Form for displaying the table view item properties.
    /// <para>Форма для отображения свойств элемента табличного представления.</para>
    /// </summary>
    public partial class FrmItemInfo : Form
    {
        private readonly TableView.Item item;   // the table view item
        private readonly BaseTables baseTables; // the configuration database tables


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmItemInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmItemInfo(TableView.Item item, BaseTables baseTables)
            : this()
        {
            this.item = item ?? throw new ArgumentNullException("item");
            this.baseTables = baseTables ?? throw new ArgumentNullException("baseTables");
        }


        /// <summary>
        /// Shows the information about the item.
        /// </summary>
        private void ShowItemInfo()
        {
            // input channel
            if (item.CnlNum > 0 && baseTables.InCnlTable.Items.TryGetValue(item.CnlNum, out InCnl inCnl))
            {
                txtInCnlNum.Text = inCnl.CnlNum.ToString();
                txtInCnlName.Text = inCnl.Name;

                if (inCnl.ObjNum != null && baseTables.ObjTable.Items.TryGetValue(inCnl.ObjNum.Value, out Obj obj))
                {
                    txtInCnlObjNum.Text = obj.ObjNum.ToString();
                    txtInCnlObjName.Text = obj.Name;
                }

                if (inCnl.KPNum != null && baseTables.KPTable.Items.TryGetValue(inCnl.KPNum.Value, out KP kp))
                {
                    txtInCnlKPNum.Text = kp.KPNum.ToString();
                    txtInCnlKPName.Text = kp.Name;
                }
            }

            // output channel
            if (item.CtrlCnlNum > 0 && baseTables.CtrlCnlTable.Items.TryGetValue(item.CtrlCnlNum, out CtrlCnl ctrlCnl))
            {
                txtCtrlCnlNum.Text = ctrlCnl.CtrlCnlNum.ToString();
                txtCtrlCnlName.Text = ctrlCnl.Name;

                if (ctrlCnl.ObjNum != null && baseTables.ObjTable.Items.TryGetValue(ctrlCnl.ObjNum.Value, out Obj obj))
                {
                    txtCtrlCnlObjNum.Text = obj.ObjNum.ToString();
                    txtCtrlCnlObjName.Text = obj.Name;
                }

                if (ctrlCnl.KPNum != null && baseTables.KPTable.Items.TryGetValue(ctrlCnl.KPNum.Value, out KP kp))
                {
                    txtCtrlCnlKPNum.Text = kp.KPNum.ToString();
                    txtCtrlCnlKPName.Text = kp.Name;
                }
            }
        }


        private void FrmItemInfo_Load(object sender, EventArgs e)
        {
            Translator.TranslateForm(this, GetType().FullName);
            ShowItemInfo();
        }
    }
}
