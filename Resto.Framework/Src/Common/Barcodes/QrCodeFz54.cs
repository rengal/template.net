using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// QR код, содержащий реквизиты для проверки кассового чека.
    /// </summary>
    public static class QrCodeFz54
    {
        public static bool IsValid([NotNull] string barcodeString)
        {
            return barcodeString.ToLowerInvariant().Contains("&fp=");
        }
      
    }
}
