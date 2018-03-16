/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : Scheme Editor
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// The phrases used in the application
    /// <para>Фразы, используемые приложением</para>
    /// </summary>
    internal static class AppPhrases
    {
        static AppPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Scheme.Editor.FrmMain
        public static string CloseSecondInstance { get; private set; }
        public static string FailedToStartEditor { get; private set; }
        public static string OpenBrowserError { get; private set; }
        public static string PointerItem { get; private set; }
        public static string SchemeFileFilter { get; private set; }
        public static string SaveSchemeConfirm { get; private set; }
        public static string RestartNeeded { get; private set; }

        // Словарь Scada.Scheme.Editor.FrmSettings
        public static string WebDirNotExists { get; private set; }
        public static string ChooseWebDir { get; private set; }

        // Словарь Scada.Scheme.Editor.Editor
        public static string EditorTitle { get; private set; }

        // Словарь Scada.Scheme.Editor.FormState
        public static string LoadFormStateError { get; private set; }
        public static string SaveFormStateError { get; private set; }

        private static void SetToDefault()
        {
            CloseSecondInstance = Localization.Dict.GetEmptyPhrase("CloseSecondInstance");
            FailedToStartEditor = Localization.Dict.GetEmptyPhrase("FailedToStartEditor");
            OpenBrowserError = Localization.Dict.GetEmptyPhrase("OpenBrowserError");
            PointerItem = Localization.Dict.GetEmptyPhrase("PointerItem");
            SchemeFileFilter = Localization.Dict.GetEmptyPhrase("SchemeFileFilter");
            SaveSchemeConfirm = Localization.Dict.GetEmptyPhrase("SaveSchemeConfirm");
            RestartNeeded = Localization.Dict.GetEmptyPhrase("RestartNeeded");

            WebDirNotExists = Localization.Dict.GetEmptyPhrase("WebDirNotExists");
            ChooseWebDir = Localization.Dict.GetEmptyPhrase("ChooseWebDir");

            EditorTitle = Localization.Dict.GetEmptyPhrase("EditorTitle");

            LoadFormStateError = Localization.Dict.GetEmptyPhrase("LoadFormStateError");
            SaveFormStateError = Localization.Dict.GetEmptyPhrase("SaveFormStateError");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.FrmMain", out dict))
            {
                CloseSecondInstance = dict.GetPhrase("CloseSecondInstance", CloseSecondInstance);
                FailedToStartEditor = dict.GetPhrase("FailedToStartEditor", FailedToStartEditor);
                OpenBrowserError = dict.GetPhrase("OpenBrowserError", OpenBrowserError);
                PointerItem = dict.GetPhrase("PointerItem", PointerItem);
                SchemeFileFilter = dict.GetPhrase("SchemeFileFilter", SchemeFileFilter);
                SaveSchemeConfirm = dict.GetPhrase("SaveSchemeConfirm", SaveSchemeConfirm);
                RestartNeeded = dict.GetPhrase("RestartNeeded", RestartNeeded);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.FrmSettings", out dict))
            {
                WebDirNotExists = dict.GetPhrase("WebDirNotExists", WebDirNotExists);
                ChooseWebDir = dict.GetPhrase("ChooseWebDir", ChooseWebDir);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.Editor", out dict))
            {
                EditorTitle = dict.GetPhrase("EditorTitle", EditorTitle);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.FormState", out dict))
            {
                LoadFormStateError = dict.GetPhrase("LoadFormStateError", LoadFormStateError);
                SaveFormStateError = dict.GetPhrase("SaveFormStateError", SaveFormStateError);
            }
        }
    }
}
