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
using System.Xml;

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
            /// Объект для сравнения телефонных номеров по имени владельца
            /// </summary>
            public static readonly PhoneNumberByNameComparer ByNameComparer = new PhoneNumberByNameComparer();

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

            /// <summary>
            /// Определить, равен ли телефонный номер заданному номеру
            /// </summary>
            public bool Equals(PhoneNumber other)
            {
                return other != null &&
                    string.Equals(Number, other.Number, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Класс для сравнения телефонных номеров по имени владельца
        /// </summary>
        public class PhoneNumberByNameComparer : IComparer<PhoneNumber>
        {
            /// <summary>
            /// Сравнить два объекта
            /// </summary>
            public int Compare(PhoneNumber x, PhoneNumber y)
            {
                int comp = string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
                return comp == 0 ?
                    string.Compare(x.Number, y.Number, StringComparison.OrdinalIgnoreCase) :
                    comp;
            }
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
                PhoneNumbers = new List<PhoneNumber>();
            }

            /// <summary>
            /// Получить или установить наименование группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить телефонные номера, упорядоченные по имени владельца
            /// </summary>
            /// <remarks>Имя владельца может быть не уникально в группе</remarks>
            public List<PhoneNumber> PhoneNumbers { get; private set; }

            /// <summary>
            /// Найти индекс телефонного номера в списке
            /// </summary>
            public int FindPhoneNumberIndex(string number)
            {
                int cnt = PhoneNumbers.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (PhoneNumbers[i].Number.Equals(number, StringComparison.OrdinalIgnoreCase))
                        return i;
                }
                return -1;
            }
            /// <summary>
            /// Найти индекс для вставки телефонного номера
            /// </summary>
            public int FindPhoneNumberInsertIndex(PhoneNumber phoneNumber)
            {
                int ind = PhoneNumbers.BinarySearch(phoneNumber, PhoneNumber.ByNameComparer);
                return ind >= 0 ? ind : ~ind;
            }
            /// <summary>
            /// Получить массив телефонных номеров группы
            /// </summary>
            public string[] GetPhoneNumbers()
            {
                int cnt = PhoneNumbers.Count;
                string[] phoneNumbers = new string[cnt];
                for (int i = 0; i < cnt; i++)
                    phoneNumbers[i] = PhoneNumbers[i].Number;
                return phoneNumbers;
            }
            /// <summary>
            /// Определить, равна ли группа телефонных номеров заданной группе
            /// </summary>
            public bool Equals(PhoneGroup other)
            {
                return other != null && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            }
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
            try
            {
                // очистка справочника
                PhoneGroups.Clear();

                // загрузка справочника
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNodeList phoneGroupNodes = xmlDoc.DocumentElement.SelectNodes("PhoneGroup");

                foreach (XmlElement phoneGroupElem in phoneGroupNodes)
                {
                    PhoneGroup phoneGroup = new PhoneGroup(phoneGroupElem.GetAttribute("name"));
                    if (!PhoneGroups.ContainsKey(phoneGroup.Name))
                        PhoneGroups.Add(phoneGroup.Name, phoneGroup);

                    XmlNodeList phoneNumberNodes = phoneGroupElem.SelectNodes("PhoneNumber");
                    HashSet<string> phoneNumberSet = new HashSet<string>(); // контроль уникальности номеров в группе

                    foreach (XmlElement phoneNumberElem in phoneNumberNodes)
                    {
                        PhoneNumber phoneNumber = new PhoneNumber(phoneNumberElem.GetAttribute("number"), 
                            phoneNumberElem.GetAttribute("name"));
                        if (!phoneNumberSet.Contains(phoneNumber.Number))
                        {
                            phoneGroup.PhoneNumbers.Add(phoneNumber);
                            phoneNumberSet.Add(phoneNumber.Number);
                        }
                    }

                    phoneGroup.PhoneNumbers.Sort(PhoneNumber.ByNameComparer);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpDllSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить телефонный справочник в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("Phonebook");
                xmlDoc.AppendChild(rootElem);

                foreach (PhoneGroup phoneGroup in PhoneGroups.Values)
                {
                    XmlElement phoneGroupElem = xmlDoc.CreateElement("PhoneGroup");
                    phoneGroupElem.SetAttribute("name", phoneGroup.Name);
                    rootElem.AppendChild(phoneGroupElem);

                    foreach (PhoneNumber phoneNumber in phoneGroup.PhoneNumbers)
                    {
                        XmlElement phoneNumberElem = xmlDoc.CreateElement("PhoneNumber");
                        phoneNumberElem.SetAttribute("number", phoneNumber.Number);
                        phoneNumberElem.SetAttribute("name", phoneNumber.Name);
                        phoneGroupElem.AppendChild(phoneNumberElem);
                    }
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpDllSettingsError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }
    }
}
