using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Акты списания"
    /// </summary>
    public class WriteoffProperties
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
        public Store DefaultStore { get; set; }

        /// <summary>
        /// На склад
        /// </summary>
        public Store StoreTo { get; set; }

        /// <summary>
        /// На счет
        /// </summary>
        public Account AccountTo { get; set; }
    }
}