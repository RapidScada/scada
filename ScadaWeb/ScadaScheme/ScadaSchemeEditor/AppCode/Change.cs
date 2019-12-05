/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : Single scheme change
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.Scheme.DataTransfer;
using Scada.Scheme.Model;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Single scheme change.
    /// <para>Одно изменение схемы.</para>
    /// </summary>
    internal class Change
    {
        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров.
        /// </summary>
        protected Change()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Change(SchemeChangeTypes changeType)
        {
            ChangeType = changeType;
            Stamp = 0;
            ChangedObject = null;
            OldObject = null;
            ComponentID = -1;
            ImageName = "";
            OldImageName = "";
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Change(SchemeChangeTypes changeType, object changedObject, object oldKey) 
            : this(changeType)
        {
            switch (changeType)
            {
                case SchemeChangeTypes.SchemeDocChanged:
                    if (changedObject is SchemeDocument schemeDoc)
                        ChangedObject = schemeDoc.Copy();
                    else
                        throw new ScadaException("SchemeDocument expected.");
                    break;

                case SchemeChangeTypes.ComponentAdded:
                case SchemeChangeTypes.ComponentChanged:
                case SchemeChangeTypes.ComponentDeleted:
                    if (changedObject is BaseComponent component)
                    {
                        ChangedObject = component.Clone();
                        ComponentID = component.ID;
                    }
                    else
                    {
                        throw new ScadaException("BaseComponent expected.");
                    }
                    break;

                case SchemeChangeTypes.ImageAdded:
                case SchemeChangeTypes.ImageRenamed:
                case SchemeChangeTypes.ImageDeleted:
                    if (changedObject is Image image)
                    {
                        ChangedObject = image.Copy();
                        ImageName = image.Name;
                        OldImageName = (oldKey as string) ?? "";
                    }
                    else
                    {
                        throw new ScadaException("Image expected.");
                    }
                    break;
            }
        }


        /// <summary>
        /// Получить тип изменения схемы.
        /// </summary>
        public SchemeChangeTypes ChangeType { get; private set; }

        /// <summary>
        /// Получить или установить уникальную метку изменения в пределах открытой схемы.
        /// </summary>
        /// <remarks>Каждая следующая метка больше, чем предыдущая.</remarks>
        public long Stamp { get; set; }

        /// <summary>
        /// Получить или установить добавленный, изменившийся или удалённый объект.
        /// </summary>
        public object ChangedObject { get; set; }

        /// <summary>
        /// Получить или установить копию изменившегося объекта в предыдущем состоянии.
        /// </summary>
        public object OldObject { get; set; }

        /// <summary>
        /// Получить или установить ид. компонента.
        /// </summary>
        public int ComponentID { get; set; }

        /// <summary>
        /// Получить или установить наименование изображения.
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Получить или установить старое наименование изображения в случае переименования.
        /// </summary>
        public string OldImageName { get; set; }


        /// <summary>
        /// Преобразовать изменение для передачи WCF-сервисом.
        /// </summary>
        public Change ConvertToDTO()
        {
            Change changeDTO = new Change(ChangeType)
            {
                Stamp = Stamp,
                ComponentID = ComponentID,
                ImageName = ImageName,
                OldImageName = OldImageName
            };

            switch (ChangeType)
            {
                case SchemeChangeTypes.SchemeDocChanged:
                case SchemeChangeTypes.ComponentAdded:
                case SchemeChangeTypes.ComponentChanged:
                    changeDTO.ChangedObject = ChangedObject;
                    break;

                case SchemeChangeTypes.ImageAdded:
                    changeDTO.ChangedObject = new ImageDTO((Image)ChangedObject);
                    break;
            }

            return changeDTO;
        }
    }
}
