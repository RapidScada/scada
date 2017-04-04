/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Modified : 2017
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
        public static string MainFormTitle { get; private set; }
        public static string CloseSecondInstance { get; private set; }
        public static string FailedToStartEditor { get; private set; }

        private static void SetToDefault()
        {
            MainFormTitle = Localization.Dict.GetEmptyPhrase("MainFormTitle");
            CloseSecondInstance = Localization.Dict.GetEmptyPhrase("CloseSecondInstance");
            FailedToStartEditor = Localization.Dict.GetEmptyPhrase("FailedToStartEditor");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Editor.FrmMain", out dict))
            {
                MainFormTitle = dict.GetPhrase("this", MainFormTitle);
                CloseSecondInstance = dict.GetPhrase("CloseSecondInstance", CloseSecondInstance);
                FailedToStartEditor = dict.GetPhrase("FailedToStartEditor", FailedToStartEditor);
            }
        }
    }
}
