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
 * Summary  : History of scheme changes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Scheme.Model;
using System;
using System.Collections.Generic;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// History of scheme changes
    /// <para>История изменений схемы</para>
    /// </summary>
    internal class History
    {
        /// <summary>
        /// Точка истории
        /// </summary>
        private class Point
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Point()
            {
                Changes = new List<Change>();
            }

            /// <summary>
            /// Получить изменения
            /// </summary>
            public List<Change> Changes { get; private set; }
        }


        /// <summary>
        /// Размер хранимой истории
        /// </summary>
        private const int HistorySize = 20;

        private readonly Log log;   // журнал приложения
        private List<Point> points; // точки истории
        private int headIndex;      // индекс добавления точек истории
        private Point currentPoint; // текущая сохраняемая точка истории
        private bool addingEnabled; // добавление изменений разрешено
        private SchemeDocument schemeDocCopy; // копия свойств документа схемы
        private SortedList<int, BaseComponent> componentsCopy; // копия компонентов схемы


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private History()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public History(Log log)
        {
            this.log = log;
            points = new List<Point>();
            headIndex = 0;
            currentPoint = null;
            addingEnabled = true;
            schemeDocCopy = new SchemeDocument();
            componentsCopy = new SortedList<int, BaseComponent>();
        }


        /// <summary>
        /// Получить признак, что возможно отменить действие
        /// </summary>
        public bool CanUndo
        {
            get
            {
                lock (this)
                {
                    return headIndex > 0;
                }
            }
        }
        
        /// <summary>
        /// Получить признак, что возможно вернуть действие
        /// </summary>
        public bool CanRedo
        {
            get
            {
                lock (this)
                {
                    return headIndex < points.Count;
                }
            }
        }


        /// <summary>
        /// Добавить точку истории
        /// </summary>
        private void AddPoint(Point point)
        {
            // удаление точек истории после индекса добавления
            if (headIndex < points.Count)
                points.RemoveRange(headIndex, points.Count - headIndex);

            // добавление точки истории
            points.Add(point);

            // удаление части точек истории, если превышена длина истории
            int removeCnt = points.Count - HistorySize;
            if (removeCnt > 0)
                points.RemoveRange(0, removeCnt);

            // смещение индекса добавления
            headIndex = points.Count;

            // вызов события
            OnHistoryChanged();
        }

        /// <summary>
        /// Заменить действие на противоположное
        /// </summary>
        private Change ReverseChange(Change srcChange)
        {
            switch (srcChange.ChangeType)
            {
                case SchemeChangeTypes.SchemeDocChanged:
                    return new Change(SchemeChangeTypes.SchemeDocChanged)
                    {
                        ChangedObject = srcChange.OldObject
                    };

                case SchemeChangeTypes.ComponentAdded:
                    return new Change(SchemeChangeTypes.ComponentDeleted)
                    {
                        ChangedObject = srcChange.ChangedObject,
                        ComponentID = srcChange.ComponentID
                    };

                case SchemeChangeTypes.ComponentChanged:
                    return new Change(SchemeChangeTypes.ComponentChanged)
                    {
                        ChangedObject = srcChange.OldObject,
                        ComponentID = srcChange.ComponentID
                    };

                case SchemeChangeTypes.ComponentDeleted:
                    return new Change(SchemeChangeTypes.ComponentAdded)
                    {
                        ChangedObject = srcChange.ChangedObject,
                        ComponentID = srcChange.ComponentID
                    };

                case SchemeChangeTypes.ImageAdded:
                    return new Change(SchemeChangeTypes.ImageDeleted)
                    {
                        ChangedObject = srcChange.ChangedObject,
                        ImageName = srcChange.ImageName
                    };

                case SchemeChangeTypes.ImageRenamed:
                    return new Change(SchemeChangeTypes.ImageRenamed)
                    {
                        ChangedObject = srcChange.OldObject,
                        ImageName = srcChange.OldImageName,
                        OldImageName = srcChange.ImageName
                    };

                case SchemeChangeTypes.ImageDeleted:
                    return new Change(SchemeChangeTypes.ImageAdded)
                    {
                        ChangedObject = srcChange.ChangedObject,
                        ImageName = srcChange.ImageName
                    };

                default:
                    throw new ScadaException("Unknown type of scheme changes.");
            }
        }

        /// <summary>
        /// Вызвать событие HistoryChanged
        /// </summary>
        private void OnHistoryChanged()
        {
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Очистить историю
        /// </summary>
        public void Clear()
        {
            points.Clear();
            headIndex = 0;
            currentPoint = null;
            addingEnabled = true;
            schemeDocCopy = new SchemeDocument();
            componentsCopy.Clear();
            OnHistoryChanged();
        }

        /// <summary>
        /// Создать копию свойств документа и компонентов схемы
        /// </summary>
        public void MakeCopy(SchemeView schemeView)
        {
            try
            {
                lock (this)
                {
                    Clear();
                    schemeView.SchemeDoc.CopyTo(schemeDocCopy);

                    foreach (BaseComponent component in schemeView.Components.Values)
                    {
                        BaseComponent componentCopy = component.Clone();
                        componentsCopy.Add(componentCopy.ID, componentCopy);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при создании копии свойств документа и компонентов схемы" :
                    "Error making copy of the scheme document properties and components");
            }
        }

        /// <summary>
        /// Начать заполнение точки истории
        /// </summary>
        public void BeginPoint()
        {
            try
            {
                lock (this)
                {
                    if (currentPoint == null)
                        currentPoint = new Point();
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка в начале заполнения точки истории" :
                    "Error beginning of history point");
            }
        }

        /// <summary>
        /// Завершить заполнение точки истории
        /// </summary>
        public void EndPoint()
        {
            try
            {
                lock (this)
                {
                    if (currentPoint != null)
                    {
                        AddPoint(currentPoint);
                        currentPoint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при завершении заполнения точки истории" :
                    "Error ending of history point");
            }
        }

        /// <summary>
        /// Отключить добавление изменений
        /// </summary>
        public void DisableAdding()
        {
            addingEnabled = false;
        }

        /// <summary>
        /// Включить добавление изменений
        /// </summary>
        public void EnableAdding()
        {
            addingEnabled = true;
        }

        /// <summary>
        /// Добавить изменение в историю
        /// </summary>
        public void Add(SchemeChangeTypes changeType, object changedObject, object oldKey = null)
        {
            try
            {
                lock (this)
                {
                    Change change = new Change(changeType, changedObject, oldKey);

                    switch (changeType)
                    {
                        case SchemeChangeTypes.SchemeDocChanged:
                            change.OldObject = schemeDocCopy;
                            schemeDocCopy = (SchemeDocument)change.ChangedObject;
                            break;

                        case SchemeChangeTypes.ComponentAdded:
                            componentsCopy[change.ComponentID] = (BaseComponent)change.ChangedObject;
                            break;

                        case SchemeChangeTypes.ComponentChanged:
                            change.OldObject = componentsCopy[change.ComponentID];
                            componentsCopy[change.ComponentID] = (BaseComponent)change.ChangedObject;
                            break;

                        case SchemeChangeTypes.ComponentDeleted:
                            componentsCopy.Remove(change.ComponentID);
                            break;
                    }

                    if (addingEnabled)
                    {
                        if (currentPoint == null)
                        {
                            Point point = new Point();
                            point.Changes.Add(change);
                            AddPoint(point);
                        }
                        else
                        {
                            currentPoint.Changes.Add(change);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при добавлении изменения в историю" :
                    "Error adding a change to the history");
            }
        }

        /// <summary>
        /// Получить изменения для отмены действия
        /// </summary>
        public List<Change> GetUndoChanges()
        {
            List<Change> changes = new List<Change>();

            try
            {
                lock (this)
                {
                    if (headIndex > 0)
                    {
                        headIndex--;
                        Point point = points[headIndex];

                        foreach (Change change in point.Changes)
                        {
                            changes.Add(ReverseChange(change));
                        }

                        OnHistoryChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изменений для отмены действия" :
                    "Error getting changes for undoing action");
            }

            return changes;
        }

        /// <summary>
        /// Получить изменения для возврата действия
        /// </summary>
        public List<Change> GetRedoChanges()
        {
            try
            {
                lock (this)
                {
                    if (headIndex < points.Count)
                    {
                        Point point = points[headIndex];
                        headIndex++;
                        OnHistoryChanged();
                        return point.Changes;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении изменений для возврата действия" :
                    "Error getting changes for redoing action");
            }

            return new List<Change>();
        }


        /// <summary>
        /// Событие возникающее при изменении истории действий
        /// </summary>
        public event EventHandler HistoryChanged;
    }
}
