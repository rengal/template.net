using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class ScheduleType
    {
        #region Contants

        /// <summary>
        /// Максимальная длина смены (в днях)
        /// </summary>
        public const int MAX_DAY_SESSION = 2;

        #endregion Contants

        /// <summary>
        /// True, если у смены явка по умолчанию, имеет статус не явка.
        /// </summary>
        public bool IsAbsenceDefaultAttendance
        {
            get
            {
                return defaultAttendanceType != null && !defaultAttendanceType.Status.GetValueOrFakeDefault();
            }
        }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}