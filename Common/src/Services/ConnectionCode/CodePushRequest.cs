using System;
using System.Runtime.Serialization;

namespace Resto.Common.Services.ConnectionCode
{
    /// <summary>
    /// Пакет передачи данных по терминалу.
    /// </summary>
    [DataContract]
    internal sealed class CodePushRequest
    {
        public CodePushRequest(string serverUrl, Guid agentId, Guid terminalId)
        {
            ServerUrl = serverUrl;
            AgentId = agentId;
            TerminalId = terminalId;
        }

        [DataMember(Name = "serverUrl")]
        public string ServerUrl;

        [DataMember(Name = "agentId")]
        public Guid AgentId;

        [DataMember(Name = "terminalId")]
        public Guid TerminalId;
    }
}
