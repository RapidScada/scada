/*
 * Copyright 2016 Mikhail Shiryaev
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
 * Module   : ScadaWebCommon
 * Summary  : View settings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2016
 */

namespace Scada.Web
{
    /// <summary>
    /// View settings
    /// <para>Настройки представлений</para>
    /// </summary>
    public class ViewSettings : ISettings
    {
        /// <summary>
        /// Имя файла настроек по умолчанию
        /// </summary>
        public const string DefFileName = "ViewSettings.xml";

        
        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewSettings()
        {
        }


        /// <summary>
        /// Создать новый объект настроек
        /// </summary>
        public ISettings Create()
        {
            return new ViewSettings();
        }

        /// <summary>
        /// Определить, равны ли заданные настройки текущим настройкам
        /// </summary>
        public bool Equals(ISettings settings)
        {
            return settings == this;
        }

        /// <summary>
        /// Загрузить настройки из файла
        /// </summary>
        public bool LoadFromFile(string fileName, out string errMsg)
        {
            errMsg = "ViewSettings not implemented";
            return false;
        }

        /// <summary>
        /// Сохранить настройки в файле
        /// </summary>
        public bool SaveToFile(string fileName, out string errMsg)
        {
            errMsg = "ViewSettings not implemented";
            return false;
        }
    }
}
