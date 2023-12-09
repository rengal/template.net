using System.Linq;
using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public abstract class AlignBase : IAlign
    {
        public abstract void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider);

        protected static void Fill(StringBuilder line, int start, int length, string fillSymbols, ICharWidthProvider charWidthProvider)
        {
            var fillSymbolsTapeLength = GetCharsWidth(fillSymbols, charWidthProvider);
            if (length < fillSymbolsTapeLength)
                return;
            
            var count = length/fillSymbolsTapeLength;
            line.Insert(start, fillSymbols, count);
            start += count*fillSymbols.Length;
            length -= count*fillSymbolsTapeLength;

            var lastPartLength = 0;
            foreach (var t in fillSymbols)
            {
                length -= charWidthProvider.GetCharWidth(t);
                if (length < 0)
                    break;
                lastPartLength++;
            }
            line.Insert(start, fillSymbols.Substring(0, lastPartLength));
        }

        protected static int GetCharsWidth(string s, ICharWidthProvider charWidthProvider)
        {
            return s.Sum(c => charWidthProvider.GetCharWidth(c));
        }
        
        protected static int GetCharsWidth(StringBuilder sb, ICharWidthProvider charWidthProvider)
        {
            var l = 0;
            for (var i = 0; i < sb.Length; i++)
                l += charWidthProvider.GetCharWidth(sb[i]);
            return l;
        }
    }
}
