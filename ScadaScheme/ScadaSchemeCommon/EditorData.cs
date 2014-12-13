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
 * Summary  : Scheme editor data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Drawing;
using System.IO;
using Utils;

namespace Scada.Scheme
{
    /// <summary>
    /// Scheme editor data
    /// <para>Данные редактора схем</para>
    /// </summary>
    public class EditorData
    {
        /// <summary>
        /// Длительность ожидания считывания изменений
        /// </summary>
        private readonly TimeSpan WaitForChange = TimeSpan.FromSeconds(3);

        private volatile SchemeView.SchemeChange schemeChange; // изменение схемы


        /// <summary>
        /// Конструктор
        /// </summary>
        public EditorData()
        {
            schemeChange = null;

            ClientID = "";
            FileName = "";
            Modified = false;
            SchemeView = new SchemeView();
            AddedElement = null;
            CursorPosition = Point.Empty;
            SelectElement = null;
            SetFormTitle = null;
        }


        /// <summary>
        /// Получить или установить идентификатор клиента
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Получить или установить имя файла схемы
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получить или установить признак изменения схемы
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Получить представление схемы
        /// </summary>
        public SchemeView SchemeView { get; private set; }

        /// <summary>
        /// Получить или установить изменение схемы
        /// </summary>
        public SchemeView.SchemeChange SchemeChange
        {
            get
            {
                return schemeChange;
            }
            set
            {
                // проверка, что изменение считано
                if (value != null && schemeChange != null)
                    throw new Exception("Не удалось отобразить изменение на схеме.");

                // ожидание считывания изменений
                /*if (value != null && schemeChange != null)
                {
                    DateTime startDT = DateTime.Now;
                    DateTime curDT;

                    do
                    {
                        Thread.Sleep(100);
                        curDT = DateTime.Now;
                    } while (schemeChange != null && curDT > startDT && curDT - startDT <= WaitForChange);

                    if (schemeChange != null)
                        throw new Exception("Не удалось отобразить изменение на схеме.");
                }*/

                // установка изменения
                schemeChange = value;
                Modified = true;
            }
        }

        /// <summary>
        /// Получить или установить добавляемый элемент
        /// </summary>
        public SchemeView.Element AddedElement { get; set; }

        /// <summary>
        /// Получить или установить позицию указателя мыши
        /// </summary>
        public Point CursorPosition { get; set; }

        /// <summary>
        /// Получить или установить метод выбора элемента схемы на форме редактора
        /// </summary>
        public Action<object> SelectElement { get; set; }

        /// <summary>
        /// Получить или установить метод установки заголовка формы
        /// </summary>
        public Action SetFormTitle { get; set; }


        /// <summary>
        /// Установить новый идентификатор клиента
        /// </summary>
        public void NewClientID()
        {
            ClientID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Загрузить схему из файла
        /// </summary>
        public bool LoadSchemeFromFile(out string errMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    // очистка схемы
                    SchemeView.Clear();
                }
                else
                {
                    // загрузка схемы
                    using (FileStream fileStream =
                        new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        SchemeView.LoadFromStream(fileStream);
                    }
                }

                Modified = false;
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = SchemePhrases.LoadSchemeError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Записать схему в файл
        /// </summary>
        public bool SaveSchemeToFile(out string errMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                    throw new Exception(SchemePhrases.FileNameUndefined);

                using (FileStream fileStream =
                    new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    SchemeView.SaveToStream(fileStream);
                }

                Modified = false;
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = SchemePhrases.SaveSchemeError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Попытаться установить изменение схемы, выдав сообщение в случае ошибки
        /// </summary>
        public void TrySetSchemeChange(SchemeView.SchemeChange change)
        {
            try
            {
                SchemeChange = change;
            }
            catch (Exception ex)
            {
                SchemeApp.GetSchemeApp().Log.WriteAction(ex.Message, Log.ActTypes.Exception);
                ScadaUtils.ShowError(ex.Message);
            }
        }
    }
}
