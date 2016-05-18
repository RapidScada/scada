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
 * Module   : SCADA-Administrator
 * Summary  : Creating channels according to specified devices
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2016
 */

using Scada;
using Scada.Comm.Devices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
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
            private KPInfo()
            {
                Enabled = false;
                Color = Color.Gray;
                DefaultCnls = null;

                Selected = false;
                KPNum = 0;
                KPName = "";
                CommLineNum = 0;
                ObjNum = DBNull.Value;
                DllFileName = "";
                DllState = DllStates.NotFound;

                InCnlNumsErr = false;
                FirstInCnlNum = -1;
                LastInCnlNum = -1;

                CtrlCnlNumsErr = false;
                FirstCtrlCnlNum = -1;
                LastCtrlCnlNum = -1;
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
            /// Получить или установить прототипы каналов КП по умолчанию
            /// </summary>
            public KPView.KPCnlPrototypes DefaultCnls { get; set; }

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
            /// Получить или установить номер линии связи
            /// </summary>
            public int CommLineNum { get; set; }
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
                        FirstInCnlNum < 0 ? "" :
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
                        FirstCtrlCnlNum < 0 ? "" :
                        FirstCtrlCnlNum == 0 ? AppPhrases.DevHasNoCnls :
                        FirstCtrlCnlNum == LastCtrlCnlNum ? FirstCtrlCnlNum.ToString() :
                        FirstCtrlCnlNum.ToString() + " - " + LastCtrlCnlNum;
                }
            }

            /// <summary>
            /// Создать объект информации о КП
            /// </summary>
            public static KPInfo Create(DataRow rowKP, DataTable tblKPType)
            {
                CreateCnls.KPInfo kpInfo = new CreateCnls.KPInfo();
                kpInfo.KPNum = (int)rowKP["KPNum"];
                kpInfo.KPName = (string)rowKP["Name"];
                object commLineNum = rowKP["CommLineNum"];
                kpInfo.CommLineNum = commLineNum == DBNull.Value ? 0 : (int)commLineNum;

                tblKPType.DefaultView.RowFilter = "KPTypeID = " + rowKP["KPTypeID"];
                object dllFileName = tblKPType.DefaultView[0]["DllFileName"];
                kpInfo.DllFileName = dllFileName == null || dllFileName == DBNull.Value ?
                    "" : (string)dllFileName;

                return kpInfo;
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
            StreamWriter writer, string errDescr)
        {
            dataTable.DefaultView.Sort = "Name";

            for (int i = 0; i < dictList.Count; i++)
            {
                string key = dictList.Keys[i];
                int ind = dataTable.DefaultView.Find(key);
                if (ind < 0)
                {
                    writer.WriteLine(string.Format(errDescr, key));
                    return false;
                }
                else
                {
                    dictList[key] = (int)dataTable.DefaultView[ind][idName];
                }
            }

            return true;
        }

        /// <summary>
        /// Создать строку входного канала
        /// </summary>
        private static DataRow CreateInCnlRow(DataTable tblInCnl, DataTable tblFormat, 
            SortedList<string, int> paramList, SortedList<string, int> unitList, 
            KPView.InCnlPrototype inCnl, object objNum, int kpNum, string kpNameToInsert, StreamWriter writer)
        {
            DataRow newInCnlRow = tblInCnl.NewRow();
            newInCnlRow["CnlNum"] = inCnl.CnlNum;
            newInCnlRow["Active"] = true;

            int maxCnlNameLen = tblInCnl.Columns["Name"].MaxLength;
            string cnlName = kpNameToInsert + inCnl.CnlName;
            if (cnlName.Length > maxCnlNameLen)
            {
                cnlName = cnlName.Substring(0, maxCnlNameLen);
                writer.WriteLine(string.Format(AppPhrases.InCnlNameTrancated, inCnl.CnlNum));
            }
            newInCnlRow["Name"] = cnlName;

            newInCnlRow["CnlTypeID"] = inCnl.CnlTypeID;
            newInCnlRow["ObjNum"] = objNum;
            newInCnlRow["KPNum"] = kpNum;
            newInCnlRow["Signal"] = inCnl.Signal;
            newInCnlRow["FormulaUsed"] = inCnl.FormulaUsed;
            newInCnlRow["Formula"] = inCnl.Formula;
            newInCnlRow["Averaging"] = inCnl.Averaging;
            newInCnlRow["ParamID"] = string.IsNullOrEmpty(inCnl.ParamName) ?
                DBNull.Value : (object)paramList[inCnl.ParamName];

            newInCnlRow["FormatID"] = DBNull.Value;
            if (inCnl.ShowNumber)
            {
                int ind = tblFormat.DefaultView.Find(new object[] { true, inCnl.DecDigits });
                if (ind >= 0)
                    newInCnlRow["FormatID"] = tblFormat.DefaultView[ind]["FormatID"];
                else
                    writer.WriteLine(string.Format(AppPhrases.NumFormatNotFound, inCnl.CnlNum, inCnl.DecDigits));
            }
            else
            {
                int ind = tblFormat.DefaultView.Find(new object[] { false, DBNull.Value });
                if (ind >= 0)
                    newInCnlRow["FormatID"] = tblFormat.DefaultView[ind]["FormatID"];
                else
                    writer.WriteLine(string.Format(AppPhrases.TextFormatNotFound, inCnl.CnlNum));
            }

            newInCnlRow["UnitID"] = string.IsNullOrEmpty(inCnl.UnitName) ?
                DBNull.Value : (object)unitList[inCnl.UnitName];
            newInCnlRow["CtrlCnlNum"] =
                inCnl.CtrlCnlProps != null && inCnl.CtrlCnlProps.CtrlCnlNum > 0 ?
                    (object)inCnl.CtrlCnlProps.CtrlCnlNum : DBNull.Value;
            newInCnlRow["EvEnabled"] = inCnl.EvEnabled;
            newInCnlRow["EvSound"] = inCnl.EvSound;
            newInCnlRow["EvOnChange"] = inCnl.EvOnChange;
            newInCnlRow["EvOnUndef"] = inCnl.EvOnUndef;
            newInCnlRow["LimLowCrash"] = double.IsNaN(inCnl.LimLowCrash) ?
                DBNull.Value : (object)inCnl.LimLowCrash;
            newInCnlRow["LimLow"] = double.IsNaN(inCnl.LimLow) ?
                DBNull.Value : (object)inCnl.LimLow;
            newInCnlRow["LimHigh"] = double.IsNaN(inCnl.LimHigh) ?
                DBNull.Value : (object)inCnl.LimHigh;
            newInCnlRow["LimHighCrash"] = double.IsNaN(inCnl.LimHighCrash) ?
                DBNull.Value : (object)inCnl.LimHighCrash;
            newInCnlRow["ModifiedDT"] = DateTime.Now;

            return newInCnlRow;
        }

        /// <summary>
        /// Создать строку канала управления
        /// </summary>
        private static DataRow CreateCtrlCnlRow(DataTable tblCtrlCnl, SortedList<string, int> cmdValList,
            KPView.CtrlCnlPrototype ctrlCnl, object objNum, int kpNum, string kpNameToInsert, StreamWriter writer)
        {
            DataRow newCtrlCnlRow = tblCtrlCnl.NewRow();
            newCtrlCnlRow["CtrlCnlNum"] = ctrlCnl.CtrlCnlNum;
            newCtrlCnlRow["Active"] = true;

            int maxCtrlCnlNameLen = tblCtrlCnl.Columns["Name"].MaxLength;
            string ctrlCnlName = kpNameToInsert + ctrlCnl.CtrlCnlName;
            if (ctrlCnlName.Length > maxCtrlCnlNameLen)
            {
                ctrlCnlName = ctrlCnlName.Substring(0, maxCtrlCnlNameLen);
                writer.WriteLine(string.Format(AppPhrases.CtrlCnlNameTrancated, ctrlCnl.CtrlCnlNum));
            }
            newCtrlCnlRow["Name"] = ctrlCnlName;

            newCtrlCnlRow["CmdTypeID"] = ctrlCnl.CmdTypeID;
            newCtrlCnlRow["ObjNum"] = objNum;
            newCtrlCnlRow["KPNum"] = kpNum;
            newCtrlCnlRow["CmdNum"] = ctrlCnl.CmdNum;
            newCtrlCnlRow["CmdValID"] = string.IsNullOrEmpty(ctrlCnl.CmdValName) ?
                DBNull.Value : (object)cmdValList[ctrlCnl.CmdValName];
            newCtrlCnlRow["FormulaUsed"] = ctrlCnl.FormulaUsed;
            newCtrlCnlRow["Formula"] = ctrlCnl.Formula;
            newCtrlCnlRow["EvEnabled"] = ctrlCnl.EvEnabled;
            newCtrlCnlRow["ModifiedDT"] = DateTime.Now;

            return newCtrlCnlRow;
        }

        /// <summary>
        /// Сохранить каналы в БД
        /// </summary>
        private static bool UpdateCnls(DataTable dataTable, string descr, StreamWriter writer, out int updRowCnt)
        {
            updRowCnt = 0;
            int errRowCnt = 0;
            DataRow[] rowsInError = null;

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
                writer.WriteLine(string.Format(descr, updRowCnt) + " " + 
                    string.Format(AppPhrases.ErrorsCount, errRowCnt));
                foreach (DataRow row in rowsInError)
                    writer.WriteLine(string.Format(AppPhrases.CnlError,  row[0], row.RowError));
            }

            return errRowCnt == 0;
        }


        /// <summary>
        /// Расчитать номера каналов и записать их в список информации о КП
        /// </summary>
        public static bool CalcCnlNums(Dictionary<string, Type> kpViewTypes, List<KPInfo> kpInfoList, 
            Scada.Comm.AppDirs commDirs, List<int> inCnlNums, CnlNumParams inCnlNumParams, 
            List<int> ctrlCnlNums, CnlNumParams ctrlCnlNumParams, out string errMsg)
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
                // загрузка настроек SCADA-Коммуникатора
                Scada.Comm.Settings commSett = new Scada.Comm.Settings();
                if (!commSett.Load(commDirs.ConfigDir + Scada.Comm.Settings.DefFileName, out errMsg))
                    throw new Exception(errMsg);

                // заполнение справочника свойств КП
                Dictionary<int, KPView.KPProperties> kpPropsDict = new Dictionary<int, KPView.KPProperties>();
                foreach (Scada.Comm.Settings.CommLine commLine in commSett.CommLines)
                {
                    foreach (Scada.Comm.Settings.KP kp in commLine.ReqSequence)
                    {
                        if (!kpPropsDict.ContainsKey(kp.Number))
                            kpPropsDict.Add(kp.Number, new KPView.KPProperties(commLine.CustomParams, kp.CmdLine));
                    }
                }

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
                        KPView kpView = null;
                        try
                        {
                            kpView = KPFactory.GetKPView(kpViewType, kpInfo.KPNum);
                            KPView.KPProperties kpProps;
                            if (kpPropsDict.TryGetValue(kpInfo.KPNum, out kpProps))
                                kpView.KPProps = kpProps;
                            kpView.AppDirs = commDirs;
                        }
                        catch
                        {
                            kpInfo.SetInCnlNums(true);
                            kpInfo.SetCtrlCnlNums(true);
                            continue;
                        }

                        // получение прототипов каналов КП по умолчанию
                        try
                        {
                            kpInfo.DefaultCnls = kpView.DefaultCnls;
                        }
                        catch
                        {
                            kpInfo.SetInCnlNums(true);
                            kpInfo.SetCtrlCnlNums(true);
                            continue;
                        }

                        // определение номеров входных каналов с учётом занятых существующими каналами номеров
                        if (kpInfo.DefaultCnls != null && kpInfo.DefaultCnls.InCnls.Count > 0)
                        {
                            hasChannels = true;

                            int firstInCnlNum; // номер первого входного канала КП
                            int lastInCnlNum;  // номер последнего входного канала КП
                            int newInCnlInd;   // новый индекс списка номеров входных каналов
                            CalcFirstAndLastNums(curInCnlNum, curInCnlInd, inCnlNums, inCnlNumsCnt,
                                kpInfo.DefaultCnls.InCnls.Count, inCnlsSpace, inCnlsMultiple,
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
                        if (kpInfo.DefaultCnls != null && kpInfo.DefaultCnls.CtrlCnls.Count > 0)
                        {
                            hasChannels = true;

                            int firstCtrlCnlNum; // номер первого канала управления КП
                            int lastCtrlCnlNum;  // номер последнего канала управления КП
                            int newCtrlCnlInd;   // новый индекс списка номеров каналов управления
                            CalcFirstAndLastNums(curCtrlCnlNum, curCtrlCnlInd, ctrlCnlNums, ctrlCnlNumsCnt,
                                kpInfo.DefaultCnls.CtrlCnls.Count, ctrlCnlsSpace, ctrlCnlsMultiple,
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
                        kpInfo.SetInCnlNums(false, -1, -1);
                        kpInfo.SetCtrlCnlNums(false, -1, -1);
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
                errMsg = AppPhrases.CalcCnlNumsError + ":\r\n" + ex.Message;
            }

            return hasChannels && !hasErrors;
        }

        /// <summary>
        /// Создать каналы в базе конфигурации, используя ранее рассчитанные номера и прототипы каналов
        /// </summary>
        public static bool CreateChannels(List<KPInfo> kpInfoList, bool insertKPName, 
            string logFileName, out bool logCreated, out string msg)
        {
            logCreated = false;
            msg = "";
            StreamWriter writer = null;

            try
            {
                // создание журанала создания каналов
                writer = new StreamWriter(logFileName, false, Encoding.UTF8);
                logCreated = true;

                string title = DateTime.Now.ToString("G", Localization.Culture) + " " + AppPhrases.CreateCnlsTitle;
                writer.WriteLine(title);
                writer.WriteLine(new string('-', title.Length));
                writer.WriteLine();

                // формирование списков идентификаторов используемых значений из справочников базы конфигурации
                SortedList<string, int> paramList = new SortedList<string, int>();
                SortedList<string, int> unitList = new SortedList<string, int>();
                SortedList<string, int> cmdValList = new SortedList<string, int>();                

                foreach (KPInfo kpInfo in kpInfoList)
                {
                    if (kpInfo.DefaultCnls != null)
                    {
                        foreach (KPView.InCnlPrototype inCnl in kpInfo.DefaultCnls.InCnls)
                        {
                            string s = inCnl.ParamName;
                            if (!string.IsNullOrEmpty(s) && !paramList.ContainsKey(s))
                                paramList.Add(s, -1);

                            s = inCnl.UnitName;
                            if (!string.IsNullOrEmpty(s) && !unitList.ContainsKey(s))
                                unitList.Add(s, -1);
                        }

                        foreach (KPView.CtrlCnlPrototype ctrlCnl in kpInfo.DefaultCnls.CtrlCnls)
                        {
                            string s = ctrlCnl.CmdValName;
                            if (!string.IsNullOrEmpty(s) && !cmdValList.ContainsKey(s))
                                cmdValList.Add(s, -1);
                        }
                    }
                }

                // определение идентификаторов по справочникам базы конфигурации
                writer.WriteLine(AppPhrases.CheckDicts);
                bool paramError = !FindDictIDs(paramList, Tables.GetParamTable(), "ParamID", writer, 
                    AppPhrases.ParamNotFound);
                bool unitError = !FindDictIDs(unitList, Tables.GetUnitTable(), "UnitID", writer, 
                    AppPhrases.UnitNotFound);
                bool cmdValError = !FindDictIDs(cmdValList, Tables.GetCmdValTable(), "CmdValID", writer, 
                    AppPhrases.CmdValsNotFound);

                if (paramError || unitError || cmdValError)
                {
                    msg = AppPhrases.CreateCnlsImpossible;
                    writer.WriteLine(msg);
                    return false;
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
                    foreach (KPInfo kpInfo in kpInfoList)
                    {
                        if (kpInfo.Selected)
                        {
                            int inCnlNum = kpInfo.FirstInCnlNum;
                            int ctrlCnlNum = kpInfo.FirstCtrlCnlNum;
                            object objNum = kpInfo.ObjNum;
                            int kpNum = kpInfo.KPNum;
                            string kpNameToInsert = insertKPName ? kpInfo.KPName + " - " : "";

                            // создание каналов управления
                            foreach (KPView.CtrlCnlPrototype ctrlCnl in kpInfo.DefaultCnls.CtrlCnls)
                            {
                                ctrlCnl.CtrlCnlNum = ctrlCnlNum;
                                DataRow newCtrlCnlRow = CreateCtrlCnlRow(tblCtrlCnl, cmdValList, 
                                    ctrlCnl, objNum, kpNum, kpNameToInsert, writer);
                                tblCtrlCnl.Rows.Add(newCtrlCnlRow);
                                ctrlCnlNum++;
                            }

                            // создание входных каналов
                            foreach (KPView.InCnlPrototype inCnl in kpInfo.DefaultCnls.InCnls)
                            {
                                inCnl.CnlNum = inCnlNum;
                                DataRow newInCnlRow = CreateInCnlRow(tblInCnl, tblFormat, paramList, unitList,
                                    inCnl, objNum, kpNum, kpNameToInsert, writer);
                                tblInCnl.Rows.Add(newInCnlRow);
                                inCnlNum++;
                            }
                        }
                    }

                    // сохранение каналов в БД
                    int updRowCnt1, updRowCnt2;
                    bool updateOK = UpdateCnls(tblCtrlCnl, AppPhrases.AddedCtrlCnlsCount, writer, out updRowCnt1);
                    updateOK = UpdateCnls(tblInCnl, AppPhrases.AddedInCnlsCount, writer, out updRowCnt2) && updateOK;
                    msg = updateOK ? AppPhrases.CreateCnlsComplSucc : AppPhrases.CreateCnlsComplWithErr;
                    writer.WriteLine();
                    writer.WriteLine(msg);

                    if (updRowCnt1 + updRowCnt2 > 0)
                        msg += AppPhrases.RefreshRequired;

                    return updateOK;
                }
            }
            catch (Exception ex)
            {
                msg = AppPhrases.CreateCnlsError + ":\r\n" + ex.Message;
                try { writer.WriteLine(msg); }
                catch { }
                return false;
            }
            finally
            {
                try { writer.Close(); }
                catch { }
            }
        }
    }
}
