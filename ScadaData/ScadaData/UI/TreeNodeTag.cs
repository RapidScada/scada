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
 * Summary  : Represents an object associated with a tree node
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Windows.Forms;

namespace Scada.UI
{
    /// <summary>
    /// Represents an object associated with a tree node.
    /// <para>Представляет объект, связанный с узлом дерева.</para>
    /// </summary>
    public class TreeNodeTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeNodeTag()
        {
            FormType = null;
            FormArgs = null;
            ExistingForm = null;
            RelatedObject = null;
            NodeType = "";
        }


        /// <summary>
        /// Gets or sets the type of form to create.
        /// </summary>
        public Type FormType { get; set; }

        /// <summary>
        /// Gets or sets the form creation arguments.
        /// </summary>
        public object[] FormArgs { get; set; }

        /// <summary>
        /// Gets or sets a form that already exists.
        /// </summary>
        public Form ExistingForm { get; set; }

        /// <summary>
        /// Gets or sets the object related to the node.
        /// </summary>
        public object RelatedObject { get; set; }

        /// <summary>
        /// Gets or sets the node type that determines possible actions with the node.
        /// </summary>
        public string NodeType { get; set; }
    }
}
