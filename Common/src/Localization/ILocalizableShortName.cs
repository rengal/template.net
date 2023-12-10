using Resto.Framework.Data;

namespace Resto.Common.Localization
{
    /// <summary>
    /// Интерфейс Data-классов (см. <see cref="DataClassAttribute" />),
    /// обладающих локализуемым сокращенным именем.
    /// </summary>
    /// <remarks>
    /// Data-классы, реализующие свойство ShortNameResId, локализованы на сервере
    /// и содержат локализованные имена в ресурсных файлах.
    /// </remarks>
    public interface ILocalizableShortName
    {
        /// <summary>
        /// Возвращает ключ ресурса с локализованным коротким именем текущего объекта.
        /// </summary>
        string ShortNameResId { get; }
    }
}
