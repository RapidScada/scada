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
 * Module   : PlgConfig
 * Summary  : The phrases used by the plugin
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

#pragma warning disable 1591 // отключение warning CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Web.Plugins.Config
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

        // Словарь Scada.Web.Plugins.Config.WFrmPlugins
        public static string NameCol { get; private set; }
        public static string DescrCol { get; private set; }
        public static string StateCol { get; private set; }
        public static string ActivateBtn { get; private set; }
        public static string DeactivateBtn { get; private set; }
        public static string InactiveState { get; private set; }
        public static string ActiveState { get; private set; }
        public static string NotLoadedState { get; private set; }
        public static string PluginActivated { get; private set; }
        public static string PluginDeactivated { get; private set; }
        public static string PluginVersion { get; private set; }

        // Словарь Scada.Web.Plugins.Config.WFrmWebConfig
        public static string UnknownPlugin { get; private set; }
        public static string IncorrectFields { get; private set; }
        public static string ConfigSaved { get; private set; }

        // Словарь Scada.Web.Plugins.PlgConfigSpec
        public static string WebConfigMenuItem { get; private set; }
        public static string InstalledPluginsMenuItem { get; private set; }

        private static void SetToDefault()
        {
            NameCol = Localization.Dict.GetEmptyPhrase("NameCol");
            DescrCol = Localization.Dict.GetEmptyPhrase("DescrCol");
            StateCol = Localization.Dict.GetEmptyPhrase("StateCol");
            ActivateBtn = Localization.Dict.GetEmptyPhrase("ActivateBtn");
            DeactivateBtn = Localization.Dict.GetEmptyPhrase("DeactivateBtn");
            InactiveState = Localization.Dict.GetEmptyPhrase("InactiveState");
            ActiveState = Localization.Dict.GetEmptyPhrase("ActiveState");
            NotLoadedState = Localization.Dict.GetEmptyPhrase("NotLoadedState");
            PluginActivated = Localization.Dict.GetEmptyPhrase("PluginActivated");
            PluginDeactivated = Localization.Dict.GetEmptyPhrase("PluginDeactivated");
            PluginVersion = Localization.Dict.GetEmptyPhrase("PluginVersion");

            UnknownPlugin = Localization.Dict.GetEmptyPhrase("UnknownPlugin");
            IncorrectFields = Localization.Dict.GetEmptyPhrase("IncorrectFields");
            ConfigSaved = Localization.Dict.GetEmptyPhrase("ConfigSaved");

            WebConfigMenuItem = Localization.Dict.GetEmptyPhrase("WebConfigMenuItem");
            InstalledPluginsMenuItem = Localization.Dict.GetEmptyPhrase("InstalledPluginsMenuItem");
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Config.WFrmPlugins", out dict))
            {
                NameCol = dict.GetPhrase("NameCol", NameCol);
                DescrCol = dict.GetPhrase("DescrCol", DescrCol);
                StateCol = dict.GetPhrase("StateCol", StateCol);
                ActivateBtn = dict.GetPhrase("ActivateBtn", ActivateBtn);
                DeactivateBtn = dict.GetPhrase("DeactivateBtn", DeactivateBtn);
                InactiveState = dict.GetPhrase("InactiveState", InactiveState);
                ActiveState = dict.GetPhrase("ActiveState", ActiveState);
                NotLoadedState = dict.GetPhrase("NotLoadedState", NotLoadedState);
                PluginActivated = dict.GetPhrase("PluginActivated", PluginActivated);
                PluginDeactivated = dict.GetPhrase("PluginDeactivated", PluginDeactivated);
                PluginVersion = dict.GetPhrase("PluginVersion", PluginVersion);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.Config.WFrmWebConfig", out dict))
            {
                UnknownPlugin = dict.GetPhrase("UnknownPlugin", UnknownPlugin);
                IncorrectFields = dict.GetPhrase("IncorrectFields", IncorrectFields);
                ConfigSaved = dict.GetPhrase("ConfigSaved", ConfigSaved);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Web.Plugins.PlgConfigSpec", out dict))
            {
                WebConfigMenuItem = dict.GetPhrase("WebConfigMenuItem", WebConfigMenuItem);
                InstalledPluginsMenuItem = dict.GetPhrase("InstalledPluginsMenuItem", InstalledPluginsMenuItem);
            }
        }
    }
}
