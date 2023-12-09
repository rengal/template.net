using System.Collections.Generic;

namespace Resto.Framework.Common.Print.VirtualTape.PostProcess;

public class Bp21TapePostProcessor : TsplTapePostProcessor
{
    protected override void StartLabelHandler(List<byte> buffer, List<DocumentLine> lines)
    {
        buffer.AddRange(PrinterEncoding.GetBytes($"CLS{LineDelimiter}"));
    }

}
