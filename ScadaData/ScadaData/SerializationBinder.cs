/*
 * Copyright 2017 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData
 * Summary  : Control class loading during serialization
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2014
 * Modified : 2017
 */

using System;
using System.Reflection;

namespace Scada
{
    /// <summary>
    /// Control class loading during serialization
    /// <para>Контроль загружаемых классов в процессе сериализации</para>
    /// </summary>
    /// <remarks>Класс необходим из-за особенностей работы .NET,
    /// объект должен создаваться в той сборке, в которой используется</remarks>
    public class SerializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        /// <summary>
        /// Сборка, в которой производится поиск типов
        /// </summary>
        protected Assembly assembly;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SerializationBinder()
            : base()
        {
            assembly = null;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SerializationBinder(Assembly assembly)
            : base()
        {
            this.assembly = assembly;
        }

        /// <summary>
        /// Управляет привязкой сериализованного объекта к типу
        /// </summary>
        public override Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                Assembly asm = assembly ?? Assembly.GetExecutingAssembly();
                return asm.GetType(typeName, true, true);
            }
            catch
            {
                if (typeName.Contains("System.Collections.Generic.List"))
                {
                    // удаление информации о сборке
                    int ind1 = typeName.IndexOf(",");
                    int ind2 = typeName.IndexOf("]");
                    if (ind1 < ind2)
                        typeName = typeName.Remove(ind1, ind2 - ind1);
                }

                return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), true, true);
            }
        }
    }
}
