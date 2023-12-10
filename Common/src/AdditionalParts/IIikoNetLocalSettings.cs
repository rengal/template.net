namespace Resto.Data
{
    /// <summary>
    /// Настройки подключения к iikoNet, специфичные для RMS.
    /// Интерфейс понадобился, чтобы обращаться единообразно как к словарю IikoNetSettings.departmentSpecific,
    /// так и с "настройками по умолчанию", хранящимися в старых полях прямо в IikoNetSettings.
    /// </summary>
    public interface IIikoNetLocalSettings
    {
        string IikoNetPosServerAddress { get; set; }
        bool ShouldUsePos { get; set; }
        string IikoNetId { get; set; }
        string Password { get; set; }
    }
}
