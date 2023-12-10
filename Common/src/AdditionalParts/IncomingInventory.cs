using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class IncomingInventory
    {
        private Store docStore;

        public override Store DocStore
        {
            get { return docStore; }
            set { docStore = value; }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return null; }
            set { }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get
            {
                return Items.Select(item =>
                    new IncomingDocumentItem(
                        item.Id,
                        item.Num,
                        item.Product,
                        ProductSizeServerConstants.INSTANCE.DefaultProductSize,
                        ProductSizeServerConstants.INSTANCE.DefaultAmountFactor,
                        item.Amount,
                        item.Unit,
                        item.ContainerId.GetValueOrFakeDefault())
                    {
                        ContainerName = item.ContainerName,
                    }).ToList();
            }
            set { }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.INCOMING_INVENTORY; }
        }

        /// <summary>
        /// Раньше при переходе от одной версии к другой, в инвентаризациях были баги, метод чинит эти баги.
        /// </summary>
        public void BugFixInventory()
        {
            int bugFlag = FindBugNumStep();
            if (bugFlag == 0) return;
            if ((bugFlag & 1) == 1)
            {
                int i = 0;
                foreach (IncomingInventoryFirstStepItem item in ItemsFirstStep)
                {
                    item.Num = ++i;
                }
            }
            if ((bugFlag & 2) == 2)
            {
                int i = 0;
                foreach (IncomingInventoryItem item in Items)
                {
                    item.Num = ++i;
                }
            }
        }

        /// <summary>
        /// Метод находит шаги инвентаризации где есть баги
        /// </summary>
        private int FindBugNumStep()
        {
            int i = 0;
            if (ItemsFirstStep.Any(item => item.Num < 0))
            {
                i |= 1;
            }
            if (Items.Any(item => item.Num < 0))
            {
                i |= 2;
            }
            return i;
        }
    }
}