using System;

namespace Resto.Common
{
    /// <summary>
    /// Элемент автодобавления блюд.
    /// </summary>
    public class AutoAdditionElement
    {
        /// <summary>
        /// Идентификатор автодобавлемого блюда.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Количество автодобавлямого блюда.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Идентификатор гостя, которому автодобавляется блюдо.
        /// </summary>
        public Guid GuestId { get; private set; }

        public AutoAdditionElement(Guid productId, decimal amount, Guid guestId)
        {
            ProductId = productId;
            Amount = amount;
            GuestId = guestId;
        }
    }
}
