using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class PaymentStrategySpecification : IEquatable<PaymentStrategySpecification>
    {
        public bool Equals(PaymentStrategySpecification other)
        {
            return Equals(MainRole, other.MainRole) &&
                   Items.All(item => other.Items.Contains(item)) &&
                   other.Items.All(item => Items.Contains(item));
        }

        public override bool Equals(object obj)
        {
            var other = obj as PaymentStrategySpecification;
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return new
            {
                MainRole,
                Items
            }.GetHashCode();
        }

        public override string ToString()
        {
            return new ToStringBuilder<PaymentStrategySpecification>()
                .AddProperty(spec => spec.MainRole)
                .AddProperty(spec => spec.Items)
                .GetString(this);
        }
    }
}
