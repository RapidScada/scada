/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Summary  : Creating channels according to specified devices
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace ScadaAdmin
{
    /// <summary>
    /// Creating channels according to selected devices
    /// <para>Создание каналов в соответствии с указанными устройствами</para>
    /// </summary>
    internal static class CreateCnls
    {
        /// <summary>
        /// Состояния библиотеки КП
        /// </summary>
        public enum DllStates { NotFound, Loaded, Error }

        /// <summary>
        /// Информация о КП, для которого создаются каналы
        /// </summary>
        public class KPInfo
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPInfo()
            {
                Enabled = false;
                Color = Color.Gray;
                KPView = null;

                Selected = false;
                KPNum = 0;
                KPName = "";
                ObjNum = DBNull.Value;
                DllFileName = "";
                DllState = DllStates.NotFound;

                InCnlNumsErr = false;
                FirstInCnlNum = 0;
                LastInCnlNum = 0;

                CtrlCnlNumsErr = false;
                FirstCtrlCnlNum = 0;
                LastCtrlCnlNum = 0;
            }

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
            public DllStates DllState { get; set; }
            /// <summary>
            /// Получить имя файла и состояние загрузки DLL
            /// </summary>
            public string DllWithState
            {
                get
                {
                    string state;
                    switch (DllState)
                    {
                        case DllStates.NotFound:
                            state = AppPhrases.DllNotFound;
                            break;
                        case DllStates.Loaded:
                            state = AppPhrases.DllLoaded;
                            break;
                        default: // DllStates.Error
                            state = AppPhrases.DllError;
                            break;
                    }

                    return DllFileName + " (" + state + ")";
                }
            }

            /// <summary>
            /// Получить или установить признак ошибки при расчёте номеров входных каналов
            /// </summary>
            public bool InCnlNumsErr { get; set; }
            /// <summary>
            /// Получить или установить номер первого входного канала
            /// </summary>
            public int FirstInCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер последнего входного канала
            /// </summary>
            public int LastInCnlNum { get; set; }
            /// <summary>
            /// Получить номера входных каналов
            /// </summary>
            public string InCnls
            {
                get
                {
                    return 
                        InCnlNumsErr ? AppPhrases.DevCalcError :
                        FirstInCnlNum == 0 ? AppPhrases.DevHasNoCnls :
                        FirstInCnlNum == LastInCnlNum ? FirstInCnlNum.ToString() :
                        FirstInCnlNum.ToString() + " - " + LastInCnlNum;
                }
            }

            /// <summary>
            /// Получить или установить признак ошибки при расчёте номеров каналов управления
            /// </summary>
            public bool CtrlCnlNumsErr { get; set; }
            /// <summary>
            /// Получить или установить номер первого канала управления
            /// </summary>
            public int FirstCtrlCnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер последнего канала управления
            /// </summary>
            public int LastCtrlCnlNum { get; set; }
            /// <summary>
            /// Получить номера каналов управления
            /// </summary>
            public string CtrlCnls
            {
                get
                {
                    return 
                        CtrlCnlNumsErr ? AppPhrases.DevCalcError :
                        FirstCtrlCnlNum == 0 ? AppPhrases.DevHasNoCnls :
                        FirstCtrlCnlNum == LastCtrlCnlNum ? FirstCtrlCnlNum.ToString() :
                        FirstCtrlCnlNum.ToString() + " - " + LastCtrlCnlNum;
                }
            }

            /// <summary>
            /// Установить номера входных каналов
            /// </summary>
            public void SetInCnlNums(bool error, int firstNum = 0, int lastNum = 0)
            {
                InCnlNumsErr = error;
                FirstInCnlNum = firstNum;
                LastInCnlNum = lastNum;
            }
            /// <summary>
            /// Установить номера каналов управления
            /// </summary>
            public void SetCtrlCnlNums(bool error, int firstNum = 0, int lastNum = 0)
            {
                CtrlCnlNumsErr = error;
                FirstCtrlCnlNum = firstNum;
                LastCtrlCnlNum = lastNum;
            }
        }

        /// <summary>
        /// Параметры нумерации каналов
        /// </summary>
        public class CnlNumParams
        {
            /// <summary>
            /// Получить или установить начальный номер канала
            /// </summary>
            public int Start { get; set; }
            /// <summary>
            /// Получить или установить кратность первого канала для КП
            /// </summary>
            public int Multiple { get; set; }
            /// <summary>
            /// Получить или установить смещение первого канала для КП
            /// </summary>
            public int Shift { get; set; }
            /// <summary>
            /// Получить или установить минимальное количество свободных номеров каналов между КП
            /// </summary>
            public int Space { get; set; }
        }


        /// <summary>
        /// Расчитать номера первого и последнего каналов КП
        /// </summary>
        private static void CalcFirstAndLastNums(int curCnlNum, int curCnlInd, List<int> cnlNums, int cnlNumsCnt,
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
        /// Определение идентификаторов в справочнике
        /// </summary>
        private static bool FindDictIDs(SortedList<string, int> dictList, DataTable dataTable, string idName, 
            string errDescr)
        {
            bool error = false;
            /*dataTable.DefaultView.Sort = "Name";

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
            }*/

            return error;
        }

        /// <summary>
        /// Сохранить каналы в БД
        /// </summary>
        private static bool UpdateCnls(DataTable dataTable, string descr, out int updRowCnt)
        {
            updRowCnt = 0;
            int errRowCnt = 0;
            /*DataRow[] rowsInError = null;

            SqlCeDataAdapter sqlAdapter = dataTable.ExtendedProperties["DataAdapter"] as SqlCeDataAdapter;
            updRowCnt = sqlAdapter.Update(dataTable);

            if (dataTable.HasErrors)
            {
                rowsInError = dataTable.GetErrors();
                errRowCnt = rowsInError.Length;
            }

            if (errRowCnt == 0)
            {
                writer.WriteLine(string.Format(descr, updRowCnt));
            }
            else
            {
                writer.WriteLine(string.Format(descr, updRowCnt) + ". " + 
                    string.Format(AppPhrases.ErrorsCount, errRowCnt));
                foreach (DataRow row in rowsInError)
                    writer.WriteLine(string.Format(AppPhrases.CnlError,  row[0], row.RowError));
            }*/

            return errRowCnt == 0;
        }


        /// <summary>
        /// Расчитать номера каналов и записать их в список информации о КП
        /// </summary>
        public static bool CalcCnlNums(Dictionary<string, Type> kpViewTypes, List<KPInfo> kpInfoList,
            List<int> inCnlNums, CnlNumParams inCnlNumParams, List<int> ctrlCnlNums, CnlNumParams ctrlCnlNumParams, 
            out string errMsg)
        {
            if (kpViewTypes == null)
                throw new ArgumentNullException("kpViewTypes");
            if (kpInfoList == null)
                throw new ArgumentNullException("kpInfoList");
            if (inCnlNums == null)
                throw new ArgumentNullException("inCnlNums");
            if (inCnlNumParams == null)
                throw new ArgumentNullException("inCnlNumParams");
            if (ctrlCnlNums == null)
                throw new ArgumentNullException("ctrlCnlNums");
            if (ctrlCnlNumParams == null)
                throw new ArgumentNullException("ctrlCnlNumParams");

            bool hasChannels = false;
            bool hasErrors = false;
            errMsg = "";

            try
            {
                // определение стартового номера входного канала
                int inCnlsStart = inCnlNumParams.Start;
                int inCnlsMultiple = inCnlNumParams.Multiple;
                int inCnlsSpace = inCnlNumParams.Space;
                int remainder = inCnlsStart % inCnlsMultiple;
                int curInCnlNum = remainder > 0 ? inCnlsStart - remainder : inCnlsStart;
                curInCnlNum += inCnlNumParams.Shift;
                if (curInCnlNum < inCnlsStart)
                    curInCnlNum += inCnlsMultiple;

                // определение стартового номера канала управления
                int ctrlCnlsStart = ctrlCnlNumParams.Start;
                int ctrlCnlsMultiple = ctrlCnlNumParams.Multiple;
                int ctrlCnlsSpace = ctrlCnlNumParams.Space;
                remainder = ctrlCnlsStart % ctrlCnlNumParams.Multiple;
                int curCtrlCnlNum = remainder > 0 ? ctrlCnlsStart - remainder : ctrlCnlsStart;
                curCtrlCnlNum += ctrlCnlNumParams.Shift;
                if (curCtrlCnlNum < ctrlCnlNumParams.Start)
                    curCtrlCnlNum += ctrlCnlNumParams.Multiple;

                // расчёт номеров каналов КП
                int curInCnlInd = 0;
                int inCnlNumsCnt = inCnlNums.Count;
                int curCtrlCnlInd = 0;
                int ctrlCnlNumsCnt = ctrlCnlNums.Count;

                foreach (KPInfo kpInfo in kpInfoList)
                {
                    if (kpInfo.Selected)
                    {
                        // получение типа интерфейса КП
                        Type kpViewType;
                        if (!kpViewTypes.TryGetValue(kpInfo.DllFileName, out kpViewType))
                            continue;

                        // создание экземпляра класса интерфейса КП
                        try
                        {
                            kpInfo.KPView = KPFactory.GetKPView(kpViewType, kpInfo.KPNum);
                        }
                        catch
                        {
                            kpInfo.SetInCnlNums(true);
                            kpInfo.SetCtrlCnlNums(true);
                        }

                        // получение прототипов каналов КП по умолчанию
                        KPView.KPCnlPrototypes defaultCnls = kpInfo.KPView.DefaultCnls;

                        // определение номеров входных каналов с учётом занятых существующими каналами номеров
                        if (defaultCnls.InCnls.Count > 0)
                        {
                            hasChannels = true;

                            int firstInCnlNum; // номер первого входного канала КП
                            int lastInCnlNum;  // номер последнего входного канала КП
                            int newInCnlInd;   // новый индекс списка номеров входных каналов
                            CalcFirstAndLastNums(curInCnlNum, curInCnlInd, inCnlNums, inCnlNumsCnt,
                                defaultCnls.InCnls.Count, inCnlsSpace, inCnlsMultiple,
                                out firstInCnlNum, out lastInCnlNum, out newInCnlInd);

                            if (lastInCnlNum > ushort.MaxValue)
                            {
                                hasErrors = true;
                                kpInfo.SetInCnlNums(true);
                            }
                            else
                            {
                                kpInfo.SetInCnlNums(false, firstInCnlNum, lastInCnlNum);

                                curInCnlInd = newInCnlInd;
                                curInCnlNum = firstInCnlNum;
                                do { curInCnlNum += inCnlsMultiple; }
                                while (curInCnlNum - lastInCnlNum <= inCnlsSpace);
                            }
                        }
                        else
                        {
                            // номера каналов не назначаются, т.к. КП не поддерживает создание входных каналов
                            kpInfo.SetInCnlNums(false);
                        }

                        // определение номеров каналов управления с учётом занятых существующими каналами номеров
                        if (defaultCnls.CtrlCnls.Count > 0)
                        {
                            int firstCtrlCnlNum; // номер первого канала управления КП
                            int lastCtrlCnlNum;  // номер последнего канала управления КП
                            int newCtrlCnlInd;   // новый индекс списка номеров каналов управления
                            CalcFirstAndLastNums(curCtrlCnlNum, curCtrlCnlInd, ctrlCnlNums, ctrlCnlNumsCnt,
                                defaultCnls.CtrlCnls.Count, ctrlCnlsSpace, ctrlCnlsMultiple,
                                out firstCtrlCnlNum, out lastCtrlCnlNum, out newCtrlCnlInd);

                            if (lastCtrlCnlNum > ushort.MaxValue)
                            {
                                hasErrors = true;
                                kpInfo.SetCtrlCnlNums(true);
                            }
                            else
                            {
                                kpInfo.SetCtrlCnlNums(false, firstCtrlCnlNum, lastCtrlCnlNum);

                                curCtrlCnlInd = newCtrlCnlInd;
                                curCtrlCnlNum = firstCtrlCnlNum;
                                do { curCtrlCnlNum += ctrlCnlsMultiple; }
                                while (curCtrlCnlNum - lastCtrlCnlNum <= ctrlCnlsSpace);
                            }
                        }
                        else
                        {
                            // номера каналов не назначаются, т.к. КП не поддерживает создание каналов управления
                            kpInfo.SetCtrlCnlNums(false);
                        }
                    }
                    else
                    {
                        // номера каналов не назначаются, т.к. КП не выбран
                        kpInfo.SetInCnlNums(false);
                        kpInfo.SetCtrlCnlNums(false);
                    }
                }

                if (hasErrors)
                    errMsg = AppPhrases.CalcCnlNumsErrors;
                else if (!hasChannels)
                    errMsg = AppPhrases.CreatedCnlsMissing;
            }
            catch (Exception ex)
            {
                hasErrors = true;
                AppUtils.ProcError(AppPhrases.CalcCnlNumsError + ":\r\n" + ex.Message);
            }

            return hasChannels && !hasErrors;
        }

        /// <summary>
        /// Создать каналы, используя ранее рассчитанные номера
        /// </summary>
        public static bool CreateChannels(List<KPInfo> kpInfoList, bool insertKPName, 
            string logFileName, out bool logCreated, out string msg)
        {
            //writer = null;
            logCreated = false;
            msg = "";

            /*try
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
                            string kpNameToInsert = insertKPName ? kpParams.KPName + " - " : "";

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
                                    newCtrlCnlRow["Name"] = kpNameToInsert + ctrlCnlProps.Name;
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
                                newInCnlRow["Name"] = kpNameToInsert + inCnlProps.Name;
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
                    int updRowCnt1, updRowCnt2;
                    bool updateOK = UpdateCnls(tblCtrlCnl, AppPhrases.AddedCtrlCnlsCount, out updRowCnt1);
                    updateOK = UpdateCnls(tblInCnl, AppPhrases.AddedInCnlsCount, out updRowCnt2) && updateOK;
                    string msg = updateOK ? AppPhrases.CreateCnlsComplSucc : AppPhrases.CreateCnlsComplWithErr;
                    writer.WriteLine();
                    writer.WriteLine(msg);

                    if (updRowCnt1 + updRowCnt2 > 0)
                        msg += AppPhrases.RefreshRequired;

                    if (updateOK)
                        ScadaUtils.ShowInfo(msg);
                    else
                        AppUtils.ProcError(msg);
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
            }*/

            return false;
        }
    }
}
