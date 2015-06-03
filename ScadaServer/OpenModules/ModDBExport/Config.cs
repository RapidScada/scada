/*
 * Модуль экспорта в БД
 * Конфигурация модуля
 *  
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Server.Module.DBExport
{
    internal class Config
    {
        /// <summary>
        /// Параметры экспорта
        /// </summary>
        public class ExportParams
        {
            public bool ExportCurData
            {
                get
                {
                    return string.IsNullOrEmpty(ExportCurDataQuery);
                }
            }
            public bool ExportArcData
            {
                get
                {
                    return string.IsNullOrEmpty(ExportArcDataQuery);
                }
            }
            public bool ExportEvent
            {
                get
                {
                    return string.IsNullOrEmpty(ExportEventQuery);
                }
            }

            public string ExportCurDataQuery { get; set; }
            public string ExportArcDataQuery { get; set; }
            public string ExportEventQuery { get; set; }
        }

        /// <summary>
        /// Назначение экспорта
        /// </summary>
        public class ExportDestination
        {
            public DataSource DataSource { get; set; }
            public ExportParams ExportParams { get; set; }
        }


        /// <summary>
        /// Имя файла конфигурации
        /// </summary>
        private const string ConfigFileName = "ModDBExportControl.xml";


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private Config()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Config(string configDir)
        {
            FileName = ScadaUtils.NormalDir(configDir) + ConfigFileName;
            SetToDefault();
        }


        /// <summary>
        /// Получить полное имя файла конфигурации
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Получить назначения экспорта
        /// </summary>
        public List<ExportDestination> ExportDestinations { get; private set; }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            if (ExportDestinations == null)
                ExportDestinations = new List<ExportDestination>();
            else
                ExportDestinations.Clear();
        }

        /// <summary>
        /// Загрузить конфигурацию модуля
        /// </summary>
        public bool Load(out string errMsg)
        {
            // временно
            ExportDestination expDest = new ExportDestination();
            expDest.DataSource = new SqlDataSource()
            {
                ConnectionString = ""
            };
            expDest.ExportParams = new ExportParams()
            {
                ExportCurDataQuery = "INSERT INTO CnlData(DateTime, CnlNum, Val, Stat) VALUES (GETDATE(), {0}, {1}, {2})"
            };

            ExportDestinations.Add(expDest);

            errMsg = "";
            return true;
        }
    }
}
