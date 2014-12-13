/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : SCADA-Communicator Service
 * Summary  : Receive commands via TCP and files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2008
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Scada.Comm.KP;
using Utils;

namespace Scada.Comm.Svc
{
    /// <summary>
    /// Receive commands via TCP and files
    /// <para>Приём команд по протоколу TCP и через файлы</para>
    /// </summary>
    sealed class CommandReader
    {
        private Manager mngr;            // менеджер, управляющий работой программы
        private ServerCommEx serverComm; // ссылка на объект обмена данными со SCADA-Сервером
        private string cmdDir;           // директория команд
        private Log log;                 // ссылка на основной log-файл программы
        private Thread thread;           // поток приёма команд


        /// <summary>
        /// Конструктор
        /// </summary>
        public CommandReader(Manager mngr)
        {
            this.mngr = mngr;
            serverComm = mngr.ServerComm;
            cmdDir = Manager.CmdDir;
            log = mngr.Log;
            thread = null;
        }


        /// <summary>
        /// Получить поток приёма команд
        /// </summary>
        public Thread Thread
        {
            get
            {
                return thread;
            }
        }


        /// <summary>
        /// Загрузить из файла команду и проверить её корретность
        /// </summary>
        private bool LoadCmd(string fileName, out string cmdType, 
            out Dictionary<string, string> cmdParams, out KPLogic.Command kpCmd)
        {
            bool result = false;
            cmdType = "";
            cmdParams = null;
            kpCmd = null;

            FileStream fileStream = null;
            StreamReader streamReader = null;

            try
            {
                string target = "";
                DateTime date = DateTime.MinValue;
                DateTime time = DateTime.MinValue;
                int lifeTime = 0;
                bool endFound = false;

                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(fileStream, Encoding.Default);

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine().Trim();
                    string lineL = line.ToLower();

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
                        else if (lineL.StartsWith("date="))
                        {
                            string[] vals = lineL.Remove(0, 5).Split('.');
                            date = new DateTime(int.Parse(vals[2]), int.Parse(vals[1]), int.Parse(vals[0]));
                        }
                        else if (lineL.StartsWith("time="))
                        {
                            string[] vals = lineL.Remove(0, 5).Split(':');
                            time = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, 
                                DateTime.MinValue.Day, int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
                        }
                        else if (lineL.StartsWith("lifetime="))
                        {
                            lifeTime = int.Parse(lineL.Remove(0, 9));
                        }
                        else if (lineL.StartsWith("cmdtype="))
                        {
                            cmdType = line.Remove(0, 8);

                            try
                            {
                                KPLogic.CmdType kpCmdType = (KPLogic.CmdType)Enum.Parse(
                                    typeof(KPLogic.CmdType), cmdType, true);
                                kpCmd = new KPLogic.Command(kpCmdType);
                            }
                            catch { }
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

                            if (kpCmd != null)
                            {
                                if (lineL.StartsWith("kpnum="))
                                {
                                    kpCmd.KPNum = int.Parse(lineL.Remove(0, 6));
                                }
                                else if (lineL.StartsWith("cmdnum="))
                                {
                                    if (kpCmd.CmdType != KPLogic.CmdType.Request)
                                        kpCmd.CmdNum = int.Parse(lineL.Remove(0, 7));
                                }
                                else if (lineL.StartsWith("cmdval="))
                                {
                                    if (kpCmd.CmdType == KPLogic.CmdType.Standard)
                                    {
                                        lineL = lineL.Remove(0, 7);
                                        string s1 = lineL.Replace('.', ',');
                                        string s2 = lineL.Replace(',', '.');
                                        double val;
                                        if (double.TryParse(s1, out val))
                                            kpCmd.CmdVal = val;
                                        else if (double.TryParse(s2, out val))
                                            kpCmd.CmdVal = val;
                                    }
                                }
                                else if (lineL.StartsWith("cmddata="))
                                {
                                    if (kpCmd.CmdType == KPLogic.CmdType.Binary)
                                    {
                                        int len = lineL.Length;
                                        byte[] cmdData = new byte[(len - 8) / 2];

                                        for (int i = 9, j = 0; i < len; i += 2, j++)
                                        {
                                            string byteStr = lineL[i - 1].ToString() + lineL[i].ToString();
                                            cmdData[j] = byte.Parse(byteStr, NumberStyles.HexNumber);
                                        }

                                        kpCmd.CmdData = cmdData;
                                    }
                                }
                            }
                        }
                    }
                }

