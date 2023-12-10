using System;
using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Возвраты от покупателей"
    /// </summary>
    public class IncomingReturnedInvoiceProperties
    {
        #region Properties

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата и время возврата
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
        /// Оприходовать на склад
        /// </summary>
        public Store DefaultStore { get; set; }

        /// <summary>
        /// Расходная накладная накладная
        /// </summary>
        public String InvoiceText { get; set; }

        /// <summary>
        /// Поставщик 
        /// </summary>
        public User Supplier { get; set; }

        /// <summary>
        /// Расходный счет
        /// </summary>
        public Account AccountTo { get; set; }

        /// <summary>
        /// Счет выручки
        /// </summary>
        public Account RevenueAccount { get; set; }

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
