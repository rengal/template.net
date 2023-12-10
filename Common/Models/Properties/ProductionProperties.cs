using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Акты приготовления"
    /// </summary>
    public class ProductionProperties
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата и время
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
        /// Склад
        /// </summary>
        public Store StoreFrom { get; set; }

        /// <summary>
        /// На склад
        /// </summary>
        public Store StoreTo { get; set; }
    }
}