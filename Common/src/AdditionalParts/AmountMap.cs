using System.Collections.Generic;

namespace Resto.Data
{
    public sealed class AmountMap<T>
    {
        private readonly Dictionary<T, decimal> amounts = new Dictionary<T, decimal>();

        public void Add(T t, decimal amount)
        {
            decimal newAmount = amounts.ContainsKey(t) ? amounts[t] + amount : amount;
            amounts[t] = newAmount;
        }

        public void AddAll(AmountMap<T> source)
        {
            foreach (KeyValuePair<T, decimal> item in source.amounts)
            {
                Add(item.Key, item.Value);
            }
        }

        public Dictionary<T, decimal>.KeyCollection GetKeys()
        {
            return amounts.Keys;
        }

        public decimal GetAmount(T t)
        {
            return amounts.ContainsKey(t) ? amounts[t] : 0;
        }

        public void SetAmount(T t, decimal value)
        {
            if (amounts.ContainsKey(t))
                amounts[t] = value;
        }
    }
}