/*
 * Библиотека слоёв связи
 * Родительский класс пользовательского интерфейса слоя связи
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// Родительский класс пользовательского интерфейса слоя связи
    /// </summary>
    public abstract class CommLayerView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommLayerView()
        {
            LangDir = "";
            CanShowProps = false;
        }


        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Получить описание слоя связи
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Получить или установить директорию языковых файлов
        /// </summary>
        public string LangDir { get; set; }

        /// <summary>
        /// Получить возможность отображения свойств слоя связи
        /// </summary>
        public bool CanShowProps { get; protected set; }


        /// <summary>
        /// Построить строку информации о свойствах слоя связи
        /// </summary>
        protected string BuildPropsInfo(Dictionary<string, string> layerParams, 
            string[] paramNames, object[] defParamVals)
        {
            // проверка параметров метода
            if (layerParams == null)
                throw new ArgumentNullException("layerParams");
            if (paramNames == null)
                throw new ArgumentNullException("paramNames");
            if (defParamVals == null)
                throw new ArgumentNullException("defParamVals");
            if (paramNames.Length != defParamVals.Length)
                throw new ArgumentException("Lengths of paramNames and defParamVals must be equal.");

            // формирование строки вида "Param1 = val1, Param2 = val2"
            StringBuilder sbPropsInfo = new StringBuilder();
            int len = paramNames.Length;
            int last = len - 1;

            for (int i = 0; i < len; i++)
            {
                string paramName = paramNames[i];
                string paramVal;
                sbPropsInfo.Append(paramName).Append(" = ")
                    .Append(layerParams.TryGetValue(paramName, out paramVal) ? paramVal : defParamVals[i])
                    .Append(i < last ? ", " : "");
            }

            return sbPropsInfo.ToString();
        }

        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public virtual void ShowProps(Dictionary<string, string> layerParams)
        {
        }
        
        /// <summary>
        /// Получить информацию о свойствах слоя связи
        /// </summary>
        public abstract string GetPropsInfo(Dictionary<string, string> layerParams);
    }
}
