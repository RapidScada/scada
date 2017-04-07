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
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;

namespace Scada.Scheme.Editor.AppCode
{
    /// <summary>
    /// Type attributes translation
    /// <para>Перевод атрибутов типа</para>
    /// </summary>
    internal class AttrTranslator
    {
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

            }
        }
    }
}
