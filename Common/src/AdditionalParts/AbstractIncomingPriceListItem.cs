using System;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <remarks>
    /// Бэковское дополнение для работы с классом AbstractIncomingPriceListItem.
    /// </remarks>
    public partial class AbstractIncomingPriceListItem : IEquatable<AbstractIncomingPriceListItem>, IWithContainerId
    {
        public Container Container
        {
            get { return NativeProduct.GetContainerById(ContainerId); }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is AbstractIncomingPriceListItem))
            {
                return false;
            }
            return Equals((AbstractIncomingPriceListItem)obj);
        }

        public bool Equals(AbstractIncomingPriceListItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (!base.Equals(other))
            {
                return false;
            }
            return NativeProduct.Equals(other.NativeProduct) && ContainerId == other.ContainerId;
        }

        public override int GetHashCode()
        {
            return new {NativeProduct, ContainerId}.GetHashCode();
        }
    }
}
