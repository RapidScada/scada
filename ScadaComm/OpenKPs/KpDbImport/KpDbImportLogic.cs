/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpDBImport
 * Summary  : Device communication logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

using Scada.Comm.Devices.DbImport.Configuration;
using Scada.Comm.Devices.DbImport.Data;
using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device communication logic.
    /// <para>Логика работы КП.</para>
    /// </summary>
    public class KpDbImportLogic : KPLogic
    {
        /// <summary>
        /// Supported tag types.
        /// </summary>
        private enum TagType { Number, String, DateTime };


        private DataSource dataSource; // the data source
        private TagType[] tagTypes;    // the types of the device tags


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KpDbImportLogic(int number)
            : base(number)
        {
            ConnRequired = false;

            dataSource = null;
            tagTypes = null;
        }


        /// <summary>
        /// Initializes the data source.
        /// </summary>
        private void InitDataSource(KpConfig config)
        {
            switch (config.DataSourceType)
            {
                case DataSourceType.MSSQL:
                    dataSource = new SqlDataSource();
                    break;
                case DataSourceType.Oracle:
                    dataSource = new OraDataSource();
                    break;
                case DataSourceType.PostgreSQL:
                    dataSource = new PgSqlDataSource();
                    break;
                case DataSourceType.MySQL:
                    dataSource = new MySqlDataSource();
                    break;
                case DataSourceType.OLEDB:
                    dataSource = new OleDbDataSource();
                    break;
                default:
                    dataSource = null;
                    WriteToLog(Localization.UseRussian ?
                        "Data source type is not set or not supported" :
                        "Тип источника данных не задан или не поддерживается");
                    break;
            }

            if (dataSource != null)
            {
                string connStr = string.IsNullOrEmpty(config.DbConnSettings.ConnectionString) ?
                    dataSource.BuildConnectionString(config.DbConnSettings) :
                    config.DbConnSettings.ConnectionString;

                if (string.IsNullOrEmpty(connStr))
                {
                    dataSource = null;
                    WriteToLog(Localization.UseRussian ?
                        "Соединение не определено" :
                        "Connection is undefined");
                }
                else
                {
                    dataSource.Init(connStr, config);
                }
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        private void InitDeviceTags(KpConfig config)
        {
            string[] tagNames = GetTagNames(config);
            int tagCnt = tagNames.Length;
            List<KPTag> kpTags = new List<KPTag>(tagCnt);
            tagTypes = new TagType[tagCnt];

            for (int i = 0; i < tagCnt; i++)
            {
                kpTags.Add(new KPTag(i + 1, tagNames[i]));
                tagTypes[i] = TagType.Number;
            }

            InitKPTags(kpTags);
        }

        /// <summary>
        /// Decodes the specified object and converts it to a tag data.
        /// </summary>
        private SrezTableLight.CnlData DecodeTag(object val, out TagType tagType)
        {
            try
            {
                if (val == DBNull.Value)
                {
                    tagType = TagType.Number;
                    return SrezTableLight.CnlData.Empty;
                }
                else if (val is string)
                {
                    tagType = TagType.String;
                    return new SrezTableLight.CnlData(ScadaUtils.EncodeAscii((string)val), BaseValues.CnlStatuses.Defined);
                }
                else if (val is DateTime)
                {
                    tagType = TagType.DateTime;
                    return new SrezTableLight.CnlData(ScadaUtils.EncodeDateTime((DateTime)val), BaseValues.CnlStatuses.Defined);
                }
                else
                {
                    tagType = TagType.Number;
                    return new SrezTableLight.CnlData(Convert.ToDouble(val), BaseValues.CnlStatuses.Defined);
                }
            }
            catch
            {
                tagType = TagType.Number;
                return SrezTableLight.CnlData.Empty;
            }
        }

        /// <summary>
        /// Validates the data source.
        /// </summary>
        private bool ValidateDataSource()
        {
            if (dataSource == null)
            {
                WriteToLog(Localization.UseRussian ?
                    "Нормальное взаимодействие с КП невозможно, т.к. источник данных не определён" :
                    "Normal device communication is impossible because data source is undefined");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Validates the database command.
        /// </summary>
        private bool ValidateCommand(DbCommand dbCommand)
        {
            if (dbCommand == null)
            {
                WriteToLog(Localization.UseRussian ?
                    "Нормальное взаимодействие с КП невозможно, т.к. SQL-команда не определена" :
                    "Normal device communication is impossible because SQL command is undefined");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Connects to the database.
        /// </summary>
        private bool Connect()
        {
            try
            {
                dataSource.Connect();
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ? 
                    "Ошибка при соединении с БД: {0}" :
                    "Error connecting to DB: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                dataSource.Disconnect();
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ? 
                    "Ошибка при разъединении с БД: {0}" :
                    "Error disconnecting from DB: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Requests data from the database.
        /// </summary>
        private bool Request()
        {
            try
            {
                WriteToLog(Localization.UseRussian ?
                    "Запрос данных" :
                    "Data request");

                using (DbDataReader reader = dataSource.SelectCommand.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        WriteToLog(CommPhrases.ResponseOK);

                        int tagCnt = KPTags.Length;
                        int fieldCnt = reader.FieldCount;

                        for (int i = 0, cnt = Math.Min(tagCnt, fieldCnt); i < cnt; i++)
                        {
                            KPTags[i].Name = reader.GetName(i);
                            SetCurData(i, DecodeTag(reader[i], out TagType tagType));
                            tagTypes[i] = tagType;
                        }

                        InvalidateCurData(tagCnt, fieldCnt - tagCnt);
                    }
                    else
                    {
                        WriteToLog(Localization.UseRussian ?
                            "Данные отсутствуют" :
                            "No data available");
                        InvalidateCurData();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка при выполнении запроса: {0}" :
                    "Error executing query: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Sends the command to the database.
        /// </summary>
        private bool SendDbCommand(DbCommand dbCommand)
        {
            try
            {
                WriteToLog(Localization.UseRussian ?
                    "Запрос на изменение данных" :
                    "Data modification request");
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "Ошибка при отправке команды БД: {0}" :
                    "Error sending command to the database: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Converts the tag data to string.
        /// </summary>
        protected override string ConvertTagDataToStr(int signal, SrezTableLight.CnlData tagData)
        {
            if (tagData.Stat > 0)
            {
                switch (tagTypes[signal - 1])
                {
                    case TagType.String:
                        return ScadaUtils.DecodeAscii(tagData.Val);
                    case TagType.DateTime:
                        return ScadaUtils.DecodeDateTime(tagData.Val).ToLocalizedString();
                }
            }

            return base.ConvertTagDataToStr(signal, tagData);
        }


        /// <summary>
        /// Performs a communication session with the device.
        /// </summary>
        public override void Session()
        {
            base.Session();
            lastCommSucc = false;

            if (ValidateDataSource() && ValidateCommand(dataSource.SelectCommand))
            {
                // request data
                int tryNum = 0;

                while (RequestNeeded(ref tryNum))
                {
                    if (Connect() && Request())
                        lastCommSucc = true;

                    Disconnect();
                    FinishRequest();
                    tryNum++;
                }

                if (!lastCommSucc && !Terminated)
                    InvalidateCurData();
            }
            else
            {
                Thread.Sleep(ReqParams.Delay);
            }

            // calculate session stats
            CalcSessStats();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCmd(Command cmd)
        {
            base.SendCmd(cmd);

            if (CanSendCmd)
            {
                lastCommSucc = false;

                if ((cmd.CmdTypeID == BaseValues.CmdTypes.Standard || cmd.CmdTypeID == BaseValues.CmdTypes.Binary) &&
                    (dataSource.ExportCommands.TryGetValue(cmd.CmdNum, out DbCommand dbCommand) || 
                    dataSource.ExportCommands.TryGetValue(0, out dbCommand)))
                {
                    if (ValidateDataSource() && ValidateCommand(dbCommand))
                    {
                        dataSource.SetCmdParam(dbCommand, "cmdVal", 
                            cmd.CmdTypeID == BaseValues.CmdTypes.Standard ? (object)cmd.CmdVal : cmd.GetCmdDataStr());
                        dataSource.SetCmdParam(dbCommand, "cmdNum", cmd.CmdNum);
                        int tryNum = 0;

                        while (RequestNeeded(ref tryNum))
                        {
                            if (Connect() && SendDbCommand(dbCommand))
                                lastCommSucc = true;

                            Disconnect();
                            FinishRequest();
                            tryNum++;
                        }
                    }
                }
                else
                {
                    WriteToLog(CommPhrases.IllegalCommand);
                }
            }

            CalcCmdStats();
        }

        /// <summary>
        /// Performs actions after adding the device to a communication line.
        /// </summary>
        public override void OnAddedToCommLine()
        {
            // load configuration
            KpConfig config = new KpConfig();

            if (config.Load(KpConfig.GetFileName(AppDirs.ConfigDir, Number), out string errMsg))
            {
                InitDataSource(config);
                InitDeviceTags(config);
                CanSendCmd = config.ExportCmds.Count > 0;
            }
            else
            {
                dataSource = null;
                WriteToLog(errMsg);
            }
        }


        /// <summary>
        /// Gets an array of tag names according to the configuration.
        /// </summary>
        internal static string[] GetTagNames(KpConfig config)
        {
            int tagCount = config.AutoTagCount ? config.CalcTagCount() : config.TagCount;
            string[] tagNames = new string[tagCount];

            for (int i = 0;  i < tagCount; i++)
            {
                tagNames[i] = "Tag " + (i + 1);
            }

            return tagNames;
        }
    }
}
