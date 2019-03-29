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
 * Module   : Communicator Shell
 * Summary  : The class contains utility methods for the Communicator shell
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System.Drawing;
using System.Windows.Forms;

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// The class contains utility methods for the Communicator shell.
    /// <para>Класс, содержащий вспомогательные методы для оболочки Коммуникатора.</para>
    /// </summary>
    public static class CommShellUtils
    {
        /// <summary>
        /// The command sender specified in command files.
        /// </summary>
        public const string CommandSender = "ScadaCommShell";

        /// <summary>
        /// Draws a list box item representing a tab.
        /// </summary>
        public static void DrawTabItem(this ListBox listBox, DrawItemEventArgs e)
        {
            const int PaddingLeft = 5;
            string text = (string)listBox.Items[e.Index];
            SizeF textSize = e.Graphics.MeasureString(text, listBox.Font);
            Brush brush = e.State.HasFlag(DrawItemState.Selected) ?
                SystemBrushes.HighlightText : SystemBrushes.WindowText;

            e.DrawBackground();
            e.Graphics.DrawString(text, listBox.Font, brush,
                e.Bounds.Left + PaddingLeft, e.Bounds.Top + (listBox.ItemHeight - textSize.Height) / 2);
            e.DrawFocusRectangle();
        }
    }
}
