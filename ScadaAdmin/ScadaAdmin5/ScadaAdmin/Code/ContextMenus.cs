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
 * Summary  : References to the context menus
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// References to the context menus.
    /// <para>Ссылки на контекстные меню.</para>
    /// </summary>
    internal class ContextMenus
    {
        public ContextMenuStrip ProjectMenu { get; set; }

        public ContextMenuStrip CnlTableMenu { get; set; }

        public ContextMenuStrip DirectoryMenu { get; set; }

        public ContextMenuStrip FileItemMenu { get; set; }

        public ContextMenuStrip InstanceMenu { get; set; }

        public ContextMenuStrip ServerMenu { get; set; }

        public ContextMenuStrip CommMenu { get; set; }

        public ContextMenuStrip CommLineMenu { get; set; }

        public ContextMenuStrip DeviceMenu { get; set; }
    }
}
