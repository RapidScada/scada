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
 * Summary  : Parses scheme files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Scheme.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Scheme.TemplateBindingEditor.Code
{
    /// <summary>
    /// Parses scheme files.
    /// <para>Разбирает файлы схем.</para>
    /// </summary>
    internal static class SchemeParser
    {
        /// <summary>
        /// Parses the specified scheme file returning bindings.
        /// </summary>
        public static bool Parse(string fileName, out List<ComponentItem> components, 
            out List<ComponentBindingItem> bindings, out string errMsg)
        {
            try
            {
                components = new List<ComponentItem>();
                bindings = new List<ComponentBindingItem>();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                if (rootElem.SelectSingleNode("Components") is XmlNode componentsNode)
                {
                    foreach (XmlNode componentNode in componentsNode.ChildNodes)
                    {
                        int id = componentNode.GetChildAsInt("ID");
                        string name = componentNode.GetChildAsString("Name");
                        string imageName = componentNode.GetChildAsString("ImageName", null);
                        string text = componentNode.GetChildAsString("Text");
                        string displayName = 
                            BaseComponent.BuildDisplayName(id, name, imageName ?? text, componentNode.LocalName);

                        components.Add(new ComponentItem
                        {
                            ID = id,
                            DisplayName = displayName
                        });

                        if (componentNode.SelectSingleNode("InCnlNum") != null ||
                            componentNode.SelectSingleNode("CtrlCnlNum") != null)
                        {
                            // only dynamic components
                            bindings.Add(new ComponentBindingItem
                            {
                                CompID = id,
                                CompDisplayName = displayName,
                                InCnlNum = componentNode.GetChildAsInt("InCnlNum", int.MinValue),
                                CtrlCnlNum = componentNode.GetChildAsInt("CtrlCnlNum", int.MinValue)
                            });
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                components = null;
                bindings = null;
                errMsg = AppPhrases.ErrorParsingScheme + ": " + ex.Message;
                return false;
            }
        }
    }
}
