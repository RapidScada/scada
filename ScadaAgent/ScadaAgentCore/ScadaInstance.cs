using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Agent
{
    public class ScadaInstance
    {
        public bool ValidateUser(string username, string encryptedPassword, out string errMsg)
        {
            errMsg = "";
            return true;
        }
    }
}
