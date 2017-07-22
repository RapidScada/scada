using Scada.Server.Svc;

namespace FormulaTester
{
    public partial class CustomFormulas : CalcEngine
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
