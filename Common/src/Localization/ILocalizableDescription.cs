using Resto.Framework.Data;

namespace Resto.Common.Localization
{
    /// <summary>
    /// Интерфейс Data-классов (см. <see cref="DataClassAttribute" />),
    /// обладающих локализуемым описанием.
    /// </summary>
    /// <remarks>
    /// Data-классы, реализующие свойство DescriptionResId, локализованы на сервере
    /// и содержат локализованные описания в ресурсных файлах.
    /// </remarks>
    public interface ILocalizableDescription
    {
        /// <summary>
        /// Возвращает ключ ресурса с локализованным описанием текущего объекта.
        /// </summary>
        string DescriptionResId { get; }
    }
}
