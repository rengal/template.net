using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Стоп-лист
    /// </summary>
    public partial class FrontBalances
    {
        [CanBeNull]
        public static FrontBalances Instance
        {
            get
            {
                return EntityManager.INSTANCE.GetSingleton<FrontBalances>();
            }
        }

        /// <summary>
        /// Использовать отдельные стоп-листы для каждой группы
        /// </summary>
        public bool IsUseStopListForGroup
        {
            get { return balancesByGroups != null; }
        }

    }
}
