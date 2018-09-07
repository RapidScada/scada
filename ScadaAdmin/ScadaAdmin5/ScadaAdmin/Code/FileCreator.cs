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
 * Summary  : Creates files of a project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.IO;
using System.Xml;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates files of a project.
    /// <para>Создаёт файлы проекта.</para>
    /// </summary>
    internal static class FileCreator
    {
        /// <summary>
        /// Creates a new file of the specified type.
        /// </summary>
        public static void CreateFile(string fileName, KnownFileType fileType)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            {
                if (fileType == KnownFileType.SchemeView ||
                    fileType == KnownFileType.TableView || 
                    fileType == KnownFileType.XmlFile)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmlDoc.AppendChild(xmlDecl);

                    if (fileType == KnownFileType.SchemeView)
                        xmlDoc.AppendChild(xmlDoc.CreateElement("SchemeView"));
                    else if (fileType == KnownFileType.TableView)
                        xmlDoc.AppendChild(xmlDoc.CreateElement("TableView"));

                    xmlDoc.Save(stream);
                }
            }
        }

        /// <summary>
        /// Gets an extension corresponds to the file type.
        /// </summary>
        public static string GetExtension(KnownFileType fileType)
        {
            switch (fileType)
            {
                case KnownFileType.SchemeView:
                    return "sch";
                case KnownFileType.TableView:
                    return "tbl";
                case KnownFileType.TextFile:
                    return "txt";
                case KnownFileType.XmlFile:
                    return "xml";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Gets a file type by the extension.
        /// </summary>
        public static KnownFileType GetFileType(string extension)
        {
            string ext = extension == null ? "" : extension.ToLowerInvariant();

            if (ext == "sch")
                return KnownFileType.SchemeView;
            else if (ext == "tbl")
                return KnownFileType.TableView;
            else if (ext == "txt")
                return KnownFileType.TextFile;
            else if (ext == "xml")
                return KnownFileType.XmlFile;
            else
                return KnownFileType.None;
        }

        /// <summary>
        /// Checks whether the extension is known.
        /// </summary>
        public static bool ExtensionIsKnown(string extension)
        {
            return GetFileType(extension) != KnownFileType.None;
        }
    }
}
