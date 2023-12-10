using System;

namespace Resto.Data
{
    /// <remarks>
    /// Бэковское дополнение для работы с классом SupplierPriceListItem.
    /// </remarks>
    public partial class SupplierPriceListItem : IEquatable<SupplierPriceListItem>
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is SupplierPriceListItem))
            {
                return false;
            }
            return Equals((SupplierPriceListItem)obj);
        }

        public bool Equals(SupplierPriceListItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (!base.Equals(other))
            {
                return false;
            }
            return SupplierProduct.Equals(other.SupplierProduct) && NativeProduct.Equals(other.NativeProduct) && ContainerId == other.ContainerId;
        }

        public override int GetHashCode()
        {
            return new {SupplierProduct, NativeProduct, ContainerId}.GetHashCode();
        }
    }
}
