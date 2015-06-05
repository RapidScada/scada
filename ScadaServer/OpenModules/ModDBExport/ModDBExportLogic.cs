/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ModDBExport
 * Summary  : Server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 * 
 * Description
 * Server module for real time data export from Rapid SCADA to DB.
 */

using Scada.Data;
using Scada.Server.Modules.DBExport;
using System;
using System.Data.Common;
using System.Text;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module logic
    /// <para>Логика работы серверного модуля</para>
    /// </summary>
    public class ModDBExportLogic : ModLogic
    {
        /// <summary>
        /// Имя файла журнала работы модуля
        /// </summary>
        internal const string LogFileName = "ModDBExport.log";


        private bool normalWork; // признак нормальной работы модуля
        private Log log;         // журнал работы модуля
        private Config config;   // конфигурация модуля


        /// <summary>
        /// Конструктор
        /// </summary>
        public ModDBExportLogic()
        {
            normalWork = true;
            log = null;
            config = null;
        }


        /// <summary>
        /// Получить имя модуля
        /// </summary>
        public override string Name
        {
            get
            {
                return "ModDBExport";
            }
        }


        /// <summary>
        /// Разъединиться с БД с выводом возможной ошибки в журнал
        /// </summary>
        private void Disconnect(DataSource dataSource)
        {
            try
            {
                dataSource.Disconnect();
            }
            catch (Exception ex)
            {
                log.WriteAction(string.Format(Localization.UseRussian ? "Ошибка при разъединении с БД {0}: {1}" :
                    "Error disconnecting from DB {0}: {1}", dataSource.Name, ex.Message));
            }
        }

        /// <summary>
        /// Экспортировать срез в БД
        /// </summary>
        private void ExportSrez(DataSource dataSource, DbCommand cmd, int[] cnlNums, SrezTableLight.Srez srez)
        {
            foreach (int cnlNum in cnlNums)
            {
                SrezTableLight.CnlData cnlData;

                if (srez.GetCnlData(cnlNum, out cnlData))
                {
                    dataSource.SetCmdParam(cmd, "cnlNum", cnlNum);
                    dataSource.SetCmdParam(cmd, "val", cnlData.Val);
                    dataSource.SetCmdParam(cmd, "stat", cnlData.Stat);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Выполнить действия при запуске работы сервера
        /// </summary>
        public override void OnServerStart()
        {
            // вывод в журнал
            log = new Log(Log.Formats.Simple);
            log.Encoding = Encoding.UTF8;
            log.FileName = LogDir + LogFileName;
            log.WriteBreak();
            log.WriteAction(string.Format(ModPhrases.StartModule, Name));

            // загрука конфигурации
            config = new Config(ConfigDir);
            string errMsg;

            if (config.Load(out errMsg))
            {
                // инициализация источников данных
                int i = 0;
                while (i < config.ExportDestinations.Count)
                {
                    Config.ExportDestination expDest = config.ExportDestinations[i];
                    DataSource dataSource = expDest.DataSource;

                    try
                    {
                        dataSource.InitConnection();
                        dataSource.InitCommands(expDest.ExportParams.ExportCurDataQuery,
                            expDest.ExportParams.ExportArcDataQuery, expDest.ExportParams.ExportEventQuery);
                        i++;
                    }
                    catch (Exception ex)
                    {
                        log.WriteAction(string.Format(Localization.UseRussian ? 
                            "Ошибка при инициализации источника данных {0}: {1}" : 
                            "Error initializing data source {0}: {1}", dataSource.Name, ex.Message));
                        // исключение из работы назначения, источник данных которого не был успешно инициализирован
                        config.ExportDestinations.RemoveAt(i);
                    }
                }
            }
            else
            {
                normalWork = false;
                log.WriteAction(errMsg);
                log.WriteAction(ModPhrases.NormalModExecImpossible);
            }
        }

        /// <summary>
        /// Выполнить действия при остановке работы сервера
        /// </summary>
        public override void OnServerStop()
        {
            // разъединение с БД
            foreach (Config.ExportDestination expDest in config.ExportDestinations)
                Disconnect(expDest.DataSource);

            // вывод в журнал
            log.WriteAction(string.Format(ModPhrases.StopModule, Name));
            log.WriteBreak();
        }

        /// <summary>
        /// Выполнить действия после обработки новых текущих данных
        /// </summary>
        public override void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            // экспорт текущих данных в БД
            if (normalWork)
            {
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    if (expDest.ExportParams.ExportCurData)
                    {
                        DataSource dataSource = expDest.DataSource;

                        try
                        {
                            dataSource.Connect();
                            ExportSrez(dataSource, dataSource.ExportCurDataCmd, cnlNums, curSrez);
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ? 
                                "Ошибка при экспорте текущих данных в БД {0}: {1}" :
                                "Error export current data to DB {0}: {1}", dataSource.Name, ex.Message));
                        }
                        finally
                        {
                            Disconnect(dataSource);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Выполнить действия после обработки новых архивных данных
        /// </summary>
        public override void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
            // экспорт архивных данных в БД
            if (normalWork)
            {
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    if (expDest.ExportParams.ExportArcData)
                    {
                        DataSource dataSource = expDest.DataSource;

                        try
                        {
                            dataSource.Connect();
                            dataSource.SetCmdParam(dataSource.ExportArcDataCmd, "dateTime", arcSrez.DateTime);
                            ExportSrez(dataSource, dataSource.ExportArcDataCmd, cnlNums, arcSrez);
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при экспорте текущих данных в БД {0}: {1}" :
                                "Error export current data to DB {0}: {1}", dataSource.Name, ex.Message));
                        }
                        finally
                        {
                            Disconnect(dataSource);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Выполнить действия после создания события и записи на диск
        /// </summary>
        public override void OnEventCreated(EventTableLight.Event ev)
        {
            // экспорт события в БД
            if (normalWork)
            {
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    if (expDest.ExportParams.ExportEvent)
                    {
                        DataSource dataSource = expDest.DataSource;

                        try
                        {
                            dataSource.Connect();
                            DbCommand cmd = dataSource.ExportEventCmd;
                            dataSource.SetCmdParam(cmd, "number", ev.Number);
                            dataSource.SetCmdParam(cmd, "dateTime", ev.DateTime);
                            dataSource.SetCmdParam(cmd, "objNum", ev.ObjNum);
                            dataSource.SetCmdParam(cmd, "kpNum", ev.KPNum);
                            dataSource.SetCmdParam(cmd, "paramID", ev.ParamID);
                            dataSource.SetCmdParam(cmd, "cnlNum", ev.CnlNum);
                            dataSource.SetCmdParam(cmd, "oldCnlVal", ev.OldCnlVal);
                            dataSource.SetCmdParam(cmd, "oldCnlStat", ev.OldCnlStat);
                            dataSource.SetCmdParam(cmd, "newCnlVal", ev.NewCnlVal);
                            dataSource.SetCmdParam(cmd, "newCnlStat", ev.NewCnlStat);
                            dataSource.SetCmdParam(cmd, "checked", ev.Checked);
                            dataSource.SetCmdParam(cmd, "userID", ev.UserID);
                            dataSource.SetCmdParam(cmd, "descr", ev.Descr);
                            dataSource.SetCmdParam(cmd, "data", ev.Data);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(string.Format(Localization.UseRussian ?
                                "Ошибка при экспорте события в БД {0}: {1}" :
                                "Error export event to DB {0}: {1}", dataSource.Name, ex.Message));
                        }
                        finally
                        {
                            Disconnect(dataSource);
                        }
                    }
                }
            }
        }
    }
}
