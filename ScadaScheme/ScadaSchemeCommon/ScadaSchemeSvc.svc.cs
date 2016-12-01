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
 * Summary  : WCF service for interacting of the application modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.SessionState;
using Scada.Client;
using Scada.Web;
using Utils;

namespace Scada.Scheme
{
    /// <summary>
    /// WCF service for interacting of the application modules
    /// <para>WCF-служба для взаимодействия модулей приложения</para>
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ScadaSchemeSvc : IScadaSchemeSvc
    {
        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        public SchemeSettings GetSettings()
        {
            return SchemeApp.GetSchemeApp().SchemeSettings;
        }

        /// <summary>
        /// Загрузить схему
        /// </summary>
        public bool LoadScheme(string clientID, int viewSetIndex, int viewIndex, out SchemeView.SchemeData schemeData)
        {
            bool result = false;
            schemeData = null;
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();

            if (schemeApp.WorkMode == SchemeApp.WorkModes.Edit)
            {
                // загрузка схемы для SCADA-Редактора схем
                EditorData editorData = schemeApp.EditorData;
                SchemeView schemeView = null;

                if (editorData.ClientID == clientID)
                    schemeView = editorData.SchemeView;
                else
                    editorData.ClientID = clientID;

                if (schemeView == null)
                {
                    string errMsg;
                    if (editorData.LoadSchemeFromFile(out errMsg))
                        schemeView = editorData.SchemeView;
                    else
                        schemeApp.Log.WriteAction(errMsg, Log.ActTypes.Exception);
                }

                if (schemeView != null)
                {
                    schemeData = new SchemeView.SchemeData(schemeView);
                    schemeData.CnlList = null;
                    result = true;
                }
            }
            else
            {
                // загрузка схемы для SCADA-Web
                UserData userData = UserData.GetUserData();
                HttpContext context = HttpContext.Current;

                if (userData.LoggedOn)
                {
                    BaseView baseView;
                    MainData.Right right;
                    bool ok = userData.GetView(typeof(SchemeView), viewSetIndex, viewIndex, out baseView, out right);
                    SchemeView schemeView = baseView as SchemeView;

                    if (ok && schemeView != null && right.ViewRight)
                    {
                        schemeApp.Log.WriteAction(string.Format(Localization.UseRussian ? 
                            "Загружена схема {0} пользователем {1}" : "Scheme {0} has been loaded by user {1}",
                            schemeView.ItfObjName, userData.UserLogin), Log.ActTypes.Action);

                        // сохранение списка входных каналов схемы в сессии
                        HttpSessionState session = context == null ? null : context.Session;
                        if (session != null && clientID != null)
                        {
                            Dictionary<string, object> schemeClients = 
                                session["ScadaSchemeClients"] as Dictionary<string, object>;
                            if (schemeClients == null)
                            {
                                schemeClients = new Dictionary<string, object>();
                                session.Add("ScadaSchemeClients", schemeClients);
                            }
                            schemeClients[clientID] = schemeView.CnlList;
                        }

                        // получение данных схемы
                        schemeData = new SchemeView.SchemeData(schemeView);
                        schemeData.CtrlRight = right.CtrlRight;
                        result = true;
                    }
                    else
                    {
                        string itfObjName = schemeView == null || string.IsNullOrEmpty(schemeView.ItfObjName) ?
                            "" : " " + schemeView.ItfObjName;
                        schemeApp.Log.WriteAction(string.Format(Localization.UseRussian ? 
                            "Не удалось загрузить схему{0} пользователем {1}" : "Unable to load scheme{0} by user {1}",
                            itfObjName, userData.UserLogin), Log.ActTypes.Error);
                    }
                }
                else
                {
                    HttpRequest request = context == null ? null : context.Request;
                    string host = request == null ? "" : 
                        (Localization.UseRussian ? ". Хост: " : ". Host: ") + request.UserHostAddress;
                    schemeApp.Log.WriteAction((Localization.UseRussian ? 
                        "Невозможно загрузить схему, т.к. пользователь не вошёл в систему" : 
                        "Unable to load scheme because user is not logged on") + host, Log.ActTypes.Error);
                }
            }

            return result;
        }

        /// <summary>
        /// Загрузить данные входных каналов, которые используются в схеме
        /// </summary>
        public bool LoadCnlData(string clientID, List<int> cnlList, out List<SchemeView.CnlData> cnlDataList)
        {
            // проверка режима работы приложения
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            if (schemeApp.WorkMode != SchemeApp.WorkModes.Monitor)
            {
                cnlDataList = null;
                return false;
            }

            // получение списка входных каналов, если он неопределён
            HttpContext context = HttpContext.Current;
            if (cnlList == null && context != null)
            {
                HttpSessionState session = context.Session;
                Dictionary<string, object> schemeClients = session == null ? null :
                    session["ScadaSchemeClients"] as Dictionary<string, object>;
                if (schemeClients != null && clientID != null)
                    cnlList = schemeClients[clientID] as List<int>;
            }

            if (cnlList == null)
            {
                HttpRequest request = context == null ? null : context.Request;
                string host = request == null ? "" :
                    (Localization.UseRussian ? ". Хост: " : ". Host: ") + request.UserHostAddress;
                schemeApp.Log.WriteAction((Localization.UseRussian ? "Не удалось получить список входных каналов" : 
                    "Unable to get input channels list") + host, Log.ActTypes.Error);
                cnlDataList = null;
                return false;
            }
            else
            {
                // получение данных входных каналов
                MainData mainData = schemeApp.MainData;
                mainData.RefreshData();
                cnlDataList = new List<SchemeView.CnlData>();

                foreach (int cnlNum in cnlList)
                {
                    double val;
                    int stat;
                    string color;
                    SchemeView.CnlData cnlData = new SchemeView.CnlData();
                    mainData.GetCurData(cnlNum, out val, out stat);
                    cnlData.Val = val;
                    cnlData.ValStr = mainData.GetCnlVal(cnlNum, false, out color);
                    cnlData.ValStrWithUnit = mainData.GetCnlVal(cnlNum, true, out color);
                    cnlData.Stat = stat;
                    cnlData.Color = color;
                    cnlDataList.Add(cnlData);
                }

                return true;
            }
        }

        /// <summary>
        /// Получить изменение схемы, которое необходимо отобразить, передав позицию указателя мыши
        /// </summary>
        public bool GetChange(string clientID, Point cursorPosition, out SchemeView.SchemeChange schemeChange)
        {
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            if (schemeApp.WorkMode == SchemeApp.WorkModes.Edit && schemeApp.EditorData.ClientID == clientID)
            {
                schemeApp.EditorData.CursorPosition = cursorPosition;
                schemeChange = schemeApp.EditorData.SchemeChange;
                return schemeChange != null && schemeChange.ChangeType != SchemeView.ChangeType.Unchanged;
            }
            else
            {
                schemeChange = null;
                return false;
            }
        }
        
        /// <summary>
        /// Очистить информацию об изменении схемы после её обработки
        /// </summary>
        public void ClearChange(string clientID)
        {
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            if (schemeApp.WorkMode == SchemeApp.WorkModes.Edit && schemeApp.EditorData.ClientID == clientID)
                schemeApp.EditorData.SchemeChange = null;
        }

        /// <summary>
        /// Выбрать элемент схемы
        /// </summary>
        public void SelectElement(string clientID, int elementID, int clickX, int clickY)
        {
            SchemeApp schemeApp = SchemeApp.GetSchemeApp();
            EditorData editorData = schemeApp.EditorData;

            if (schemeApp.WorkMode == SchemeApp.WorkModes.Edit && editorData.ClientID == clientID)
            {
                // выбор элемента или схемы в режиме редактирования
                SchemeView schemeView = editorData.SchemeView;
                SchemeView.Element addedElement = editorData.AddedElement;

                if (addedElement == null)
                {
                    SchemeView.Element elem;
                    if (elementID <= 0)
                        editorData.SelectElement(schemeView.SchemeParams);
                    else if (schemeView.ElementDict.TryGetValue(elementID, out elem))
                        editorData.SelectElement(elem);
                }
                else
                {
                    // добавление элемента в заданную позицию в режиме редактирования
                    addedElement.Location = new SchemeView.Point(clickX, clickY);
                    schemeView.ElementList.Add(addedElement);
                    schemeView.ElementDict[addedElement.ID] = addedElement;

                    // создание объекта для передачи изменений
                    SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ElementAdded);
                    change.ElementData = new SchemeView.ElementData(addedElement);
                    
                    try
                    {
                        // установка изменения, которая может вызвать исключение
                        editorData.SchemeChange = change;

                        // выбор добавленного элемента
                        editorData.SelectElement(addedElement);
                        editorData.AddedElement = null;
                    }
                    catch (Exception ex)
                    {
                        WriteException(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Записать исключение в журнал приложения
        /// </summary>
        public void WriteException(string message)
        {
            SchemeApp.GetSchemeApp().Log.WriteAction(message, Log.ActTypes.Exception);
        }
    }
}
