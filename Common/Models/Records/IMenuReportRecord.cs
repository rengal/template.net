using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    /// <summary>
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelMenuReportItem"/> из вью-модели грида "Прейскурант
    /// товаров и услуг" <see cref="MenuReportRecord"/>
    /// </summary>
    public interface IMenuReportRecord
    {
        /// <summary>
        /// Продукт с размером
        /// </summary>
        ProductSizeKey ProductSizeKey { get; }

        /// <summary>
        /// Расписание
        /// </summary>
        string ScheduleName { get; }

        /// <summary>
        /// Время
        /// </summary>
        string SchedulePeriods { get; }

        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Следующая цена
        /// </summary>
        decimal NextPrice { get; }

        /// <summary>
        /// Дата начала следующей цены
        /// </summary>
        DateTime? NextPriceDate { get; }

        /// <summary>
        /// Номер документа (либо обозначение "По карточке" для цен из карточек продуктов).
        /// </summary>
        string DocumentNumber { get; }
    }
}
