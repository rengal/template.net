using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Настройки учёта рабочего времени.
    /// </summary>
    public partial class SalarySettings
    {
        public static SalarySettings Settings
        {
            get
            {
                var ss = EntityManager.INSTANCE.GetSingleton<SalarySettings>();

                if (ss == null)
                {
                    throw new RestoException(string.Format("{0} object doesn't exist.", nameof(SalarySettings)));
                }
                return ss;
            }
        }

        /// <summary>
        /// Возвращает оклад по значению почасовой оплаты, исходя из количества рабочих часов в месяц
        /// </summary>
        /// <param name="perHourPayment">Почасовая оплата</param>
        /// <returns>Оценочная величина оклада</returns>
        public static decimal GetSteadySalary(decimal perHourPayment)
        {
            return perHourPayment * Settings.WorkingHoursPerMonth;
        }

        /// <summary>
        /// Возвращает величину почасовой оплаты по окладу, исходя из количества рабочих часов в месяц
        /// </summary>
        /// <param name="steadySalary">Оклад</param>
        /// <returns>Оценочная величина почасовой оплаты</returns>
        public static decimal GetPerHourPayment(decimal steadySalary)
        {
            return Settings.WorkingHoursPerMonth != decimal.Zero ? steadySalary / Settings.WorkingHoursPerMonth : decimal.Zero;
        }
    }
}