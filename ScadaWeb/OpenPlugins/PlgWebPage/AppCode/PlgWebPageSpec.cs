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
 * Module   : PlgWebPage
 * Summary  : Web page plugin specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using Scada.Web.Plugins.WebPage;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Web page plugin specification.
    /// <para>Спецификация плагина веб-страниц.</para>
    /// </summary>
    public class PlgWebPageSpec : PluginSpec
    {
        /// <summary>
        /// The plugin version.
        /// </summary>
        internal const string PlgVersion = "5.0.1.0";

        private DictUpdater dictUpdater; // updates the plugin dictionary


        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public override string Name
        {
            get
            {
                return Localization.UseRussian ?
                    "Веб-страницы" :
                    "Web pages";
            }
        }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Плагин обеспечивает отображение произвольных веб-страниц." :
                    "The plugin provides displaying arbitrary web pages.";
            }
        }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public override string Version
        {
            get
            {
                return PlgVersion;
            }
        }

        /// <summary>
        /// Get the view specifications that the plugin implements.
        /// </summary>
        public override List<ViewSpec> ViewSpecs
        {
            get
            {
                return new List<ViewSpec>() { new WebPageViewSpec() };
            }
        }

        
        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Init()
        {
            dictUpdater = new DictUpdater(
                string.Format("{0}WebPage{1}lang{1}", AppDirs.PluginsDir, Path.DirectorySeparatorChar),
                "PlgWebPage", null, Log);
        }

        /// <summary>
        /// Perform actions after a successful user login.
        /// </summary>
        public override void OnUserLogin(UserData userData)
        {
            dictUpdater.Update();
        }
    }
}
