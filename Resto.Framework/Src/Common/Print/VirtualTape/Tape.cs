using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.Print.Alignment;
using Resto.Framework.Common.Print.Tags;
using Resto.Framework.Common.Print.TextBlockFormatters;
using Resto.Framework.Common.Print.VirtualTape.Fonts;
using Resto.Framework.Properties;

namespace Resto.Framework.Common.Print.VirtualTape
{
    public partial class Tape : ITape, ITextBlock
    {
        private static readonly Lazy<XslCompiledTransform> docPreProcess = new Lazy<XslCompiledTransform>(
            () =>
            {
                var temp = new XslCompiledTransform();
                temp.Load(XmlReader.Create(new StringReader(ResourceFile.DocPreprocess)));
                return temp;
            });

        private static XslCompiledTransform DocPreProcess
        {
            get { return docPreProcess.Value; }
        }

        private const string EmptyFillSymbols = "Fill symbols cannot be empty";
        private const string FontOutOfRange = "Font doesn't exist in current FontPack";
        private const string NullFillSymbols = "Fill symbols cannot be null";
        private const string Space = " ";

        /// <summary>
        /// Текущая обрабатываемая строка. Описание данных см. в классе <see cref="DocumentLine"/>
        /// </summary>
        private readonly StringBuilder currentContent = new StringBuilder();
        private readonly StringBuilder currentRaw = new StringBuilder();

        /// <summary>
        /// Строки в чеке. Описание данных см. в классе <see cref="DocumentLine"/>
        /// </summary>
        private readonly List<DocumentLine> lines;
        private string fillSymbols;

        public IAlign Align { get; private set; }
        public string FillSymbols
        {
            get
            {
                return fillSymbols;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), NullFillSymbols);
                if (value == string.Empty)
                    throw new ArgumentOutOfRangeException(nameof(value), EmptyFillSymbols);

