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
 * Summary  : Form for editing image dictionary
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing image dictionary
    /// <para>Форма редактирования словаря изображений</para>
    /// </summary>
    internal partial class FrmImageDialog : Form
    {
        /// <summary>
        /// Элемент списка изображений
        /// </summary>
        private class ImageListItem //: SchemeView.ICheckNameUnique
        {
            /// <summary>
            /// Конструктор, ограничивающий создание объекта без параметров
            /// </summary>
            private ImageListItem()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public ImageListItem(Image image, Func<string, bool> nameIsUniqueMethod)
            {
                Image = image;
                Source = null;
                Name = image.Name;
                DataSize = 0;
                ImageSize = new Size(0, 0);
                Format = "";
                NameIsUniqueMethod = nameIsUniqueMethod;
            }

            /// <summary>
            /// Получить изображение
            /// </summary>
            [Browsable(false)]
            public Image Image { get; private set; }
            /// <summary>
            /// Получить данные изображения
            /// </summary>
            [Browsable(false)]
            public System.Drawing.Image Source { get; private set; }
            /// <summary>
            /// Получить или установить наименование изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Изображение"), DisplayName("Наименование")]
#else
            [Category("Image"), DisplayName("Name")]
#endif
            //[TypeConverter(typeof(SchemeView.NameConverter))]
            #endregion
            public string Name { get; set; }
            /// <summary>
            /// Получить размер данных изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Изображение"), DisplayName("Размер данных")]
#else
            [Category("Image"), DisplayName("Data size")]
#endif
            #endregion
            public int DataSize { get; private set; }
            /// <summary>
            /// Получить размер изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Изображение"), DisplayName("Размер")]
#else
            [Category("Image"), DisplayName("Size")]
#endif
            #endregion
            public Size ImageSize { get; private set; }
            /// <summary>
            /// Получить формат данных изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Изображение"), DisplayName("Формат")]
#else
            [Category("Image"), DisplayName("Format")]
#endif
            #endregion
            public string Format { get; private set; }
            /// <summary>
            /// Получить формат метод проверки уникальности наименования
            /// </summary>
            [Browsable(false)]
            public Func<string, bool> NameIsUniqueMethod { get; private set; }

            /// <summary>
            /// Загрузить изображение из его данных
            /// </summary>
            public void LoadImage()
            {
                if (Source == null && Image != null && Image.Data != null)
                {
                    // using не используется, т.к. поток memStream должен существовать одновременно с изображением
                    MemoryStream memStream = new MemoryStream(Image.Data);
                    Source = Bitmap.FromStream(memStream);
                    DataSize = (int)memStream.Length;
                    ImageSize = new Size(Source.Width, Source.Height);

                    ImageFormat imageFormat = Source.RawFormat;
                    if (imageFormat.Equals(ImageFormat.Bmp))
                        Format = "Bmp";
                    else if (imageFormat.Equals(ImageFormat.Gif))
                        Format = "Gif";
                    else if (imageFormat.Equals(ImageFormat.Jpeg))
                        Format = "Jpeg";
                    else if (imageFormat.Equals(ImageFormat.Png))
                        Format = "Png";
                    else if (imageFormat.Equals(ImageFormat.Tiff))
                        Format = "Tiff";
                }
            }
            /// <summary>
            /// Проверить уникальность наименования
            /// </summary>
            public bool NameIsUnique(string name)
            {
                return NameIsUniqueMethod == null ? true : NameIsUniqueMethod(name);
            }
            /// <summary>
            /// Получить строковое представление объекта
            /// </summary>
            public override string ToString()
            {
                return Name;
            }
        }


        private Dictionary<string, Image> images; // словарь изображений схемы
        IObservableItem observableItem;           // элемент, изменения которого отслеживаются


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmImageDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор для режима редактирования словаря изображений
        /// </summary>
        public FrmImageDialog(Dictionary<string, Image> images, IObservableItem observableItem)
            : this()
        {
            if (images == null)
                throw new ArgumentNullException("images");
            if (observableItem == null)
                throw new ArgumentNullException("observableItem");

            this.images = images;
            this.observableItem = observableItem;
            SelectedImageName = "";
            CanSelectImage = false;

            btnSelectEmpty.Visible = false;
            btnSelect.Visible = false;
        }

        /// <summary>
        /// Конструктор для режима выбора изображения
        /// </summary>
        public FrmImageDialog(string imageName, Dictionary<string, Image> images, IObservableItem observableItem)
            : this()
        {
            if (images == null)
                throw new ArgumentNullException("images");
            if (observableItem == null)
                throw new ArgumentNullException("observableItem");

            this.images = images;
            this.observableItem = observableItem;
            SelectedImageName = imageName;
            CanSelectImage = true;
        }


        /// <summary>
        /// Получить признак, что форма позволяет выбирать изображение
        /// </summary>
        public bool CanSelectImage { get; private set; }

        /// <summary>
        /// Получить наименование выбранного изображения
        /// </summary>
        public string SelectedImageName { get; private set; }


        /// <summary>
        /// Заполнить список изображений
        /// </summary>
        private void FillImageList()
        {
            try
            {
                lbImages.BeginUpdate();
                Image selImage = null;

                foreach (Image image in images.Values)
                {
                    if (image.Name == SelectedImageName)
                        selImage = image;
                    else
                        lbImages.Items.Add(new ImageListItem(image, ImageNameIsUnique));
                }

                if (selImage != null)
                    lbImages.SelectedIndex = lbImages.Items.Add(new ImageListItem(selImage, ImageNameIsUnique));
            }
            finally
            {
                lbImages.EndUpdate();
            }
        }

        /// <summary>
        /// Установить доступность кнопок
        /// </summary>
        private void SetBtnsEnabled()
        {
            btnSelect.Enabled = btnSave.Enabled = btnDel.Enabled = 
                lbImages.SelectedIndex >= 0;
        }

        /// <summary>
        /// Проверить уникальность наименования изображения
        /// </summary>
        private bool ImageNameIsUnique(string name)
        {
            return !images.ContainsKey(name);
        }


        private void FrmImageDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid.FrmImageDialog");
            //openFileDialog.Filter = saveFileDialog.Filter = "";

            // заполнение списка изображений
            FillImageList();

            // установка доступности кнопок
            SetBtnsEnabled();
        }

        private void lbImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод свойств выбранного изображения
            ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;

            if (imageInfo == null)
            {
                pictureBox.Image = null;
            }
            else
            {
                try
                {
                    imageInfo.LoadImage();
                }
                catch (Exception ex)
                {
                    //ScadaUiUtils.ShowError(SchemePhrases.LoadImageError + ":\n" + ex.Message);
                }

                if (imageInfo.Source != null)
                    pictureBox.SizeMode = imageInfo.ImageSize.Width <= pictureBox.Width &&
                        imageInfo.ImageSize.Height <= pictureBox.Height ?
                        PictureBoxSizeMode.CenterImage : PictureBoxSizeMode.Zoom;

                pictureBox.Image = imageInfo.Source;
            }

            propGrid.SelectedObject = imageInfo;
            btnSelect.Enabled = btnSave.Enabled = btnDel.Enabled = imageInfo != null;
        }

        private void lbImage_DoubleClick(object sender, EventArgs e)
        {
            if (btnSelect.Visible)
                btnSelect_Click(null, null);
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // обновление наименования изображения
            if (e.ChangedItem.PropertyDescriptor.Name == "Name")
            {
                string oldName = (string)e.OldValue;
                string newName = (string)e.ChangedItem.Value;

                if (oldName != newName)
                {
                    // изменение наименования изображения
                    ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;
                    imageInfo.Name = newName;
                    imageInfo.Image.Name = newName;

                    // обновление словаря изображений схемы
                    images.Remove(oldName);
                    images.Add(newName, imageInfo.Image);

                    // обновление списка изображений на форме
                    lbImages.SelectedIndexChanged -= lbImage_SelectedIndexChanged;
                    lbImages.BeginUpdate();

                    lbImages.Items.RemoveAt(lbImages.SelectedIndex);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);
                    
                    lbImages.EndUpdate();
                    lbImages.SelectedIndexChanged += lbImage_SelectedIndexChanged;

                    // создание объекта для передачи изменений
                    //SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageRenamed);
                    //change.ImageOldName = oldName;
                    //change.ImageNewName = newName;
                    //editorData.TrySetSchemeChange(change);
                    //editorData.SetFormTitle();
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // добавление изображения в словарь изображений схемы
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image image = new Image();
                    string name = Path.GetFileName(openFileDialog.FileName);
                    image.Name = images.ContainsKey(name) ? "image" + (images.Count + 1) : name;

                    using (FileStream fileStream = new FileStream(
                        openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        image.Data = new byte[fileStream.Length];
                        fileStream.Read(image.Data, 0, image.Data.Length);
                    }

                    ImageListItem imageInfo = new ImageListItem(image, ImageNameIsUnique);
                    images.Add(image.Name, image);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);

                    // создание объекта для передачи изменений
                    //SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageAdded);
                    //change.Image = image;
                    //editorData.TrySetSchemeChange(change);
                    //editorData.SetFormTitle();
                }
                catch (Exception ex)
                {
                    //ScadaUiUtils.ShowError(SchemePhrases.LoadImageError + ":\n" + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение изображения в выбранный файл
            ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;

            if (imageInfo != null && imageInfo.Image != null)
            {
                saveFileDialog.FileName = imageInfo.Name;
                saveFileDialog.DefaultExt = imageInfo.Format.ToLowerInvariant();

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(
                            saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            fileStream.Write(imageInfo.Image.Data, 0, imageInfo.DataSize);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ScadaUiUtils.ShowError(SchemePhrases.SaveImageError + ":\n" + ex.Message);
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // удаление изображения из словаря изображений схемы
            ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;
            int selInd = lbImages.SelectedIndex;

            if (imageInfo != null)
            {
                images.Remove(imageInfo.Name);
                lbImages.Items.RemoveAt(selInd);
                int itemCnt = lbImages.Items.Count;
                if (itemCnt > 0)
                    lbImages.SelectedIndex = selInd < itemCnt ? selInd : itemCnt - 1;

                // создание объекта для передачи изменений
                //SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageDeleted);
                //change.ImageOldName = imageInfo.Name;
                //editorData.TrySetSchemeChange(change);
                //editorData.SetFormTitle();
            }

            propGrid.Select();
        }

        private void btnSelectEmpty_Click(object sender, EventArgs e)
        {
            // выбор пустого изображения
            SelectedImageName = "";
            DialogResult = DialogResult.OK;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // установка выбранного изображения
            ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;
            SelectedImageName = imageInfo.Name;
            DialogResult = DialogResult.OK;
        }
    }
}
