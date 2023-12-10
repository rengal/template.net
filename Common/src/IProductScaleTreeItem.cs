using System;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Элемент в дереве "Шкалы размеров" (см. <see cref="ProductScalesControl"/>).
    /// Представляет шкалу <see cref="ProductScale"/> или входящий в шкалу размер <see cref="ProductSize"/>.
    /// </summary>
    public interface IProductScaleTreeItem : IDeletable
    {
        /// <summary>
        /// Идентификатор элемента
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Идентификатор родительского элемента
        /// </summary>
        /// <remarks>
        /// Используется контролом <see cref="TreeList"/> для преобразования
        /// списка <see cref="IProductScaleTreeItem"/> в древовидное представление.
        /// </remarks>
        Guid? ParentId { get; }

        /// <summary>
        /// Колонка "Название"
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Колонка "Название для кухни"
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Колонка "По умолчанию"
        /// </summary>
        bool? IsDefault { get; }

        /// <summary>
        /// Шкала
        /// </summary>
        /// <remarks>
        /// Для элемента, представляющего размер шкалы,
        /// возвращается шкала, в которую входит этот размер.
        /// </remarks>
        ProductScale Scale { get; }

        /// <summary>
        /// <c>true</c>, если родительский элемент удалён
        /// </summary>
        bool IsParentDeleted { get; }
    }
}