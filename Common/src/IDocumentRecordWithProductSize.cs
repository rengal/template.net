using Resto.Data;

namespace Resto.Common
{
    /// <summary>
    /// Интерфейс для строки документа, содержащей продукт,
    /// размер продукта и коэффициент списания.
    /// </summary>
    /// <remarks>
    /// Используется в редакторах документов для установки размера по умолчанию
    /// и коэффициента списания в зависимости от выбранного продукта и его количества.
    /// См. <see cref="ProductSizeExtensions.SetDefaultProductSize"/> и
    /// <see cref="ProductSizeExtensions.UpdateAmountFactor"/>.
    /// </remarks>
    public interface IDocumentRecordWithProductSize
    {
        Product Product { get; }
        ProductSize ProductSize { get; set; }
        decimal Amount { get; }
        decimal AmountFactor { get; set; }
    }
}