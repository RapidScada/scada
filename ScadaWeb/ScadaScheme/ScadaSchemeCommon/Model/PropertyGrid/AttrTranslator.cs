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
 * Module   : ScadaSchemeCommon
 * Summary  : Type attributes translation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;
using System.Collections.Generic;
using CM = System.ComponentModel;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Type attributes translation
    /// <para>Перевод атрибутов типа</para>
    /// </summary>
    public class AttrTranslator
    {
        /// <summary>
        /// Атрибуты свойства
        /// </summary>
        private class PropAttrs
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PropAttrs()
            {
                DisplayName = "";
                Category = "";
                Description = "";
            }

            /// <summary>
            /// Получить или установить отображаемое имя
            /// </summary>
            public string DisplayName { get; set; }
            /// <summary>
            /// Получить или установить категорию
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// Получить или установить описание
            /// </summary>
            public string Description { get; set; }
        }


        /// <summary>
        /// Получить словарь атрибутов свойств
        /// </summary>
        private static Dictionary<string, PropAttrs> GetPropAttrsDict(Localization.Dict dict)
        {
            Dictionary<string, PropAttrs> propAttrsDict = new Dictionary<string, PropAttrs>();

            foreach (string phraseKey in dict.Phrases.Keys)
            {
                string phraseVal = dict.Phrases[phraseKey];
                int dotPos = phraseKey.IndexOf('.');

                if (0 < dotPos && dotPos < phraseKey.Length - 1)
                {
                    // если точка в середине ключа фразы, то слева от точки - имя свойства, справа - имя атрибута
                    string propName = phraseKey.Substring(0, dotPos);
                    string attrName = phraseKey.Substring(dotPos + 1);
                    bool attrAssigned = true;

                    PropAttrs propAttrs;
                    if (!propAttrsDict.TryGetValue(propName, out propAttrs))
                        propAttrs = new PropAttrs();

                    if (attrName == "DisplayName")
                    {
                        propAttrs.DisplayName = phraseVal;
                    }
                    else if (attrName == "Category")
                    {
                        propAttrs.Category = phraseVal;
                    }
                    else if (attrName == "Description")
                    {
                        propAttrs.Description = phraseVal;
                    }
                    else
                    {
                        attrAssigned = false;
                    }

                    if (attrAssigned)
                        propAttrsDict[propName] = propAttrs;
                }
            }

            return propAttrsDict;
        }

        /// <summary>
        /// Перевести атрибуты заданного типа
        /// </summary>
        /// <remarks>Используется словарь с ключём, совпадающим с полным именем типа</remarks>
        public void TranslateAttrs(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string dictKey = type.FullName;
            Localization.Dict dict;

            if (Localization.Dictionaries.TryGetValue(dictKey, out dict))
            {
                Dictionary<string, PropAttrs> propAttrsDict = GetPropAttrsDict(dict);
                CM.PropertyDescriptorCollection allProps = CM.TypeDescriptor.GetProperties(type);

                foreach (CM.PropertyDescriptor prop in allProps)
                {
                    PropAttrs propAttrs;

                    if (propAttrsDict.TryGetValue(prop.Name, out propAttrs))
                    {
                        CM.AttributeCollection attrs = prop.Attributes;

                        foreach (Attribute attr in attrs)
                        {
                            if (attr is DisplayNameAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.DisplayName))
                                    ((DisplayNameAttribute)attr).DisplayNameValue = propAttrs.DisplayName;
                            }
                            else if (attr is CategoryAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Category))
                                    ((CategoryAttribute)attr).CategoryName = propAttrs.Category;
                            }
                            else if (attr is DescriptionAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Description))
                                    ((DescriptionAttribute)attr).DescriptionValue = propAttrs.Description;
                            }
                        }
                    }
                }
            }
        }
    }
}
