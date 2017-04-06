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
 * Summary  : WCF service for interacting with the editor JavaScript code
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.DataTransfer;
using Scada.Scheme.Model;
using Scada.Web;
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// WCF service for interacting with the editor JavaScript code
    /// <para>WCF-сервис для взаимодействия с JavaScript-кодом редактор</para>
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchemeEditorSvc
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
        /// Общие данные приложения
        /// </summary>
        private static readonly AppData AppData = AppData.GetAppData();
        /// <summary>
        /// Редактор
        /// </summary>
        private static readonly Editor Editor = AppData.Editor;


        /// <summary>
        /// Разрешить кросс-доменный доступ к сервису
        /// </summary>
        private void AllowAccess()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin: *");
        }


        /// <summary>
        /// Временно
        /// </summary>
        [OperationContract]
        [WebGet]
        public string DoWork(string arg)
        {
            AllowAccess();
            return JsSerializer.Serialize("Test Result, arg = " + arg);
        }

        /// <summary>
        /// Получить свойства документа схемы
        /// </summary>
        /// <remarks>Возвращает SchemeDocDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetSchemeDoc(string editorID)
        {
            try
            {
                AllowAccess();
                SchemeDocDTO dto;

                if (editorID == Editor.EditorID)
                {
                    SchemeView srcSchemeView = Editor.SchemeView;
                    dto = srcSchemeView == null ?
                        new SchemeDocDTO() :
                        new SchemeDocDTO(srcSchemeView.SchemeDocument) { ViewStamp = srcSchemeView.Stamp };
                }
                else
                {
                    dto = new SchemeDocDTO() { EditorUnknown = true };
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении документа схемы" :
                    "Error getting scheme document properties");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Получить компоненты схемы
        /// </summary>
        /// <remarks>Возвращает ComponentsDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetComponents(string editorID, long viewStamp, int startIndex, int count)
        {
            return "";
        }

        /// <summary>
        /// Получить изображения схемы
        /// </summary>
        /// <remarks>Возвращает ImagesDTO в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetImages(string editorID, long viewStamp, int startIndex, int totalDataSize)
        {
            return "";
        }
    }
}
