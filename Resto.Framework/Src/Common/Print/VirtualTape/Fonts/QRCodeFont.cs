namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public sealed class QRCodeFont : Font
    {
        public bool IsSupported { get; set; }
        public delegate string ConvertToQRCodeDelegate(string text, string align, string size, string correction);
        public ConvertToQRCodeDelegate ConvertToQRCode { get; set; }        
    }
}
