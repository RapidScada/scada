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
 * Module   : ScadaWebCommon
 * Summary  : The common phrases for schemes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Scheme
{
    /// <summary>
    /// The common phrases for schemes
    /// <para>Общие фразы для схем</para>
    /// </summary>
    public static class SchemePhrases
    {
        static SchemePhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Scheme.CompManager
        public static string UnknownComponent { get; private set; }
        public static string CompLibraryNotFound { get; private set; }
        public static string UnableCreateComponent { get; private set; }
        public static string ErrorCreatingComponent { get; private set; }

        // Словарь Scada.Scheme.SchemeView
        public static string LoadSchemeViewError { get; private set; }
        public static string SaveSchemeViewError { get; private set; }
        public static string IncorrectFileFormat { get; private set; }

        // Словарь Scada.Scheme.Model.PropertyGrid
        public static string StringConvertError { get; private set; }
        public static string StringUniqueError { get; private set; }
        public static string TrueValue { get; private set; }
        public static string FalseValue { get; private set; }
        public static string EmptyValue { get; private set; }
        public static string CollectionValue { get; private set; }
        public static string BoldSymbol { get; private set; }
        public static string ItalicSymbol { get; private set; }
        public static string UnderlineSymbol { get; private set; }

        // Словарь Scada.Scheme.Model.PropertyGrid.FrmImageDialog
        public static string ImageFileFilter { get; private set; }
        public static string DisplayImageError { get; private set; }
        public static string LoadImageError { get; private set; }
        public static string SaveImageError { get; private set; }

        private static void SetToDefault()
        {
            UnknownComponent = Localization.Dict.GetEmptyPhrase("UnknownComponent");
            CompLibraryNotFound = Localization.Dict.GetEmptyPhrase("CompLibraryNotFound");
            UnableCreateComponent = Localization.Dict.GetEmptyPhrase("UnableCreateComponent");
            ErrorCreatingComponent = Localization.Dict.GetEmptyPhrase("ErrorCreatingComponent");

            LoadSchemeViewError = Localization.Dict.GetEmptyPhrase("LoadSchemeViewError");
            SaveSchemeViewError = Localization.Dict.GetEmptyPhrase("SaveSchemeViewError");
            IncorrectFileFormat = Localization.Dict.GetEmptyPhrase("IncorrectFileFormat");

            StringConvertError = Localization.Dict.GetEmptyPhrase("StringConvertError");
            StringUniqueError = Localization.Dict.GetEmptyPhrase("StringUniqueError");
            TrueValue = Localization.Dict.GetEmptyPhrase("TrueValue");
            FalseValue = Localization.Dict.GetEmptyPhrase("FalseValue");
            EmptyValue = Localization.Dict.GetEmptyPhrase("EmptyValue");
            CollectionValue = Localization.Dict.GetEmptyPhrase("CollectionValue");
            BoldSymbol = Localization.Dict.GetEmptyPhrase("BoldSymbol");
            ItalicSymbol = Localization.Dict.GetEmptyPhrase("ItalicSymbol");
            UnderlineSymbol = Localization.Dict.GetEmptyPhrase("UnderlineSymbol");

            ImageFileFilter = Localization.Dict.GetEmptyPhrase("ImageFileFilter");
            DisplayImageError = Localization.Dict.GetEmptyPhrase("DisplayImageError");
            LoadImageError = Localization.Dict.GetEmptyPhrase("LoadImageError");
            SaveImageError = Localization.Dict.GetEmptyPhrase("SaveImageError");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.CompManager", out dict))
            {
                UnknownComponent = dict.GetPhrase("UnknownComponent", UnknownComponent);
                CompLibraryNotFound = dict.GetPhrase("CompLibraryNotFound", CompLibraryNotFound);
                UnableCreateComponent = dict.GetPhrase("UnableCreateComponent", UnableCreateComponent);
                ErrorCreatingComponent = dict.GetPhrase("ErrorCreatingComponent", ErrorCreatingComponent);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.SchemeView", out dict))
            {
                LoadSchemeViewError = dict.GetPhrase("LoadSchemeViewError", LoadSchemeViewError);
                SaveSchemeViewError = dict.GetPhrase("SaveSchemeViewError", SaveSchemeViewError);
                IncorrectFileFormat = dict.GetPhrase("IncorrectFileFormat", IncorrectFileFormat);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Model.PropertyGrid", out dict))
            {
                StringConvertError = dict.GetPhrase("StringConvertError", StringConvertError);
                StringUniqueError = dict.GetPhrase("StringUniqueError", StringUniqueError);
                TrueValue = dict.GetPhrase("TrueValue", TrueValue);
                FalseValue = dict.GetPhrase("FalseValue", FalseValue);
                EmptyValue = dict.GetPhrase("EmptyValue", EmptyValue);
                CollectionValue = dict.GetPhrase("CollectionValue", CollectionValue);
                BoldSymbol = dict.GetPhrase("BoldSymbol", BoldSymbol);
                ItalicSymbol = dict.GetPhrase("ItalicSymbol", ItalicSymbol);
                UnderlineSymbol = dict.GetPhrase("UnderlineSymbol", UnderlineSymbol);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Scheme.Model.PropertyGrid.FrmImageDialog", out dict))
            {
                ImageFileFilter = dict.GetPhrase("ImageFileFilter", ImageFileFilter);
                DisplayImageError = dict.GetPhrase("DisplayImageError", DisplayImageError);
                LoadImageError = dict.GetPhrase("LoadImageError", LoadImageError);
                SaveImageError = dict.GetPhrase("SaveImageError", SaveImageError);
            }
        }
    }
}
