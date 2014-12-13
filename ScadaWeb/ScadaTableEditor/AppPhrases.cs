/*
 * SCADA-Редактор таблиц
 * Фразы, используемые приложением
 * 
 * Разработчик:
 * 2014, Ширяев Михаил
 */

using Scada;

namespace ScadaTableEditor
{
    /// <summary>
    /// Фразы, используемые приложением
    /// </summary>
    internal static class AppPhrases
    {
        static AppPhrases()
        {
            SetToDefault();
        }

        public static string FormTitle { get; private set; }
        public static string OpenFileFilter { get; private set; }
        public static string SaveFileFilter { get; private set; }
        public static string SaveConfirm { get; private set; }
        public static string AllObjItem { get; private set; }
        public static string AllKPItem { get; private set; }
        public static string LoadBaseError { get; private set; }
        public static string GetInCnlError { get; private set; }
        public static string GetCtrlCnlError { get; private set; }

        private static void SetToDefault()
        {
            FormTitle = "SCADA-Редактор таблиц";
            OpenFileFilter = "Табличные представления (*.tbl; *.ofm)|*.tbl;*.ofm|Все файлы (*.*)|*.*";
            SaveFileFilter = "Табличные представления (*.tbl)|*.tbl|Все файлы (*.*)|*.*";
            SaveConfirm = "Табличное представление было изменено. Сохранить изменения?";
            AllObjItem = "<Все объекты>";
            AllKPItem = "<Все КП>";
            LoadBaseError = "Ошибка при загрузке данных базы конфигурации";
            GetInCnlError = "Ошибка при получении информации о входном канале";
            GetCtrlCnlError = "Ошибка при получении информации о канале управления";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("ScadaTableEditor.FrmMain", out dict))
            {
                FormTitle = dict.GetPhrase("this", FormTitle);
                OpenFileFilter = dict.GetPhrase("OpenFileFilter", OpenFileFilter);
                SaveFileFilter = dict.GetPhrase("SaveFileFilter", SaveFileFilter);
                SaveConfirm = dict.GetPhrase("SaveConfirm", SaveConfirm);
                AllObjItem = dict.GetPhrase("AllObjItem", AllObjItem);
                AllKPItem = dict.GetPhrase("AllKPItem", AllKPItem);
                LoadBaseError = dict.GetPhrase("LoadBaseError", LoadBaseError);
            }

            if (Localization.Dictionaries.TryGetValue("ScadaTableEditor.FrmItemInfo", out dict))
            {
                GetInCnlError = dict.GetPhrase("GetInCnlError", GetInCnlError);
                GetCtrlCnlError = dict.GetPhrase("GetCtrlCnlError", GetCtrlCnlError);
            }
        }
    }
}
