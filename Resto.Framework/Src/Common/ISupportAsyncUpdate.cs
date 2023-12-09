
namespace Resto.Framework.Common
{
    /// <summary>
    /// Интерфейс отвечает за асинхронное обновление данных (себестоимость номенклатуры, остатки номенклатуры)
    /// </summary>
    public interface ISupportAsyncUpdate
    {
        /// <summary>
        /// Реализует асинхронное обновление необходимой информации для контрола
        /// </summary>
        void AsyncUpdateControl();
    }
}
