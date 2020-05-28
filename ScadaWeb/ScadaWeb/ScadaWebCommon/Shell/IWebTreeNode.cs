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
 * Summary  : Interface that represents tree node to display on a web page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System.Collections;
using System.Collections.Generic;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Interface that represents tree node to display on a web page.
    /// <para>Интерфейс, представляющий узел дерева для отображения на веб-странице.</para>
    /// </summary>
    public interface IWebTreeNode
    {
        /// <summary>
        /// Получить текст
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Получить признак, что узел скрыт.
        /// </summary>
        bool Hidden { get; }

        /// <summary>
        /// Получить ссылку
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Получить скрипт
        /// </summary>
        /// <remarks>Скрипт имеет приоритет перед ссылкой, но наличие ссылки позволяет 
        /// открывать страницу в новом окне из стандартного контекстного меню</remarks>
        string Script { get; }

        /// <summary>
        /// Получить ссылку на иконку
        /// </summary>
        string IconUrl { get; }

        /// <summary>
        /// Получить или установить уровень вложенности
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Получить дочерние узлы
        /// </summary>
        IList Children { get; }

        /// <summary>
        /// Получить атрибуты данных в виде пар "имя-значение"
        /// </summary>
        SortedList<string, string> DataAttrs { get; }


        /// <summary>
        /// Определить, что узел соответствует выбранному объекту
        /// </summary>
        bool IsSelected(object selObj);
    }
}
