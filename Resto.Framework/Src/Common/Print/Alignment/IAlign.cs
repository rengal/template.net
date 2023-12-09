using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public interface IAlign
    {
        void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider);
    }
}
