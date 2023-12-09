using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Print.Tags;
using Resto.Framework.Common.Print.Tags.Xml;
using Resto.Framework.Src.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public sealed class FontPack
    {
        public FontPack(Font font0, Font font1, Font font2)
        {
            Font0 = font0.Clone();
            Font1 = font1.Clone();
            Font2 = font2.Clone();
        }

        private Font font0;
        public Font Font0
        {
            get => font0;
            private set
            {
                font0 = value;
                font0.Id = FontAttrValue.F0.GetName();
            }
        }

        private Font font1;
        public Font Font1
        {
            get { return font1; }
            private set
            {
                font1 = value;
                font1.Id = FontAttrValue.F1.GetName();
            }
        }

        private Font font2;
        public Font Font2
        {
            get => font2;
            private set
            {
                font2 = value;
                font2.Id = FontAttrValue.F2.GetName();
            }
        }

        private BarCodeFont barCode;
        public BarCodeFont BarCode
        {
            get => barCode;
            set
            {
                barCode = value;
                if (barCode != null)
                    barCode.Id = TagBarCode.TagName;
            }
        }

        private LogoFont logo;
        public LogoFont Logo
        {
            get => logo;
            set
            {
                logo = value;
                if (logo != null)
                    logo.Id = TagLogo.TagName;
            }
        }

        private ImageFont image;
        public ImageFont Image
        {
            get => image;
            set
            {
                image = value;
                if (image != null)
                    image.Id = TagImage.TagName;
            }
        }

        private QRCodeFont qrCode;
        public QRCodeFont QRCode
        {
            get => qrCode;
            set
            {
                qrCode = value;
                if (qrCode != null)
                    qrCode.Id = TagQRCode.TagName;
            }
        }

        private PulseFont pulse;
        public PulseFont Pulse
        {
            get => pulse;
            set
            {
                pulse = value;
                if (pulse != null)
                    pulse.Id = TagPulse.TagName;
            }
        }

        public FontGlyph FontGlyph { get; set; }

        internal bool ContainsFont([NotNull] Font font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            return
                font == font0 ||
                font == font1 ||
                font == font2 ||
                font == barCode ||
                font == logo ||
                font == pulse ||
                font == qrCode ||
                font == image;
        }

        internal Font this[string id]
        {
            get
            {
                if (font0.Id == id)
                    return font0;
                if (font1.Id == id)
                    return font1;
                if (font2.Id == id)
                    return font2;
                if (barCode != null && barCode.Id == id)
                    return barCode;
                if (pulse != null && pulse.Id == id)
                    return pulse;
                if (image != null && image.Id == id)
                    return image;
                throw new NotSupportedException();
            }
        }
    }
}