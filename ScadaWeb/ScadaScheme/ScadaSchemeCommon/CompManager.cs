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
 * Module   : ScadaSchemeCommon
 * Summary  : Manages scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme
{
    /// <summary>
    /// Manages scheme components
    /// <para>Менеджер, управляющий компонентами схемы</para>
    /// </summary>
    public sealed class CompManager
    {
        private static readonly CompManager instance; // экземпляр объекта менеджера

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static CompManager()
        {
            instance = new CompManager();
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта из других классов
        /// </summary>
        private CompManager()
        {
        }


        /// <summary>
        /// Получить фабрику компонентов по префиксу XML-элементов
        /// </summary>
        public CompFactory GetCompFactory(string xmlPrefix)
        {
            return null;
        }

        /// <summary>
        /// Получить единственный экземпляр менеджера
        /// </summary>
        public static CompManager GetInstance()
        {
            return instance;
        }
    }
}
