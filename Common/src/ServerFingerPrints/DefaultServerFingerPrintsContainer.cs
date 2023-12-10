using Resto.Data;

namespace Resto.Common
{
    /// <summary>
    /// Контейнер с "отпечатками" сервера по умолчанию.
    /// </summary>
    public class DefaultServerFingerPrintsContainer : IWithServerFingerPrints
    {
        /// <inheritdoc />
        public ServerFingerPrintsInfo ServerFingerPrintsInfo { get; set; }

        /// <inheritdoc />
        public bool HasFingerPrintsInfo => ServerFingerPrintsInfo != null;

        /// <inheritdoc />
        public void UpdateFingerPrints()
        {
            // do nothing
        }
    }
}