namespace Resto.Framework.Common.Print.TextBlockFormatters
{
    public sealed class CutFormatter : ITextBlockFormatter
    {
        public void Format(ITextBlock textBlock, string text, int position, int lineLength)
        {
            var tapeLength = 0;
            var textLength = 0;
            if (lineLength == position)
            {
                textBlock.NewLine();
                for (var i = 0; i < text.Length; i++)
                {
                    var charWidth = textBlock.GetCharWidth(text[i]);
                    if (tapeLength + charWidth > lineLength)
                    {
                        //Сурогатные символы не разъединяем  
                        if (i != 0 && char.IsSurrogatePair(text, i - 1))
                            textLength--;
                        break;
                    }
                    tapeLength += charWidth;
                    textLength++;
                }
            }
            else
            {
                for (var i = 0; i < text.Length; i++)
                {
                    var charWidth = textBlock.GetCharWidth(text[i]);
                    if (tapeLength + charWidth > lineLength - position)
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
            textBlock.Append(text, 0, textLength);
        }
    }
}
