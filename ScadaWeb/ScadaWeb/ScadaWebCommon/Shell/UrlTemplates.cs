/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Summary  : URL templates of the web application pages
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2018
 */

namespace Scada.Web.Shell
{
    /// <summary>
    /// URL templates of the web application pages
    /// <para>Шаблоны адресов страниц веб-приложения</para>
    /// </summary>
    public static class UrlTemplates
    {
        /// <summary>
        /// Вход в систему с указанием ссылки для возврата
        /// </summary>
        public const string LoginWithReturn = "~/Login.aspx?return={0}";

        /// <summary>
        /// Вход в систему с указанием ссылки для возврата и выводом сообщения
        /// </summary>
        public const string LoginWithAlert = "~/Login.aspx?return={0}&alert={1}";
        
        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public const string User = "~/User.aspx?userID={0}";

        /// <summary>
        /// Представление
        /// </summary>
        public const string View = "~/View.aspx?viewID={0}";

        /// <summary>
        /// Отсутствующее представление
        /// </summary>
        public const string NoView = "~/NoView.aspx";

        /// <summary>
        /// Сайт сбора статистики
        /// </summary>
        public const string Stats = "stats.rapidscada.net?serverID={0}";
    }
}
