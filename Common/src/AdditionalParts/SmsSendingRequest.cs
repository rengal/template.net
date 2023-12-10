using System;

namespace Resto.Data
{
    public partial class SmsSendingRequest
    {
        #region Constructor

        public SmsSendingRequest(string text, string receiver, Guid? orderId, bool verificationSms)
            : this(text, receiver, null, null, orderId, null, null, null, null, null, verificationSms)
        {
        }

        #endregion
    }
}