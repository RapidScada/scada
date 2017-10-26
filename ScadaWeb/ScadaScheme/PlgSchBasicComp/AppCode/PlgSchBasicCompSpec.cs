using Scada.Scheme;
using System;
using System.Collections.Generic;
using System.Web;

namespace Scada.Web.Plugins
{
    public class PlgSchBasicCompSpec : PluginSpec, ISchemeComp
    {
        /// <summary>
        /// Версия плагина
        /// </summary>
        internal const string PlgVersion = "5.0.0.0";


        /// <summary>
        /// Получить наименование плагина
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Основные компоненты схем" :
                    "Basic scheme components";
            }
        }

        /// <summary>
        /// Получить описание плагина
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Набор основных компонентов для отображения на мнемосхемах." :
                    "A set of basic components for display on schemes.";
            }
        }

        /// <summary>
        /// Получить версию плагина
        /// </summary>
        public override string Version
        {
            get
            {
                return PlgVersion;
            }
        }
    }
}