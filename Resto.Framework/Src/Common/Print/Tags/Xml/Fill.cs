using System.Xml.Linq;

namespace Resto.Framework.Common.Print.Tags.Xml
{
    /// <summary>Заполнение пустого пространства символами (по умолчанию '-').</summary>
    public class Fill : XElement
    {
        protected XAttribute symbolsAttribute;

        protected Fill(string name, string symbol)
            : base(name)
        {
            addSymbolAttribute(symbol);
        }

        public Fill(string symbol)
            : base(TagFill.Fill.Name)
        {
            addSymbolAttribute(symbol);
        }

        public Fill(string symbol, object content)
            : base(TagFill.Fill.Name, content)
        {
            addSymbolAttribute(symbol);
        }

        public Fill(string symbol, params object[] content)
            : base(TagFill.Fill.Name, content)
        {
            addSymbolAttribute(symbol);
        }

        private void addSymbolAttribute(string value)
        {
            Add(symbolsAttribute = new XAttribute(TagFill.SYMBOLS_ATTRIBUTE, value));
        }
    }
}
