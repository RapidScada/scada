/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : KpSms
 * Summary  : Phone book
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices.KpSms
{
    /// <summary>
    /// Phone book
    /// <para>Телефонный справочник</para>
    /// </summary>
    internal class Phonebook
    {
        /// <summary>
        /// Телефонный номер
        /// </summary>
        public class PhoneNumber
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneNumber()
                : this("", "")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneNumber(string number, string name)
            {
                Number = number ?? "";
                Name = name ?? "";
            }

            /// <summary>
            /// Получить или установить номер
            /// </summary>
            public string Number { get; set; }
            /// <summary>
            /// Получить или установить имя владельца
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// Группа телефонных номеров
        /// </summary>
        public class PhoneGroup
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneGroup()
                : this("")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneGroup(string name)
            {
                Name = name ?? "";
                PhoneNumbers = new SortedList<string, PhoneNumber>();
            }

            /// <summary>
            /// Получить или установить наименование группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить телефонные номера, упорядоченные по имени владельца
            /// </summary>
            public SortedList<string, PhoneNumber> PhoneNumbers { get; private set; }
        }


        /// <summary>
        /// Имя файла телефонного справочника по умолчанию
        /// </summary>
        public const string DefFileName = "KpSmsPhonebook.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Phonebook()
        {
            PhoneGroups = new SortedList<string, PhoneGroup>();
        }


        /// <summary>
        /// Получить группы телефонных номеров, упорядоченные по наименованию
        /// </summary>
        public SortedList<string, PhoneGroup> PhoneGroups { get; private set; }


        /// <summary>
        /// Загрузить телефонный справочник из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            errMsg = "";
            return true;
        }

        /// <summary>
        /// Сохранить телефонный справочник в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            errMsg = "";
            return true;
        }
    }
}
