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
 * Module   : Administrator
 * Summary  : Recently selected objects
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2020
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Recently selected objects.
    /// <para>Недавно выбранные объекты.</para>
    /// </summary>
    public class RecentSelection
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RecentSelection()
        {
            Reset();
        }


        /// <summary>
        /// Gets or sets the name of the recently selected instance.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the number of the recently selected communication line.
        /// </summary>
        public int CommLineNum { get; set; }

        /// <summary>
        /// Gets or sets the ID of the recently selected device.
        /// </summary>
        public int KPNum { get; set; }

        /// <summary>
        /// Gets or sets the ID of the recently selected device type.
        /// </summary>
        public int KPTypeID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the recently selected object.
        /// </summary>
        public int ObjNum { get; set; }


        /// <summary>
        /// Resets the selected objects.
        /// </summary>
        public void Reset()
        {
            InstanceName = "";
            CommLineNum = 0;
            KPNum = 0;
            KPTypeID = 0;
            ObjNum = 0;
        }
    }
}
