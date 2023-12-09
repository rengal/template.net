using System;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;
using Resto.Framework.Common.Print.Tags;
using Resto.Framework.Common.Helpers;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

/// <summary>
/// Постпроцессор ленты преобразующий строки ленты в команды Slcs
/// </summary>
public class SlcsTapePostProcessor : Epl2TapePostProcessor
{
    public SlcsTapePostProcessor()
    {
        DefaultBarcodeHeightRatio = 0.35;
    }

    protected override void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines)
    {
        buffer.AddRange(PrinterEncoding.GetBytes("@\r\n"));
    }

    protected override IEnumerable<(XElement, string)> EndLabelHandler(List<byte> buffer, XElement xDocument)
    {
        buffer.AddRange(PrinterEncoding.GetBytes("P\r\n"));
        return new[] { (xDocument, PrinterEncoding.GetString(buffer.ToArray())) };
    }

    private (int fontNumber, int height) GetFontInfo(XElement xElement)
    {
        if (xElement.Name.LocalName.Equals(TagFont.F0.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font0Number, Font0Height);

        if (xElement.Name.LocalName.Equals(TagFont.F1.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font1Number, Font1Height);

        if (xElement.Name.LocalName.Equals(TagFont.F2.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font2Number, Font2Height);

        throw new ArgumentException(nameof(xElement));
    }

    protected override int TextHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (_, isBold, isReverse, _) = GetTextAttributes(xElement);
        var (fontNumber, height) = GetFontInfo(xElement);
        var spanText = xElement.Value.TrimEnd(' ', '\t');
        spanText = spanText.Replace("\\", "\\\\");
        spanText = spanText.Replace("\"", "\\\"");
        var cmd = string.Format(CultureInfo.InvariantCulture,
            "T{0},{1},{2},1,1,0,0,{3},{4},F'{5}'\r\n",
            LabelMarginHor, topPos, fontNumber,
            isReverse ? "R" : "N",
            isBold ? "B" : "N",
            spanText);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        return height;
    }

    protected override int BarcodeHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (barcodeHeight, isHri, xPos, _) = GetBarcodeAttributes(xElement, false, DefaultBarcodeHeightRatio);

        // Подсчитать ширину штрихкода и отцентрировать его 
        // Добавляем штрихкод (EAN13)
        var cmd = string.Format(CultureInfo.InvariantCulture,
            "B1{0},{1},7,{2},0,{3},0,{4},'{5}'\r\n",
            xPos, topPos + BarcodeMargin, NarrowBarWidth,
            isHri ? barcodeHeight - Font1Height - BarcodeMargin : barcodeHeight - BarcodeMargin,
            isHri ? 1 : 0,
            EscapeChars(xElement.Value));
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        return barcodeHeight + 2 * BarcodeMargin;
    }

    protected override int QrCodeHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        int ToModuleSize(string size)
        {
            return size switch
            {
                "tiny" => 2,
                "small" => 3,
                "normal" => 4,
                "large" => 5,
                "extralarge" => 6,
                _ => 5
            };
        }

        const int verticalMargin = 2;
        var (moduleSize, correctionLevel, qrSizeInDots, xPos) = GetQrCodeAttributes(xElement, ToModuleSize);

        // Добавляем 2D-штрихкод (QR)
        var cmd = string.Format(CultureInfo.InvariantCulture,
            "B2{0},{1},Q,2,{2},{3},0,'{4}'\r\n",
            xPos, topPos + BarcodeMargin, correctionLevel, moduleSize, xElement.Value);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));

        return qrSizeInDots + 2 * verticalMargin;
    }

    protected override int PagecutHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        buffer.AddRange(PrinterEncoding.GetBytes("P\r"));
        buffer.AddRange(PrinterEncoding.GetBytes("@\r\n"));
        return 0;
    }

    protected override int ImageHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (bitmap, xPos) = GetImageAttributes(xElement, topPos);
        if (bitmap == null)
            return 0;

        var (imageData, widthInBytes) = ImageHelper.ImageToByteArray(bitmap, true);

        var xL = (byte)(xPos % 256);
        var xH = (byte)(xPos / 256);
        var yL = (byte)(topPos % 256);
        var yH = (byte)(topPos / 256);
        var dhL = (byte)(widthInBytes % 256);
        var dhH = (byte)(widthInBytes / 256);
        var dvL = (byte)(bitmap.Height % 256);
        var dvH = (byte)(bitmap.Height / 256);

        buffer.AddRange(PrinterEncoding.GetBytes("LD"));
        buffer.AddRange(new[] {xL, xH, yL, yH, dhL, dhH, dvL, dvH});
        buffer.AddRange(imageData);
        buffer.AddRange(PrinterEncoding.GetBytes("\r\n"));

        return bitmap.Height;
    }

    private static string EscapeChars(string source)
    {
        return source
            .TrimEnd(' ', '\t')
            .Replace("\"", "\\[\"]");
    }
}