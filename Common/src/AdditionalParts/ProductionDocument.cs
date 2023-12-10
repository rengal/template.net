using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class ProductionDocument
    {
        public Store StoreFrom
        {
            get { return Store; }
            set { Store = value; }
        }

        public Store StoreTo
        {
            get { return (Store)AccountTo; }
            set { AccountTo = value; }
        }

        public override Store DocStore
        {
            get { return StoreFrom; }
            set { StoreFrom = value; }
        }

        public override Store DocStoreTo
        {
            get { return StoreTo; }
            set { StoreTo = value; }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return null; }
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
                    .Select(input =>
                        new IncomingDocumentItem(
                            input.Id,
                            input.Num,
                            input.Product,
                            input.ProductSize,
                            input.AmountFactor,
                            input.Amount,
                            input.AmountUnit,
                            input.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set
            {
                Items.Set(value.Select(input =>
                                       new ProductionDocumentItem(
                                           input.Id,
                                           input.Number,
                                           input.Product,
                                           input.Amount,
                                           input.Unit,
                                           input.ProductSize,
                                           input.AmountFactor,
                                           this)
                                           {
                                               ContainerId = input.Container.Id,
                                           }));
            }
        }

        protected override List<AbstractProductsWriteoffDocumentItem> AbstractItems
        {
            get
            {
                return Items.ConvertAll<AbstractProductsWriteoffDocumentItem>(
                    input => input
                    );
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.PRODUCTION_DOCUMENT; }
        }
    }
}