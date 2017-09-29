/*
 * Copyright 2017 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Provides data for events caused by an object change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2017
 */

using System;

namespace Scada.UI
{
    /// <summary>
    /// Provides data for events caused by an object change
    /// <para>Предоставляет данные для событий, вызванных изменением объекта</para>
    /// </summary>
    public class ObjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ObjectChangedEventArgs(object changedObject)
            : this(changedObject, null)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ObjectChangedEventArgs(object changedObject, object changeArgument)
        {
            ChangedObject = changedObject;
            ChangeArgument = changeArgument;
        }


        /// <summary>
        /// Получить изменённый объект
        /// </summary>
        public object ChangedObject { get; protected set; }

        /// <summary>
        /// Получить аргумент, описывающий изменения
        /// </summary>
        public object ChangeArgument { get; protected set; }
    }
}
