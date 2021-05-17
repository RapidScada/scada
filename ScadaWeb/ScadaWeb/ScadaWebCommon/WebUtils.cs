/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Scada.Web
{
    /// <summary>
    /// The class contains utility methods for web applications.
    /// <para>Класс, содержащий вспомогательные методы для веб-приложений.</para>
    /// </summary>
    public static partial class WebUtils
    {
        /// <summary>
        /// Версия веб-приложения.
        /// </summary>
        public const string AppVersion = "5.1.3.0";
        /// <summary>
        /// Шиблон для вставки стилей на веб-страницу.
        /// </summary>
        public const string StyleTemplate = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        /// <summary>
        /// Шиблон для вставки скрипта на веб-страницу.
        /// </summary>
        public const string ScriptTemplate = "<script type=\"text/javascript\" src=\"{0}\"></script>";
        /// <summary>
        /// Начало отчёта времени в Unix, которое используется в Javascript реализации даты.
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// Длительность хранения данных в кэше приложения.
        /// </summary>
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);
        /// <summary>
        /// Длительность хранения данных в cookies.
        /// </summary>
        public static readonly TimeSpan CookieExpiration = TimeSpan.FromDays(7);


        /// <summary>
        /// Проверить HTTP-контекст и его основные свойства на null.
        /// </summary>
        public static void CheckHttpContext(HttpContext httpContext, bool checkCookies = false)
        {
            const string msg = "HTTP context or its properties are undefined.";

            if (httpContext == null)
                throw new ArgumentNullException("httpContext", msg);
            if (httpContext.Session == null)
                throw new ArgumentNullException("httpContext.Session", msg);
            if (httpContext.Request == null)
                throw new ArgumentNullException("httpContext.Request", msg);
            if (httpContext.Response == null)
                throw new ArgumentNullException("httpContext.Response", msg);

            if (checkCookies)
            {
                if (httpContext.Request.Cookies == null)
                    throw new ArgumentNullException("httpContext.Request.Cookies", msg);
                if (httpContext.Response.Cookies == null)
                    throw new ArgumentNullException("httpContext.Response.Cookies", msg);
            }
        }

        /// <summary>
        /// Проверить, что выполняется AJAX-запрос.
        /// </summary>
        public static bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        /// Отключить кэширование страницы.
        /// </summary>
        public static void DisablePageCache(HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            response.AppendHeader("Pragma", "No-cache");
            response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
        }

        /// <summary>
        /// Преобразовать строку для вывода на веб-страницу, заменив "\n" на тег "br".
        /// </summary>
        public static string HtmlEncodeWithBreak(object val)
        {
            return HttpUtility.HtmlEncode(val).Replace("\n", "<br />");
        }

        /// <summary>
        /// Преобразовать словарь в объект JavaScript.
        /// </summary>
        public static string DictionaryToJs(Localization.Dict dict)
        {
            StringBuilder sbJs = new StringBuilder();
            sbJs.AppendLine("{");

            if (dict != null)
            {
                foreach (KeyValuePair<string, string> pair in dict.Phrases)
                {
                    sbJs.Append(pair.Key).Append(": \"")
                        .Append(HttpUtility.JavaScriptStringEncode(pair.Value)).AppendLine("\",");
                }
            }

            sbJs.Append("}");
            return sbJs.ToString();
        }

        /// <summary>
        /// Получить словарь по ключу и преобразовать в объект JavaScript.
        /// </summary>
        public static string DictionaryToJs(string dictKey)
        {
            Localization.Dict dict;
            Localization.Dictionaries.TryGetValue(dictKey, out dict);
            return DictionaryToJs(dict);
        }

        /// <summary>
        /// Преобразовать дату в время в число миллисекунд для создания даты в JavaScript.
        /// </summary>
        public static long DateTimeToJs(DateTime dateTime)
        {
            return dateTime > UnixEpoch ? (long)(dateTime - UnixEpoch).TotalMilliseconds : 0;
        }

        /// <summary>
        /// Получить объект для передачи данных, содержащий информацию об ошибке, в формате JSON.
        /// </summary>
        public static string GetErrorJson(this JavaScriptSerializer jsSerializer, Exception ex)
        {
            return jsSerializer.Serialize(new DataTransferObject(false, ex.Message));
        }
    }
}
