/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Handy and thread safe access to the client cache data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Data.Configuration;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using Utils;

namespace Scada.Client
{
    /// <summary>
    /// Handy and thread safe access to the client cache data.
    /// <para>Удобный и потокобезопасный доступ к данным кэша клиентов.</para>
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// Кэш данных.
        /// </summary>
        protected readonly DataCache dataCache;
        /// <summary>
        /// Журнал.
        /// </summary>
        protected readonly Log log;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров.
        /// </summary>
        protected DataAccess()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataAccess(DataCache dataCache, Log log)
        {
            if (dataCache == null)
                throw new ArgumentNullException("dataCache");
            if (log == null)
                throw new ArgumentNullException("log");

            this.dataCache = dataCache;
            this.log = log;
        }


        /// <summary>
        /// Получить кэш данных.
        /// </summary>
        public DataCache DataCache
        {
            get
            {
                return dataCache;
            }
        }


        /// <summary>
        /// Создать свойства объекта интерфейса на основе строки таблицы интерфейса.
        /// </summary>
        /// <param name="rowView">The row from the Interface table.</param>
        /// <param name="v58plus">Indicates whether the version of Rapid SCADA 5.8 or higher.</param>
        protected UiObjProps GetUiObjFromRow(DataRowView rowView, bool v58plus)
        {
            UiObjProps uiObjProps = UiObjProps.Parse((string)rowView["Name"]);
            uiObjProps.UiObjID = (int)rowView["ItfID"];
            uiObjProps.Title = (string)rowView["Descr"];

            if (v58plus)
            {
                string typeCode = (string)rowView["TypeCode"];
                if (!string.IsNullOrEmpty(typeCode))
                {
                    uiObjProps.TypeCode = typeCode;
                    uiObjProps.BaseUiType = UiObjProps.GetBaseUiType(typeCode);
                }

                uiObjProps.Args = (string)rowView["Args"];
                uiObjProps.Hidden = (bool)rowView["Hidden"];
                uiObjProps.ObjNum = (int)rowView["ObjNum"];
            }

            return uiObjProps;
        }

        /// <summary>
        /// Gets the parent roles of the specified role including the specified role itself.
        /// </summary>
        protected List<int> GetParentRoles(int roleID)
        {
            HashSet<int> roleIDSet = new HashSet<int>(); // set of parent roles and the specified role
            List<int> roleIDList = new List<int>();      // similar role list ordered by inheritance

            roleIDSet.Add(roleID); // to avoid infinite loop

            // the RoleRef table has been added since version 5.8
            if (BaseTables.CheckColumnsExist(dataCache.BaseTables.RoleRefTable))
            {
                dataCache.BaseTables.RoleRefTable.DefaultView.Sort = "ChildRoleID";
                AppendParentRoles(roleIDSet, roleIDList, roleID);
            }

            roleIDList.Add(roleID);
            return roleIDList;
        }

        /// <summary>
        /// Appends parent role IDs recursively.
        /// </summary>
        protected void AppendParentRoles(HashSet<int> roleIDSet, List<int> roleIDList, int childRoleID)
        {
            foreach (DataRowView rowView in dataCache.BaseTables.RoleRefTable.DefaultView.FindRows(childRoleID))
            {
                int parentRoleID = (int)rowView["ParentRoleID"];

                if (roleIDSet.Add(parentRoleID))
                {
                    roleIDList.Add(parentRoleID);
                    AppendParentRoles(roleIDSet, roleIDList, parentRoleID);
                }
            }
        }

