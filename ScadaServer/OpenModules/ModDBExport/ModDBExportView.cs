/*
 * Модуль экспорта в БД
 * Пользовательский интерфейс серверного модуля
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

namespace Scada.Server.Module
{
    /// <summary>
    /// Пользовательский интерфейс серверного модуля
    /// </summary>
    public class ModDBExportView : ModView
    {
        /// <summary>
        /// Получить описание модуля
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ? "Экспорт в БД." : "Export to DB.";
            }
        }
    }
}
