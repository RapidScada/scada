/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ScadaAgentNet
 * Summary  : Message for uploading the configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.IO;
using System.ServiceModel;

namespace Scada.Agent.Net
{
    /// <summary>
    /// Message for uploading the configuration
    /// <para>Сообщение для загрузки конфигурации</para>
    /// </summary>
    [MessageContract]
    public class ConfigUploadMessage : IDisposable
    {
        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        [MessageHeader]
        public long SessionID;

        /// <summary>
        /// Параметры передачи конфигурации
        /// </summary>
        [MessageHeader]
        public ConfigOptions ConfigOptions;

        /// <summary>
        /// Поток данных конфигурации
        /// </summary>
        [MessageBodyMember]
        public Stream Stream;


        /// <summary>
        /// Очистить ресурсы объекта
        /// </summary>
        public void Dispose()
        {
            if (Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }
        }
    }
}
