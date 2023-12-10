using System;

namespace Resto.Data
{
    public partial class CustomerPhone : ICloneable
    {
        public override string ToString()
        {
            return PhoneNumber;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public CustomerPhone Clone()
        {
            return new CustomerPhone
                {
                    PhoneNumber = PhoneNumber,
                    Comment = Comment,
                    IsMain = IsMain
                };
        }

        public override bool Equals(object obj)
        {
            var phone = obj as CustomerPhone;
            return phone != null && string.Equals(phone.PhoneNumber, PhoneNumber);
        }
    }
}