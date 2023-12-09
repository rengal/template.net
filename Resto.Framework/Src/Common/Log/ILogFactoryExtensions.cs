using System;
using Resto.Framework.Attributes.JetBrains;
using log4net;

namespace Resto.Framework.Common
{
    public static class ILoggingFactoryExtensions
    {
        [NotNull]
        public static ILog GetLogger<T>([NotNull] this ILogFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return factory.GetLogger(typeof(T));
        }
    }
}
