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
using Scada.Server.Module.DBExport;
using System;
using System.Data.Common;
using System.Text;
using Utils;

namespace Scada.Server.Module
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


        private bool normalWork;             // признак нормальной работы модуля
        private Log log;                     // журнал работы модуля
        private Config config;               // конфигурация модуля


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
        /// Выполнить действия при запуске работы сервера
        /// </summary>
        public override void OnServerStart()
        {
            // вывод в журнал
            log = new Log(Log.Formats.Simple);
            log.Encoding = Encoding.UTF8;
            log.FileName = LogDir + LogFileName;
            log.WriteBreak();
            log.WriteAction(string.Format(Localization.UseRussian ?
                "Запуск работы модуля {0}" : "Start {0} module", Name)); // !!! вынести в общие константы для сервера (рефакторинг констант)

            // загрука конфигурации
            config = new Config(ConfigDir);
            string errMsg;

            if (config.Load(out errMsg))
            {
                // инициализация источников данных
                foreach (Config.ExportDestination expDest in config.ExportDestinations)
                {
                    expDest.DataSource.InitConnection();
                    expDest.DataSource.InitCommands(expDest.ExportParams.ExportCurDataQuery,
                        expDest.ExportParams.ExportArcDataQuery, expDest.ExportParams.ExportEventQuery);
                }
            }
            else
            {
                normalWork = false;
                log.WriteAction(errMsg);
                log.WriteAction(Localization.UseRussian ?
                    "Нормальная работа модуля невозможна" : "Normal module execution is impossible"); // !!! вынести в общие константы
            }
        }

        /// <summary>
        /// Выполнить действия после обработки новых текущих данных
        /// </summary>
        public override void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
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
                            DbCommand cmd = dataSource.ExportCurDataCmd;

                            foreach (int cnlNum in cnlNums)
                            {
                                SrezTableLight.CnlData cnlData;

                                if (curSrez.GetCnlData(cnlNum, out cnlData))
                                {
                                    dataSource.SetCmdParam(cmd, "cnlNum", cnlNum);
                                    dataSource.SetCmdParam(cmd, "val", cnlData.Val);
                                    dataSource.SetCmdParam(cmd, "stat", cnlData.Stat);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteAction(ex.Message);
                        }
                        finally
                        {
                            dataSource.Disconnect();
                        }
                    }
                }
            }
        }
    }
}
