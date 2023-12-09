using System;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Print.VirtualTape
{
    /// <summary>
    /// Класс для описания отформатированной для принтера строки текстового документа
    /// </summary>
    public sealed class DocumentLine
    {
        /// <summary>
        /// Строка в виде xml-объекта. Имя тега означает используемый шрифт или тег, описывающий команду для принтера (qrcode, barcode, logo, pagecut, pulse)
        /// Значение тега - текст строки или значение штрихкода, qr-кода, номер логотипа (не содержит Esc-последовательность для принтера)
        /// Для некоторых тегов разрешается использование атрибутов (например, qrcode)
        /// </summary>
        [NotNull]
        public XElement Element { get; }

        /// <summary>
        /// Esc-последовательность для команда принтера, если строка содержит текстовую или графическую информацию.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Esc-последовательность для команды принтера без печати текстовой или графической информации (pulse, pagecut)
        /// </summary>
        public string Raw { get; }

        public DocumentLine([NotNull] XElement element, [CanBeNull] string content, [CanBeNull] string raw)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(raw))
                throw new ArgumentException($"{nameof(content)} and {nameof(raw)} cannot be both defined");
            Element = element;
            Content = content;
            Raw = raw;
        }
    }
}
