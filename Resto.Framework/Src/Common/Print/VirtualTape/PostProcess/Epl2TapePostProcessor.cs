using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;
using Resto.Framework.Common.Print.Tags;
using System.Text.RegularExpressions;
using Resto.Framework.Common.Helpers;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

/// <summary>
/// Постпроцессор ленты преобразующий строки ленты в команды Epl2
/// </summary>
public class Epl2TapePostProcessor : BaseLabelPostProcessor
{
    private bool hasCyrillicLetters;

    private (int fontNumber, int Height) GetEpl2FontInfo(XElement xElement)
    {
        if (xElement.Name.LocalName.Equals(TagFont.F0.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font0Number, Font0Height);

        if (xElement.Name.LocalName.Equals(TagFont.F1.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font1Number, Font1Height);

        if (xElement.Name.LocalName.Equals(TagFont.F2.Name, StringComparison.InvariantCultureIgnoreCase))
            return (Font2Number, Font2Height);

        throw new ArgumentException(nameof(xElement));
    }

    protected override void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines)
    {
        //RMS-54441 workaround: если в тексте есть двойные кавычки и кириллица - ломается кодировка. Поэтому если найдена кириллица заменяем двойные кавычки на одинарные
        hasCyrillicLetters = lines.Any(tapeLine => Regex.IsMatch(tapeLine.Content, @"\p{IsCyrillic}"));

        // Очищаем графический буфер принтера 
        buffer.AddRange(PrinterEncoding.GetBytes("N\n"));

        // - Устанавливаем необходимую кодировку (windows-1251)
        buffer.AddRange(PrinterEncoding.GetBytes("I8,C\n"));

        if (PrintOrientation == PrintOrientation.TopToBottom)
        {
            // - Устанавливаем направление печати (сверху вниз)
            buffer.AddRange(PrinterEncoding.GetBytes("ZT\n"));
        }
        else if (PrintOrientation == PrintOrientation.BottomToTop)
        {
            // - Устанавливаем направление печати (снизу вверх)
            buffer.AddRange(PrinterEncoding.GetBytes("ZB\n"));
        }

        //Устанавливаем размер этикетки
        buffer.AddRange(PrinterEncoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "q{0}\nQ{1},26\n",
            LabelWidth, LabelHeight)));
    }

    protected override IEnumerable<(XElement, string)> EndLabelHandler(List<byte> buffer, XElement xDocument)
    {
        // Добавляем команду печати этикетки (1 копию)
        buffer.AddRange(PrinterEncoding.GetBytes("P1\n"));
        return new[] { (xDocument, PrinterEncoding.GetString(buffer.ToArray())) };
    }

    protected override int TextHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (_, _, isReverse, _) = GetTextAttributes(xElement);
        var (fontNumber, height) = GetEpl2FontInfo(xElement);
        // Удаляем пробельные символы в конце 
        var text = xElement.Value.TrimEnd(' ', '\t');
        //Если есть кириллица - заменяем " на ', иначе происходят ошибки с кодировкой (RMS-54441)
        if (hasCyrillicLetters)
            text = text.Replace('"', '\'');
        // меняем \ на \\ 
        text = text.Replace("\\", "\\\\");
        // меняем " на \"
        text = text.Replace("\"", "\\\"");
        var cmd = string.Format(CultureInfo.InvariantCulture, "A{0},{1},0,{2},1,{3},{4},\"{5}\"\n",
            LabelMarginHor,
            topPos,
            fontNumber,
            1,
            isReverse ? 'R' : 'N',
            text);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        return height + 2;
    }

    protected override int BarcodeHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (barcodeHeight, isHri, xPos, _) = GetBarcodeAttributes(xElement, false, DefaultBarcodeHeightRatio);

        var cmd = string.Format(CultureInfo.InvariantCulture, "B{0},{1},0,E30,{2},0,{3},{4},\"{5}\"\n",
            xPos,
            topPos + BarcodeMargin,
            NarrowBarWidth,
            isHri ? barcodeHeight - Font1Height - BarcodeMargin : barcodeHeight - 3,
            isHri ? 'B' : 'N',
            xElement.Value);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        return barcodeHeight + 2 * BarcodeMargin;
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
            "b{0},{1},Q,m2,s{2},e{3},\"{4}\"\n",
            xPos, topPos, moduleSize, correctionLevel, xElement.Value);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));

        return qrSizeInDots + 2 * verticalMargin;
    }

    protected override int PagecutHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        buffer.AddRange(PrinterEncoding.GetBytes("P1\n"));
        buffer.AddRange(PrinterEncoding.GetBytes("\nN\n"));
        return 0;
    }

    protected override int ImageHandler(XElement xElement, List<byte> buffer, int topPos)
    {
        var (bitmap, xPos) = GetImageAttributes(xElement, topPos);
        if (bitmap == null)
            return 0;

        var (imageData, widthInBytes) = ImageHelper.ImageToByteArray(bitmap);

        var cmd = string.Format(CultureInfo.InvariantCulture, "GW{0},{1},{2},{3},", xPos, topPos, widthInBytes,
            bitmap.Height);
        buffer.AddRange(PrinterEncoding.GetBytes(cmd));
        buffer.AddRange(imageData);
        buffer.AddRange(PrinterEncoding.GetBytes("\n"));

        return bitmap.Height;
    }
}
