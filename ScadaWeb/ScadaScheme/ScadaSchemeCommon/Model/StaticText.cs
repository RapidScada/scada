using Scada.Scheme.Model.PropertyGrid;
using System;
using System.Xml;

namespace Scada.Scheme.Model
{
    public class StaticText : BaseComponent
    {
        [DisplayName("Txt"), Category(Categories.Appearance), Description("Initial descr")]
        public string Text { get; set; }

        [DisplayName(""), Category(""), Description("")]
        public string Text2 { get; set; }

        public override void LoadFromXml(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }
    }
}
