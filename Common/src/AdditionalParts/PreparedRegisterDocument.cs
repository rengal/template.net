using System.Collections.Generic;

namespace Resto.Data
{
    public sealed partial class PreparedRegisterDocument
    {
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
            get { return DepartmentTo; }
            set { DepartmentTo = value; }
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
                /*List<IncomingDocumentItem> items = new List<IncomingDocumentItem>();
                foreach (PreparedRegisterItem item in Items)
                {
                    items.Add(new IncomingDocumentItem(item.Num, item.Product, item.Amount, item.AmountUnit, 0));
                }
                return items;*/
                return null;
            }
            set
            {
                /*List<ProductionOrderItem> items = new List<ProductionOrderItem>();
                foreach (IncomingDocumentItem item in value)
                {
                    items.Add(
                        new ProductionOrderItem(GuidGenerator.Next(), this, item.Product, item.Number, item.Amount,
                                                item.Unit));
                }
                Items = items;*/
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.PREPARED_REGISTER; }
        }
    }
}