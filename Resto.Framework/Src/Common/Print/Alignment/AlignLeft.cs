using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public sealed class AlignLeft : AlignBase
    {
        public static readonly AlignLeft Instance;

        private AlignLeft() { }

        static AlignLeft()
        {
            Instance = new AlignLeft();
        }

        public override void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider)
        {
            var lineLength = line.Length;
            var lineLengthOnTape = GetCharsWidth(line, charWidthProvider);
            Fill(line, lineLength, tapeCurrentWidth - lineLengthOnTape, fillSymbols, charWidthProvider);
        }
    }
}
