using System;

namespace Resto.Common
{
    /// <summary>
    /// Контейнер с "отпечатками" сервера.
    /// </summary>
    public class ServerFingerPrintsContainer
    {
        private static readonly Lazy<DefaultServerFingerPrintsContainer> Lazy =
            new Lazy<DefaultServerFingerPrintsContainer>(() => new DefaultServerFingerPrintsContainer());

        private static readonly object Locker = new object();
        private static IWithServerFingerPrints instance;

        public static IWithServerFingerPrints Instance
        {
            get
            {
                lock (Locker)
                {
                    if (instance != null)
                    {
                        return instance;
                    }
                }

                return Lazy.Value;
            }
            set
            {
                lock (Locker)
                {
                    instance = value;
                }
            }
        }
    }
}