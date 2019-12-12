/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Represents special paste parameters
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Represents special paste parameters.
    /// <para>Представляет параметры специальной вставки.</para>
    /// </summary>
    public class PasteSpecialParams
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PasteSpecialParams()
        {
            InCnlOffset = 0;
            CtrlCnlOffset = 0;
        }


        /// <summary>
        /// Gets or sets the offset of input channel numbers.
        /// </summary>
        public int InCnlOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of output channel numbers.
        /// </summary>
        public int CtrlCnlOffset { get; set; }
    }
}
