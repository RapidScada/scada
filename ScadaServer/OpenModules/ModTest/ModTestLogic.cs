/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : ModTest
 * Summary  : Server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2017
 * 
 * Description
 * Server module for testing.
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module logic
    /// <para>Логика работы серверного модуля</para>
    /// </summary>
    public class ModTestLogic : ModLogic
    {
        public ModTestLogic()
        {
        }


        public override string Name
        {
            get
            {
                return "ModTest";
            }
        }


        public override void OnServerStart()
        {
            // the method executes once on server start
            // метод выполняется один раз при запуске сервера
            base.OnServerStart();            
        }

        public override void OnServerStop()
        {
            // the method executes once on server stop
            // метод выполняется один раз при остановке сервера
            base.OnServerStop();
        }

        public override void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            // the method executes when new current data have been processed by the server,
            // the channel numbers are sorted in ascending order
            // метод выполняется после обработки новых текущих данных сервером,
            // номера каналов упорядочены по возрастанию
            const int MyCnlNum = 1;
            const int MyKpNum = 1;
            const int MyCmdNum = 1;
            const double MyCmdVal = 1.0;

            // send a command if the value of MyCnlNum channel greater than 200
            WriteToLog("Process current data by the module " + Name, Log.ActTypes.Action);
            SrezTableLight.CnlData cnlData;
            if (curSrez.GetCnlData(MyCnlNum, out cnlData) && cnlData.Val > 200)
            {
                WriteToLog("Send command by the module " + Name, Log.ActTypes.Action);
                Command cmd = new Command(BaseValues.CmdTypes.Standard);
                cmd.KPNum = MyKpNum;
                cmd.CmdNum = MyCmdNum;
                cmd.CmdVal = MyCmdVal;
                ServerCommands.PassCommand(cmd);
            }
        }

        public override void OnCurDataCalculated(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            // the method executes after current data calculation (approximately every 100 ms)
            // метод выполняется после вычисления дорасчётных каналов текущего среза (примерно каждые 100 мс)
        }

        public override void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
            // the method executes when new archive data have been processed by the server
            // метод выполняется после обработки новых архивных данных сервером
            WriteToLog("Process archive data by the module " + Name, Log.ActTypes.Action);
        }

        public override void OnEventCreating(EventTableLight.Event ev)
        {
            // the method executes on event creating, event properties could be changed here
            // метод выполняется при создании события, свойства события можно изменить здесь
        }

        public override void OnEventCreated(EventTableLight.Event ev)
        {
            // the method executes after event creating
            // метод выполняется после создания события
            WriteToLog("Process event creating by the module " + Name, Log.ActTypes.Action);
        }

        public override void OnEventChecked(DateTime date, int evNum, int userID)
        {
            // the method executes after event check
            // метод выполняется после квитирования события
            WriteToLog("Process event check by the module " + Name, Log.ActTypes.Action);
        }

        public override void OnCommandReceived(int ctrlCnlNum, Command cmd, int userID, ref bool passToClients)
        {
            // the method executes when a command has been received
            // метод выполняется после приёма команды ТУ
            WriteToLog("Process command by the module " + Name, Log.ActTypes.Action);
        }
    }
}
