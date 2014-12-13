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
 * Module   : ScadaWebCommon
 * Summary  : Application user data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2007
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Scada.Client;
using Utils;

namespace Scada.Web
{
    /// <summary>
    /// Application user data
    /// <para>Данные пользователя приложения</para>
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// Права на набор представлений и входящие в него представления
        /// </summary>
        private class ViewSetRight
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public ViewSetRight(ViewSettings.ViewSet viewSet)
            {
                ViewSet = viewSet;
                Right = MainData.Right.NoRights;
                ViewRightArr = null;
            }

            /// <summary>
            /// Получить набор представлений
            /// </summary>
            public ViewSettings.ViewSet ViewSet { get; private set; }
            /// <summary>
            /// Получить или установить права на набор представлений
            /// </summary>
            public MainData.Right Right { get; set; }
            /// <summary>
            /// Получить или установить массив прав на представления из набора
            /// </summary>
            public MainData.Right[] ViewRightArr { get; set; }
        }


        private SortedList<string, MainData.Right> rightList; // список прав пользователя на объекты интерфейса
        private List<ViewSetRight> viewSetRightList; // список прав на наборы представлений и представления


        /// <summary>
        /// Конструктор
        /// </summary>
        private UserData()
        {
            ViewSettings = new ViewSettings();
            Logout();
        }


        /// <summary>
        /// Получить имя пользователя
        /// </summary>
        public string UserLogin { get; private set; }

        /// <summary>
        /// Получить идентификатор пользователя в базе конфигурации
        /// </summary>
        public int UserID { get; private set; }

        /// <summary>
        /// Получить роль пользователя
        /// </summary>
        public ServerComm.Roles Role { get; private set; }

        /// <summary>
        /// Получить идентификатор роли пользователя
        /// </summary>
        public int RoleID { get; private set; }

        /// <summary>
        /// Получить наименование роли пользователя
        /// </summary>
        public string RoleName { get; private set; }

        /// <summary>
        /// Получить признак, выполнен ли вход пользователя в систему
        /// </summary>
        public bool LoggedOn { get; private set; }

        /// <summary>
        /// Получить дату и время входа пользователя в систему
        /// </summary>
        public DateTime LogOnDT { get; private set; }
        
        /// <summary>
        /// Получить настройки представлений
        /// </summary>
        public ViewSettings ViewSettings { get; private set; }


        /// <summary>
        /// Инициализировать список прав на наборы представлений и входящие в них представления
        /// </summary>
        private void InitViewSetRightList(List<ViewSettings.ViewSet> viewSetList)
        {
            viewSetRightList = new List<ViewSetRight>();

            if (viewSetList != null)
            {
                foreach (ViewSettings.ViewSet viewSet in viewSetList)
                {
                    ViewSetRight viewSetRight = new ViewSetRight(viewSet);
                    viewSetRight.Right = GetRight(viewSet.Name);
                    viewSetRightList.Add(viewSetRight);
                }
            }
        }
        
        /// <summary>
        /// Инициализировать права на представления из набора
        /// </summary>
        private void InitViewRightArr(ViewSetRight viewSetRight)
        {
            ViewSettings.ViewSet viewSet = viewSetRight.ViewSet;

            if (viewSet != null && viewSet.Count > 0)
            {
                bool viewSetViewRight = viewSetRight.Right.ViewRight;
                bool viewSetCtrlRight = viewSetRight.Right.CtrlRight;
                int viewCnt = viewSet.Count;
                MainData.Right[] viewRightArr = new MainData.Right[viewCnt];

                for (int i = 0; i < viewCnt; i++)
                {
                    MainData.Right right = GetRight(Path.GetFileName(viewSet[i].FileName));
                    viewRightArr[i].ViewRight = right.ViewRight && viewSetViewRight;
                    viewRightArr[i].CtrlRight = right.CtrlRight && viewSetCtrlRight;
                }

                viewSetRight.ViewRightArr = viewRightArr;
            }
        }
        

        /// <summary>
        /// Выполнить вход пользователя в систему
        /// </summary>
        /// <remarks>Если пароль равен null, то он не проверяется</remarks>
        public bool Login(string login, string password, out string errMsg)
        {
            login = login == null ? "" : login.Trim();
            int roleID;

            if (AppData.MainData.CheckUser(login, password, password != null, out roleID, out errMsg))
            {
                UserLogin = login;
                Role = ServerComm.GetRole(roleID);
                RoleID = roleID;
                RoleName = AppData.MainData.GetRoleName(RoleID);
                UserID = AppData.MainData.GetUserID(login);

                LoggedOn = true;
                LogOnDT = DateTime.Now;
                rightList = AppData.MainData.GetRightList(roleID);
                InitViewSetRightList(ViewSettings.ViewSetList);

                AppData.Log.WriteAction((password == null ? 
                    (Localization.UseRussian ? "Вход в систему без пароля: " : "Login without a password: ") : 
                    (Localization.UseRussian ? "Вход в систему: " : "Login: ")) +
                    login + " (" + RoleName + ")", Log.ActTypes.Action);
                return true;
            }
            else
            {
                Logout();

                string err = login == "" ? errMsg : login + " - " + errMsg;
                AppData.Log.WriteAction((Localization.UseRussian ? "Неудачная попытка входа в систему: " : 
                    "Unsuccessful login attempt: ") + err, Log.ActTypes.Error);
                return false;
            }
        }

        /// <summary>
        /// Выполнить вход пользователя в систему без проверки пароля
        /// </summary>
        public bool Login(string login)
        {
            string errMsg;
            return Login(login, null, out errMsg);
        }

        /// <summary>
        /// Завершить работу пользователя с определёнными ранее именем
        /// </summary>
        public void Logout()
        {
            UserLogin = "";
            UserID = 0;
            Role = ServerComm.Roles.Disabled;
            RoleID = (int)Role;
            RoleName = "";
            LoggedOn = false;
            LogOnDT = DateTime.MinValue;
            ViewSettings.ClearViewCash();

            rightList = null;
            viewSetRightList = null;
        }

        /// <summary>
        /// Получить права пользователя на объект интерфейса
        /// </summary>
        public MainData.Right GetRight(string itfObjName)
        {
            MainData.Right right;

            if (Role == ServerComm.Roles.Custom)
            {
                if (rightList == null || !rightList.TryGetValue(itfObjName, out right))
                    right = MainData.Right.NoRights;
            }
            else
            {
                right = new MainData.Right();
                right.CtrlRight = Role == ServerComm.Roles.Admin || Role == ServerComm.Roles.Dispatcher;
                right.ViewRight = right.CtrlRight || Role == ServerComm.Roles.Guest;
            }

            return right;
        }

        /// <summary>
        /// Получить права пользователя на набор представлений
        /// </summary>
        public MainData.Right GetViewSetRight(int viewSetIndex)
        {
            return viewSetRightList != null && 0 <= viewSetIndex && viewSetIndex < viewSetRightList.Count ?
                viewSetRightList[viewSetIndex].Right : MainData.Right.NoRights;
        }

        /// <summary>
        /// Получить права пользователя на представление
        /// </summary>
        public MainData.Right GetViewRight(int viewSetIndex, int viewIndex)
        {
            MainData.Right right = MainData.Right.NoRights;

            if (viewSetRightList != null && 0 <= viewSetIndex && viewSetIndex < viewSetRightList.Count)
            {
                ViewSetRight viewSetRight = viewSetRightList[viewSetIndex];

                if (viewSetRight.ViewRightArr == null)
                    InitViewRightArr(viewSetRight);

                MainData.Right[] viewRightArr = viewSetRight.ViewRightArr;

                if (viewRightArr != null && 0 <= viewIndex && viewIndex < viewRightArr.Length)
                    right = viewRightArr[viewIndex];
            }

            return right;
        }

        /// <summary>
        /// Получить представление заданного типа и права на него
        /// </summary>
        public bool GetView(Type viewType, int viewSetIndex, int viewIndex, 
            out BaseView view, out MainData.Right right)
        {
            bool result = false;
            view = null;
            right = MainData.Right.NoRights;

            try
            {
                if (viewSetRightList != null && 0 <= viewSetIndex && viewSetIndex < viewSetRightList.Count)
                {
                    ViewSetRight viewSetRight = viewSetRightList[viewSetIndex];
                    ViewSettings.ViewSet viewSet = viewSetRight.ViewSet;

                    if (viewSetRight.ViewRightArr == null)
                        InitViewRightArr(viewSetRight);

                    MainData.Right[] viewRightArr = viewSetRight.ViewRightArr;

                    if (viewSet != null && viewRightArr != null && 0 <= viewIndex && 
                        viewIndex < viewSet.Count && viewIndex < viewRightArr.Length)
                    {
                        ViewSettings.ViewInfo viewInfo = viewSet[viewIndex];
                        right = viewRightArr[viewIndex];

                        if (viewType == null)
                        {
                            view = viewInfo.ViewCash;
                            return view != null;
                        }
                        else if (viewInfo.Type == viewType.Name)
                        {
                            if (viewInfo.ViewCash != null && viewInfo.ViewCash.GetType() == viewType)
                            {
                                view = viewInfo.ViewCash;
                                result = true;
                            }
                            else
                            {
                                view = (BaseView)Activator.CreateInstance(viewType);

                                if (!view.StoredOnServer)
                                    view.ItfObjName = Path.GetFileName(viewInfo.FileName);

                                if (!view.StoredOnServer || 
                                    AppData.MainData.ServerComm.ReceiveView(viewSet.Directory + viewInfo.FileName, view))
                                {
                                    AppData.MainData.RefreshBase();
                                    view.BindCnlProps(AppData.MainData.CnlPropsArr);
                                    viewInfo.ViewCash = view;
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteAction((Localization.UseRussian ? "Ошибка при получении представления: " : 
                    "Error getting view: ") + ex.Message, Log.ActTypes.Exception);
            }

            return result;
        }


        /// <summary>
        /// Получить данные пользователя приложения
        /// </summary>
        /// <remarks>Для веб-приложения данные пользователя сохраняются в сессии</remarks>
        public static UserData GetUserData()
        {
            HttpSessionState session = HttpContext.Current == null ? null : HttpContext.Current.Session;
            UserData userData = session == null ? null : session["UserData"] as UserData;

            if (userData == null)
            {
                AppData.InitAppData();
                userData = new UserData();

                if (session != null)
                    session.Add("UserData", userData);

                // загрузка настроек представлений
                string errMsg;
                if (!userData.ViewSettings.LoadFromFile(AppData.ConfigDir + ViewSettings.DefFileName, out errMsg))
                    AppData.Log.WriteAction(errMsg, Log.ActTypes.Exception);
            }

            return userData;
        }
    }
}