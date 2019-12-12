/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : PlgSchBasicComp
 * Summary  : Factory for creating basic scheme components
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2018
 */

using Scada.Scheme;
using Scada.Scheme.Model;

namespace Scada.Web.Plugins.SchBasicComp
{
    /// <summary>
    /// Factory for creating basic scheme components.
    /// <para>Фабрика для создания основных компонентов схемы.</para>
    /// </summary>
    public class BasicCompFactory : CompFactory
    {
        /// <summary>
        /// Создать компонент схемы.
        /// </summary>
        public override BaseComponent CreateComponent(string typeName, bool nameIsShort)
        {
            if (NameEquals("Button", typeof(Button).FullName, typeName, nameIsShort))
                return new Button();
            else if (NameEquals("Led", typeof(Led).FullName, typeName, nameIsShort))
                return new Led();
            else if (NameEquals("Link", typeof(Link).FullName, typeName, nameIsShort))
                return new Link();
            else if (NameEquals("Toggle", typeof(Toggle).FullName, typeName, nameIsShort))
                return new Toggle();
            else
                return null;
        }
    }
}
