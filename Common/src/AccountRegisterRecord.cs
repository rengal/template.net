using Resto.Common.Extensions;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class AccountRegisterRecord
    {
        [Transient]
        private bool created;
        [Transient]
        private bool modified;
        [Transient]
        private decimal? filteredBalance;

        public bool Created
        {
            get { return created; }
            set { created = value; }
        }

        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        /// <summary>
        /// Баланс с учётом фильтров по подразделению и контрагенту
        /// </summary>
        public decimal? FilteredBalance
        {
            get { return filteredBalance; }
            set { filteredBalance = value; }
        }

        /// <summary>
        /// Возвращает true, если запись создана, не была редактируема и сумма равна 0.
        /// </summary>
        public bool IsEmptyRow
        {
            get
            {
                return Created && !Modified && Sum.GetValueOrFakeDefault() == 0;
            }
        }

        /// <summary>
        /// Возвращает true, если запись создана или была редактируема
        /// </summary>
        public bool IsCreatedOrModified
        {
            get
            {
                return Created || Modified;   
            }            
        }

        public static bool operator ==(AccountRegisterRecord a, AccountRegisterRecord b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Id.GetValueOrFakeDefault() == b.Id.GetValueOrFakeDefault() && a.Sum.GetValueOrFakeDefault() == b.Sum.GetValueOrFakeDefault();
        }

        public static bool operator !=(AccountRegisterRecord a, AccountRegisterRecord b)
        {
            return !(a == b);
        }
    }
}