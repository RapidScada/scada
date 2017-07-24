/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Modified : 2017
 */

namespace FormulaTester
{
    /// <summary>
    /// Example of formulas for testing
    /// <para>Пример тестируемых формул</para>
    /// </summary>
    /// <remarks>
    /// Tip: Hold down the Alt key to make a rectangle multiple line selection.
    /// <para>Совет: Удерживайте клавишу Alt, чтобы выделить прямоугольную область из нескольких строк.</para>
    /// </remarks>
    public class CustomFormulas : FormulaTester
    {
        /// <summary>
        /// Linear transform the input channel value
        /// <para>Линейное преобразовать значение входного канала</para>
        /// </summary>
        public double LinearTransform(double a, double b)
        {
            return a * Cnl + b;
        }

        /// <summary>
        /// Calculates the sum of the values of the specified input channels
        /// <para>Вычислить сумму значений указанных входных каналов</para>
        /// </summary>
        public double CnlValSum(params int[] cnlNums)
        {
            double sum = 0.0;

            if (cnlNums != null)
            {
                foreach (int cnlNum in cnlNums)
                {
                    sum += Val(cnlNum);
                }
            }

            return sum;
        }
    }
}
