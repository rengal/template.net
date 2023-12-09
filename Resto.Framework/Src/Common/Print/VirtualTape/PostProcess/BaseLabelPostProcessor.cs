using System;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using Resto.Framework.Common.Print.Tags;
using log4net;
using System.Drawing;
using System.Globalization;
using Resto.Framework.Common.Helpers;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

/// <summary>
/// Постпроцессор ленты преобразующий строки ленты в команды Epl2
/// </summary>
public abstract class BaseLabelPostProcessor : BaseTapePostProcessor
{
    #region Inner Types
    
    private delegate int TagHandler(XElement xElement, List<byte> buffer, int topPos);
    
    #endregion

    protected static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(BaseTapePostProcessor));

    /// <summary>
    /// Вернуть ширину закрашиваемой полосы штрихкода в точках
    /// </summary>
    public int NarrowBarWidth { get; set; }

    /// <summary>
    /// Отступ для barcode и qrCode по вертикали от соседних элементов
    /// </summary>
    public int BarcodeMargin { get; set; }

    /// <summary>
    /// Отношение высоты штрихкода к его ширине по умолчанию
    /// </summary>
    protected double DefaultBarcodeHeightRatio { get; set; }

    /// <summary>
    /// Вернуть высоту штрихкода в точках
    /// </summary>
    protected int BarCodeHeight { get; set; }
    public int Font0Number { get; set; }
    public int Font1Number { get; set; }
    public int Font2Number { get; set; }

    /// <summary>
    /// Высота в точках шрифта f0
    /// </summary>
    public int Font0Height { get; set; }

    /// <summary>
    /// Высота в точках шрифта f1
    /// </summary>
    public int Font1Height { get; set; }

    /// <summary>
    /// Высота в точках шрифта f2
    /// </summary>
    public int Font2Height { get; set; }

    /// <summary>
    /// Вернуть\установить ширину этикетки в точках 
    /// </summary>
    public int LabelWidth { get; set; }

    /// <summary>
    /// Вернуть\установить высоту этикетки в точках 
    /// </summary>
    public int LabelHeight { get; set; }

    /// <summary>
    /// Вернуть\установить отступ горизонтальный этикетки в точках 
    /// </summary>
    public int LabelMarginHor { get; set; }

    /// <summary>
    /// Вернуть\установить отступ вертикальный этикетки в точках 
    /// </summary>
    public int LabelMarginVert { get; set; }

    public int Dpi { get; set; }

    public Encoding PrinterEncoding { get; set; }

    /// <summary>
    /// Вернуть/установить текущую ленту
    /// </summary>
    /// <remarks>Инициализируется в <see cref="M:Process"/></remarks>
    protected ITape Tape { get; set; }

    private readonly Dictionary<string, TagHandler> tagHandlers;

    protected BaseLabelPostProcessor()
    {
        NarrowBarWidth = 2;
        DefaultBarcodeHeightRatio = 0.3;
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

    /// <summary>
    /// Обработать ленту и вернуть форматированную понятную для данного принтера строку 
    /// </summary>
    /// <param name="tape">Обрабатываемая лента</param>
    /// <returns>Сформированная для отправки принтеру строка</returns>
    public override IEnumerable<(XElement document, string text)> Process(ITape tape)
    {
        if (tape == null)
            throw new ArgumentNullException(nameof(tape));

        Tape = tape;
        BarCodeHeight = Math.Max(LabelHeight / 4, 80);
        var buffer = new List<byte>(128);

        var xDocument = new XElement(TagDoc.Instance.Name);

        // Обрабатываем документ
        // Текущая верхняя координата линии
        var currentPosY = LabelMarginVert;

        var tapeLines = tape.GetLines();

        StartLabelHandler(buffer, tapeLines);

        for (var i = 0; i < tapeLines.Count; i++)
        {
            var tapeLine = tapeLines[i];
            xDocument.Add(tapeLine.Element);

            var isPagecut = tapeLine.Element.Name.LocalName.Equals(TagPagecut.Instance.Name,
                StringComparison.InvariantCultureIgnoreCase);
            if (!tagHandlers.TryGetValue(tapeLine.Element.Name.LocalName, out var handler))
                Log.Warn($"Failed to process tag '{tapeLine.Element.Name.LocalName}'");
            else
            {
                var isLastElement = i + 1 == tapeLines.Count;
                if (!isLastElement || !isPagecut)
                    currentPosY += handler(tapeLine.Element, buffer, currentPosY);
            }

            if (isPagecut)
                currentPosY = LabelMarginVert;
        }

        return EndLabelHandler(buffer, xDocument);
    }

    protected (bool isItalic, bool isBold, bool isReverse, bool isUnderline) GetTextAttributes(XElement xElement)
    {
        var isItalic = string.Equals(xElement.Attribute("italic")?.Value, "on",
            StringComparison.InvariantCultureIgnoreCase);
        var isBold = string.Equals(xElement.Attribute("bold")?.Value, "on",
            StringComparison.InvariantCultureIgnoreCase);
        var isReverse = string.Equals(xElement.Attribute("reverse")?.Value, "on",
            StringComparison.InvariantCultureIgnoreCase);
        var isUnderLine = string.Equals(xElement.Attribute("underline")?.Value, "on",
            StringComparison.InvariantCultureIgnoreCase);

        return (isItalic, isBold, isReverse, isUnderLine);
    }

    protected (int height, bool isHri, int xPos, int alignmentValue) GetBarcodeAttributes(XElement xElement, bool useNativeAlignment, double defaultHeigthRatio)
    {
        var alignAttr = xElement.Attribute("align")?.Value ?? "center";
        
        var heightRatioStr = xElement.Attribute("heightRatio")?.Value;
        if (string.IsNullOrEmpty(heightRatioStr) || !double.TryParse(heightRatioStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var heightRatio))
            heightRatio = defaultHeigthRatio;
        
        var hriStr = xElement.Attribute("hri")?.Value;
        var isHri = !string.Equals(hriStr, "off", StringComparison.InvariantCultureIgnoreCase);

        var barcodeWidth = ImageHelper.CalcEan13BarCodeWidth(xElement.Value, NarrowBarWidth);
        var xPos = useNativeAlignment
            ? alignAttr switch
            {
                "center" => LabelWidth / 2,
                "right" => Math.Max(0, LabelWidth - 2 * LabelMarginHor - 1),
                _ => LabelMarginHor
            }
            : alignAttr switch
            {
                "center" => LabelWidth / 2 - barcodeWidth / 2,
                "right" => Math.Max(0, LabelWidth - barcodeWidth - 2 * LabelMarginHor - 1),
                _ => LabelMarginHor
            };

        var alignmentValue = useNativeAlignment
            ? alignAttr switch
            {
                "center" => 2,
                "right" => 3,
                _ => 1
            }
            : 1;
        var height = (int)(barcodeWidth * heightRatio);
        return (height, isHri, xPos, alignmentValue);
    }

    protected (int moduleSize, char correctionLevel, int sizeInDots, int xPos) GetQrCodeAttributes(XElement xElement, Func<string, int> sizeToModuleFunc)
    {
        var alignAttr = xElement.Attribute("align")?.Value ?? "center";
        var sizeAttr = xElement.Attribute("size")?.Value ?? "normal";
        var moduleSize = sizeToModuleFunc(sizeAttr);
        var correction = xElement.Attribute("correction")?.Value ?? "medium";
        var correctionLevel = correction switch
        {
            "low" => 'L',
            "medium" => 'M',
            "high" => 'Q',
            "ultra" => 'H',
            _ => 'M'
        };

        var qrSizeInDots = moduleSize * ImageHelper.CalculateQRCodeModulesCount(xElement.Value, correctionLevel);

        var xPos = alignAttr switch
        {
            "center" => Math.Max(0, (LabelWidth - qrSizeInDots) / 2),
            "right" => Math.Max(0, LabelWidth - 2 * LabelMarginHor - qrSizeInDots),
            _ => LabelMarginHor
        };

        return (moduleSize, correctionLevel, qrSizeInDots, xPos);
    }

    protected (Bitmap bitmap, int xPos) GetImageAttributes(XElement xElement, int topPos)
    {
        var align = xElement.Attribute("align")?.Value ?? "center";
        var resizeMode = xElement.Attribute("resizeMode")?.Value ?? "clip";
        if (topPos >= LabelHeight)
            return (null, 0);
        return ImageHelper.CreateBitmapFromBase64(xElement.Value, LabelWidth, LabelHeight - topPos, align, resizeMode, false);
    }

    protected abstract void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines);
    protected abstract IEnumerable<(XElement, string)> EndLabelHandler(List<byte> buffer, XElement document);
    protected abstract int TextHandler(XElement xElement, List<byte> buffer, int topPos);
    protected abstract int BarcodeHandler(XElement xElement, List<byte> buffer, int topPos);
    protected abstract int QrCodeHandler(XElement xElement, List<byte> buffer, int topPos);
    protected abstract int PagecutHandler(XElement xElement, List<byte> buffer, int topPos);
    protected abstract int ImageHandler(XElement xElement, List<byte> buffer, int topPos);
}
