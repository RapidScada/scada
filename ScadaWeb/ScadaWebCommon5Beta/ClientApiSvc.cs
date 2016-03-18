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
 * Summary  : WCF-service that provides the client API
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
    /// WCF-service that provides the client API
    /// <para>WCF-сервис, обеспечивающий работу клиентского API</para>
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ClientApiSvc
    {
        /// <summary>
        /// Класс объекта для передачи данных входого канала
        /// </summary>
        private class CnlDataDTO
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public CnlDataDTO(int cnlNum)
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
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer();
        /// <summary>
        /// Обеспечивает форматирование данных входных каналов и событий
        /// </summary>
        private static readonly DataFormatter DataFormatter = new DataFormatter();


        /// <summary>
        /// Получить текущие данные входного канала
        /// </summary>
        /// <remarks>Возвращает SrezTableLight.CnlData, преобразованный в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlData(int cnlNum)
        {
            try
            {
                SrezTableLight.CnlData cnlData = AppData.DataAccess.GetCurCnlData(cnlNum);
                return JsSerializer.Serialize(cnlData);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении текущих данных входного канала {0}" :
                    "Error getting current data of the input channel {0}", cnlNum);
                return "";
            }
        }

        /// <summary>
        /// Получить полные текущие данные входного канала
        /// </summary>
        /// <remarks>Возвращает CnlDataDTO, преобразованный в JSON</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataFull(int cnlNum)
        {
            try
            {
                CnlDataDTO cnlDataDTO = new CnlDataDTO(cnlNum);
                DataAccess dataAccess = AppData.DataAccess;
                DateTime dataAge;
                SrezTableLight.CnlData cnlData = dataAccess.GetCurCnlData(cnlNum, out dataAge);
                cnlDataDTO.Val = cnlData.Val;
                cnlDataDTO.Stat = cnlData.Stat;

                string emptyVal;
                if (DataFormatter.CurDataVisible(dataAge, DateTime.Now, out emptyVal))
                {
                    InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                    string text;
                    string textWithUnit;
                    DataFormatter.FormatCnlVal(cnlData.Val, cnlData.Stat, cnlProps, out text, out textWithUnit);

                    cnlDataDTO.Text = text;
                    cnlDataDTO.TextWithUnit = textWithUnit;
                    cnlDataDTO.Color = DataFormatter.GetCnlValColor(cnlData.Val, cnlData.Stat, cnlProps, 
                        dataAccess.GetColorByStat);
                }
                else
                {
                    cnlDataDTO.Text = emptyVal;
                    cnlDataDTO.TextWithUnit = emptyVal;
                }

                return JsSerializer.Serialize(cnlDataDTO);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении полных текущих данных входного канала {0}" :
                    "Error getting full current data of the input channel {0}", cnlNum);
                return "";
            }
        }

        /// <summary>
        /// Получить текущие данные входных каналов представления
        /// </summary>
        /// <remarks>Возвращает CnlDataDTO[], преобразованный в JSON.
        /// Представление должно быть уже загружено в кеш</remarks>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataByView(int viewID)
        {
            try
            {
                CnlDataDTO[] cnlDataDTOs;
                BaseView view = AppData.ViewCache.GetViewFromCache(viewID);

                if (view == null)
                {
                    AppData.Log.WriteError(string.Format(Localization.UseRussian ?
                        "Не удалось получить представление с ид.={0} из кеша" :
                        "Unable to get view with ID={0} from the cache", viewID));
                    cnlDataDTOs = new CnlDataDTO[0];
                }
                else
                {
                    List<int> cnlList = view.CnlList;
                    int cnlCnt = cnlList.Count;
                    cnlDataDTOs = new CnlDataDTO[cnlCnt];

                    DataAccess dataAccess = AppData.DataAccess;
                    DateTime dataAge;
                    SrezTableLight.Srez snapshot = dataAccess.DataCache.GetCurSnapshot(out dataAge);

                    string emptyVal = "";
                    bool dataVisible = snapshot != null && 
                        DataFormatter.CurDataVisible(dataAge, DateTime.Now, out emptyVal);

                    for (int i = 0; i < cnlCnt; i++)
                    {
                        int cnlNum = cnlList[i];
                        CnlDataDTO cnlDataDTO = new CnlDataDTO(cnlNum);
                        cnlDataDTOs[i] = cnlDataDTO;

                        if (dataVisible)
                        {
                            SrezTableLight.CnlData cnlData;
                            snapshot.GetCnlData(cnlNum, out cnlData);

                            InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                            string text;
                            string textWithUnit;
                            DataFormatter.FormatCnlVal(cnlData.Val, cnlData.Stat, cnlProps, out text, out textWithUnit);

                            cnlDataDTO.Text = text;
                            cnlDataDTO.TextWithUnit = textWithUnit;
                            cnlDataDTO.Color = DataFormatter.GetCnlValColor(cnlData.Val, cnlData.Stat, cnlProps,
                                dataAccess.GetColorByStat);
                        }
                        else
                        {
                            cnlDataDTO.Text = cnlDataDTO.TextWithUnit = emptyVal;
                        }
                    }
                }

                return JsSerializer.Serialize(cnlDataDTOs);
            }
            catch (Exception ex)
            {
                AppData.Log.WriteException(ex, Localization.UseRussian ?
                    "Ошибка при получении текущих данных входных каналов предсталения с ид.={0}" :
                    "Error getting current input channel data of the view with id={0}", viewID);
                return "";
            }
        }
    }
}