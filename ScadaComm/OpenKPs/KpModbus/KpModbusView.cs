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
 * Module   : KpModbus
 * Summary  : Device driver user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2021
 */

using Scada.Comm.Devices.Modbus.Protocol;
using Scada.Comm.Devices.Modbus.UI;
using Scada.Data.Configuration;
using Scada.UI;
using System.Collections.Generic;
using System.IO;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device driver user interface.
    /// <para>Пользовательский интерфейс драйвера КП.</para>
    /// </summary>
    public class KpModbusView : KPView
    {
        /// <summary>
        /// The driver version.
        /// </summary>
        internal const string KpVersion = "5.1.3.1";

        /// <summary>
        /// The UI customization object.
        /// </summary>
        private static readonly UiCustomization UiCustomization = new UiCustomization();


        /// <summary>
        /// Initializes a new instance of the class. Designed for general configuring.
        /// </summary>
        public KpModbusView()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class. Designed for configuring a particular device.
        /// </summary>
        public KpModbusView(int number)
            : base(number)
        {
            CanShowProps = true;
        }


        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ? 
                    "Взаимодействие с контроллерами по протоколу Modbus.\n\n" +
                    "Пользовательский параметр линии связи:\n" +
                    "TransMode - режим передачи данных (RTU, ASCII, TCP).\n\n" + 
                    "Параметр командной строки:\n" +
                    "имя файла шаблона.\n\n" +
                    "Команды ТУ:\n" +
                    "определяются шаблоном (стандартные или бинарные)." :

                    "Interacting with controllers via Modbus protocol.\n\n" +
                    "Custom communication line parameter:\n" +
                    "TransMode - data transmission mode (RTU, ASCII, TCP).\n\n" +
                    "Command line parameter:\n" +
                    "template file name.\n\n" +
                    "Commands:\n" +
                    "defined by template (standard or binary).";
            }
        }

        /// <summary>
        /// Gets the driver version.
        /// </summary>
        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }

        /// <summary>
        /// Gets the default channel prototypes.
        /// </summary>
        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                // загрузка шаблона устройства
                string fileName = KPProps == null ? "" : KPProps.CmdLine.Trim();

                if (fileName == "")
                    return null;

                string filePath = Path.IsPathRooted(fileName) ? fileName : Path.Combine(AppDirs.ConfigDir, fileName);
                DeviceTemplate deviceTemplate = GetUiCustomization().TemplateFactory.CreateDeviceTemplate();

                if (!deviceTemplate.Load(filePath, out string errMsg))
                    throw new ScadaException(errMsg);

                // создание прототипов каналов КП
                return CreateCnlPrototypes(deviceTemplate);
            }
        }


        /// <summary>
        /// Creates channel prototypes based on the device template.
        /// </summary>
        protected virtual KPCnlPrototypes CreateCnlPrototypes(DeviceTemplate deviceTemplate)
        {
            KPCnlPrototypes prototypes = new KPCnlPrototypes();
            List<InCnlPrototype> inCnls = prototypes.InCnls;
            List<CtrlCnlPrototype> ctrlCnls = prototypes.CtrlCnls;

            // создание прототипов входных каналов
            int signal = 1;
            foreach (ElemGroup elemGroup in deviceTemplate.ElemGroups)
            {
                bool isTS =
                    elemGroup.TableType == TableType.DiscreteInputs ||
                    elemGroup.TableType == TableType.Coils;

                foreach (Elem elem in elemGroup.Elems)
                {
                    InCnlPrototype inCnl = isTS ?
                        new InCnlPrototype(elem.Name, BaseValues.CnlTypes.TS) :
                        new InCnlPrototype(elem.Name, BaseValues.CnlTypes.TI);
                    inCnl.Signal = signal++;

                    if (isTS)
                    {
                        inCnl.ShowNumber = false;
                        inCnl.UnitName = BaseValues.UnitNames.OffOn;
                        inCnl.EvEnabled = true;
                        inCnl.EvOnChange = true;
                    }

                    inCnls.Add(inCnl);
                }
            }

            // создание прототипов каналов управления
            foreach (ModbusCmd modbusCmd in deviceTemplate.Cmds)
            {
                CtrlCnlPrototype ctrlCnl = modbusCmd.TableType == TableType.Coils && modbusCmd.Multiple ?
                    new CtrlCnlPrototype(modbusCmd.Name, BaseValues.CmdTypes.Binary) :
                    new CtrlCnlPrototype(modbusCmd.Name, BaseValues.CmdTypes.Standard);
                ctrlCnl.CmdNum = modbusCmd.CmdNum;

                if (modbusCmd.TableType == TableType.Coils && !modbusCmd.Multiple)
                    ctrlCnl.CmdValName = BaseValues.CmdValNames.OffOn;

                ctrlCnls.Add(ctrlCnl);
            }

            return prototypes;
        }

        /// <summary>
        /// Localizes the driver UI.
        /// </summary>
        protected virtual void Localize()
        {
            if (!Localization.LoadDictionaries(AppDirs.LangDir, "KpModbus", out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            KpPhrases.Init();
        }

        /// <summary>
        /// Gets a UI customization object.
        /// </summary>
        protected virtual UiCustomization GetUiCustomization()
        {
            return UiCustomization;
        }


        /// <summary>
        /// Shows the driver properties.
        /// </summary>
        public override void ShowProps()
        {
            Localize();

            if (Number > 0)
            {
                // show properties of the particular device
                FrmDevProps.ShowDialog(Number, KPProps, AppDirs, GetUiCustomization());
            }
            else
            {
                // show the device template editor
                FrmDevTemplate.ShowDialog(AppDirs, GetUiCustomization());
            }
        }
    }
}
