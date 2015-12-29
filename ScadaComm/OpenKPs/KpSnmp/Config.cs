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
 * Module   : KpSnmp
 * Summary  : Device communication configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices.KpSnmp
{
    /// <summary>
    /// Device communication configuration
    /// <para>Конфигурация связи с КП</para>
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// Группа переменных
        /// </summary>
        public class Group
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Group()
            {
                Name = "";
                Variables = new List<Variable>();
            }

            /// <summary>
            /// Получить или установить имя группы
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить переменные
            /// </summary>
            public List<Variable> Variables { get; private set; }
        }

        /// <summary>
        /// Переменная
        /// </summary>
        public class Variable
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Variable()
            {
                Name = "";
                OID = "";
            }

            /// <summary>
            /// Получить или установить имя переменной
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить идентификатор переменной
            /// </summary>
            public string OID { get; set; }
        }

        /// <summary>
        /// Пароль на чтение данных по умоланию
        /// </summary>
        public const string DefReadCommunity = "public";
        /// <summary>
        /// Пароль на запись данных по умоланию
        /// </summary>
        public const string DefWriteCommunity = "private";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Config()
        {
            SetToDefault();
        }


        /// <summary>
        /// Получить или установить пароль на чтение данных
        /// </summary>
        public string ReadCommunity { get; set; }

        /// <summary>
        /// Получить или установить пароль на запись данных
        /// </summary>
        public string WriteCommunity { get; set; }

        /// <summary>
        /// Получить граппы переменных
        /// </summary>
        public List<Group> Groups { get; private set; }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            ReadCommunity = DefReadCommunity;
            WriteCommunity = DefWriteCommunity;
            Groups = new List<Group>();
        }
        
        /// <summary>
        /// Загрузить конфигурацию из файла
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            SetToDefault();

            try
            {
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.LoadKpSettingsError + ":\r\n" + ex.Message;
                return false;
            }
        }

        
        /// <summary>
        /// Сохранить конфигурацию в файле
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommPhrases.SaveKpSettingsError + ":\r\n" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Получить имя файла конфигурации
        /// </summary>
        public static string GetFileName(string configDir, int kpNum)
        {
            return configDir + "KpSnmp_" + CommUtils.AddZeros(kpNum, 3) + ".xml";
        }
    }
}
