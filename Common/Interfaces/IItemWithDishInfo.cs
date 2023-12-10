using System;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс, описывающий блюдо банкета/доставки, как закрытых, так и открытых
    /// </summary>
    public interface IItemWithDishInfo
    {
        /// <summary>
        /// Идентификатор блюда (сущности)
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Блюдо
        /// </summary>
        Product DishInfo { get; set; }

        /// <summary>
        /// Склад, с которого списывается блюдо
        /// </summary>
        Store Store { get; set; }

        /// <summary>
        /// Количество блюда
        /// </summary>
        decimal? DishAmount { get; set; }

        /// <summary>
        /// Цена проданного/заказанного блюда
        /// </summary>
        decimal? DishPrice { get; set; }

        /// <summary>
        /// Сумма строки без учета скидки и наценки
        /// </summary>
        decimal? DishSum { get; set; }
    }
}
