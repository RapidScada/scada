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
 * Module   : AddressBook
 * Summary  : Address book
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.Collections.Generic;

namespace Scada.Comm.Devices.AddressBook
{
    /// <summary>
    /// Address book
    /// <para>Адресная книга</para>
    /// </summary>
    public class AddressBook
    {
        /// <summary>
        /// Группа контактов
        /// </summary>
        public class ContactGroup
        {

        }

        /// <summary>
        /// Контакт
        /// </summary>
        public class Contact
        {

        }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        public class PhoneNumber
        {

        }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public class Email
        {

        }


        /// <summary>
        /// Имя файла адресной книги по умолчанию
        /// </summary>
        public const string DefFileName = "AddressBook.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public AddressBook()
        {
            ContactGroups = new SortedList<string, ContactGroup>();
        }


        /// <summary>
        /// Получить группы телефонных контактов, упорядоченные по наименованию
        /// </summary>
        public SortedList<string, ContactGroup> ContactGroups { get; private set; }
    }
}
