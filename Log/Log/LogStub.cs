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
 * Module   : Log
 * Summary  : Log stub
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using System;

namespace Utils
{
    /// <summary>
    /// Log stub
    /// <para>Заглушка журнала</para>
    /// </summary>
    public class LogStub : ILog
    {
        /// <summary>
        /// Записать действие определённого типа в журнал
        /// </summary>
        public void WriteAction(string text, Log.ActTypes actType)
        {
        }

        /// <summary>
        /// Записать информационное действие в журнал
        /// </summary>
        public void WriteInfo(string text)
        {
        }

        /// <summary>
        /// Записать обычное действие в журнал
        /// </summary>
        public void WriteAction(string text)
        {
        }

        /// <summary>
        /// Записать ошибку в журнал
        /// </summary>
        public void WriteError(string text)
        {
        }

        /// <summary>
        /// Записать исключение в журнал
        /// </summary>
        public void WriteException(Exception ex, string errMsg = "", params object[] args)
        {
        }

        /// <summary>
        /// Записать строку в журнал
        /// </summary>
        public void WriteLine(string text = "")
        {
        }

        /// <summary>
        /// Записать разделитель в журнал
        /// </summary>
        public void WriteBreak()
        {
        }
    }
}
