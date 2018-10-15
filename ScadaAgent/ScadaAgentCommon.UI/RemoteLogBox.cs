using Scada.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaAgentCommon.UI
{
    public class RemoteLogBox : LogBox
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RemoteLogBox(ListBox listBox, bool colorize = false)
            : base(listBox, colorize)
        {

        }
    }
}
