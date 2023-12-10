using System;

using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Документы: Возвратные накладные"
    /// </summary>
    public class ReturnedInvoiceProperties
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
        /// Отгрузить со склада
        /// </summary>
        public Store DefaultStore { get; set; }

        /// <summary>
        /// Приходная накладная
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