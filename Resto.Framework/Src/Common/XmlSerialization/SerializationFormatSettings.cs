using System.Xml;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization
{
    public sealed class SerializationFormatSettings
    {
        #region Instances
        private static readonly SerializationFormatSettings @default = new SerializationFormatSettings(false, false);
        private static readonly SerializationFormatSettings withXmlDeclarationAndIndentation = new SerializationFormatSettings(true, false);
        private static readonly SerializationFormatSettings withoutXmlDeclarationAndIndentation = new SerializationFormatSettings(false, true);

        /// <summary>
        /// Без отступов, с xml декларацией.
        /// </summary>
        [NotNull]
        public static SerializationFormatSettings Default
        {
            get { return @default; }
        }

        /// <summary>
        /// Без отступов, без xml декларации.
        /// </summary>
        [NotNull]
        public static SerializationFormatSettings WithoutXmlDeclarationAndIndentation
        {
            get { return withoutXmlDeclarationAndIndentation; }
        }

        /// <summary>
        /// С отступами и xml декларацией
        /// </summary>
        [NotNull]
        public static SerializationFormatSettings WithXmlDeclarationAndIndentation
        {
            get { return withXmlDeclarationAndIndentation; }
        }
        #endregion

        #region Fields
        private readonly XmlWriterSettings xmlWriterSettings;
        #endregion

        #region Ctor
        private SerializationFormatSettings(bool fancyFormat, bool omitXmlDeclaration)
        {
            xmlWriterSettings = new XmlWriterSettings
            {
                CheckCharacters = false,
                OmitXmlDeclaration = omitXmlDeclaration,
                Indent = fancyFormat
            };
        }
        #endregion

        #region Props
        [NotNull]
        public XmlWriterSettings XmlWriterSettings
        {
            get { return xmlWriterSettings; }
        }
        #endregion
    }
}