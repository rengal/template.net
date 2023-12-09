// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Способ добавления скидки в заказ
    /// </summary>
    public enum DiscountSource
    {
        /// <summary>
        /// Скидка добавлена пользователем (вручную или прокаткой карты)
        /// </summary>
        User,

        /// <summary>
        /// Скидка добавлена автоматически (по таймеру)
        /// </summary>
        Auto,

        /// <summary>
        /// Скидка добавлена плагином
        /// </summary>
        Plugin,

    }
}
