using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    /// <remarks>
    /// Бэковское дополнение для работы с классом AbstractIncomingPriceList.
    /// </remarks>
    public partial class AbstractIncomingPriceList<TKey, TItem> : IPriceList<TItem> where TItem : AbstractIncomingPriceListItem
    {
        #region Properties

        /// <summary>
        /// Прямая ссылка на список связок.
        /// Не использовать в бизнес-логике: нужно различать удаленные и неудаленные строки.
        /// </summary>
        protected List<TItem> ModifiableItems => items;

        /// <summary>
        /// Возвращает коллекцию всех связок прайс-листа
        /// </summary>
        public IReadOnlyCollection<TItem> AllItems => items;

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает true, если дата лежит в пределах прайса.
        /// </summary>
        /// <remarks>
        /// На сервере даты хранятся как 1 - 10 и 10 - т.д., а отображаюся как 1 - 9 и 10 - т.д.
        /// Поэтому по верхней гранифе нет равенства.
        /// </remarks>
        public bool ContainsDate(DateTime date)
        {
            return date >= DateFrom.GetValueOrDefault() && date < DateTo.GetValueOrDefault();
        }

        /// <summary>
        /// Добавить новую связку в прайс-лист
        /// </summary>
        /// <param name="item">Добавляемая связка</param>
        public void AddPriceItem([NotNull] TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Add(item);
        }

        public void UpdatePriceItems(IReadOnlyCollection<TItem> newItems)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            var updatedItemsIds = newItems.Select(item => item.Id);

            // связки которые не входят в список newItems помечаются как удаленные
            var deletedItems = items.Where(item => !updatedItemsIds.Contains(item.Id)).ToList();
            deletedItems.ForEach(item => item.Deleted = true);

            items.Clear();
            items.AddRange(deletedItems);
            items.AddRange(newItems);
        }

        #endregion
    }
}