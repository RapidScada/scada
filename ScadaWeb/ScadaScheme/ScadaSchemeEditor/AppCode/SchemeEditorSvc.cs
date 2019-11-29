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
 * Module   : PlgScheme
 * Summary  : WCF service for interacting with the editor JavaScript code
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Scheme.DataTransfer;
using Scada.Scheme.Model;
using Scada.Web;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// WCF service for interacting with the editor JavaScript code.
    /// <para>WCF-сервис для взаимодействия с JavaScript-кодом редактора.</para>
    /// </summary>
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchemeEditorSvc
    {
        /// <summary>
        /// Максимальное количество символов строке данных в формате JSON, 10 МБ.
        /// </summary>
        private const int MaxJsonLen = 10485760;
        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса.
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLen };
        /// <summary>
        /// Общие данные приложения.
        /// </summary>
        private static readonly AppData AppData = AppData.GetAppData();
        /// <summary>
        /// Редактор.
        /// </summary>
        private static readonly Editor Editor = AppData.Editor;


        /// <summary>
        /// Разрешить кросс-доменный доступ к сервису.
        /// </summary>
        private void AllowAccess()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin: *");
        }

        /// <summary>
        /// Проверить аргументы метода сервиса.
        /// </summary>
        private bool CheckArguments(string editorID, long viewStamp, SchemeDTO dto)
        {
            if (editorID == Editor.EditorID)
            {
                SchemeView schemeView = Editor.SchemeView;

                if (schemeView != null)
                {
                    dto.ViewStamp = schemeView.Stamp;

                    if (SchemeUtils.ViewStampsMatched(viewStamp, schemeView.Stamp))
                        return true;
                }
            }
            else
            {
                dto.EditorUnknown = true;
            }

            return false;
        }


        /// <summary>
        /// Получить свойства документа схемы.
        /// </summary>
        /// <remarks>Возвращает SchemeDocDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string GetSchemeDoc(string editorID, long viewStamp)
        {
            try
            {
                AllowAccess();
                SchemeDocDTO dto = new SchemeDocDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    lock (Editor.SchemeView.SyncRoot)
                    {
                        dto.SchemeDoc = Editor.SchemeView.SchemeDoc;
                    }
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
        /// Получить компоненты схемы.
        /// </summary>
        /// <remarks>Возвращает ComponentsDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string GetComponents(string editorID, long viewStamp, int startIndex, int count)
        {
            try
            {
                AllowAccess();
                ComponentsDTO dto = new ComponentsDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    lock (Editor.SchemeView.SyncRoot)
                    {
                        dto.CopyComponents(Editor.SchemeView.Components.Values, startIndex, count);
                    }
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении компонентов схемы" :
                    "Error getting scheme components");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Получить изображения схемы.
        /// </summary>
        /// <remarks>Возвращает ImagesDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string GetImages(string editorID, long viewStamp, int startIndex, int totalDataSize)
        {
            try
            {
                AllowAccess();
                ImagesDTO dto = new ImagesDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    lock (Editor.SchemeView.SyncRoot)
                    {
                        dto.CopyImages(Editor.SchemeView.SchemeDoc.Images.Values, startIndex, totalDataSize);
                    }
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изображений схемы" :
                    "Error getting scheme images");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Получить ошибки при загрузке схемы.
        /// </summary>
        /// <remarks>Возвращает SchemeDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string GetLoadErrors(string editorID, long viewStamp)
        {
            try
            {
                AllowAccess();
                SchemeDTO dto = new SchemeDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    lock (Editor.SchemeView.SyncRoot)
                    {
                        dto.Data = Editor.SchemeView.LoadErrors.ToArray();
                    }
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении ошибок при загрузке схемы" :
                    "Error getting loading errors of the scheme");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Получить изменения схемы.
        /// </summary>
        /// <remarks>Возвращает ChangesDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string GetChanges(string editorID, long viewStamp, long changeStamp, string status)
        {
            try
            {
                AllowAccess();
                ChangesDTO dto = new ChangesDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    dto.Changes = Editor.GetChanges(changeStamp);
                    dto.SelCompIDs = Editor.GetSelectedComponentIDs();
                    dto.NewCompMode = Editor.PointerMode != PointerMode.Select;
                    dto.EditorTitle = Editor.Title;
                    dto.FormState = AppData.MainForm.GetFormState();
                    Editor.Status = status;
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изменений схемы" :
                    "Error getting scheme chages");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Добавить компонент на схему.
        /// </summary>
        /// <remarks>Возвращает SchemeDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string AddComponent(string editorID, long viewStamp, int x, int y)
        {
            try
            {
                AllowAccess();
                SchemeDTO dto = new SchemeDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                {
                    switch (Editor.PointerMode)
                    {
                        case PointerMode.Create:
                            dto.Success = Editor.CreateComponent(x, y);
                            break;
                        case PointerMode.Paste:
                            dto.Success = Editor.PasteFromClipboard(x, y, false);
                            break;
                        case PointerMode.PasteSpecial:
                            dto.Success = Editor.PasteFromClipboard(x, y, true);
                            break;
                    }
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при добавлении компонента на схему" :
                    "Error adding component to the scheme");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Изменить выбор компонентов схемы.
        /// </summary>
        /// <remarks>Возвращает SchemeDTO в формате в JSON.</remarks>
        [OperationContract]
        [WebGet]
        public string ChangeSelection(string editorID, long viewStamp, string action, int componentID)
        {
            try
            {
                AllowAccess();
                SchemeDTO dto = new SchemeDTO();

                if (CheckArguments(editorID, viewStamp, dto) &&
                    Enum.TryParse(action, true, out SelectAction selectAction))
                {
                    Editor.PerformSelectAction(selectAction, componentID);
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при изменении выбора компонентов схемы" :
                    "Error changing scheme component selection");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Переместить и изменить размер выбранных компонентов схемы.
        /// </summary>
        [OperationContract]
        [WebGet]
        public string MoveResize(string editorID, long viewStamp, int dx, int dy, int w, int h)
        {
            try
            {
                AllowAccess();
                SchemeDTO dto = new SchemeDTO();

                if (CheckArguments(editorID, viewStamp, dto))
                    Editor.MoveResizeSelected(dx, dy, w, h, false);

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при перемещении и изменении размера выбранных компонентов схемы" :
                    "Error moving and resizing selected scheme components");
                return JsSerializer.GetErrorJson(ex);
            }
        }

        /// <summary>
        /// Выполнить действие главной формы.
        /// </summary>
        [OperationContract]
        [WebGet]
        public string FormAction(string editorID, long viewStamp, string action)
        {
            try
            {
                AllowAccess();
                SchemeDTO dto = new SchemeDTO();

                if (CheckArguments(editorID, viewStamp, dto) &&
                    Enum.TryParse(action, true, out FormAction formAction))
                {
                    AppData.MainForm.PerformAction(formAction);
                }

                return JsSerializer.Serialize(dto);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при выполнении действия главной формы" :
                    "Error performing action of the main form");
                return JsSerializer.GetErrorJson(ex);
            }
        }
    }
}
