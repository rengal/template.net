namespace Resto.Framework.Common.Print.TextBlockFormatters
{
    public interface ITextBlock
    {
        void Append(string text, int start, int width);
        void NewLine();
        int GetCharWidth(char c);
    }
}
