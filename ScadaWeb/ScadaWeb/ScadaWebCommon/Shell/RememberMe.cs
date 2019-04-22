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
 * Module   : ScadaWebCommon
 * Summary  : Allows to remember that a user is logged on
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using Utils;

namespace Scada.Web.Shell
{
    /// <summary>
    /// Allows to remember that a user is logged on
    /// <para>Позволяет запоминать, что пользователь вошёл в систему</para>
    /// </summary>
    /// <remarks>The object must be a singleton
    /// <para>Объект должен являться синглтоном</para></remarks>
    public class RememberMe
    {
        /// <summary>
        /// Учётные данные
        /// </summary>
        public class Credentials
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Credentials()
                : this(GenerateID(), GenerateID(), DateTime.Now)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Credentials(string browserID)
                : this(browserID, GenerateID(), DateTime.Now)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Credentials(string browserID, string oneTimePassword, DateTime createDT)
            {
                BrowserID = browserID ?? "";
                OneTimePassword = oneTimePassword ?? "";
                CreateDT = createDT;
            }

            /// <summary>
            /// Получить идентификатор, присваиваемый браузеру пользователя
            /// </summary>
            public string BrowserID { get; private set; }
            /// <summary>
            /// Одноразовый пароль
            /// </summary>
            public string OneTimePassword { get; private set; }
            /// <summary>
            /// Получить дату и время создания учётных данных
            /// </summary>
            public DateTime CreateDT { get; private set; }

            /// <summary>
            /// Генерировать идентификатор случайным образом
            /// </summary>
            public static string GenerateID()
            {
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Длительность хранения информации о входе пользователя в систему
        /// </summary>
        protected static readonly TimeSpan ExpireSpan = WebUtils.CookieExpiration;

        /// <summary>
        /// Объект для работы с хранилищем приложения
        /// </summary>
        protected readonly Storage storage;
        /// <summary>
        /// Журнал
        /// </summary>
        protected readonly Log log;
        /// <summary>
        /// Объект для синхронизации достапа к файлам
        /// </summary>
        protected readonly object fileLock;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected RememberMe()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RememberMe(Storage storage, Log log)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");
            if (log == null)
                throw new ArgumentNullException("log");

            this.storage = storage;
            this.log = log;
            fileLock = new object();
        }


        /// <summary>
        /// Получить имя файла учётных данных пользователя
        /// </summary>
        protected string GetCredentialsFileName(string username, bool forceDir = false)
        {
            string userAppDir = storage.GetUserAppDir(username);
            if (forceDir)
                Storage.ForceDir(userAppDir);
            return userAppDir + "RememberMe.xml";
        }

