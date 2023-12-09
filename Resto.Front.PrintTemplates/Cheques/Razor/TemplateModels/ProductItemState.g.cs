// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Состояние элемента заказа (блюда)
    /// </summary>
    public enum ProductItemState
    {
        /// <summary>
        /// Новое
        /// </summary>
        Added,

        /// <summary>
        /// Распечатано в сервисном чеке, но приготовление ещё не началось
        /// </summary>
        PrintedNotCooking,

        /// <summary>
        /// Готовится
        /// </summary>
        CookingStarted,

        /// <summary>
        /// Приготовлено
        /// </summary>
        CookingCompleted,

        /// <summary>
        /// Подано
        /// </summary>
        Served,

        /// <summary>
        /// В пречеке
        /// </summary>
        InBill,

    }
}
