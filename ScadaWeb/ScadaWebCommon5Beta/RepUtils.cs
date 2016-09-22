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
 * Summary  : The class contains utility methods for generating reports
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Web;
using Utils;
using Utils.Report;

namespace Scada.Web
{
    /// <summary>
    /// The class contains utility methods for generating reports
    /// <para>Класс, содержащий вспомогательные методы для генерации отчётов</para>
    /// </summary>
    public static class RepUtils
    {
        /// <summary>
        /// Генерировать отчёт для загрузки через браузер
        /// </summary>
        public static void GenerateReport(RepBuilder repBuilder, object[] repParams, 
            string templateDir, string fileName, HttpResponse response)
        {
            if (repBuilder == null)
                throw new ArgumentNullException("repBuilder");
            if (response == null)
                throw new ArgumentNullException("response");

            try
            {
                response.ClearHeaders();
                response.ClearContent();

                response.ContentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileName))
                    response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");

                repBuilder.SetParams(repParams);
                repBuilder.Make(response.OutputStream, templateDir);
            }
            catch
            {
                response.ClearHeaders();
                response.ClearContent();
                response.ContentType = "text/html";
                throw;
            }
        }

        /// <summary>
        /// Сформировать имя файла отчёта
        /// </summary>
        public static string BuildFileName(string prefix, string extension)
        {
            return prefix + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "." + extension;
        }

        /// <summary>
        /// Записать сообщение о генерации отчёта в журнал
        /// </summary>
        public static void WriteGenerationAction(Log log, RepBuilder repBuilder, UserData userData)
        {
            log.WriteAction(string.Format(WebPhrases.GenReport, repBuilder.RepName, userData.UserProps.UserName));
        }
    }
}
