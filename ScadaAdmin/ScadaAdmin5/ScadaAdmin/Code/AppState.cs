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
 * Module   : Administrator
 * Summary  : State of application controls
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// State of application controls.
    /// <para>Состояние элементов управления в приложении.</para>
    /// </summary>
    public class AppState
    {
        /// <summary>
        /// State of form controls.
        /// <para>Состояние элементов управления формы.</para>
        /// </summary>
        public class FormState
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public FormState()
            {
                Left = 0;
                Top = 0;
                Width = 0;
                Height = 0;
                Maximized = true;
            }


            /// <summary>
            /// Tests whether the form state is empty.
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    return Width > 0 && Height > 0;
                }
            }
            /// <summary>
            /// Gets or sets the form horizontal position.
            /// </summary>
            public int Left { get; set; }
            /// <summary>
            /// Gets or sets the form vertical position.
            /// </summary>
            public int Top { get; set; }
            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int Height { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether the form is maximized.
            /// </summary>
            public bool Maximized { get; set; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppState()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the state of the main form controls.
        /// </summary>
        public FormState MainFormState { get; private set; }

        /// <summary>
        /// Gets or sets the directory of projects.
        /// </summary>
        public string ProjectDir { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            MainFormState = new FormState();
            ProjectDir = "";
        }

        /// <summary>
        /// Initializes the application state.
        /// </summary>
        public void Init(string exeDir)
        {
            ProjectDir = Path.GetFullPath(Path.Combine(exeDir, "..", "Projects"));
        }
    }
}
