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
 * Module   : Formula Tester Executor
 * Summary  : The class to run tests
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using FormulaTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scada.Data.Tables;

namespace FormulaTesterExec
{
    /// <summary>
    /// The class to run tests
    /// <para>Класс для запуска тестов</para>
    /// </summary>
    [TestClass]
    public class CustomFormulasTest
    {
        [TestMethod]
        public void LinearTransformTest()
        {
            CustomFormulas customFormulas = new CustomFormulas();
            customFormulas.SetCurCnl(101, new SrezTableLight.CnlData(100.0, 1));
            double actualResult = customFormulas.LinearTransform(5, 10);
            Assert.AreEqual(510, actualResult);
        }

        [TestMethod]
        public void CnlValSumTest()
        {
            CustomFormulas customFormulas = new CustomFormulas();
            customFormulas.SetVal(101, 1);
            customFormulas.SetVal(102, 2);
            customFormulas.SetVal(103, 3);
            customFormulas.SetVal(104, 4);
            customFormulas.SetVal(105, 5);
            double actualResult = customFormulas.CnlValSum(101, 102, 103, 104, 105);
            Assert.AreEqual(15, actualResult);
        }
    }
}
