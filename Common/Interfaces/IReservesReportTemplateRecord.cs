using Resto.Data;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Данные для печатной формы "Отчет о банкете".
    /// </summary>
    public interface IReservesReportTemplateRecord
    {
        /// <summary>
        /// Зал
        /// </summary>
        string RestaurantSection { get; }

        /// <summary>
        /// Столы
        /// </summary>
        string ReservedTables { get; }

        /// <summary>
        /// Тип (банкет / резерв)
        /// </summary>
        string BrdType { get; }

        /// <summary>
        /// Тип события
        /// </summary>
        string ActivityType { get; }

        /// <summary>
        /// Предоплата
        /// </summary>
        decimal PrepaySum { get; }

        /// <summary>
        /// Резерв
        /// </summary>
        Reserve Reserve { get; }
    }
}