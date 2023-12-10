using System.Runtime.Serialization;

namespace Resto.Common.Services.ConnectionCode
{
    /// <summary>
    /// Запрос на получение информации по терминалу.
    /// </summary>
    [DataContract]
    internal sealed class CodePopRequest
    {
        public CodePopRequest(int code)
        {
            Code = code;
        }

        [DataMember(Name = "code")]
        public int Code;
    }
}
