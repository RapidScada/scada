/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Summary  : Interface of the WCF service for interacting of the application modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;

namespace Scada.Scheme
{
    /// <summary>
    ///  Interface of the WCF service for interacting of the application modules
    /// <para>Интерфейс WCF-службы для взаимодействия модулей приложения</para>
    /// </summary>
    [ServiceContract]
    public interface IScadaSchemeSvc
    {
        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        [OperationContract]
        SchemeSettings GetSettings();

        /// <summary>
        /// Загрузить схему
        /// </summary>
        /// <remarks>Клиент должен сгенерировать и передать свой уникальный идентификатор, 
        /// по которому сервер сохраняет список используемых входных каналов</remarks>
        [OperationContract]
        bool LoadScheme(string clientID, int viewSetIndex, int viewIndex, out SchemeView.SchemeData schemeData);

        /// <summary>
        /// Загрузить данные входных каналов, которые используются в схеме
        /// </summary>
        /// <remarks>Список каналов необходимо указывать при повторном вызове метода в том случае, 
        /// если не удалось получить данные без указания списка каналов</remarks>
        [OperationContract]
        bool LoadCnlData(string clientID, List<int> cnlList, out List<SchemeView.CnlData> cnlDataList);

        /// <summary>
        /// Получить изменение схемы, которое необходимо отобразить, передав позицию указателя мыши
        /// </summary>
        /// <remarks>Метод используется в режиме редактирования схемы</remarks>
        [OperationContract]
        bool GetChange(string clientID, Point cursorPosition, out SchemeView.SchemeChange schemeChange);

        /// <summary>
        /// Очистить информацию об изменении схемы после её обработки
        /// </summary>
        /// <remarks>Метод используется в режиме редактирования схемы</remarks>
        [OperationContract]
        void ClearChange(string clientID);

        /// <summary>
        /// Выбрать элемент схемы
        /// </summary>
        /// <remarks>Метод используется в режиме редактирования схемы</remarks>
        [OperationContract]
        void SelectElement(string clientID, int elementID, int clickX, int clickY);

        /// <summary>
        /// Записать исключение в журнал приложения
        /// </summary>
        [OperationContract]
        void WriteException(string message);
    }
}