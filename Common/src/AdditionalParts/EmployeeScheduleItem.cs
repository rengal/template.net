using System;
using System.Text;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;

namespace Resto.Data
{
    /// <summary>
    /// Смена.
    /// </summary>
    public partial class EmployeeScheduleItem
    {
        private string stringValue = string.Empty;

        public EmployeeScheduleItem GetDeepClone()
        {
            return (EmployeeScheduleItem)DeepClone(Guid.NewGuid());
        }

        /// <summary>
        /// Если типовая смена была изменена.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                // Если изменено 
                //  + время начала
                //  + длительность смены                
                //  + длительность неоплачиваемого времени
                return !DateFrom.GetValueOrFakeDefault().TimeOfDay.TotalMinutes.EqualsEx(ScheduleType.Start.Minutes) ||
                       !(DateTo.GetValueOrFakeDefault() - DateFrom.GetValueOrFakeDefault()).TotalMinutes.EqualsEx(ScheduleType.Length) || 
                       NonPaidMinutes != ScheduleType.NonPaidMinutes;
            }
        }

        /// <summary>
        /// String value, которое выводится в ячейку грида.
        /// </summary>
        public string StringValue
        {
            set { stringValue = value; }

            get { return stringValue; }
        }
        
        #region Overridden

        public override bool Equals(object obj)
        {
            var item = (EmployeeScheduleItem) obj;

            return item != null && Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return StringValue;
        }

        #endregion
    }
}