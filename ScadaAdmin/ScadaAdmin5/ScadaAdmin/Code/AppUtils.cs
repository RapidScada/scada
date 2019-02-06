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
 * Module   : Administrator
 * Summary  : The class contains utility methods for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System.Diagnostics;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The class contains utility methods for the application.
    /// <para>Класс, содержащий вспомогательные методы приложения.</para>
    /// </summary>
    internal static class AppUtils
    {
        /// <summary>
        /// Opens the specified file in the default text editor.
        /// </summary>
        public static void OpenTextFile(string fileName)
        {
            Process.Start(fileName);
        }
    }
}
