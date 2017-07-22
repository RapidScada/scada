using FormulaTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormulaTesterExec
{
    [TestClass]
    public class ExampleTest
    {
        [TestMethod]
        public void LinearTransformTest()
        {
            CustomFormulas customFormulas = new CustomFormulas();
            double actualResult = customFormulas.LinearTransform(5, 10);
            Assert.AreEqual(10, actualResult);
        }

        [TestMethod]
        public void CnlValSumTest()
        {
            CustomFormulas customFormulas = new CustomFormulas();
            double actualResult = customFormulas.CnlValSum(102, 102, 103, 104, 105);
            Assert.AreEqual(15, actualResult);
        }
    }
}
