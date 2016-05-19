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
 * Summary  : WCF service that provides the client API
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Client;
using Scada.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Web
{
    /// <summary>
    /// WCF service that provides the client API
    /// <para>WCF-сервис, обеспечивающий работу клиентского API</para>
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ClientApiSvc
    {
        /// <summary>
        /// Расширенные данные входого канала
        /// </summary>
        private class CnlDataExt
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CnlDataExt(int cnlNum)
            {
                CnlNum = cnlNum;
                Val = 0.0;
                Stat = 0;
                Text = "";
                TextWithUnit = "";
                Color = "";
            }

            /// <summary>
            /// Получить номер входного канала
            /// </summary>
            public int CnlNum { get; private set; }
            /// <summary>
            /// Получить или установить значение
            /// </summary>
            public double Val { get; set; }
            /// <summary>
            /// Получить или установить статус
            /// </summary>
            public int Stat { get; set; }
            /// <summary>
            /// Получить или установить отображаемый текст
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Получить или установить отображаемый текст с размерностью
            /// </summary>
            public string TextWithUnit { get; set; }
            /// <summary>
            /// Получить или установить цвет
            /// </summary>
            public string Color { get; set; }
        }

        /// <summary>
        /// Максимальное количество символов в строке данных формата JSON, 10 МБ
        /// </summary>
        private const int MaxJsonLen = 10485760;
        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLen };
        /// <summary>
        /// Обеспечивает форматирование данных входных каналов и событий
        /// </summary>
        private static readonly DataFormatter DataFormatter = new DataFormatter();
        /// <summary>
        /// Общие данные веб-приложения
        /// </summary>
        private static readonly AppData AppData = AppData.GetAppData();


        /// <summary>
        /// Получить расширенные текущие данные входных каналов
        /// </summary>
        private CnlDataExt[] GetCnlDataExtArr(IList<int> cnlList)
        {
            int cnlCnt = cnlList.Count;
            CnlDataExt[] cnlDataExtArr = new CnlDataExt[cnlCnt];

            DataAccess dataAccess = AppData.DataAccess;
            DateTime dataAge;
            SrezTableLight.Srez snapshot = dataAccess.DataCache.GetCurSnapshot(out dataAge);

            string emptyVal = "";
            bool dataVisible = snapshot != null &&
                DataFormatter.CurDataVisible(dataAge, DateTime.Now, out emptyVal);

            for (int i = 0; i < cnlCnt; i++)
            {
                int cnlNum = cnlList[i];
                CnlDataExt cnlDataExt = new CnlDataExt(cnlNum);
                cnlDataExtArr[i] = cnlDataExt;

                SrezTableLight.CnlData cnlData;
                snapshot.GetCnlData(cnlNum, out cnlData);
                cnlDataExt.Val = cnlData.Val;
                cnlDataExt.Stat = cnlData.Stat;

                if (dataVisible)
                {
                    InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                    string text;
                    string textWithUnit;
                    DataFormatter.FormatCnlVal(cnlData.Val, cnlData.Stat, cnlProps, out text, out textWithUnit);

                    cnlDataExt.Text = text;
                    cnlDataExt.TextWithUnit = textWithUnit;
                    cnlDataExt.Color = DataFormatter.GetCnlValColor(cnlData.Val, cnlData.Stat, cnlProps,
                        dataAccess.GetColorByStat);
                }
                else
                {
                    cnlDataExt.Text = cnlDataExt.TextWithUnit = emptyVal;
                }
            }

            return cnlDataExtArr;
        }

        /// <summary>
        /// Получить объект для передачи данных, содержащий информацию об ошибке, в формате JSON
        /// </summary>
        private string GetErrorDtoJs(Exception ex)
        {
            return JsSerializer.Serialize(new DataTransferObject(false, ex.Message));
        }


        /// <summary>
        /// Проверить, что пользователь вошёл систему
        /// </summary>
        /// <remarks>Возвращает bool, упакованный в DataTransferObject, в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string CheckLoggedOn()
        {
            try
            {
                bool loggedOn = AppData.CheckLoggedOn(false);
                return JsSerializer.Serialize(new DataTransferObject(loggedOn));
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при проверке того, что пользователь вошел в систему" :
                    "Error checking that a user is logged on");
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить текущие данные входного канала
        /// </summary>
        /// <remarks>Возвращает SrezTableLight.CnlData, упакованный в DataTransferObject, в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlData(int cnlNum)
        {
            try
            {
                AppData.CheckLoggedOn();
                SrezTableLight.CnlData cnlData = AppData.DataAccess.GetCurCnlData(cnlNum);
                return JsSerializer.Serialize(new DataTransferObject(cnlData));
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении текущих данных входного канала {0}" :
                    "Error getting current data of the input channel {0}", cnlNum);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить расширенные текущие данные входного канала
        /// </summary>
        /// <remarks>Возвращает CnlDataExt, упакованный в DataTransferObject, в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataExt(int cnlNum)
        {
            try
            {
                AppData.CheckLoggedOn();
                CnlDataExt cnlDataExt = new CnlDataExt(cnlNum);
                DataAccess dataAccess = AppData.DataAccess;
                DateTime dataAge;
                SrezTableLight.CnlData cnlData = dataAccess.GetCurCnlData(cnlNum, out dataAge);
                cnlDataExt.Val = cnlData.Val;
                cnlDataExt.Stat = cnlData.Stat;

                string emptyVal;
                if (DataFormatter.CurDataVisible(dataAge, DateTime.Now, out emptyVal))
                {
                    InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                    string text;
                    string textWithUnit;
                    DataFormatter.FormatCnlVal(cnlData.Val, cnlData.Stat, cnlProps, out text, out textWithUnit);

                    cnlDataExt.Text = text;
                    cnlDataExt.TextWithUnit = textWithUnit;
                    cnlDataExt.Color = DataFormatter.GetCnlValColor(cnlData.Val, cnlData.Stat, cnlProps, 
                        dataAccess.GetColorByStat);
                }
                else
                {
                    cnlDataExt.Text = emptyVal;
                    cnlDataExt.TextWithUnit = emptyVal;
                }

                return JsSerializer.Serialize(new DataTransferObject(cnlDataExt));
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении расширенных текущих данных входного канала {0}" :
                    "Error getting extended current data of the input channel {0}", cnlNum);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить расширенные текущие данные заданных входных каналов
        /// </summary>
        /// <remarks>Возвращает CnlDataExt[], упакованный в DataTransferObject, в формате в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataExtByCnlNums(string cnlNums)
        {
            try
            {
                AppData.CheckLoggedOn();
                int[] cnlNumArr = WebUtils.QueryParamToIntArray(cnlNums);
                CnlDataExt[] cnlDataExtArr = GetCnlDataExtArr(cnlNumArr);
                return JsSerializer.Serialize(new DataTransferObject(cnlDataExtArr));
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении расширенных текущих данных заданных входных каналов" :
                    "Error getting extended current data of the specified input channels");
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить расширенные текущие данные входных каналов представления
        /// </summary>
        /// <remarks>Возвращает CnlDataExt[], упакованный в DataTransferObject, в формате в JSON.
        /// Представление должно быть уже загружено в кеш (для ускорения работы метода)</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataExtByView(int viewID)
        {
            try
            {
                AppData.CheckLoggedOn();
                BaseView view = AppData.ViewCache.GetViewFromCache(viewID);

                if (view == null)
                {
                    throw new ScadaException(Localization.UseRussian ?
                        "Не удалось получить представление из кеша" :
                        "Unable to get view from the cache");
                }
                else
                {
                    CnlDataExt[] cnlDataExtArr = GetCnlDataExtArr(view.CnlList);
                    return JsSerializer.Serialize(new DataTransferObject(cnlDataExtArr));
                }
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении расширенных текущих данных входных каналов предсталения с ид.={0}" :
                    "Error getting extended current input channel data of the view with id={0}", viewID);
                return GetErrorDtoJs(ex);
            }
        }

        /// <summary>
        /// Получить метку представления из кеша
        /// </summary>
        /// <remarks>Возвращает long, упакованный в DataTransferObject, в формате в JSON</remarks>
        public string GetViewStamp(int viewID)
        {
            try
            {
                AppData.CheckLoggedOn();
                BaseView view = AppData.ViewCache.GetViewFromCache(viewID);
                long stamp = view == null ? 0 : view.Stamp;
                return JsSerializer.Serialize(new DataTransferObject(stamp));
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении метки предсталения с ид.={0} из кеша" :
                    "Error getting stamp of the view with id={0} from the cache", viewID);
                return GetErrorDtoJs(ex);
            }
        }
    }
}