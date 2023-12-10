using Microsoft.Extensions.DependencyInjection;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public sealed class CommonEntitiesProviderFactory : IEntitiesProviderFactory
    {
        public IEntitiesProvider Create()
        {
            var instance = ServiceProviderExtensions.GetService<IBaseEntityManager>();
            return instance;
        }
    }
}