/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : The class contains utility methods for the whole system. UI utilities
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada
{
    partial class ScadaUtils
    {
        /// <summary>
        /// Размер отображаемых данных журналов, 10 КБ
        /// </summary>
        private const long LogViewSize = 10240;
        /// <summary>
        /// Порог количества строк в таблице для выбора режима автоподбора ширины столбцов
        /// </summary>
        private const int GridAutoResizeBoundary = 100;
        /// <summary>
        /// Адрес русскоязычного сайта проекта
        /// </summary>
        private const string WebsiteRu = "http://rapidscada.ru";
        /// <summary>
        /// Адрес англоязычного сайта проекта
        /// </summary>
        private const string WebsiteEn = "http://rapidscada.org";


        /// <summary>
        /// Показать информационное сообщение
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, CommonPhrases.InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Показать сообщение об ошибке
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void ShowError(string message)
        {
            MessageBox.Show(message, CommonPhrases.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Показать предупреждение
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Установить значение элемента управления типа NumericUpDown в пределах допустимого диапазона
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void SetNumericValue(this NumericUpDown num, decimal val)
        {
            if (val < num.Minimum)
                num.Value = num.Minimum;
            else if (val > num.Maximum)
                num.Value = num.Maximum;
            else
                num.Value = val;
        }

        /// <summary>
        /// Автоподбор ширины столбцов таблицы с выбором режима в зависимости от количества строк
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void AutoResizeColumns(this DataGridView dataGridView)
        {
            if (dataGridView.RowCount <= GridAutoResizeBoundary)
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            else
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        /// <summary>
        /// Загрузить из файлов изображение и гиперссылку для формы о программе
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static bool LoadAboutForm(string exeDir, Form frmAbout, PictureBox pictureBox, Label lblLink,
            out bool imgLoaded, out string linkUrl, out string errMsg)
        {
            imgLoaded = false;
            linkUrl = Localization.UseRussian ? WebsiteRu : WebsiteEn;
            errMsg = "";

            // загрузка заставки из файла, если он существует
            try
            {
                string imgFileName = exeDir + "About.jpg";
                if (File.Exists(imgFileName))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imgFileName);
                    pictureBox.Image = image;
                    imgLoaded = true;

                    // проверка, корректировка и установка размеров формы и изображения
                    int width;
                    if (image.Width < 100) 
                        width = 100;
                    else if (image.Width > 800) 
                        width = 800;
                    else 
                        width = image.Width;

                    int height;
                    if (image.Height < 100) 
                        height = 100;
                    else if (image.Height > 600) 
                        height = 600;
                    else 
                        height = image.Height;

                    frmAbout.Width = pictureBox.Width = width;
                    frmAbout.Height = pictureBox.Height = height;
                }
            }
            catch (OutOfMemoryException)
            {
                errMsg = string.Format(CommonPhrases.LoadImageError, CommonPhrases.IncorrectFileFormat);
            }
            catch (Exception ex)
            {
                errMsg = string.Format(CommonPhrases.LoadImageError, ex.Message);
            }

            if (errMsg == "")
            {
                // загрузка гиперссылки из файла, если он существует
                StreamReader reader = null;
                try
                {
                    string linkFileName = exeDir + "About.txt";
                    if (File.Exists(linkFileName))
                    {
                        reader = new StreamReader(linkFileName, Encoding.Default);
                        linkUrl = reader.ReadLine();

                        if (string.IsNullOrEmpty(linkUrl))
                        {
                            lblLink.Visible = false;
                        }
                        else
                        {
                            linkUrl = linkUrl.Trim();
                            string pos = reader.ReadLine();

                            if (!string.IsNullOrEmpty(pos))
                            {
                                string[] parts = pos.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                int x, y, w, h;

                                if (parts.Length >= 4 && int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y) &&
                                    int.TryParse(parts[2], out w) && int.TryParse(parts[3], out h))
                                {
                                    // проверка положения и размеров
                                    if (x < 0) 
                                        x = 0;
                                    else if (x >= frmAbout.Width) 
                                        x = frmAbout.Width - 1;
                                    if (y < 0) 
                                        y = 0;
                                    else if (y >= frmAbout.Height) 
                                        y = frmAbout.Height - 1;

                                    if (x + w >= frmAbout.Width) 
                                        w = frmAbout.Width - x;
                                    if (w <= 0) 
                                        w = 1;
                                    if (y + h >= frmAbout.Height) 
                                        h = frmAbout.Height - y;
                                    if (h <= 0) 
                                        h = 1;

                                    lblLink.Left = x;
                                    lblLink.Top = y;
                                    lblLink.Width = w;
                                    lblLink.Height = h;
                                    lblLink.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    linkUrl = "";
                    lblLink.Visible = false;
                    errMsg = string.Format(CommonPhrases.LoadHyperlinkError, ex.Message);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
                lblLink.Visible = false;
            }

            return errMsg == "";
        }

        /// <summary>
        /// Загрузить строки из файла
        /// </summary>
        /// <remarks>Если fullLoad равен false, то объём загружаемых данных не более LogViewSize</remarks>
        private static List<string> LoadStrings(string fileName, bool fullLoad)
        {
            using (FileStream fileStream =
                new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                List<string> stringList = new List<string>();
                long fileSize = fileStream.Length;
                long dataSize = fullLoad ? fileSize : LogViewSize;
                long filePos = fileSize - dataSize;
                if (filePos < 0)
                    filePos = 0;

                if (fileStream.Seek(filePos, SeekOrigin.Begin) == filePos)
                {
                    using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        bool addLine = fileSize <= dataSize;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (addLine)
                                stringList.Add(line);
                            else
                                addLine = true;
                        }
                    }
                }

                return stringList;
            }
        }

        /// <summary>
        /// Обновить список строк, загрузив данные из файла
        /// </summary>
        [Obsolete("Use ScadaUiUtils class")]
        public static void RefreshListBox(this ListBox listBox, string fileName, bool fullLoad,
            ref DateTime fileAge)
        {
            Monitor.Enter(listBox);

            try
            {
                if (File.Exists(fileName))
                {
                    DateTime newFileAge = GetLastWriteTime(fileName);

                    if (fileAge != newFileAge)
                    {
                        // загрузка строк из файла
                        List<string> stringList = LoadStrings(fileName, fullLoad);
                        int newLineCnt = stringList.Count;

                        // проверка для исключения отображения данных, считыванных в момент записи файла
                        if (newLineCnt > 0 || (DateTime.Now - newFileAge).TotalMilliseconds > 50)
                        {
                            fileAge = newFileAge;

                            // вывод данных в список
                            int oldLineCnt = listBox.Items.Count;
                            int selectedIndex = listBox.SelectedIndex;
                            int topIndex = listBox.TopIndex;

                            listBox.BeginUpdate();

                            for (int i = 0; i < newLineCnt; i++)
                            {
                                if (i < oldLineCnt)
                                    listBox.Items[i] = stringList[i];
                                else
                                    listBox.Items.Add(stringList[i]);
                            }

                            for (int i = newLineCnt; i < oldLineCnt; i++)
                                listBox.Items.RemoveAt(newLineCnt);

                            // установка позиции прокрутки списка
                            if (listBox.SelectionMode == SelectionMode.One && newLineCnt > 0)
                            {
                                if (selectedIndex < 0 && !fullLoad)
                                    listBox.SelectedIndex = newLineCnt - 1; // прокрутка в конец списка
                                else
                                    listBox.TopIndex = topIndex;
                            }

                            listBox.EndUpdate();
                        }
                    }
                }
                else
                {
                    if (listBox.Items.Count == 1)
                    {
                        listBox.Items[0] = CommonPhrases.NoData;
                    }
                    else
                    {
                        listBox.Items.Clear();
                        listBox.Items.Add(CommonPhrases.NoData);
                    }
                    fileAge = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                if (listBox.Items.Count == 2)
                {
                    listBox.Items[0] = CommonPhrases.ErrorWithColon;
                    listBox.Items[1] = ex.Message;
                }
                else
                {
                    listBox.Items.Clear();
                    listBox.Items.Add(CommonPhrases.ErrorWithColon);
                    listBox.Items.Add(ex.Message);
                }
                fileAge = DateTime.MinValue;
            }
            finally
            {
                Monitor.Exit(listBox);
            }
        }
    }
}
