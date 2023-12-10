using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class PaymentStrategySpecificationItem : IEquatable<PaymentStrategySpecificationItem>
    {
        public PaymentStrategySpecificationItem([NotNull] Role role, bool zeroPaymentForAddedRoles)
        {
            Role = role;
            ScheduleType = role.ScheduleType;
            PaymentPerHour = zeroPaymentForAddedRoles ? 0 : role.PaymentPerHour;
            SteadySalary = zeroPaymentForAddedRoles ? 0 : role.SteadySalary;
        }

        public bool Equals(PaymentStrategySpecificationItem other)
        {
            return Equals(Role, other.Role) &&
                Equals(ScheduleType, other.ScheduleType) &&
                Equals(PaymentPerHour, other.PaymentPerHour) &&
                Equals(SteadySalary, other.SteadySalary);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PaymentStrategySpecificationItem;
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
                Role,
                ScheduleType,
                PaymentPerHour,
                SteadySalary
            }.GetHashCode();
        }
    }
}
