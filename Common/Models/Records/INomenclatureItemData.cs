using System.Collections.Generic;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Models
{
    /// <summary>
    /// Данные карточки элемента номенклатуры.
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelNomenclatureItem"/>
    /// </summary>
    public interface INomenclatureItemData
    {
        /// <summary>
        /// Продукты
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Размер блюда
        /// </summary>
        [CanBeNull]
        ProductSize ProductSize { get; }

        /// <summary>
        /// Торговое предприятие
        /// </summary>
        [CanBeNull]
        DepartmentEntity Department { get; }

        /// <summary>
        /// Товары поставщиков
        /// </summary>
        IEnumerable<ISupplierInfo> SupplierInfos { get; }

        /// <summary>
        /// Пищевая ценность
        /// </summary>
        IEnumerable<IFoodValueRecord> FoodValueRecords { get; }

        /// <summary>
        /// Группы аллергенов.
        /// </summary>
        IEnumerable<AllergenGroup> AllergenGroups { get; } 

        /// <summary>
        /// Инициализация данных
        /// </summary>
        void Init();
    }
}