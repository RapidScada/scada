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
 * Module   : SCADA-Scheme Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Стартовая веб-страница редактора
        /// </summary>
        private const string StartPage = "editor.html";

        private AppData appData; // общие данные приложения
        private Mutex mutex;     // объект для проверки запуска второй копии приложения


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            appData = AppData.GetAppData();
            mutex = null;

            Application.ThreadException += Application_ThreadException;
        }

        /// <summary>
        /// Локализовать приложение
        /// </summary>
        private void Localize()
        {
            string errMsg;

            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaData", out errMsg))
                CommonPhrases.Init();
            else
                appData.Log.WriteError(errMsg);

            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaSchemeEditor", out errMsg))
            {
                Translator.TranslateForm(this, "Scada.Scheme.Editor.FrmMain");
                AppPhrases.Init();
            }
            else
            {
                appData.Log.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Проверить, что запущена вторая копия приложения
        /// </summary>
        private bool SecondInstanceExists()
        {
            try
            {
                bool createdNew;
                mutex = new Mutex(true, "ScadaSchemeEditorMutex", out createdNew);
                return !createdNew;
            }
            catch (Exception ex)
            {
                appData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при проверке существования второй копии приложения" :
                    "Error checking existence of a second copy of the application");
                return false;
            }
        }

        /// <summary>
        /// Открыть браузер со страницей редактора
        /// </summary>
        private void OpenBrowser()
        {
            Uri startUri = new Uri(appData.AppDirs.WebDir + StartPage);
            //Process.Start("firefox", startUri.AbsoluteUri);
            Process.Start(startUri.AbsoluteUri);
        }


        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string errMsg = CommonPhrases.UnhandledException + ":\r\n" + e.Exception.Message;
            appData.Log.WriteAction(errMsg, Log.ActTypes.Exception);
            ScadaUiUtils.ShowError(errMsg);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // инициализация общих данных приложения
            appData.Init(Path.GetDirectoryName(Application.ExecutablePath));

            // локализация приложения
            Localize();

            // проверка существования второй копии приложения
            if (SecondInstanceExists())
            {
                ScadaUiUtils.ShowInfo(AppPhrases.CloseSecondInstance);
                Close();
                appData.Log.WriteAction(Localization.UseRussian ?
                    "Вторая копия Редактора схем закрыта." :
                    "The second instance of Scheme Editor has been closed.");
                return;
            }

            // запуск механизма редактора схем
            if (appData.StartEditor())
            {
                // открытие браузера со страницей редактора
                OpenBrowser();
            }
            else
            {
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.FailedToStartEditor, appData.Log.FileName));
                Close();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // завершить работу приложения
            appData.FinalizeApp();
        }

        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            // активировать форму при наведении мыши
            if (ActiveForm != this)
                BringToFront();
        }


        private void btnFileNew_Click(object sender, EventArgs e)
        {
            // создание новой схемы
            appData.Editor.NewScheme();
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {

        }

        private void btnFileSave_ButtonClick(object sender, EventArgs e)
        {

        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void btnFileOpenBrowser_Click(object sender, EventArgs e)
        {
            OpenBrowser();
        }

        private void btnEditCut_Click(object sender, EventArgs e)
        {

        }

        private void btnEditCopy_Click(object sender, EventArgs e)
        {

        }

        private void btnEditPaste_Click(object sender, EventArgs e)
        {

        }

        private void btnEditUndo_Click(object sender, EventArgs e)
        {

        }

        private void btnEditRedo_Click(object sender, EventArgs e)
        {

        }

        private void btnSchemePointer_Click(object sender, EventArgs e)
        {

        }

        private void btnSchemeDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnHelpAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            FrmAbout.ShowAbout(appData.AppDirs.ExeDir, appData.Log, this);
        }
    }
}
