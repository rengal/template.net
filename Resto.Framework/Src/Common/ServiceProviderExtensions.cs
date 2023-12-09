using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="serviceProvider">Object that provides custom support to other objects.</param>
        /// <returns>A service object of type serviceType. -or- null if there is no service object of type serviceType.</returns>
        [CanBeNull]
        public static T TryGetService<T>([NotNull] this IServiceProvider serviceProvider) where T : class
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            return (T)serviceProvider.GetService(typeof(T));
        }

        [NotNull]
        public static T GetService<T>(this IServiceProvider serviceProvider)  where  T : class
        {
            var service = TryGetService<T>(serviceProvider);
            if (service == null)
                throw new InvalidOperationException("Cannot find service in serviceProvider");

            return service;
        }
    }
}
