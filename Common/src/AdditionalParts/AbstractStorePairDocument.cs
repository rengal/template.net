namespace Resto.Data
{
    public abstract partial class AbstractStorePairDocument
    {
        public override Store DocStore
        {
            get { return storeFrom; }
            set { storeFrom = value; }
        }

        public override Store DocStoreTo
        {
            get { return storeTo; }
            set { storeTo = value; }
        }
    }
}