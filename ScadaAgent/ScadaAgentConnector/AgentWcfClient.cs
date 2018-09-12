namespace Scada.Agent.Connector
{
    public class AgentWcfClient
    {
        public AgentWcfClient(object connOptions)
        {
        }


        public void Open()
        {

        }

        public void Close()
        {

        }

        public void GetAvailableConfig(out ConfigParts configParts)
        {
            configParts = ConfigParts.None;
        }

        public void DownloadConfig(string destFileName, ConfigOptions configOptions)
        {
        }

        public void UploadConfig(string sourceFileName, ConfigOptions ConfigOptions)
        {

        }
    }
}
