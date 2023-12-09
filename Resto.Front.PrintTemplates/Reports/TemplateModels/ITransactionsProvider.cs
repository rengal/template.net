using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public interface ITransactionsProvider
    {
        [PublicAPI, NotNull]
        IEnumerable<IOrderPaymentTransaction> GetOrderPaymentTransactions();

        [PublicAPI, NotNull]
        IEnumerable<IOrderPaymentTransaction> GetOrderPaymentTransactionsBySession([NotNull] ICafeSession session);

        [PublicAPI, NotNull]
        IEnumerable<IOrderPaymentTransaction> GetOrderDonationTransactionsBySession([NotNull] ICafeSession session);

        [PublicAPI, NotNull]
        IEnumerable<IPayInOutFiscalTransaction> GetPayInOutFiscalTransactionsBySession([NotNull] ICafeSession session);

        [PublicAPI, NotNull]
        IEnumerable<IOrderPaymentTransaction> GetOrderPaymentTransactionsByOrderCloseInfo([NotNull] IOrderCloseInfo orderCloseInfo);
    }
}