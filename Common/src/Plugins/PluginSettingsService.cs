using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.Plugins
{
    public static class PluginSettingsService
    {
        /// <summary>
        /// Максимальный размер настроек плагина в строковом представлении для хранения в базе данных.
        /// </summary>
        private const int MaxPluginSettingsDataLength = 5000;

        public static string Serialize([NotNull] this List<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var xmlSettings = new XmlWriterSettings
            {
                CheckCharacters = true,
                OmitXmlDeclaration = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, xmlSettings))
            {
                new XmlSerializer(typeof(List<Setting>)).Serialize(writer, settings);
                var xmlData = sw.ToString();
                writer.Close();
                sw.Close();
                if (xmlData.Length > MaxPluginSettingsDataLength)
                    throw new ArgumentException(@"Object to store is too large.", nameof(settings));
                return xmlData;
            }
        }

        [NotNull]
        public static List<Setting> Deserialize([CanBeNull] string xml)
        {
            if (xml.IsNullOrEmpty())
                return new List<Setting>(0);

            var xmlSettings = new XmlReaderSettings
            {
                CheckCharacters = true
            };

            using (var sr = new StringReader(xml))
            using (var reader = XmlReader.Create(sr, xmlSettings))
            {
                var obj = new XmlSerializer(typeof(List<Setting>)).Deserialize(reader);
                reader.Close();
                sr.Close();
                return (List<Setting>)obj;
            }
        }
    }
}
