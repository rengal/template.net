using System.Collections.Generic;

namespace Resto.Framework.Common.CardProcessor
{
    public interface ICardReaderConfig
    {
        bool Configured { get; set; }
        void Save();
        string KodosPortName { get; set; }
        string KodosPrefix { get; set; }
        string KodosSuffix { get; set; }
        List<string> OPOSBarcodeScanerName { get; set; }
        List<string> ComBarcodeScanerPort { get; set; }
        string ComIikoCardPort { get; set; }
        int? ComCardReaderBaudrate { get; set; }
        string ComCardReaderTrackPattern { get; set; }
        int? ComCardReaderMinBytes { get; set; }
        string ComCardReaderPrefix { get; set; }
        string ComCardReaderSuffix { get; set; }
        bool ComCardReaderHex { get; set; }
        int? ComCardReaderHexToDec { get; set; }
        bool ComCardReaderAddTrailingSlash { get; set; }
        bool ComCardReaderIsHardwareFlowControl { get; set; }

        string ZReaderPort { get; set; }
        int? ZReaderPortType { get; set; }
        string ZReaderName { get; set; }
        int? ZReaderDataOffset { get; set; }
        int? ZReaderDataLength { get; set; }
        string ZReaderDataType { get; set; }
        string ZReaderAuthKey { get; set; }

        bool ParsecReaderEnabled { get; set; }
        bool ParsecReaderHex { get; set; }

        string ProximusPortName { get; set; }
        int? ProximusDataLength { get; set; }

        string PCSCCardReaderName { get; set; }
        byte? PCSCCardUIDLength { get; set; }
    }
}
