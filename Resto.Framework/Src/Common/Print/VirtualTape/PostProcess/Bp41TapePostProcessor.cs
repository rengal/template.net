using System.Collections.Generic;
using System.Xml.Linq;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

public class Bp41TapePostProcessor : TsplTapePostProcessor
{
    protected override string LineDelimiter => "\r\n";

    protected override void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines)
    {
        buffer.AddRange(PrinterEncoding.GetBytes($"SIZE {LabelWidthInMm} mm, {LabelHeightInMm} mm{LineDelimiter}"));
        buffer.AddRange(PrinterEncoding.GetBytes($"CLS{LineDelimiter}"));
    }

    protected override IEnumerable<(XElement, string)> EndLabelHandler(List<byte> buffer, XElement xDocument)
    {
        var result = new List<(XElement, string)>
        {
            //Для BP41 команду PRINT отправляем в отдельной таске
            (xDocument, PrinterEncoding.GetString(buffer.ToArray())),
            (null, $"PRINT 1{LineDelimiter}")
        };
        return result;
    }
}
