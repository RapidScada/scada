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
 * Summary  : The class for transfer scheme image
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using System;
using System.IO;

namespace Scada.Scheme.DataTransfer
{
    /// <summary>
    /// The class for transfer scheme image
    /// <para>Класс для передачи изображения схемы</para>
    /// </summary>
    public class ImageDTO
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ImageDTO()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ImageDTO(Image image)
        {
            Name = image.Name ?? "";
            Data = Convert.ToBase64String(image.Data == null ? new byte[0] : image.Data,
                Base64FormattingOptions.None);
            SetMediaType();
        }


        /// <summary>
        /// Получить наименование
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Получить медиа-тип
        /// </summary>
        public string MediaType { get; protected set; }

        /// <summary>
        /// Получить данные в формате base 64
        /// </summary>
        public string Data { get; protected set; }


        /// <summary>
        /// Установить медиа-тип на основе наименования
        /// </summary>
        private void SetMediaType()
        {
            string ext = Path.GetExtension(Name).ToLowerInvariant();
            if (ext == ".png")
                MediaType = "image/png";
            else if (ext == ".jpg")
                MediaType = "image/jpeg";
            else if (ext == ".gif")
                MediaType = "image/gif";
            else if (ext == ".svg")
                MediaType = "image/svg+xml";
            else
                MediaType = "";
        }
    }
}
