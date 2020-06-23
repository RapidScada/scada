/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : The class contains user interface utility methods for the whole system
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// The class contains user interface utility methods for the whole system.
    /// <para>Класс, содержащий вспомогательные методы работы с пользовательским интерфейсом для всей системы.</para>
    /// </summary>
    public static class ScadaUiUtils
    {
        /// <summary>
        /// Размер отображаемых данных журналов, КБ.
        /// </summary>
        private const long LogViewSize = 10240;
        /// <summary>
        /// Порог количества строк в таблице для выбора режима автоподбора ширины столбцов.
        /// </summary>
        private const int GridAutoResizeBoundary = 100;
        /// <summary>
        /// The maximum column width in DataGridView in pixels.
        /// </summary>
        private const int MaxColumnWidth = 500;
        /// <summary>
        /// Адрес русскоязычного сайта проекта.
        /// </summary>
        private const string WebsiteRu = "http://rapidscada.ru";
        /// <summary>
        /// Адрес англоязычного сайта проекта.
        /// </summary>
        private const string WebsiteEn = "http://rapidscada.org";
        
        /// <summary>
        /// The log refresh interval on local connection, ms.
        /// </summary>
        public const int LogLocalRefreshInterval = 500;
        /// <summary>
        /// The log refresh interval on remote connection, ms.
        /// </summary>
        public const int LogRemoteRefreshInterval = 1000;
        /// <summary>
        /// The log refresh timer interval when the form is hidden, ms.
        /// </summary>
        public const int LogInactiveTimerInterval = 10000;


        /// <summary>
        /// Загрузить строки из файла.
        /// </summary>
        /// <remarks>Если fullLoad равен false, то объём загружаемых данных не более LogViewSize.</remarks>
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
        /// Показать информационное сообщение.
        /// </summary>
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message?.Trim(), CommonPhrases.InfoCaption, 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Показать сообщение об ошибке.
        /// </summary>
        public static void ShowError(string message)
        {
            MessageBox.Show(message?.Trim(), CommonPhrases.ErrorCaption, 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Показать предупреждение.
        /// </summary>
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message?.Trim(), CommonPhrases.WarningCaption, 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        /// <summary>
        /// Установить значение элемента управления типа NumericUpDown в пределах его допустимого диапазона.
        /// </summary>
        public static void SetValue(this NumericUpDown num, decimal val)
        {
            if (val < num.Minimum)
                num.Value = num.Minimum;
            else if (val > num.Maximum)
                num.Value = num.Maximum;
            else
                num.Value = val;
        }

        /// <summary>
        /// Установить время элемента управления типа DateTimePicker.
        /// </summary>
        public static void SetTime(this DateTimePicker picker, DateTime time)
        {
            DateTime date = picker.MinDate;
            picker.Value = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        /// <summary>
        /// Установить время элемента управления типа DateTimePicker.
        /// </summary>
        public static void SetTime(this DateTimePicker picker, TimeSpan timeSpan)
        {
            DateTime date = picker.MinDate;
            picker.Value = (new DateTime(date.Year, date.Month, date.Day)).Add(timeSpan);
        }

        /// <summary>
        /// Установить выбранный элемент выпадающего списка, 
        /// используя карту соответствия значений и индексов элементов списка.
        /// </summary>
        public static void SetSelectedItem(this ComboBox comboBox, object value,
            Dictionary<string, int> valueToItemIndex, int defaultIndex = -1)
        {
            string valStr = value.ToString();
            if (valueToItemIndex.ContainsKey(valStr))
                comboBox.SelectedIndex = valueToItemIndex[valStr];
            else if (defaultIndex >= 0)
                comboBox.SelectedIndex = defaultIndex;
        }

        /// <summary>
        /// Получить выбранный элемент выпадающего списка, 
        /// используя карту соответствия индексов элементов списка и значений.
        /// </summary>
        public static object GetSelectedItem(this ComboBox comboBox, Dictionary<int, object> itemIndexToValue)
        {
            if (itemIndexToValue.TryGetValue(comboBox.SelectedIndex, out object val))
                return val;
            else
                throw new InvalidOperationException("Unable to find combo box selected index in the dictionary.");
        }

        /// <summary>
        /// Sets the file dialog filter suppressing possible exception.
        /// </summary>
        public static void SetFilter(this FileDialog fileDialog, string filter)
        {
            try { fileDialog.Filter = filter; }
            catch { }
        }

        /// <summary>
        /// Распознать текст элемента управления и пробразовать его в значение перечислимого типа.
        /// </summary>
        public static T ParseText<T>(this Control control) where T : struct
        {
            if (Enum.TryParse<T>(control.Text, true, out T val))
                return val;
            else
                throw new FormatException("Unable to parse text of the control.");
        }

        /// <summary>
        /// Автоподбор ширины столбцов таблицы с выбором режима в зависимости от количества строк.
        /// </summary>
        public static void AutoSizeColumns(this DataGridView dataGridView)
        {
            if (dataGridView.RowCount <= GridAutoResizeBoundary)
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            else
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Width > MaxColumnWidth)
                    column.Width = MaxColumnWidth;
            }
        }

        /// <summary>
        /// Загрузить новые данные в элемент списка из файла.
        /// </summary>
        [Obsolete("Use LogBox")]
        public static void ReloadItems(this ListBox listBox, string fileName, bool fullLoad,
            ref DateTime fileAge)
        {
            Monitor.Enter(listBox);

            try
            {
                if (File.Exists(fileName))
                {
                    DateTime newFileAge = ScadaUtils.GetLastWriteTime(fileName);

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

        /// <summary>
        /// Tests whether the specified area is visible on any of the available screens.
        /// </summary>
        public static bool AreaIsVisible(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Bounds.IntersectsWith(rect))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Загрузить из файлов изображение и гиперссылку для формы о программе.
        /// </summary>
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

                                if (parts.Length >= 4 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y) &&
                                    int.TryParse(parts[2], out int w) && int.TryParse(parts[3], out int h))
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
        /// Получить ссылку на онлайн генератор ключей.
        /// </summary>
        public static string GetKeyGenUrl(string prod, bool trial, bool? useRussian = null)
        {
            if (useRussian ?? Localization.UseRussian)
            {
                return trial ?
                    "https://rapidscada.net/trial/?prod=" + prod + "&lang=ru" :
                    "https://rapidscada.ru/download-all-files/purchase-module/";
            }
            else
            {
                return trial ?
                    "https://rapidscada.net/trial/?prod=" + prod :
                    "https://rapidscada.org/download-all-files/purchase-module/";
            }
        }
    }
}
