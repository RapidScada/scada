using System.Reflection;

namespace Scada.Web.Plugins.SchBasicComp
{
    internal static class PlgUtils
    {
        /// <summary>
        /// Контролирует загрузку классов при клонировании компонентов
        /// </summary>
        public static readonly SerializationBinder SerializationBinder = 
            new SerializationBinder(Assembly.GetExecutingAssembly());
    }
}