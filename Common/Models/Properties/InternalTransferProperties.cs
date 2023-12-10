using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Внутреннее перемещение"
    /// </summary>
    public class InternalTransferProperties
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
        /// Склад источник
        /// </summary>
        public Store StoreFrom { get; set; }

        /// <summary>
        /// Склад получатель
        /// </summary>
        public Store StoreTo { get; set; }
    }
}