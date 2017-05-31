/*
 * Copyright 2017 Mikhail Shiryaev
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
 * Module   : Scheme Editor
 * Summary  : The class for transfer form state
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// The class for transfer form state
    /// <para>Класс для передачи состояния формы</para>
    /// </summary>
    public class FormStateDTO
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FormStateDTO()
        {
            StickToLeft = false;
            StickToRight = false;
            Width = 0;
        }


        /// <summary>
        /// Получить или установить признак, что форма прикреплена к левому краю экрана
        /// </summary>
        public bool StickToLeft { get; set; }

        /// <summary>
        /// Получить или установить признак, что форма прикреплена к правому краю экрана
        /// </summary>
        public bool StickToRight { get; set; }

        /// <summary>
        /// Получить или установить ширину формы
        /// </summary>
        public int Width { get; set; }
    }
}
