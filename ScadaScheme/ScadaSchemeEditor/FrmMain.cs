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
 * Module   : SCADA-Scheme Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
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
        /// Имя файла схемы по умолчанию
        /// </summary>
        private const string DefFileName = "NewScheme.sch";

        private Mutex mutex;                      // объект для проверки запуска второй копии программы
        private string schemeUrl;                 // ссылка на веб-страницу, обеспечивающущю отображение схем
        private EditorData editorData;            // данные редактора схем
        private Log log;                          // журнал приложения
        private SchemeView.Element elemClipboard; // буфер обмена элементов схемы
        private ServiceHost schemeSvcHost;        // хост WCF-службы для взаимодействия частей приложения
        private ServiceHost domainSvcHost;        // хост WCF-службы для обеспечения кросс-доменного доступа
        private Thread schemeExThread;            // поток для обмена данными со схемой


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Запустить WCF-службы
        /// </summary>
        private bool StartWCF()
        {
            try
            {
                ScadaSchemeSvc schemeSvc = new ScadaSchemeSvc();
                schemeSvcHost = new ServiceHost(schemeSvc);
                ServiceBehaviorAttribute behavior = 
                    schemeSvcHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behavior.InstanceContextMode = InstanceContextMode.Single;
                behavior.UseSynchronizationContext = false;
                schemeSvcHost.Open();

                domainSvcHost = new ServiceHost(typeof(CrossDomainSvc));
                domainSvcHost.Open();

                return true;
            }
            catch (Exception ex)
            {
                log.WriteAction("Ошибка при запуске WCF-служб: " + ex.Message, Log.ActTypes.Exception);
                ScadaUtils.ShowError(
                    "Ошибка при запуске служб обмена данными.\nНормальная работа программы невозможна.");
                return false;
            }
        }

        /// <summary>
        /// Сохранить схему
        /// </summary>
        private bool SaveScheme(bool saveAs)
        {
            bool result = false;

            if (editorData.FileName == "")
            {
                saveFileDialog.FileName = DefFileName;
                saveAs = true;
            }
            else
            {
                saveFileDialog.FileName = editorData.FileName;
            }

            if (!saveAs || saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveAs)
                    editorData.FileName = saveFileDialog.FileName;
                string errMsg;
                if (editorData.SaveSchemeToFile(out errMsg))
                {
                    result = true;
                }
                else
                {
                    log.WriteAction(errMsg, Log.ActTypes.Exception);
                    ScadaUtils.ShowError(errMsg);
                }
                SetFormTitle();
            }

            return result;
        }

        /// <summary>
        /// Проверить возможность закрыть схему
        /// </summary>
        private bool CanCloseScheme()
        {
            if (editorData.Modified)
            {
                switch (MessageBox.Show(SchemePhrases.SaveConfirm, CommonPhrases.QuestionCaption, 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        return SaveScheme(false);
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Выбрать элемент
        /// </summary>
        private void SelectElement(object elem)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object>(SelectElement), elem);
            }
            else
            {
                if (elem != null)
                    lblNoSelObj.Visible = false;

                propGrid.SelectedObject = elem;

                if (elem == null)
                    lblNoSelObj.Visible = true;

                miSchemeCancelAddElem.Enabled = btnSchemeCancelAddElem.Enabled = false;
                miEditCut.Enabled = btnEditCut.Enabled = 
                    miEditCopy.Enabled = btnEditCopy.Enabled = 
                    miSchemeDelElem.Enabled = btnSchemeDelElem.Enabled = elem is SchemeView.Element;
            }
        }

        /// <summary>
        /// Установить заголовок формы
        /// </summary>
        private void SetFormTitle()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(SetFormTitle));
            }
            else
            {
                string title = SchemePhrases.EditorFormTitle + " - " +
                    (editorData.FileName == "" ? DefFileName : Path.GetFileName(editorData.FileName)) +
                    (editorData.Modified ? "*" : "");
                if (Text != title)
                    Text = title;
            }
        }

        /// <summary>
        /// Установить признак изменения в заголовке формы
        /// </summary>
        private void SetFormTitleModified()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(SetFormTitleModified));
            }
            else
            {
                if (editorData.Modified && !Text.EndsWith("*"))
                    Text += "*";
            }
        }

        /// <summary>
        /// Отобразить позицию указателя мыши над схемой
        /// </summary>
        private void ShowCursorPosition()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ShowCursorPosition));
            }
            else
            {
                lblStatus.Text = editorData.CursorPosition.X < 0 ? "" :
                    string.Format("X = {0}, Y = {1}", editorData.CursorPosition.X, editorData.CursorPosition.Y);
            }
        }
        
        /// <summary>
        /// Обмен данными со схемой
        /// </summary>
        /// <remarks>Метод вызывается в отдельном потоке</remarks>
        private void SchemeExchange()
        {
            while (true)
            {
                SetFormTitleModified();
                ShowCursorPosition();
                Thread.Sleep(100);
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            // определение директории исполняемого файла приложения
            string exeDir = ScadaUtils.NormalDir(Path.GetDirectoryName(Application.ExecutablePath));

            // локализация приложения
            if (!Localization.UseRussian)
            {
                string langDir = exeDir + "lang\\";
                string errMsg;

                if (Localization.LoadDictionaries(langDir, "ScadaData", out errMsg))
                    CommonPhrases.Init();
                else
                    ScadaUtils.ShowError(errMsg);

                if (Localization.LoadDictionaries(langDir, "ScadaSchemeEditor", out errMsg))
                {
                    Localization.TranslateForm(this, "Scada.Scheme.Editor.FrmMain");
                    SchemePhrases.InitStatic();
                    openFileDialog.Filter = saveFileDialog.Filter = SchemePhrases.FileFilter;
                }
                else
                {
                    ScadaUtils.ShowError(errMsg);
                }
            }

            // инициализация данных
            SchemeApp schemeApp = SchemeApp.InitSchemeApp(SchemeApp.WorkModes.Edit);
            schemeUrl = exeDir + "web\\ScadaScheme.html?editMode=true";
            editorData = schemeApp.EditorData;
            editorData.SelectElement = SelectElement;
            editorData.SetFormTitle = SetFormTitle;
            log = schemeApp.Log;
            elemClipboard = null;
            schemeSvcHost = null;
            domainSvcHost = null;
            schemeExThread = null;
            
            // проверка запуска второй копии программы
            try
            {
                bool createdNew;
                mutex = new Mutex(true, "ScadaSchemeEditorMutex", out createdNew);

                if (!createdNew)
                {
                    ScadaUtils.ShowInfo("SCADA-Редактор схем уже запущен.\nВторая копия будет закрыта.");
                    Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.WriteAction("Ошибка при проверке запуска второй копии программы: " + ex.Message,
                    Log.ActTypes.Exception);
            }

            // запуск WCF-служб
            if (StartWCF())
            {
                // создание и запуск потока для обмена данными со схемой
                schemeExThread = new Thread(SchemeExchange);
                schemeExThread.Start();

                // настройка элементов управления
                miEditCut.Enabled = btnEditCut.Enabled = false;
                miEditCopy.Enabled = btnEditCopy.Enabled = false;
                miEditPaste.Enabled = btnEditPaste.Enabled = false;
                miSchemeCancelAddElem.Enabled = btnSchemeCancelAddElem.Enabled = false;
                miSchemeDelElem.Enabled = btnSchemeDelElem.Enabled = false;

                // создание новой схемы
                miFileNew_Click(null, null);
            }
            else
            {
                // блокировка элементов управления
                foreach (ToolStripItem item in miFile.DropDown.Items)
                    item.Enabled = item == miFileExit;
                foreach (ToolStripItem item in miEdit.DropDown.Items)
                    item.Enabled = false;
                foreach (ToolStripItem item in miScheme.DropDown.Items)
                    item.Enabled = false;
                foreach (ToolStripItem item in toolMain.Items)
                    item.Enabled = false;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверка возможности закрыть схему
            e.Cancel = !CanCloseScheme();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // остановка потока для обмена данными со схемой
            if (schemeExThread != null)
                schemeExThread.Abort();

            // остановка WCF-служб
            if (schemeSvcHost != null)
            {
                try { schemeSvcHost.Close(); }
                catch { schemeSvcHost.Abort(); }
            }

            if (domainSvcHost != null)
            {
                try { domainSvcHost.Close(); }
                catch { domainSvcHost.Abort(); }
            }
        }


        private void miFileNew_Click(object sender, EventArgs e)
        {
            // создание новой схемы
            if (CanCloseScheme())
            {
                editorData.NewClientID();
                editorData.FileName = "";
                editorData.Modified = false;
                webBrowser.Url = new Uri(schemeUrl);
                SelectElement(null);
                SetFormTitle();
                miSchemeCancelAddElem_Click(null, null);
            }
        }

        private void miFileOpen_Click(object sender, EventArgs e)
        {
            // открытие схемы
            if (CanCloseScheme() && openFileDialog.ShowDialog() == DialogResult.OK)
            {
                editorData.NewClientID();
                editorData.FileName = openFileDialog.FileName;
                editorData.Modified = false;
                webBrowser.Url = new Uri(schemeUrl);
                SelectElement(null);
                SetFormTitle();
                miSchemeCancelAddElem_Click(null, null);
            }
        }

        private void miFileSave_Click(object sender, EventArgs e)
        {
            // сохранение схемы
            SaveScheme(false);
        }

        private void miEditSaveAs_Click(object sender, EventArgs e)
        {
            // сохранение схемы с выбором имени файла
            SaveScheme(true);
        }

        private void miEditCut_Click(object sender, EventArgs e)
        {
            // вырезать элемент в буфер
            miEditCopy_Click(null, null);
            miSchemeDelElem_Click(null, null);
        }

        private void miEditCopy_Click(object sender, EventArgs e)
        {
            // копировать элемент в буфер
            SchemeView.Element elem = propGrid.SelectedObject as SchemeView.Element;

            if (elem != null)
            {
                elemClipboard = (SchemeView.Element)elem.Clone();
                miEditPaste.Enabled = btnEditPaste.Enabled = true;
            }
        }

        private void miEditPaste_Click(object sender, EventArgs e)
        {
            // вставить элемент из буфера
            if (elemClipboard != null)
            {
                elemClipboard.ID = editorData.SchemeView.GetNextElementID();
                editorData.AddedElement = elemClipboard;
                elemClipboard = (SchemeView.Element)elemClipboard.Clone();
                miSchemeCancelAddElem.Enabled = btnSchemeCancelAddElem.Enabled = true;
            }
        }

        private void miFileExit_Click(object sender, EventArgs e)
        {
            // закрыть приложение
            Close();
        }

        private void miSchemeAddElem_Click(object sender, EventArgs e)
        {
            // добавление элемента
            SchemeView.Element elem;

            if (sender == miSchemeAddStText || sender == btnSchemeAddStText)
                elem = new SchemeView.StaticText();
            else if (sender == miSchemeAddDynText || sender == btnSchemeAddDynText)
                elem = new SchemeView.DynamicText();
            else if (sender == miSchemeAddStPic || sender == btnSchemeAddStPic)
                elem = new SchemeView.StaticPicture();
            else // miSchemeAddDynPic или btnSchemeAddDynPic
                elem = new SchemeView.DynamicPicture();

            elem.ID = editorData.SchemeView.GetNextElementID();
            editorData.AddedElement = elem;
            miSchemeCancelAddElem.Enabled = btnSchemeCancelAddElem.Enabled = true;
        }

        private void miSchemeCancelAddElem_Click(object sender, EventArgs e)
        {
            // отмена добавления элемента
            editorData.AddedElement = null;
            miSchemeCancelAddElem.Enabled = btnSchemeCancelAddElem.Enabled = false;
        }

        private void miSchemeDelElem_Click(object sender, EventArgs e)
        {
            // удаление элемента схемы 
            SchemeView.Element elem = propGrid.SelectedObject as SchemeView.Element;

            if (elem != null)
            {
                // удаление элемента из представления
                editorData.SchemeView.ElementList.Remove(elem);
                editorData.SchemeView.ElementDict.Remove(elem.ID);

                // создание объекта для передачи изменений
                SchemeView.SchemeChange change = new SchemeView.SchemeChange(SchemeView.ChangeType.ElementDeleted);
                change.ElementID = elem.ID;
                editorData.TrySetSchemeChange(change);

                // очистка таблицы свойств
                propGrid.SelectedObject = null;
            }
        }

        private void miHelpAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            (new FrmAbout()).ShowDialog();
        }


        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // создание объекта для передачи изменений
            object selObj = propGrid.SelectedObject;
            SchemeView.SchemeChange change = null;

            if (selObj is SchemeView.Element)
            {
                change = new SchemeView.SchemeChange(SchemeView.ChangeType.ElementChanged);
                change.ElementData = new SchemeView.ElementData((SchemeView.Element)selObj);
            }
            else if (selObj is SchemeView.Scheme)
            {
                change = new SchemeView.SchemeChange(SchemeView.ChangeType.SchemeChanged);
                change.SchemeParams = (SchemeView.Scheme)selObj;
            }

            if (change != null)
            {
                editorData.TrySetSchemeChange(change);
                SetFormTitleModified();
            }
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = e.Url.LocalPath != schemeUrl;
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
