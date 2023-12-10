namespace Resto.Data
{
    public partial class AgentDevice
    {
        public override string ToString()
        {
            return NameLocal;
        }

        public bool IsExternal()
        {
            return Settings is ExternalDeviceSettings;
        }

        public bool IsNullDriver()
        {
            return driver is ChequePrinterNullDriver || driver is PrinterNullDriver || driver is ScaleNullDriver;
        }

        public bool IsAgentDevice()
        {
            return Settings != null && !(Settings is ExternalDeviceSettings);
        }
    }
}