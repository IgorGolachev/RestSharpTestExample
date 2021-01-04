using System.Configuration;

namespace core.Util
{
    public sealed class ConfigHelper
    {
        #region AppConfigValues
        public static string ExecutionEnvironment = ConfigurationManager.AppSettings["ExecutionEnvironment"];
        public static string DomainName = ConfigurationManager.AppSettings["DomainName"];
        public static string BaseURL = ConfigurationManager.AppSettings["BaseURL"];
        public static string DBUserName = ConfigurationManager.AppSettings["DBUserName"];
        public static string DBUserPassword = ConfigurationManager.AppSettings["DBUserPassword"];
        public static string RestSharpTimeOut = ConfigurationManager.AppSettings["RestSharpTimeOut"];
        #endregion
    }
}