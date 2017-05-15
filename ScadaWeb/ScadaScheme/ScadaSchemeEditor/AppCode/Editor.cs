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

using Scada.Scheme.Model;
using Scada.Web;
using System;
using System.Collections.Generic;
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

        private readonly Log log;     // журнал приложения
        private List<Change> changes; // изменения схемы
        private long changeStampCntr; // счётчик для генерации меток изменений схемы


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
            changes = new List<Change>();
            changeStampCntr = 0;

            EditorID = GetRandomString(EditorIDLength);
            SchemeView = null;
            FileName = "";
            Modified = false;
            NewComponentTypeName = "";
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
        /// Получить или установить имя типа компонента, который может быть создан пользователем
        /// </summary>
        public string NewComponentTypeName { get; set; }


        /// <summary>
        /// Подписаться на изменения схемы
        /// </summary>
        private void SubscribeToChanges()
        {
            if (SchemeView != null)
            {
                SchemeView.SchemeDoc.ItemChanged += Scheme_ItemChanged;

                foreach (BaseComponent component in SchemeView.Components)
                    component.ItemChanged += Scheme_ItemChanged;
            }
        }

        /// <summary>
        /// Обработать событие изменения схемы
        /// </summary>
        private void Scheme_ItemChanged(object sender, SchemeChangeTypes changeType, object changedObject, object oldKey)
        {
            lock (changes)
            {
                Change change = new Change(changeType) { Stamp = ++changeStampCntr };

                if (changeType == SchemeChangeTypes.ComponentDeleted)
                {
                    if (changedObject is BaseComponent)
                        change.DeletedComponentID = ((BaseComponent)changedObject).ID;
                }
                else if (changeType == SchemeChangeTypes.ImageRenamed ||
                    changeType == SchemeChangeTypes.ImageDeleted)
                {
                    change.ImageName = (changedObject as Image)?.Name;
                    change.OldImageName = oldKey as string;
                }
                else
                {
                    change.ChangedObject = changedObject;
                }

                changes.Add(change);
            }
        }


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
            SubscribeToChanges();
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
            SubscribeToChanges();

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

        /// <summary>
        /// Получить изменения схемы для передачи
        /// </summary>
        public List<Change> GetChanges(long trimBeforeStamp)
        {
            lock (changes)
            {
                while (changes.Count > 0 && changes[0].Stamp <= trimBeforeStamp)
                {
                    changes.RemoveAt(0);
                }

                List<Change> destChanges = new List<Change>(changes.Count);

                foreach (Change change in changes)
                {
                    destChanges.Add(change.ConvertToDTO());
                }

                return new List<Change>(destChanges);
            }
        }

        /// <summary>
        /// Получить идентификаторы выбранных компонентов схемы
        /// </summary>
        public List<int> GetSelectedComponentIDs()
        {
            return new List<int>();
        }
    }
}
