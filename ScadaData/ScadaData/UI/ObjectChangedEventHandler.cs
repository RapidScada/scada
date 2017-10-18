/*
 * Copyright 2017 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Represents a method that will handle an event raised when an object property is changed
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2017
 */

namespace Scada.UI
{
    /// <summary>
    /// Represents a method that will handle an event raised when an object property is changed
    /// <para>Представляет метод для обработки события, возникающего при изменении свойств объекта</para>
    /// </summary>
    public delegate void ObjectChangedEventHandler(
        object sender,
        ObjectChangedEventArgs e);
}
