namespace Resto.Common.Plastek
{
    public static class PlastekCustomerCardStatusExtensions
    {
        public static bool IsActive(this PlastekCustomerCardStatus cardStatus)
        {
            return cardStatus == PlastekCustomerCardStatus.Active || cardStatus == PlastekCustomerCardStatus.Authorized;
        }
    }
}
