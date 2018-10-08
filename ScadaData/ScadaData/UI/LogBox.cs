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
        /// Initializes a new instance of the class.
        /// </summary>
        public LogBox(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox ?? throw new ArgumentNullException("richTextBox");
            LogViewSize = DefaultLogViewSize;
            Colorize = false;
        }


        /// <summary>
        /// Gets or sets the size of displayed part of a log in bytes.
        /// </summary>
        public int LogViewSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lines should be colorized depending on key words.
        /// </summary>
        public bool Colorize { get; set; }


        /// <summary>
        /// Appends lines to the text box and removes the beginning if necessary.
        /// </summary>
        public void AppendLines(ICollection<string> lines)
        {

        }
    }
}
