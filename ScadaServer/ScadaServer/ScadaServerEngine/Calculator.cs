/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : SCADA-Server Service
 * Summary  : Channel calculator 
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2020
 */

using Microsoft.CSharp;
using Scada.Data.Tables;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Utils;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Channel calculator 
    /// <para>Калькулятор каналов</para>
    /// </summary>
    sealed class Calculator
    {
        /// <summary>
        /// Делегат вычисления данных входного канала
        /// </summary>
        public delegate void CalcCnlDataDelegate(ref SrezTableLight.CnlData cnlData);
        /// <summary>
        /// Делегат вычисления значения стандартной команды
        /// </summary>
        public delegate void CalcCmdValDelegate(ref double cmdVal);
        /// <summary>
        /// Делегат вычисления данных бинарной команды
        /// </summary>
        public delegate void CalcCmdDataDelegate(ref byte[] cmdData);

        private MainLogic mainLogic;         // ссылка на объект основной логики приложения
        private Log appLog;                  // журнал приложения
        private List<string> exprList;       // список выражений для компилирования
        private object calcEngine;           // механизм, обеспечивающий работу калькулятора


        /// <summary>
        /// Конструктор
        /// </summary>
        private Calculator()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Calculator(MainLogic mainLogic)
        {
            if (mainLogic == null)
                throw new ArgumentNullException("mainLogic");
            if (mainLogic.AppLog == null)
                throw new ArgumentNullException("mainLogic.AppLog");

            this.mainLogic = mainLogic;
            appLog = mainLogic.AppLog;
            exprList = new List<string>();
            calcEngine = null;
        }


        /// <summary>
        /// Добавить исходный код формулы входного канала
        /// </summary>
        /// <remarks>
        /// Требуется одно или два выражения на языке C#, разделённые точкой с запятой,
        /// для вычисления значения и статуса канала соответственно</remarks>
        public void AddCnlFormulaSource(int cnlNum, string source)
        {
            if (cnlNum > 0)
            {
                string[] parts = string.IsNullOrEmpty(source) ? new string[0] : source.Split(';');
                string part0 = parts.Length < 1 ? "" : parts[0].Trim();
                string part1 = parts.Length < 2 ? "" : parts[1].Trim();

                string calcCnlValName = "CalcCnl" + cnlNum + "Val";
                string calcCnlValExpr = part0 == "" ? "CnlVal" : "Convert.ToDouble(" + part0 + ")";
                string calcCnlValSrc = string.Format("public double {0}() {{ return {1}; }}", 
                    calcCnlValName, calcCnlValExpr);
                exprList.Add(calcCnlValSrc);

                string calcCnlStatName = "CalcCnl" + cnlNum + "Stat";
                string calcCnlStatExpr = part1 == "" ? "CnlStat" : "Convert.ToInt32(" + part1 + ")";
                string calcCnlStatSrc = string.Format("public int {0}() {{ return {1}; }}", 
                    calcCnlStatName, calcCnlStatExpr);
                exprList.Add(calcCnlStatSrc);

                string calcCnlDataSrc = string.Format("public void CalcCnl{0}" + 
                    "Data(ref SrezTableLight.CnlData cnlData) {{ try {{ BeginCalcCnlData({0}, cnlData); " + 
                    "cnlData = new SrezTableLight.CnlData({1}(), {2}()); }} finally {{ EndCalcCnlData(); }}}}",
                    cnlNum, calcCnlValName, calcCnlStatName);
                exprList.Add(calcCnlDataSrc);
            }
        }

        /// <summary>
        /// Добавить исходный код формулы канала управления для стандартной команды
        /// </summary>
        public void AddCtrlCnlStandardFormulaSource(int ctrlCnlNum, string source)
        {
            if (ctrlCnlNum > 0)
            {
                string calcCmdValExpr = string.IsNullOrEmpty(source) ? "CmdVal" : 
                    string.Format("Convert.ToDouble({0})", source.Trim());
                exprList.Add(string.Format(
                    "public void CalcCmdVal{0}(ref double cmdVal) " +
                    "{{ try {{ BeginCalcCmdData({0}, cmdVal, null); cmdVal = {1}; }} " + 
                    "finally {{ EndCalcCmdData(); }}}}", ctrlCnlNum, calcCmdValExpr));
            }
        }

        /// <summary>
        /// Добавить исходный код формулы канала управления для бинарной команды
        /// </summary>
        public void AddCtrlCnlBinaryFormulaSource(int ctrlCnlNum, string source)
        {
            if (ctrlCnlNum > 0)
            {
                string calcCmdDataExpr = string.IsNullOrEmpty(source) ? "CmdData" : source.Trim();
                exprList.Add(string.Format(
                    "public void CalcCmdData{0}(ref byte[] cmdData) " +
                    "{{ try {{ BeginCalcCmdData({0}, 0.0, cmdData); cmdData = {1}; }} " + 
                    "finally {{ EndCalcCmdData(); }}}}", ctrlCnlNum, calcCmdDataExpr));
            }
        }

        /// <summary>
        /// Добавить исходный код вспомогательной формулы
        /// </summary>
        /// <remarks>Требуется исходный код члена класса на языке C#</remarks>
        public void AddAuxFormulaSource(string source)
        {
            source = source == null ? "" : source.Trim();
            if (source != "")
                exprList.Add(source);
        }

        /// <summary>
        /// Компилировать исходный код формул калькулятора
        /// </summary>
        public bool CompileSource()
        {
            try
            {
                // загрузка исходного кода класса CalcEngine
                string source;

                using (Stream stream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("Scada.Server.Engine.CalcEngine.cs"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        source = reader.ReadToEnd();
                    }
                }

                // добавление членов класса CalcEngine
                const string InsertMarker = "/*TODO*/";
                int insertInd = source.IndexOf(InsertMarker);

                if (insertInd >= 0)
                {
                    StringBuilder sourceSB = new StringBuilder(source.Substring(0, insertInd));

                    for (int i = 0, cnt = exprList.Count; i < cnt; i++)
                    {
                        sourceSB.AppendLine(exprList[i]);
                    }

                    sourceSB.Append(source.Substring(insertInd + InsertMarker.Length));
                    source = sourceSB.ToString();
                }

                // сохранение исходного кода класса CalcEngine в файле для анализа
                string sourceFileName = mainLogic.AppDirs.LogDir + "CalcEngine.cs";
                File.WriteAllText(sourceFileName, source, Encoding.UTF8);

                // компилирование исходного кода класса CalcEngine
                CompilerParameters compParams = new CompilerParameters();
                compParams.GenerateExecutable = false;
                compParams.GenerateInMemory = true;
                compParams.IncludeDebugInformation = false;
                compParams.ReferencedAssemblies.Add("System.dll");
                compParams.ReferencedAssemblies.Add("System.Core.dll");
                compParams.ReferencedAssemblies.Add(mainLogic.AppDirs.ExeDir + "ScadaData.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults compilerResults = compiler.CompileAssemblyFromSource(compParams, source);

                if (compilerResults.Errors.HasErrors)
                {
                    appLog.WriteAction(Localization.UseRussian ? 
                        "Ошибка при компилировании исходного кода формул: " :
                        "Error compiling the source code of the formulas: ", Log.ActTypes.Error);

                    foreach (CompilerError error in compilerResults.Errors)
                        appLog.WriteLine(string.Format(Localization.UseRussian ? 
                            "Строка {0}, колонка {1}: error {2}: {3}" : 
                            "Line {0}, column {1}: error {2}: {3}", 
                            error.Line, error.Column, error.ErrorNumber, error.ErrorText));

                    appLog.WriteLine(string.Format(Localization.UseRussian ? 
                        "Для ознакомления с исходным кодом см. файл {0}" : 
                        "See the file {0} with the source code", 
                        sourceFileName));
                    return false;
                }
                else
                {
                    Type calcEngineType = compilerResults.CompiledAssembly.GetType(
                        "Scada.Server.Engine.CalcEngine", true);
                    calcEngine = Activator.CreateInstance(calcEngineType, 
                        new Func<int, SrezTableLight.CnlData>(mainLogic.GetProcSrezCnlData),
                        new Action<int, SrezTableLight.CnlData>(mainLogic.SetProcSrezCnlData));

                    appLog.WriteAction(Localization.UseRussian ? 
                        "Исходный код формул калькулятора откомпилирован" :
                        "The formulas source code has been compiled", Log.ActTypes.Action);
                    return true;
                }
            }
            catch (Exception ex)
            {
                appLog.WriteAction((Localization.UseRussian ? 
                    "Ошибка при компилировании исходного кода формул: " :
                    "Error compiling the source code of the formulas: ") + ex.Message, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Получить метод вычисления данных входного канала
        /// </summary>
        public CalcCnlDataDelegate GetCalcCnlData(int cnlNum)
        {
            try
            {
                return (CalcCnlDataDelegate)Delegate.CreateDelegate(typeof(CalcCnlDataDelegate), 
                    calcEngine, "CalcCnl" + cnlNum + "Data", false, true);
            }
            catch (Exception ex)
            {
                appLog.WriteAction(string.Format( Localization.UseRussian ? 
                    "Ошибка при получении метода вычисления данных для входного канала {0}: {1}" :
                    "Error getting calculation data method for the input channel {0}: {1}", 
                    cnlNum, ex.Message), Log.ActTypes.Exception);
                return null;
            }
        }

        /// <summary>
        /// Получить метод вычисления значения стандартной команды
        /// </summary>
        public CalcCmdValDelegate GetCalcCmdVal(int ctrlCnlNum)
        {
            try
            {
                return (CalcCmdValDelegate)Delegate.CreateDelegate(typeof(CalcCmdValDelegate),
                    calcEngine, "CalcCmdVal" + ctrlCnlNum, false, true);
            }
            catch (Exception ex)
            {
                appLog.WriteAction(string.Format(Localization.UseRussian ? 
                    "Ошибка при получении метода вычисления значения команды для канала управления {0}: {1}" :
                    "Error getting calculation commmand value method for the output channel {0}: {1}", 
                    ctrlCnlNum, ex.Message), Log.ActTypes.Exception);
                return null;
            }
        }

        /// <summary>
        /// Получить метод вычисления данных бинарной команды
        /// </summary>
        public CalcCmdDataDelegate GetCalcCmdData(int ctrlCnlNum)
        {
            try
            {
                return (CalcCmdDataDelegate)Delegate.CreateDelegate(typeof(CalcCmdDataDelegate),
                    calcEngine, "CalcCmdData" + ctrlCnlNum, false, true);
            }
            catch (Exception ex)
            {
                appLog.WriteAction(string.Format(Localization.UseRussian ?
                    "Ошибка при получении метода вычисления данных команды для канала управления {0}: {1}" :
                    "Error getting calculation commmand data method for the output channel {0}: {1}",
                    ctrlCnlNum, ex.Message), Log.ActTypes.Exception);
                return null;
            }
        }

        /// <summary>
        /// Очистить формулы
        /// </summary>
        public void ClearFormulas()
        {
            exprList.Clear();
            calcEngine = null;
        }
    }
}