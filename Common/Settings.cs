namespace Resto.Common.Properties
{
    public static class Settings
    {
        public static string IIKO_APPDATA_SUBPATH = "iiko";
        public static string RMS_EDITION_SUBPATH = "Rms";
        public static string CHAIN_EDITION_SUBPATH = "Chain";
        public static string BACK_LOG_FILENAME = "back-log.log";
        public static decimal MAX_DECIMAL_VALUE = 999999999.999999999m;
        public static string HELP_URI = "https://{language}.iiko.help/articles/#!iiko{product}-{version:-}/{topic}";
        public static int SERVICE_METHOD_TIMEOUT_MILLISECONDS = 259200000;
        public static int GETTING_SERVER_INFO_TIMEOUT_MILLISECONDS = 10000;
        public static string SERVER_INFO_PAGE_ADDRESS = "{0}://{1}:{2}{3}/get_server_info.jsp?encoding=UTF-8";
        public static string SERVER_IPluginsDirectoryRelativePathNFO_PAGE_ADDRESS = "Plugins";
        public static bool IgnoreCleverenceDriverDriverExceptions = false;
        public static int LogFileAgeDays = 90;
    }
}
