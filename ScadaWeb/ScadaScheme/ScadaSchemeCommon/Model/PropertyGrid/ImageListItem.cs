﻿/*
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
 * Summary  : Image list item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Image list item
    /// <para>Элемент списка изображений</para>
    /// </summary>
    public class ImageListItem : IUniqueItem
    {
        private const string ImageCat = "Image";            // наименование категории изображения
        private Func<string, bool> imageNameIsUniqueMethod; // метод проверки наименования на уникальность
        private System.Drawing.Image source;                // источник данных изображения

        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private ImageListItem()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ImageListItem(Image image, Func<string, bool> imageNameIsUniqueMethod)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (imageNameIsUniqueMethod == null)
                throw new ArgumentNullException("imageNameIsUniqueMethod");

            this.imageNameIsUniqueMethod = imageNameIsUniqueMethod;
            source = null;

            Image = image;
            Name = image.Name;
            DataSize = 0;
            ImageSize = Size.Empty;
            Format = "";
        }


        /// <summary>
        /// Получить изображение
        /// </summary>
        #region Attributes
        [Browsable(false)]
        #endregion
        public Image Image { get; private set; }

        /// <summary>
        /// Получить источник данных изображения
        /// </summary>
        #region Attributes
        [Browsable(false)]
        #endregion
        public System.Drawing.Image Source
        {
            get
            {
                if (source == null)
                    LoadImage();
                return source;
            }
        }

        /// <summary>
        /// Получить или установить наименование изображения
        /// </summary>
        #region Attributes
        [DisplayName("Name"), Category(ImageCat)]
        [TypeConverter(typeof(UniqueStringConverter))]
        #endregion
        public string Name { get; set; }

        /// <summary>
        /// Получить размер данных изображения
        /// </summary>
        #region Attributes
        [DisplayName("Data size"), Category(ImageCat)]
        #endregion
        public int DataSize { get; private set; }

        /// <summary>
        /// Получить размер изображения
        /// </summary>
        #region Attributes
        [DisplayName("Size"), Category(ImageCat)]
        #endregion
        public Size ImageSize { get; private set; }

        /// <summary>
        /// Получить формат данных изображения
        /// </summary>
        #region Attributes
        [DisplayName("Format"), Category(ImageCat)]
        #endregion
        public string Format { get; private set; }


        /// <summary>
        /// Загрузить изображение из его данных
        /// </summary>
        private void LoadImage()
        {
            if (Image == null || Image.Data == null)
            {
                source = null;
            }
            else
            {
                // using не используется, т.к. поток memStream должен существовать одновременно с изображением
                MemoryStream memStream = new MemoryStream(Image.Data);
                source = System.Drawing.Image.FromStream(memStream);
                DataSize = (int)memStream.Length;
                ImageSize = new Size(Source.Width, Source.Height);

                ImageFormat imageFormat = Source.RawFormat;
                if (imageFormat.Equals(ImageFormat.Gif))
                    Format = "Gif";
                else if (imageFormat.Equals(ImageFormat.Jpeg))
                    Format = "Jpeg";
                else if (imageFormat.Equals(ImageFormat.Png))
                    Format = "Png";
                else
                    Format = "";
            }
        }

        /// <summary>
        /// Проверить наименование на уникальность
        /// </summary>
        public bool KeyIsUnique(string key)
        {
            return imageNameIsUniqueMethod(key);
        }

        /// <summary>
        /// Получить строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
