using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Отчеты: Инвентаризация"
    /// </summary>
    public class InventoryDocumentProperties
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTime Datе { get; set; }

        /// <summary>
        /// Концепция
        /// </summary>
        public Conception Conception { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Склад
        /// </summary>
        public Store Store { get; set; }

        /// <summary>
        /// Излишки (cчет)
        /// </summary>
        public Account SurplusAccount { get; set; }

        /// <summary>
        /// Излишки (сумма)
        /// </summary>
        public decimal SurplusSum { get; set; }

        /// <summary>
        /// Недостача (счет)
        /// </summary>
        public Account ShortageAccount { get; set; }

        /// <summary>
        /// Недостача (сумма)
        /// </summary>
        public decimal ShortageSum { get; set; }
    }
}