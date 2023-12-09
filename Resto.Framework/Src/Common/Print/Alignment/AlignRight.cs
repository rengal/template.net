using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public sealed class AlignRight : AlignBase
    {
        public static readonly AlignRight Instance;

        private AlignRight() { }

        static AlignRight()
        {
            Instance = new AlignRight();
        }

        public override void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider)
        {
            var lineLengthOnTape = GetCharsWidth(line, charWidthProvider);
            Fill(line, 0, tapeCurrentWidth - lineLengthOnTape, fillSymbols, charWidthProvider);
        }
    }
}
