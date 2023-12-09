namespace Resto.Framework.Common
{
    /// <summary>
    /// Типы событий об изменении состояния устройств
    /// </summary>
    public enum DeviceEventType
    {
        /// <summary>
        /// Значение не определено
        /// </summary>
        Unknown,
        
        /// <summary>
        /// Устройство добавлено в систему и готово к работе
        /// </summary>
        Added,

        /// <summary>
        /// Устройство удалено из системы
        /// </summary>
        Removed
    }
}