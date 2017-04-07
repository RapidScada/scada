using System;
using System.ComponentModel;
using System.Xml;

namespace Scada.Scheme.Model
{
    public class StaticText : BaseComponent
    {
        public class MyCategoryAttribute : CategoryAttribute
        {
            public string Cat { get; set; }

            protected override string GetLocalizedString(string value)
            {
                return "My " + Cat + " - " + value;
            }
        }

        [Description("Initial descr"), MyCategory(), DisplayName("Txt")]
        public string Text { get; set; }

        public override void LoadFromXml(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }
    }
}
