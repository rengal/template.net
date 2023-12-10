using System;
using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelPriceTagProperties"/>
    /// </summary>
    public interface IPriceTagProperties
    {
        /// <summary>
        /// Юр. лицо
        /// </summary>
        JurPerson JurPerson { get; }

        /// <summary>
        /// Торговое предприятие
        /// </summary>
        DepartmentEntity DepartmentEntity { get; }

        /// <summary>
        /// Дата печати ценников
        /// </summary>
        DateTime Date { get; }
    }
}