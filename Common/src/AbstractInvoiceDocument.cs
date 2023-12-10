using System.Linq;
using Resto.Common.Extensions;

namespace Resto.Data
{
    /// <summary>
    /// Родитель различных накладных и актов реализации
    /// </summary>
    public partial class AbstractInvoiceDocument
    {
        /// <summary>
        /// Сумма без НДС
        /// </summary>
        public decimal Sum
        {
            get
            {
                return Items.Sum(item => item.SumWithoutDiscount);
            }
        }

        /// <summary>
        /// Сумма НДС
        /// </summary>
        public decimal NdsSum
        {
            get
            {
                return Items.Sum(item => item.SumWithoutDiscount - item.SumWithoutNds ?? 0m);
            }
        }

        /// <summary>
        /// Юр. лицо.
        /// </summary> 
        /// <remarks>
        /// Для получения юр. лица ориентируемся на <see cref="DefaultStore"/>.
        /// Если в комплишене "Оприходовать на склад" ничего указано,
        /// берется склад из первой записи документа.
        /// https://jira.iiko.ru/browse/RMS-45554
        /// </remarks>
        public JurPerson JurPerson
        {
            get
            {
                if (DefaultStore != null)
                {
                    return DefaultStore.GetJurPerson();
                }

                var firstItem = Items.FirstOrDefault();
                if (firstItem == null || firstItem.Store == null)
                {
                    return null;
                }

                return firstItem.Store.GetJurPerson();
            }
        }
    }
}
