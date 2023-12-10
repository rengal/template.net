namespace Resto.Data
{
    public partial class EpitomeDriverSettings
    {
        public string ToConnectionString()
        {
            string connectionString = "Data source=" + Server;
            connectionString += ";Initial Catalog=" + Database;
            connectionString += ";Connection Timeout=" + TimeOut;
            if (UseWindowsAccount)
            {
                connectionString += ";Integrated Security=true";
            }
            else
            {
                connectionString += ";Integrated Security=false";
                connectionString += ";User ID=" + Login;
                connectionString += ";Password=" + Password;
            }

            return connectionString;
        }
    }
}