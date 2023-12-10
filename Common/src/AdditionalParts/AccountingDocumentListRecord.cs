using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public partial class AccountingDocumentListRecord
    {
        public override string Name
        {
            get { return counteragent != null ? counteragent.NameLocal: ""; }
        }

        public override string StoreInfoField
        {
            get { return string.Join(", ", assignedStores.Select(s => s.NameLocal).ToArray()); }
        }

        public override string StoreFromAsString
        {
            get { return StoreFrom != null ? StoreFrom.ToString() : ""; }
        }

        public override string StoreToAsString
        {
            get { return StoreTo != null ? StoreTo.ToString() : ""; }
        }

        public override string DepartmentToAsString
        {
            get { return ""; }
        }

        public override decimal? SumWithoutNdsToShow
        {
            get { return sumWithoutNds; }
            set { base.SumWithoutNdsToShow = value; }
        }

        public override decimal SumToShow
        {
            get { return sum ?? new decimal(); }
            set { sum = value; }
        }

        public override decimal? NDSSum
        {
            get { return sum - sumWithoutNds; }
            set { base.NDSSum = value; }
        }

        public override List<Store> StoresList
        {
            get
            {
                List<Store> stores = new List<Store>();
                if (StoreTo != null)
                {
                    stores.Add(StoreTo);
                }
                if (StoreFrom != null)
                {
                    stores.Add(StoreFrom);
                }
                return assignedStores.Count == 0 ? stores : assignedStores;
            }
        }
    }
}