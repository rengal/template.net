using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class RestaurantSection : ICorporatedEntityProps
    {
        public RestaurantSection(Guid id, string name, Group group, IPrinterDevice billPrinter, IPrinterDevice dishPrinter,
                                 Store defaultStore, int index)
            : base(id, new LocalizableValue(name))
        {
            this.group = group;
            this.billPrinter = billPrinter;
            tables = new HashSet<Table>();
            this.dishPrinter = dishPrinter;
            this.defaultStore = defaultStore;
            cookingPlaceMap = new Dictionary<CookingPlaceType, CookingPlaceTypeSettings>();
            terminals = new HashSet<Terminal>();
            this.index = index;
            kitchenProductCookingScenario = new KitchenProductCookingScenario(
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.QUEUED, KitchenProductCookingStatus.QUEUED.ToString(), true, 0, true),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.READY_FOR_COOKING, KitchenProductCookingStatus.READY_FOR_COOKING.ToString(), true),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.COOKING_1, KitchenProductCookingStatus.COOKING_1.ToString(), true),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.COOKING_2, KitchenProductCookingStatus.COOKING_2.ToString()),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.COOKING_3, KitchenProductCookingStatus.COOKING_3.ToString()),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.COOKING_4, KitchenProductCookingStatus.COOKING_4.ToString()),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.COMPLETED, KitchenProductCookingStatus.COMPLETED.ToString(), true),
                new KitchenProductCookingStatusSettings(KitchenProductCookingStatus.SERVED, KitchenProductCookingStatus.SERVED.ToString(), true, 0));
        }

        /// <summary>
        /// Возвращает все неудаленные столы.
        /// </summary>
        /// <remarks>
        /// RMS-49123. Использует только неудаленные столы.
        /// </remarks>
        public IEnumerable<Table> NotDeletedTables => Tables.Where(table => !table.Deleted);

        public override string ToString()
        {
            return NameLocal;
        }

        #region ICorporatedEntityProps implementation

        public string CEDescription
        {
            get { return string.Empty; }
            set { /* do nothing */ }
        }

        public PersistedEntity CEParent
        {
            get { return Group; }
            set
            {
                var groupEntity = value as Group;
                if (groupEntity == null)
                {
                    throw new ArgumentException("CEParent must be Group");
                }
                Group = groupEntity;
            }
        }

        public CorporatedEntityType CEType
        {
            get { return CorporatedEntityType.RESTAURANTSECTION; }
        }

        #endregion
    }
}