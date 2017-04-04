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

using System;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Editor data and logic
    /// <para>Данные и логика редактора</para>
    /// </summary>
    internal class Editor
    {
        /// <summary>
        /// Длина идентификатора сессии
        /// </summary>
        private const int SessionIDLength = 10;
        /// <summary>
        /// Длина идентификатора редактируемой схемы
        /// </summary>
        private const int SchemeIDLength = 10;
        /// <summary>
        /// Имя файла веб-страницы редактора
        /// </summary>
        private const string WebPageFileName = "editor.html";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Editor()
        {
            SessionID = GetRandomString(SessionIDLength);
            SchemeID = "";
            SchemeView = null;
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
        /// Обновить идентификатор редактируемой схемы
        /// </summary>
        private void RefreshSchemeID()
        {
            SchemeID = GetRandomString(SchemeIDLength);
        }


        /// <summary>
        /// Получить идентификатор сессии
        /// </summary>
        public string SessionID { get; private set; }

        /// <summary>
        /// Получить идентификатор редактируемой схемы
        /// </summary>
        public string SchemeID { get; private set; }

        /// <summary>
        /// Получить представление редактируемой схемы
        /// </summary>
        public SchemeView SchemeView { get; private set; }


        /// <summary>
        /// Создать файл веб-страницы редактора
        /// </summary>
        public bool CreateWebPage(string webDir)
        {
            try
            {
                string fileName = webDir + WebPageFileName;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Создать новую схему
        /// </summary>
        public void NewScheme()
        {
            SchemeView = new SchemeView();
            RefreshSchemeID();
        }

        /// <summary>
        /// Загрузить схему из файла
        /// </summary>
        public bool LoadSchemeFromFile(string fileName, out string errMsg)
        {
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Записать схему в файл
        /// </summary>
        public bool SaveSchemeToFile(string fileName, out string errMsg)
        {
            errMsg = "";
            return true;
        }
    }
}
