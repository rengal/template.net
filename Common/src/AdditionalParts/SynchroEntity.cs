using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class SynchroEntity
    {
        public string CashSystem
        {
            get
            {
                // Кассовая система есть только у Торговых Предприятий
                return (Department as Department != null && (Department as Department).CashSystem != null)
                           ? (Department as Department).CashSystem.GetLocalName()
                           : "";
            }
        }
    }
}