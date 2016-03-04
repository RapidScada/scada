/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : PlgSchemeCommon
 * Summary  : WCF service for interacting with the scheme JavaScript code
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Scheme;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// WCF service for interacting with the scheme JavaScript code
    /// <para>WCF-служба для взаимодействия с JavaScript-кодом схемы</para>
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchemeSvc
    {
        /// <summary>
        /// Ответ на запрос элементов схемы
        /// </summary>
        public class GetElementsResponse
        {

        }

        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        protected static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

        /// <summary>
        /// Получить элементы схемы
        /// </summary>
        [OperationContract]
        [WebGet]
        public string GetElements(int viewID, long viewStamp, int startIndex, int count)
        {
            try
            {
                // TODO: возвращаемая структура должна ещё содержать viewStamp и признак окончания чтения
                SchemeView schemeView = AppData.ViewCache.GetView<SchemeView>(viewID, true);

                List<SchemeView.Element> srcElems = schemeView.ElementList;
                List<SchemeView.Element> destElems = new List<SchemeView.Element>(count);
                int srcCnt = srcElems.Count;

                for (int i = startIndex, j = 0; i < srcCnt && j < count; i++, j++)
                    destElems.Add(schemeView.ElementList[i]);

                return JsSerializer.Serialize(destElems);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении элементов схемы с id={0}" :
                    "Error getting the elements of the scheme with id={0}");
                return "";
            }
        }
    }
}
