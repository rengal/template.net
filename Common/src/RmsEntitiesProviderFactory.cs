using Resto.Framework.Data;

namespace Resto.Data
{
    public class RmsEntitiesProviderFactory : IEntitiesProviderFactory
    {
        public IEntitiesProvider Create()
        {
            return new RMSEntityManager();
        }
    }
}