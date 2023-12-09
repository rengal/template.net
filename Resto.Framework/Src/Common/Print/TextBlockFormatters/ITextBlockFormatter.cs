namespace Resto.Framework.Common.Print.TextBlockFormatters
{
    public interface ITextBlockFormatter
    {
        void Format(ITextBlock textBlock, string text, int startPosition, int lineLength);
    }
}
