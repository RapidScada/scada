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
        /// Функция извлечения сборок по имени
        /// </summary>
        protected Func<AssemblyName, Assembly> assemblyResolver;
        /// <summary>
        /// Функция извлечения типов по имени
        /// </summary>
        protected Func<Assembly, string, bool, Type> typeResolver;


        /// <summary>
        /// Конструктор
        /// </summary>
        public SerializationBinder()
            : base()
        {
            assembly = Assembly.GetExecutingAssembly();
            InitResolvers();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SerializationBinder(Assembly assembly)
            : base()
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            this.assembly = assembly;
            InitResolvers();
        }

        /// <summary>
        /// Инициализировать функции извлечения сборок и типов
        /// </summary>
        protected void InitResolvers()
        {
            assemblyResolver = (AssemblyName asmName) =>
            {
                return string.Equals(asmName.FullName, assembly.FullName, StringComparison.Ordinal) ?
                    assembly :
                    Assembly.Load(asmName);
            };

            typeResolver = (Assembly asm, string typeName, bool ignoreCase) =>
            {
                return asm.GetType(typeName, false, ignoreCase);
            };
        }


        /// <summary>
        /// Управляет привязкой сериализованного объекта к типу
        /// </summary>
        public override Type BindToType(string assemblyName, string typeName)
        {
            return string.Equals(assemblyName, assembly.FullName, StringComparison.Ordinal) ?
                assembly.GetType(typeName, true, false) :
                Type.GetType(string.Format("{0}, {1}", typeName, assemblyName), 
                    assemblyResolver, typeResolver, true, false);
        }
    }
}
