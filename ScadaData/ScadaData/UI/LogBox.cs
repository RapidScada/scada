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
        /// The control to display a log.
        /// </summary>
        protected RichTextBox richTextBox;
        /// <summary>
        /// The local file name of the log.
        /// </summary>
        protected string logFileName;
        /// <summary>
        /// The last write time of the local log file (UTC).
        /// </summary>
        protected DateTime logFileAge;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LogBox(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox ?? throw new ArgumentNullException("richTextBox");
            logFileName = "";
            logFileAge = DateTime.MinValue;

            FullLogView = false;
            LogViewSize = DefaultLogViewSize;
            Colorize = false;
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
        /// Sets the text box lines.
        /// </summary>
        public void SetLines(ICollection<string> lines)
        {
            richTextBox.Text = string.Join(Environment.NewLine, lines);
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
                        logFileAge = newLogFileAge;
                        SetLines(ReadLines());
                    }
                }
                else
                {
                    richTextBox.Text = CommonPhrases.NoData;
                }
            }
            catch (Exception ex)
            {
                richTextBox.Text = ex.Message;
            }
        }
    }
}
