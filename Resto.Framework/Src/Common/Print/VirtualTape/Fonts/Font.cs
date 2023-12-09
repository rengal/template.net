namespace Resto.Framework.Common.Print.VirtualTape.Fonts
{
    public class Font
    {
        public string Esc { get; set; }
        public int Width { get; set; }
        public string Id { get; internal set; }
        public delegate string ReplaceUnsupportedCharsDelegate(string text);
        public ReplaceUnsupportedCharsDelegate ReplaceUnsupportedChars { get; set; }
        
        public Font()
        {
            Esc = string.Empty;
        }

        public Font Clone()
        {
            return new Font
            {
                Id = Id, Esc = Esc, Width = Width, ReplaceUnsupportedChars = ReplaceUnsupportedChars
            };
        }

        public sealed override string ToString()
        {
            return $"{GetType().Name}: Esc: {Esc}, Width: {Width}";
        }
    }
}