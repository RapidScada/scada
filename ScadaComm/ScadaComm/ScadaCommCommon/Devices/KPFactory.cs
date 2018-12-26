/*
 * Copyright 2015 Mikhail Shiryaev
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
 * Module   : ScadaCommCommon
 * Summary  : Factory for creating device class instances
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System;
using System.IO;
using System.Reflection;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Factory for creating device instances
    /// <para>Фабрика для создания экземпляров классов КП</para>
    /// </summary>
    public static class KPFactory
    {
        /// <summary>
        /// Получить экземпляр класса логики КП, загрузив его тип из библиотеки
        /// </summary>
        public static KPLogic GetKPLogic(string kpDir, string dllName, int kpNum)
        {
            if (kpDir == null)
                throw new ArgumentNullException("kpDir");
            if (dllName == null)
                throw new ArgumentNullException("dllName");
            
            Type kpLogicType;

            try
            {
                Assembly asm = Assembly.LoadFile(kpDir + dllName);
                string typeFullName = "Scada.Comm.Devices." + Path.GetFileNameWithoutExtension(dllName) + "Logic";
                kpLogicType = asm.GetType(typeFullName, true);
            }
            catch (Exception ex)
            {
                throw new ScadaException(string.Format(Localization.UseRussian ?
                    "Ошибка при получении типа логики КП из библиотеки {0}: {1}" :
                    "Error getting device logic type from the library {0}: {1}", dllName, ex.Message), ex);
            }

            return GetKPLogic(kpLogicType, kpNum);
        }

        /// <summary>
        /// Получить экземпляр класса логики КП заданного типа
        /// </summary>
        public static KPLogic GetKPLogic(Type kpLogicType, int kpNum)
        {
            if (kpLogicType == null)
                throw new ArgumentNullException("kpLogicType");

            try
            {
                return (KPLogic)Activator.CreateInstance(kpLogicType, kpNum);
            }
            catch (Exception ex)
            {
                throw new ScadaException(string.Format(Localization.UseRussian ?
                    "Ошибка при создании экземпляра класса логики КП {0}: {1}" :
                    "Error creating device logic instance of the class {0}: {1}", kpLogicType.Name, ex.Message), ex);
            }
        }

        /// <summary>
        /// Получить тип интерфейса КП из библиотеки
        /// </summary>
        public static Type GetKPViewType(string dllPath)
        {
            try
            {
                Assembly asm = Assembly.LoadFile(dllPath);
                string typeFullName = "Scada.Comm.Devices." + Path.GetFileNameWithoutExtension(dllPath) + "View";
                return asm.GetType(typeFullName, true);
            }
            catch (Exception ex)
            {
                throw new ScadaException(string.Format(CommPhrases.GetViewTypeError, 
                    Path.GetFileName(dllPath), ex.Message), ex);
            }
        }
        
        /// <summary>
        /// Получить экземпляр класса интерфейса КП, загрузив его тип из библиотеки
        /// </summary>
        public static KPView GetKPView(string dllPath, int kpNum = 0)
        {
            Type kpViewType = GetKPViewType(dllPath);
            return GetKPView(kpViewType, kpNum);
        }

        /// <summary>
        /// Получить экземпляр класса интерфейса КП заданного типа
        /// </summary>
        public static KPView GetKPView(Type kpViewType, int kpNum = 0)
        {
            if (kpViewType == null)
                throw new ArgumentNullException("kpViewType");

            try
            {
                return kpNum > 0 ? 
                    (KPView)Activator.CreateInstance(kpViewType, kpNum) :
                    (KPView)Activator.CreateInstance(kpViewType);
            }
            catch (Exception ex)
            {
                throw new ScadaException(string.Format(CommPhrases.CreateViewError, 
                    kpViewType.Name, ex.Message), ex);
            }
        }
    }
}
