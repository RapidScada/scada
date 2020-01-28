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
 * Module   : Administrator
 * Summary  : Represents a directory or file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using System;
using System.IO;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents a directory or file.
    /// <para>Представляет директорию или файл.</para>
    /// </summary>
    internal class FileItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FileItem(DirectoryInfo directoryInfo)
        {
            Update(directoryInfo);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FileItem(FileInfo fileInfo)
        {
            Update(fileInfo);
        }


        /// <summary>
        /// Gets or sets the full directory name or file name.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the short directory name or file name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the item is a directory.
        /// </summary>
        public bool IsDirectory { get; private set; }


        /// <summary>
        /// Updates the properties according to the specified directory.
        /// </summary>
        public void Update(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            Path = directoryInfo.FullName;
            Name = directoryInfo.Name;
            IsDirectory = true;
        }

        /// <summary>
        /// Updates the properties according to the specified file.
        /// </summary>
        public void Update(FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException("fileInfo");

            Path = fileInfo.FullName;
            Name = fileInfo.Name;
            IsDirectory = false;
        }
    }
}
