using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Класс для описания полных прайс-листов.
    /// </summary>
    public partial class PriceListItem : IPriceListItem
    {
        public Department DepartmentNotNull
        {
            // Пока-что прейскуранты поддерживаются только для Торговых Предприятий
            get
            {
                if (department as Department == null)
                {
                    throw new RestoException("This is not department in PriceListItem");
                }
                return department as Department;
            }
        }
    }
}