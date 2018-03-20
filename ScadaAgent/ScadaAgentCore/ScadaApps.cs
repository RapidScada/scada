using System;

namespace Scada.Agent
{
    [Flags]
    public enum ScadaApps
    {
        None = 0,
        Server = 1,
        Communicator = 2,
        Webstation = 4
    }
}
