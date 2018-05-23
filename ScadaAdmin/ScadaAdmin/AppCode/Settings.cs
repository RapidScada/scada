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
 * Module   : SCADA-Administrator
 * Summary  : Application settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2010
 * Modified : 2018
 */

using System;
using System.IO;
using System.Xml;
using Scada;
using Utils;

namespace ScadaAdmin
{
    /// <summary>
    /// Application settings
    /// <para>Настройки приложения</para>
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Настройки приложения
        /// </summary>
        public class AppSettings
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public AppSettings()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить или установить файл базы конфигурации SCADA-Администратора
            /// </summary>
            public string BaseSDFFile { get; set; }
            /// <summary>
            /// Получить или установить директорию базы конфигурации SCADA-Сервера
            /// </summary>
            public string BaseDATDir { get; set; }
            /// <summary>
            /// Получить или установить директорию резервного копирования базы конфигурации
            /// </summary>
            public string BackupDir { get; set; }
            /// <summary>
            /// Получить или установить директорию SCADA-Коммуникатора
            /// </summary>
            public string CommDir { get; set; }
            /// <summary>
            /// Получить или установить признак автоматического резервирования базы конфигурации при передаче серверу
            /// </summary>
            public bool AutoBackupBase { get; set; }

            /// <summary>
            /// Установить настройки приложения по умолчанию
            /// </summary>
            public void SetToDefault()
            {
                BaseSDFFile = @"C:\SCADA\BaseSDF\ScadaBase.sdf";
                BaseDATDir = @"C:\SCADA\BaseDAT\";
                BackupDir = @"C:\SCADA\ScadaAdmin\Backup\";
                CommDir = @"C:\SCADA\ScadaComm\";
                AutoBackupBase = true;
            }
        }

        /// <summary>
        /// Состояние главной формы
        /// </summary>
        public class FormState
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public FormState()
            {
                SetToDefault();
            }

            /// <summary>
            /// Получить признак, что состояние формы не определено
            /// </summary>
            public bool IsEmpty { get; set; }
            /// <summary>
            /// Получить или установить позицию формы по горизонтали
            /// </summary>
            public int Left { get; set; }
            /// <summary>
            /// Получить или установить позицию формы по вертикали
            /// </summary>
            public int Top { get; set; }
            /// <summary>
            /// Получить или установить ширину формы
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// Получить или установить высоту формы
            /// </summary>
            public int Height { get; set; }
            /// <summary>
            /// Получить или установить признак, что форма развёрнута
            /// </summary>
            public bool Maximized { get; set; }
            /// <summary>
            /// Получить или установить ширину дерева проводника
            /// </summary>
            public int ExplorerWidth { get; set; }
            /// <summary>
            /// Получить или установить наименование соединения с удалённым сервером
            /// </summary>
            public string ServerConn { get; set; }

            /// <summary>
            /// Установить состояние главной формы по умолчанию
            /// </summary>
            public void SetToDefault()
            {
                IsEmpty = true;
                Left = 0;
                Top = 0;
                Width = 0;
                Height = 0;
                Maximized = false;
                ExplorerWidth = 0;
                ServerConn = "";
            }
        }


        /// <summary>
        /// Имя файла настроек приложения
        /// </summary>
        private const string AppSettingsFileName = "ScadaAdminConfig.xml";
        /// <summary>
        /// Имя файла состояния главной формы
        /// </summary>
        private const string FormStateFileName = "ScadaAdminState.xml";


        /// <summary>
        /// Конструктор
        /// </summary>
        public Settings()
        {
            AppSett = new AppSettings();
            FormSt = new FormState();
        }


        /// <summary>
        /// Получить параметры приложения
        /// </summary>
        public AppSettings AppSett { get; private set; }

        /// <summary>
        /// Получить состояние главной формы
        /// </summary>
        public FormState FormSt { get; private set; }


        /// <summary>
        /// Загрузить настройки приложения из файла
        /// </summary>
        public bool LoadAppSettings(out string errMsg)
        {
            // установка параметров по умолчанию
            AppSett.SetToDefault();

            // загрузка из файла
            string fileName = AppData.AppDirs.ConfigDir + AppSettingsFileName;

            try
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                // получение значений параметров
                XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("Param");
                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    string name = xmlElement.GetAttribute("name");
                    string nameL = name.ToLowerInvariant();
                    string val = xmlElement.GetAttribute("value");

                    try
                    {
                        if (nameL == "basesdffile")
                            AppSett.BaseSDFFile = val;
                        else if (nameL == "basedatdir")
                            AppSett.BaseDATDir = ScadaUtils.NormalDir(val);
                        else if (nameL == "backupdir")
                            AppSett.BackupDir = ScadaUtils.NormalDir(val);
                        else if (nameL == "commdir")
                            AppSett.CommDir = ScadaUtils.NormalDir(val);
                        else if (nameL == "backuponpassbase")
                            AppSett.AutoBackupBase = bool.Parse(val);
                    }
                    catch
                    {
                        throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.LoadAppSettingsError + ":\r\n" + ex.Message;
                AppData.ErrLog.WriteAction(errMsg, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Сохранить настройки приложения в файле
        /// </summary>
        public bool SaveAppSettings(out string errMsg)
        {
            try
            {
                // формирование XML-документа
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminConfig");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendParamElem("BaseSDFFile", AppSett.BaseSDFFile, 
                    "Файл базы конфигурации в формате SDF", "Configuration database file in SDF format");
                rootElem.AppendParamElem("BaseDATDir", AppSett.BaseDATDir,
                    "Директория базы конфигурации в формате DAT", "Configuration database in DAT format directory");
                rootElem.AppendParamElem("BackupDir", AppSett.BackupDir,
                    "Директория резервного копирования базы конфигурации", "Configuration database backup directory");
                rootElem.AppendParamElem("CommDir", AppSett.CommDir,
                    "Директория SCADA-Коммуникатора", "SCADA-Communicator directory");
                rootElem.AppendParamElem("AutoBackupBase", AppSett.AutoBackupBase,
                    "Автоматически резервировать базу конфигурации", "Automatically backup the configuration database");

                // сохранение в файле
                xmlDoc.Save(AppData.AppDirs.ConfigDir + AppSettingsFileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = CommonPhrases.SaveAppSettingsError + ":\r\n" + ex.Message;
                AppData.ErrLog.WriteAction(errMsg, Log.ActTypes.Exception);
                return false;
            }
        }

        /// <summary>
        /// Загрузить состояние главной формы из файла
        /// </summary>
        public void LoadFormState()
        {
            // установка состояния по умолчанию
            FormSt.SetToDefault();

            // загрузка из файла
            string fileName = AppData.AppDirs.ConfigDir + FormStateFileName;

            if (File.Exists(fileName))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);

                    // получение значений параметров
                    XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("Param");
                    foreach (XmlElement xmlElement in xmlNodeList)
                    {
                        string name = xmlElement.GetAttribute("name");
                        string nameL = name.ToLowerInvariant();
                        string val = xmlElement.GetAttribute("value");

                        try
                        {
                            if (nameL == "left")
                                FormSt.Left = int.Parse(val);
                            else if (nameL == "top")
                                FormSt.Top = int.Parse(val);
                            else if (nameL == "width")
                                FormSt.Width = int.Parse(val);
                            else if (nameL == "height")
                                FormSt.Height = int.Parse(val);
                            else if (nameL == "maximized")
                                FormSt.Maximized = bool.Parse(val);
                            else if (nameL == "explorerwidth")
                                FormSt.ExplorerWidth = int.Parse(val);
                            else if (nameL == "serverconn")
                                FormSt.ServerConn = val;
                        }
                        catch
                        {
                            throw new Exception(string.Format(CommonPhrases.IncorrectXmlParamVal, name));
                        }
                    }

                    FormSt.IsEmpty = false;
                }
                catch (Exception ex)
                {
                    FormSt.IsEmpty = true;
                    AppData.ErrLog.WriteAction((Localization.UseRussian ? 
                        "Ошибка при загрузке состояния главной формы:\r\n" : 
                        "Error loading main form state:\r\n") + ex.Message, Log.ActTypes.Exception);
                }
            }
        }

        /// <summary>
        /// Сохранить состояние главной формы в файле
        /// </summary>
        public bool SaveFormState(out string errMsg)
        {
            try
            {
                // формирование XML-документа
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminState");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendParamElem("Left", FormSt.Left);
                rootElem.AppendParamElem("Top", FormSt.Top);
                rootElem.AppendParamElem("Width", FormSt.Width);
                rootElem.AppendParamElem("Height", FormSt.Height);
                rootElem.AppendParamElem("Maximized", FormSt.Maximized);
                rootElem.AppendParamElem("ExplorerWidth", FormSt.ExplorerWidth);
                rootElem.AppendParamElem("ServerConn", FormSt.ServerConn);

                // сохранение в файле
                xmlDoc.Save(AppData.AppDirs.ConfigDir + FormStateFileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = (Localization.UseRussian ? "Ошибка при сохранении файла состояния главной формы:\r\n" :
                    "Error saving main form state:\r\n") + ex.Message;
                AppData.ErrLog.WriteAction(errMsg, Log.ActTypes.Exception);
                return false;
            }
        }
    }
}
