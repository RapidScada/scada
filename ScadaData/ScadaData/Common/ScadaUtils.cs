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
 * Summary  : The class contains utility methods for the whole system
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using WinForms = System.Windows.Forms;

namespace Scada
{
    /// <summary>
    /// The class contains utility methods for the whole system
    /// <para>Класс, содержащий вспомогательные методы для всей системы</para>
    /// </summary>
    public static partial class ScadaUtils
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
        /// Длительность хранения данных в cookies
        /// </summary>
        public static readonly TimeSpan CookieExpiration = TimeSpan.FromDays(30);

        private static NumberFormatInfo nfi; // формат вещественных чисел


        /// <summary>
        /// Конструктор
        /// </summary>
        static ScadaUtils()
        {
            nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        }


        /// <summary>
        /// Добавить "\" к имени директории, если необходимо
        /// </summary>
        public static string NormalDir(string dir)
        {
            dir = dir == null ? "" : dir.Trim();
            if (dir.Length > 0 && dir[dir.Length - 1] != '\\') dir += @"\";
            return dir;
        }

        /// <summary>
        /// Определить, является ли заданная строка записью даты, используя Localization.Culture
        /// </summary>
        public static bool StrIsDate(string s)
        {
            DateTime dateTime;
            return DateTime.TryParse(s, Localization.Culture, DateTimeStyles.None, out dateTime) ?
                dateTime.TimeOfDay.TotalMilliseconds == 0 : false;
        }

