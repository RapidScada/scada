/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Editor data and logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

using Scada.Scheme.Model;
using Scada.Scheme.Model.DataTypes;
using Scada.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Editor data and logic.
    /// <para>Данные и логика редактора.</para>
    /// </summary>
    internal sealed class Editor
    {
        /// <summary>
        /// Длина идентификатора редактора
        /// </summary>
        private const int EditorIDLength = 10;
        /// <summary>
        /// Имя файла веб-страницы редактора
        /// </summary>
        private const string WebPageFileName = "editor.html";
        /// <summary>
        /// Путь к директории плагинов относительно веб-страницы редактора
        /// </summary>
        private const string PluginsRoot = "../";
        /// <summary>
        /// Имя файла схемы по умолчанию
        /// </summary>
        public const string DefSchemeFileName = "NewScheme.sch";

        private readonly CompManager compManager;  // менеджер компонентов
        private readonly Log log;                  // журнал приложения
        private List<Change> changes;              // изменения схемы
        private bool modified;                     // признак изменения схемы
        private long changeStampCntr;              // счётчик для генерации меток изменений схемы
        private PointerMode pointerMode;          // режим указателя мыши редактора
        private string status;                     // статус редактора
        private List<BaseComponent> selComponents; // выбранные компоненты схемы
        private List<BaseComponent> clipboard;     // буфер обмена, содержащий скопированные компоненты


        /// <summary>
        /// Конструктор
        /// </summary>
        public Editor(CompManager compManager, Log log)
        {
            this.compManager = compManager ?? throw new ArgumentNullException("compManager");
            this.log = log ?? throw new ArgumentNullException("log");
            changes = new List<Change>();
            changeStampCntr = 0;
            modified = false;
            pointerMode = PointerMode.Select;
            selComponents = new List<BaseComponent>();
            clipboard = new List<BaseComponent>();

            EditorID = GetRandomString(EditorIDLength);
            SchemeView = null;
            FileName = "";
            History = new History(log);
            PasteSpecialParams = new PasteSpecialParams();
            NewComponentTypeName = "";
        }


        /// <summary>
        /// Получить случайную строку символов
        /// </summary>
        private static string GetRandomString(int length)
        {
            const string Abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyxz0123456789";
            int abcLen = Abc.Length;
            Random rand = new Random();
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
                chars[i] = Abc[rand.Next(abcLen)];

            return new string(chars);
    }


        /// <summary>
        /// Получить идентификатор редактора
        /// </summary>
        public string EditorID { get; private set; }

        /// <summary>
        /// Получить представление редактируемой схемы
        /// </summary>
        public SchemeView SchemeView { get; private set; }

        /// <summary>
        /// Получить имя файла схемы
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Получить признак изменения схемы
        /// </summary>
        public bool Modified
        {
            get
            {
                return modified;
            }
            private set
            {
                if (modified != value)
                {
                    modified = value;
                    OnModifiedChanged();
                }
            }
        }

        /// <summary>
        /// Получить заголовок редактора
        /// </summary>
        public string Title
        {
            get
            {
                return (string.IsNullOrEmpty(FileName) ? DefSchemeFileName : Path.GetFileName(FileName)) +
                   (Modified ? "*" : "") + " - " + AppPhrases.EditorTitle;
            }
        }

        /// <summary>
        /// Получить историю изменений
        /// </summary>
        public History History { get; private set; }

        /// <summary>
        /// Получить или установить режим указателя мыши редактора
        /// </summary>
        public PointerMode PointerMode
        {
            get
            {
                return pointerMode;
            }
            set
            {
                if (pointerMode != value)
                {
                    pointerMode = value;
                    OnPointerModeChanged();
                }

                if (pointerMode != PointerMode.Create)
                    NewComponentTypeName = "";
            }
        }

        /// <summary>
        /// Gets the special paste parameters.
        /// </summary>
        public PasteSpecialParams PasteSpecialParams { get; private set; }

        /// <summary>
        /// Получить или установить статус редактора - полезную информацию для пользователя
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnStatusChanged();
                }
            }
        }

        /// <summary>
        /// Получить или установить имя типа компонента, который может быть создан пользователем
        /// </summary>
        public string NewComponentTypeName { get; set; }

        /// <summary>
        /// Получить признак, что имеются выбранные компоненты схемы
        /// </summary>
        public bool SelectionNotEmpty
        {
            get
            {
                return selComponents.Count > 0;
            }
        }

        /// <summary>
        /// Получить признак, что буфер обмена не пуст
        /// </summary>
        public bool ClipboardNotEmpty
        {
            get
            {
                return clipboard.Count > 0;
            }
        }


        /// <summary>
        /// Инициализировать схему, создав новую или загрузив из файла
        /// </summary>
        private bool InitScheme(string fileName, out string errMsg)
        {
            ClearChanges();
            ClearSelComponents();
            SchemeView = new SchemeView();
            bool loadOK;

            if (string.IsNullOrEmpty(fileName))
            {
                loadOK = true;
                errMsg = "";
            }
            else
            {
                lock (SchemeView.SyncRoot)
                {
                    loadOK = SchemeView.LoadFromFile(fileName, out errMsg);
                }

                if (!loadOK)
                    log.WriteError(errMsg);
            }

            FileName = fileName;
            Modified = false;
            History.Clear();
            PointerMode = PointerMode.Select;
            SubscribeToSchemeChanges();
            OnSelectionChanged();
            return loadOK;
        }

        /// <summary>
        /// Подписаться на изменения схемы
        /// </summary>
        private void SubscribeToSchemeChanges()
        {
            if (SchemeView != null)
            {
                SchemeView.SchemeDoc.ItemChanged += Scheme_ItemChanged;

                foreach (BaseComponent component in SchemeView.Components.Values)
                    component.ItemChanged += Scheme_ItemChanged;
            }
        }

        /// <summary>
        /// Очистить изменения схемы
        /// </summary>
        private void ClearChanges()
        {
            lock (changes)
            {
                changes.Clear();
                changeStampCntr = 0;
            }
        }

        /// <summary>
        /// Очистить выбранные компоненты схемы
        /// </summary>
        private void ClearSelComponents()
        {
            lock (selComponents)
            {
                selComponents.Clear();
            }
        }

        /// <summary>
        /// Применить заданные изменения схемы
        /// </summary>
        private void ApplyChanges(List<Change> changeList)
        {
            lock (SchemeView.SyncRoot)
            {
                lock (selComponents)
                {
                    SchemeDocument schemeDoc = SchemeView.SchemeDoc;
                    History.DisableAdding();
                    selComponents.Clear();

                    foreach (Change change in changeList)
                    {
                        switch (change.ChangeType)
                        {
                            case SchemeChangeTypes.SchemeDocChanged:
                                // копирование изменений свойств документа схемы
                                ((SchemeDocument)change.ChangedObject).CopyTo(SchemeView.SchemeDoc);
                                schemeDoc.OnItemChanged(SchemeChangeTypes.SchemeDocChanged, schemeDoc);
                                break;

                            case SchemeChangeTypes.ComponentAdded:
                                // создание копии компонента
                                BaseComponent addedComponent = ((BaseComponent)change.ChangedObject).Clone();

                                // добавление компонента на схему
                                SchemeView.Components[addedComponent.ID] = addedComponent;
                                schemeDoc.OnItemChanged(SchemeChangeTypes.ComponentAdded, addedComponent);

                                // выбор добавленного компонента
                                selComponents.Add(addedComponent);
                                break;

                            case SchemeChangeTypes.ComponentChanged:
                                // создание копии компонента
                                BaseComponent changedComponent = ((BaseComponent)change.ChangedObject).Clone();

                                // замена компонента на схеме
                                SchemeView.Components[changedComponent.ID] = changedComponent;
                                changedComponent.OnItemChanged(SchemeChangeTypes.ComponentChanged, changedComponent);

                                // выбор изменённого компонента
                                selComponents.Add(changedComponent);
                                break;

                            case SchemeChangeTypes.ComponentDeleted:
                                // удаление компонента
                                BaseComponent deletedComponent = (BaseComponent)change.ChangedObject;
                                SchemeView.Components.Remove(deletedComponent.ID);
                                schemeDoc.OnItemChanged(SchemeChangeTypes.ComponentDeleted, deletedComponent);
                                break;

                            case SchemeChangeTypes.ImageAdded:
                                // добавление изображения
                                Image addedImage = ((Image)change.ChangedObject).Copy();
                                schemeDoc.Images[addedImage.Name] = addedImage;
                                schemeDoc.OnItemChanged(SchemeChangeTypes.ImageAdded, addedImage);
                                break;

                            case SchemeChangeTypes.ImageRenamed:
                                // переименование изображения
                                Image renamedImage = ((Image)change.ChangedObject).Copy();
                                schemeDoc.Images.Remove(change.OldImageName);
                                schemeDoc.Images[renamedImage.Name] = renamedImage;
                                schemeDoc.OnItemChanged(
                                    SchemeChangeTypes.ImageRenamed, renamedImage, change.OldImageName);
                                break;

                            case SchemeChangeTypes.ImageDeleted:
                                // удаление изображения
                                Image deletedImage = (Image)change.ChangedObject;
                                schemeDoc.Images.Remove(deletedImage.Name);
                                schemeDoc.OnItemChanged(SchemeChangeTypes.ImageDeleted, deletedImage);
                                break;
                        }
                    }

                    History.EnableAdding();
                }
            }

            OnSelectionChanged();
            PointerMode = PointerMode.Select;
        }

        /// <summary>
        /// Вызвать событие ModifiedChanged
        /// </summary>
        private void OnModifiedChanged()
        {
            ModifiedChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Вызвать событие PointerModeChanged
        /// </summary>
        private void OnPointerModeChanged()
        {
            PointerModeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Вызвать событие StatusChanged
        /// </summary>
        private void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Вызвать событие SelectionChanged
        /// </summary>
        private void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Вызвать событие SelectionPropsChanged
        /// </summary>
        private void OnSelectionPropsChanged()
        {
            SelectionPropsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Вызвать событие ClipboardChanged
        /// </summary>
        private void OnClipboardChanged()
        {
            ClipboardChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обработать событие изменения схемы
        /// </summary>
        private void Scheme_ItemChanged(object sender, SchemeChangeTypes changeType, object changedObject, object oldKey)
        {
            // создание объекта изменения
            Change change = new Change(changeType, changedObject, oldKey)
            {
                Stamp = ++changeStampCntr
            };

            // добавление изменения в список изменений
            lock (changes)
            {
                changes.Add(change);
            }

            // добавление изменения в историю
            History.Add(changeType, changedObject, oldKey);

            // установка признака изменений
            Modified = true;
        }


        /// <summary>
        /// Получить полный путь файла веб-страницы редактора
        /// </summary>
        public string GetWebPageFilePath(string webDir)
        {
            return Path.Combine(webDir, "plugins", "SchemeEditor", WebPageFileName);
        }

        /// <summary>
        /// Создать веб-страницу редактора
        /// </summary>
        public bool CreateWebPage(string webDir, string serviceUrl)
        {
            try
            {
                // загрузка шаблона веб-страницы
                string webPageTemplate;

                using (Stream stream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("Scada.Scheme.Editor.Web.plugins.SchemeEditor.editor.html"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        webPageTemplate = reader.ReadToEnd();
                    }
                }

                // формирование списка стилей компонентов схемы
                StringBuilder sbCompStyles = new StringBuilder();
                List<string> compStyles = compManager.GetAllStyles();

                foreach (string stylePath in compStyles)
                {
                    sbCompStyles.AppendFormat(WebUtils.StyleTemplate, PluginsRoot + stylePath).AppendLine();
                }

                // формирование списка скриптов компонентов схемы
                StringBuilder sbCompScripts = new StringBuilder();
                List<string> compScripts = compManager.GetAllScripts();

                foreach (string scriptPath in compScripts)
                {
                    sbCompScripts.AppendFormat(WebUtils.ScriptTemplate, PluginsRoot + scriptPath).AppendLine();
                }

                // формирование скрипта редактора
                StringBuilder sbEditorScript = new StringBuilder();
                sbEditorScript
                    .AppendLine("<script type=\"text/javascript\">")
                    .AppendFormat("var editorID = '{0}';", EditorID)
                    .AppendLine()
                    .Append("var phrases = ")
                    .Append(WebUtils.DictionaryToJs("Scada.Scheme.Editor.Js"))
                    .AppendLine(";")
                    .AppendFormat("var serviceUrl = '{0}';", serviceUrl)
                    .AppendLine()
                    .Append("</script>");

                // формирование содержимого веб-страницы
                string webPageContent = string.Format(webPageTemplate, 
                    sbCompStyles.ToString(), 
                    sbCompScripts.ToString(), 
                    sbEditorScript.ToString());

                // запись файла веб-страницы
                using (StreamWriter writer = new StreamWriter(GetWebPageFilePath(webDir), false, Encoding.UTF8))
                {
                    writer.Write(webPageContent);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при создании веб-страницы редактора" :
                    "Error creating editor web page");
                return false;
            }
        }

        /// <summary>
        /// Создать новую схему
        /// </summary>
        public void NewScheme()
        {
            InitScheme("", out string errMsg);
        }

        /// <summary>
        /// Загрузить схему из файла
        /// </summary>
        public bool LoadSchemeFromFile(string fileName, out string errMsg)
        {
            return InitScheme(fileName, out errMsg);
        }

        /// <summary>
        /// Записать схему в файл
        /// </summary>
        public bool SaveSchemeToFile(string fileName, out string errMsg)
        {
            FileName = fileName;

            if (SchemeView == null)
            {
                errMsg = "";
                return true;
            }
            else
            {
                bool saveOK;

                lock (SchemeView.SyncRoot)
                {
                    saveOK = SchemeView.SaveToFile(fileName, out errMsg);
                }

                if (saveOK)
                    Modified = false;
                else
                    log.WriteError(errMsg);

                return saveOK;
            }
        }

        /// <summary>
        /// Получить изменения схемы для передачи
        /// </summary>
        public Change[] GetChanges(long trimBeforeStamp)
        {
            lock (changes)
            {
                while (changes.Count > 0 && changes[0].Stamp <= trimBeforeStamp)
                {
                    changes.RemoveAt(0);
                }

                int cnt = changes.Count;
                Change[] destChanges = new Change[cnt];

                for (int i = 0; i < cnt; i++)
                {
                    destChanges[i] = changes[i].ConvertToDTO();
                }

                return destChanges;
            }
        }

        /// <summary>
        /// Получить выбранные компоненты схемы
        /// </summary>
        public BaseComponent[] GetSelectedComponents()
        {
            lock (selComponents)
            {
                int cnt = selComponents.Count;
                BaseComponent[] destSelComponents = new BaseComponent[cnt];

                for (int i = 0; i < cnt; i++)
                {
                    destSelComponents[i] = selComponents[i];
                }

                return destSelComponents;
            }
        }

        /// <summary>
        /// Получить идентификаторы выбранных компонентов схемы
        /// </summary>
        public int[] GetSelectedComponentIDs()
        {
            lock (selComponents)
            {
                int cnt = selComponents.Count;
                int[] ids = new int[cnt];

                for (int i = 0; i < cnt; i++)
                {
                    ids[i] = selComponents[i].ID;
                }

                return ids;
            }
        }

        /// <summary>
        /// Создать компонент схемы
        /// </summary>
        public bool CreateComponent(int x, int y)
        {
            try
            {
                // проверка возможности создания компонента
                if (SchemeView == null)
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Схема не загружена." :
                        "Scheme is not loaded.");
                }

                if (string.IsNullOrEmpty(NewComponentTypeName))
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Не определён тип создаваемого компонента." :
                        "Type of the creating component is not defined.");
                }

                // создание компонента
                BaseComponent component = compManager.CreateComponent(NewComponentTypeName);

                if (component == null)
                {
                    return false;
                }
                else
                {
                    component.ID = SchemeView.GetNextComponentID();
                    component.Location = new Point(x, y);
                    component.SchemeView = SchemeView;
                    component.ItemChanged += Scheme_ItemChanged;

                    // добавление компонента на схему
                    lock (SchemeView.SyncRoot)
                    {
                        SchemeView.Components[component.ID] = component;
                    }

                    SchemeView.SchemeDoc.OnItemChanged(SchemeChangeTypes.ComponentAdded, component);

                    // выбор добавленного компонента
                    lock (selComponents)
                    {
                        selComponents.Clear();
                        selComponents.Add(component);
                    }

                    OnSelectionChanged();
                    PointerMode = PointerMode.Select;
                    return true;
                }
            }
            catch (ScadaException ex)
            {
                log.WriteError(string.Format(Localization.UseRussian ?
                    "Ошибка при создании компонента схемы: {0}" :
                    "Error creating scheme component: {0}", ex.Message));
                return false;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при создании компонента схемы" :
                    "Error creating scheme component");
                return false;
            }
        }

        /// <summary>
        /// Удалить выбранные компоненты схемы
        /// </summary>
        public void DeleteSelected()
        {
            try
            {
                if (SchemeView != null)
                {
                    // удаление выбранных компонентов
                    lock (SchemeView.SyncRoot)
                    {
                        lock (selComponents)
                        {
                            if (selComponents.Count > 0)
                            {
                                History.BeginPoint();

                                foreach (BaseComponent selComponent in selComponents)
                                {
                                    SchemeView.Components.Remove(selComponent.ID);
                                    SchemeView.SchemeDoc.OnItemChanged(SchemeChangeTypes.ComponentDeleted, selComponent);
                                }

                                History.EndPoint();
                                selComponents.Clear();
                            }
                        }
                    }

                    OnSelectionChanged();
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при удалении выбранных компонентов схемы" :
                    "Error deleting selected scheme components");
            }
        }

        /// <summary>
        /// Переместить и изменить размер выбранных компонентов схемы
        /// </summary>
        public void MoveResizeSelected(int dx, int dy, int w, int h, bool writeChanges)
        {
            try
            {
                bool moved = dx != 0 || dy != 0;
                bool resized = w > 0 && h > 0;

                if ((moved || resized) && SchemeView != null)
                {
                    // удаление выбранных компонентов
                    lock (SchemeView.SyncRoot)
                    {
                        lock (selComponents)
                        {
                            History.BeginPoint();

                            foreach (BaseComponent selComponent in selComponents)
                            {
                                if (moved)
                                    selComponent.Location = 
                                        new Point(selComponent.Location.X + dx, selComponent.Location.Y + dy);

                                if (resized)
                                    selComponent.Size = new Size(w, h);

                                if (writeChanges)
                                {
                                    selComponent.OnItemChanged(SchemeChangeTypes.ComponentChanged, selComponent);
                                }
                                else
                                {
                                    History.Add(SchemeChangeTypes.ComponentChanged, selComponent);
                                    Modified = true;
                                }
                            }

                            History.EndPoint();
                            OnSelectionPropsChanged();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при перемещении и изменении размера выбранных компонентов схемы" :
                    "Error moving and resizing selected scheme components");
            }
        }

        /// <summary>
        /// Выбрать компонент схемы
        /// </summary>
        public void SelectComponent(int componentID, bool append = false)
        {
            try
            {
                if (SchemeView != null)
                {
                    BaseComponent component;
                    lock (SchemeView.SyncRoot)
                    {
                        SchemeView.Components.TryGetValue(componentID, out component);
                    }

                    if (component != null)
                    {
                        lock (selComponents)
                        {
                            if (append)
                            {
                                if (!selComponents.Contains(component))
                                    selComponents.Add(component);
                            }
                            else
                            {
                                selComponents.Clear();
                                selComponents.Add(component);
                            }
                        }

                        OnSelectionChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при выборе компонента схемы" :
                    "Error selecting scheme component");
            }
        }

        /// <summary>
        /// Отменить выбор компонента схемы
        /// </summary>
        public void DeselectComponent(int componentID)
        {
            try
            {
                if (SchemeView != null)
                {
                    lock (selComponents)
                    {
                        for (int i = 0, cnt = selComponents.Count; i < cnt; i++)
                        {
                            if (selComponents[i].ID == componentID)
                            {
                                selComponents.RemoveAt(i);
                                OnSelectionChanged();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при отмене выбора компонента схемы" :
                    "Error deselecting scheme component");
            }
        }

        /// <summary>
        /// Отменить выбор всех компонентов схемы
        /// </summary>
        public void DeselectAll()
        {
            try
            {
                if (SchemeView != null)
                {
                    ClearSelComponents();
                    OnSelectionChanged();
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при отмене выбора всех компонентов схемы" :
                    "Error deselecting all scheme components");
            }
        }
        
        /// <summary>
        /// Выполнить действие по выбору компонента
        /// </summary>
        public void PerformSelectAction(SelectAction selectAction, int componentID)
        {
            switch (selectAction)
            {
                case SelectAction.Select:
                    SelectComponent(componentID);
                    break;
                case SelectAction.Append:
                    SelectComponent(componentID, true);
                    break;
                case SelectAction.Deselect:
                    DeselectComponent(componentID);
                    break;
                case SelectAction.DeselectAll:
                    DeselectAll();
                    break;
            }
        }

        /// <summary>
        /// Копировать выбранные компоненты схемы в буфер обмена
        /// </summary>
        public void CopyToClipboard()
        {
            try
            {
                lock (clipboard)
                {
                    clipboard.Clear();

                    // копирование в буфер обмена
                    int minX = int.MaxValue;
                    int minY = int.MaxValue;

                    lock (selComponents)
                    {
                        foreach (BaseComponent selComponent in selComponents)
                        {
                            if (minX > selComponent.Location.X)
                                minX = selComponent.Location.X;

                            if (minY > selComponent.Location.Y)
                                minY = selComponent.Location.Y;

                            clipboard.Add(selComponent.Clone());
                        }
                    }

                    // нормализация положения скопированных компонентов
                    foreach (BaseComponent component in clipboard)
                    {
                        component.Location = new Point(component.Location.X - minX, component.Location.Y - minY);
                    }
                }

                OnClipboardChanged();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при копировании выбранных компонентов схемы в буфер обмена" :
                    "Error copying selected scheme components to clipboard");
            }
        }

        /// <summary>
        /// Вставить компоненты схемы из буфера обмена
        /// </summary>
        public bool PasteFromClipboard(int x, int y, bool pasteSpecial)
        {
            try
            {
                if (SchemeView == null)
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Схема не загружена." :
                        "Scheme is not loaded.");
                }

                lock (SchemeView.SyncRoot)
                {
                    lock (clipboard)
                    {
                        lock (selComponents)
                        {
                            selComponents.Clear();
                            History.BeginPoint();

                            int inCnlOffset = pasteSpecial ? PasteSpecialParams.InCnlOffset : 0;
                            int ctrlCnlOffset = pasteSpecial ? PasteSpecialParams.CtrlCnlOffset : 0;

                            foreach (BaseComponent srcComponent in clipboard)
                            {
                                BaseComponent newComponent = srcComponent.Clone();
                                newComponent.ID = SchemeView.GetNextComponentID();
                                newComponent.Location = new Point(newComponent.Location.X + x, newComponent.Location.Y + y);

                                if (pasteSpecial && newComponent is IDynamicComponent dynamicComponent)
                                {
                                    if (dynamicComponent.InCnlNum > 0)
                                        dynamicComponent.InCnlNum += inCnlOffset;
                                    if (dynamicComponent.CtrlCnlNum > 0)
                                        dynamicComponent.CtrlCnlNum += ctrlCnlOffset;
                                }

                                SchemeView.Components[newComponent.ID] = newComponent;
                                SchemeView.SchemeDoc.OnItemChanged(SchemeChangeTypes.ComponentAdded, newComponent);
                                selComponents.Add(newComponent);
                            }

                            History.EndPoint();
                        }
                    }
                }

                OnSelectionChanged();
                PointerMode = PointerMode.Select;
                return true;
            }
            catch (ScadaException ex)
            {
                log.WriteError(string.Format(Localization.UseRussian ?
                    "Ошибка при вставке компонентов схемы из буфера обмена: {0}" :
                    "Error pasting scheme components from clipboard: {0}", ex.Message));
                return false;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при вставке компонентов схемы из буфера обмена" :
                    "Error pasting scheme components from clipboard");
                return false;
            }
        }

        /// <summary>
        /// Отменить последнее действие
        /// </summary>
        public void Undo()
        {
            try
            {
                if (SchemeView != null)
                    ApplyChanges(History.GetUndoChanges());
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при отмене последнего действия" :
                    "Error undoing the last action");
            }
        }

        /// <summary>
        /// Вернуть последнее действие
        /// </summary>
        public void Redo()
        {
            try
            {
                if (SchemeView != null)
                    ApplyChanges(History.GetRedoChanges());
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при возврате последнего действия" :
                    "Error redoing the last action");
            }
        }


        /// <summary>
        /// Событие возникающее при изменении схемы
        /// </summary>
        public event EventHandler ModifiedChanged;

        /// <summary>
        /// Событие возникающее при изменении режима указателя мыши редактора
        /// </summary>
        public event EventHandler PointerModeChanged;

        /// <summary>
        /// Событие возникающее при изменении статуса редактора
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// Событие возникающее при изменении набора выбранных компонентов схемы
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Событие возникающее при изменении свойств выбранных компонентов схемы
        /// </summary>
        public event EventHandler SelectionPropsChanged;

        /// <summary>
        /// Событие возникающее при изменении содержимого буфера обмена
        /// </summary>
        public event EventHandler ClipboardChanged;
    }
}
