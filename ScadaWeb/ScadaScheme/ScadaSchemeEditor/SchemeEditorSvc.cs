using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;

namespace Scada.Scheme.Editor
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchemeEditorSvc
    {
        /// <summary>
        /// Мексимальное количество символов строке данных в формате JSON, 10 МБ
        /// </summary>
        private const int MaxJsonLen = 10485760;
        /// <summary>
        /// Обеспечивает сериализацию результатов методов сервиса
        /// </summary>
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLen };


        /// <summary>
        /// Разредить кросс-доменный доступ к сервису
        /// </summary>
        private void AllowAccess()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin: *");
        }


        [OperationContract]
        [WebGet]
        public string DoWork(string arg)
        {
            AllowAccess();
            return JsSerializer.Serialize("Test Result, arg = " + arg);
        }
    }
}
