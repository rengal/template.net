using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Акты продаж"
    /// </summary>
    public class SalesInvoiceProperties
    {
        #region Properties

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата и время выдачи товара
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Концепция
        /// </summary>
        public Conception Conception { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Место списания
        /// </summary>
        public Store DefaultStore { get; set; }

        /// <summary>
        /// Счет выручки
        /// </summary>
        public Account AccountRevenue { get; set; }

        /// <summary>
        /// Расходный счет
        /// </summary>
        public Account AccountTo { get; set; }

        #endregion
    }
}