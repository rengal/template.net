using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Resto.Framework.Common.Print.VirtualTape;

namespace Resto.Framework.Common.Print.Alignment
{
    public sealed class AlignJustify : AlignBase
    {
        private const string Space = " ";

        public static readonly AlignJustify Instance;
        private static readonly char[] NoBreakSpace;

        private AlignJustify() { }

        static AlignJustify()
        {
            Instance = new AlignJustify();
            NoBreakSpace = new[] { '\u00A0', '\u202F', '\u2060', '\uFEFF' };
        }
        
        public override void Format(StringBuilder line, int tapeCurrentWidth, string fillSymbols, ICharWidthProvider charWidthProvider)
        {
            var spacePositions = GetSpacePositions(line);
            if (spacePositions.Count == 0)
            {
                AlignLeft.Instance.Format(line, tapeCurrentWidth, fillSymbols, charWidthProvider);
            }
            else
            {
                var lineLength = GetCharsWidth(line, charWidthProvider);
                var spaceWidth = GetCharsWidth(Space, charWidthProvider);

                var spacesToAdd = (tapeCurrentWidth - lineLength)/(spacePositions.Count * spaceWidth);
                if (spacesToAdd == 0)
                    return;

                var needAdditionalSpace = (tapeCurrentWidth - lineLength - spacePositions.Count * spaceWidth * spacesToAdd) / spaceWidth;
                for (var i = spacePositions.Count - 1; i >= spacePositions.Count - needAdditionalSpace; i--)
                {
                    line.Insert(spacePositions[i], Space, spacesToAdd + 1);
                }
                for (var i = spacePositions.Count - needAdditionalSpace - 1; i >= 0; i--)
                {
                    line.Insert(spacePositions[i], Space, spacesToAdd);
                }
            }
        }

        private static List<int> GetSpacePositions(StringBuilder line)
        {
            var spaces = new List<int>();
            for (var i = 0; i < line.Length; i++)
                if (IsWhiteSpace(line[i]))
                    spaces.Add(i);

            return spaces;
        }

        private static bool IsWhiteSpace(char c)
        {
            return char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator && !NoBreakSpace.Contains(c);
        }
    }
}
