
namespace Resto.Data
{
    public interface IInvoiceItem
    {
        decimal SumWithNds { get; }
        decimal SumWithoutNds { get; }
        string MeasureUnits { get; }
        string MeasureUnitsOKEI { get; }
        decimal NdsSum { get; }
        decimal NdsPercent { get; }
        string OrderNumber { get; set; }
        decimal Price { get; }
        decimal PriceWithoutVat { get; }
        decimal Quantity { get; }
        string WareCode { get; }
        string WareName { get; }
    }
}
