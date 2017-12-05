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
 * Module   : PlgScheme
 * Summary  : WCF service for interacting with the scheme JavaScript code
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2017
 */

using Scada.Scheme;
using Scada.Scheme.DataTransfer;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Web.Plugins.Scheme
{
    /// <summary>
    /// WCF service for interacting with the scheme JavaScript code
    /// <para>WCF-сервис для взаимодействия с JavaScript-кодом схемы</para>
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchemeSvc
    {
        /// <summary>
        /// Максимальное количество символов строке данных в формате JSON, 10 МБ
        /// </summary>
        private const int MaxJsonLen = 10485760;
        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLen };
        /// <summary>
        /// Общие данные веб-приложения
        /// </summary>
        private static readonly AppData AppData = AppData.GetAppData();


        /// <summary>
        /// Получить объект для передачи данных, содержащий информацию об ошибке, в формате JSON
        /// </summary>
        private string GetErrorDtoJs(Exception ex)
        {
            return JsSerializer.Serialize(new DataTransferObject(false, ex.Message));
        }
        
        /// <summary>
        /// Получить схему из кеша или от сервера с проверкой прав на неё
        /// </summary>
        private SchemeView GetSchemeView(int viewID, UserRights userRights)
        {
            if (!userRights.GetUiObjRights(viewID).ViewRight)
                throw new ScadaException(CommonPhrases.NoRights);

            return AppData.ViewCache.GetView<SchemeView>(viewID, true);
        }


        /// <summary>
        /// Получить свойства документа схемы
        /// </summary>
        /// <remarks>Возвращает SchemeDocDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetSchemeDoc(int viewID, long viewStamp)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                SchemeDocDTO dto = new SchemeDocDTO();
                dto.ViewStamp = schemeView.Stamp;

                if (SchemeUtils.ViewStampsMatched(viewStamp, schemeView.Stamp))
                    dto.SchemeDoc = schemeView.SchemeDoc;

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств документа схемы с ид.={0}" :
                    "Error getting document properties of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        /// <remarks>Возвращает ComponentsDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetComponents(int viewID, long viewStamp, int startIndex, int count)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                ComponentsDTO dto = new ComponentsDTO(count);
                dto.ViewStamp = schemeView.Stamp;

                if (SchemeUtils.ViewStampsMatched(viewStamp, schemeView.Stamp))
                    dto.CopyComponents(schemeView.Components.Values, startIndex, count);

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении компонентов схемы с ид.={0}" :
                    "Error getting components of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить изображения схемы
        /// </summary>
        /// <remarks>Возвращает ImagesDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetImages(int viewID, long viewStamp, int startIndex, int totalDataSize)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                ImagesDTO dto = new ImagesDTO();
                dto.ViewStamp = schemeView.Stamp;

                if (SchemeUtils.ViewStampsMatched(viewStamp, schemeView.Stamp))
                    dto.CopyImages(schemeView.SchemeDoc.Images.Values, startIndex, totalDataSize);

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изображений схемы с ид.={0}" :
                    "Error getting images of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить ошибки при загрузке схемы
        /// </summary>
        /// <remarks>Возвращает SchemeDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetLoadErrors(int viewID, long viewStamp)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                SchemeDTO dto = new SchemeDTO();
                dto.ViewStamp = schemeView.Stamp;

                if (SchemeUtils.ViewStampsMatched(viewStamp, schemeView.Stamp))
                    dto.Data = schemeView.LoadErrors.ToArray();

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении ошибок при загрузке схемы с ид.={0}" :
                    "Error getting loading errors of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }
    }
}
