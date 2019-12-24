/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Represents scheme arguments in template mode
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System.Collections.Generic;

namespace Scada.Scheme.Template
{
    /// <summary>
    /// Represents scheme arguments in template mode.
    /// <para>Представляет аргументы схемы в режиме шаблона.</para>
    /// </summary>
    public class TemplateArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TemplateArgs()
        {
            InCnlOffset = 0;
            CtrlCnlOffset = 0;
            TitleCompID = 0;
            BindingFileName = "";
        }


        /// <summary>
        /// Gets or sets the offset of input channel numbers.
        /// </summary>
        public int InCnlOffset { get; set; }
        
        /// <summary>
        /// Gets or sets the offset of output channel numbers.
        /// </summary>
        public int CtrlCnlOffset { get; set; }

        /// <summary>
        /// Gets or sets the ID of the component that displays a scheme title.
        /// </summary>
        public int TitleCompID { get; set; }

        /// <summary>
        /// Gets or sets the name of the file that defines bindings.
        /// </summary>
        public string BindingFileName { get; set; }


        /// <summary>
        /// Initializes the template arguments.
        /// </summary>
        public void Init(SortedList<string, string> args)
        {
            InCnlOffset = args.GetValueAsInt("inCnlOffset");
            CtrlCnlOffset = args.GetValueAsInt("ctrlCnlOffset");
            TitleCompID = args.GetValueAsInt("titleCompID");
            BindingFileName = args.GetValueAsString("bindingFileName");
        }
    }
}
