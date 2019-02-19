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
 * Module   : Administrator
 * Summary  : Represents additional options of a column
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents additional options of a column.
    /// <para>Представляет дополнительные параметры столбца.</para>
    /// </summary>
    internal class ColumnOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ColumnOptions()
        {
            Kind = ColumnKind.Unspecified;
            DefaultValue = null;
            MaxLength = 0;
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ColumnOptions(ColumnKind kind, int maxLength = 0)
            : this()
        {
            Kind = kind;
            MaxLength = maxLength;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ColumnOptions(int maxLength)
            : this(ColumnKind.Unspecified, maxLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <remarks>Use this constructor for primary key columns.</remarks>
        public ColumnOptions(int min, int max)
            : this()
        {
            Kind = ColumnKind.PrimaryKey;
            Minimum = min;
            Maximum = max;
        }


        /// <summary>
        /// Gets or sets the column kind.
        /// </summary>
        public ColumnKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the default value for the column.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of a text column.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed primary key value.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed primary key value.
        /// </summary>
        public int Maximum { get; set; }
    }
}
