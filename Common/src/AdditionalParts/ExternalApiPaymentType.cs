using System.Linq;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ExternalApiPaymentType
    {
        /// <summary>
        /// Является ли тип оплаты типом оплаты iikoCard5, используемым для скидок.
        /// </summary>
        public bool IsIikoCard5ForDiscount()
        {
            if (!PaymentSystem.IsIikoCard5)
                return false;

            return EntityManager.INSTANCE.GetAllNotDeleted<IikoCard5Settings>(s => Equals(s.RelatedTypeForDiscount, this)).SingleOrDefault() != null;
        }
    }
}
