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
 * Module   : SCADA-Table Editor
 * Summary  : Item information form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2016
 */

using Scada;
using Scada.UI;
using Scada.Web.Plugins.Table;
using System;
using System.Data;
using System.Windows.Forms;

namespace ScadaTableEditor
{
    /// <summary>
    /// Item information form
    /// <para>Форма информации об элементе</para>
    /// </summary>
    public partial class FrmItemInfo : Form
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        private FrmItemInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Получить имя объекта по номеру
        /// </summary>
        private static string GetObjName(int objNum, DataTable tblObj)
        {
            DataView view = new DataView(tblObj);
            view.RowFilter = "ObjNum = " + objNum;
            return view.Count > 0 ? (string)view[0]["Name"] : "";
        }

        /// <summary>
        /// Получить имя КП по номеру
        /// </summary>
        private static string GetKPName(int kpNum, DataTable tblKP)
        {
            DataView view = new DataView(tblKP);
            view.RowFilter = "KPNum = " + kpNum;
            return view.Count > 0 ? (string)view[0]["Name"] : "";
        }

        /// <summary>
        /// Отобразить форму настроек приложения
        /// </summary>
        public static void Show(TableView.Item item, DataTable tblInCnl, DataTable tblCtrlCnl, 
            DataTable tblObj, DataTable tblKP)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (tblInCnl == null)
                throw new ArgumentNullException("tblInCnl");
            if (tblCtrlCnl == null)
                throw new ArgumentNullException("tblCtrlCnl");
            if (tblObj == null)
                throw new ArgumentNullException("tblObj");
            if (tblKP == null)
                throw new ArgumentNullException("tblKP");

            // создание и перевод формы
            FrmItemInfo frmItemInfo = new FrmItemInfo();
            Translator.TranslateForm(frmItemInfo, "ScadaTableEditor.FrmItemInfo");

            // получение информации о входном канале
            if (item.CnlNum > 0)
            {
                string cnlNumStr = item.CnlNum.ToString();
                frmItemInfo.txtInCnlNum.Text = cnlNumStr;

                try
                {
                    DataView view = new DataView(tblInCnl);
                    view.RowFilter = "CnlNum = " + cnlNumStr;

                    if (view.Count > 0)
                    {
                        DataRowView rowView = view[0];
                        frmItemInfo.txtInCnlName.Text = (string)rowView["Name"];

                        int objNum = (int)rowView["ObjNum"];
                        if (objNum > 0)
                        {
                            frmItemInfo.txtInCnlObjNum.Text = objNum.ToString();
                            frmItemInfo.txtInCnlObjName.Text = GetObjName(objNum, tblObj);
                        }

                        int kpNum = (int)rowView["KPNum"];
                        if (kpNum > 0)
                        {
                            frmItemInfo.txtInCnlKPNum.Text = kpNum.ToString();
                            frmItemInfo.txtInCnlKPName.Text = GetKPName(kpNum, tblKP);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScadaUiUtils.ShowError(AppPhrases.GetInCnlError + ":\n" + ex.Message);
                }
            }

            // получение информации о канале управления
            if (item.CtrlCnlNum > 0)
            {
                string ctrlCnlNumStr = item.CtrlCnlNum.ToString();
                frmItemInfo.txtCtrlCnlNum.Text = ctrlCnlNumStr;

                try
                {
                    DataView view = new DataView(tblCtrlCnl);
                    view.RowFilter = "CtrlCnlNum = " + ctrlCnlNumStr;

                    if (view.Count > 0)
                    {
                        DataRowView rowView = view[0];
                        frmItemInfo.txtCtrlCnlName.Text = (string)rowView["Name"];

                        int objNum = (int)rowView["ObjNum"];
                        if (objNum > 0)
                        {
                            frmItemInfo.txtCtrlCnlObjNum.Text = objNum.ToString();
                            frmItemInfo.txtCtrlCnlObjName.Text = GetObjName(objNum, tblObj);
                        }

                        int kpNum = (int)rowView["KPNum"];
                        if (kpNum > 0)
                        {
                            frmItemInfo.txtCtrlCnlKPNum.Text = kpNum.ToString();
                            frmItemInfo.txtCtrlCnlKPName.Text = GetKPName(kpNum, tblKP);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScadaUiUtils.ShowError(AppPhrases.GetCtrlCnlError + ":\n" + ex.Message);
                }
            }

            // отображение формы
            frmItemInfo.ShowDialog();
        }
    }
}
