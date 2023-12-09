using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Resto.Framework.Attributes.JetBrains;
using log4net;

namespace Resto.Framework.Common.Print.VirtualTape
{
    public partial class Tape
    {
        private static readonly Lazy<XmlReaderSettings> XmlReaderSettingsForValidate = new Lazy<XmlReaderSettings>(CreateXmlReaderSettingsForValidate);

        private static void ValidateXml(string xmlDoc)
        {
            using (var reader = XmlReader.Create(new StringReader(xmlDoc), XmlReaderSettingsForValidate.Value))
            {
                while (reader.Read())
                {}
            }
        }

        [NotNull]
        private static XmlReaderSettings CreateXmlReaderSettingsForValidate()
        {
            XmlSchema schema;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Resto.Framework.Src.Common.Print.Doc.xsd"))
            {
                Debug.Assert(stream != null);
                schema = XmlSchema.Read(stream, SchemaValidationEventHandler);
            }

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(schema);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += SchemaValidationEventHandler;

            return settings;
        }

        private static void SchemaValidationEventHandler(object sender, ValidationEventArgs args)
        {
            var log = LogManager.GetLogger(typeof(Tape));
            log.ErrorFormat("Print document validation error: {0} {1}", args.Severity, args.Message);
#if DEBUG
            if (args.Severity == XmlSeverityType.Error)
                throw new InvalidOperationException("Printing document schema validation error: " + args.Message);
#endif
        }
    }
}