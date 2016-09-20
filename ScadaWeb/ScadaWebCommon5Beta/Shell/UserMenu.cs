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
 * Summary  : Main menu accessible to the web application user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Plugins;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Main menu accessible to the web application user
    /// <para>Главное меню, доступное пользователю веб-приложения</para>
    /// </summary>
    public class UserMenu
    {
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;

        
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected UserMenu()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public UserMenu(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;
            MenuItems = new List<MenuItem>();
        }


        /// <summary>
        /// Получить элементы меню, доступные пользователю
        /// </summary>
        public List<MenuItem> MenuItems { get; protected set; }


        /// <summary>
        /// Рекурсивно слить элементы меню
        /// </summary>
        protected void MergeMenuItems(List<MenuItem> existingItems, List<MenuItem> addedItems, int level)
        {
            if (addedItems != null)
            {
                addedItems.Sort();

                foreach (MenuItem addedItem in addedItems)
                {
                    addedItem.Level = level;
                    int ind = existingItems.BinarySearch(addedItem);

                    if (ind >= 0)
                    {
                        MenuItem existingItem = existingItems[ind];

                        if (existingItem.Subitems.Count > 0 && addedItem.Subitems.Count > 0)
                        {
                            // рекурсивное добавление подпунктов меню
                            MergeMenuItems(existingItem.Subitems, addedItem.Subitems, level + 1);
                        }
                        else
                        {
                            // упрощённое добавление подпунктов меню
                            addedItem.Subitems.Sort();
                            existingItem.Subitems.AddRange(addedItem.Subitems);
                            SetMenuItemLevels(addedItem.Subitems, level + 1);
                        }
                    }
                    else
                    {
                        // вставка элемента вместе с его дочерними элементами
                        addedItem.Subitems.Sort();
                        existingItems.Insert(~ind, addedItem);
                        SetMenuItemLevels(addedItem.Subitems, level + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Рекурсивно установить уровень вложенности элементов меню
        /// </summary>
        protected void SetMenuItemLevels(List<MenuItem> items, int level)
        {
            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    item.Level = level;
                    SetMenuItemLevels(item.Subitems, level + 1);
                }
            }
        }


        /// <summary>
        /// Инициализировать меню пользователя
        /// </summary>
        public void Init(UserData userData)
        {
            if (userData == null)
                throw new ArgumentNullException("userData");

            try
            {
                // очистка меню
                MenuItems.Clear();

                // слияние меню плагинов
                if (userData.PluginSpecs != null)
                {
                    foreach (PluginSpec pluginSpec in userData.PluginSpecs)
                        MergeMenuItems(MenuItems, pluginSpec.GetMenuItems(userData), 0);
                }

                // добавление пунктов меню по умолчанию
                List<MenuItem> defaultItems = new List<MenuItem>()
                {
                    MenuItem.FromStandardMenuItem(StandardMenuItems.Reports),
                    MenuItem.FromStandardMenuItem(StandardMenuItems.About)
                };
                MergeMenuItems(MenuItems, defaultItems, 0);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при инициализации меню пользователя" :
                    "Error initializing user menu");
            }
        }
    }
}
