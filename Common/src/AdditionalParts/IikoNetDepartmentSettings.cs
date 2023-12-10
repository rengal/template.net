namespace Resto.Data
{
    /// <summary>
    /// Настройки подключения к iikoNet, специфичные для RMS.
    /// Т.к. это embedded-объект, не копипастил его во фронте.
    /// </summary>
    public partial class IikoNetDepartmentSettings : IIikoNetLocalSettings
    {
        #region Overrides

        public override bool Equals(object obj)
        {
            var comparableSettings = obj as IikoNetDepartmentSettings;
            if (comparableSettings == null)
            {
                return false;
            }

            return comparableSettings.IikoNetId == IikoNetId && comparableSettings.Password == Password &&
                   comparableSettings.iikoNetPosServerAddress == IikoNetPosServerAddress &&
                   comparableSettings.ShouldUsePos == ShouldUsePos;
        }

        public override int GetHashCode()
        {
            return new { IikoNetId, Password, IikoNetPosServerAddress, ShouldUsePos }.GetHashCode();
        }

        #endregion

        #region Public merhods

        public IikoNetDepartmentSettings(IIikoNetLocalSettings localSettings)
        {
            IikoNetPosServerAddress = localSettings.IikoNetPosServerAddress;
            ShouldUsePos = localSettings.ShouldUsePos;
            IikoNetId = localSettings.IikoNetId;
            Password = localSettings.Password;
        }

        #endregion
    }
}
