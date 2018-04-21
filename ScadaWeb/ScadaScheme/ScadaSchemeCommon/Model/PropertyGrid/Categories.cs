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
 * Module   : ScadaSchemeCommon
 * Summary  : Categories of PropertyGrid
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

#pragma warning disable 1591 // CS1591: Missing XML comment for publicly visible type or member

namespace Scada.Scheme.Model.PropertyGrid
{
    /// <summary>
    /// Categories of PropertyGrid
    /// <para>Категории PropertyGrid</para>
    /// </summary>
    /// <remarks>
    /// Predefined categories:
    /// https://msdn.microsoft.com/en-us/library/system.componentmodel.categoryattribute(v=vs.110).aspx
    /// </remarks>
    public static class Categories
    {
        public const string Appearance = "Appearance";
        public const string Behavior = "Behavior";
        public const string Data = "Data";
        public const string Design = "Design";
        public const string Layout = "Layout";
    }
}
