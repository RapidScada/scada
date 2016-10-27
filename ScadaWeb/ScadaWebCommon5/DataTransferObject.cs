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
 * Module   : ScadaWebCommon
 * Summary  : Data transfer object for WCF services
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

namespace Scada.Web
{
    /// <summary>
    /// Data transfer object for WCF services
    /// <para>Объект для передачи данных WCF-сервисами</para>
    /// </summary>
    public class DataTransferObject
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DataTransferObject()
            : this(true, "")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataTransferObject(object data)
            : this(true, "")
        {
            Data = data;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataTransferObject(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Data = null;
        }


        /// <summary>
        /// Получить или установить признак, что объект получен успешно
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Получить или установить сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Получить или установить данные, запрашиваемые клиентом
        /// </summary>
        public object Data { get; set; }
    }
}
