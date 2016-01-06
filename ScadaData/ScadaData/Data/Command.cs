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
 * Module   : ScadaData
 * Summary  : Telecontrol command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.Text;

namespace Scada.Data
{
    /// <summary>
    /// Telecontrol command
    /// <para>Команда ТУ</para>
    /// </summary>
    /// <remarks>Serializable attribute required for deep clone of an object
    /// <para>Атрибут Serializable необходим для глубокого клонирования объекта</para></remarks>
    [Serializable]
    public class Command
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Command()
            : this(BaseValues.CmdTypes.Standard)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Command(int cmdTypeID)
        {
            CreateDT = DateTime.Now;
            CmdTypeID = cmdTypeID;
            KPNum = 0;
            CmdNum = 0;
            CmdVal = 0.0;
            CmdData = null;
        }


        /// <summary>
        /// Получить дату и время создания команды
        /// </summary>
        public DateTime CreateDT { get; protected set; }

        /// <summary>
        /// Получить или установить идентификатор типа команды
        /// </summary>
        public int CmdTypeID { get; set; }

        /// <summary>
        /// Получить или установить номер КП
        /// </summary>
        public int KPNum { get; set; }

        /// <summary>
        /// Получить или установить номер команды
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Получить или установить значение команды
        /// </summary>
        public double CmdVal { get; set; }

        /// <summary>
        /// Получить или установить данные команды
        /// </summary>
        public byte[] CmdData { get; set; }


        /// <summary>
        /// Получить данные команды, преобразованные в строку
        /// </summary>
        public string GetCmdDataStr()
        {
            try { return CmdData == null ? "" : Encoding.Default.GetString(CmdData); }
            catch { return ""; }
        }

        /// <summary>
        /// Подготовить данные команды для передачи клиентам по TCP
        /// </summary>
        public void PrepareCmdData()
        {
            if (CmdTypeID == BaseValues.CmdTypes.Standard)
                CmdData = BitConverter.GetBytes(CmdVal);
            else if (CmdTypeID == BaseValues.CmdTypes.Request)
                CmdData = BitConverter.GetBytes((UInt16)KPNum);
        }

        /// <summary>
        /// Получить кодовое обозначение типа команды по идентификатору
        /// </summary>
        public string GetCmdTypeCode()
        {
            return BaseValues.CmdTypes.GetCmdTypeCode(CmdTypeID);
        }

        /// <summary>
        /// Получить описание команды
        /// </summary>
        public string GetCmdDescr()
        {
            return GetCmdDescr(0, 0);
        }

        /// <summary>
        /// Получить описание команды с указанием канала управления и пользователя
        /// </summary>
        public string GetCmdDescr(int ctrlCnlNum, int userID)
        {
            const int VisCmdDataLen = 10; // длина отображаемой части данных команды
            StringBuilder sb = new StringBuilder();

            if (Localization.UseRussian)
            {
                sb.Append("Команда ТУ: ");
                if (ctrlCnlNum > 0)
                    sb.Append("канал упр. = ").Append(ctrlCnlNum).Append(", ");
                if (userID > 0)
                    sb.Append("ид. польз. = ").Append(userID).Append(", ");
                sb.Append("тип = ").Append(BaseValues.CmdTypes.GetCmdTypeCode(CmdTypeID));
                if (KPNum > 0)
                    sb.Append(", номер КП = ").Append(KPNum);
                if (CmdTypeID == BaseValues.CmdTypes.Standard)
                    sb.Append(", значение = ").AppendFormat(CmdVal.ToString("N3", Localization.Culture));
                if (CmdTypeID == BaseValues.CmdTypes.Binary && CmdData != null)
                    sb.Append(", данные = ").Append(ScadaUtils.BytesToHex(CmdData, 0, 
                        Math.Min(VisCmdDataLen, CmdData.Length)));
            }
            else
            {
                sb.Append("Command: ");
                if (ctrlCnlNum > 0)
                    sb.Append("out channel = ").Append(ctrlCnlNum).Append(", ");
                if (userID > 0)
                    sb.Append("user ID = ").Append(userID).Append(", ");
                sb.Append("type = ").Append(BaseValues.CmdTypes.GetCmdTypeCode(CmdTypeID));
                if (KPNum > 0)
                    sb.Append(", device num. = ").Append(KPNum);
                if (CmdTypeID == BaseValues.CmdTypes.Standard)
                    sb.Append(", value = ").AppendFormat(CmdVal.ToString("N3", Localization.Culture));
                if (CmdTypeID == BaseValues.CmdTypes.Binary && CmdData != null)
                    sb.Append(", data = ").Append(ScadaUtils.BytesToHex(CmdData, 0,
                        Math.Min(VisCmdDataLen, CmdData.Length)));
            }

            return sb.ToString();
        }
    }
}
