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
 * Summary  : Web application settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2016
 */

using Scada.Client;

namespace Scada.Web
{
    /// <summary>
    /// Web application settings
    /// <para>Настройки веб-приложения</para>
    /// </summary>
    public class WebSettings
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public WebSettings()
        {
            CommSettings = new CommSettings();
        }


        /// <summary>
        /// Настройки соединения с сервером
        /// </summary>
        public CommSettings CommSettings { get; protected set; }
    }
}
