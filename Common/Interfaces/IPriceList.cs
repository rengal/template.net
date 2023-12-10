using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс, описывающий прайс-листы
    /// </summary>
    /// <typeparam name="TItem">Тип поддерживаемых прайс-листов</typeparam>
    public interface IPriceList<TItem> : IWithId<Guid> where TItem : AbstractIncomingPriceListItem
    {
        /// <summary>
        /// Дата начала действия прайс-листа
        /// </summary>
        DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата окончания действия прайс-листа
        /// </summary>
        DateTime? DateTo { get; set; }

        /// <summary>
        /// Информация об изменении пайс-листа
        /// </summary>
        [CanBeNull]
        OperationInfo ModifiedInfo { get; set; }

        /// <summary>
        /// Все элементы прайс-листа
        /// </summary>
        [NotNull]
        IReadOnlyCollection<TItem> AllItems { get; }

        /// <summary>
        /// Обновляет элементы прайс-листа
        /// </summary>
        /// <param name="newItems">Обновленная коллекция элементов</param>
        void UpdatePriceItems([NotNull] IReadOnlyCollection<TItem> newItems);
    }
}