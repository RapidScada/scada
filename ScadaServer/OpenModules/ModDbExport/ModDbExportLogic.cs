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
 * Summary  : Server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Server.Modules.DbExport;
using Scada.Server.Modules.DbExport.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module logic.
    /// <para>Логика работы серверного модуля.</para>
    /// </summary>
    public class ModDbExportLogic : ModLogic
    {
        /// <summary>
        /// The log file name.
        /// </summary>
        private const string LogFileName = "ModDbExport.log";

        private Log log;                  // the module log
        private List<Exporter> exporters; // the active exporters


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDbExportLogic()
        {
            log = null;
            exporters = null;
        }


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "ModDbExport";
            }
        }


        /// <summary>
        /// Loads a map of channels and devices.
        /// </summary>
        private bool LoadEntityMap(out EntityMap entityMap)
        {
            try
            {
                BaseTable<InCnl> inCnlTable = new BaseTable<InCnl>("InCnl", "CnlNum", CommonPhrases.InCnlTable);

                BaseAdapter adapter = new BaseAdapter()
                {
                    Directory = Settings.BaseDATDir,
                    TableName = "incnl.dat"
                };

                adapter.Fill(inCnlTable, false);
                entityMap = new EntityMap();
                entityMap.Init(inCnlTable);

                log.WriteAction(Localization.UseRussian ?
                    "Карта каналов и КП загружена" :
                    "Map of channels and devices loaded");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при загрузке карты каналов и КП" :
                    "Error loading map of channels and devices");
                entityMap = null;
                return false;
            }
        }

        /// <summary>
        /// Performs actions when starting Server.
        /// </summary>
        public override void OnServerStart()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            // write to log
            log = new Log(Log.Formats.Simple) { FileName = Path.Combine(AppDirs.LogDir, LogFileName) };
            log.WriteBreak();
            log.WriteAction(string.Format(ModPhrases.StartModule, Name));

            // load configuration
            exporters = new List<Exporter>();
            ModConfig config = new ModConfig();
            string errMsg = "";

            if (LoadEntityMap(out EntityMap entityMap) &&
                config.Load(Path.Combine(AppDirs.ConfigDir, ModConfig.ConfigFileName), out errMsg) &&
                config.Validate(out List<ExportTargetConfig> activeExportConfigs, out errMsg))
            {
                // create and start exporters
                log.WriteAction(Localization.UseRussian ?
                    "Запуск экспорта" :
                    "Start export");

                foreach (ExportTargetConfig exporterConfig in activeExportConfigs)
                {
                    try
                    {
                        Exporter exporter = new Exporter(exporterConfig, entityMap, ServerData, AppDirs, Settings.ArcDir);
                        exporters.Add(exporter);
                        exporter.Start();
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, Localization.UseRussian ?
                            "Ошибка при создании экспортёра" :
                            "Error creating exporter");
                    }
                }
            }
            else
            {
                if (errMsg != "")
                    log.WriteError(errMsg);

                log.WriteError(ModPhrases.NormalModExecImpossible);
            }
        }

        /// <summary>
        /// Performs actions when Server stops.
        /// </summary>
        public override void OnServerStop()
        {
            // stop exporters
            log.WriteAction(Localization.UseRussian ?
                "Остановка экспорта" :
                "Stop export");

            foreach (Exporter exporter in exporters)
            {
                exporter.Stop();
            }

            // write to log
            log.WriteAction(string.Format(ModPhrases.StopModule, Name));
            log.WriteBreak();
        }

        /// <summary>
        /// Performs actions after receiving and processing new current data.
        /// </summary>
        public override void OnCurDataProcessed(int[] cnlNums, SrezTableLight.Srez curSrez)
        {
            if (exporters.Count > 0)
            {
                SrezTableLight.Srez snapshot = new SrezTableLight.Srez(DateTime.Now, cnlNums, curSrez);

                foreach (Exporter exporter in exporters)
                {
                    exporter.EnqueueCurData(snapshot);
                }
            }
        }

        /// <summary>
        /// Performs actions after receiving and processing new archive data.
        /// </summary>
        public override void OnArcDataProcessed(int[] cnlNums, SrezTableLight.Srez arcSrez)
        {
            if (exporters.Count > 0 && arcSrez != null)
            {
                SrezTableLight.Srez snapshot = new SrezTableLight.Srez(arcSrez.DateTime, cnlNums, arcSrez);

                foreach (Exporter exporter in exporters)
                {
                    exporter.EnqueueArcData(snapshot);
                }
            }
        }

        /// <summary>
        /// Performs actions after creating an event.
        /// </summary>
        public override void OnEventCreated(EventTableLight.Event ev)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueEvent(ev);
            }
        }

        /// <summary>
        /// Performs actions after receiving the telecontrol command.
        /// </summary>
        public override void OnCommandReceived(int ctrlCnlNum, Command cmd, int userID, ref bool passToClients)
        {
            foreach (Exporter exporter in exporters)
            {
                exporter.EnqueueCmd(ctrlCnlNum, cmd, ref passToClients);
            }
        }
    }
}
