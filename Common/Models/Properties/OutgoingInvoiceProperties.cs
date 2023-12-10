using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Расходные накладные"
    /// </summary>
    public class OutgoingInvoiceProperties
    {
        #region Properties

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата и время получения
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
        /// Тип покупателя
        /// </summary>
        public CounteragentType BuyerType { get; set; }

        /// <summary>
        /// Покупатель
        /// </summary>
        public User Buyer { get; set; }

        /// <summary>
        /// Отгрузить со склада
        /// </summary>
        public Store DefaultStore { get; set; }

        /// <summary>
        /// Счет выручки
        /// </summary>
        public Account AccountRevenue { get; set; }

        /// <summary>
        /// Расчетный счет
        /// </summary>
        public Account AccountTo { get; set; }

        /// <summary>
        /// Международный номер банковского счета.
        /// </summary>
        public string Iban { get; set; }

        /// <summary>
        /// SWIFT-код.
        /// </summary>
        public string SwiftBic { get; set; }

        /// <summary>
        /// Регистрационный номер.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Срок оплаты.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Дата оплаты.
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        #endregion
    }
}