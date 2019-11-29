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
 * Summary  : Specifies the functionality provided by the main form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Specifies the functionality provided by the main form
    /// <para>Определяет функционал, предоставляемый главной формой</para>
    /// </summary>
    public interface IMainForm
    {
        /// <summary>
        /// Выполнить заданное действие
        /// </summary>
        void PerformAction(FormAction formAction);

        /// <summary>
        /// Получить состояние формы
        /// </summary>
        FormStateDTO GetFormState();
    }
}
