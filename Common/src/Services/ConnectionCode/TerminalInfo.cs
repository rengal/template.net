using System;
using System.Runtime.Serialization;

namespace Resto.Common.Services.ConnectionCode
{
    /// <summary>
    /// Ответ сервиса, содержащий информацию о терминале.
    /// </summary>
    [DataContract]
    public sealed class TerminalInfo
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "serverUrl")]
        public string ServerUrl { get; set; }

        [DataMember(Name = "agentId")]
        public Guid AgentId { get; set; }

        [DataMember(Name = "terminalId")]
        public Guid TerminalId { get; set; }
    }
}
