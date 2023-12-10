using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class TransformationDocument
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
                        input.ProductSize,
                        input.AmountFactor,
                        input.Amount,
                        input.AmountUnitFrom,
                        input.ContainerFromId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set
            {
                Items.Set(value.Select(input =>
                    new TransformationDocumentItem(
                        input.Id,
                        input.Number,
                        input.Product,
                        input.Amount,
                        this,
                        input.ProductSize,
                        input.AmountFactor,
                        input.Container.Id,
                        input.Unit,
                        Product,
                        ContainerId.GetValueOrFakeDefault())));
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.TRANSFORMATION_DOCUMENT; }
        }

        bool? ManualOrAutomaticDocument.Editable
        {
            get { return Editable; }
        }

        bool? ManualOrAutomaticDocument.IsAutomatic
        {
            get { return IsAutomatic; }
        }
    }
}