                if (cmdParams != null && target == "scadacommsvc" && endFound)
                {
                    // удаление успешно обработанного файла команды
                    streamReader.Close();
                    streamReader = null;
                    fileStream.Close();
                    fileStream = null;
                    File.Delete(fileName);

                    // проверка команды
                    DateTime cmdDT = new DateTime(date.Year, date.Month, date.Day,
                        time.Hour, time.Minute, time.Second);
                    DateTime nowDT = DateTime.Now;
                    string cmdInfo = (Localization.UseRussian ? " Тип: " : " Type: ") + cmdType;
                    cmdInfo += kpCmd == null ? "" : (Localization.UseRussian ? 
                        ", КП: " + kpCmd.KPNum + ", номер: " + kpCmd.CmdNum :
                        ", device: " + kpCmd.KPNum + ", number: " + kpCmd.CmdNum);

                    if (nowDT.AddSeconds(-lifeTime) <= cmdDT && cmdDT <= nowDT.AddSeconds(lifeTime))
                    {
                        log.WriteAction((Localization.UseRussian ? "Получена команда из файла." : 
                            "The command is received from file.") + cmdInfo, Log.ActTypes.Action);
                        result = true;
                    }
                    else
                    {
                        log.WriteAction((Localization.UseRussian ? "Получена неактуальная команда из файла." :
                            "The outdated command is received from file.") + cmdInfo, Log.ActTypes.Action);
                    }

                    cmdType = cmdType.ToLower();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при приёме команды из файла {0}: {1}" :
                    "Error receiving command from file {0}: {1}", Path.GetFileName(fileName), ex.Message), 
                    Log.ActTypes.Exception);
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
                if (fileStream != null)
                    fileStream.Close();
            }

            return result;
        }

        /// <summary>
        /// Приём команд, метод вызывается в отдельном потоке
        /// </summary>
        private void Execute()
        {
            while (true)
            {
                try
                {
                    // приём команд от SCADA-Сервера
                    if (serverComm != null)
                    {
                        KPLogic.Command cmd;
                        while (serverComm.ReceiveCommand(out cmd))
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ? 
                                "Получена команда от SCADA-Сервера. Тип: {0}, КП: {1}, номер: {2}" : 
                                "The command is received from SCADA-Server. Type: {0}, device: {1}, number: {2}", 
                                cmd.CmdType, cmd.KPNum, cmd.CmdNum), Log.ActTypes.Action);
                            mngr.PassCmd(cmd);
                        }
                    }

                    // приём команд из файлов
                    if (cmdDir != "" && Directory.Exists(cmdDir))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(cmdDir);
                        FileInfo[] fileInfoAr = dirInfo.GetFiles("cmd*.dat", SearchOption.TopDirectoryOnly);

                        foreach (FileInfo fileInfo in fileInfoAr)
                        {
                            string cmdType;
                            Dictionary<string, string> cmdParams;
                            KPLogic.Command kpCmd;

                            if (LoadCmd(fileInfo.FullName, out cmdType, out cmdParams, out kpCmd))
                            {
                                if (kpCmd == null)
                                {
                                    int lineNum;
                                    if ((cmdType == "startline" || cmdType == "stopline" || cmdType == "restartline") &&
                                        cmdParams.ContainsKey("linenum") && int.TryParse(cmdParams["linenum"], out lineNum))
                                    {
                                        // запуск или остановка линии связи
                                        if (cmdType == "startline")
                                            mngr.StartCommLine(lineNum);
                                        else if (cmdType == "stopline")
                                            mngr.StopCommLine(lineNum);
                                        else
                                            mngr.RestartCommLine(lineNum);
                                    }
                                    else
                                    {
                                        log.WriteAction(Localization.UseRussian ? "Некорректные данные команды" :
                                            "Incorrect command data", Log.ActTypes.Error);
                                    }
                                }
                                else
                                {
                                    // передача команды КП
                                    mngr.PassCmd(kpCmd);
                                }
                            }
                        }
                    }

                    Thread.Sleep(200);
                }
                catch (ThreadAbortException)
                {
                    log.WriteAction(Localization.UseRussian ? "Прерывание приёма команд" : 
                        "Receiving commands aborted", Log.ActTypes.Action);
                }
                catch (Exception ex)
                {
                    log.WriteAction((Localization.UseRussian ? "Ошибка при приёме команд: " : 
                        "Error receiving commands: ") + ex.Message, Log.ActTypes.Exception);
                }
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
    }
}