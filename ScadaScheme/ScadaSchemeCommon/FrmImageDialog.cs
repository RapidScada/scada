/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Editing image dictionary form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Scada.Client;

namespace Scada.Scheme
{
    /// <summary>
    /// Editing image dictionary form
    /// <para>Форма редактирования словаря изображений</para>
    /// </summary>
    public partial class FrmImageDialog : Form
    {
        /// <summary>
        /// Информация об изображении
        /// </summary>
        private class ImageInfo : SchemeView.ICheckNameUnique
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ImageInfo(SchemeView.Image image, Func<string, bool> nameIsUniqueMethod)
            {
                Image = image;
                Source = null;
                Name = image.Name;
                DataSize = 0;
                ImageSize = new SchemeView.Size(0, 0);
                Format = "";
                NameIsUniqueMethod = nameIsUniqueMethod;
            }

            /// <summary>
            /// Получить изображение
            /// </summary>
            [Browsable(false)]
            public SchemeView.Image Image { get; private set; }
            /// <summary>
            /// Получить данные изображения
            /// </summary>
            [Browsable(false)]
            public Image Source { get; private set; }
            /// <summary>
            /// Получить или установить наименование изображения
            /// </summary>
            #region Attributes
#if USE_RUSSIAN
            [Category("Изображение"), DisplayName("Наименование")]
#else
            [Category("Image"), DisplayName("Name")]
#endif
            [TypeConverter(typeof(SchemeView.NameConverter))]
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
            public SchemeView.Size ImageSize { get; private set; }
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
                    ImageSize = new SchemeView.Size(Source.Width, Source.Height);

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


        private EditorData editorData;                          // ссылка на данные редактора схем
        private Dictionary<string, SchemeView.Image> imageDict; // ссылка на словарь изображений схемы


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmImageDialog()
        {
            InitializeComponent();
            imageDict = null;
            CanSelectImage = true;
            SelectedImage = null;
        }


        /// <summary>
        /// Получить или установить признак, возможно ли с помощью формы выбирать изображение
        /// </summary>
        public bool CanSelectImage { get; set; }

        /// <summary>
        /// Получить или установить выбранное изображение
        /// </summary>
        public SchemeView.Image SelectedImage { get; set; }


        /// <summary>
        /// Проверить уникальность наименования изображения
        /// </summary>
        private bool ImageNameIsUnique(string name)
        {
            return !imageDict.ContainsKey(name);
        }

        /// <summary>
        /// Начать ожидание считывания изменения
        /// </summary>
        private void StartWaitForChange()
        {
            btnOpen.Enabled = btnDel.Enabled = btnClear.Enabled = btnSelect.Enabled = false;
            tmrAllowChange.Enabled = true;
        }


        private void FrmImageDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Localization.TranslateForm(this, "Scada.Scheme.FrmImageDialog");

            // установка видимости кнопок выбора изображения
            if (!CanSelectImage)
            {
                btnClear.Visible = false;
                btnSelect.Visible = false;
            }

            // вывод словаря изображений схемы
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            editorData = schemeApp.EditorData;
            if (editorData != null && editorData.SchemeView != null)
                imageDict = schemeApp.EditorData.SchemeView.ImageDict;

            if (imageDict != null)
            {
                lbImages.BeginUpdate();
                string selName = SelectedImage == null ? "" : SelectedImage.Name;
                SchemeView.Image selImage = null;

                foreach (SchemeView.Image image in imageDict.Values)
                {
                    if (image.Name == selName)
                        selImage = image;
                    else
                        lbImages.Items.Add(new ImageInfo(image, ImageNameIsUnique));
                }

                if (selImage != null)
                    lbImages.SelectedIndex = lbImages.Items.Add(new ImageInfo(selImage, ImageNameIsUnique));

                lbImages.EndUpdate();
                btnOpen.Enabled = true;
            }
        }

