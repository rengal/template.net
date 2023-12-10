using System;
using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    public class StoreAccountsPair
    {
        public StoreAccountsPair()
        {
        }

        public StoreAccountsPair(Store store, Account accountPlus, Account accountMinus)
        {
            StoreId = store.Id;
            AccountPlusId = accountPlus.Id;
            AccountMinusId = accountMinus.Id;
        }

        public Guid StoreId { get; set; }

        public Guid AccountPlusId { get; set; }

        public Guid AccountMinusId { get; set; }

        public Store Store
        {
            get
            {
                try
                {
                    return EntityManager.INSTANCE.Get<Store>(StoreId);
                }
                catch
                {
                    return null;
                }
            }
        }

        public Account AccountPlus
        {
            get
            {
                try
                {
                    return EntityManager.INSTANCE.Get<Account>(AccountPlusId);
                }
                catch
                {
                    return null;
                }
            }
        }

        public Account AccountMinus
        {
            get
            {
                try
                {
                    return EntityManager.INSTANCE.Get<Account>(AccountMinusId);
                }
                catch
                {
                    return null;
                }
            }
        }
    }

    public class StoreAccountsMap : List<StoreAccountsPair>
    {
        public KeyValuePair<Account, Account>? this[Store store]
        {
            get
            {
                var pair = Find(sap => sap.Store == store);
                if (pair != null)
                {
                    return new KeyValuePair<Account, Account>(pair.AccountPlus, pair.AccountMinus);
                }

                return null;
            }

            set
            {
                if (store == null)
                {
                    return;
                }

                var pair = Find(sap => sap.Store == store);
                if (pair == null)
                {
                    if (value.HasValue)
                    {
                        Add(new StoreAccountsPair(store, value.Value.Key, value.Value.Value));
                    }
                }
                else
                {
                    if (value.HasValue)
                    {
                        pair.AccountPlusId = value.Value.Key.Id;
                        pair.AccountMinusId = value.Value.Value.Id;
                    }
                    else
                    {
                        Remove(pair);
                    }
                }
            }
        }
    }
}
