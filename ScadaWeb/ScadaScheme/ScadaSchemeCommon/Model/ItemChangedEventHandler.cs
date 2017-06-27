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
 * Summary  : Represents a method that will handle an event raised when a scheme item is changed
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Model
{
    /// <summary>
    /// Represents a method that will handle an event raised when a scheme item is changed
    /// <para>Представляет метод для обработки события, возникающего при изменении элемента схемы</para>
    /// </summary>
    public delegate void ItemChangedEventHandler(
        object sender, 
        SchemeChangeTypes changeType,
        object changedObject,
        object oldKey);
}
