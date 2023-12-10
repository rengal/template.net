using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;

namespace Resto.Data
{
    /// <summary>
    /// Родитель исходящих(с точки зрения движения товаров) документов
    /// </summary>
    public partial class AbstractOutgoingDocument
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
                    .Select(item =>
                        new IncomingDocumentItem(
                            item.Id,
                            item.Num,
                            item.Product,
                            item is ProductSizeDocumentItem ? (item as ProductSizeDocumentItem).ProductSize : ProductSizeServerConstants.INSTANCE.DefaultProductSize,
                            item is ProductSizeDocumentItem ? (item as ProductSizeDocumentItem).AmountFactor : ProductSizeServerConstants.INSTANCE.DefaultAmountFactor,
                            item.Amount,
                            item.AmountUnit,
                            item.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set { }
        }
    }
}