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
 * Module   : ScadaCommCommon
 * Summary  : The base class for device communication logic. Nested types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2019
 */

using Scada.Data.Tables;
using System;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    partial class KPLogic
    {
        /// <summary>
        /// Статистика работы КП
        /// </summary>
        public struct KPStats
        {
            /// <summary>
            /// Получить или установить количество сеансов опроса
            /// </summary>
            public int SessCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных сеансов опроса
            /// </summary>
            public int SessErrCnt { get; set; }
            /// <summary>
            /// Получить или установить количество команд ТУ
            /// </summary>
            public int CmdCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных команд ТУ
            /// </summary>
            public int CmdErrCnt { get; set; }
            /// <summary>
            /// Получить или установить количество запросов
            /// </summary>
            public int ReqCnt { get; set; }
            /// <summary>
            /// Получить или установить количество неудачных запросов
            /// </summary>
            public int ReqErrCnt { get; set; }

            /// <summary>
            /// Сбросить статистику
            /// </summary>
            public void Reset()
            {
                SessCnt = 0;
                SessErrCnt = 0;
                CmdCnt = 0;
                CmdErrCnt = 0;
                ReqCnt = 0;
                ReqErrCnt = 0;
            }
        }

        /// <summary>
        /// Тег КП
        /// </summary>
        public class KPTag
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPTag()
                : this(0, "")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPTag(int signal, string name)
            {
                Signal = signal;
                Index = -1;
                Name = name;
                CnlNum = 0;
                ObjNum = 0;
                ParamID = 0;
                Aux = null;
            }

            /// <summary>
            /// Получить или установить сигнал (номер тега)
            /// </summary>
            public int Signal { get; set; }
            /// <summary>
            /// Gets or sets the tag index.
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// Получить или установить наименование
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Получить или установить номер входного канала базы конфигурации, привязанного к тегу
            /// </summary>
            public int CnlNum { get; set; }
            /// <summary>
            /// Получить или установить номер объекта входного канала
            /// </summary>
            public int ObjNum { get; set; }
            /// <summary>
            /// Получить или установить идентификатор параметра входного канала
            /// </summary>
            /// <remarks>Необходим для событий КП</remarks>
            public int ParamID { get; set; }
            /// <summary>
            /// Gets or sets the auxiliary object that contains data about the tag.
            /// </summary>
            public object Aux { get; set; }
        }

        /// <summary>
        /// Группа тегов КП
        /// </summary>
        public class TagGroup
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public TagGroup()
                : this("")
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public TagGroup(string name)
            {
                Name = name;
                KPTags = new List<KPTag>();
            }

            /// <summary>
            /// Получить наименование группы
            /// </summary>
            public string Name { get; protected set; }
            /// <summary>
            /// Получить список тегов КП, входящих в группу
            /// </summary>
            public List<KPTag> KPTags { get; protected set; }

            /// <summary>
            /// Creates and adds a new tag to the group.
            /// </summary>
            public KPTag AddNewTag(int signal, string name, object aux = null)
            {
                KPTag kpTag = new KPTag(signal, name) { Aux = aux };
                KPTags.Add(kpTag);
                return kpTag;
            }
        }

        /// <summary>
        /// Срез данных тегов за определённый момент времени
        /// </summary>
        /// <remarks>Особенность среза тегов заключается в использовании тегов КП вместо входных каналов</remarks>
        public class TagSrez
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            protected TagSrez()
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public TagSrez(int tagCnt)
            {
                if (tagCnt <= 0)
                    throw new ArgumentException("Tag count must be positive.", "tagCnt");

                DateTime = DateTime.MinValue;
                KPTags = new KPTag[tagCnt];
                TagData = new SrezTableLight.CnlData[tagCnt];
                Descr = "";
            }

            /// <summary>
            /// Получить или установить временную метку
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// Получить ссылки на теги КП, входящие в срез
            /// </summary>
            public KPTag[] KPTags { get; protected set; }
            /// <summary>
            /// Получить данные тегов
            /// </summary>
            public SrezTableLight.CnlData[] TagData { get; protected set; }
            /// <summary>
            /// Получить или установить описание среза для вывода в журнал
            /// </summary>
            public string Descr { get; set; }

            /// <summary>
            /// Получить массив индексов тегов среза, привязанных к входным каналам
            /// </summary>
            public List<int> GetBoundTagIndexes()
            {
                List<int> indexes = new List<int>();
                int len = KPTags.Length;
                for (int i = 0; i < len; i++)
                {
                    KPTag kpTag = KPTags[i];
                    if (kpTag != null && kpTag.CnlNum > 0)
                        indexes.Add(i);
                }
                return indexes;
            }
            /// <summary>
            /// Установить данные тега среза
            /// </summary>
            public void SetTagData(int tagIndex, double newVal, int newStat)
            {
                SetTagData(tagIndex, new SrezTableLight.CnlData(newVal, newStat));
            }
            /// <summary>
            /// Установить данные тега среза
            /// </summary>
            public void SetTagData(int tagIndex, SrezTableLight.CnlData newData)
            {
                if (0 <= tagIndex && tagIndex < TagData.Length)
                    TagData[tagIndex] = newData;
            }
        }

        /// <summary>
        /// Событие КП
        /// </summary>
        /// <remarks>Особенность события КП заключается в использовании тега КП вместо входного канала</remarks>
        public class KPEvent
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPEvent()
                : this(DateTime.MinValue, 0, null)
            {
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public KPEvent(DateTime dateTime, int kpNum, KPTag kpTag)
            {
                DateTime = dateTime;
                KPNum = kpNum;
                KPTag = kpTag;
                OldData = SrezTableLight.CnlData.Empty;
                NewData = SrezTableLight.CnlData.Empty;
                Checked = false;
                UserID = 0;
                Descr = "";
                Data = "";
            }

            /// <summary>
            /// Получить или установить временную метку
            /// </summary>
            public DateTime DateTime { get; set; }
            /// <summary>
            /// Получить или установить номер КП из базы конфигурации
            /// </summary>
            public int KPNum { get; set; }
            /// <summary>
            /// Получить или установить ссылку на параметр КП
            /// </summary>
            public KPTag KPTag { get; set; }
            /// <summary>
            /// Получить или установить старые данные параметра КП
            /// </summary>
            public SrezTableLight.CnlData OldData { get; set; }
            /// <summary>
            /// Получить или установить новые данные параметра КП
            /// </summary>
            public SrezTableLight.CnlData NewData { get; set; }
            /// <summary>
            /// Получить или установить признак квитирования события
            /// </summary>
            public bool Checked { get; set; }
            /// <summary>
            /// Получить или установить идентификатор пользователя из базы конфигурации, квитировавшего событие
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// Получить или установить описание события
            /// </summary>
            public string Descr { get; set; }
            /// <summary>
            /// Получить или установить дополнительные данные события
            /// </summary>
            public string Data { get; set; }
        }

        /// <summary>
        /// Состояния работы КП
        /// </summary>
        public enum WorkStates
        {
            /// <summary>
            /// Неопределено
            /// </summary>
            Undefined,
            /// <summary>
            /// Норма
            /// </summary>
            Normal,
            /// <summary>
            /// Ошибка
            /// </summary>
            Error
        }
    }
}
