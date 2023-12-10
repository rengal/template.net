using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Используется для тестов.
    /// </summary>
    public class TestRmsEntitiesProviderFactory : IEntitiesProviderFactory
    {
        public IEntitiesProvider Create()
        {
            return new RMSEntityManager();
        }
    }
}