namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public sealed class LogoFont : Font
    {
        public bool IsSupported { get; set; }
        public delegate string ConvertToLogoDelegate(string text);
        public ConvertToLogoDelegate ConvertToLogo { get; set; }
    }
}
