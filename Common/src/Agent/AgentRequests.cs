using System;
using Resto.Common.Properties;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    #region AgentRequestVisitor Interface

    public interface AgentRequestVisitor
    {
        #region Methods

        ServerResponse Visit(AgentGetTask request);
        ServerResponse Visit(AgentOkResult result);
        ServerResponse Visit(AgentErrorResult result);
        ServerResponse Visit(AgentDeviceMessage result);

        #endregion Methods
    }

    #endregion AgentRequestVisitor Interface

    #region ICallback Interface

    public interface ICallback<T> where T : AgentOkResult
    {
        void Completed(T result);

        void Failed(AgentErrorResult result);
    }

    #endregion ICallback Interface

    #region AgentMessage Class

    public partial class AgentMessage
    {
        public void CheckAndCall<T>(AgentOkResult result, ICallback<T> callback) where T : AgentOkResult
        {
            try
            {
                callback.Completed((T)result);
            }
            catch (InvalidCastException)
            {
                AgentErrorResult error = new AgentErrorResult();
                error.Message = string.Format(Resources.AgentMessageInvalidResultTypeObtained, result.GetType().Name, BrandInfo.PosAgent);

                callback.Failed(error);
            }
        }
    }

    #endregion AgentMessage Class

    #region AgentRequest Class

    public partial class AgentRequest
    {
        public abstract ServerResponse Process(AgentRequestVisitor visitor);
    }

    #endregion AgentRequest Class

    #region AgentGetTask Class

    public partial class AgentGetTask
    {
        public override ServerResponse Process(AgentRequestVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    #endregion AgentGetTask Class

    #region AgentOkResult Class

    public partial class AgentOkResult
    {
        public AgentOkResult(bool success)
        {
            Success = success;
        }

        public override ServerResponse Process(AgentRequestVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    #endregion AgentOkResult Class

    #region AgentErrorResult Class

    public partial class AgentErrorResult
    {
        [NotNull, Pure]
#pragma warning disable 618
        public static AgentErrorResult Create(string message) => new AgentErrorResult { Message = message };
#pragma warning restore 618

        public override ServerResponse Process(AgentRequestVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    #endregion AgentErrorResult Class

    #region AgentDeviceMessage Class

    public partial class AgentDeviceMessage
    {
        public override ServerResponse Process(AgentRequestVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    #endregion AgentDeviceMessage Class
}
