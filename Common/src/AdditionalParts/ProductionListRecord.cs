namespace Resto.Data
{
    public partial class ProductionListRecord
    {
        public override string DepartmentToAsString
        {
            get { return DepartmentTo == null ? string.Empty : DepartmentTo.NameLocal; }
        }

        public override string ExecutorToAsString
        {
            get { return SupplierTo != null ? SupplierTo.NameLocal : DepartmentToAsString; }
        }

        public override string StoreFromAsString
        {
            get { return SupplierFrom != null ? SupplierFrom.NameLocal : base.StoreFromAsString; }
        }
    }
}