using System.Globalization;
using System.Linq;

namespace Resto.Framework.Common.Print.TextBlockFormatters
{
    public sealed class SplitFormatter : ITextBlockFormatter
    {
        private const int LineStartDefault = 0;
        private const int WhiteSpaceDefaultIndex = -1;

        private static readonly char[] NoBreakSpace;

        private int CharIndex { get; set; }
        private int TapeLineLength { get; set; }
        private int LineStart { get; set; }
        private ITextBlock TextBlock { get; set; }
        private string Text { get; set; }
        private int WhiteSpaceSequenceStart { get; set; }
        private int WhiteSpaceSequenceEnd { get; set; }
        private int Position { get; set; }

        private bool IsRemainLengthExceeded
        {
            get { return CharIndex - LineStart >= RemainTextLength; }
        }

        private bool IsWordExceededTextLineLength
        {
            get { return CharIndex - WhiteSpaceSequenceEnd > TextLineLength; }
        }

        private bool IsLastChar
        {
            get { return CharIndex + 1 == Text.Length; }
        }

        private bool IsLineOfSpacesBeforeCurrentChar
        {
            get { return WhiteSpaceSequenceStart <= LineStart && WhiteSpaceSequenceEnd == CharIndex - 1; }
        }

        private bool IsCurrentCharAfterSpaces
        {
            get { return WhiteSpaceSequenceEnd == CharIndex - 1; }
        }

        private int TextLineLength
        {
            get;
            set;
        }

        private int RemainTextLength
        {
            get { return TextLineLength - Position; }
        }

        static SplitFormatter()
        {
            NoBreakSpace = new[] { '\u00A0', '\u202F', '\u2060', '\uFEFF' };
        }

        public void Format(ITextBlock textBlock, string text, int startPosition, int lineLength)
        {
            TapeLineLength = lineLength;
            TextBlock = textBlock;
            Text = text;
            LineStart = LineStartDefault;
            WhiteSpaceSequenceStart = WhiteSpaceDefaultIndex;
            WhiteSpaceSequenceEnd = WhiteSpaceDefaultIndex;

            TextLineLength = CalculateTextLineLength();
            Position = startPosition;

            for (CharIndex = 0; CharIndex < Text.Length; CharIndex++)
            {
                HandleChar();
            }
            EnsureLastLineAppended();
        }

        private void BreakLine()
        {
            if (IsWordExceededTextLineLength)
            {
                TextBlock.Append(Text, LineStart, TextLineLength);
                HandleRN();
                LineStart = CharIndex;
            }
            else if (WhiteSpaceSequenceStart >= 0)
            {
                if (WhiteSpaceSequenceStart > LineStart)
                {
                    TextBlock.Append(Text, LineStart, WhiteSpaceSequenceStart - LineStart);
                }
                LineStart = WhiteSpaceSequenceEnd + 1;
            }
            NewLine();
        }

        private void NewLine()
        {
            TextBlock.NewLine();
            Position = 0;
            TextLineLength = CalculateTextLineLength();
        }

        private void EnsureLastLineAppended()
        {
            if (Text.Length == LineStart) { return; }

            if (WhiteSpaceSequenceEnd != Text.Length - 1 || LineStart == WhiteSpaceSequenceStart)
            {
                TextBlock.Append(Text, LineStart, Text.Length - LineStart);
            }
            else
            {
                if (WhiteSpaceSequenceStart > LineStart)
                    TextBlock.Append(Text, LineStart, WhiteSpaceSequenceStart - LineStart);
            }
        }

        private void HandleChar()
        {
            if (IsLineSeparator(CharIndex))
            {
                var lineLengthBeforeLineBreak = GetLineLengthBeforeLineBreak();
                if (!IsLineOfSpacesBeforeCurrentChar) { TextBlock.Append(Text, LineStart, lineLengthBeforeLineBreak); }
                HandleRN();
                LineStart = CharIndex + 1;
                NewLine();

                if (IsLastChar) { return; }
            }
            if (IsWhiteSpace(CharIndex))
            {
                UpdateWhiteSpaceSequence();
            }

            if (IsRemainLengthExceeded)
            {
                BreakLine();
            }
        }

        private int GetLineLengthBeforeLineBreak()
        {
            return IsCurrentCharAfterSpaces ? WhiteSpaceSequenceStart - LineStart : CharIndex - LineStart;
        }

        private void HandleRN()
        {
            if (Text[CharIndex] == '\r' && !IsLastChar && Text[CharIndex + 1] == '\n')
            {
                CharIndex++;
            }
        }

        private bool IsLineSeparator(int index)
        {
            var c = Text[index];
            return char.GetUnicodeCategory(c) == UnicodeCategory.LineSeparator || c == '\r' || c == '\n';
        }

        private int CalculateTextLineLength()
        {
            var lengthOfText = 0;
            var remainTapeLineLength = TapeLineLength - Position;
            for (var i = LineStart; i < Text.Length; i++)
            {
                var c = Text[i];
                var charWidth = TextBlock.GetCharWidth(c);
                if (charWidth > remainTapeLineLength)
                {   //Сурогатные символы не разъединяем  
                    if (i != 0 && char.IsSurrogatePair(Text, i - 1))
                        lengthOfText--;
                    return lengthOfText;
                }
                remainTapeLineLength -= charWidth;
                lengthOfText++;
            }
            return lengthOfText + remainTapeLineLength;
        }

        private bool IsWhiteSpace(int index)
        {
            var c = Text[index];
            return char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator && !NoBreakSpace.Contains(c);
        }

        private void UpdateWhiteSpaceSequence()
        {
            WhiteSpaceSequenceStart = CharIndex;
            while (CharIndex + 1 < Text.Length && IsWhiteSpace(CharIndex + 1))
            {
                CharIndex++;
            }
            WhiteSpaceSequenceEnd = CharIndex;
        }
    }
}
