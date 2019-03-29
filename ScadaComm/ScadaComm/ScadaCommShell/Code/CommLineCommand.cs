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
 * Module   : Communicator Shell
 * Summary  : Sends commands to communication lines
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Sends commands to communication lines.
    /// <para>Отправляет команды линиям связи.</para>
    /// </summary>
    public class CommLineCommand
    {
        private readonly Settings.CommLine commLine;  // the communication line which is the command destination
        private readonly CommEnvironment environment; // the application environment

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommLineCommand(Settings.CommLine commLine, CommEnvironment environment)
        {
            this.commLine = commLine ?? throw new ArgumentNullException("commLine");
            this.environment = environment ?? throw new ArgumentNullException("environment");
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        public bool Send(CommLineCmd commLineCmd, out string msg)
        {
            string[] cmdParams = new string[] { "LineNum=" + commLine.Number };

            if (CommUtils.SaveCmd(environment.AppDirs.CmdDir, CommShellUtils.CommandSender, commLineCmd.ToString(),
                new string[] { "LineNum=" + commLine.Number }, out msg))
            {
                return true;
            }
            else
            {
                environment.ErrLog.WriteError(msg);
                return false;
            }
        }
    }
}
