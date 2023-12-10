using Resto.Framework.Data;

namespace Resto.Common.Localization
{
    /// <summary>
    /// Интерфейс Data-классов (см. <see cref="DataClassAttribute" />),
    /// обладающих локализуемым именем.
    /// </summary>
    /// <remarks>
    /// Data-классы, реализующие свойство NameResId, локализованы на сервере
    /// и содержат локализованные описания в ресурсных файлах.
    /// </remarks>
    public interface ILocalizableName
    {
        /// <summary>
        /// Возвращает ключ ресурса с локализованным описанием текущего объекта.
        /// </summary>
        string NameResId { get; }
    }
}
