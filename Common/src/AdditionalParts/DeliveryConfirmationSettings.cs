using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class DeliveryConfirmationSettings
    {
        [CanBeNull]
        public static DeliveryConfirmationSettings Instance
        {
            get { return EntityManager.INSTANCE.GetSingleton<DeliveryConfirmationSettings>(); }
        }

        /// <summary>
        /// Проверяет, что настройки подтверждения инициализированы.
        /// Не инициализированы, если это реплицирующийся РМС до первой репликации.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если данные инициализированы.
        /// </returns>
        public static bool CheckInitialized()
        {
            return Instance != null || !CompanySetup.IsReplicationConfigured;
        }
    }
}
