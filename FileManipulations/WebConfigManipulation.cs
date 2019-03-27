using System.Configuration;

namespace Framework.FileManipulations
{
    public static class WebConfigManipulation
    {
        public static string GetConfig(string config)
        {
            return ConfigurationManager.AppSettings[config];
        }
    }
}
