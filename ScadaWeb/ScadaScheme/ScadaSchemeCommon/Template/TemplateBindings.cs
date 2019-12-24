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
 * Summary  : Represents bindings of a scheme template
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Scheme.Template
{
    /// <summary>
    /// Represents bindings of a scheme template.
    /// <para>Представляет привязки шаблона схемы.</para>
    /// </summary>
    public class TemplateBindings
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TemplateBindings()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets or sets the scheme template file name.
        /// </summary>
        public string TemplateFileName { get; set; }

        /// <summary>
        /// Gets the bindings of the scheme components.
        /// </summary>
        public SortedDictionary<int, ComponentBinding> ComponentBindings { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            TemplateFileName = "";
            ComponentBindings = new SortedDictionary<int, ComponentBinding>();
        }

        /// <summary>
        /// Loads the bindings from the specified file.
        /// </summary>
        public void Load(string fileName)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;
                TemplateFileName = rootElem.GetChildAsString("TemplateFileName");

                foreach (XmlNode bindingNode in rootElem.SelectNodes("Binding"))
                {
                    ComponentBinding binding = new ComponentBinding();
                    binding.LoadFromXml(bindingNode);

                    if (binding.CompID > 0)
                        ComponentBindings[binding.CompID] = binding;
                }
            }
            catch (Exception ex)
            {
                throw new ScadaException(SchemePhrases.LoadTemplateBindingsError, ex);
            }
        }
    }
}
