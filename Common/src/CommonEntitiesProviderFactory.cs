using Microsoft.Extensions.DependencyInjection;
using Resto.Framework.Data;

namespace Resto.Data
{
    public sealed class CommonEntitiesProviderFactory : IEntitiesProviderFactory
    {
        public IEntitiesProvider Create()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var instance = ServiceProviderServiceExtensions.GetService<IBaseEntityManager>(serviceProvider);
            return instance;
            //return UnityHelper.GetFactoryContainer().Resolve<IBaseEntityManager>();
        }
    }
}