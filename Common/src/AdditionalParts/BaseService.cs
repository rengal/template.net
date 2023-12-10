using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class BaseService
    {
        protected BaseService(Guid id, DateTime dateIncoming, DocumentStatus status, string documentNumber, User supplier, Account defaultAccount)
            : this(id, dateIncoming, documentNumber, status, supplier)
        {
        }

        /// <summary>
        /// Сумма, без НДС.
        /// </summary>
        public Decimal Sum
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Сумма НДС.
        /// </summary>
        public Decimal NdsSum
        {
            get { return 0; }
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
                return null;
            }
            set { }
        }

        public override ICollection<ValidationWarning> SuppressedWarnings
        {
            get
            {
                return new List<ValidationWarning> { ValidationWarning.SUPPLIER_PRICE_DEVIATION_LIMIT_EXCEEDED };
            }
        }
    }
}