using System;

namespace Resto.Data
{
    public partial class CustomerEmail : ICloneable
    {
        public override bool Equals(object obj)
        {
            var otherEmail = obj as CustomerEmail;
            return otherEmail != null && string.Equals(otherEmail.Email, Email);
        }

        public override string ToString()
        {
            return Email;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public CustomerEmail Clone()
        {
            return new CustomerEmail
                {
                    Email = Email,
                    IsMain = IsMain,
                    Comment = Comment,
                };
        }
    }
}