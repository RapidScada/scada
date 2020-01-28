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
 * Summary  : Specifies the contextual information about a scheme
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using Scada.Web;
using System;
using System.Reflection;

namespace Scada.Scheme
{
    /// <summary>
    /// Specifies the contextual information about a scheme.
    /// <para>Определяет контекстную информацию о схеме.</para>
    /// </summary>
    public sealed class SchemeContext
    {
        /// <summary>
        /// The current scheme context.
        /// </summary>
        private static readonly SchemeContext instance;


        /// <summary>
        /// Initializes the static data of the class.
        /// </summary>
        static SchemeContext()
        {
            instance = new SchemeContext();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private SchemeContext()
        {
            EditorMode = GetEditorMode();
            AppDirs = null;
        }


        /// <summary>
        /// Gets a value indicating that the running application is Scheme Editor.
        /// </summary>
        public bool EditorMode { get; private set; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; private set; }


        /// <summary>
        /// Determines whether the editor mode is running.
        /// </summary>
        private bool GetEditorMode()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            return asm != null && asm.GetName().Name == "ScadaSchemeEditor";
        }

        /// <summary>
        /// Initializes the scheme context.
        /// </summary>
        public void Init(AppDirs appDirs)
        {
            AppDirs = appDirs ?? throw new ArgumentNullException("appDirs");
        }

        /// <summary>
        /// Gets the current scheme context.
        /// </summary>
        public static SchemeContext GetInstance()
        {
            return instance;
        }
    }
}
