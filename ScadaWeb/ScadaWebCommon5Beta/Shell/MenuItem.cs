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
 * Module   : ScadaWebCommon
 * Summary  : Menu item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Generic;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Menu item
    /// <para>Элемент меню</para>
    /// </summary>
    public class MenuItem : IComparable<MenuItem>
    {
        /// <summary>
        /// Предзаданные порядки сортировки элементов меню
        /// </summary>
        public static class SortOrders
        {
            /// <summary>
            /// Первый
            /// </summary>
            public const int First = 0;
            /// <summary>
            /// Последний
            /// </summary>
            public const int Last = 1000;
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected MenuItem()
            : this("", "")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MenuItem(string text, string url, int sortOrder = SortOrders.First)
        {
            Text = text;
            Url = url;
            SortOrder = sortOrder;
            Subitems = new List<MenuItem>();
        }


        /// <summary>
        /// Получить текст
        /// </summary>
        public string Text { get; protected set; }

        /// <summary>
        /// Получить ссылку
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Получить порядок сортировки
        /// </summary>
        public int SortOrder { get; protected set; }

        /// <summary>
        /// Получить подпункты меню
        /// </summary>
        public List<MenuItem> Subitems { get; protected set; }


        /// <summary>
        /// Сравнить текущий объект с другим объектом такого же типа
        /// </summary>
        public int CompareTo(MenuItem other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                int comp1 = SortOrder.CompareTo(other.SortOrder);
                if (comp1 == 0)
                {
                    int comp2 = string.Compare(Text, other.Text, StringComparison.OrdinalIgnoreCase);
                    return comp2 == 0 ?
                        string.Compare(Url, other.Url, StringComparison.OrdinalIgnoreCase) :
                        comp2;
                }
                else
                {
                    return comp1;
                }
            }
        }

        /// <summary>
        /// Создать элемент меню на основе стандартного элемента меню
        /// </summary>
        public static MenuItem FromStandardMenuItem(StandardMenuItems standardMenuItem)
        {
            switch (standardMenuItem)
            {
                case StandardMenuItems.Reports:
                    return new MenuItem(WebPhrases.ReportsMenuItem, "~/Reports.aspx", 100);
                case StandardMenuItems.Config:
                    return new MenuItem(WebPhrases.ConfigMenuItem, "", 200);
                default: // StandardMenuItem.About
                    return new MenuItem(WebPhrases.AboutMenuItem, "~/About.aspx", SortOrders.Last);
            }
        }
    }
}
