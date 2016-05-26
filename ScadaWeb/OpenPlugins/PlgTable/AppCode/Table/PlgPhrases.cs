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
 * Module   : PlgTable
 * Summary  : The phrases used by the plugin
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web
{
    /// <summary>
    /// The phrases used by the plugin
    /// <para>Фразы, используемые плагином</para>
    /// </summary>
    public static class PlgPhrases
    {
        static PlgPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Web.Plugins.Table.EventsWndSpec
        public static string EventsTitle { get; private set; }

        // Словарь Scada.Web.Plugins.Table.TableView
        public static string LoadTableViewError { get; private set; }
        public static string SaveTableViewError { get; private set; }

        private static void SetToDefault()
        {
            EventsTitle = Localization.Dict.GetEmptyPhrase("EventsTitle");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.EventsWndSpec", out dict))
            {
                EventsTitle = dict.GetPhrase("EventsTitle", EventsTitle);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Table.TableView", out dict))
            {
                LoadTableViewError = dict.GetPhrase("LoadTableViewError", LoadTableViewError);
                SaveTableViewError = dict.GetPhrase("SaveTableViewError", SaveTableViewError);
            }
        }
    }
}
