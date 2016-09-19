/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : User interface object properties
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// User interface object properties
    /// <para>Свойства объекта пользовательского интерфейса</para>
    /// </summary>
    public class UiObjProps
    {
        /// <summary>
        /// Базовые типы объектов пользовательского интерфейса
        /// </summary>
        [Flags]
        public enum BaseUiTypes
        {
            /// <summary>
            /// Представление (по умолчанию)
            /// </summary>
            View,
            /// <summary>
            /// Отчёт
            /// </summary>
            Report,
            /// <summary>
            /// Окно данных
            /// </summary>
            DataWnd
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public UiObjProps()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UiObjProps(int viewID)
        {
            UiObjID = viewID;
            Title = "";
            Path = "";
            TypeCode = "";
            BaseUiType = BaseUiTypes.View;
        }


        /// <summary>
        /// Получить или установить идентификатор объекта пользовательского интерфейса
        /// </summary>
        public int UiObjID { get; set; }

        /// <summary>
        /// Получить или установить заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получить или установить путь
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Получить или установить код типа
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// Получить или установить базовый тип
        /// </summary>
        public BaseUiTypes BaseUiType { get; set; }


        /// <summary>
        /// Извлечь путь, код типа и базовый тип объекта интерфейса из заданной строки
        /// </summary>
        public static UiObjProps Parse(string s)
        {
            s = s ?? "";
            int sepInd = s.IndexOf('@');
            string path = (sepInd >= 0 ? s.Substring(0, sepInd) : s).Trim();
            string typeCode = sepInd >= 0 ? s.Substring(sepInd).Trim() : "";
            BaseUiTypes baseUiType = BaseUiTypes.View;

            if (typeCode.EndsWith("Report", StringComparison.Ordinal))
            {
                baseUiType = BaseUiTypes.Report;
            }
            else if (typeCode.EndsWith("DataWnd", StringComparison.Ordinal))
            {
                baseUiType = BaseUiTypes.DataWnd;
            }
            else if (typeCode == "")
            {
                if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    typeCode = "WebPageView";
                }
                else
                {
                    string ext = System.IO.Path.GetExtension(path);
                    typeCode = ext == null ? "" : ext.TrimStart('.');
                }
            }

            return new UiObjProps()
            {
                Path = path,
                TypeCode = typeCode,
                BaseUiType = baseUiType
            };
        }
    }
}
