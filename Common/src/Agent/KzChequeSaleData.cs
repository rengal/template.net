namespace Resto.Data
{
    /// <summary>
    /// Дополнительные данные о товаре в соответствии с законодательством Казахстана
    /// </summary>
    public sealed class KzChequeSaleData
    {
        public KzChequeSaleData()
        {
        }

        public KzChequeSaleData(int? unit)
        {
            Unit = unit;
        }

        /// <summary>
        /// Единица измерения (реквизит 2108). Допустимые значения:
        /// 0 - штуки
        /// 11 - килограммы
        /// 41 - литры
        /// </summary>
        public int? Unit { get; set; }
    }
}