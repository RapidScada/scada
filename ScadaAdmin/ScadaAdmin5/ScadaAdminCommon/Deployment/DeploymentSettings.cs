using System;
using System.Collections.Generic;

namespace Scada.Admin.Deployment
{
    public class DeploymentSettings
    {
        public SortedList<string, DeploymentProfile> Profiles { get; protected set; }
    }
}
