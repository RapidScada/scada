/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Summary  : Paths to the additional scripts those implement the shell functionality
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Paths to the additional scripts those implement the shell functionality
    /// <para>Пути к дополнительным скриптам, реализующим функциональность оболочки</para>
    /// </summary>
    public class ScriptPaths
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ScriptPaths()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить путь к скрипту отображения графика
        /// </summary>
        public string ChartScriptPath { get; set; }

        /// <summary>
        /// Получить или установить путь к скрипту отправки команды
        /// </summary>
        public string CmdScriptPath { get; set; }

        /// <summary>
        /// Получить или установить путь к скрипту квитирования события
        /// </summary>
        public string EventAckScriptPath { get; set; }

        /// <summary>
        /// Gets or sets the user profile URL template.
        /// </summary>
        public string UserProfilePath { get; set; }

        /// <summary>
        /// Получить или установить пути к дополнительным скриптам
        /// </summary>
        public ICollection<string> ExtraScripts { get; set; }

        
        /// <summary>
        /// Установить значения по умолчанию
        /// </summary>
        public void SetToDefault()
        {
            ChartScriptPath = "";
            CmdScriptPath = "";
            EventAckScriptPath = "";
            UserProfilePath = "";
            ExtraScripts = null;
        }

        /// <summary>
        /// Генерировать HTML-код для добавления скриптов на веб-страницу
        /// </summary>
        public string GenerateHtml()
        {
            StringBuilder sbHtml = new StringBuilder();

            if (!string.IsNullOrEmpty(ChartScriptPath))
                sbHtml.AppendLine(string.Format(WebUtils.ScriptTemplate, VirtualPathUtility.ToAbsolute(ChartScriptPath)));

            if (!string.IsNullOrEmpty(CmdScriptPath))
                sbHtml.AppendLine(string.Format(WebUtils.ScriptTemplate, VirtualPathUtility.ToAbsolute(CmdScriptPath)));

            if (!string.IsNullOrEmpty(EventAckScriptPath))
                sbHtml.AppendLine(string.Format(WebUtils.ScriptTemplate, VirtualPathUtility.ToAbsolute(EventAckScriptPath)));

            if (ExtraScripts != null)
            {
                foreach (string s in ExtraScripts)
                {
                    sbHtml.AppendLine(string.Format(WebUtils.ScriptTemplate, VirtualPathUtility.ToAbsolute(s)));
                }
            }

            return sbHtml.ToString();
        }
    }
}
