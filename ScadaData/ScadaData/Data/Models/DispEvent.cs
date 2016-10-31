/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Summary  : Displayed event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Displayed event
    /// <para>Отображаемое событие</para>
    /// </summary>
    /// <remarks>Свойства имеют короткие имена для передачи в формате JSON</remarks>
    public class DispEvent
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DispEvent()
        {
            Num = 0;
            Time = "";
            Obj = "";
            KP = "";
            Cnl = "";
            Text = "";
            Ack = "";
            Color = "";
            Sound = false;
        }


        /// <summary>
        /// Получить или установить порядковый номер
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// Получить или установить отформатированную дату и время
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Получить или установить наименование объекта
        /// </summary>
        public string Obj { get; set; }

        /// <summary>
        /// Получить или установить наименование КП
        /// </summary>
        public string KP { get; set; }

        /// <summary>
        /// Получить или установить наименование входного канала
        /// </summary>
        public string Cnl { get; set; }

        /// <summary>
        /// Получить или установить текст события
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Получить или установить информацию о квитировании
        /// </summary>
        public string Ack { get; set; }

        /// <summary>
        /// Получить или установить цвет
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Получить или установить признак воспроизведения звука
        /// </summary>
        public bool Sound { get; set; }
    }
}
