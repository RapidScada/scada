using System.IO;
using System.ServiceModel;

namespace Scada.Agent.Wcf
{
    [MessageContract]
    public class ConfigUploadMessage
    {
        [MessageHeader]
        public long SessionID;

        [MessageHeader]
        public ConfigOptions ConfigOptions;

        [MessageBodyMember]
        public Stream Stream;
    }
}
