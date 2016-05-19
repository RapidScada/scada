using System;

namespace Scada.Web.Plugins.Table
{
    [Obsolete("For test purposes")]
    public class CustomDataWndSpec : DataWindowSpec
    {
        public override string ContentTypeCode
        {
            get
            {
                return "CustomData";
            }
        }

        public override string Name
        {
            get
            {
                return "Custom data";
            }
        }

        public override string Url
        {
            get
            {
                return "~/plugins/Table/CustomData.aspx";
            }
        }
    }
}