        /// <summary>
        /// Преобразовать строку в дату, используя Localization.Culture
        /// </summary>
        public static DateTime StrToDate(string s)
        {
            DateTime dateTime;
            if (DateTime.TryParse(s, Localization.Culture, DateTimeStyles.None, out dateTime))
                return dateTime.Date;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Преобразовать строку в вещественное число
        /// </summary>
        /// <remarks>Метод работает с разделителями целой части '.' и ','.
        /// Если преобразование невозможно, возвращается double.NaN</remarks>
        public static double StrToDouble(string s)
        {
            try { return ParseDouble(s); }
            catch { return double.NaN; }
        }

        /// <summary>
        /// Преобразовать строку в вещественное число. Метод работает с разделителями целой части '.' и ','
        /// </summary>
        /// <remarks>Если преобразование невозможно, вызывается исключение FormatException</remarks>
        public static double StrToDoubleExc(string s)
        {
            try { return ParseDouble(s); }
            catch { throw new FormatException(string.Format(CommonPhrases.NotNumber, s)); }
        }

        /// <summary>
        /// Преобразовать строку в вещественное число. Метод работает с разделителями целой части '.' и ','
        /// </summary>
        public static double ParseDouble(string s)
        {
            nfi.NumberDecimalSeparator = s.Contains(".") ? "." : ",";
            return double.Parse(s, nfi);
        }
        
        /// <summary>
        /// Преобразовать массив байт в строку на основе 16-ричного представления
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            int len = bytes == null ? 0 : bytes.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// Преобразовать строку 16-ричных чисел в массив байт
        /// </summary>
        public static bool HexToBytes(string s, out byte[] bytes)
        {
            int len = s == null ? 0 : s.Length;

            if (len > 0 && len % 2 == 0)
            {
                bool parseOk = true;
                bytes = new byte[len / 2];

                for (int i = 0, j = 0; i < len && parseOk; i += 2, j++)
                {
                    try { bytes[j] = (byte)int.Parse(s.Substring(i, 2), NumberStyles.HexNumber); }
                    catch { parseOk = false; }
                }

                return parseOk;
            }
            else
            {
                bytes = new byte[0];
                return false;
            }
        }
        
        /// <summary>
        /// Вычислить хеш-функцию MD5
        /// </summary>
        public static string ComputeHash(string s)
        {
            return BytesToHex(MD5.Create().ComputeHash(Encoding.Default.GetBytes(s)));
        }
        
        /// <summary>
        /// Глубокое (полное) клонирование объекта
        /// </summary>
        /// <remarks>Все клонируемые объекты должны иметь атрибут Serializable</remarks>
        public static object DeepClone(object obj, SerializationBinder binder = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (binder != null)
                    bf.Binder = binder;
                bf.Serialize(ms, obj);

                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Получить время последней записи в файл
        /// </summary>
        public static DateTime GetLastWriteTime(string fileName)
        {
            try { return File.GetLastWriteTime(fileName); }
            catch { return DateTime.MinValue; }
        }



        /// <summary>
        /// Показать информационное сообщение
        /// </summary>
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, CommonPhrases.InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Показать сообщение об ошибке
        /// </summary>
        public static void ShowError(string message)
        {
            MessageBox.Show(message, CommonPhrases.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Показать предупреждение
        /// </summary>
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Установить значение элемента управления типа NumericUpDown в пределах допустимого диапазона
        /// </summary>
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
        public static bool LoadAboutForm(string imgFileName, string linkFileName,
            Form frmAbout, PictureBox pictureBox, WinForms.Label lblLink, 
            out string link, out string errMsg)
        {
            errMsg = "";
            link = "";

            // загрузка заставки из файла, если он существует
            try
            {
                if (File.Exists(imgFileName))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imgFileName);
                    pictureBox.Image = image;

                    // проверка, корректировка и установка размеров формы и изображения
                    int width;
                    if (image.Width < 100) width = 100;
                    else if (image.Width > 800) width = 800;
                    else width = image.Width;

                    int height;
                    if (image.Height < 100) height = 100;
                    else if (image.Height > 600) height = 600;
                    else height = image.Height;

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
                    if (File.Exists(linkFileName))
                    {
                        reader = new StreamReader(linkFileName, Encoding.Default);
                        link = reader.ReadLine();

                        if (!string.IsNullOrEmpty(link))
                        {
                            link = link.Trim();
                            string pos = reader.ReadLine();

                            if (!string.IsNullOrEmpty(pos))
                            {
                                string[] parts = pos.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                int x, y, w, h;

                                if (parts.Length >= 4 && int.TryParse(parts[0], out x) && int.TryParse(parts[1], out y) &&
                                    int.TryParse(parts[2], out w) && int.TryParse(parts[3], out h))
                                {
                                    // проверка положения и размеров
                                    if (x < 0) x = 0;
                                    else if (x >= frmAbout.Width) x = frmAbout.Width - 1;
                                    if (y < 0) y = 0;
                                    else if (y >= frmAbout.Height) y = frmAbout.Height - 1;

                                    if (x + w >= frmAbout.Width) w = frmAbout.Width - x;
                                    if (w <= 0) w = 1;
                                    if (y + h >= frmAbout.Height) h = frmAbout.Height - y;
                                    if (h <= 0) h = 1;

                                    lblLink.Left = x;
                                    lblLink.Top = y;
                                    lblLink.Width = w;
                                    lblLink.Height = h;
                                    lblLink.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblLink.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    link = "";
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
        public static void RefreshListBox(this WinForms.ListBox listBox, string fileName, bool fullLoad, 
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
                            if (listBox.SelectionMode == WinForms.SelectionMode.One && newLineCnt > 0)
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
        /// Отключить кэширование страницы
        /// </summary>
        [Obsolete("Use Scada.Web.ScadaWebUtils")]
        public static void DisablePageCache(HttpResponse response)
        {
            if (response != null)
            {
                response.AppendHeader("Pragma", "No-cache");
                response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0");
            }
        }

        /// <summary>
        /// Преобразовать строку для вывода на веб-страницу, заменив "\n" на тег "br"
        /// </summary>
        [Obsolete("Use Scada.Web.ScadaWebUtils")]
        public static string HtmlEncodeWithBreak(string s)
        {
            return HttpUtility.HtmlEncode(s).Replace("\n", "<br />");
        }
    }
}