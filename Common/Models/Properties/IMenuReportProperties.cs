using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelMenuReportProperties"/>
    /// </summary>
    public interface IMenuReportProperties
    {
        /// <summary>
        /// Дата
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Ценовая категория
        /// </summary>
        ClientPriceCategory UserSelectedPriceCategory { get; }

        /// <summary>
        /// Торговое предприятие
        /// </summary>
        Department SelectedDepartment { get; }

        /// <summary>
        /// Названия периодов действия
        /// </summary>
        string ScheduleNames { get; }
    }
}
