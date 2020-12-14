/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : PlgScheme
 * Summary  : Represents scheme options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using System;
using System.Globalization;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// Represents scheme options.
    /// <para>Представляет параметры схемы.</para>
    /// </summary>
    internal class SchemeOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SchemeOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            ScaleType = options.GetValueAsEnum("ScaleType", ScaleType.Numeric);
            ScaleValue = options.GetValueAsDouble("ScaleValue", 100) / 100;
            RememberScale = options.GetValueAsBool("RememberScale", true);
        }


        /// <summary>
        /// Gets or sets the scale type.
        /// </summary>
        public ScaleType ScaleType { get; set; }

        /// <summary>
        /// Gets or sets the scale value.
        /// </summary>
        public double ScaleValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember last scheme scale.
        /// </summary>
        public bool RememberScale { get; set; }


        /// <summary>
        /// Converts the options to JavaScript.
        /// </summary>
        public string ToJs()
        {
            return 
                "{ " +
                $"scaleType: {(int)ScaleType}, " +
                $"scaleValue: {ScaleValue.ToString(CultureInfo.InvariantCulture)}, " +
                $"rememberScale: {RememberScale.ToString().ToLowerInvariant()} " +
                "}";
        }
    }
}
