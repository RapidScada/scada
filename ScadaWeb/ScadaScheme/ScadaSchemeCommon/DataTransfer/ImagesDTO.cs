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
 * Summary  : The class for transfer scheme images
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using System.Collections.Generic;

namespace Scada.Scheme.DataTransfer
{
    /// <summary>
    /// The class for transfer scheme images
    /// <para>Класс для передачи изображений схемы</para>
    /// </summary>
    public class ImagesDTO : SchemeDTO
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ImagesDTO()
            : base()
        {
            EndOfImages = false;
            Images = new List<ImageDTO>();
        }


        /// <summary>
        /// Получить или установить признак, что считаны все изображения схемы
        /// </summary>
        public bool EndOfImages { get; set; }

        /// <summary>
        /// Получить изображения схемы
        /// </summary>
        public List<ImageDTO> Images { get; protected set; }


        /// <summary>
        /// Копировать заданные изображения в объект для передачи данных
        /// </summary>
        public void CopyImages(ICollection<Image> srcImages, int startIndex, int totalDataSize)
        {
            int i = 0;
            int size = 0;

            foreach (Image image in srcImages)
            {
                if (i >= startIndex)
                {
                    Images.Add(new ImageDTO(image));
                    if (image.Data != null)
                        size += image.Data.Length;
                }

                if (size >= totalDataSize)
                    break;

                i++;
            }

            EndOfImages = i == srcImages.Count;
        }
    }
}
