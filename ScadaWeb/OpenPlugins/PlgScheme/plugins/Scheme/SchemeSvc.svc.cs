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
        /// Класс объекта для передачи элементов схемы
        /// </summary>
        private class ElementsDTO
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ElementsDTO(int capacity = -1)
            {
                ViewStamp = 0;
                EndOfScheme = false;
                Elements = capacity > 0 ? 
                    new List<SchemeView.Element>(capacity) : 
                    new List<SchemeView.Element>();
            }

            /// <summary>
            /// Получить или установить метку представления, уникальную в пределах приложения
            /// </summary>
            public long ViewStamp { get; set; }
            /// <summary>
            /// Получить или установить признак, что считаны все элементы схемы
            /// </summary>
            public bool EndOfScheme { get; set; }
            /// <summary>
            /// Получить элементы схемы
            /// </summary>
            public List<SchemeView.Element> Elements { get; private set; }
        }


        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer();


        /// <summary>
        /// Получить элементы схемы
        /// </summary>
        [OperationContract]
        [WebGet]
        public string GetElements(int viewID, long viewStamp, int startIndex, int count)
        {
            try
            {
                ElementsDTO dto = new ElementsDTO(count);
                SchemeView schemeView = AppData.ViewCache.GetView<SchemeView>(viewID, true);
                List<SchemeView.Element> srcElems = schemeView.ElementList;
                int srcCnt = srcElems.Count;

                dto.ViewStamp = schemeView.Stamp;
                dto.EndOfScheme = startIndex + count >= srcCnt;

                for (int i = startIndex, j = 0; i < srcCnt && j < count; i++, j++)
                    dto.Elements.Add(srcElems[i]);

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении элементов схемы с id={0}" :
                    "Error getting the elements of the scheme with id={0}", viewID);
                return "";
            }
        }
    }
}
