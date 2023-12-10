using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public sealed partial class ConsolidatedOrderDocument
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
            set
            {
                Items.Set(value.Select(item =>
                                       new ConsolidatedOrderItem(
                                           item.Id, 
                                           item.Number, 
                                           item.Product, 
                                           item.Amount, 
                                           item.Unit, 
                                           this)
                                           {
                                               ContainerId = item.Container.Id,
                                           }));
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.CONSOLIDATED_ORDER; }
        }
    }
}