        /// <summary>
        /// Загрузить учётные данные пользователя из файла
        /// </summary>
        protected List<Credentials> LoadCredentials(string username)
        {
            List<Credentials> credList = new List<Credentials>();
            string fileName = GetCredentialsFileName(username);

            if (File.Exists(fileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                XmlNode usernameNode = rootElem.SelectSingleNode("Username");
                string loadedUsername = usernameNode == null ? "" : usernameNode.InnerText;

                if (string.Equals(username, loadedUsername, StringComparison.OrdinalIgnoreCase))
                {
                    XmlNodeList credNodes = rootElem.SelectNodes("Credentials");
                    if (credNodes != null)
                    {
                        foreach (XmlElement credElem in credNodes)
                        {
                            credList.Add(new Credentials(
                                credElem.GetChildAsString("BrowserID"),
                                credElem.GetChildAsString("OneTimePassword"),
                                credElem.GetChildAsDateTime("CreateDT")));
                        }
                    }
                }
            }

            return credList;
        }

        /// <summary>
        /// Сохранить учётные данные пользователя в файле
        /// </summary>
        protected void SaveCredentials(string username, List<Credentials> credList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("RememberMe");
            xmlDoc.AppendChild(rootElem);
            rootElem.AppendElem("Username", username);

            foreach (Credentials cred in credList)
            {
                XmlElement credElem = rootElem.AppendElem("Credentials");
                credElem.AppendElem("BrowserID", cred.BrowserID);
                credElem.AppendElem("OneTimePassword", cred.OneTimePassword);
                credElem.AppendElem("CreateDT", cred.CreateDT);
            }

            xmlDoc.Save(GetCredentialsFileName(username, true));
        }

        /// <summary>
        /// Проверить, что пользователю разрешён вход в систему
        /// </summary>
        protected bool ValidateUser(string username, Credentials cred, out Credentials newCred, out string alert)
        {
            lock (fileLock)
            {
                bool validated = false;
                newCred = null;
                alert = "";

                // загрузка учётных данных
                List<Credentials> credList = LoadCredentials(username);
                bool credListModified = false;
                bool credFound = false;
                DateTime nowDT = DateTime.Now;
                string browserID = cred.BrowserID;
                int i = 0;

                while (i < credList.Count)
                {
                    Credentials loadedCred = credList[i];

                    if (nowDT - loadedCred.CreateDT > ExpireSpan)
                    {
                        // удаление устаревших учётных данных
                        credList.RemoveAt(i);
                        credListModified = true;
                        i--;
                    }
                    else if (!credFound && string.Equals(loadedCred.BrowserID, browserID, StringComparison.Ordinal))
                    {
                        credFound = true;

                        if (string.Equals(loadedCred.OneTimePassword, cred.OneTimePassword, StringComparison.Ordinal))
                        {
                            // установка признака, что проверка пройдена успешно
                            validated = true;
                        }
                        else
                        {
                            alert = WebPhrases.SecurityViolation;
                            log.WriteError(string.Format(Localization.UseRussian ?
                                "Попытка использования устаревшего одноразового пароля! " + 
                                "Возможна утечка данных пользователя {0}" :
                                "Attempting to use the obsolete one-time password! " + 
                                "Possible data leak of the user {0}",
                                username));
                        }

                        // удаление использованных учётных данных
                        credList.RemoveAt(i);
                        credListModified = true;
                        i--;
                    }

                    i++;
                }

                // создание новых учётных данных в случае успешной проверки
                if (validated)
                {
                    newCred = new Credentials(browserID);
                    credList.Add(newCred);
                    credListModified = true;
                }

                // сохранение изменившихся учётных данных
                if (credListModified)
                    SaveCredentials(username, credList);

                return validated;
            }
        }

        /// <summary>
        /// Создать учётные данные для пользователя на стороне сервера
        /// </summary>
        protected Credentials CreateCredentials(string username)
        {
            lock (fileLock)
            {
                // загрузка учётных данных
                List<Credentials> credList = LoadCredentials(username);
                // создание и добавление новых учётных данных
                Credentials cred = new Credentials();
                credList.Add(cred);
                // сохранение учётных данных
                SaveCredentials(username, credList);
                return cred;
            }
        }

        /// <summary>
        /// Создать cookie для записи информации о входе в систему
        /// </summary>
        protected HttpCookie CreateCookie(string username, Credentials cred)
        {
            HttpCookie cookie = new HttpCookie("User");
            cookie.Values.Set("Username", HttpUtility.UrlEncode(username));
            cookie.Values.Set("BrowserID", cred == null ? "" : cred.BrowserID);
            cookie.Values.Set("OneTimePassword", cred == null ? "" : cred.OneTimePassword);
            cookie.Expires = DateTime.Now.Add(ExpireSpan);
            return cookie;
        }


        /// <summary>
        /// Проверить, что пользователю, данные которого записаны в cookies, разрешён вход в систему
        /// </summary>
        public bool ValidateUser(HttpContext httpContext, out string username, out string alert)
        {
            username = "";
            alert = "";

            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                HttpCookie reqCookie = httpContext.Request.Cookies["User"];

                if (reqCookie != null && reqCookie.HasKeys)
                {
                    username = HttpUtility.UrlDecode(reqCookie.Values["Username"]);
                    Credentials cred = new Credentials(
                        reqCookie.Values["BrowserID"], reqCookie.Values["OneTimePassword"], DateTime.MinValue);
                    Credentials newCred;

                    if (username != "" && ValidateUser(username, cred, out newCred, out alert))
                    {
                        HttpCookie respCookie = CreateCookie(username, newCred);
                        httpContext.Response.Cookies.Set(respCookie);
                        return true;
                    }
                    else
                    {
                        HttpCookie respCookie = CreateCookie(username, null);
                        httpContext.Response.Cookies.Set(respCookie);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при проверке входа пользователя {0}" :
                    "Error validating login of the user {0}", username);
                return false;
            }
        }

        /// <summary>
        /// Извлечь имя пользователя из cookies
        /// </summary>
        public string RestoreUsername(HttpContext httpContext)
        {
            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                HttpCookie cookie = httpContext.Request.Cookies["User"];
                return cookie != null && cookie.HasKeys ? 
                    HttpUtility.UrlDecode(cookie.Values["Username"]) : "";
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при извлечении имени пользователя из cookies" :
                    "Error restoring username from the cookies");
                return "";
            }
        }

        /// <summary>
        /// Запомнить имя пользователя в cookies
        /// </summary>
        public void RememberUsername(string username, HttpContext httpContext)
        {
            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                HttpCookie cookie = CreateCookie(username, null);
                httpContext.Response.Cookies.Set(cookie);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при сохранении имени пользователя в cookies" :
                    "Error saving username to the cookies");
            }
        }

        /// <summary>
        /// Запомнить, что пользователь вошёл в систему
        /// </summary>
        public void RememberUser(string username, HttpContext httpContext)
        {
            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                Credentials cred = CreateCredentials(username);
                HttpCookie cookie = CreateCookie(username, cred);
                httpContext.Response.Cookies.Set(cookie);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при сохранении информации о входе пользователя {0}" :
                    "Error saving login information of the user {0}", username);
            }
        }

        /// <summary>
        /// Удалить информацию о входе в систему из cookies
        /// </summary>
        public void ForgetUser(HttpContext httpContext)
        {
            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                HttpCookie reqCookie = httpContext.Request.Cookies["User"];

                if (reqCookie != null && reqCookie.HasKeys)
                {
                    string username = HttpUtility.UrlDecode(reqCookie.Values["Username"]);
                    HttpCookie respCookie = CreateCookie(username, null);
                    httpContext.Response.Cookies.Set(respCookie);
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при удалении информации о входе пользователя из cookies" :
                    "Error deleting login information from the cookies");
            }
        }

        /// <summary>
        /// Полностью удалить информацию о входах пользователя на стороне сервера и из cookies
        /// </summary>
        public void CompletelyForgetUser(string username, HttpContext httpContext)
        {
            try
            {
                WebUtils.CheckHttpContext(httpContext, true);
                HttpCookie respCookie = CreateCookie("", null);
                httpContext.Response.Cookies.Set(respCookie);

                lock (fileLock)
                {
                    File.Delete(GetCredentialsFileName(username));
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при полном удалении информации о входах пользователя {0}" :
                    "Error completely deleting login information of the user {0}", username);
            }
        }
    }
}
