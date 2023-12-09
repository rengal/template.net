using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Resto.Framework.Common.Print.Alignment;
using Resto.Framework.Common.Print.Tags;
using Resto.Framework.Common.Print.TextBlockFormatters;
using Resto.Framework.Common.Print.VirtualTape.Fonts;

namespace Resto.Framework.Common.Print.VirtualTape
{
    public interface ITape
    {
        IAlign Align { get; }
        Font CurrentFont { get; }
        FontGlyphFlags CurrentFontGlyphFlags { get; }
        string FillSymbols { get; set; }
        FontPack Fonts { get; }
        ICharWidthProvider CharWidthProvider { get; }
        ITextBlockFormatter TextBlockFormatter { get; set; }
        int TotalLength { get; }
        bool RightToLeftSupported { get; }

        void AppendTextBlock(string s);
        void AppendLine(XElement xElement, string content, string raw);
        void Print(XmlNode node, Dictionary<string, ITag> tags);
        void Print(XNode node, Dictionary<string, ITag> tags);

        /// <summary>
        /// Первый компонент пары каждого элемента списка - соответствующий текст строки.
        /// Второй компонент пары - команды принтера (Esc-последовательность), выполняющаяся после печати этой строки и не связанная 
        /// с форматированием текста в чеке - например, открытие ДЯ. Соответствующие тэги игнорируются при форматировании и выравнивании
        /// текста, Esc-последовательность передается в принтер в неизменном виде.
        /// </summary>
        List<DocumentLine> GetLines();

        void NewLine();
        void NewParagraph();
        void Pagecut();
        void SetAlign(IAlign align);
        void SetFont(Font font);
        void SetFontGlyphFlags(FontGlyphFlags fontGlyphFlags);
        void AppendLineWithoutFormatting(XElement element, string text);
        void StartNewLineWithFontEsc();
    }
}