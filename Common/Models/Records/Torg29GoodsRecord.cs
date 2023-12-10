using Resto.Data;

namespace Resto.Common.Models.Records
{
    /// <summary>
    /// Запись товарного отчета по продуктам.
    /// </summary>
    public sealed class Torg29GoodsRecord : Torg29RecordBase
    {
        /// <summary>
        /// Код продукта.
        /// </summary>
        public string ProductCode
        {
            get { return Nomenclature != null ? Nomenclature.Num : null; }
        }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public MeasureUnit MeasureUnit
        {
            get { return Nomenclature != null ? Nomenclature.MainUnit : null; }
        }

        /// <summary>
        /// Единица измерения строкой.
        /// </summary>
        public string MeasureUnitAsString
        {
            get { return MeasureUnit != null ? MeasureUnit.ToString() : ""; }
        }

        /// <summary>
        /// Продукт.
        /// </summary>
        public Product Nomenclature { get; set; }

        /// <summary>
        /// Продукт строкой.
        /// </summary>
        public string NomenclatureAsString
        {
            get { return Nomenclature != null ? Nomenclature.ToString() : ""; }
        }

        /// <summary>
        /// Количество в единице измерения.
        /// </summary>
        public decimal AmountInMeasureUnit { get; set; }

        /// <summary>
        /// Учетная себестоимость за единицу.
        /// </summary>
        public decimal CostPricePerUnit { get; set; }

        /// <summary>
        /// Учетная себестоимость.
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// Себестоимость без НДС.
        /// </summary>
        public decimal CostPriceWithoutNDS { get; set; }

        /// <summary>
        /// Сумма НДС.
        /// </summary>
        public decimal NDSSum { get; set; }

        /// <summary>
        /// Процент НДС.
        /// </summary>
        public decimal NDSPercent { get; set; }

        /// <summary>
        /// Получить себестоимость c НДС.
        /// </summary>
        public decimal CostPriceWithNDS
        {
            get { return CostPriceWithoutNDS + NDSSum; }
        }

        /// <summary>
        /// Категория продукта.
        /// </summary>
        public ProductCategory Category
        {
            get { return Nomenclature != null ? Nomenclature.Category : null; }
        }

        /// <summary>
        /// Категория продукта строкой.
        /// </summary>
        public string CategoryAsString
        {
            get { return Category != null ? Category.ToString() : null; }
        }

        /// <summary>
        /// Бухгалтерская категория.
        /// </summary>
        public AccountingCategory AccountingCategory
        {
            get { return Nomenclature != null ? Nomenclature.AccountingCategory : null; }
        }

        /// <summary>
        /// Бухгалтерская категория строкой.
        /// </summary>
        public string AccountingCategoryAsString
        {
            get { return AccountingCategory != null ? AccountingCategory.ToString() : null; }
        }

        /// <summary>
        /// Тип элемента номенклатуры.
        /// </summary>
        public string ProductType
        {
            get { return Nomenclature != null ? Nomenclature.Type.ToString() : null; }
        }

        /// <summary>
        /// Родительская группа.
        /// </summary>
        public ProductGroup ParentGroup
        {
            get { return Nomenclature != null ? Nomenclature.Parent : null; }
        }

        /// <summary>
        /// Родительская группа строкой.
        /// </summary>
        public string ParentGroupAsString
        {
            get { return ParentGroup != null ? ParentGroup.ToString() : null; }
        }
    }
}