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
 * Module   : ScadaWebCommon
 * Summary  : The common phrases for schemes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Scheme
{
    /// <summary>
    /// The common phrases for schemes.
    /// <para>Общие фразы для схем.</para>
    /// </summary>
    public static class SchemePhrases
    {
        // Scada.Scheme.Model.PropertyGrid
        public static string StringConvertError { get; private set; }
        public static string StringUniqueError { get; private set; }
        public static string TrueValue { get; private set; }
        public static string FalseValue { get; private set; }
        public static string EmptyValue { get; private set; }
        public static string ObjectValue { get; private set; }
        public static string CollectionValue { get; private set; }
        public static string ComponentNotFound { get; private set; }
        public static string BoldSymbol { get; private set; }
        public static string ItalicSymbol { get; private set; }
        public static string UnderlineSymbol { get; private set; }

        // Scada.Scheme.Model.PropertyGrid.FrmImageDialog
        public static string ImageFileFilter { get; private set; }
        public static string DisplayImageError { get; private set; }
        public static string LoadImageError { get; private set; }
        public static string SaveImageError { get; private set; }

        // Scada.Scheme.Model.PropertyGrid.FrmRangeDialog
        public static string RangeNotValid { get; private set; }

        // Scada.Scheme.Template.TemplateBindings
        public static string LoadTemplateBindingsError { get; private set; }
        public static string SaveTemplateBindingsError { get; private set; }

        // Scada.Scheme.CompManager
        public static string UnknownComponent { get; private set; }
        public static string CompLibraryNotFound { get; private set; }
        public static string UnableCreateComponent { get; private set; }
        public static string ErrorCreatingComponent { get; private set; }

        // Scada.Scheme.SchemeView
        public static string LoadSchemeViewError { get; private set; }
        public static string SaveSchemeViewError { get; private set; }
        public static string IncorrectFileFormat { get; private set; }

        public static void Init()
        {
            Localization.Dict dict = Localization.GetDictionary("Scada.Scheme.Model.PropertyGrid");
            StringConvertError = dict.GetPhrase("StringConvertError");
            StringUniqueError = dict.GetPhrase("StringUniqueError");
            TrueValue = dict.GetPhrase("TrueValue");
            FalseValue = dict.GetPhrase("FalseValue");
            EmptyValue = dict.GetPhrase("EmptyValue");
            ObjectValue = dict.GetPhrase("ObjectValue");
            CollectionValue = dict.GetPhrase("CollectionValue");
            ComponentNotFound = dict.GetPhrase("ComponentNotFound");
            BoldSymbol = dict.GetPhrase("BoldSymbol");
            ItalicSymbol = dict.GetPhrase("ItalicSymbol");
            UnderlineSymbol = dict.GetPhrase("UnderlineSymbol");

            dict = Localization.GetDictionary("Scada.Scheme.Model.PropertyGrid.FrmImageDialog");
            ImageFileFilter = dict.GetPhrase("ImageFileFilter");
            DisplayImageError = dict.GetPhrase("DisplayImageError");
            LoadImageError = dict.GetPhrase("LoadImageError");
            SaveImageError = dict.GetPhrase("SaveImageError");

            dict = Localization.GetDictionary("Scada.Scheme.Model.PropertyGrid.FrmRangeDialog");
            RangeNotValid = dict.GetPhrase("RangeNotValid");

            dict = Localization.GetDictionary("Scada.Scheme.Template.TemplateBindings");
            LoadTemplateBindingsError = dict.GetPhrase("LoadTemplateBindingsError");
            SaveTemplateBindingsError = dict.GetPhrase("SaveTemplateBindingsError");

            dict = Localization.GetDictionary("Scada.Scheme.CompManager");
            UnknownComponent = dict.GetPhrase("UnknownComponent");
            CompLibraryNotFound = dict.GetPhrase("CompLibraryNotFound");
            UnableCreateComponent = dict.GetPhrase("UnableCreateComponent");
            ErrorCreatingComponent = dict.GetPhrase("ErrorCreatingComponent");

            dict = Localization.GetDictionary("Scada.Scheme.SchemeView");
            LoadSchemeViewError = dict.GetPhrase("LoadSchemeViewError");
            SaveSchemeViewError = dict.GetPhrase("SaveSchemeViewError");
            IncorrectFileFormat = dict.GetPhrase("IncorrectFileFormat");
        }
    }
}
