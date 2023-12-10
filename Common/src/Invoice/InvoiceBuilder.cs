using System;
using System.Linq;
using System.ComponentModel;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public class InvoiceBuilder
    {
        protected DocumentType documentType;
        protected InvoiceItemBuilder itemBuilder;

        public InvoiceBuilder(DocumentType documentType)
        {
            this.documentType = documentType;
            itemBuilder = InvoiceItemBuilder.CreateItemBuilder(documentType);
        }

        public AbstractInvoiceDocument CreateInvoice(Guid id, DateTime dateIncoming,
            DocumentStatus documentStatus, string documentNumber, User supplier, Account revenueAccount,
            Account revenueDebitAccount, Account revenueCreditAccount, Account accountTo, Store defaultStore, BindingList<InvoiceRecord> invoiceRecords)
        {
            AbstractInvoiceDocument result = CreateInvoice(id, dateIncoming, documentStatus, documentNumber,
                                                           supplier, revenueAccount, revenueDebitAccount, revenueCreditAccount, accountTo,
                                                           defaultStore);
            UpdateInvoiceItems(result, invoiceRecords);
            return result;
        }


        public AbstractInvoiceDocument CreateInvoice(Guid id, DateTime dateIncoming,
            DocumentStatus documentStatus, string documentNumber, User supplier, Account revenueAccount,
            Account revenueDebitAccount, Account revenueCreditAccount, Account accountTo, Store defaultStore)
        {
            if (documentType == DocumentType.OUTGOING_INVOICE)
                return new OutgoingInvoice(id, dateIncoming, documentNumber, documentStatus,
                    supplier, revenueAccount, revenueDebitAccount, accountTo, CafeSetup.INSTANCE.ChartOfAccounts.DiscountsAccount, defaultStore);

            if (documentType == DocumentType.INCOMING_INVOICE)
                return new IncomingInvoice(id, dateIncoming, documentStatus, documentNumber,
                    supplier, defaultStore);

            if (documentType == DocumentType.INCOMING_SERVICE)
                return new IncomingService(id, dateIncoming, documentStatus, documentNumber,
                    supplier, revenueAccount, revenueCreditAccount);

            if (documentType == DocumentType.OUTGOING_SERVICE)
                return new OutgoingService(id, dateIncoming, documentStatus, documentNumber,
                    supplier, revenueAccount, revenueDebitAccount);

            if (documentType == DocumentType.SALES_DOCUMENT)
                return new SalesDocument(id, dateIncoming, documentNumber, documentStatus, supplier,
                                         revenueAccount, revenueDebitAccount, accountTo,
                                         CafeSetup.INSTANCE.ChartOfAccounts.DiscountsAccount)
                { Editable = true, DefaultStore = defaultStore };
            return null;
        }

        public void UpdateInvoiceItems(AbstractInvoiceDocument document, BindingList<InvoiceRecord> invoiceRecords)
        {
            document.Items.Clear();
            invoiceRecords
                .Where(rec => !rec.IsEmptyRecord)
                .ToList()
                .ForEach(rec => rec.InvoiceItem = itemBuilder.CreateInvoiceItem(rec, document));
            document.Items.AddRange(invoiceRecords.Select(r => r.InvoiceItem).Where(i => i != null));
        }
    }
}
