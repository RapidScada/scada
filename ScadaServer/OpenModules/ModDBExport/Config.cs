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
 * Summary  : Module configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Scada.Server.Modules.DBExport
{
    /// <summary>
    /// Module configuration
    /// <para>Конфигурация модуля</para>
    /// </summary>
    [Serializable]
    internal class Config
    {
        /// <summary>
        /// Контроль привязки сериализованных объектов к типам
        /// </summary>
        /// <remarks>Класс необходим из-за ошибки в .NET и должен быть расположен именно в данной сборке</remarks>
        private class ConfigBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                try
                {
                    return Assembly.GetExecutingAssembly().GetType(typeName, true, true);
                }
                catch
                {
                    ScadaUtils.CorrectTypeName(ref typeName);
                    return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), true, true);
                }
            }
        }

        /// <summary>
        /// Параметры экспорта
        /// </summary>
        [Serializable]
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
        [Serializable]
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
            expDest.DataSource = new MySqlDataSource()
            {
                Server = "localhost",
                Database = "moddbexport",
                User = "root",
                Password = "mylittlesql"
            };
            expDest.ExportParams = new ExportParams()
            {
                ExportCurDataQuery = "INSERT INTO CnlData(DateTime, CnlNum, Val, Stat) VALUES (NOW(), @cnlNum, @val, @stat)"
            };

            ExportDestinations.Add(expDest);

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Клонировать конфигурацию модуля
        /// </summary>
        public Config Clone()
        {
            return (Config)ScadaUtils.DeepClone(this, new ConfigBinder());
        }
    }
}
