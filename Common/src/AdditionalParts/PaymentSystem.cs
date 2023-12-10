namespace Resto.Data
{
    public partial class PaymentSystem
    {
        public bool IsIikoNet
        {
            get { return NameLocal == PaymentSystemNames.IikoNet; }
        }

        public bool IsPlastek
        {
            get { return NameLocal == PaymentSystemNames.Plastek; }
        }

        public bool IsSbrf
        {
            get { return NameLocal == PaymentSystemNames.Sbrf; }
        }

        public bool IsSmartSale
        {
            get { return NameLocal == PaymentSystemNames.SmartSale; }
        }

        public bool IsTrpos
        {
            get { return NameLocal == PaymentSystemNames.Trpos; }
        }

        public bool IsIikoCard5
        {
            get { return NameLocal == PaymentSystemNames.IikoCard5; }
        }

        public bool IsIikoCard51
        {
            get { return NameLocal == PaymentSystemNames.IikoCard51; }
        }
    }
}
