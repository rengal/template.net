using System;

namespace Resto.Common.Models
{
    /// <summary>
    /// Контейнер для полей UI "Отчеты: Товарный отчет"
    /// </summary>
    public class TradeReportProperties
    {
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Окончания периода
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Сумма всех приходов
        /// </summary>
        public decimal IncomeSum { get; set; }

        /// <summary>
        /// Сумма всех расходов
        /// </summary>
        public decimal OutcomeSum { get; set; }

        /// <summary>
        /// Начальный баланс складов
        /// </summary>
        public decimal StartSum { get; set; }

        /// <summary>
        /// Конечный баланс складов
        /// </summary>
        public decimal EndSum { get; set; }
    }
}