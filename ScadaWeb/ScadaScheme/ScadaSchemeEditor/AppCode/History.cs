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


        private readonly Log log;   // журнал приложения
        private List<Point> points; // точки истории
        private int pointsHead;     // позиция добавления точек истории
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
            pointsHead = 0;
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
                return false;
            }
        }
        
        /// <summary>
        /// Получить признак, что возможно вернуть действие
        /// </summary>
        public bool CanRedo
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Сделать копию свойств документа и компонентов схемы
        /// </summary>
        public void MakeCopy(SchemeView schemeView)
        {

        }

        /// <summary>
        /// Начать сохранение точки истории
        /// </summary>
        public void BeginPoint()
        {

        }

        /// <summary>
        /// Завершить сохранение точки истории
        /// </summary>
        public void EndPoint()
        {

        }

        /// <summary>
        /// Отключить добавление изменений
        /// </summary>
        public void DisableAdding()
        {

        }

        /// <summary>
        /// Включить добавление изменений
        /// </summary>
        public void EnableAdding()
        {

        }

        /// <summary>
        /// Добавить изменение в историю
        /// </summary>
        public void Add(SchemeChangeTypes changeType, object changedObject, object oldKey = null)
        {

        }

        /// <summary>
        /// Получить изменения, необходимые для отмены действия
        /// </summary>
        public List<Change> GetUndoChanges()
        {
            return new List<Change>();
        }

        /// <summary>
        /// Получить изменения, необходимые для возврата действия
        /// </summary>
        public List<Change> GetRedoChanges()
        {
            return new List<Change>();
        }
    }
}
