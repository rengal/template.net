using System;

namespace Resto.Data
{
    public partial class BarcodeContainer
    {
        public Guid GetContainerIdOrDefault()
        {
            return Container == null ? Guid.Empty : Container.Id;
        }

        public string BarcodeString
        {
            get
            {
                return string.IsNullOrEmpty(Barcode)
                           ? null : Barcode;
            }
        }
    }
}