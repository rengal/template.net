using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class QueryInfoResult
    {
        public QueryInfoResult(Guid? agentId, long localTime, string name, Guid? taskId, bool success, string message, bool warnings, Guid? deviceId,
            List<FiscalRegisterPaymentTypeInfo> paymentTypesInfo,
            List<FiscalRegisterTaxItem> taxesInfo,
            List<SupportedCommand> supportedCommands, 
            bool capQueryElectronicJournalByLastSession
            )
            : base(agentId, localTime, name, taskId, success, message, warnings, deviceId)
        {
            this.paymentTypesInfo = paymentTypesInfo;
            this.taxesInfo = taxesInfo;
            this.supportedCommands = supportedCommands;
            this.capQueryElectronicJournalByLastSession = capQueryElectronicJournalByLastSession;
        }
    }
}
