using Resto.Data;

namespace Resto.Common
{
    /// <summary>
    /// Класс для генерации списка списываемых блюд в IngredientWriteoffForm.
    /// </summary>
    public class ProductStoreAndAmount
    {
        public Store Store { get; set; }
        public Product Product { get; set; }
        public decimal InCount { get; set; }
    }
}
