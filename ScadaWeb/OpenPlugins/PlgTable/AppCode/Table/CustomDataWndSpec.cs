using System;

namespace Scada.Web.Plugins.Table
{
    [Obsolete("For test purposes")]
    public class CustomDataWndSpec : ContentSpec
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
                return "CustomData";
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