using System;
using Resto.Data;

namespace Resto.Common.Models.Records
{
    /// <summary>
    /// Запись товарного отчета (общая)
    /// </summary>
    public sealed class Torg29Record : Torg29RecordBase
    {
        /// <summary>
        /// Сумма тары
        /// </summary>
        public decimal ContainerSumm { get; set; }

        /// <summary>
        /// Сумма товара
        /// </summary>
        public decimal GoodsSumm { get; set; }

        /// <summary>
        /// Сумма документа
        /// </summary>
        public decimal DocumentSum { get; set; }

        /// <summary>
        /// Наименование позиции
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public User Supplier { get; set; }

        /// <summary>
        /// Идентификатор документа.
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Название для отчета.
        /// </summary>
        public string ReportName { get; set; }
    }
}