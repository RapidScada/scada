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
 * Module   : ModTest
 * Summary  : Server module user interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2014
 */

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module user interface
    /// <para>Пользовательский интерфейс серверного модуля</para>
    /// </summary>
    public class ModTestView : ModView
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ModTestView()
        {
            CanShowProps = true;
        }

        /// <summary>
        /// Получить описание модуля
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ? "Серверный модуль для тестирования." : "Server module for testing.";
            }
        }

        /// <summary>
        /// Отобразить свойства модуля
        /// </summary>
        public override void ShowProps()
        {
            FrmAbout frmAbout = new FrmAbout();
            frmAbout.ShowDialog();
        }
    }
}
