using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using System;

namespace Resto.Common.Extensions
{
    /// <summary>
    /// Extension-методы для класса <see cref="PastOrderPayment"/>
    /// </summary>
    public static class PastOrderPaymentExtensions
    {
        /// <summary>
        /// Является ли оплата предоплатой.
        /// </summary>
        public static bool IsPrepay([NotNull] this PastOrderPayment pastOrderPayment)
        {
            if (pastOrderPayment is null)
                throw new ArgumentNullException(nameof(pastOrderPayment));

            return pastOrderPayment.TransactionType == TransactionType.PREPAY_CLOSED || pastOrderPayment.TransactionType == TransactionType.PREPAY_CLOSED_RETURN;
        }
        /// <summary>
        /// Является ли оплата чаевыми.
        /// </summary>
        public static bool IsDonation([NotNull] this PastOrderPayment pastOrderPayment)
        {
            if (pastOrderPayment is null)
                throw new ArgumentNullException(nameof(pastOrderPayment));

            return pastOrderPayment.TransactionType == TransactionType.TIPS;
        }
    }
}
