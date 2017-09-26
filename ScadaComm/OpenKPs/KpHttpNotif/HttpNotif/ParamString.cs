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
 * Module   : KpHttpNotif
 * Summary  : Parameterized string
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using System.Collections.Generic;

namespace Scada.Comm.Devices.HttpNotif
{
    /// <summary>
    /// Parameterized string
    /// <para>Параметризованная строка</para>
    /// </summary>
    internal class ParamString
    {
        /// <summary>
        /// Параметр строки
        /// </summary>
        public class Param
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Param()
                : this("")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public Param(string name)
            {
                Name = name;
                PartIndexes = new List<int>();
            }

            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить индексы параметра среди частей строки
            /// </summary>
            public List<int> PartIndexes { get; protected set; }
        }


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected ParamString()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParamString(string srcString)
        {
            SrcString = srcString;
            Parse();
        }


        /// <summary>
        /// Получить исходную строку
        /// </summary>
        public string SrcString { get; protected set; }

        /// <summary>
        /// Получить части строки
        /// </summary>
        public string[] StringParts { get; protected set; }

        /// <summary>
        /// Получить словарь параметров, ключ - имя параметра
        /// </summary>
        public Dictionary<string, Param> Params { get; protected set; }


        /// <summary>
        /// Выполнить разбор строки
        /// </summary>
        protected void Parse()
        {
            List<string> stringParts = new List<string>();
            Dictionary<string, Param> stringParams = new Dictionary<string, Param>();

            // разбиение строки на части, разделяемые скобками { и }
            if (!string.IsNullOrEmpty(SrcString))
            {
                int ind = 0;
                int len = SrcString.Length;

                while (ind < len)
                {
                    int braceInd1 = SrcString.IndexOf('{', ind);
                    if (braceInd1 < 0)
                    {
                        stringParts.Add(SrcString.Substring(ind));
                        break;
                    }
                    else
                    {
                        int braceInd2 = SrcString.IndexOf('}', braceInd1 + 1);
                        int paramNameLen = braceInd2 - braceInd1 - 1;

                        if (paramNameLen <= 0)
                        {
                            stringParts.Add(SrcString.Substring(ind));
                            break;
                        }
                        else
                        {
                            string paramName = SrcString.Substring(braceInd1 + 1, paramNameLen);
                            Param param;

                            if (!stringParams.TryGetValue(paramName, out param))
                            {
                                param = new Param(paramName);
                                stringParams.Add(paramName, param);
                            }

                            stringParts.Add(SrcString.Substring(ind, braceInd1 - ind));
                            param.PartIndexes.Add(stringParts.Count);
                            stringParts.Add("");
                            ind = braceInd2 + 1;
                        }
                    }
                }
            }

            StringParts = stringParts.ToArray();
            Params = stringParams;
        }

        /// <summary>
        /// Установить значение параметра
        /// </summary>
        public void SetParam(string paramName, string paramVal)
        {
            Param param;
            if (Params.TryGetValue(paramName, out param))
            {
                int partsLen = StringParts.Length;

                foreach (int index in param.PartIndexes)
                {
                    if (0 <= index && index < partsLen)
                        StringParts[index] = paramVal;
                }
            }
        }

        /// <summary>
        /// Получить строковое представление объекта
        /// </summary>
        public override string ToString()
        {
            return string.Join("", StringParts);
        }
    }
}
