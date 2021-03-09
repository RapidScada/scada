/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Modified : 2021
 */

using Scada.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Form for editing image dictionary.
    /// <para>Форма редактирования словаря изображений.</para>
    /// </summary>
    internal partial class FrmImageDialog : Form
    {
        private readonly Dictionary<string, Image> images; // словарь изображений схемы
        private readonly IObservableItem observableItem;   // элемент, изменения которого отслеживаются
        private readonly CtrlSvgViewer ctrlSvgViewer;      // the control for displaying SVG


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров.
        /// </summary>
        private FrmImageDialog()
        {
            InitializeComponent();

            if (ScadaUtils.IsRunningOnWin)
            {
                ctrlSvgViewer = new CtrlSvgViewer()
                {
                    Location = pictureBox.Location,
                    Size = pictureBox.Size,
                    TabStop = false
                };
                Controls.Add(ctrlSvgViewer);
            }
            else
            {
                ctrlSvgViewer = null;
            }
        }

        /// <summary>
        /// Конструктор для режима редактирования словаря изображений.
        /// </summary>
        public FrmImageDialog(Dictionary<string, Image> images, IObservableItem observableItem)
            : this()
        {
            this.images = images ?? throw new ArgumentNullException("images");
            this.observableItem = observableItem ?? throw new ArgumentNullException("observableItem");

            SelectedImageName = "";
            CanSelectImage = false;
            ImageDir = "";

            btnSelectEmpty.Visible = false;
            btnSelect.Visible = false;
        }

        /// <summary>
        /// Конструктор для режима выбора изображения.
        /// </summary>
        public FrmImageDialog(string imageName, Dictionary<string, Image> images, IObservableItem observableItem)
            : this()
        {
            this.images = images ?? throw new ArgumentNullException("images");
            this.observableItem = observableItem ?? throw new ArgumentNullException("observableItem");

            SelectedImageName = imageName;
            CanSelectImage = true;
        }


        /// <summary>
        /// Получить признак, что форма позволяет выбирать изображение.
        /// </summary>
        public bool CanSelectImage { get; private set; }

        /// <summary>
        /// Получить наименование выбранного изображения.
        /// </summary>
        public string SelectedImageName { get; private set; }

        /// <summary>
        /// Получить или установить директорию, из которой открывались изображения.
        /// </summary>
        public string ImageDir { get; set; }


        /// <summary>
        /// Заполнить список изображений.
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
        /// Makes the appropriate image viewer visible.
        /// </summary>
        private void ChooseImageViewer(ImageListItem imageInfo)
        {
            if (ctrlSvgViewer != null)
            {
                if (imageInfo != null && imageInfo.IsSvg)
                {
                    pictureBox.Visible = false;
                    ctrlSvgViewer.Visible = true;
                }
                else
                {
                    pictureBox.Visible = true;
                    ctrlSvgViewer.Visible = false;
                }
            }
        }

        /// <summary>
        /// Установить доступность кнопок.
        /// </summary>
        private void SetBtnsEnabled()
        {
            btnSelect.Enabled = btnSave.Enabled = btnDel.Enabled = 
                lbImages.SelectedIndex >= 0;
        }

        /// <summary>
        /// Проверить уникальность наименования изображения.
        /// </summary>
        private bool ImageNameIsUnique(string name)
        {
            return !images.ContainsKey(name);
        }

        /// <summary>
        /// Загрузить изображение из файла.
        /// </summary>
        private bool LoadImage(string fileName, out Image image)
        {
            try
            {
                image = new Image();

                using (FileStream fileStream = 
                    new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    image.Data = new byte[fileStream.Length];
                    fileStream.Read(image.Data, 0, image.Data.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(SchemePhrases.LoadImageError + ":\n" + ex.Message);
                image = null;
                return false;
            }
        }

        /// <summary>
        /// Сохранить изображение в файле.
        /// </summary>
        private void SaveImage(string fileName, ImageListItem imageInfo)
        {
            try
            {
                using (FileStream fileStream = 
                    new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    fileStream.Write(imageInfo.Image.Data, 0, imageInfo.DataSize);
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(SchemePhrases.SaveImageError + ":\n" + ex.Message);
            }
        }


        private void FrmImageDialog_Load(object sender, EventArgs e)
        {
            // перевод формы
            Translator.TranslateForm(this, "Scada.Scheme.Model.PropertyGrid.FrmImageDialog");
            openFileDialog.SetFilter(SchemePhrases.ImageFileFilter);
            saveFileDialog.SetFilter(SchemePhrases.ImageFileFilter);

            // установка начальной директории изображений
            openFileDialog.InitialDirectory = ImageDir;

            // заполнение списка изображений
            FillImageList();

            // установка доступности кнопок
            SetBtnsEnabled();
        }

        private void lbImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод выбранного изображения
            ImageListItem imageInfo = lbImages.SelectedItem as ImageListItem;
            ChooseImageViewer(imageInfo);

            if (imageInfo == null)
            {
                pictureBox.Image = null;
            }
            else
            {
                try
                {
                    if (ctrlSvgViewer != null && imageInfo.IsSvg)
                    {
                        // display SVG
                        ctrlSvgViewer.ShowImage(imageInfo.Image.Data);
                    }
                    else
                    {
                        // display other type of image in the picture box
                        pictureBox.Image = imageInfo.Source;
                        pictureBox.SizeMode =
                            imageInfo.ImageSize.Width <= pictureBox.Width &&
                            imageInfo.ImageSize.Height <= pictureBox.Height ?
                            PictureBoxSizeMode.CenterImage : PictureBoxSizeMode.Zoom;
                    }
                }
                catch (Exception ex)
                {
                    ChooseImageViewer(null);
                    pictureBox.Image = null;
                    ScadaUiUtils.ShowError(SchemePhrases.DisplayImageError + ":\n" + ex.Message);
                }
            }

            propGrid.SelectedObject = imageInfo;
            SetBtnsEnabled();
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
                    ImageListItem imageInfo = propGrid.SelectedObject as ImageListItem;
                    imageInfo.Image.Name = newName;

                    // обновление словаря изображений схемы
                    images.Remove(oldName);
                    images[newName] = imageInfo.Image;

                    // обновление списка изображений на форме
                    lbImages.SelectedIndexChanged -= lbImage_SelectedIndexChanged;
                    lbImages.BeginUpdate();
                    lbImages.Items.RemoveAt(lbImages.SelectedIndex);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);
                    lbImages.EndUpdate();
                    lbImages.SelectedIndexChanged += lbImage_SelectedIndexChanged;

                    // отслеживание изменений
                    observableItem.OnItemChanged(SchemeChangeTypes.ImageRenamed, imageInfo.Image, oldName);
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // добавление изображения в словарь изображений схемы
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageDir = openFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);

                if (LoadImage(openFileDialog.FileName, out Image image))
                {
                    string name = Path.GetFileName(openFileDialog.FileName);
                    image.Name = images.ContainsKey(name) ? "image" + (images.Count + 1) : name;

                    ImageListItem imageInfo = new ImageListItem(image, ImageNameIsUnique);
                    images.Add(image.Name, image);
                    lbImages.SelectedIndex = lbImages.Items.Add(imageInfo);

                    // отслеживание изменений
                    observableItem.OnItemChanged(SchemeChangeTypes.ImageAdded, image);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение изображения в выбранный файл
            if (lbImages.SelectedItem is ImageListItem imageInfo && imageInfo.Image != null)
            {
                saveFileDialog.FileName = imageInfo.Name;
                saveFileDialog.DefaultExt = imageInfo.Format.ToLowerInvariant();

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveImage(saveFileDialog.FileName, imageInfo);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            // удаление изображения из словаря изображений схемы
            if (lbImages.SelectedItem is ImageListItem imageInfo)
            {
                int selInd = lbImages.SelectedIndex;
                images.Remove(imageInfo.Name);
                lbImages.Items.RemoveAt(selInd);
                int itemCnt = lbImages.Items.Count;
                if (itemCnt > 0)
                    lbImages.SelectedIndex = selInd < itemCnt ? selInd : itemCnt - 1;

                // отслеживание изменений
                observableItem.OnItemChanged(SchemeChangeTypes.ImageDeleted, imageInfo.Image);
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
            SelectedImageName = lbImages.SelectedItem is ImageListItem imageInfo ? imageInfo.Name : "";
            DialogResult = DialogResult.OK;
        }
    }
}
