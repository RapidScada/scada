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
 * Module   : SCADA-Administrator
 * Summary  : Creating channels form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Scada;
using Scada.Comm.KP;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// Creating channels form
    /// <para>Форма создания каналов</para>
    /// </summary>
    public partial class FrmCreateCnls : Form
    {
        /// <summary>
        /// Параметры выбираемого КП, для которого создаются каналы
        /// </summary>
        private class KPParams
        {
            /// <summary>
            /// Получить или установить признак, что выбор разрешён
            /// </summary>
            public bool Enabled { get; set; }
            /// <summary>
            /// Получить или установить цвет соответствующей строки таблицы
            /// </summary>
            public Color Color { get; set; }
            /// <summary>
            /// Получить или установить пользовательский интерфейс КП
            /// </summary>
            public KPView KPView { get; set; }

            /// <summary>
            /// Получить или установить признак, что КП выбран
            /// </summary>
            public bool Selected { get; set; }
            /// <summary>
            /// Получить или установить номер КП
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить наименование КП
            /// </summary>
            public string KPName { get; set; }
            /// <summary>
            /// Получить или установить номер объекта
            /// </summary>
            public object ObjNum { get; set; }
            /// <summary>
            /// Получить или установить имя файла DLL
            /// </summary>
            public string DllFileName { get; set; }
            /// <summary>
            /// Получить или установить состояние DLL
            /// </summary>
            public string DllState { get; set; }

            /// <summary>
            /// Получить или установить признак ошибки при расчёте номеров входных каналов
            /// </summary>
            public bool InCnlsError { get; set; }
            /// <summary>
            /// Получить или установить номера входных каналов
            /// </summary>
            public string InCnls { get; set; }
            /// <summary>
            /// Получить или установить номер первого входного канала
            /// </summary>
            public int FirstInCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер последнего входного канала
            /// </summary>
            public int LastInCnlNum { get; set; }

            /// <summary>
            /// Получить или установить признак ошибки при расчёте номеров каналов управления
            /// </summary>
            public bool CtrlCnlsError { get; set; }
            /// <summary>
            /// Получить или установить номера каналов управления
            /// </summary>
            public string CtrlCnls { get; set; }
            /// <summary>
            /// Получить или установить номер первого канала управления
            /// </summary>
            public int FirstCtrlCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер последнего канала управления
            /// </summary>
            public int LastCtrlCnlNum { get; set; }
        }


        private static string lastKPDir = "";                        // последняя использованная директория библиотек КП
        private static SortedList<string, KPView> kpViewList = null; // список объектов пользовательского интерфейса КП, 
                                                                     // упорядоченный по имении DLL
        private List<KPParams> kpParamsList; // список параметров выбираемых КП
        private List<int> inCnlNums;         // список номеров входных каналов
        private List<int> ctrlCnlNums;       // список номеров каналов управления
        StreamWriter writer;                 // объект для записи в журнал создания каналов


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmCreateCnls()
        {
            InitializeComponent();

            kpParamsList = new List<KPParams>();
            inCnlNums = null;
            ctrlCnlNums = null;
            writer = null;
            KPDir = "";
            gvKPSel.AutoGenerateColumns = false;
        }


        /// <summary>
        /// Получить или установить директорию библиотек КП
        /// </summary>
        public string KPDir { get; set; }


        /// <summary>
        /// Загрузить библиотеки КП
        /// </summary>
        private void LoadKPDlls()
        {
            if (kpViewList == null || lastKPDir != KPDir)
            {
                lastKPDir = KPDir;
                kpViewList = new SortedList<string, KPView>();

                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(KPDir);
                    FileInfo[] fileInfoAr = dirInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

                    foreach (FileInfo fileInfo in fileInfoAr)
                    {
                        if (fileInfo.Name.ToLower() != "kp.dll")
                        {
                            KPView kpView;

                            try
                            {
                                Assembly asm = Assembly.LoadFile(fileInfo.FullName);
                                string shtName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                                Type kpType = asm.GetType("Scada.Comm.KP." + shtName + "View");
                                kpView = Activator.CreateInstance(kpType) as KPView;
                            }
                            catch
                            {
                                kpView = null;
                            }

                            kpViewList.Add(fileInfo.Name, kpView);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppUtils.ProcError(AppPhrases.LoadKPDllError + ":\r\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Заполнить таблицу КП
        /// </summary>
        private void FillKPGrid()
        {
            try
            {
                DataTable tblObj = Tables.GetObjTable();
                tblObj.Rows.Add(DBNull.Value, AppPhrases.UndefinedItem);
                DataGridViewComboBoxColumn colObjNum = (DataGridViewComboBoxColumn)gvKPSel.Columns["colObjNum"];
                colObjNum.DataSource = tblObj;
                colObjNum.DisplayMember = "Name";
                colObjNum.ValueMember = "ObjNum";

                DataTable tblKP = Tables.GetKPTable();
                DataTable tblKPType = Tables.GetKPTypeTable();
                foreach (DataRow rowKP in tblKP.Rows)
                {
                    KPParams kpParams = new KPParams();
                    kpParams.KPNum = (int)rowKP["KPNum"];
                    kpParams.KPName = (string)rowKP["Name"];
                    kpParams.ObjNum = DBNull.Value;

                    tblKPType.DefaultView.RowFilter = "KPTypeID = " + rowKP["KPTypeID"];
                    object dllFileName = tblKPType.DefaultView[0]["DllFileName"];
                    kpParams.DllFileName = dllFileName == null || dllFileName == DBNull.Value ? "" : (string)dllFileName;

                    if (kpParams.DllFileName == "")
                    {
                        kpParams.Enabled = false;
                        kpParams.Color = Color.Gray;
                        kpParams.KPView = null;
                        kpParams.Selected = false;
                        kpParams.DllState = "";
                    }
                    else
                    {
                        if (kpViewList.ContainsKey(kpParams.DllFileName))
                        {
                            kpParams.KPView = kpViewList[kpParams.DllFileName];
                            if (kpParams.KPView == null)
                            {
                                kpParams.Enabled = false;
                                kpParams.Color = Color.Red;
                                kpParams.Selected = false;
                                kpParams.DllState = AppPhrases.DllError;
                            }
                            else
                            {
                                kpParams.Enabled = true;
                                kpParams.Color = Color.Black;
                                kpParams.Selected = true;
                                kpParams.DllState = AppPhrases.DllLoaded;
                            }
                        }
                        else
                        {
                            kpParams.Enabled = false;
                            kpParams.Color = Color.Gray;
                            kpParams.KPView = null;
                            kpParams.Selected = false;
                            kpParams.DllState = AppPhrases.DllNotFound;
                        }
                    }

                    kpParams.InCnlsError = false;
                    kpParams.InCnls = "";
                    kpParams.CtrlCnlsError = false;
                    kpParams.CtrlCnls = "";
                    kpParamsList.Add(kpParams);
                }
                gvKPSel.DataSource = kpParamsList;
            }
            catch (Exception ex)
            {
                AppUtils.ProcError(AppPhrases.FillKPGridError + ":\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// Расчитать номера первого и последнего каналов КП
        /// </summary>
        private void CalcFirstAndLastNums(int curCnlNum, int curCnlInd, List<int> cnlNums, int cnlNumsCnt,
            int cnlCnt, int cnlsSpace, int cnlsMultiple, out int firstCnlNum, out int lastCnlNum, out int newInCnlInd)
        {
            firstCnlNum = curCnlNum;
            bool defined = false;

            do
            {
                lastCnlNum = firstCnlNum + cnlCnt - 1;
                // поиск индекса занятого канала, номер которого больше или равен первому
                while (curCnlInd < cnlNumsCnt && cnlNums[curCnlInd] < firstCnlNum)
                    curCnlInd++;
                // проверка допустимости первого канала КП
                if (curCnlInd > 0 && firstCnlNum - cnlNums[curCnlInd - 1] <= cnlsSpace)
                    firstCnlNum += cnlsMultiple;
                // проверка допустимости последнего канала КП
                else if (curCnlInd < cnlNumsCnt && cnlNums[curCnlInd] - lastCnlNum <= cnlsSpace)
                    firstCnlNum += cnlsMultiple;
                else
                    defined = true;
            }
            while (!defined);

            newInCnlInd = curCnlInd;
        }

        /// <summary>
        /// Расчитать номера каналов
        /// </summary>
        private bool CalcCnls(out string errMsg)
        {
            bool hasChannels = false;
            bool hasErrors = false;
            errMsg = "";

            try
            {
                // получение списка номеров существующих входных каналов
                if (inCnlNums == null)
                    inCnlNums = Tables.GetInCnlNums();
                int curInCnlInd = 0;
                int inCnlNumsCnt = inCnlNums.Count;

                // получение списка номеров существующих каналов управления
                if (ctrlCnlNums == null)
                    ctrlCnlNums = Tables.GetCtrlCnlNums();
                int curCtrlCnlInd = 0;
                int ctrlCnlNumsCnt = ctrlCnlNums.Count;

                // определение стартового входного канала
                int inCnlsStart = decimal.ToInt32(numInCnlsStart.Value);
                int inCnlsMultiple = decimal.ToInt32(numInCnlsMultiple.Value);
                int inCnlsShift = decimal.ToInt32(numInCnlsShift.Value);
                int inCnlsSpace = decimal.ToInt32(numInCnlsSpace.Value);
                int remainder = inCnlsStart % inCnlsMultiple;
                int curInCnlNum = remainder > 0 ? inCnlsStart - remainder : inCnlsStart;
                curInCnlNum += inCnlsShift;
                if (curInCnlNum < inCnlsStart)
                    curInCnlNum += inCnlsMultiple;

                // определение стартового канала управления
                int ctrlCnlsStart = decimal.ToInt32(numCtrlCnlsStart.Value);
                int ctrlCnlsMultiple = decimal.ToInt32(numCtrlCnlsMultiple.Value);
                int ctrlCnlsShift = decimal.ToInt32(numCtrlCnlsShift.Value);
                int ctrlCnlsSpace = decimal.ToInt32(numCtrlCnlsSpace.Value);
                remainder = ctrlCnlsStart % ctrlCnlsMultiple;
                int curCtrlCnlNum = remainder > 0 ? ctrlCnlsStart - remainder : ctrlCnlsStart;
                curCtrlCnlNum += ctrlCnlsShift;
                if (curCtrlCnlNum < ctrlCnlsStart)
                    curCtrlCnlNum += ctrlCnlsMultiple;

                // расчёт номеров каналов КП
                foreach (KPParams kpParams in kpParamsList)
                {
                    if (kpParams.Selected)
                    {
                        // определение номеров входных каналов с учётом занятых существующими каналами номеров
                        List<KPView.InCnlProps> defaultCnls = kpParams.KPView.DefaultCnls;
                        int inCnlCnt = defaultCnls == null ? 0 : defaultCnls.Count;
                        if (inCnlCnt > 0)
                        {
                            hasChannels = true;

                            int firstInCnlNum; // номер первого входного канала КП
                            int lastInCnlNum;  // номер последнего входного канала КП
                            int newInCnlInd;   // новый индекс списка номеров входных каналов
                            CalcFirstAndLastNums(curInCnlNum, curInCnlInd, inCnlNums, inCnlNumsCnt,
                                inCnlCnt, inCnlsSpace, inCnlsMultiple, 
                                out firstInCnlNum, out lastInCnlNum, out newInCnlInd);

                            if (lastInCnlNum > ushort.MaxValue)
                            {
                                hasErrors = true;
                                kpParams.InCnlsError = true;
                                kpParams.FirstInCnlNum = 0;
                                kpParams.LastInCnlNum = 0;
                            }
                            else
                            {
                                kpParams.InCnlsError = false;
                                kpParams.FirstInCnlNum = firstInCnlNum;
                                kpParams.LastInCnlNum = lastInCnlNum;

                                curInCnlInd = newInCnlInd;
                                curInCnlNum = firstInCnlNum;
                                do { curInCnlNum += inCnlsMultiple; }
                                while (curInCnlNum - lastInCnlNum <= inCnlsSpace);
                            }

                            // определение номеров каналов управления с учётом занятых существующими каналами номеров
                            int ctrlCnlCnt = kpParams.KPView.DefaultCtrlCnlCount;
                            if (ctrlCnlCnt > 0)
                            {
                                int firstCtrlCnlNum; // номер первого канала управления КП
                                int lastCtrlCnlNum;  // номер последнего канала управления КП
                                int newCtrlCnlInd;   // новый индекс списка номеров каналов управления
                                CalcFirstAndLastNums(curCtrlCnlNum, curCtrlCnlInd, ctrlCnlNums, ctrlCnlNumsCnt,
                                    ctrlCnlCnt, ctrlCnlsSpace, ctrlCnlsMultiple,
                                    out firstCtrlCnlNum, out lastCtrlCnlNum, out newCtrlCnlInd);

                                if (lastCtrlCnlNum > ushort.MaxValue)
                                {
                                    hasErrors = true;
                                    kpParams.CtrlCnlsError = true;
                                    kpParams.FirstCtrlCnlNum = 0;
                                    kpParams.LastCtrlCnlNum = 0;
                                }
                                else
                                {
                                    kpParams.CtrlCnlsError = false;
                                    kpParams.FirstCtrlCnlNum = firstCtrlCnlNum;
                                    kpParams.LastCtrlCnlNum = lastCtrlCnlNum;

                                    curCtrlCnlInd = newCtrlCnlInd;
                                    curCtrlCnlNum = firstCtrlCnlNum;
                                    do { curCtrlCnlNum += ctrlCnlsMultiple; }
                                    while (curCtrlCnlNum - lastCtrlCnlNum <= ctrlCnlsSpace);
                                }
                            }
                        }
                        else
                        {
                            // номера каналов не назначаются, т.к. КП не поддерживает создание каналов
                            kpParams.InCnlsError = false;
                            kpParams.FirstInCnlNum = 0;
                            kpParams.LastInCnlNum = 0;
                            kpParams.CtrlCnlsError = false;
                            kpParams.FirstCtrlCnlNum = 0;
                            kpParams.LastCtrlCnlNum = 0;
                        }

                        // определение текста ячеек таблицы с номерами каналов
                        kpParams.InCnls = kpParams.InCnlsError ? AppPhrases.DevCalcError : 
                            kpParams.FirstInCnlNum == 0 ? AppPhrases.DevHasNoCnls : 
                            kpParams.FirstInCnlNum == kpParams.LastInCnlNum ? kpParams.FirstInCnlNum.ToString() :
                            kpParams.FirstInCnlNum.ToString() + " - " + kpParams.LastInCnlNum;
                        kpParams.CtrlCnls = kpParams.CtrlCnlsError ? AppPhrases.DevCalcError : 
                            kpParams.FirstCtrlCnlNum == 0 ? AppPhrases.DevHasNoCnls : 
                            kpParams.FirstCtrlCnlNum == kpParams.LastCtrlCnlNum ? kpParams.FirstCtrlCnlNum.ToString() :
                            kpParams.FirstCtrlCnlNum.ToString() + " - " + kpParams.LastCtrlCnlNum;
                    }
                    else
                    {
                        // номера каналов не назначаются, т.к. КП не выбран
                        kpParams.InCnlsError = false;
                        kpParams.FirstInCnlNum = 0;
                        kpParams.LastInCnlNum = 0;
                        kpParams.CtrlCnlsError = false;
                        kpParams.FirstCtrlCnlNum = 0;
                        kpParams.LastCtrlCnlNum = 0;

                        kpParams.InCnls = "";
                        kpParams.CtrlCnls = "";
                    }
                }

                if (hasErrors)
                    errMsg = AppPhrases.CalcCnlNumsErrors;
                else if (!hasChannels)
                    errMsg = AppPhrases.CreatedCnlsMissing;
                btnCalc.Enabled = false;
            }
            catch (Exception ex)
            {
                hasErrors = true;
                AppUtils.ProcError(AppPhrases.CalcCnlNumsError + ":\r\n" + ex.Message);
            }

            return hasChannels && !hasErrors;
        }

        /// <summary>
        /// Определение идентификаторов в справочнике
        /// </summary>
        private bool FindDictIDs(SortedList<string, int> dictList, DataTable dataTable, string idName, string errDescr)
        {
            bool error = false;
            dataTable.DefaultView.Sort = "Name";

            for (int i = 0; i < dictList.Count; i++)
            {
                string key = dictList.Keys[i];
                int ind = dataTable.DefaultView.Find(key);
                if (ind < 0)
                {
                    error = true;
                    writer.WriteLine(string.Format(errDescr, key));
                }
                else
                {
                    dictList[key] = (int)dataTable.DefaultView[ind][idName];
                }
            }

            return error;
        }

        /// <summary>
        /// Сохранить каналы в БД
        /// </summary>
        private bool UpdateCnls(DataTable dataTable, string descr)
        {
            int updRows = 0;
            int errRows = 0;
            DataRow[] rowsInError = null;

            SqlCeDataAdapter sqlAdapter = dataTable.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
            updRows = sqlAdapter.Update(dataTable);

            if (dataTable.HasErrors)
            {
                rowsInError = dataTable.GetErrors();
                errRows = rowsInError.Length;
            }

            if (errRows == 0)
            {
                writer.WriteLine(string.Format(descr, updRows));
            }
            else
            {
                writer.WriteLine(string.Format(descr, updRows) + ". " + 
                    string.Format(AppPhrases.ErrorsCount, errRows));
                foreach (DataRow row in rowsInError)
                    writer.WriteLine(string.Format(AppPhrases.CnlError,  row[0], row.RowError));
            }

            return errRows == 0;
        }

        /// <summary>
        /// Создать каналы
        /// </summary>
        private void CreateCnls()
        {
            writer = null;
            bool logCreated = false;
            string logFileName = AppData.ExeDir + "ScadaAdminCreateCnls.txt";

            try
            {
                // создание журанала создания каналов
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;

                string title = DateTime.Now.ToString("G", Localization.Culture) + " " + AppPhrases.CreateCnlsTitle;
                writer.WriteLine(title);
                writer.WriteLine(new string('-', title.Length));
                writer.WriteLine();

                // определение используемых выбранными КП объектов пользовательского интерфейса КП
                List<KPView> usedKPViewList = new List<KPView>();
                foreach (KPParams kpParams in kpParamsList)
                {
                    if (kpParams.Selected)
                    {
                        KPView kpView = kpParams.KPView;
                        if (kpView != null && !usedKPViewList.Contains(kpView))
                            usedKPViewList.Add(kpView);
                    }
                }

                // формирование справочников используемых наименований
                SortedList<string, int> paramList = new SortedList<string, int>();
                SortedList<string, int> unitList = new SortedList<string, int>();
                SortedList<string, int> cmdValList = new SortedList<string, int>();

                foreach (KPView kpView in usedKPViewList)
                {
                    if (kpView.DefaultCnls != null)
                    {
                        foreach (KPView.InCnlProps inCnlProps in kpView.DefaultCnls)
                        {
                            string s = inCnlProps.ParamName;
                            if (s != "" && !paramList.ContainsKey(s))
                                paramList.Add(s, -1);

                            s = inCnlProps.UnitName;
                            if (s != "" && !unitList.ContainsKey(s))
                                unitList.Add(s, -1);

                            if (inCnlProps.CtrlCnlProps != null)
                            {
                                s = inCnlProps.CtrlCnlProps.CmdValName;
                                if (s != "" && !cmdValList.ContainsKey(s))
                                    cmdValList.Add(s, -1);
                            }
                        }
                    }
                }

                // определение идентификаторов в справочниках
                writer.WriteLine(AppPhrases.CheckDicts);
                bool paramError = FindDictIDs(paramList, Tables.GetParamTable(), "ParamID", AppPhrases.ParamNotFound);
                bool unitError = FindDictIDs(unitList, Tables.GetUnitTable(), "UnitID", AppPhrases.UnitNotFound);
                bool cmdValError = FindDictIDs(cmdValList, Tables.GetCmdValTable(), "CmdValID", 
                    AppPhrases.CmdValsNotFound);

                if (paramError || unitError || cmdValError)
                {
                    writer.WriteLine(AppPhrases.CreateCnlsImpossible);
                }
                else
                {
                    writer.WriteLine(AppPhrases.CreateCnlsStart);

                    // заполнение схем таблиц входных каналов и каналов управления
                    DataTable tblInCnl = new DataTable("InCnl");
                    DataTable tblCtrlCnl = new DataTable("CtrlCnl");
                    Tables.FillTableSchema(tblInCnl);
                    Tables.FillTableSchema(tblCtrlCnl);

                    // получение таблицы форматов чисел
                    DataTable tblFormat = Tables.GetFormatTable();
                    tblFormat.DefaultView.Sort = "ShowNumber, DecDigits";

                    // создание каналов для КП
                    foreach (KPParams kpParams in kpParamsList)
                    {
                        if (kpParams.Selected)
                        {
                            int inCnlNum = kpParams.FirstInCnlNum;
                            int ctrlCnlNum = kpParams.FirstCtrlCnlNum;

                            foreach (KPView.InCnlProps inCnlProps in kpParams.KPView.DefaultCnls)
                            {
                                KPView.CtrlCnlProps ctrlCnlProps = inCnlProps.CtrlCnlProps;
                                object lastCtrlCnlNum;
                                if (ctrlCnlProps == null)
                                {
                                    lastCtrlCnlNum = DBNull.Value;
                                }
                                else
                                {
                                    // создание канала управления
                                    DataRow newCtrlCnlRow = tblCtrlCnl.NewRow();
                                    newCtrlCnlRow["CtrlCnlNum"] = ctrlCnlNum;
                                    newCtrlCnlRow["Active"] = true;
                                    newCtrlCnlRow["Name"] = ctrlCnlProps.Name;
                                    newCtrlCnlRow["CmdTypeID"] = (int)ctrlCnlProps.CmdType;
                                    newCtrlCnlRow["ObjNum"] = kpParams.ObjNum;
                                    newCtrlCnlRow["KPNum"] = kpParams.KPNum;
                                    newCtrlCnlRow["CmdNum"] = ctrlCnlProps.CmdNum;
                                    newCtrlCnlRow["CmdValID"] = ctrlCnlProps.CmdValName == "" ?
                                        DBNull.Value : (object)cmdValList[ctrlCnlProps.CmdValName];
                                    newCtrlCnlRow["FormulaUsed"] = ctrlCnlProps.FormulaUsed;
                                    newCtrlCnlRow["Formula"] = ctrlCnlProps.Formula;
                                    newCtrlCnlRow["EvEnabled"] = ctrlCnlProps.EvEnabled;
                                    newCtrlCnlRow["ModifiedDT"] = DateTime.Now;

                                    tblCtrlCnl.Rows.Add(newCtrlCnlRow);
                                    lastCtrlCnlNum = ctrlCnlNum;
                                    ctrlCnlNum++;
                                }

                                // создание входного канала
                                DataRow newInCnlRow = tblInCnl.NewRow();
                                newInCnlRow["CnlNum"] = inCnlNum;
                                newInCnlRow["Active"] = true;
                                newInCnlRow["CnlNum"] = inCnlNum;
                                newInCnlRow["Name"] = inCnlProps.Name;
                                newInCnlRow["CnlTypeID"] = (int)inCnlProps.CnlType;
                                newInCnlRow["ModifiedDT"] = DateTime.Now;
                                newInCnlRow["ObjNum"] = kpParams.ObjNum;
                                newInCnlRow["KPNum"] = kpParams.KPNum;
                                newInCnlRow["Signal"] = inCnlProps.Signal;
                                newInCnlRow["FormulaUsed"] = inCnlProps.FormulaUsed;
                                newInCnlRow["Formula"] = inCnlProps.Formula;
                                newInCnlRow["Averaging"] = inCnlProps.Averaging;
                                newInCnlRow["ParamID"] = inCnlProps.ParamName == "" ?
                                    DBNull.Value : (object)paramList[inCnlProps.ParamName];

                                newInCnlRow["FormatID"] = DBNull.Value;
                                if (inCnlProps.ShowNumber)
                                {
                                    int ind = tblFormat.DefaultView.Find(new object[] { true, inCnlProps.DecDigits });
                                    if (ind >= 0)
                                    {
                                        newInCnlRow["FormatID"] = tblFormat.DefaultView[ind]["FormatID"];
                                    }
                                    else
                                    {
                                        writer.WriteLine(string.Format(
                                            AppPhrases.NumFormatNotFound, inCnlNum, inCnlProps.DecDigits));
                                    }
                                }
                                else
                                {
                                    int ind = tblFormat.DefaultView.Find(new object[] { false, DBNull.Value });
                                    if (ind >= 0)
                                    {
                                        newInCnlRow["FormatID"] = tblFormat.DefaultView[ind]["FormatID"];
                                    }
                                    else
                                    {
                                        writer.WriteLine(string.Format(AppPhrases.TextFormatNotFound, inCnlNum));
                                    }
                                }

                                newInCnlRow["UnitID"] = inCnlProps.UnitName == "" ?
                                    DBNull.Value : (object)unitList[inCnlProps.UnitName];
                                newInCnlRow["CtrlCnlNum"] = lastCtrlCnlNum;
                                newInCnlRow["EvEnabled"] = inCnlProps.EvEnabled;
                                newInCnlRow["EvSound"] = inCnlProps.EvSound;
                                newInCnlRow["EvOnChange"] = inCnlProps.EvOnChange;
                                newInCnlRow["EvOnUndef"] = inCnlProps.EvOnUndef;
                                newInCnlRow["LimLowCrash"] = double.IsNaN(inCnlProps.LimLowCrash) ?
                                    DBNull.Value : (object)inCnlProps.LimLowCrash;
                                newInCnlRow["LimLow"] = double.IsNaN(inCnlProps.LimLow) ?
                                    DBNull.Value : (object)inCnlProps.LimLow;
                                newInCnlRow["LimHigh"] = double.IsNaN(inCnlProps.LimHigh) ?
                                    DBNull.Value : (object)inCnlProps.LimHigh;
                                newInCnlRow["LimHighCrash"] = double.IsNaN(inCnlProps.LimHighCrash) ?
                                    DBNull.Value : (object)inCnlProps.LimHighCrash;

                                tblInCnl.Rows.Add(newInCnlRow);
                                inCnlNum++;
                            }
                        }
                    }

                    // сохранение каналов в БД
                    bool updateOk = UpdateCnls(tblCtrlCnl, AppPhrases.AddedCtrlCnlsCount);
                    updateOk = UpdateCnls(tblInCnl, AppPhrases.AddedInCnlsCount) && updateOk;
                    string msg = updateOk ? AppPhrases.CreateCnlsComplSucc : AppPhrases.CreateCnlsComplWithErr;
                    writer.WriteLine();
                    writer.WriteLine(msg);
                    if (updateOk)
                    {
                        ScadaUtils.ShowInfo(msg + AppPhrases.RefreshRequired);
                    }
                    else
                    {
                        AppData.ErrLog.WriteAction(msg, Log.ActTypes.Error);
                        ScadaUtils.ShowError(msg + AppPhrases.RefreshRequired);
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = AppPhrases.CreateCnlsError + ":\r\n" + ex.Message;
                try { writer.WriteLine(errMsg); }
                catch { }
                AppUtils.ProcError(errMsg);
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }

            if (logCreated)
                Process.Start(logFileName);
        }


        private void FrmCreateCnls_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "ScadaAdmin.FrmCreateCnls");
        }

        private void FrmCreateCnls_Shown(object sender, EventArgs e)
        {
            // загрузка библиотек КП
            LoadKPDlls();

            // заполнение таблицы КП
            FillKPGrid();

            // расчёт и отображение номеров каналов
            string errMsg;
            btnCreate.Enabled = CalcCnls(out errMsg);
            gvKPSel.Invalidate();
        }

        private void gvKPSel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // установка цвета ячейки
            int rowInd = e.RowIndex;
            if (0 <= rowInd && rowInd < kpParamsList.Count)
            {
                int colInd = e.ColumnIndex;
                if (colInd == colInCnls.Index && kpParamsList[rowInd].InCnlsError ||
                    colInd == colCtrlCnls.Index && kpParamsList[rowInd].CtrlCnlsError)
                    e.CellStyle.ForeColor = Color.Red;
                else
                    e.CellStyle.ForeColor = kpParamsList[rowInd].Color;
            }
        }

        private void gvKPSel_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int rowInd = e.RowIndex;
            if (0 <= rowInd && rowInd < kpParamsList.Count)
            {
                if (kpParamsList[rowInd].Enabled)
                {
                    if (e.ColumnIndex == colSelected.Index)
                    {
                        // разрешение расчёта и создания каналов
                        btnCalc.Enabled = true;
                        btnCreate.Enabled = true;
                    }
                }
                else
                {
                    // отмена редактирования ячейки
                    e.Cancel = true;
                }
            }
        }

        private void numCnls_ValueChanged(object sender, EventArgs e)
        {
            btnCalc.Enabled = true;
            btnCreate.Enabled = true;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // выбор всех КП
            foreach (KPParams kpParams in kpParamsList)
                kpParams.Selected = kpParams.Enabled;
            gvKPSel.Invalidate();

            btnCalc.Enabled = true;
            btnCreate.Enabled = true;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            // отмена выбора всех КП
            foreach (KPParams kpParams in kpParamsList)
                kpParams.Selected = false;
            gvKPSel.Invalidate();

            btnCalc.Enabled = true;
            btnCreate.Enabled = true;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            // расчёт и отображение номеров каналов
            string errMsg;
            btnCreate.Enabled = CalcCnls(out errMsg);
            gvKPSel.Invalidate();
            if (errMsg != "")
                ScadaUtils.ShowError(errMsg);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (btnCalc.Enabled)
                btnCalc_Click(null, null);

            // создание каналов
            if (btnCreate.Enabled)
                CreateCnls();
        }
    }
}