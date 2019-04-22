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
 * Module   : Administrator
 * Summary  : State of application controls
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// State of application controls.
    /// <para>Состояние элементов управления в приложении.</para>
    /// </summary>
    public class AppState
    {
        /// <summary>
        /// The default state file name.
        /// </summary>
        public const string DefFileName = "ScadaAdminState.xml";
        /// <summary>
        /// The number of recent projects.
        /// </summary>
        public const int RecentProjectCount = 10;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppState()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the state of the main form.
        /// </summary>
        public MainFormState MainFormState { get; private set; }

        /// <summary>
        /// Gets or sets the directory of projects.
        /// </summary>
        public string ProjectDir { get; set; }

        /// <summary>
        /// Gets the recently opened projects.
        /// </summary>
        public List<string> RecentProjects { get; private set; }

        /// <summary>
        /// Gets the recently selected objects.
        /// </summary>
        public RecentSelection RecentSelection { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            MainFormState = new MainFormState();
            ProjectDir = ScadaUtils.IsRunningOnWin ? @"C:\SCADA\Projects\" : "/opt/scada/Projects/";
            RecentProjects = new List<string>();
            RecentSelection = new RecentSelection();
        }

        /// <summary>
        /// Loads the state from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (File.Exists(fileName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);
                    XmlElement rootElem = xmlDoc.DocumentElement;

                    if (rootElem.SelectSingleNode("MainFormState") is XmlNode mainFormStateNode)
                        MainFormState.LoadFromXml(mainFormStateNode);

                    ProjectDir = rootElem.GetChildAsString("ProjectDir");

                    if (rootElem.SelectSingleNode("RecentProjects") is XmlNode recentProjectsNode)
                    {
                        XmlNodeList pathNodeList = recentProjectsNode.SelectNodes("Path");
                        foreach (XmlNode pathNode in pathNodeList)
                        {
                            RecentProjects.Add(pathNode.InnerText);
                        }
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.LoadAppStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves the state to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaAdminState");
                xmlDoc.AppendChild(rootElem);

                MainFormState.SaveToXml(rootElem.AppendElem("MainFormState"));
                rootElem.AppendElem("ProjectDir", ProjectDir);

                XmlElement recentProjectsElem = rootElem.AppendElem("RecentProjects");
                foreach (string path in RecentProjects)
                {
                    recentProjectsElem.AppendElem("Path", path);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.SaveAppStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Adds the recent project to the list.
        /// </summary>
        public void AddRecentProject(string path)
        {
            RecentProjects.Remove(path);
            RecentProjects.Add(path);

            while (RecentProjects.Count > RecentProjectCount)
            {
                RecentProjects.RemoveAt(0);
            }
        }

        /// <summary>
        /// Removes the recent project from the list.
        /// </summary>
        public void RemoveRecentProject(string path)
        {
            RecentProjects.Remove(path);
        }
    }
}
