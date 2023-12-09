using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public sealed class AlignCenter : AlignBase
    {
        public static readonly AlignCenter Instance;

        private AlignCenter() { }

        static AlignCenter()
        {
            Instance = new AlignCenter();
        }

        public override void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider)
        {
            var lineLengthOnTape = GetCharsWidth(line, charWidthProvider);

            var leftLength = (tapeCurrentWidth - lineLengthOnTape) / 2;
            var leftStart = 0;
            Fill(line, leftStart, leftLength, fillSymbols, charWidthProvider);

            var rigthLength = tapeCurrentWidth - lineLengthOnTape - leftLength;
            var rightStart = line.Length;
            Fill(line, rightStart, rigthLength, fillSymbols, charWidthProvider);
        }
    }
}
