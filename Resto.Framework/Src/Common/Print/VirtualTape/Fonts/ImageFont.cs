using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public class ImageFont : Font
    {
        public delegate string ConvertToImageDelegate(string base64Image, string align, string resizeMode);

        [CanBeNull]
        public ConvertToImageDelegate ConvertToImage { get; set; }
    }
}