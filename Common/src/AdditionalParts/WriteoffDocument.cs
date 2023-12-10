using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class WriteoffDocument
    {
        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            set
            {
                Items.Set(value.Select(item => 
                    new WriteoffDocumentItem(
                        item.Id, 
                        item.Number, 
                        item.Product, 
                        item.Amount,
                        item.Unit,
                        item.ProductSize,
                        item.AmountFactor,
                        this)
                        {
                            ContainerId = item.Container.Id,
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
            get { return DocumentType.WRITEOFF_DOCUMENT; }
        }
    }
}