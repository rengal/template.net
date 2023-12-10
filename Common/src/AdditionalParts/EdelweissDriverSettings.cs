namespace Resto.Data
{
    public partial class EdelweissDriverSettings
    {
        public string ToConnectionString()
        {
            string connectionString =
                "Driver=Adaptive Server Anywhere 9.0;" +
                "Eng=" + EdelDb + ";" +
                "Links = tcpip(Host=" + EdelServer + ");" +
                "Uid=" + Login + ";" +
                "Pwd=" + Password + ";";

            return connectionString;
        }
    }
}