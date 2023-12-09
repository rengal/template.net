using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Resto.Framework.Common.Print.Tags;
using System.Globalization;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess
{
    /// <summary>
    /// Подпроцессор обрабатываюший ленту по умолчанию
    /// </summary>
    public class DefaultTapePostProcessor : BaseTapePostProcessor
    {
        private delegate XElement TagHandler(XElement xElement);
        private readonly Dictionary<string, TagHandler> tagHandlers;

        public DefaultTapePostProcessor()
        {
            tagHandlers = new Dictionary<string, TagHandler>
            {
                { TagFont.F0.Name, TextHandler },
                { TagFont.F1.Name, TextHandler },
                { TagFont.F2.Name, TextHandler },
                { TagBarCode.Instance.Name, BarcodeHandler },
                { TagQRCode.Instance.Name, QrCodeHandler },
                { TagImage.Instance.Name, ImageHandler },
                { TagPagecut.Instance.Name, PagecutHandler }
            };
        }

        public override IEnumerable<(XElement document, string text)> Process(ITape tape)
        {
            if (tape == null)
                throw new ArgumentNullException(nameof(tape));

            var xDocument = new XElement(TagDoc.Instance.Name);

            var tapeLines = tape.GetLines();

            foreach (var tapeLine in tapeLines)
            {
                xDocument.Add(tapeLine.Element);

                if (!tagHandlers.TryGetValue(tapeLine.Element.Name.LocalName, out var handler))
                    continue;

                var postProcessElement = handler(tapeLine.Element);
                xDocument.Add(postProcessElement);
            }

            return new[] { (xDocument, string.Empty) };
        }

        protected XElement TextHandler(XElement xElement)
        {
            return xElement;
        }

        protected XElement BarcodeHandler(XElement xElement)
        {
            const double defaultHeigthRatio = 0.35;
            var alignAttr = xElement.Attribute("align")?.Value ?? "center";

            var heightRatioStr = xElement.Attribute("heightRatio")?.Value;
            if (string.IsNullOrEmpty(heightRatioStr) || !double.TryParse(heightRatioStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var heightRatio))
                heightRatio = defaultHeigthRatio;

            var hriStr = xElement.Attribute("hri")?.Value;
            var isHri = !string.Equals(hriStr, "off", StringComparison.InvariantCulture);

            // EncodingOptions ^ options = gcnew EncodingOptions();
            // options->Height = imgHeight;
            // options->Width = imgWidth;
            // options->PureBarcode = !isHri;
            // BarcodeWriter ^ barcodeWriter = gcnew BarcodeWriter();
            // barcodeWriter->Format = barcodeFormat;
            // barcodeWriter->Options = options;
            // System::Drawing::Bitmap ^ bitmap = nullptr;
            // try
            // {
            //     bitmap = barcodeWriter->Write(barcodeText);
            //     if (bitmap == nullptr)
            //     {
            //         LOG(Log::WARN) << L"PrintUtils::CreateGDIBarcodeBitmap() - Barcode is empty";
            //         return (HBITMAP)NULL;
            //     }
            // }
            // catch (System::Exception^e)
            // {
            //     errMsg = marshal_as<std::wstring>(e->Message);
            //     LOG(Log::WARN) << L"PrintUtils::CreateGDIBarcodeBitmap() - Barcode failed: " << errMsg.c_str();
            //     return (HBITMAP)NULL;
            // }
            return xElement;
        }

        protected XElement QrCodeHandler(XElement xElement)
        {
            // int ToModuleSize(string size)
            // {
            //     return size switch
            //     {
            //         "tiny" => 3,
            //         "small" => 4,
            //         "normal" => 6,
            //         "large" => 8,
            //         "extralarge" => 10,
            //         _ => 6
            //     };
            // }
            //
            // const int verticalMargin = 2;
            // var (moduleSize, correctionLevel, qrSizeInDots, xPos) = GetQrCodeAttributes(xElement, ToModuleSize);
            //
            // var cmd = string.Format(CultureInfo.InvariantCulture,
            //     "b{0},{1},Q,m2,s{2},e{3},\"{4}\"\n",
            //     xPos, topPos, moduleSize, correctionLevel, xElement.Value);
            // buffer.AddRange(PrinterEncoding.GetBytes(cmd));
            //
            // return qrSizeInDots + 2 * verticalMargin;
            return xElement;
        }

        protected XElement PagecutHandler(XElement xElement)
        {
            // buffer.AddRange(PrinterEncoding.GetBytes("P1\n"));
            // buffer.AddRange(PrinterEncoding.GetBytes("\nN\n"));
            // return 0;
            return xElement;
        }

        protected XElement ImageHandler(XElement xElement)
        {
            // var (bitmap, xPos) = GetImageAttributes(xElement, topPos);
            // var (imageData, widthInBytes) = ImageHelper.ImageToByteArray(bitmap);
            return xElement;
        }
    }
}
