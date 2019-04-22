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
 * Module   : ScadaCommEngine
 * Summary  : Receive commands via TCP and files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2019
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Utils;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Receive commands via TCP and files.
    /// <para>Приём команд по протоколу TCP и через файлы.</para>
    /// </summary>
    internal sealed class CommandReader
    {
        private readonly Manager mngr;            // менеджер, управляющий работой программы
        private readonly ServerCommEx serverComm; // ссылка на объект обмена данными со SCADA-Сервером
        private readonly string cmdDir;           // директория команд
        private readonly Log log;                 // ссылка на основной log-файл программы
        private Thread thread;                    // поток приёма команд


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommandReader(Manager mngr)
        {
            this.mngr = mngr ?? throw new ArgumentNullException("mngr");
            serverComm = mngr.ServerComm;
            cmdDir = mngr.AppDirs.CmdDir;
            log = mngr.AppLog;
            thread = null;
        }


        /// <summary>
        /// Получить короткое описание команды
        /// </summary>
        private string GetCmdShortDescr(Command cmd)
        {
            if (Localization.UseRussian)
            {
                return cmd == null ? 
                    "команда управления приложением" :
                    new StringBuilder()
                        .Append("тип=").Append(cmd.GetCmdTypeCode())
                        .Append(", КП=").Append(cmd.KPNum)
                        .Append(", номер=").Append(cmd.CmdNum)
                        .ToString();
            }
            else
            {
                return cmd == null ? 
                    "application control command" :
                    new StringBuilder()
                        .Append("type=").Append(cmd.GetCmdTypeCode())
                        .Append(", device=").Append(cmd.KPNum)
                        .Append(", number=").Append(cmd.CmdNum)
                        .ToString();
            }
        }

        /// <summary>
        /// Принять команду от SCADA-Сервера
        /// </summary>
        private void ReceiveFromServer()
        {
            if (serverComm != null)
            {
                Command cmd;
                while (serverComm.ReceiveCommand(out cmd))
                {
                    log.WriteAction((Localization.UseRussian ?
                        "Получена команда от SCADA-Сервера: " :
                        "The command is received from SCADA-Server: ") + GetCmdShortDescr(cmd));
                    mngr.PassCmd(cmd);
                }
            }
        }

        /// <summary>
        /// Принять команду из файла
        /// </summary>
        private void ReceiveFromFile()
        {
            if (Directory.Exists(cmdDir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(cmdDir);
                FileInfo[] fileInfoAr = dirInfo.GetFiles("cmd*.dat", SearchOption.TopDirectoryOnly);

                foreach (FileInfo fileInfo in fileInfoAr)
                {
                    if (LoadCmdFromFile(fileInfo.FullName, 
                        out string cmdType, out Dictionary<string, string> cmdParams, out Command cmd))
                    {
                        if (cmd == null)
                        {
                            // команда относится не к КП, а к линии связи
                            if (Enum.TryParse(cmdType, true, out CommLineCmd commLineCmd) &&
                                cmdParams.TryGetValue("linenum", out string lineNumStr) &&
                                int.TryParse(lineNumStr, out int lineNum))
                            {
                                // запуск или остановка линии связи
                                if (commLineCmd == CommLineCmd.StartLine)
                                    mngr.StartCommLine(lineNum);
                                else if (commLineCmd == CommLineCmd.StopLine)
                                    mngr.StopCommLine(lineNum);
                                else
                                    mngr.RestartCommLine(lineNum);
                            }
                            else
                            {
                                log.WriteError(Localization.UseRussian ?
                                    "Некорректные данные команды" :
                                    "Incorrect command data");
                            }
                        }
                        else
                        {
                            // передача команды КП
                            mngr.PassCmd(cmd);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Загрузить из файла команду и проверить её корретность
        /// </summary>
        private bool LoadCmdFromFile(string fileName, out string cmdType,
            out Dictionary<string, string> cmdParams, out Command cmd)
        {
            bool result = false;
            cmdType = "";
            cmdParams = null;
            cmd = null;

            FileStream fileStream = null;
            StreamReader streamReader = null;

            try
            {
                // считывание команды из файла
                string target = "";
                DateTime dateTime = DateTime.MinValue;
                int lifeTime = 0;
                bool endFound = false;

                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(fileStream, Encoding.Default);

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine().Trim();
                    string lineL = line.ToLowerInvariant();

                    if (cmdParams == null)
                    {
                        if (lineL == "[command]")
                            cmdParams = new Dictionary<string, string>();
                    }
                    else
                    {
                        if (lineL.StartsWith("target="))
                        {
                            target = lineL.Remove(0, 7);
                        }
                        else if (lineL.StartsWith("datetime="))
                        {
                            dateTime = DateTime.Parse(lineL.Remove(0, 9), DateTimeFormatInfo.InvariantInfo);
                        }
                        else if (lineL.StartsWith("lifetime="))
                        {
                            lifeTime = int.Parse(lineL.Remove(0, 9));
                        }
                        else if (lineL.StartsWith("cmdtype="))
                        {
                            cmdType = line.Remove(0, 8);
                            int cmdTypeID = BaseValues.CmdTypes.ParseCmdTypeCode(cmdType);
                            if (cmdTypeID >= 0)
                                cmd = new Command(cmdTypeID);
                        }
                        else if (lineL.StartsWith("end="))
                        {
                            endFound = true;
                        }
                        else
                        {
                            int ind = lineL.IndexOf("=");
                            if (ind >= 0)
                                cmdParams[lineL.Substring(0, ind)] = lineL.Substring(ind + 1);

                            if (cmd != null)
                            {
                                if (lineL.StartsWith("kpnum="))
                                {
                                    cmd.KPNum = int.Parse(lineL.Remove(0, 6));
                                }
                                else if (lineL.StartsWith("cmdnum="))
                                {
                                    if (cmd.CmdTypeID != BaseValues.CmdTypes.Request)
                                        cmd.CmdNum = int.Parse(lineL.Remove(0, 7));
                                }
                                else if (lineL.StartsWith("cmdval="))
                                {
                                    if (cmd.CmdTypeID == BaseValues.CmdTypes.Standard)
                                        cmd.CmdVal = ScadaUtils.ParseDouble(lineL.Remove(0, 7));
                                }
                                else if (lineL.StartsWith("cmddata="))
                                {
                                    if (cmd.CmdTypeID == BaseValues.CmdTypes.Binary)
                                    {
                                        byte[] cmdData;
                                        if (ScadaUtils.HexToBytes(lineL.Remove(0, 8), out cmdData))
                                            cmd.CmdData = cmdData;
                                    }
                                }
                            }
                        }
                    }
                }

                if (cmdParams != null && target == "scadacommsvc" && endFound)
                {
                    // проверка актуальности команды
                    DateTime nowDT = DateTime.Now;
                    if (nowDT.AddSeconds(-lifeTime) <= dateTime && dateTime <= nowDT.AddSeconds(lifeTime))
                    {
                        log.WriteAction((Localization.UseRussian ?
                            "Получена команда из файла: " :
                            "The command is received from file: ") + GetCmdShortDescr(cmd));
                        result = true;
                    }
                    else
                    {
                        log.WriteAction((Localization.UseRussian ?
                            "Получена неактуальная команда из файла: " :
                            "The outdated command is received from file: ") + GetCmdShortDescr(cmd));
                    }

                    cmdType = cmdType.ToLowerInvariant();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при приёме команды из файла {0}" :
                    "Error receiving command from file {0}", Path.GetFileName(fileName));
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }

            if (result)
            {
                // удаление успешно обработанного файла команды
                try { File.Delete(fileName); }
                catch { }
                return true;
            }
            else
            {
                // переименование файла команды, который не был обработан
                try { File.Move(fileName, fileName + ".err"); }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// Приём команд, метод вызывается в отдельном потоке
        /// </summary>
        private void Execute()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        ReceiveFromServer();
                        ReceiveFromFile();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, Localization.UseRussian ?
                            "Ошибка при приёме команд" :
                            "Error receiving commands");
                    }

                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
            catch (ThreadAbortException)
            {
                log.WriteAction(Localization.UseRussian ?
                    "Прерывание приёма команд" :
                    "Receiving commands aborted");
            }
        }


        /// <summary>
        /// Запустить поток приёма команд
        /// </summary>
        public void StartThread()
        {
            thread = new Thread(new ThreadStart(Execute));
            thread.Start();
        }

        /// <summary>
        /// Остановить поток приёма команд
        /// </summary>
        public void StopThread()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
    }
}
