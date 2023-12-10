using System;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс строки бизнес-документа для которой можно рассчитывать себестоимость.
    /// </summary>
    public interface IItemWithCost
    {
        /// <summary>
        /// Идентификатор строки
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Кол-во продукта в его базовых единицах
        /// </summary>
        decimal Amount { get; }
    }
}