        private void lbImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод свойств выбранного изображения
            ImageInfo imageInfo = lbImages.SelectedItem as ImageInfo;

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
                    ScadaUtils.ShowError(SchemePhrases.LoadImageError + ":\n" + ex.Message);
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
                    ImageInfo imageInfo = lbImages.SelectedItem as ImageInfo;
                    imageInfo.Name = newName;
                    imageInfo.Image.Name = newName;

                    // обновление словаря изображений схемы
                    imageDict.Remove(oldName);
                    imageDict.Add(newName, imageInfo.Image);

                    // обновление списка изображений на форме
                    lbImages.SelectedIndexChanged -= lbImage_SelectedIndexChanged;
                    lbImages.BeginUpdate();

                    lbImages.Items.RemoveAt(lbImages.SelectedIndex);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);
                    
                    lbImages.EndUpdate();
                    lbImages.SelectedIndexChanged += lbImage_SelectedIndexChanged;

                    // создание объекта для передачи изменений
                    SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageRenamed);
                    change.ImageOldName = oldName;
                    change.ImageNewName = newName;
                    editorData.TrySetSchemeChange(change);
                    editorData.SetFormTitle();
                    StartWaitForChange();
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
                    SchemeView.Image image = new SchemeView.Image();
                    string name = Path.GetFileName(openFileDialog.FileName);
                    image.Name = imageDict.ContainsKey(name) ? "image" + (imageDict.Count + 1) : name;

                    using (FileStream fileStream = new FileStream(
                        openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        image.Data = new byte[fileStream.Length];
                        fileStream.Read(image.Data, 0, image.Data.Length);
                    }

                    ImageInfo imageInfo = new ImageInfo(image, ImageNameIsUnique);
                    imageDict.Add(image.Name, image);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);

                    // создание объекта для передачи изменений
                    SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageAdded);
                    change.Image = image;
                    editorData.TrySetSchemeChange(change);
                    editorData.SetFormTitle();
                    StartWaitForChange();
                }
                catch (Exception ex)
                {
                    ScadaUtils.ShowError(SchemePhrases.LoadImageError + ":\n" + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение изображения в выбранный файл
            ImageInfo imageInfo = lbImages.SelectedItem as ImageInfo;

            if (imageInfo != null && imageInfo.Image != null)
            {
                saveFileDialog.FileName = imageInfo.Name;
                saveFileDialog.DefaultExt = imageInfo.Format.ToLower();

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
                        ScadaUtils.ShowError(SchemePhrases.SaveImageError + ":\n" + ex.Message);
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // удаление изображения из словаря изображений схемы
            ImageInfo imageInfo = lbImages.SelectedItem as ImageInfo;
            int selInd = lbImages.SelectedIndex;

            if (imageInfo != null)
            {
                imageDict.Remove(imageInfo.Name);
                lbImages.Items.RemoveAt(selInd);
                int itemCnt = lbImages.Items.Count;
                if (itemCnt > 0)
                    lbImages.SelectedIndex = selInd < itemCnt ? selInd : itemCnt - 1;

                // создание объекта для передачи изменений
                SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ImageDeleted);
                change.ImageOldName = imageInfo.Name;
                editorData.TrySetSchemeChange(change);
                editorData.SetFormTitle();
                StartWaitForChange();
            }

            propGrid.Select();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // установка выбранного изображения пустым
            SelectedImage = null;
            DialogResult = DialogResult.OK;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // установка выбранного изображения
            ImageInfo imageInfo = lbImages.SelectedItem as ImageInfo;
            if (imageInfo != null)
            {
                SelectedImage = new SchemeView.Image();
                SelectedImage.Name = imageInfo.Name;
            }
            DialogResult = DialogResult.OK;
        }

        private void tmrAllowChange_Tick(object sender, EventArgs e)
        {
            if (editorData.SchemeChange == null)
            {
                tmrAllowChange.Enabled = false;
                btnOpen.Enabled = btnDel.Enabled = btnClear.Enabled = btnSelect.Enabled = true;
            }
        }
    }
}
