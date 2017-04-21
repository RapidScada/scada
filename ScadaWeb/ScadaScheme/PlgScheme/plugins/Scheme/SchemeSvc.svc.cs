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
 * Module   : PlgScheme
 * Summary  : WCF service for interacting with the scheme JavaScript code
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Scheme;
using Scada.Scheme.DataTransfer;
using Scada.Scheme.Model;
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
        /// Получить свойства схемы
        /// </summary>
        /// <remarks>Возвращает SchemePropsDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetSchemeProps(int viewID, long viewStamp)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                SchemeDocDTO dto = new SchemeDocDTO();
                dto.ViewStamp = schemeView.Stamp;

                if (viewStamp == 0 || viewStamp == dto.ViewStamp)
                    dto.SchemeDoc = schemeView.SchemeDoc;

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств схемы с ид.={0}" :
                    "Error getting the properties of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        /// <remarks>Возвращает ComponentsDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetElements(int viewID, long viewStamp, int startIndex, int count)
        {
            try
            {
                UserRights userRights;
                AppData.CheckLoggedOn(out userRights);

                SchemeView schemeView = GetSchemeView(viewID, userRights);
                ComponentsDTO dto = new ComponentsDTO(count);
                dto.ViewStamp = schemeView.Stamp;

                if (viewStamp == 0 || viewStamp == dto.ViewStamp)
                {
                    List<BaseComponent> srcComponents = schemeView.Components;
                    int srcCnt = srcComponents.Count;
                    dto.EndOfComponents = startIndex + count >= srcCnt;

                    for (int i = startIndex, j = 0; i < srcCnt && j < count; i++, j++)
                        dto.Components.Add(srcComponents[i]);
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении компонентов схемы с ид.={0}" :
                    "Error getting the components of the scheme with ID={0}", viewID);
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

                if (viewStamp == 0 || viewStamp == dto.ViewStamp)
                {
                    Dictionary<string, Image>.ValueCollection images = schemeView.SchemeDoc.Images.Values;
                    int i = 0;
                    int size = 0;

                    foreach (Image image in images)
                    {
                        if (i >= startIndex)
                        {
                            dto.Images.Add(new ImageDTO(image));
                            if (image.Data != null)
                                size += image.Data.Length;
                        }

                        if (size >= totalDataSize)
                            break;

                        i++;
                    }

                    dto.EndOfImages = i == images.Count;
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изображений схемы с ид.={0}" :
                    "Error getting the images of the scheme with ID={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }
    }
}
