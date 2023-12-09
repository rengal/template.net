
namespace Resto.Framework.Common.Print.TextBlockFormatters
{
    public sealed class WrapFormatter : ITextBlockFormatter
    {
        public void Format(ITextBlock textBlock, string text, int position, int lineLength)
        {
            var start = 0;
            var tapeLength = 0;
            var textLength = 0;
            for (var i = 0; i < text.Length; i++)
            {
                var charWidth = textBlock.GetCharWidth(text[i]);
                if (tapeLength + charWidth > lineLength - position)
                {
                    //Сурогатные символы не разъединяем  
                    if (i != 0 && char.IsSurrogatePair(text, i - 1))
                        textLength--;
                    break;
                }
                tapeLength += charWidth;
                textLength++;
            }

            while (start < text.Length)
            {
                if (start > 0 || tapeLength == 0)
                {
                    textBlock.NewLine();
                    tapeLength = 0;
                    textLength = 0;
                    for (var i = start; i < text.Length && tapeLength < lineLength; i++)
                    {
                        var charWidth = textBlock.GetCharWidth(text[i]);
                        if (tapeLength + charWidth > lineLength)
                        {
                            //Сурогатные символы не разъединяем  
                            if (char.IsSurrogatePair(text, i - 1))
                                textLength--;
                            break;
                        }
                        tapeLength += charWidth;
                        textLength++;
                    }
                }
                textBlock.Append(text, start, textLength);
                start += textLength;
            }
        }
    }
}
