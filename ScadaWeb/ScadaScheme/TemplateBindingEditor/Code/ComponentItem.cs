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
 * Module   : Template Binding Editor
 * Summary  : Represents a component item to display on a form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Scheme.TemplateBindingEditor.Code
{
    /// <summary>
    /// Represents a component item to display on a form.
    /// <para>Представляет элемент компонента для отображения на форме.</para>
    /// </summary>
    internal class ComponentItem : IComparable<ComponentItem>
    {
        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        public int CompareTo(ComponentItem other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
