/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents parameters of a module command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Globalization;

namespace Scada.Server.Modules.DbExport
{
    /// <summary>
    /// Represents parameters of a module command.
    /// <para>Представляет параметры команды модуля.</para>
    /// </summary>
    internal class CmdParams
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CmdParams()
        {
            Action = CmdAction.None;
            MinDT = DateTime.MinValue;
            MaxDT = DateTime.MinValue;
        }


        /// <summary>
        /// Gets or sets the command action.
        /// </summary>
        public CmdAction Action { get; set; }

        /// <summary>
        /// Gets or sets the beginning of the time range.
        /// </summary>
        public DateTime MinDT { get; set; }

        /// <summary>
        /// Gets or sets the end of the time range.
        /// </summary>
        public DateTime MaxDT { get; set; }


        /// <summary>
        /// Converts the string to command parameters.
        /// </summary>
        public static bool Parse(string s, out CmdParams cmdParams, out string errMsg)
        {
            try
            {
                cmdParams = new CmdParams();
                string[] lines = (s ?? "").Split('\n');

                foreach (string line in lines)
                {
                    int idx = line.IndexOf('=');

                    if (idx > 0)
                    {
                        string key = line.Substring(0, idx).Trim().ToLowerInvariant();
                        string value = line.Substring(idx + 1).Trim();

                        if (key == "cmd")
                            cmdParams.Action = Enum.TryParse(value, true, out CmdAction action) ? action : CmdAction.None;
                        else if (key == "mindt")
                            cmdParams.MinDT = DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo);
                        else if (key == "maxdt")
                            cmdParams.MaxDT = DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo);
                    }
                }

                if (cmdParams.Action != CmdAction.None && 
                    cmdParams.MinDT > DateTime.MinValue && cmdParams.MaxDT > DateTime.MinValue)
                {
                    errMsg = "";
                    return true;
                }
                else
                {
                    errMsg = Localization.UseRussian ?
                        "Некорректная команда" :
                        "Incorrect command";
                    return false;
                }
            }
            catch
            {
                cmdParams = null;
                errMsg = Localization.UseRussian ?
                    "Ошибка при разборе команды" :
                    "Error parsing command";
                return false;
            }
        }
    }
}
