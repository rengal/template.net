using System.Text.RegularExpressions;

namespace Resto.Data
{
    public partial class EgaisMark
    {
        public static readonly Regex validSymbolsRegex = new Regex("^[a-zA-Z0-9]*$");

        public bool HasInvalidSymbols
        {
            get { return !validSymbolsRegex.IsMatch(Mark); }
        }

        public override int GetHashCode()
        {
            return mark != null ? mark.GetHashCode() : 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EgaisMark;
            return other != null && mark == other.mark;
        }
    }
}
