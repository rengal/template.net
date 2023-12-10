using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class ServiceProviderExtensions
    {
        private static IServiceProvider serviceProvider;

        public static void SetServiceProvider(IServiceProvider value)
        {
            serviceProvider = value;
        }

        [NotNull]
        public static T GetService<T>()  where  T : class
        {
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Call SetServiceProvider first");
            }

            if (serviceProvider.GetService(typeof(T)) is not T service)
                throw new InvalidOperationException("Init service first");
            return service;
        }

    }
}
