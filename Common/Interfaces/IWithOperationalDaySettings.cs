using Resto.Data;

namespace Resto.Common
{
    /// <summary>
    /// Описывает сущность, содержащую настройки учетного дня.
    /// </summary>
    /// <remarks>
    /// Настройки обновляются на РМС при сохранении CafeSetup.
    /// На стороне Чейна не редактируются.
    /// На РМС должны совпадать с настройкой из CafeSetup.
    /// Не получилось полностью вынести из CafeSetup в DepartmentEntity
    /// и Corporation, т.к., неизвестно в каких отчетах какую дату использовать
    /// чтобы не было расхождений с чейном.
    /// </remarks>
    public interface IWithOperationalDaySettings
    {
        /// <summary>
        /// Настройки операционного времени.
        /// </summary>
        BusinessDateSettings BusinessDateSettings { get; }

        /// <summary>
        /// Настройки операционного дня.
        /// </summary>
        OperationalDaySettings OperationalDaySettings { get; }
    }
}