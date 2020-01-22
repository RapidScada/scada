/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : Template Binding Editor
 * Summary  : The phrases used in the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Scheme.TemplateBindingEditor.Code
{
    /// <summary>
    /// The phrases used in the application.
    /// <para>Фразы, используемые приложением.</para>
    /// </summary>
    internal static class AppPhrases
    {
        // Scada.Scheme.TemplateBindingEditor.Code.SchemeParser
        public static string ErrorParsingScheme { get; private set; }

        // Scada.Scheme.TemplateBindingEditor.Forms.FrmMain
        public static string EditorTitle { get; private set; }
        public static string BindingsFileFilter { get; private set; }
        public static string SaveBindingsConfirm { get; private set; }
        public static string InterfaceDirNotFound { get; private set; }
        public static string UnableOpenTemplate { get; private set; }
        public static string UnableLoadTemplate { get; private set; }
        public static string TemplateNotFound { get; private set; }
        public static string TemplateLoaded { get; private set; }
        public static string WrongTemplatePath { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Scheme.TemplateBindingEditor.Code.SchemeParser");
            ErrorParsingScheme = dict.GetPhrase("ErrorParsingScheme");

            dict = Localization.GetDictionary("Scada.Scheme.TemplateBindingEditor.Forms.FrmMain");
            EditorTitle = dict.GetPhrase("EditorTitle");
            BindingsFileFilter = dict.GetPhrase("BindingsFileFilter");
            SaveBindingsConfirm = dict.GetPhrase("SaveBindingsConfirm");
            InterfaceDirNotFound = dict.GetPhrase("InterfaceDirNotFound");
            UnableLoadTemplate = dict.GetPhrase("UnableLoadTemplate");
            UnableOpenTemplate = dict.GetPhrase("UnableOpenTemplate");
            TemplateNotFound = dict.GetPhrase("TemplateNotFound");
            TemplateLoaded = dict.GetPhrase("TemplateLoaded");
            WrongTemplatePath = dict.GetPhrase("WrongTemplatePath");
        }
    }
}
