using Resto.Data;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common
{
    /// <summary>
    /// Интерфейс, описывающий класс, содержащий "отпечатки" сервера.
    /// </summary>
    public interface IWithServerFingerPrints
    {
        /// <summary>
        /// "Отпечатки" сервера.
        /// </summary>
        [CanBeNull]
        ServerFingerPrintsInfo ServerFingerPrintsInfo { get; set; }

        /// <summary>
        /// Есть ли "отпечатки" сервера.
        /// </summary>
        bool HasFingerPrintsInfo { get; }

        /// <summary>
        /// Обновление "отпечатков".
        /// </summary>
        void UpdateFingerPrints();
    }
}