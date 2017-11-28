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
 * Module   : Scheme Editor
 * Summary  : Main form of the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using Scada.Scheme.Model.PropertyGrid;
using Scada.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Main form of the application
    /// <para>Главная форма приложения</para>
    /// </summary>
    public partial class FrmMain : Form, IMainForm
    {
        private readonly AppData appData;  // общие данные приложения
        private readonly Log log;          // журнал приложения
        private readonly Editor editor;    // редактор

        private Mutex mutex;               // объект для проверки запуска второй копии приложения
        private bool compTypesChanging;    // пользователь изменяет выбранный элемент lvCompTypes
        private bool schCompChanging;      // пользователь изменяет выбранный элемент cbSchComp
        private FormStateDTO formStateDTO; // состояние формы для передачи


        /// <summary>
        /// Конструктор
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();

            appData = AppData.GetAppData();
            log = appData.Log;
            editor = appData.Editor;
            mutex = null;
            compTypesChanging = false;
            schCompChanging = false;
            formStateDTO = null;

            editor.ModifiedChanged += Editor_ModifiedChanged;
            editor.PointerModeChanged += Editor_PointerModeChanged;
            editor.StatusChanged += Editor_StatusChanged;
            editor.SelectionChanged += Editor_SelectionChanged;
            editor.SelectionPropsChanged += Editor_SelectionPropsChanged;
            editor.ClipboardChanged += Editor_ClipboardChanged;
            editor.History.HistoryChanged += History_HistoryChanged;
        }


        /// <summary>
        /// Локализовать форму
        /// </summary>
        private void LocalizeForm()
        {
            string errMsg;

            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaData", out errMsg))
                CommonPhrases.Init();
            else
                log.WriteError(errMsg);

            if (Localization.LoadDictionaries(appData.AppDirs.LangDir, "ScadaSchemeEditor", out errMsg))
            {
                Translator.TranslateForm(this, "Scada.Scheme.Editor.FrmMain");
                SchemePhrases.Init();
                AppPhrases.Init();
                ofdScheme.Filter = sfdScheme.Filter = AppPhrases.SchemeFileFilter;
            }
            else
            {
                log.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Локализовать атрибуты для отображения свойств компонентов
        /// </summary>
        private void LocalizeAttributes()
        {
            try
            {
                AttrTranslator attrTranslator = new AttrTranslator();
                attrTranslator.TranslateAttrs(typeof(SchemeDocument));
                attrTranslator.TranslateAttrs(typeof(BaseComponent));
                attrTranslator.TranslateAttrs(typeof(StaticText));
                attrTranslator.TranslateAttrs(typeof(DynamicText));
                attrTranslator.TranslateAttrs(typeof(StaticPicture));
                attrTranslator.TranslateAttrs(typeof(DynamicPicture));
                attrTranslator.TranslateAttrs(typeof(Condition));
                attrTranslator.TranslateAttrs(typeof(FrmImageDialog.ImageListItem));
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при локализации атрибутов" :
                    "Error localizing attributes");
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
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при проверке существования второй копии приложения" :
                    "Error checking existence of a second copy of the application");
                return false;
            }
        }

        /// <summary>
        /// Заполнить список типов компонентов
        /// </summary>
        private void FillComponentTypes()
        {
            try
            {
                lvCompTypes.BeginUpdate();
                CompLibSpec[] specs = appData.CompManager.GetSortedSpecs();

                foreach (CompLibSpec spec in specs)
                {
                    ListViewGroup listViewGroup = new ListViewGroup(spec.GroupHeader);

                    // добавление элемента с указателем
                    lvCompTypes.Items.Add(new ListViewItem(
                        "Pointer", "pointer.png", listViewGroup) { IndentCount = 1 });

                    // добавление компонентов
                    foreach (CompItem compItem in spec.CompItems)
                    {
                        string imageKey = "image" + ilCompTypes.Images.Count;
                        ilCompTypes.Images.Add(imageKey, compItem.Icon);

                        lvCompTypes.Items.Add(new ListViewItem()
                        {
                            Text = compItem.DisplayName,
                            ImageKey = imageKey,
                            Tag = compItem.CompType?.FullName,
                            Group = listViewGroup,
                            IndentCount = 1
                        });
                    }

                    lvCompTypes.Groups.Add(listViewGroup);
                }
            }
            finally
            {
                lvCompTypes.EndUpdate();
            }
        }

        /// <summary>
        /// Открыть браузер со страницей редактора
        /// </summary>
        private void OpenBrowser()
        {
            Uri startUri = new Uri(editor.GetWebPageFilePath(appData.AppDirs.WebDir));
            //Process.Start("firefox", startUri.AbsoluteUri);
            Process.Start(startUri.AbsoluteUri);
        }

        /// <summary>
        /// Инициализировать схему, создав новую или загрузив из файла
        /// </summary>
        private void InitScheme(string fileName = "")
        {
            bool loadOK;
            string errMsg;

            if (string.IsNullOrEmpty(fileName))
            {
                loadOK = true;
                errMsg = "";
                editor.NewScheme();
            }
            else
            {
                loadOK = editor.LoadSchemeFromFile(fileName, out errMsg);
            }

            appData.AssignViewStamp(editor.SchemeView);
            FillSchemeComponents();
            ShowSchemeSelection();
            SubscribeToSchemeChanges();
            editor.History.MakeCopy(editor.SchemeView);
            Text = editor.Title;

            if (!loadOK)
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Сохранить схему
        /// </summary>
        private bool SaveScheme(bool saveAs)
        {
            bool result = false;
            bool refrPropGrid = propertyGrid.SelectedObject is SchemeDocument &&
                ((SchemeDocument)propertyGrid.SelectedObject).Version != SchemeUtils.SchemeVersion;

            if (string.IsNullOrEmpty(editor.FileName))
            {
                sfdScheme.FileName = Editor.DefSchemeFileName;
                saveAs = true;
            }
            else
            {
                sfdScheme.FileName = editor.FileName;
            }

            if (!saveAs || sfdScheme.ShowDialog() == DialogResult.OK)
            {
                // сохранение схемы
                string errMsg;
                if (editor.SaveSchemeToFile(sfdScheme.FileName, out errMsg))
                {
                    result = true;
                }
                else
                {
                    log.WriteError(errMsg);
                    ScadaUiUtils.ShowError(errMsg);
                }

                // обновить свойства документа схемы, если файл сохраняется другой версией редактора
                if (refrPropGrid)
                    propertyGrid.Refresh();
            }

            return result;
        }

        /// <summary>
        /// Подтвердить возможность закрыть схему
        /// </summary>
        private bool ConfirmCloseScheme()
        {
            if (editor.Modified)
            {
                switch (MessageBox.Show(AppPhrases.SaveSchemeConfirm, CommonPhrases.QuestionCaption,
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
        /// Заполнить выпадающий список компонентов схемы
        /// </summary>
        private void FillSchemeComponents()
        {
            try
            {
                cbSchComp.BeginUpdate();
                cbSchComp.Items.Clear();

                if (editor.SchemeView != null)
                {
                    lock (editor.SchemeView)
                    {
                        cbSchComp.Items.Add(editor.SchemeView.SchemeDoc);

                        foreach (BaseComponent component in editor.SchemeView.Components.Values)
                        {
                            cbSchComp.Items.Add(component);
                        }
                    }
                }
            }
            finally
            {
                cbSchComp.EndUpdate();
            }
        }

        /// <summary>
        /// Отобразить свойства выбранных компонентов схемы
        /// </summary>
        private void ShowSchemeSelection()
        {
            BaseComponent[] selection = editor.GetSelectedComponents();
            object[] selObjects;

            if (selection != null && selection.Length > 0)
            {
                // выбор компонентов схемы
                selObjects = selection;
            }
            else
            {
                // выбор свойств документа схемы
                selObjects = editor.SchemeView == null ?
                    null : new object[] { editor.SchemeView.SchemeDoc };
            }

            // отображение выбранных объектов
            propertyGrid.SelectedObjects = selObjects;

            // выбор объекта в выпадающем списке
            if (!schCompChanging)
            {
                cbSchComp.SelectedIndexChanged -= cbSchComp_SelectedIndexChanged;
                cbSchComp.SelectedItem = selObjects != null && selObjects.Length == 1 ? selObjects[0] : null;
                cbSchComp.SelectedIndexChanged += cbSchComp_SelectedIndexChanged;
            }

            // установка доступности кнопок
            SetButtonsEnabled();
        }

        /// <summary>
        /// Подписаться на изменения схемы
        /// </summary>
        private void SubscribeToSchemeChanges()
        {
            SchemeView schemeView = editor.SchemeView;

            if (schemeView != null)
            {
                schemeView.SchemeDoc.ItemChanged += Scheme_ItemChanged;

                foreach (BaseComponent component in schemeView.Components.Values)
                    component.ItemChanged += Scheme_ItemChanged;
            }
        }

        /// <summary>
        /// Установить доступность кнопок панели инструментов
        /// </summary>
        private void SetButtonsEnabled()
        {
            btnEditCut.Enabled = btnEditCopy.Enabled = btnEditDelete.Enabled = editor.SelectionNotEmpty;
            btnEditPaste.Enabled = editor.ClipboardNotEmpty;
            btnEditPointer.Enabled = editor.PointerMode != Editor.PointerModes.Select;
            btnEditUndo.Enabled = editor.History.CanUndo;
            btnEditRedo.Enabled = editor.History.CanRedo;
        }

        /// <summary>
        /// Выполнить метод потокобезопасно
        /// </summary>
        private void ExecuteAction(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        /// <summary>
        /// Обновить объект состояния формы
        /// </summary>
        private void UpdateFormStateDTO(FormState formState)
        {
            formStateDTO = formState.GetFormStateDTO(WindowState != FormWindowState.Minimized);
        }

        /// <summary>
        /// Обновить объект состояния формы
        /// </summary>
        private void UpdateFormStateDTO()
        {
            UpdateFormStateDTO(new FormState(this, true));
        }


        /// <summary>
        /// Выполнить заданное действие
        /// </summary>
        public void PerformAction(FormActions formAction)
        {
            ExecuteAction(() =>
            {
                BringToFront();

                switch (formAction)
                {
                    case FormActions.New:
                        btnFileNew_Click(null, null);
                        break;
                    case FormActions.Open:
                        btnFileOpen_Click(null, null);
                        break;
                    case FormActions.Save:
                        btnFileSave_ButtonClick(null, null);
                        break;
                    case FormActions.Cut:
                        btnEditCut_Click(null, null);
                        break;
                    case FormActions.Copy:
                        btnEditCopy_Click(null, null);
                        break;
                    case FormActions.Paste:
                        btnEditPaste_Click(null, null);
                        break;
                    case FormActions.Undo:
                        btnEditUndo_Click(null, null);
                        break;
                    case FormActions.Redo:
                        btnEditRedo_Click(null, null);
                        break;
                    case FormActions.Pointer:
                        btnEditPointer_Click(null, null);
                        break;
                    case FormActions.Delete:
                        btnEditDelete_Click(null, null);
                        break;
                }
            });
        }

        /// <summary>
        /// Получить состояние формы
        /// </summary>
        public FormStateDTO GetFormState()
        {
            return formStateDTO;
        }


        private void Scheme_ItemChanged(object sender, SchemeChangeTypes changeType, 
            object changedObject, object oldKey)
        {
            ExecuteAction(() =>
            {
                switch (changeType)
                {
                    case SchemeChangeTypes.ComponentAdded:
                        // привязка события на изменение компонента
                        ((BaseComponent)changedObject).ItemChanged += Scheme_ItemChanged;

                        // добавление компонента в выпадающий список
                        cbSchComp.Items.Add(changedObject);
                        break;

                    case SchemeChangeTypes.ComponentChanged:
                        // обновление текста выпадающего списка при изменении отображаемого наименования выбранного объекта
                        object selItem = cbSchComp.SelectedItem;
                        if (selItem != null)
                        {
                            string newDisplayName = selItem.ToString();
                            string oldDisplayName = cbSchComp.Text;
                            if (oldDisplayName != newDisplayName)
                                cbSchComp.Items[cbSchComp.SelectedIndex] = selItem;
                        }
                        break;

                    case SchemeChangeTypes.ComponentDeleted:
                        // удаление компонента из выпадающего списка
                        cbSchComp.Items.Remove(changedObject);
                        break;
                }

                SetButtonsEnabled();
            });
        }

        private void Editor_ModifiedChanged(object sender, EventArgs e)
        {
            ExecuteAction(() => { Text = editor.Title; });
        }

        private void Editor_PointerModeChanged(object sender, EventArgs e)
        {
            ExecuteAction(() =>
            {
                // очистка типа создаваемых компонентов, если режим создания выключен
                if (!compTypesChanging && editor.PointerMode != Editor.PointerModes.Create)
                {
                    lvCompTypes.SelectedIndexChanged -= lvCompTypes_SelectedIndexChanged;
                    lvCompTypes.SelectedItems.Clear();
                    lvCompTypes.SelectedIndexChanged += lvCompTypes_SelectedIndexChanged;
                }

                // установка доступности кнопок
                SetButtonsEnabled();
            });
        }

        private void Editor_StatusChanged(object sender, EventArgs e)
        {
            // вывод статуса редактора
            ExecuteAction(() => { lblStatus.Text = editor.Status; });
        }

        private void Editor_SelectionChanged(object sender, EventArgs e)
        {
            // отображение свойств выбранных компонентов схемы
            ExecuteAction(ShowSchemeSelection);
        }

        private void Editor_SelectionPropsChanged(object sender, EventArgs e)
        {
            // обновление значений свойств
            ExecuteAction(propertyGrid.Refresh);
        }

        private void Editor_ClipboardChanged(object sender, EventArgs e)
        {
            // установка доступности кнопок
            ExecuteAction(SetButtonsEnabled);
        }

        private void History_HistoryChanged(object sender, EventArgs e)
        {
            // установка доступности кнопок
            ExecuteAction(SetButtonsEnabled);
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            // инициализация общих данных приложения
            appData.Init(Path.GetDirectoryName(Application.ExecutablePath), this);

            // локализация
            LocalizeForm();
            LocalizeAttributes();

            // проверка существования второй копии приложения
            if (SecondInstanceExists())
            {
                ScadaUiUtils.ShowInfo(AppPhrases.CloseSecondInstance);
                Close();
                log.WriteAction(Localization.UseRussian ?
                    "Вторая копия Редактора схем закрыта." :
                    "The second instance of Scheme Editor has been closed.");
                return;
            }

            // настройка элментов управления
            lvCompTypes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            lblStatus.Text = "";
            FillComponentTypes();

            // создание новой схемы
            InitScheme();

            // загрузка состояния формы
            FormState formState = new FormState();
            string errMsg;
            if (!formState.Load(appData.AppDirs.ConfigDir + FormState.DefFileName, out errMsg))
            {
                log.WriteError(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }
            formState.Apply(this);
            UpdateFormStateDTO();

            // запуск механизма редактора схем
            if (appData.StartEditor())
            {
                // открытие браузера со страницей редактора
                OpenBrowser();
            }
            else
            {
                ScadaUiUtils.ShowInfo(string.Format(AppPhrases.FailedToStartEditor, log.FileName));
                Close();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // проверка возможности закрыть схему
            e.Cancel = !ConfirmCloseScheme();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // сохранение состояния формы
            FormState formState = new FormState(this);
            string errMsg;
            if (!formState.Save(appData.AppDirs.ConfigDir + FormState.DefFileName, out errMsg))
            {
                log.WriteError(errMsg);
                ScadaUiUtils.ShowError(errMsg);
            }

            // завершение работы приложения
            appData.FinalizeApp();
        }

        private void FrmMain_MouseEnter(object sender, EventArgs e)
        {
            // активировать форму при наведении мыши
            if (ActiveForm != this)
                BringToFront();
        }

        private void FrmMain_Move(object sender, EventArgs e)
        {
            // притягивание формы к краям экрана при необходимости
            FormState correctFormState = new FormState(this, true);
            if (!correctFormState.PullToEdge(this))
                UpdateFormStateDTO(correctFormState);
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            UpdateFormStateDTO();
            lvCompTypes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // реализация горячих клавиш
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        btnFileNew_Click(null, null);
                        break;
                    case Keys.O:
                        btnFileOpen_Click(null, null);
                        break;
                    case Keys.S:
                        btnFileSave_ButtonClick(null, null);
                        break;
                }

                if (ActiveControl != propertyGrid)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.X:
                            btnEditCut_Click(null, null);
                            break;
                        case Keys.C:
                            btnEditCopy_Click(null, null);
                            break;
                        case Keys.V:
                            btnEditPaste_Click(null, null);
                            break;
                        case Keys.Z:
                            btnEditUndo_Click(null, null);
                            break;
                        case Keys.Y:
                            btnEditRedo_Click(null, null);
                            break;
                    }
                }
            }
            else if (ActiveControl != propertyGrid)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnEditPointer_Click(null, null);
                        break;
                    case Keys.Delete:
                        btnEditDelete_Click(null, null);
                        break;
                }
            }
        }


        private void btnFileNew_Click(object sender, EventArgs e)
        {
            // создание новой схемы
            if (ConfirmCloseScheme())
                InitScheme();
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            // открытие схемы из файла
            if (ConfirmCloseScheme())
            {
                ofdScheme.InitialDirectory = string.IsNullOrEmpty(editor.FileName) ? 
                    "" : Path.GetDirectoryName(editor.FileName);
                ofdScheme.FileName = "";

                if (ofdScheme.ShowDialog() == DialogResult.OK)
                    InitScheme(ofdScheme.FileName);
            }
        }

        private void btnFileSave_ButtonClick(object sender, EventArgs e)
        {
            // сохранение схемы
            SaveScheme(false);
        }

        private void miFileSaveAs_Click(object sender, EventArgs e)
        {
            // сохранение схемы с выбором имени файла
            SaveScheme(true);
        }

        private void btnFileOpenBrowser_Click(object sender, EventArgs e)
        {
            OpenBrowser();
        }

        private void btnEditCut_Click(object sender, EventArgs e)
        {
            // копирование в буфер обмена и удаление выбранных компонентов схемы
            editor.CopyToClipboard();
            editor.DeleteSelected();
        }

        private void btnEditCopy_Click(object sender, EventArgs e)
        {
            // копировать выбранные компоненты в буфер обмена
            editor.CopyToClipboard();
        }

        private void btnEditPaste_Click(object sender, EventArgs e)
        {
            // включение режима вставки компонентов
            editor.PointerMode = Editor.PointerModes.Paste;
        }

        private void btnEditUndo_Click(object sender, EventArgs e)
        {
            // отмена последнего действия
            schCompChanging = true;
            editor.Undo();
            schCompChanging = false;

            // обновление списка компонентов, т.к. при отмене происходит подмена объектов
            FillSchemeComponents();
            ShowSchemeSelection();
        }

        private void btnEditRedo_Click(object sender, EventArgs e)
        {
            // возврат последнего действия
            schCompChanging = true;
            editor.Redo();
            schCompChanging = false;

            // обновление списка компонентов, т.к. при возврате происходит подмена объектов
            FillSchemeComponents();
            ShowSchemeSelection();
        }

        private void btnEditPointer_Click(object sender, EventArgs e)
        {
            // включение режима выбора компонентов
            editor.PointerMode = Editor.PointerModes.Select;
        }

        private void btnEditDelete_Click(object sender, EventArgs e)
        {
            // удаление выбранных компонентов схемы
            editor.DeleteSelected();
        }

        private void btnHelpAbout_Click(object sender, EventArgs e)
        {
            // отображение формы о программе
            FrmAbout.ShowAbout(appData.AppDirs.ExeDir, log, this);
        }


        private void lvCompTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // выбор компонента для добавления на схему
            compTypesChanging = true;
            string typeName = lvCompTypes.SelectedItems.Count > 0 ?
                lvCompTypes.SelectedItems[0].Tag as string : "";

            if (string.IsNullOrEmpty(typeName))
            {
                // включение режима выбора компонентов
                editor.PointerMode = Editor.PointerModes.Select;
            }
            else
            {
                // включение режима создания компонента
                editor.NewComponentTypeName = typeName;
                editor.PointerMode = Editor.PointerModes.Create;
            }

            compTypesChanging = false;
        }

        private void cbSchComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            // отображение свойств объекта, выбранного в выпадающем списке
            schCompChanging = true;
            BaseComponent component = cbSchComp.SelectedItem as BaseComponent;

            if (component == null)
                editor.DeselectAll();
            else
                editor.SelectComponent(component.ID);

            schCompChanging = false;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // отслеживание изменений
            if (propertyGrid.SelectedObjects != null)
            {
                editor.History.BeginPoint();

                foreach (object selObj in propertyGrid.SelectedObjects)
                {
                    if (selObj is SchemeDocument)
                        ((SchemeDocument)selObj).OnItemChanged(SchemeChangeTypes.SchemeDocChanged, selObj);
                    else if (selObj is BaseComponent)
                        ((BaseComponent)selObj).OnItemChanged(SchemeChangeTypes.ComponentChanged, selObj);
                }

                editor.History.EndPoint();
            }
        }
    }
}
