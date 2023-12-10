using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class IncomingInvoice
    {
        public IncomingInvoice(Guid id, DateTime dateIncoming, DocumentStatus status, string documentNumber,
                               User supplier, Store defaultStore)
            : this(id, dateIncoming, documentNumber, status, supplier)
        {
            DefaultStore = defaultStore;
        }

        /// <summary>
        /// Сумма, без НДС.
        /// </summary>
        public Decimal Sum
        {
            get
            {
                return Items.Sum(item => item.SumWithoutDiscount);
            }
            set { }
        }

        /// <summary>
        /// Сумма НДС.
        /// </summary>
        public Decimal NdsSum
        {
            get
            {
                return Items.Sum(item => item.Sum.GetValueOrFakeDefault() - item.SumWithoutNds.GetValueOrFakeDefault());
            }
        }

        public override Store DocStore
        {
            get { return null; }
            set { }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get
            {
                if (DefaultStore != null)
                {
                    return DefaultStore.GetDepartmentEntity();
                }
                if (Items.Any(item => item.Store != null))
                {
                    return Items.FirstOrDefault(item => item.Store != null).Store.GetDepartmentEntity();
                }
                return null;
            }
            set { }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get
            {
                return Items
                    .Select(item =>
                        new IncomingDocumentItem(
                            item.Id,
                            item.Num,
                            item.Product,
                            ProductSizeServerConstants.INSTANCE.DefaultProductSize,
                            ProductSizeServerConstants.INSTANCE.DefaultAmountFactor,
                            item.Amount,
                            item.AmountUnit,
                            item.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set { }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.INCOMING_INVOICE; }
        }

        public override ICollection<ValidationWarning> SuppressedWarnings
        {
            get
            {
                return new List<ValidationWarning> { ValidationWarning.SUPPLIER_PRICE_DEVIATION_LIMIT_EXCEEDED };
            }
        }

        bool? ManualOrAutomaticDocument.Editable
        {
            get { return Editable; }
        }

        public bool? IsAutomatic
        {
            get { return Automatic; }
        }

        /// <summary>
        /// Имеются ли у документа дополнительные расходы
        /// </summary>
        public bool HasAdditionalExpenses
        {
            get { return Items.Cast<IncomingInvoiceItem>().Any(item => item.IsAdditionalExpense); }
        }
    }
}