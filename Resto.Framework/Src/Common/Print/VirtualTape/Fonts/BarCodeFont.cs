namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public sealed class BarCodeFont : Font
    {
        public bool IsSupported { get; set; }
        public string EndEsc { get; set; }

        public delegate string ConvertToBarcodeDelegate(string text, string align, string heightRatio, string hri);
        public ConvertToBarcodeDelegate ConvertToBarcode { get; set; }
    }
}
