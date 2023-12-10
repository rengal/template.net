using Resto.Framework.Data;

namespace Resto.Common
{
    /// <summary>
    /// Комитент / поставщик
    /// </summary>
    [DataClass("Contractor")]
    public class Contractor
    {
        private readonly string name;
        private readonly string taxpayerId;
        private readonly string phoneNumber;

        public Contractor()
        { }

        public Contractor(string name, string taxpayerId, string phoneNumber)
        {
            this.name = name;
            this.taxpayerId = taxpayerId;
            this.phoneNumber = phoneNumber;
        }

        public string Name
        {
            get { return name; }
        }

        public string TaxpayerId
        {
            get { return taxpayerId; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
        }
    }
}
