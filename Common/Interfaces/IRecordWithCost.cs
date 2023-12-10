using Resto.Data;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Бэковская строка документа для которой можно рассчитывать себестоимость
    /// </summary>
    /// <typeparam name="TItem">Тип связанной строки бизнес-документа</typeparam>
    public interface IRecordWithCost<TItem> where TItem : IItemWithCost
    {
        /// <summary>
        /// Склад.
        /// Должен быть реализован если склад может быть указан у каждой строки отдельно.
        /// </summary>
        Store Store { get; }

        /// <summary>
        /// Продукт для которого рассчитывается себестоимость
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Кол-во продукта в его базовых единицах измерения
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Связанная строка бизнес-документа
        /// </summary>
        TItem Item { get; }
    }
}
