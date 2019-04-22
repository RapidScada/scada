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
 * Module   : Formula Tester
 * Summary  : The base class for testing formulas
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Data.Tables;
using Scada.Server.Engine;
using System.Collections.Generic;

namespace FormulaTester
{
    /// <summary>
    /// The base class for testing formulas.
    /// <para>Базовый класс для тестирования формул.</para>
    /// </summary>
    abstract public class FormulaTester : CalcEngine
    {
        /// <summary>
        /// Dictionary of input channel data.
        /// <para>Словарь данных входных каналов.</para>
        /// </summary>
        private Dictionary<int, SrezTableLight.CnlData> cnlDataDict;


        /// <summary>
        /// Initializes a new instance of the class.
        /// <para>Инициализирует новый экземпляр класса.</para>
        /// </summary>
        public FormulaTester()
            : base()
        {
            cnlDataDict = new Dictionary<int, SrezTableLight.CnlData>();
            getCnlData = GetCnlData;
            setCnlData = SetCnlData;
        }


        /// <summary>
        /// Get data of the specified input channel.
        /// <para>Получить данные заданного входного канала.</para>
        /// </summary>
        private SrezTableLight.CnlData GetCnlData(int cnlNum)
        {
            SrezTableLight.CnlData cnlData;
            return cnlDataDict.TryGetValue(cnlNum, out cnlData) ? cnlData : SrezTableLight.CnlData.Empty;
        }

        /// <summary>
        /// Set data of the specified input channel.
        /// <para>Установить данные заданного входного канала.</para>
        /// </summary>
        private void SetCnlData(int cnlNum, SrezTableLight.CnlData cnlData)
        {
            cnlDataDict[cnlNum] = cnlData;
        }


        /// <summary>
        /// Set the current input channel of the formula and its data.
        /// <para>Установить текущий входной канал формулы и его данные.</para>
        /// </summary>
        public void SetCurCnl(int cnlNum, SrezTableLight.CnlData cnlData)
        {
            BeginCalcCnlData(cnlNum, cnlData);
        }

        /// <summary>
        /// Set the current input channel of the formula and its data.
        /// <para>Установить текущий входной канал формулы и его данные.</para>
        /// </summary>
        public void SetCurCnl(int cnlNum, double val, int stat)
        {
            BeginCalcCnlData(cnlNum, new SrezTableLight.CnlData(val, stat));
        }

        /// <summary>
        /// Set the current output channel of the formula and the command value.
        /// <para>Установить текущий канал управления формулы и значение команды.</para>
        /// </summary>
        public void SetCurCmd(int ctrlCnlNum, double cmdVal)
        {
            BeginCalcCmdData(ctrlCnlNum, cmdVal, null);
        }

        /// <summary>
        /// Set the current output channel of the formula and the command data.
        /// <para>Установить текущий канал управления формулы и данные команды.</para>
        /// </summary>
        public void SetCurCmd(int ctrlCnlNum, byte[] cmdData)
        {
            BeginCalcCmdData(ctrlCnlNum, 0.0, cmdData);
        }
    }
}
