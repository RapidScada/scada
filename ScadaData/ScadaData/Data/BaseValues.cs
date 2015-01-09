/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : The main values from the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Data
{
    /// <summary>
    /// The main values from the configuration database
    /// <para>Основные значения из базы конфигурации</para>
    /// </summary>
    public class BaseValues
    {
        /// <summary>
        /// Роли пользователей
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// Отключен
            /// </summary>
            public const int Disabled = 0x00;
            /// <summary>
            /// Администратор
            /// </summary>
            public const int Admin = 0x01;
            /// <summary>
            /// Диспетчер
            /// </summary>
            public const int Dispatcher = 0x02;
            /// <summary>
            /// Гость
            /// </summary>
            public const int Guest = 0x03;
            /// <summary>
            /// Приложение
            /// </summary>
            public const int App = 0x04;
            /// <summary>
            /// Настраиваемая роль
            /// </summary>
            /// <remarks>Минимальный идентификатор настраиваемой роли равен 0x0B</remarks>
            public const int Custom = 0x0B;
            /// <summary>
            /// Ошибка (неверное имя пользователя или пароль)
            /// </summary>
            public const int Err = 0xFF;

            /// <summary>
            /// Получить имя роли по идентификатору
            /// </summary>
            public static string GetRoleName(int roleID)
            {
                if (Localization.UseRussian)
                {
                    switch (roleID)
                    {
                        case Disabled:
                            return "Отключен";
                        case Admin:
                            return "Администратор";
                        case Dispatcher:
                            return "Диспетчер";
                        case Guest:
                            return "Гость";
                        case App:
                            return "Приложение";
                        case Custom:
                            return "Настраиваемая роль";
                        default:
                            return "Ошибка";
                    }
                }
                else
                {
                    switch (roleID)
                    {
                        case Disabled:
                            return "Disabled";
                        case Admin:
                            return "Administrator";
                        case Dispatcher:
                            return "Dispatcher";
                        case Guest:
                            return "Guest";
                        case App:
                            return "Application";
                        case Custom:
                            return "Custom role";
                        default:
                            return "Error";
                    }
                }
            }
        }

        /// <summary>
        /// Типы каналов
        /// </summary>
        public static class CnlTypes
        {
            /// <summary>
            /// Телесигнал (ТС)
            /// </summary>
            public const int TS = 1;
            /// <summary>
            /// Телеизмерение (ТИ)
            /// </summary>
            public const int TI = 2;
            /// <summary>
            /// Дорасчётное ТИ
            /// </summary>
            public const int TIDR = 3;
            /// <summary>
            /// Минутное ТИ
            /// </summary>
            public const int TIDRM = 4;
            /// <summary>
            /// Часовое ТИ
            /// </summary>
            public const int TIDRH = 5;
            /// <summary>
            /// Количество переключений (дорасчётный)
            /// </summary>
            public const int SWCNT = 6;
            /// <summary>
            /// Дорасчётный ТС
            /// </summary>
            public const int TSDR = 7;
            /// <summary>
            /// Минутный ТС
            /// </summary>
            public const int TSDRM = 8;
            /// <summary>
            /// Часовой ТС
            /// </summary>
            public const int TSDRH = 9;

            /// <summary>
            /// Мин. идентификатор типа канала
            /// </summary>
            public const int MinCnlTypeID = 1;
            /// <summary>
            /// Макс. идентификатор типа канала
            /// </summary>
            public const int MaxCnlTypeID = 9;
        }

        /// <summary>
        /// Типы команд
        /// </summary>
        public static class CmdTypes
        {
            /// <summary>
            /// Стандартная команда (ТУ)
            /// </summary>
            public const int Standard = 0;
            /// <summary>
            /// Бинарная команда
            /// </summary>
            public const int Binary = 1;
            /// <summary>
            /// Внеочередной опрос КП
            /// </summary>
            public const int Request = 2;
        }

        /// <summary>
        /// Статусы параметров (типы событий)
        /// </summary>
        public static class ParamStat
        {
            /// <summary>
            /// Не определён
            /// </summary>
            public const int Undefined = 0;
            /// <summary>
            /// Определён
            /// </summary>
            public const int Defined = 1;
            /// <summary>
            /// Архивный
            /// </summary>
            public const int Archival = 2;
            /// <summary>
            /// Ошибка в формуле
            /// </summary>
            public const int FormulaError = 3;
            /// <summary>
            /// Изменён
            /// </summary>
            public const int Changed = 4;
            /// <summary>
            /// Недостоверен
            /// </summary>
            public const int Unreliable = 5;

            /// <summary>
            /// Аварийное занижение
            /// </summary>
            public const int LowCrash = 11;
            /// <summary>
            /// Занижение
            /// </summary>
            public const int Low = 12;
            /// <summary>
            /// Нормализация
            /// </summary>
            public const int Normal = 13;
            /// <summary>
            /// Завышение
            /// </summary>
            public const int High = 14;
            /// <summary>
            /// Аварийное завышение
            /// </summary>
            public const int HighCrash = 15;

            /// <summary>
            /// Вход разрешён
            /// </summary>
            public const int InPermitted = 101;
            /// <summary>
            /// Выход разрешён
            /// </summary>
            public const int OutPermitted = 102;
            /// <summary>
            /// Доступ запрещён
            /// </summary>
            public const int AccessDenied = 103;
            /// <summary>
            /// Повреждение ШС
            /// </summary>
            public const int WireBreak = 111;
            /// <summary>
            /// Снят с охраны
            /// </summary>
            public const int Disarm = 112;
            /// <summary>
            /// Поставлен на охрану
            /// </summary>
            public const int Arm = 113;
            /// <summary>
            /// Тревога
            /// </summary>
            public const int Alarm = 114;
        }
    }
}
