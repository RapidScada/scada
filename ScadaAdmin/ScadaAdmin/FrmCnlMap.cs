/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : SCADA-Administrator
 * Summary  : Channel map form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2018
 */

using Scada;
using Scada.UI;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScadaAdmin
{
    /// <summary>
    /// Channel map form
    /// <para>Форма карты каналов</para>
    /// </summary>
    public partial class FrmCnlMap : Form
    {
        /// <summary>
        /// Делегат получения каналов по номеру элемента
        /// </summary>
        private delegate DataTable GetCnlTableByItemNumDelegate(int itemNum);


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmCnlMap()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Сформировать строку номеров каналов
        /// </summary>
        private string MakeCnlsLine(DataTable tblCnls)
        {
            StringBuilder sbLine = new StringBuilder();
            int lineLen = 0;
            int prevCnlNum = 0;
            int curCnlNum = 0;
            int startCnlNum = 0;

            foreach (DataRowView cnlRowView in tblCnls.DefaultView)
            {
                curCnlNum = (int)cnlRowView[0];

                if (prevCnlNum > 0)
                {
                    if (prevCnlNum == curCnlNum - 1)
                    {
                        if (startCnlNum <= 0)
                            startCnlNum = prevCnlNum;
                    }
                    else
                    {
                        AppendCnlNum(sbLine, startCnlNum, prevCnlNum, ref lineLen);
                        startCnlNum = curCnlNum;
                    }
                }
                else
                {
                    startCnlNum = curCnlNum;
                }

                prevCnlNum = curCnlNum;
            }

            if (prevCnlNum > 0)
                AppendCnlNum(sbLine, startCnlNum, prevCnlNum, ref lineLen);

            if (sbLine.Length == 0)
                sbLine.Append(AppPhrases.NoChannels);

            return "    " + sbLine;
        }

        /// <summary>
        /// Добавить диапазон каналов в строку
        /// </summary>
        private void AppendCnlNum(StringBuilder sbLine, int cnlNum1, int cnlNum2, ref int lineLen)
        {
            if (sbLine.Length > 0)
            {
                sbLine.Append(", ");
                lineLen += 2;
            }

            if (lineLen >= 50 /*макс. длина строки*/)
            {
                sbLine.AppendLine().Append("    ");
                lineLen = 4;
            }

            string s = 0 < cnlNum1 && cnlNum1 < cnlNum2 ? cnlNum1 + "-" + cnlNum2 : cnlNum2.ToString();
            sbLine.Append(s);
            lineLen += s.Length;
        }


        private void FrmCnlMap_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "ScadaAdmin.FrmCnlMap");
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // создание карты каналов
            bool useInCnls = rbInCnls.Checked;
            bool groupByObj = rbGroupByObj.Checked;

            StreamWriter writer = null;
            bool mapCreated = false;
            string mapFileName = AppData.AppDirs.LogDir + "ScadaAdminCnlMap.txt";

            try
            {
                writer = new StreamWriter(mapFileName, false, Encoding.UTF8);
                mapCreated = true;

                string title = DateTime.Now.ToString("G", Localization.Culture) + " " + (useInCnls ? 
                    (groupByObj ? AppPhrases.InCnlsByObjTitle : AppPhrases.InCnlsByKPTitle) : 
                    (groupByObj ? AppPhrases.CtrlCnlsByObjTitle : AppPhrases.CtrlCnlsByKPTitle));
                writer.WriteLine(title);
                writer.WriteLine(new string('-', title.Length));

                // получение таблицы элементов, по которым выполняется группировка
                DataTable tblItems = groupByObj ? Tables.GetObjTable() : Tables.GetKPTable();
                string itemNameFormat = groupByObj ? AppPhrases.ObjectCaptionFormat : AppPhrases.KPCaptionFormat;

                // определение метода получения каналов
                GetCnlTableByItemNumDelegate getCnlTable = useInCnls ?
                    (groupByObj ? new GetCnlTableByItemNumDelegate(Tables.GetInCnlTableByObjNum) :
                        new GetCnlTableByItemNumDelegate(Tables.GetInCnlTableByKPNum)) :
                    (groupByObj ? new GetCnlTableByItemNumDelegate(Tables.GetCtrlCnlTableByObjNum) :
                        new GetCnlTableByItemNumDelegate(Tables.GetCtrlCnlTableByKPNum));

                // формирование карты каналов
                foreach (DataRowView itemRowView in tblItems.DefaultView)
                {
                    int itemNum = (int)itemRowView[0];
                    writer.WriteLine(string.Format(itemNameFormat, itemNum, itemRowView[1]));

                    writer.WriteLine(MakeCnlsLine(getCnlTable(itemNum)));
                    writer.WriteLine();
                }

                if (groupByObj)
                    writer.WriteLine(AppPhrases.UndefinedObject);
                else
                    writer.WriteLine(AppPhrases.UndefinedKP);

                DataTable tblCnls = getCnlTable(0);

                if (tblItems.DefaultView.Count > 0 || tblCnls.DefaultView.Count > 0)
                    writer.WriteLine(MakeCnlsLine(tblCnls));
                else
                    writer.WriteLine(AppPhrases.NoChannels);
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.CreateCnlMapError + ":\r\n" + ex.Message;
                try { writer.WriteLine(errMsg); }
                catch { }
                AppUtils.ProcError(errMsg);
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }

            if (mapCreated)
                Process.Start(mapFileName);
        }
    }
}
