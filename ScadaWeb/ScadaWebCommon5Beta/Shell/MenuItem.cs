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

 using System.Collections.Generic;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Menu item
    /// <para>Элемент меню</para>
    /// </summary>
    public class MenuItem
    {
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
        public MenuItem(string text, string url)
        {
            Text = text;
            Url = url;
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
        /// Получить подпункты меню
        /// </summary>
        public List<MenuItem> Subitems { get; protected set; }


        /// <summary>
        /// Создать элемент меню на основе стандартного элемента меню
        /// </summary>
        public static MenuItem FromStandardMenuItem(StandardMenuItems standardMenuItem)
        {
            switch (standardMenuItem)
            {
                case StandardMenuItems.Reports:
                    return new MenuItem(WebPhrases.ReportsMenuItem, "~/Reports.aspx");
                case StandardMenuItems.Admin:
                    return new MenuItem(WebPhrases.AdminMenuItem, "~/Admin.aspx");
                case StandardMenuItems.Config:
                    return new MenuItem(WebPhrases.ConfigMenuItem, "~/Config.aspx");
                default: // StandardMenuItem.About
                    return new MenuItem(WebPhrases.AboutMenuItem, "~/About.aspx");
            }
        }
    }
}
