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
 * Module   : Scheme Editor
 * Summary  : Editor data and logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Web;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Editor data and logic
    /// <para>Данные и логика редактора</para>
    /// </summary>
    internal class Editor
    {
        /// <summary>
        /// Длина идентификатора редактора
        /// </summary>
        private const int EditorIDLength = 10;
        /// <summary>
        /// Имя файла веб-страницы редактора
        /// </summary>
        public const string WebPageFileName = "editor.html";
        /// <summary>
        /// Имя файла схемы по умолчанию
        /// </summary>
        public const string DefSchemeFileName = "NewScheme.sch";

        protected readonly Log log; // журнал приложения


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected Editor()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Editor(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this.log = log;

            EditorID = GetRandomString(EditorIDLength);
            SchemeView = null;
            FileName = "";
            Modified = false;
        }


        /// <summary>
        /// Получить случайную строку символов
        /// </summary>
        private static string GetRandomString(int length)
        {
            const string Abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyxz0123456789";
            int abcLen = Abc.Length;
            Random rand = new Random();
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
                chars[i] = Abc[rand.Next(abcLen)];

            return new string(chars);
    }


        /// <summary>
        /// Получить идентификатор редактора
        /// </summary>
        public string EditorID { get; private set; }

        /// <summary>
        /// Получить представление редактируемой схемы
        /// </summary>
        public SchemeView SchemeView { get; private set; }

        /// <summary>
        /// Получить имя файла схемы
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Получить признак изменения схемы
        /// </summary>
        public bool Modified { get; private set; }


        /// <summary>
        /// Создать веб-страницу редактора
        /// </summary>
        public bool CreateWebPage(string webDir)
        {
            try
            {
                // загрузка шаблона веб-страницы
                string webPageTemplate;

                using (Stream stream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("Scada.Scheme.Editor.Web.editor.html"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        webPageTemplate = reader.ReadToEnd();
                    }
                }

                // создание файла веб-страницы
                StringBuilder sbCustomScript = new StringBuilder();
                sbCustomScript
                    .AppendFormat("var editorID = '{0}';", EditorID)
                    .AppendLine()
                    .Append("var phrases = ")
                    .Append(WebUtils.DictionaryToJs("Scada.Scheme.Editor.Js"));
                string webPageContent = string.Format(webPageTemplate, sbCustomScript.ToString());

                using (StreamWriter writer = new StreamWriter(webDir + WebPageFileName, false, Encoding.UTF8))
                {
                    writer.Write(webPageContent);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при создании веб-страницы редактора" :
                    "Error creating editor web page");
                return false;
            }
        }

        /// <summary>
        /// Создать новую схему
        /// </summary>
        public void NewScheme()
        {
            SchemeView = new SchemeView();
            FileName = "";
            Modified = false;
        }

        /// <summary>
        /// Загрузить схему из файла
        /// </summary>
        public bool LoadSchemeFromFile(string fileName, out string errMsg)
        {
            SchemeView = new SchemeView();
            bool loadOK = SchemeView.LoadFromFile(fileName, out errMsg);
            FileName = fileName;
            Modified = false;

            if (!loadOK)
                log.WriteError(errMsg);

            return loadOK;
        }

        /// <summary>
        /// Записать схему в файл
        /// </summary>
        public bool SaveSchemeToFile(string fileName, out string errMsg)
        {
            FileName = fileName;

            if (SchemeView == null)
            {
                errMsg = "";
                return true;
            }
            else if (SchemeView.SaveToFile(fileName, out errMsg))
            {
                return true;
            }
            else
            {
                log.WriteError(errMsg);
                return false;
            }
        }
    }
}
