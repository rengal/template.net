using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using System;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class PaymentStrategy
    {
        /// <summary>
        /// Возвращает значение оплаты за час по должности из стратегии оплаты
        /// Если нет стратегии оплаты (новая должность) то возвращается значение из должности или 0 в зависимости от выбора пользователя
        /// </summary>
        /// <param name="role">Должность</param>
        /// <param name="zeroPaymentForAddedRoles">
        /// Нужно ли по умолчанию задавать нулевые ставки оплаты, если для пользователя (или группы пользователей) должности по этой записи являются новыми
        /// True - задавать нулевые
        /// False - брать тарифы из должностей
        /// null - не было добавлено новых должностей, ничего не задавать. Либо была добавлена окладная должность, к уже существующим окладным должностям
        /// </param>
        /// <returns>Оплата за час</returns>
        public decimal? GetPaymentPerHourByRole([NotNull] Role role, bool? zeroPaymentForAddedRoles)
        {
            decimal? result;
            if (role.Equals(Specification.MainRole))
            {
                result = PaymentPerHour;
            }
            else
            {
                var specItem = Specification.Items.SingleOrDefault(item => role.Equals(item.Role));
                result = specItem == null ? null : specItem.PaymentPerHour;
            }

            if (result != null)
            {
                return result;
            }
            
            // Если это окладная должность и у пользователя уже были выбраны другие окладные должности, до редактирования тарифов
            // то значение берется из стратегии оплаты по старой должности
            var oldSalaryRole = employee.Roles.FirstOrDefault(r => r.ScheduleType == RoleScheduleType.STEADY_SALARY);

            if (role.ScheduleType == RoleScheduleType.STEADY_SALARY && oldSalaryRole != null)
            {
                return GetPaymentPerHourByRole(oldSalaryRole, null);
            }

            if (zeroPaymentForAddedRoles != null)
            {
                // В зависимости от того, что нужно сделать с новой должностью - данные берутся из должности или задается нулевое значение
                return zeroPaymentForAddedRoles.Value ? 0m : role.PaymentPerHour;
            }

            // Если role это новая должность и для пользователя нет данных в стратегиях оплаты по этой должности, 
            // и при этом не определено, что делать с новыми должностями, это ошибка
            var isNewRole = !employee.Roles.Contains(role);
            if (isNewRole)
            {
                throw new Exception(string.Format("Not found tariff for the new role ({0}) for user ({1})",
                    role,
                    Employee)); 
            }
            // Если это не новая должность и не задана ставка оплаты (RMS-45868 - возможно для старых бд), задаем нулевую ставку
            return 0m;           
        }

        /// <summary>
        /// Возвращает значение оклада по должности из стратегии оплаты
        /// Если нет стратегии оплаты (новая должность) то возвращается значение из должности или 0 в зависимости от выбора пользователя
        /// </summary>
        /// <param name="role">Должность</param>
        /// <param name="zeroPaymentForAddedRoles">
        /// Нужно ли по умолчанию задавать нулевые ставки оплаты, если для пользователя(или группы пользователей) должности по этой записи являются новыми
        /// True - задавать нулевые
        /// False - брать тарифы из должностей
        /// null - не было добавлено новых должностей, ничего не задавать. Либо была добавлена окладная должность, к уже существующим окладным должностям
        /// </param>
        /// <returns>Значение оклада</returns>
        public decimal? GetSteadySalaryByRole([NotNull] Role role, bool? zeroPaymentForAddedRoles)
        {
            decimal? result;
            if (role.Equals(Specification.MainRole))
            {
                result = SteadySalary;
            }
            else
            {
                var specItem = Specification.Items.SingleOrDefault(item => role.Equals(item.Role));
                result = specItem == null ? null : specItem.SteadySalary;    
            }
            
            if (result != null)
            {
                return result;
            }

            // Если это окладная должность и у пользователя уже были выбраны другие окладные должности, до редактирования тарифов
            // то значение берется из стратегии оплаты по старой должности
            var oldSalaryRole = employee.Roles.FirstOrDefault(r => r.ScheduleType == RoleScheduleType.STEADY_SALARY);

            if (role.ScheduleType == RoleScheduleType.STEADY_SALARY && oldSalaryRole != null)
            {
                return GetSteadySalaryByRole(oldSalaryRole, null);
            }

            if (zeroPaymentForAddedRoles != null)
            {
                // В зависимости от того, что нужно сделать с новой должностью - данные берутся из должности или задается нулевое значение
                return zeroPaymentForAddedRoles.Value ? 0m : role.SteadySalary;
            }

            // Если role это новая должность и для пользователя нет данных в стратегиях оплаты по этой должности, 
            // и при этом не определено, что делать с новыми должностями, это ошибка
            var isNewRole = !employee.Roles.Contains(role);
            if (isNewRole)
            {
                throw new Exception(string.Format("Not found tariff for the new role ({0}) for user ({1})",
                    role,
                    Employee));
            }
            // Если это не новая должность и не задана ставка оплаты (RMS-45868 - возможно для старых бд), задаем нулевую ставку
            return 0m;    
        }

        /// <summary>
        /// Возвращает значение типа оплаты по должности
        /// </summary>
        /// <param name="role">Должность</param>
        /// <returns>Значение типа оплаты</returns>
        public RoleScheduleType GetScheduleTypeByRole([NotNull] Role role)
        {
            if (role.Equals(Specification.MainRole))
            {
                return ScheduleType;
            }
            var specItem = Specification.Items.SingleOrDefault(item => role.Equals(item.Role));
            return specItem == null ? null : specItem.ScheduleType;
        }

        public override string ToString()
        {
            return new ToStringBuilder<PaymentStrategy>()
                .AddProperty(ps => ps.Id)
                .AddProperty(ps => ps.DateFrom)
                .AddProperty(ps => ps.DateTo)
                .AddProperty(ps => ps.PaymentPerHour)
                .AddProperty(ps => ps.Specification)
                .GetString(this);
        }
    }
}
