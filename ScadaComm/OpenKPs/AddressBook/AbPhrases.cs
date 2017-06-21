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
 * Summary  : The phrases used by the address book
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

namespace Scada.Comm.Devices.AB
{
    /// <summary>
    /// The phrases used by the address book
    /// <para>Фразы, используемые адресной книгой</para>
    /// </summary>
    internal static class AbPhrases
    {
        static AbPhrases()
        {
            SetToDefault();
        }

        // Словарь Scada.Comm.Devices.AddressBook
        public static string LoadAddressBookError { get; private set; }
        public static string SaveAddressBookError { get; private set; }

        // Словарь Scada.Comm.Devices.AddressBook.FrmAddressBook
        public static string AddressBookNode { get; private set; }
        public static string NewContactGroup { get; private set; }
        public static string NewContact { get; private set; }
        public static string NewPhoneNumber { get; private set; }
        public static string NewEmail { get; private set; }
        public static string ContactGroupExists { get; private set; }
        public static string ContactExists { get; private set; }
        public static string PhoneNumberExists { get; private set; }
        public static string EmailExists { get; private set; }
        public static string IncorrectEmail { get; private set; }
        public static string EmptyValueNotAllowed { get; private set; }
        public static string SavePhonebookConfirm { get; private set; }

        private static void SetToDefault()
        {
            LoadAddressBookError = "Ошибка при загрузке адресной книги";
            SaveAddressBookError = "Ошибка при сохранении адресной книги";

            AddressBookNode = "Адресная книга";
            NewContactGroup = "Новая группа";
            NewContact = "Новый контакт";
            NewPhoneNumber = "Новый тел. номер";
            NewEmail = "Новый эл. адрес";
            ContactGroupExists = "Группа контактов уже существует.";
            ContactExists = "Контакт уже существует в группе.";
            PhoneNumberExists = "Контакт уже содержит данный телефонный номер.";
            EmailExists = "Контакт уже содержит данный адрес электронной почты.";
            IncorrectEmail = "Некорректный адрес электронной почты.";
            EmptyValueNotAllowed = "Значение не может быть пустым.";
            SavePhonebookConfirm = "Адресная книга была изменена. Сохранить изменения?";
        }

        public static void Init()
        {
            Localization.Dict dict;
            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Devices.AB", out dict))
            {
                LoadAddressBookError = dict.GetPhrase("LoadAddressBookError", LoadAddressBookError);
                SaveAddressBookError = dict.GetPhrase("SaveAddressBookError", SaveAddressBookError);
            }

            if (Localization.Dictionaries.TryGetValue("Scada.Comm.Devices.AB.FrmAddressBook", out dict))
            {
                NewContactGroup = dict.GetPhrase("NewContactGroup", NewContactGroup);
                NewContact = dict.GetPhrase("NewContact", NewContact);
                NewPhoneNumber = dict.GetPhrase("NewPhoneNumber", NewPhoneNumber);
                NewEmail = dict.GetPhrase("NewEmail", NewEmail);
                AddressBookNode = dict.GetPhrase("AddressBookNode", AddressBookNode);
                ContactGroupExists = dict.GetPhrase("ContactGroupExists", ContactGroupExists);
                ContactExists = dict.GetPhrase("ContactExists", ContactExists);
                PhoneNumberExists = dict.GetPhrase("PhoneNumberExists", PhoneNumberExists);
                EmailExists = dict.GetPhrase("EmailExists", EmailExists);
                IncorrectEmail = dict.GetPhrase("IncorrectEmail", IncorrectEmail);
                EmptyValueNotAllowed = dict.GetPhrase("EmptyValueNotAllowed", EmptyValueNotAllowed);
                SavePhonebookConfirm = dict.GetPhrase("SavePhonebookConfirm", SavePhonebookConfirm);
            }
        }
    }
}
