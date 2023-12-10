using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Кеш групп аллергенов.
    /// </summary>
    /// <remarks>
    /// Списки групп аллергенов не хранятся в явном виде,
    /// а каждый раз вычисляются по иерархии тех.карт.
    /// Поэтому их получение нужно оптимизировать, чтобы
    /// каждый раз не обращаться к серверу.
    /// </remarks>
    public interface IAllergenGroupsCache
    {
        /// <summary>
        /// Получить группы аллергенов для продукта из кеша
        /// или на сервере (если в кеше их ещё нет)
        /// </summary>
        [NotNull]
        HashSet<AllergenGroup> GetAllergens([NotNull] Product product);

        /// <summary>
        /// Получить группы аллергенов для продуктов из кеша
        /// или на сервере (если в кеше их ещё нет)
        /// </summary>
        [NotNull]
        IDictionary<Product, HashSet<AllergenGroup>> GetAllergens([NotNull] IEnumerable<Product> products);

        /// <summary>
        /// <c>true</c>, если данный кеш содержит группы аллергенов для указанных отделения и даты
        /// </summary>
        /// <remarks>
        /// Используется для того, чтобы можно было сравнивать между собой экземпляры кешей
        /// и при необходимости (при смене даты или ТП в вызывающем коде) создавать новый
        /// </remarks>
        bool ContainsAllergensDataFor([CanBeNull] DepartmentEntity department, DateTime date);
    }
}