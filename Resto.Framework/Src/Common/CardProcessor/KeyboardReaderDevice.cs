using System.Collections.Generic;
using System.Xml.Serialization;
namespace Resto.Framework.Common.CardProcessor
{
    /// <summary>
    /// Клавиатурное устройство, представление для хранения в конфигурационном файле
    /// </summary>
    public sealed class KeyboardReaderDevice : ReaderDevice
    {
        public ReaderDeviceSetupMethod SetupMethod { get; set; }

        [XmlElement(ElementName = "Prefix")]
        public string MaskedPrefix
        {
            get => Prefix;
            set => Prefix = Upgrade(value);
        }

        [XmlElement(ElementName = "Suffix")]
        public string MaskedSuffix
        {
            get => Suffix;
            set => Suffix = Upgrade(value);
        }

        [XmlArray("ReplaceChars")]
        [XmlArrayItem("Item")]
        public List<ReplaceChar> ReplaceChars { get; set; }

        public string Mask { get; set; }
        public string Regex { get; set; }
        public uint IntercharacterTimeout { get; set; }
        public uint MaxInterval { get; set; }
        public uint MaxAverageInterval { get; set; }
        public bool? AllowMultiplePressedKeys { get; set; }


        [XmlIgnore]
        public string Prefix;

        [XmlIgnore]
        public string Suffix;

        public ReaderDeviceCaseConversion CaseConversion { get; set; }

        private static string Upgrade(string data)
        {
            if (data.IsNullOrEmpty())
                return string.Empty;

            if (data.EndsWith("\\r") || data.EndsWith("\\n"))
                return data.Replace("\\r", "[Enter]").Replace("\\n", "[Enter]");

            return data;
        }
    }

    public enum ReaderDeviceSetupMethod
    {
        PrefixSuffix,
        Mask,
        Regex
    }

    public enum ReaderDeviceCaseConversion
    {
        Upper,
        Lower,
        NoConversion,
        AutoDetect,
    }
}