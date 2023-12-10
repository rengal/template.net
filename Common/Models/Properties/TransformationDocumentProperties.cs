using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Акты переработки"
    /// </summary>
    public class TransformationDocumentProperties
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
        /// Со склада
        /// </summary>
        public Store StoreFrom { get; set; }

        /// <summary>
        /// На склад
        /// </summary>
        public Store StoreTo { get; set; }

        /// <summary>
        /// Продукт (разбираемый в документе)
        /// </summary>
        public IncomingDocumentRecord ProductRecord { get; set; }
    }
}