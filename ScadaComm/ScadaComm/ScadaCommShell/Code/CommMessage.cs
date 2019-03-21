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
 * Module   : Communicator Shell
 * Summary  : Messages sent by Communicator forms
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm.Shell.Code
{
    /// <summary>
    /// Messages sent by Communicator forms.
    /// <para>Сообщения, отправляемые формами Коммуникатора.</para>
    /// </summary>
    public static class CommMessage
    {
        public const string SaveSettings = "Comm.SaveSettings";
        public const string UpdateLineParams = "Comm.UpdateLineParams";
        public const string UpdateFileName = "Comm.UpdateFileName";
    }
}
