using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Приходные накладные"
    /// </summary>
    public class IncomingInvoiceProperties
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
        /// Входной номер документа
        /// </summary>
        public string IncomingNumber { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public User Supplier { get; set; }

        /// <summary>
        /// Склад
        /// </summary>
        public Store DefaultStore { get; set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public User Employee { get; set; }

        /// <summary>
        /// Счет-фактура
        /// </summary>
        public string InvoiceText { get; set; }

        /// <summary>
        /// Дата "ОТ" (пример, документ от "число")
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Срок оплаты
        /// </summary>
        public DateTime? DateDue { get; set; }

        /// <summary>
        /// Товарно-транспортная накладная
        /// </summary>
        public string TransportInvoiceNumber { get; set; }

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

        #endregion
    }
}