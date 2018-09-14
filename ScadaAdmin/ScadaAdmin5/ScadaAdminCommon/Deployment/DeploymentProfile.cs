using Scada.Agent.Connector;
using System;
using System.Collections.Generic;

namespace Scada.Admin.Deployment
{
    public class DeploymentProfile
    {
        public string Name { get; set; }

        public ConnectionSettings ConnectionSettings { get; protected set; }

        public DownloadSettings DownloadSettings { get; protected set; }

        public UploadSettings UploadSettings { get; protected set; }
    }
}
