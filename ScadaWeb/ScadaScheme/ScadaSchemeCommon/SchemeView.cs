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
 * Module   : ScadaSchemeCommon
 * Summary  : Scheme view
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Client;
using Scada.Scheme.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Scheme
{
    /// <summary>
    /// Scheme view
    /// <para>Представление схемы</para>
    /// </summary>
    public class SchemeView : BaseView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SchemeView()
            : base()
        {
            SchemeDocument = new SchemeDocument();
            Components = new List<BaseComponent>();
        }


        /// <summary>
        /// Получить свойства документа схемы
        /// </summary>
        public SchemeDocument SchemeDocument { get; protected set; }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        public List<BaseComponent> Components { get; protected set; }


        /// <summary>
        /// Загрузить представление из потока
        /// </summary>
        public override void LoadFromStream(Stream stream)
        {
            // очистка представления
            Clear();

            // загрузка представления
            Components.Add(new StaticText());
        }

        /// <summary>
        /// Загрузить схему из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            try
            {
                using (FileStream fileStream =
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    LoadFromStream(fileStream);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = SchemePhrases.LoadSchemeViewError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Очистить представление
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            Components.Clear();
        }
    }
}
