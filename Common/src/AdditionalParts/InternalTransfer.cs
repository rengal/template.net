using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class InternalTransfer
    {
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
                            null,
                            1,
                            item.Amount,
                            item.AmountUnit,
                            item.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set
            {
                Items.Set(value.Select(item =>
                                       new InternalTransferItem(
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
            get { return DocumentType.INTERNAL_TRANSFER; }
        }
    }
}