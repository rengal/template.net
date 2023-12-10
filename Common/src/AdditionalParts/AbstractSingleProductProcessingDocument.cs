namespace Resto.Data
{
    public partial class AbstractSingleProductProcessingDocument
    {
        public override Store DocStore
        {
            get { return StoreFrom; }
            set { StoreFrom = value; }
        }

        public override Store DocStoreTo
        {
            get { return StoreTo; }
            set { StoreTo = value; }
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
    }
}