        /// <summary>
        /// Получить наименование роли по идентификатору из базы конфигурации.
        /// </summary>
        protected string GetRoleNameFromBase(int roleID, string defaultRoleName)
        {
            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.RoleTable, true);
                    DataView viewRole = baseTables.RoleTable.DefaultView;
                    viewRole.Sort = "RoleID";
                    int rowInd = viewRole.Find(roleID);
                    return rowInd >= 0 ? (string)viewRole[rowInd]["Name"] : defaultRoleName;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении наименования роли по идентификатору {0}" :
                    "Error getting role name by ID {0}", roleID);
                return defaultRoleName;
            }
        }


        /// <summary>
        /// Получить свойства входного канала по его номеру.
        /// </summary>
        public InCnlProps GetCnlProps(int cnlNum)
        {
            try
            {
                if (cnlNum > 0)
                {
                    dataCache.RefreshBaseTables();

                    // необходимо сохранить ссылку, т.к. объект может быть пересоздан другим потоком
                    InCnlProps[] cnlProps = dataCache.CnlProps;

                    // поиск свойств заданного канала
                    int ind = Array.BinarySearch(cnlProps, cnlNum, InCnlProps.IntComp);
                    return ind >= 0 ? cnlProps[ind] : null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств входного канала {0}" :
                    "Error getting input channel {0} properties", cnlNum);
                return null;
            }
        }

        /// <summary>
        /// Получить свойства канала управления по его номеру.
        /// </summary>
        public CtrlCnlProps GetCtrlCnlProps(int ctrlCnlNum)
        {
            try
            {
                dataCache.RefreshBaseTables();

                // необходимо сохранить ссылку, т.к. объект может быть пересоздан другим потоком
                CtrlCnlProps[] ctrlCnlProps = dataCache.CtrlCnlProps;

                // поиск свойств заданного канала
                int ind = Array.BinarySearch(ctrlCnlProps, ctrlCnlNum, CtrlCnlProps.IntComp);
                return ind >= 0 ? ctrlCnlProps[ind] : null;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств канала управления {0}" :
                    "Error getting output channel {0} properties", ctrlCnlNum);
                return null;
            }
        }

        /// <summary>
        /// Получить свойства статуса входного канала по значению статуса.
        /// </summary>
        public CnlStatProps GetCnlStatProps(int stat)
        {
            try
            {
                dataCache.RefreshBaseTables();
                CnlStatProps cnlStatProps;
                return dataCache.CnlStatProps.TryGetValue(stat, out cnlStatProps) ?
                    cnlStatProps : null;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении цвета по статусу {0}" :
                    "Error getting color by status {0}", stat);
                return null;
            }
        }

        /// <summary>
        /// Привязать свойства входных каналов и каналов управления к элементам представления.
        /// </summary>
        public void BindCnlProps(BaseView view)
        {
            try
            {
                dataCache.RefreshBaseTables();
                DateTime baseAge = dataCache.BaseTables.BaseAge;
                if (view != null && view.BaseAge != baseAge && baseAge > DateTime.MinValue)
                {
                    lock (view.SyncRoot)
                    {
                        view.BaseAge = baseAge;
                        view.BindCnlProps(dataCache.CnlProps);
                        view.BindCtrlCnlProps(dataCache.CtrlCnlProps);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при привязке свойств каналов к элементам представления" :
                    "Error binding channel properties to the view elements");
            }
        }

        /// <summary>
        /// Получить свойства объекта пользовательского интерфейса по идентификатору.
        /// </summary>
        public UiObjProps GetUiObjProps(int uiObjID)
        {
            try
            {
                dataCache.RefreshBaseTables();

                // необходимо сохранить ссылку, т.к. объект может быть пересоздан другим потоком
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.InterfaceTable, true);
                    DataView viewInterface = baseTables.InterfaceTable.DefaultView;
                    viewInterface.Sort = "ItfID";
                    int rowInd = viewInterface.Find(uiObjID);

                    // столбец TypeCode добавлен в таблицу Интерфейс, начиная с версии 5.8
                    return rowInd >= 0 ?
                        GetUiObjFromRow(viewInterface[rowInd], viewInterface.Table.Columns.Contains("TypeCode")) :
                        null;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств объекта пользовательского интерфейса по ид.={0}" :
                    "Error getting user interface object properties by ID={0}", uiObjID);
                return null;
            }
        }

        /// <summary>
        /// Получить список свойств объектов пользовательского интерфейса.
        /// </summary>
        public List<UiObjProps> GetUiObjPropsList(UiObjProps.BaseUiTypes baseUiTypes)
        {
            List<UiObjProps> list = new List<UiObjProps>();

            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.InterfaceTable, true);
                    DataView viewInterface = baseTables.InterfaceTable.DefaultView;
                    viewInterface.Sort = "ItfID";
                    bool v58plus = viewInterface.Table.Columns.Contains("TypeCode");

                    foreach (DataRowView rowView in viewInterface)
                    {
                        UiObjProps uiObjProps = GetUiObjFromRow(rowView, v58plus);

                        if (baseUiTypes.HasFlag(uiObjProps.BaseUiType))
                            list.Add(uiObjProps);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении списка свойств объектов пользовательского интерфейса" :
                    "Error getting list of user interface object properties");
            }

            return list;
        }

        /// <summary>
        /// Получить права на объекты пользовательского интерфейса по идентификатору роли.
        /// </summary>
        public Dictionary<int, EntityRights> GetUiObjRights(int roleID)
        {
            Dictionary<int, EntityRights> rightsDict = new Dictionary<int, EntityRights>();

            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    // consider role inheritance
                    List<int> roleIDList = GetParentRoles(roleID);

                    // retrieve rights
                    BaseTables.CheckColumnsExist(baseTables.RightTable, true);
                    DataView viewRight = baseTables.RightTable.DefaultView;
                    viewRight.Sort = "RoleID";

                    foreach (int includedRoleID in roleIDList)
                    {
                        foreach (DataRowView rowView in viewRight.FindRows(includedRoleID))
                        {
                            int uiObjID = (int)rowView["ItfID"];
                            EntityRights rights = new EntityRights((bool)rowView["ViewRight"], (bool)rowView["CtrlRight"]);
                            rightsDict[uiObjID] = rights;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении прав на объекты пользовательского интерфейса для роли с ид.={0}" :
                    "Error getting access rights on user interface objects for the role with ID={0}", roleID);
            }

            return rightsDict;
        }

        /// <summary>
        /// Gets the ID of the role and all its parents.
        /// </summary>
        public List<int> GetRoleHierarchy(int roleID)
        {
            try
            {
                dataCache.RefreshBaseTables();

                lock (dataCache.BaseTables.SyncRoot)
                {
                    return GetParentRoles(roleID);
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении иерархии ролей" :
                    "Error getting the role hierarchy");
                return new List<int> { roleID };
            }
        }

        /// <summary>
        /// Получить имя пользователя по идентификатору.
        /// </summary>
        public string GetUserName(int userID)
        {
            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.UserTable, true);
                    DataView viewUser = baseTables.UserTable.DefaultView;
                    viewUser.Sort = "UserID";
                    int rowInd = viewUser.Find(userID);
                    return rowInd >= 0 ? 
                        (string)viewUser[rowInd]["Name"] : 
                        "[" + userID + "]"; // deleted or external user
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении имени пользователя по ид.={0}" :
                    "Error getting user name by ID={0}", userID);
                return null;
            }
        }

        /// <summary>
        /// Получить свойства пользователя по идентификатору.
        /// </summary>
        public UserProps GetUserProps(int userID)
        {
            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.UserTable, true);
                    DataView viewUser = baseTables.UserTable.DefaultView;
                    viewUser.Sort = "UserID";
                    int rowInd = viewUser.Find(userID);

                    if (rowInd >= 0)
                    {
                        UserProps userProps = new UserProps(userID);
                        userProps.UserName = (string)viewUser[rowInd]["Name"];
                        userProps.RoleID = (int)viewUser[rowInd]["RoleID"];
                        userProps.RoleName = GetRoleName(userProps.RoleID);
                        return userProps;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении свойств пользователя по ид.={0}" :
                    "Error getting user properties by ID={0}", userID);
                return null;
            }
        }

        /// <summary>
        /// Получить идентификатор пользователя по имени.
        /// </summary>
        public int GetUserID(string username)
        {
            try
            {
                username = username ?? "";
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.UserTable, true);
                    DataView viewUser = baseTables.UserTable.DefaultView;
                    viewUser.Sort = "Name";
                    int rowInd = viewUser.Find(username);
                    return rowInd >= 0 ? (int)viewUser[rowInd]["UserID"] : BaseValues.EmptyDataID;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении идентификатора пользователя по имени \"{0}\"" :
                    "Error getting user ID by name \"{0}\"", username);
                return BaseValues.EmptyDataID;
            }
        }

        /// <summary>
        /// Получить наименование объекта по номеру.
        /// </summary>
        public string GetObjName(int objNum)
        {
            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.ObjTable, true);
                    DataView viewObj = baseTables.ObjTable.DefaultView;
                    viewObj.Sort = "ObjNum";
                    int rowInd = viewObj.Find(objNum);
                    return rowInd >= 0 ? (string)viewObj[rowInd]["Name"] : "";
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении наименования объекта по номеру {0}" :
                    "Error getting object name by number {0}", objNum);
                return "";
            }
        }

        /// <summary>
        /// Получить наименование КП по номеру.
        /// </summary>
        public string GetKPName(int kpNum)
        {
            try
            {
                dataCache.RefreshBaseTables();
                BaseTables baseTables = dataCache.BaseTables;

                lock (baseTables.SyncRoot)
                {
                    BaseTables.CheckColumnsExist(baseTables.ObjTable, true);
                    DataView viewObj = baseTables.KPTable.DefaultView;
                    viewObj.Sort = "KPNum";
                    int rowInd = viewObj.Find(kpNum);
                    return rowInd >= 0 ? (string)viewObj[rowInd]["Name"] : "";
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении наименования КП по номеру {0}" :
                    "Error getting device name by number {0}", kpNum);
                return "";
            }
        }

        /// <summary>
        /// Получить наименование роли по идентификатору.
        /// </summary>
        public string GetRoleName(int roleID)
        {
            string roleName = BaseValues.Roles.GetRoleName(roleID); // стандартное имя роли
            return BaseValues.Roles.Custom <= roleID && roleID < BaseValues.Roles.Err ?
                GetRoleNameFromBase(roleID, roleName) :
                roleName;
        }


        /// <summary>
        /// Получить текущие данные входного канала.
        /// </summary>
        public SrezTableLight.CnlData GetCurCnlData(int cnlNum)
        {
            DateTime dataAge;
            return GetCurCnlData(cnlNum, out dataAge);
        }

        /// <summary>
        /// Получить текущие данные входного канала.
        /// </summary>
        public SrezTableLight.CnlData GetCurCnlData(int cnlNum, out DateTime dataAge)
        {
            try
            {
                SrezTableLight.Srez snapshot = dataCache.GetCurSnapshot(out dataAge);
                SrezTableLight.CnlData cnlData;
                return snapshot != null && snapshot.GetCnlData(cnlNum, out cnlData) ?
                    cnlData : SrezTableLight.CnlData.Empty;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении текущих данных входного канала {0}" :
                    "Error getting current data of the input channel {0}", cnlNum);

                dataAge = DateTime.MinValue;
                return SrezTableLight.CnlData.Empty;
            }
        }

        /// <summary>
        /// Получить отображаемое событие на основе данных события.
        /// </summary>
        /// <remarks>Метод всегда возвращает объект, не равный null.</remarks>
        public DispEvent GetDispEvent(EventTableLight.Event ev, DataFormatter dataFormatter)
        {
            DispEvent dispEvent = new DispEvent();

            try
            {
                dispEvent.Num = ev.Number;
                dispEvent.Time = ev.DateTime.ToLocalizedString();
                dispEvent.Ack = ev.Checked ? CommonPhrases.EventAck : CommonPhrases.EventNotAck;

                InCnlProps cnlProps = GetCnlProps(ev.CnlNum);
                CnlStatProps cnlStatProps = GetCnlStatProps(ev.NewCnlStat);

                if (cnlProps == null)
                {
                    dispEvent.Obj = GetObjName(ev.ObjNum);
                    dispEvent.KP = GetKPName(ev.KPNum);
                }
                else
                {
                    dispEvent.Obj = cnlProps.ObjName;
                    dispEvent.KP = cnlProps.KPName;
                    dispEvent.Cnl = cnlProps.CnlName;
                    dispEvent.Color = dataFormatter.GetCnlValColor(
                        ev.NewCnlVal, ev.NewCnlStat, cnlProps, cnlStatProps);
                    dispEvent.Sound = cnlProps.EvSound;
                }

                dispEvent.Text = dataFormatter.GetEventText(ev, cnlProps, cnlStatProps);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении отображаемого события на основе данных события" :
                    "Error getting displayed event based on the event data");
            }

            return dispEvent;
        }
    }
}
