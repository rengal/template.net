using System.ComponentModel;
using Resto.Common.Properties;

namespace Resto.Data
{
    public class ServiceTableRecord : InvoiceRecord
    {
        public ServiceTableRecord(AbstractInvoiceItem item, User supplier, BindingList<InvoiceRecord> collection) : base(item, supplier, collection)
        {
        }

        public ServiceTableRecord(int newRowNum, AbstractInvoiceItem item, User supplier, decimal maxInPrice, InvoiceEditMode invoiceRecordEditMode, BindingList<InvoiceRecord> collection) : base(newRowNum, item, supplier, maxInPrice, invoiceRecordEditMode, collection)
        {
        }

        public ServiceTableRecord(int newRowNum, User supplier, BindingList<InvoiceRecord> collection) : base(newRowNum, supplier, collection)
        {
        }

        public ServiceTableRecord(int newRowNum, User supplier, decimal maxInPrice, InvoiceEditMode invoiceRecordEditMode, BindingList<InvoiceRecord> collection) : base(newRowNum, supplier, maxInPrice, invoiceRecordEditMode, collection)
        {
        }

        public string LesTrucks { get { return Resources.ServiceTableRecord_LesTrucks; } }
    }    
}