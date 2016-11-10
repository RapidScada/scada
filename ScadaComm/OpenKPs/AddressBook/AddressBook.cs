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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Devices.AB
{
    /// <summary>
    /// Address book
    /// <para>Адресная книга</para>
    /// </summary>
    public class AddressBook : ITreeNode
    {
        /// <summary>
        /// Базовый класс элементов справочника
        /// </summary>
        public abstract class AddressBookItem : IComparable<AddressBookItem>, ITreeNode
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public AddressBookItem()
            {
                Value = "";
                Parent = null;
                Children = null;
            }

            /// <summary>
            /// Получить порядок сортировки типа элемента
            /// </summary>
            public abstract int Order { get; }
            /// <summary>
            /// Получить или установить значение элемента
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// Получить или установить родительский узел
            /// </summary>
            public ITreeNode Parent { get; set; }
            /// <summary>
            /// Получить список дочерних узлов
            /// </summary>
            public IList Children { get; protected set; }

            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return Value;
            }
            /// <summary>
            /// Сравнить данный объект с заданным
            /// </summary>
            public int CompareTo(AddressBookItem other)
            {
                if (other == null)
                {
                    return 1;
                }
                else
                {
                    int comp = Order.CompareTo(other.Order);
                    return comp == 0 ? Value.CompareTo(other.Value) : comp;
                }
            }
        }

        /// <summary>
        /// Группа контактов
        /// </summary>
        public class ContactGroup : AddressBookItem, ITreeNode
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ContactGroup()
                : this("")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ContactGroup(string name) 
                : base()
            {
                Name = name ?? "";
                Contacts = new List<Contact>();
                Children = Contacts;
            }

            /// <summary>
            /// Получить или установить имя группы
            /// </summary>
            public string Name
            {
                get
                {
                    return Value;
                }
                set
                {
                    Value = value;
                }
            }
            /// <summary>
            /// Получить контакты, упорядоченные по имени
            /// </summary>
            public List<Contact> Contacts { get; private set; }
            /// <summary>
            /// Получить порядок сортировки типа элемента
            /// </summary>
            public override int Order
            {
                get
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Контакт
        /// </summary>
        public class Contact : AddressBookItem, ITreeNode
        {
            private List<string> phoneNumbers; // телефонные номера
            private List<string> emails;       // адреса эл. почты

            /// <summary>
            /// Конструктор
            /// </summary>
            public Contact()
                : this("")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Contact(string name)
                : base()
            {
                phoneNumbers = null;
                emails = null;

                Name = name ?? "";
                ContactRecords = new List<ContactRecord>();
                Children = ContactRecords;
            }

            /// <summary>
            /// Получить или установить имя контакта
            /// </summary>
            public string Name
            {
                get
                {
                    return Value;
                }
                set
                {
                    Value = value;
                }
            }
            /// <summary>
            /// Получить записи контакта, упорядоченные по возрастанию
            /// </summary>
            public List<ContactRecord> ContactRecords { get; private set; }
            /// <summary>
            /// Получить телефонные номера
            /// </summary>
            public List<string> PhoneNumbers
            {
                get
                {
                    if (phoneNumbers == null)
                        FillPhoneNumbers();
                    return phoneNumbers;
                }
            }
            /// <summary>
            /// Получить адреса электронной почты
            /// </summary>
            public List<string> Emails
            {
                get
                {
                    if (emails == null)
                        FillEmails();
                    return emails;
                }
            }
            /// <summary>
            /// Получить порядок сортировки типа элемента
            /// </summary>
            public override int Order
            {
                get
                {
                    return 1;
                }
            }

            /// <summary>
            /// Заполнить список телефонных номеров
            /// </summary>
            public void FillPhoneNumbers()
            {
                if (phoneNumbers == null)
                    phoneNumbers = new List<string>();
                else
                    phoneNumbers.Clear();

                foreach (ContactRecord rec in ContactRecords)
                {
                    if (rec is PhoneNumber)
                        phoneNumbers.Add(rec.Value);
                }
            }
            /// <summary>
            /// Заполнить список адресов электронной почты
            /// </summary>
            public void FillEmails()
            {
                if (emails == null)
                    emails = new List<string>();
                else
                    emails.Clear();

                foreach (ContactRecord rec in ContactRecords)
                {
                    if (rec is Email)
                        emails.Add(rec.Value);
                }
            }
        }

        /// <summary>
        /// Запись контакта
        /// </summary>
        public abstract class ContactRecord : AddressBookItem, ITreeNode
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ContactRecord()
                : base()
            {
            }
        }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        public class PhoneNumber : ContactRecord
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneNumber()
                : this("")
            {

            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public PhoneNumber(string value)
                : base()
            {
                Value = value;
            }

            /// <summary>
            /// Получить порядок сортировки типа поля
            /// </summary>
            public override int Order
            {
                get
                {
                    return 2;
                }
            }
        }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public class Email : ContactRecord
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Email()
                : this("")
            {

            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Email(string value)
                : base()
            {
                Value = value;
            }

            /// <summary>
            /// Получить порядок сортировки типа поля
            /// </summary>
            public override int Order
            {
                get
                {
                    return 3;
                }
            }
        }


        /// <summary>
        /// Имя файла адресной книги по умолчанию
        /// </summary>
        public const string DefFileName = "AddressBook.xml";

        private List<Contact> allContacts; // все контакты, упорядоченные по возрастанию


        /// <summary>
        /// Конструктор
        /// </summary>
        public AddressBook()
        {
            allContacts = null;
            ContactGroups = new List<ContactGroup>();
        }


        /// <summary>
        /// Получить группы контактов, упорядоченные по имени
        /// </summary>
        public List<ContactGroup> ContactGroups { get; private set; }

        /// <summary>
        /// Получить все контакты, упорядоченные по имени
        /// </summary>
        public List<Contact> AllContacts
        {
            get
            {
                if (allContacts == null)
                    FillAllContacts();
                return allContacts;
            }
        }

        /// <summary>
        /// Получить или установить родительский узел - он всегда равен null
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                // некорректный вызов
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Получить список дочерних узлов
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return ContactGroups;
            }
        }


        /// <summary>
        /// Загрузить адресную книгу из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                // очистка адресной книги
                ContactGroups.Clear();

                // загрузка адресной книги
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList contactGroupNodes = xmlDoc.DocumentElement.SelectNodes("ContactGroup");

                foreach (XmlElement contactGroupElem in contactGroupNodes)
                {
                    ContactGroup contactGroup = new ContactGroup(contactGroupElem.GetAttribute("name"));
                    contactGroup.Parent = this;

                    XmlNodeList contactNodes = contactGroupElem.SelectNodes("Contact");
                    foreach (XmlElement contactElem in contactNodes)
                    {
                        Contact contact = new Contact(contactElem.GetAttribute("name"));
                        contact.Parent = contactGroup;

                        XmlNodeList phoneNumberNodes = contactElem.SelectNodes("PhoneNumber");
                        foreach (XmlElement phoneNumberElem in phoneNumberNodes)
                            contact.ContactRecords.Add(
                                new PhoneNumber(phoneNumberElem.InnerText) { Parent = contact });

                        XmlNodeList emailNodes = contactElem.SelectNodes("Email");
                        foreach (XmlElement emailElem in emailNodes)
                            contact.ContactRecords.Add(
                                new Email(emailElem.InnerText) { Parent = contact });

                        contact.ContactRecords.Sort();
                        contactGroup.Contacts.Add(contact);
                    }

                    contactGroup.Contacts.Sort();
                    ContactGroups.Add(contactGroup);
                }

                ContactGroups.Sort();

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AbPhrases.LoadAddressBookError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить адресную книгу в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("AddressBook");
                xmlDoc.AppendChild(rootElem);

                foreach (ContactGroup contactGroup in ContactGroups)
                {
                    XmlElement contactGroupElem = xmlDoc.CreateElement("ContactGroup");
                    contactGroupElem.SetAttribute("name", contactGroup.Name);
                    rootElem.AppendChild(contactGroupElem);

                    foreach (Contact contact in contactGroup.Contacts)
                    {
                        XmlElement contactElem = xmlDoc.CreateElement("Contact");
                        contactElem.SetAttribute("name", contact.Name);

                        foreach (ContactRecord contactRecord in contact.ContactRecords)
                        {
                            if (contactRecord is PhoneNumber)
                                contactElem.AppendElem("PhoneNumber", contactRecord);
                            else if (contactRecord is Email)
                                contactElem.AppendElem("Email", contactRecord);
                        }

                        contactGroupElem.AppendChild(contactElem);
                    }
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AbPhrases.SaveAddressBookError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Найти группу контактов
        /// </summary>
        public ContactGroup FindContactGroup(string name)
        {
            int i = ContactGroups.BinarySearch(new ContactGroup(name));
            return i >= 0 ? ContactGroups[i] : null;
        }

        /// <summary>
        /// Найти контакт
        /// </summary>
        public Contact FindContact(string name)
        {
            int i = AllContacts.BinarySearch(new Contact(name));
            return i >= 0 ? AllContacts[i] : null;
        }

        /// <summary>
        /// Заполнить список всех контактов
        /// </summary>
        public void FillAllContacts()
        {
            if (allContacts == null)
                allContacts = new List<Contact>();
            else
                allContacts.Clear();

            foreach (ContactGroup contactGroup in ContactGroups)
                foreach (Contact contact in contactGroup.Contacts)
                    allContacts.Add(contact);

            allContacts.Sort();
        }

        /// <summary>
        /// Получить строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return "Address book";
        }
    }
}
