using System;
using System.Collections.Generic;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Отчеты: РОСВ"
    /// </summary>
    public class GoodsMoveReportProperties
    {
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Окончания периода
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Склады
        /// </summary>
        public List<Store> Stores { get; set; }

        /// <summary>
        /// Категория продукта
        /// </summary>
        public ProductCategory Category { get; set; }
    }
}