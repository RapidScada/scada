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
 * Module   : ScadaData
 * Summary  : Provides displaying and updating log.
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// Provides displaying and updating log.
    /// <para>Обеспечивает отображение и обновление журнала.</para>
    /// </summary>
    public class LogBox
    {
        /// <summary>
        /// The default size of displayed part of a log in bytes.
        /// </summary>
        protected const int DefaultLogViewSize = 10240;
        /// <summary>
        /// The brush for sent data.
        /// </summary>
        protected static readonly Brush SentDataBrush = new SolidBrush(Color.Blue);
        /// <summary>
        /// The brush for received data.
        /// </summary>
        protected static readonly Brush ReceivedDataBrush = new SolidBrush(Color.Purple);
        /// <summary>
        /// The brush for acknowledgement.
        /// </summary>
        protected static readonly Brush AckBrush = new SolidBrush(Color.Green);
        /// <summary>
        /// The brush for error.
        /// </summary>
        protected static readonly Brush ErrorBrush = new SolidBrush(Color.Red);

        /// <summary>
        /// The control to display a log.
        /// </summary>
        protected readonly RichTextBox richTextBox;
        /// <summary>
        /// The control to display a log.
        /// </summary>
        protected readonly ListBox listBox;
        /// <summary>
        /// The local file name of the log.
        /// </summary>
        protected string logFileName;
        /// <summary>
        /// The last write time of the local log file (UTC).
        /// </summary>
        protected DateTime logFileAge;
        /// <summary>
        /// 
        /// </summary>
        protected int itemPaddingTop;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LogBox(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox ?? throw new ArgumentNullException("richTextBox");
            richTextBox.HideSelection = false;
            logFileName = "";
            logFileAge = DateTime.MinValue;

            FullLogView = false;
            LogViewSize = DefaultLogViewSize;
            AutoScroll = false;
            Colorize = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LogBox(ListBox listBox, bool colorize = false)
        {
            this.listBox = listBox ?? throw new ArgumentNullException("listBox");
            logFileName = "";
            logFileAge = DateTime.MinValue;
            itemPaddingTop = 0;

            if (colorize)
            {
                listBox.DrawMode = DrawMode.OwnerDrawFixed;
                listBox.DrawItem += ListBox_DrawItem;
            }

            FullLogView = false;
            LogViewSize = DefaultLogViewSize;
            AutoScroll = false;
            Colorize = colorize;
        }


        /// <summary>
        /// Reads lines from the log file.
        /// </summary>
        private List<string> ReadLines()
        {
            using (FileStream fileStream =
                new FileStream(LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                List<string> lines = new List<string>();
                long offset = FullLogView ? 0 : Math.Max(0, fileStream.Length - LogViewSize);

                if (fileStream.Seek(offset, SeekOrigin.Begin) == offset)
                {
                    using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        // add or skip the first line
                        if (!reader.EndOfStream)
                        {
                            string s = reader.ReadLine();
                            if (offset == 0)
                                lines.Add(s);
                        }

                        // read the rest lines
                        while (!reader.EndOfStream)
                        {
                            lines.Add(reader.ReadLine());
                        }
                    }
                }

                return lines;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to load a full log.
        /// </summary>
        public bool FullLogView { get; set; }

        /// <summary>
        /// Gets or sets the size of displayed part of a log in bytes.
        /// </summary>
        public int LogViewSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically scroll down the content of the text box.
        /// </summary>
        public bool AutoScroll { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lines should be colorized depending on key words.
        /// </summary>
        public bool Colorize { get; set; }

        /// <summary>
        /// Gets or sets the local file name of the log.
        /// </summary>
        public string LogFileName
        {
            get
            {
                return logFileName;
            }
            set
            {
                logFileName = value;
                logFileAge = DateTime.MinValue;
            }
        }


        /// <summary>
        /// Apply colors to the text box.
        /// </summary>
        private void ApplyColors(ICollection<string> lines)
        {
            int index = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("send", StringComparison.OrdinalIgnoreCase) ||
                    line.StartsWith("отправка", StringComparison.OrdinalIgnoreCase))
                {
                    richTextBox.Select(index, line.Length);
                    richTextBox.SelectionColor = Color.Blue;
                }
                else if (line.StartsWith("receive", StringComparison.OrdinalIgnoreCase) ||
                    line.StartsWith("приём", StringComparison.OrdinalIgnoreCase))
                {
                    richTextBox.Select(index, line.Length);
                    richTextBox.SelectionColor = Color.Purple;
                }
                else if (line.StartsWith("ok", StringComparison.OrdinalIgnoreCase))
                {
                    richTextBox.Select(index, line.Length);
                    richTextBox.SelectionColor = Color.Green;
                }
                else if (line.StartsWith("error", StringComparison.OrdinalIgnoreCase) ||
                    line.StartsWith("ошибка", StringComparison.OrdinalIgnoreCase))
                {
                    richTextBox.Select(index, line.Length);
                    richTextBox.SelectionColor = Color.Red;
                }

                index += line.Length + 1 /*new line*/;
            }
        }

        /// <summary>
        /// Chooses the appropriate brush depending on the line prefix.
        /// </summary>
        private Brush ChooseBrush(string line, bool itemSelected)
        {
            if (itemSelected)
            {
                return SystemBrushes.HighlightText;
            }
            else if (line.StartsWith("send", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("отправка", StringComparison.OrdinalIgnoreCase))
            {
                return SentDataBrush;
            }
            else if (line.StartsWith("receive", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("приём", StringComparison.OrdinalIgnoreCase))
            {
                return ReceivedDataBrush;
            }
            else if (line.StartsWith("ok", StringComparison.OrdinalIgnoreCase))
            {
                return AckBrush;
            }
            else if (line.StartsWith("error", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("ошибка", StringComparison.OrdinalIgnoreCase))
            {
                return ErrorBrush;
            }
            else
            {
                return SystemBrushes.WindowText;
            }
        }


        /// <summary>
        /// Sets the text box lines.
        /// </summary>
        public void SetLines(ICollection<string> lines)
        {
            if (richTextBox == null)
                return;

            int selectionStart = richTextBox.SelectionStart;
            int selectionLength = richTextBox.SelectionLength;
            richTextBox.Text = string.Join(Environment.NewLine, lines);

            if (Colorize)
                ApplyColors(lines);

            if (AutoScroll)
                richTextBox.Select(richTextBox.TextLength, 0);
            else
                richTextBox.Select(selectionStart, selectionLength);

            richTextBox.ScrollToCaret();
        }

        /// <summary>
        /// Sets the list box lines.
        /// </summary>
        public void SetLines2(ICollection<string> lines)
        {
            if (listBox == null)
                return;

            Graphics graphics = null;
            listBox.BeginUpdate();

            try
            {
                if (Colorize)
                {
                    graphics = listBox.CreateGraphics();
                    int lineHeight = (int)graphics.MeasureString("0", listBox.Font).Height;
                    itemPaddingTop = (listBox.ItemHeight - lineHeight) / 2;
                    listBox.HorizontalExtent = 0; // needed for horizontal scroll bar
                }

                int oldLineCnt = listBox.Items.Count;
                int newLineCnt = lines.Count;
                int topIndex = listBox.TopIndex;
                int lineIndex = 0;

                foreach (string line in lines)
                {
                    if (lineIndex < oldLineCnt)
                        listBox.Items[lineIndex] = line;
                    else
                        listBox.Items.Add(line);

                    if (graphics != null)
                    {
                        int lineWidth = (int)graphics.MeasureString(line, listBox.Font).Width;
                        listBox.HorizontalExtent = Math.Max(listBox.HorizontalExtent, lineWidth);
                    }

                    lineIndex++;
                }

                for (int i = oldLineCnt - 1; i >= newLineCnt; i--)
                {
                    listBox.Items.RemoveAt(i);
                }

                if (newLineCnt > 0)
                {
                    if (AutoScroll)
                    {
                        listBox.SelectedIndex = -1;
                        listBox.TopIndex = newLineCnt - 1;
                    }
                    else
                    {
                        listBox.TopIndex = Math.Min(topIndex, newLineCnt - 1);
                    }
                }
            }
            finally
            {
                listBox.EndUpdate();
                graphics?.Dispose();
            }
        }

        /// <summary>
        /// Refresh log content from file.
        /// </summary>
        public void RefreshFromFile()
        {
            try
            {
                if (File.Exists(LogFileName))
                {
                    DateTime newLogFileAge = File.GetLastWriteTimeUtc(LogFileName);

                    if (logFileAge != newLogFileAge)
                    {
                        List<string> lines = ReadLines();

                        if (lines.Count == 0)
                            lines.Add(CommonPhrases.NoData);

                        SetLines(lines);
                        SetLines2(lines);
                        logFileAge = newLogFileAge;
                    }
                }
                else
                {
                    richTextBox.Text = CommonPhrases.FileNotFound;
                }
            }
            catch (Exception ex)
            {
                richTextBox.Text = ex.Message;
            }
        }


        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            string line = (string)listBox.Items[e.Index];
            Brush brush = ChooseBrush(line, e.State.HasFlag(DrawItemState.Selected));

            e.DrawBackground();
            e.Graphics.DrawString(line, listBox.Font, brush, e.Bounds.Left, e.Bounds.Top + itemPaddingTop);
            e.DrawFocusRectangle();
        }
    }
}
