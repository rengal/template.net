using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    public static class BarcodeHelper
    {
        [NotNull]
        public static IBarcode Parse([NotNull] string barcodeString)
        {
            return BarcodeEan13.IsValid(barcodeString)
                ? new BarcodeEan13(barcodeString)
                : BarcodeCode128.IsUkraineMark(barcodeString)
                ? new BarcodeCode128(barcodeString)
                : BarcodePdf417.IsEgaisMark(barcodeString)
                ? new BarcodePdf417(barcodeString)
                : BarcodeDataMatrix.TryParseDataMatrixBarcode(barcodeString, out var barcodeDataMatrix)
                ? barcodeDataMatrix
                : new ArbitraryBarcode(barcodeString);
        }
    }
}
