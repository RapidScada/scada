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
 * Summary  : The class contains utility methods for web applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Web;

namespace Scada.Web
{
    /// <summary>
    /// The class contains utility methods for web applications
    /// <para>Класс, содержащий вспомогательные методы для веб-приложений</para>
    /// </summary>
    public static class ScadaWebUtils
    {
        /// <summary>
        /// Проверить, что существуют объекты HTTP-контекста, сессии и запроса.
        /// Если объекты не существуют, то вызывается исключение
        /// </summary>
        public static void CheckSessionExists()
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException("HTTP context does not exist.");

            if (HttpContext.Current.Session == null)
                throw new InvalidOperationException("HTTP session object does not exist.");

            if (HttpContext.Current.Request == null)
                throw new InvalidOperationException("HTTP request object does not exist.");
        }

        /// <summary>
        /// Отключить кэширование страницы
        /// </summary>
        public static void DisablePageCache(HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            response.AppendHeader("Pragma", "No-cache");
            response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
        }

        /// <summary>
        /// Преобразовать строку для вывода на веб-страницу, заменив "\n" на тег "br"
        /// </summary>
        public static string HtmlEncodeWithBreak(string s)
        {
            return HttpUtility.HtmlEncode(s).Replace("\n", "<br />");
        }
    }
}