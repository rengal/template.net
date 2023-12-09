namespace Resto.Framework.Common
{
    /// <summary>
    /// Интерфейс для объектов, следящих за изменениями Removable-устройств с помощью
    /// RemovableDeviceHelper
    /// </summary>
    public interface IRemovableDeviceListener
    {
        /// <summary>
        /// Метод, нотифицирующий об изменении состояния устройства
        /// </summary>
        /// <param name="eventType">Тип события</param>
        /// <param name="rootPath">Путь к устройству (буква диска)</param>
        void OnDeviceChanged(DeviceEventType eventType, string rootPath); 
    }
}