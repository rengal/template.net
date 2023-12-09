namespace Resto.Framework.Common.Currency
{
    public interface ICurrencyProvider
    {
        ICurrency Currency { get; }

        int MoneyPrecision { get; }
    }
}