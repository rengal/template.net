using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class DisassembleDocument
    {
        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get
            {
                return Items.Select(input =>
                                    new IncomingDocumentItem(
                                        input.Id,
                                        input.Num,
                                        input.Product,
                                        ProductSizeServerConstants.INSTANCE.DefaultProductSize,
                                        ProductSizeServerConstants.INSTANCE.DefaultAmountFactor,
                                        input.Amount,
                                        input.AmountUnit,
                                        input.ContainerId.GetValueOrFakeDefault())
                                    {
                                        MainProductAmountPercent = input.MainProductAmountPercent
                                    }).ToList();
            }
            set
            {
                Items.Set(value.Select(
                    input =>
                    new DisassembleDocumentItem(
                        input.Id,
                        input.Number,
                        input.Product,
                        input.Amount,
                        input.Unit,
                        this,
                        input.MainProductAmountPercent)
                    {
                        ContainerId = input.Container.Id,
                    }));
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.DISASSEMBLE_DOCUMENT; }
        }
    }
}