                fillSymbols = value;
            }
        }

        private readonly string pagecutEsc;
        public FontPack Fonts { get; private set; }
        public ICharWidthProvider CharWidthProvider { get; private set; }
        public ITextBlockFormatter TextBlockFormatter { get; set; }

        [NotNull]
        public Font CurrentFont { get; private set; }
        public FontGlyphFlags CurrentFontGlyphFlags { get; private set; }

        public int TotalLength
        {
            get { return SummarizedStoredLinesLength + CurrentLineTextLength + CurrentLineEscLength; }
        }

        public bool RightToLeftSupported { get; private set; }

        protected int StoredLinesCount
        {
            get { return lines.Count; }
        }

        private bool StartLineWithFontEsc { get; set; }
        private int SummarizedStoredLinesLength { get; set; }
        private int MaxLineLength
        {
            get
            {
                return
                    Math.Max(Math.Max(Fonts.Font0.Width, Fonts.Font1.Width), Fonts.Font2.Width) +
                    Math.Max(Math.Max(Fonts.Font0.Esc.Length, Fonts.Font1.Esc.Length), Fonts.Font2.Esc.Length);
            }
        }

        private int Position { get; set; }
        public Tape(FontPack fonts, ICharWidthProvider charWidthProvider, [NotNull] string pagecutEsc, bool isRightToLeftSupported = false)
        {
            if (pagecutEsc == null)
                throw new ArgumentNullException(nameof(pagecutEsc));

            Fonts = fonts;
            CharWidthProvider = charWidthProvider;

            Align = AlignLeft.Instance;
            currentContent.Clear();
            currentRaw.Clear();
            FillSymbols = Space;
            lines = new List<DocumentLine>();
            SummarizedStoredLinesLength = 0;
            TextBlockFormatter = new WrapFormatter();

            SetFontInternal(Fonts.Font0);
            CurrentFontGlyphFlags = new FontGlyphFlags();
            this.pagecutEsc = pagecutEsc;
            RightToLeftSupported = isRightToLeftSupported;
        }

        public virtual void AppendTextBlock(string text)
        {
            TextBlockFormatter.Format(this, text, Position, CurrentFontWidth);
        }

        public void AppendLine(XElement xElement, string content, string raw)
        {
            NewParagraph(true);
            lines.Add(new DocumentLine(xElement, content, raw));
        }

        public void AppendRawEsc(XElement xElement, string raw)
        {
            currentRaw.Append(raw);
        }

        public void AppendLineWithoutFormatting(XElement element, string text)
        {
            NewParagraph(true);
            if (StartLineWithFontEsc)
            {
                lines.Add(new DocumentLine(element, CurrentFont.Esc + text, string.Empty));
                StartLineWithFontEsc = false;
            }
            else
            {
                lines.Add(new DocumentLine(element, text, string.Empty));
            }
        }

        public void StartNewLineWithFontEsc()
        {
            StartLineWithFontEsc = true;
        }

        internal void AppendTextBlock(string format, object arg0)
        {
            AppendTextBlock(string.Format(format, arg0));
        }

        internal void AppendTextBlock(string format, params object[] args)
        {
            AppendTextBlock(string.Format(format, args));
        }

        internal void Clean()
        {
            ClearCurrentLine();
            lines.Clear();
        }

        public void Print(XmlNode node, Dictionary<string, ITag> tags)
        {
            Reset();
            try
            {
                node = PreProcessDocument(node);
                ValidateXml(node.OuterXml);
                tags[node.Name].Format(this, node, tags);
            }
            catch (Exception e)
            {
                var exsb = new StringBuilder();
                exsb.AppendFormat("Cannot print xml: {0}", node.OuterXml).AppendLine()
                    .AppendLine("Font sizes: ")
                    .AppendFormat("    Font0: {0}", Fonts.Font0.Width).AppendLine()
                    .AppendFormat("    Font1: {0}", Fonts.Font1.Width).AppendLine()
                    .AppendFormat("    Font2: {0}", Fonts.Font2.Width).AppendLine()
                    .AppendFormat("    BarCode: {0}", Fonts.BarCode != null && Fonts.BarCode.IsSupported).AppendLine()
                    .AppendFormat("    Logo: {0}", Fonts.Logo != null && Fonts.Logo.IsSupported).AppendLine()
                    .AppendFormat("    QRCode: {0}", Fonts.QRCode != null && Fonts.QRCode.IsSupported).AppendLine()
                    .AppendFormat("    Bold glyph: {0}", CurrentFontGlyphFlags != null && CurrentFontGlyphFlags.IsBold).AppendLine()
                    .AppendFormat("    Italic glyph: {0}", CurrentFontGlyphFlags != null && CurrentFontGlyphFlags.IsItalic).AppendLine()
                    .AppendFormat("    Reverse glyph: {0}", CurrentFontGlyphFlags != null && CurrentFontGlyphFlags.IsReverse).AppendLine()
                    .AppendFormat("    Underline glyph: {0}", CurrentFontGlyphFlags != null && CurrentFontGlyphFlags.IsUnderline).AppendLine();

                exsb.AppendLine("Formatted lines: ");
                foreach (var line in lines)
                {
                    exsb.AppendLine(line.Content);
                }

                exsb.AppendFormat("Current line: {0} //End of current line", currentContent).AppendLine();

                var newEx = new ArgumentOutOfRangeException(exsb.ToString(), e);
                throw newEx;
            }
        }

        private static XmlNode PreProcessDocument(XmlNode node)
        {
            var doc = new XmlDocument();
            using (var writer = doc.CreateNavigator().AppendChild())
                DocPreProcess.Transform(node, null, writer);
            return doc.FirstChild;
        }

        public void Print(XNode node, Dictionary<string, ITag> tags)
        {
            var reader = node.CreateReader();
            var doc = new XmlDocument();
            doc.Load(reader);
            Print(doc.DocumentElement, tags);
        }

        public List<DocumentLine> GetLines()
        {
            var result = new List<DocumentLine>(lines);
            if (!CurrentLineEmpty)
                result.Add(GetFormattedLine());
            return result;
        }

        public void NewLine()
        {
            NewLine(true);
        }

        public int GetCharWidth(char c)
        {
            return CharWidthProvider.GetCharWidth(c);
        }

        private int GetCharsWidth(string s, int start, int width)
        {
            return s.Skip(start).Take(width).Sum(c => GetCharWidth(c));
        }

        public void NewParagraph()
        {
            NewParagraph(false);
        }

        internal void Reset()
        {
            Align = AlignLeft.Instance;
            FillSymbols = Space;
            SummarizedStoredLinesLength = 0;
            TextBlockFormatter = new WrapFormatter();

            ClearCurrentLine();
            SetFont(Fonts.Font0);
            SetFontGlyphFlags(new FontGlyphFlags());
            lines.Clear();
        }

        public void SetAlign(IAlign align)
        {
            NewParagraph(true);
            Align = align;
        }

        public virtual void SetFont([NotNull] Font font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            SetFontInternal(font);
        }

        public virtual void SetFontGlyphFlags(FontGlyphFlags fontGlyphFlags)
        {
            CurrentFontGlyphFlags = fontGlyphFlags;
        }

        private DocumentLine GetFormattedLine()
        {
            var sb = new StringBuilder(currentContent.ToString());
            if (Align is AlignJustify)
                AlignLeft.Instance.Format(sb, CurrentFontWidth, FillSymbols, CharWidthProvider);
            else
                Align.Format(sb, CurrentFontWidth, FillSymbols, CharWidthProvider);

            var contentNoEsc = sb.ToString();
            if (StartLineWithFontEsc)
                sb.Insert(0, CurrentFont.Esc);

            var element = new XElement(CurrentFont.Id, contentNoEsc);
            SetGlyphAttributes(element);
            return new DocumentLine(element, sb.ToString(), currentRaw.ToString());
        }

        private void NewLine(bool ignoreIfLineEmpty)
        {
            if (CurrentLineEmpty && ignoreIfLineEmpty)
                return;

            var currentLineText = currentContent;

            Align.Format(currentLineText, CurrentFontWidth, FillSymbols, CharWidthProvider);
            ApplyFontGlyph(currentLineText);
            var currentLineNoEsc = currentLineText.ToString();
            if (StartLineWithFontEsc)
            {
                currentLineText.Insert(0, CurrentFont.Esc);
                StartLineWithFontEsc = false;
            }

            var element = new XElement(CurrentFont.Id, currentLineNoEsc);
            SetGlyphAttributes(element);
            lines.Add(new DocumentLine(element, currentLineText.ToString(), currentRaw.ToString()));
            SummarizedStoredLinesLength += CurrentLineTextLength + CurrentLineEscLength;
            ClearCurrentLine();
        }

        private void SetGlyphAttributes(XElement element)
        {
            if(CurrentFontGlyphFlags.IsBold)
                element.SetAttributeValue(TagFont.BoldFontAttribute, "on");
            if (CurrentFontGlyphFlags.IsItalic)
                element.SetAttributeValue(TagFont.ItalicFontAttribute, "on");
            if (CurrentFontGlyphFlags.IsReverse)
                element.SetAttributeValue(TagFont.ReverseFontAttribute, "on");
            if (CurrentFontGlyphFlags.IsUnderline)
                element.SetAttributeValue(TagFont.UnderlineFontAttribute, "on");
        }

        private void ApplyFontGlyph(StringBuilder currentLineText)
        {
            if (CurrentFontGlyphFlags.IsBold)
            {
                currentLineText.Insert(0, Fonts.FontGlyph.BoldBeginEsc);
                currentLineText.Append(Fonts.FontGlyph.BoldEndEsc);
            }
            if (CurrentFontGlyphFlags.IsItalic)
            {
                currentLineText.Insert(0, Fonts.FontGlyph.ItalicBeginEsc);
                currentLineText.Append(Fonts.FontGlyph.ItalicEndEsc);
            }
            if (CurrentFontGlyphFlags.IsReverse)
            {
                currentLineText.Insert(0, Fonts.FontGlyph.ReverseBeginEsc);
                currentLineText.Append(Fonts.FontGlyph.ReverseEndEsc);
            }
            if (CurrentFontGlyphFlags.IsUnderline)
            {
                currentLineText.Insert(0, Fonts.FontGlyph.UnderlineBeginEsc);
                currentLineText.Append(Fonts.FontGlyph.UnderlineEndEsc);
            }
        }

        private int CurrentFontWidth
        {
            get { return CurrentFont.Width; }
        }

        private void NewParagraph(bool ignoreIfLineEmpty)
        {
            if (Align is AlignJustify)
            {
                Align = AlignLeft.Instance;
                NewLine(ignoreIfLineEmpty);
                Align = AlignJustify.Instance;
            }
            else
            {
                NewLine(ignoreIfLineEmpty);
            }
        }

        private void SetFontInternal([NotNull] Font font)
        {
            Debug.Assert(font != null);

            if (!Fonts.ContainsFont(font))
                throw new ArgumentOutOfRangeException(FontOutOfRange);

            NewParagraph(true);

            StartLineWithFontEsc = true;
            CurrentFont = font;
        }

        void ITextBlock.Append(string text, int start, int width)
        {
            currentContent.Append(text, start, width);
            UpdatePosition(text, start, width);
        }

        private void UpdatePosition(string text, int start, int width)
        {
            var tapeStringLength = GetCharsWidth(text, start, width);
            if (tapeStringLength + Position > CurrentFontWidth)
                throw new ArgumentOutOfRangeException(string.Format("Invalid text position: text = '{0}', start = {1}, width = {2}, tapeStringLength = {3}, Position = {4}, CurrentFontWidth = {5}",
                    text, start, width, tapeStringLength, Position, CurrentFontWidth));

            Position += tapeStringLength;
        }

        private void ClearCurrentLine()
        {
            currentContent.Clear();
            currentRaw.Clear();
            Position = 0;
        }

        private int CurrentLineTextLength
        {
            get { return currentContent.Length; }
        }

        private int CurrentLineEscLength
        {
            get { return currentRaw.Length; }
        }

        private bool CurrentLineEmpty
        {
            get { return CurrentLineTextLength == 0 && CurrentLineEscLength == 0; }
        }

        public void Pagecut()
        {
            lines.Add(new DocumentLine(new XElement(TagPagecut.Instance.Name), pagecutEsc, string.Empty));
        }
    }
}