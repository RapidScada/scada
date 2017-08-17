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
 * Module   : KpTest
 * Summary  : Device library user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2015
 */

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Device library user interface
    /// <para>Пользовательский интерфейс библиотеки КП</para>
    /// </summary>
    public sealed class KpTestView : KPView
    {
        internal const string KpVersion = "5.0.0.1";

        public KpTestView()
            : this(0)
        {
        }

        public KpTestView(int number)
            : base(number)
        {
        }

        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ?
                    "Библиотека КП для тестирования.\n\n" +
                    "Команды ТУ:\n" +
                    "1 (бинарная) - отправка данных как строки,\n" +
                    "2 (бинарная) - отправка данных как массива байт." :

                    "Device library for testing.\n\n" +
                    "Commands:\n" +
                    "1 (binary) - send data as string.\n" +
                    "2 (binary) - send data as array of bytes.";
            }
        }

        public override string Version
        {
            get
            {
                return KpVersion;
            }
        }
    }
}
