using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Scheme
{
    public abstract class CompSpec
    {
        /// <summary>
        /// Получить иконку, отображаемую в редакторе
        /// </summary>
        public abstract object Icon { get; }

        /// <summary>
        /// Получить наименование, отображаемое в редакторе
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Получить тип компонента
        /// </summary>
        public abstract Type CompType { get; }
    }
}
