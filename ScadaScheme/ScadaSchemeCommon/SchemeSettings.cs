/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : ScadaSchemeCommon
 * Summary  : Silverlight application settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

namespace Scada.Scheme
{
    /// <summary>
    /// Silverlight application settings
    /// <para>Настройки Silverlight-приложения</para>
    /// </summary>
    public class SchemeSettings
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeSettings()
        {
            RefrFreq = 5;
            CmdEnabled = true;
            SchemePhrases = new SchemePhrases();
        }


        /// <summary>
        /// Получить или установить частоту обновления данных, с
        /// </summary>
        public int RefrFreq { get; set; }

        /// <summary>
        /// Получить или установить разрешение команд управления
        /// </summary>
        public bool CmdEnabled { get; set; }

        /// <summary>
        /// Получить или установить используемые фразы
        /// </summary>
        public SchemePhrases SchemePhrases { get; set; }
    }
}
