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
 * Summary  : The class contains utility methods for the address book
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Collections.Generic;
using System.IO;
using Utils;

namespace Scada.Comm.Devices.AB
{
    /// <summary>
    /// The class contains utility methods for the address book
    /// <para>Класс, содержащий вспомогательные методы для адресной книги</para>
    /// </summary>
    public static class AbUtils
    {
        /// <summary>
        /// Загрузить адресную книгу из файла
        /// </summary>
        public static bool LoadAddressBook(string configDir, Log.WriteLineDelegate writeToLog, 
            out AddressBook addressBook)
        {
            addressBook = new AddressBook();

            string fileName = configDir + AddressBook.DefFileName;
            if (File.Exists(fileName))
            {
                writeToLog(Localization.UseRussian ?
                    "Загрузка адресной книги" :
                    "Loading address book");
                string errMsg;

                if (addressBook.Load(fileName, out errMsg))
                {
                    return true;
                }
                else
                {
                    writeToLog(errMsg);
                    return false;
                }
            }
            else
            {
                writeToLog(Localization.UseRussian ?
                    "Адресная книга отсутствует" :
                    "Address book is missing");
                return false;
            }
        }

        /// <summary>
        /// Загрузить адресную книгу из файла или получить её из общих свойств линии связи Коммуникатора
        /// </summary>
        public static AddressBook GetAddressBook(string configDir, SortedList<string, object> commonProps, 
            Log.WriteLineDelegate writeToLog)
        {
            AddressBook addressBook;
            object addrBookObj;

            if (commonProps.TryGetValue("AddressBook", out addrBookObj))
            {
                addressBook = addrBookObj as AddressBook;
            }
            else
            {
                LoadAddressBook(configDir, writeToLog, out addressBook);
                commonProps.Add("AddressBook", addressBook);
            }

            return addressBook;
        }
    }
}
