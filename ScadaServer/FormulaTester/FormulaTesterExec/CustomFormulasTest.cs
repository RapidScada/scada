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
