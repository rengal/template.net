using Resto.Framework.Data;

namespace Resto.Data
{
    partial class StoreInfo
    {
        public StoreInfo(Store store)
        {
            this.store = new ByValue<Store>(store);
            this.useForInternalTransfers = store.ShouldBeUsedForInternalTransfers;
            this.gln = store.Gln;
        }
    }
}