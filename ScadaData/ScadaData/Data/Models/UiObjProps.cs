/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Modified : 2019
 */

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// User interface object properties.
    /// <para>Свойства объекта пользовательского интерфейса.</para>
    /// </summary>
    public class UiObjProps
    {
        /// <summary>
        /// Базовые типы объектов пользовательского интерфейса.
        /// </summary>
        [Flags]
        public enum BaseUiTypes
        {
            /// <summary>
            /// Представление (по умолчанию).
            /// </summary>
            View,
            /// <summary>
            /// Отчёт.
            /// </summary>
            Report,
            /// <summary>
            /// Окно данных.
            /// </summary>
            DataWnd
        }

        /// <summary>
        /// Виды пути
        /// </summary>
        public enum PathKinds
        {
            /// <summary>
            /// Не определён.
            /// </summary>
            Undefined,
            /// <summary>
            /// Файл.
            /// </summary>
            File,
            /// <summary>
            /// Ссылка.
            /// </summary>
            Url
        }

        /// <summary>
        /// The separator for a file path or title.
        /// </summary>
        public static readonly char[] PathSeparator = { '\\', '/' };


        /// <summary>
        /// Конструктор.
        /// </summary>
        public UiObjProps()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UiObjProps(int viewID)
        {
            UiObjID = viewID;
            Path = "";
            Args = "";
            TypeCode = "";
            Title = "";
            Hidden = false;
            ObjNum = 0;
            BaseUiType = BaseUiTypes.View;
        }


        /// <summary>
        /// Получить или установить идентификатор объекта пользовательского интерфейса.
        /// </summary>
        public int UiObjID { get; set; }

        /// <summary>
        /// Получить или установить путь.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Получить или установить дополнительные аргументы.
        /// </summary>
        public string Args { get; set; }

        /// <summary>
        /// Получить или установить код типа.
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// Получить или установить заголовок.
        /// </summary>
        /// <remarks>Заголовок может содержать полный путь в дереве представлений.</remarks>
        public string Title { get; set; }

        /// <summary>
        /// Получить или установить признак, что объект интерфейса скрыт.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Получить или установить номер объекта.
        /// </summary>
        public int ObjNum { get; set; }

        /// <summary>
        /// Получить или установить базовый тип.
        /// </summary>
        public BaseUiTypes BaseUiType { get; set; }

        /// <summary>
        /// Получить признак, что объект интерфейса пустой.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(Path) && string.IsNullOrEmpty(Title);
            }
        }

        /// <summary>
        /// Получить вид пути.
        /// </summary>
        public PathKinds PathKind
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                    return PathKinds.Undefined;
                else if (Path.Contains("://"))
                    return PathKinds.Url;
                else
                    return PathKinds.File;
            }
        }

        /// <summary>
        /// Получить короткий заголовок.
        /// </summary>
        public string ShortTitle
        {
            get
            {
                string title = Title ?? "";
                int idx = title.LastIndexOfAny(PathSeparator);
                return idx >= 0 ? title.Substring(idx + 1) : title;
            }
        }


        /// <summary>
        /// Определить базовый тип по коду типа объекта интерфейса.
        /// </summary>
        public static BaseUiTypes GetBaseUiType(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode))
                return BaseUiTypes.View;
            else if (typeCode.EndsWith("Rep", StringComparison.Ordinal))
                return BaseUiTypes.Report;
            else if (typeCode.EndsWith("Wnd", StringComparison.Ordinal))
                return BaseUiTypes.DataWnd;
            else
                return BaseUiTypes.View;
        }

        /// <summary>
        /// Извлечь путь, код типа и базовый тип объекта интерфейса из заданной строки.
        /// </summary>
        public static UiObjProps Parse(string s)
        {
            s = s ?? "";
            int sepInd = s.IndexOf('@');
            string path = (sepInd >= 0 ? s.Substring(0, sepInd) : s).Trim();
            string typeCode = sepInd >= 0 ? s.Substring(sepInd + 1).Trim() : "";

            if (typeCode == "")
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
                BaseUiType = GetBaseUiType(typeCode)
            };
        }
    }
}
