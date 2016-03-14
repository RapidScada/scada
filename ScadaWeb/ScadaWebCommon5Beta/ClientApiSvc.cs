using Scada.Client;
using Scada.Data;
using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Web
{
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
        [OperationContract]
        [WebGet]
        public string GetCurCnlData(int cnlNum)
        {
            SrezTableLight.CnlData cnlData = new SrezTableLight.CnlData();
            return JsSerializer.Serialize(cnlData);
        }

        /// <summary>
        /// Получить полные текущие данные входного канала
        /// </summary>
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataFull(int cnlNum)
        {
            try
            {
                CnlDataDTO cnlDataDTO = new CnlDataDTO(cnlNum);
                DataAccess dataAccess = AppData.DataAccess;
                InCnlProps cnlProps = dataAccess.GetCnlProps(cnlNum);
                DateTime dataDT;
                SrezTableLight.CnlData cnlData = dataAccess.GetCurCnlData(cnlNum, out dataDT);
                cnlDataDTO.Val = cnlData.Val;
                cnlDataDTO.Stat = cnlData.Stat;

                string emptyVal;
                if (DataFormatter.CurDataVisible(dataDT, DateTime.Now, out emptyVal))
                {
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
        [OperationContract]
        [WebGet]
        public string GetCurCnlDataByView(int viewID)
        {
            return "";
        }
    }
}