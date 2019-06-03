/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : ModActiveDirectory
 * Summary  : Server module logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Data.Configuration;
using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Utils;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Server module logic.
    /// <para>Логика работы серверного модуля.</para>
    /// </summary>
    public class ModActiveDirectoryLogic : ModLogic
    {
        private Dictionary<string, User> users; // the users accessed by username

        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "ModActiveDirectory";
            }
        }


        /// <summary>
        /// Finds the security groups in Active Directory that the specified group belongs to and adds them to the list.
        /// </summary>
        private static void FindOwnerGroups(DirectoryEntry entry, string group, List<string> groups)
        {
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(distinguishedName=" + group + ")";
            search.PropertiesToLoad.Add("memberOf");
            SearchResult searchRes = search.FindOne();

            if (searchRes != null)
            {
                foreach (object result in searchRes.Properties["memberOf"])
                {
                    string gr = result.ToString();
                    groups.Add(gr);
                    FindOwnerGroups(entry, gr, groups);
                }
            }
        }

        /// <summary>
        /// Checks if the list of security groups contains the specified user role.
        /// </summary>
        private static bool GroupsContain(List<string> groups, string roleName)
        {
            roleName = "CN=" + roleName;
            foreach (string group in groups)
                if (group.StartsWith(roleName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }


        /// <summary>
        /// Loads the users dictionary.
        /// </summary>
        private void LoadUsers()
        {
            users = new Dictionary<string, User>();
            BaseTable<User> tblUser = new BaseTable<User>("User", "UserID", CommonPhrases.UserTable);

            try
            {
                BaseAdapter adapter = new BaseAdapter()
                {
                    Directory = Settings.BaseDATDir,
                    TableName = "user.dat"
                };

                adapter.Fill(tblUser, false);

                foreach (User user in tblUser.EnumerateItems())
                {
                    users[user.Name.Trim().ToLowerInvariant()] = user;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(Localization.UseRussian ?
                    "{0}. Ошибка при загрузке пользователей: {1}" :
                    "{0}. Error loading users: {1}", Name, ex.ToString()),
                    Log.ActTypes.Exception);
            }
        }

        /// <summary>
        /// Performs actions when starting Server.
        /// </summary>
        public override void OnServerStart()
        {
            LoadUsers();
        }

        /// <summary>
        /// Validates user name and password returning the user role.
        /// </summary>
        public override bool ValidateUser(string username, string password, out int roleID, out bool handled)
        {
            if (Settings.UseAD)
            {
                DirectoryEntry entry = null;

                try
                {
                    // check password
                    bool pwdOK = false;

                    if (string.IsNullOrEmpty(password))
                    {
                        entry = new DirectoryEntry(Settings.LdapPath);
                        pwdOK = true;
                    }
                    else
                    {
                        entry = new DirectoryEntry(Settings.LdapPath, username, password);

                        // user authentication
                        try
                        {
                            object native = entry.NativeObject;
                            pwdOK = true;
                        }
                        catch { }
                    }

                    if (pwdOK)
                    {
                        if (users.TryGetValue(username.Trim().ToLowerInvariant(), out User user))
                        {
                            roleID = user.RoleID;
                            handled = true;
                            return true;
                        }
                        else
                        {
                            // get user security groups
                            DirectorySearcher search = new DirectorySearcher(entry);
                            search.Filter = "(sAMAccountName=" + username + ")";
                            search.PropertiesToLoad.Add("memberOf");
                            SearchResult searchRes = search.FindOne();

                            if (searchRes != null)
                            {
                                List<string> groups = new List<string>();
                                foreach (object result in searchRes.Properties["memberOf"])
                                {
                                    string group = result.ToString();
                                    groups.Add(group);
                                    FindOwnerGroups(entry, group, groups);
                                }

                                // define user role
                                if (GroupsContain(groups, "ScadaDisabled"))
                                    roleID = BaseValues.Roles.Disabled;
                                else if (GroupsContain(groups, "ScadaGuest"))
                                    roleID = BaseValues.Roles.Guest;
                                else if (GroupsContain(groups, "ScadaDispatcher"))
                                    roleID = BaseValues.Roles.Dispatcher;
                                else if (GroupsContain(groups, "ScadaAdmin"))
                                    roleID = BaseValues.Roles.Admin;
                                else if (GroupsContain(groups, "ScadaApp"))
                                    roleID = BaseValues.Roles.App;
                                else
                                    roleID = BaseValues.Roles.Err;

                                // return successful result
                                if (roleID != BaseValues.Roles.Err)
                                {
                                    handled = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format(Localization.UseRussian ?
                        "{0}. Ошибка при работе с Active Directory: {1}" :
                        "{0}. Error working with Active Directory: {1}", Name, ex.Message), 
                        Log.ActTypes.Exception);
                }
                finally
                {
                    entry?.Close();
                }
            }

            return base.ValidateUser(username, password, out roleID, out handled);
        }
    }
}
