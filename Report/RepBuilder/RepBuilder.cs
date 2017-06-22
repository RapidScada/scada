/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : Report Builder
 * Summary  : The base class for building reports
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2017
 */

using System;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace Utils.Report
{
    /// <summary>
    /// The base class for building reports
    /// <para>Базовый класс для построения отчётов</para>
    /// </summary>
    public abstract class RepBuilder
    {
        /// <summary>
        /// Получить имя отчёта
        /// </summary>
        public abstract string RepName { get; }

        /// <summary>
        /// Получить описание отчёта
        /// </summary>
        public virtual string RepDescr
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Получить формат отчёта
        /// </summary>
        public abstract string RepFormat { get; }

        /// <summary>
        /// Получить имя файла шаблона
        /// </summary>
        public virtual string TemplateFileName
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Получить или установить ссылку на объект взаимодействия с данными
        /// </summary>
        public virtual object DataRef
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// Получить класс Windows-формы для настройки параметров отчёта
        /// </summary>
        public virtual Form WinForm
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получить имя файла Web-страницы для настройки параметров отчёта
        /// </summary>
        public virtual string WebFormFileName
        {
            get
            {
                return "";
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        public RepBuilder()
        {
        }


        /// <summary>
        /// Установить параметры отчёта
        /// </summary>
        public virtual void SetParams(params object[] repParams)
        {
        }

        /// <summary>
        /// Генерировать отчёт в поток
        /// </summary>
        /// <param name="outStream">Выходной поток</param>
        /// <param name="templateDir">Директория шаблона со '\' на конце</param>
        public abstract void Make(Stream outStream, string templateDir);

        /// <summary>
        /// Генерировать отчёт в файл
        /// </summary>
        /// <param name="outFileName">Имя выходного файла</param>
        /// <param name="templateDir">Директория шаблона со '\' на конце</param>
        public virtual void Make(string outFileName, string templateDir)
        {
            Stream outStream = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
            try
            {
                Make(outStream, templateDir);
            }
            finally
            {
                outStream.Close();
            }
        }

        /// <summary>
        /// Генерировать отчёт для загрузки через браузер
        /// </summary>
        public virtual void Generate(object[] repParams, string templateDir, string fileName, HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            try
            {
                response.ClearHeaders();
                response.ClearContent();

                response.ContentType = "application/octet-stream";
                if (!string.IsNullOrEmpty(fileName))
                    response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");

                SetParams(repParams);
                Make(response.OutputStream, templateDir);
            }
            catch
            {
                response.ClearHeaders();
                response.ClearContent();
                response.ContentType = "text/html";
                throw;
            }
        }
    }
}
