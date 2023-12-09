using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Globalization;
using Resto.Framework.Common.Helpers;
using Resto.Framework.Common.Print.Tags;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

/// <summary>
/// Постпроцессор ленты преобразующий строки ленты в команды Tspl
/// </summary>
public class TsplTapePostProcessor : BaseLabelPostProcessor
{
    /// <summary>
    /// Ширина в точках шрифта f0
    /// </summary>
    public int Font0Width { get; set; }

    /// <summary>
    /// Ширина в точках шрифта f1
    /// </summary>
    public int Font1Width { get; set; }

    /// <summary>
    /// Ширина в точках шрифта f2
    /// </summary>
    public int Font2Width { get; set; }

    /// <summary>
    /// Ширина этикетки в миллиметрах
    /// </summary>
    public int LabelWidthInMm { get; set; }

    /// <summary>
    /// Высота этикетки в миллиметрах
    /// </summary>
    public int LabelHeightInMm { get; set; }

    protected virtual string LineDelimiter => "\n";

    public TsplTapePostProcessor()
    {
        DefaultBarcodeHeightRatio = 0.35;
    }

    protected override void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines)
    {
        buffer.AddRange(PrinterEncoding.GetBytes($"SIZE {LabelWidth} dot, {LabelHeight} dot{LineDelimiter}"));
        buffer.AddRange(PrinterEncoding.GetBytes($"CLS{LineDelimiter}"));
    }

    protected override IEnumerable<(XElement, string)> EndLabelHandler(List<byte> buffer, XElement xDocument)
    {
        var result = new List<(XElement, string)>();
        buffer.AddRange(PrinterEncoding.GetBytes($"PRINT 1{LineDelimiter}"));
        result.Add((xDocument, PrinterEncoding.GetString(buffer.ToArray())));
        return result;
    }

    private (int fontNumber, int fontWidth, int fontHeight) GetFontInfo(XElement xElement)
    {
        if (xElement.Name.LocalName.Equals(TagFont.F0.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font0Number, Font0Width, Font0Height);

        if (xElement.Name.LocalName.Equals(TagFont.F1.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font1Number, Font1Width, Font1Height);

        if (xElement.Name.LocalName.Equals(TagFont.F2.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font2Number, Font2Width, Font2Height);

        throw new ArgumentException(nameof(xElement));
    }

    protected override int TextHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (_, _, isReverse, _) = GetTextAttributes(xElement);
        var (fontNumber, fontWidth, fontHeight) = GetFontInfo(xElement);
        var cmd = string.Format(CultureInfo.InvariantCulture,
            "TEXT {0},{1},\"{2}\",0,1,1,\"{3}\"{4}",
            LabelMarginHor, topPos, fontNumber, EscapeChars(xElement.Value),
            LineDelimiter);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));

        if (isReverse)
        {
            var left = Math.Max(0, LabelMarginHor - 2);
            var top = Math.Max(0, topPos - 1);
            cmd = string.Format(CultureInfo.InvariantCulture,
                "REVERSE {0},{1},{2},{3}{4}",
                left, topPos - 1, 
                Math.Min(LabelWidth, xElement.Value.Trim().Length * (fontWidth + 2) + (LabelMarginHor-left)),
                Math.Min(LabelHeight, fontHeight + 1 + topPos - top),
                LineDelimiter);
            buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        }
        return fontHeight + 2;
    }

    protected override int BarcodeHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (barcodeHeight, isHri, xPos, alignmentValue) = GetBarcodeAttributes(xElement, true, DefaultBarcodeHeightRatio);

        //workaround - подпись к штрихкоду смещена влева на 1 символ
        if (alignmentValue is 0 or 1)
            xPos += (Font0Width + Font1Width) / 2;

        // Добавляем штрихкод (EAN13)                    
        var cmd = string.Format(CultureInfo.InvariantCulture,
            "BARCODE {0},{1},\"EAN13\",{2},{3},0,2,2,{4},\"{5}\"{6}",
            xPos,
            topPos + BarcodeMargin,
            isHri ? barcodeHeight - Font1Height - 1 : barcodeHeight - 1,
            isHri ? 2 : 0,
            alignmentValue,
            EscapeChars(xElement.Value), LineDelimiter);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));

        return BarCodeHeight + 2 * BarcodeMargin;
    }

    protected override int QrCodeHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        int ToModuleSize(string size)
        {
            return size switch
            {
                "tiny" => 3,
                "small" => 4,
                "normal" => 6,
                "large" => 8,
                "extralarge" => 10,
                _ => 6
            };
        }

        const int verticalMargin = 2;
        var (moduleSize, correctionLevel, qrSizeInDots, xPos) = GetQrCodeAttributes(xElement, ToModuleSize);

        var cmd = string.Format(CultureInfo.InvariantCulture,
            "QRCODE {0},{1},{2},{3},A,0,M2,\"{4}\"{5}",
            xPos, topPos + 1, correctionLevel, moduleSize, xElement.Value, LineDelimiter);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));

        return qrSizeInDots + 2 * verticalMargin;
    }

    protected override int PagecutHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        buffer.AddRange(PrinterEncoding.GetBytes($"PRINT 1{LineDelimiter}"));
        buffer.AddRange(PrinterEncoding.GetBytes($"CLS{LineDelimiter}"));
        return 0;
    }

    protected override int ImageHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (bitmap, xPos) = GetImageAttributes(xElement, topPos);
        if (bitmap == null)
            return 0;

        var (imageData, widthInBytes) = ImageHelper.ImageToByteArray(bitmap);

        var cmd = string.Format(CultureInfo.InvariantCulture,
            "BITMAP {0},{1},{2},{3},0,",
            xPos, topPos, widthInBytes, bitmap.Height);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        buffer.AddRange(imageData);
        buffer.AddRange(PrinterEncoding.GetBytes($"{LineDelimiter}"));

        return bitmap.Height;
    }

    private static string EscapeChars(string source)
    {
        return source
            .TrimEnd(' ', '\t')
            .Replace("\"", "\\[\"]");
    }
}