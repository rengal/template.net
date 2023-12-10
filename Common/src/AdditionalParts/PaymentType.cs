using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Common;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class PaymentType : IComparable, IComparable<PaymentType>
    {
        public override string ToString()
        {
            return NameLocal;
        }

        /// <summary>
        /// Строковое представление списка подразделений, для которых предназначен тип оплаты.
        /// Возвращает только активные подразделения.
        /// </summary>
        public string DepartmentList
        {
            get
            {
                var workingDepartments = MultiDepartments.Instance.WorkingDepartments.ToList();
                return Departments != null
                    ? Departments
                        .Intersect(workingDepartments)
                        .Select(department => department.NameLocal)
                        .JoinWithComma()
                    : Department.ALL_DEPARTMENTS;
            }
        }

        /// <summary>
        /// Используется ли тип оплаты для проведения скидок, предоставляемых системами лояльности.
        /// </summary>
        public bool IsUsedForDiscount()
        {
            var externalApiPaymentType = this as ExternalApiPaymentType;
            return externalApiPaymentType != null && externalApiPaymentType.IsIikoCard5ForDiscount();
        }

        /// <summary>
        /// По умолчанию тип оплаты нефискальный, кроме безналичных с установленной опцией "печатать чеки" ("является фискальным"). Где необходимо, это поведение переопределяется
        /// </summary>
        /// <returns> Является ли тип оплаты фискальным, если да - true, иначе - false</returns>
        /// <remarks>
        /// Симметричная реализация на сервере в resto.front.payment.PaymentType.isFiscalType.
        /// Бэковскую реализацию, сделанную по задаче RMS-40113, считаем первичной.
        /// </remarks>
        public virtual bool IsFiscalType()
        {
            return PaymentGroup == PaymentGroup.NON_CASH && printCheque;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((PaymentType)obj);
        }

        #endregion IComparable Members

        #region IComparable<PaymentType> Members

        public int CompareTo(PaymentType other)
        {
            if (other == null)
            {
                // В нашем случае объект меньше значения null
                // (поведение взял из ранней реализации IComparable, которую затем переписал).
                return -1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);

            // Если имена одинаковые, сравниваем по идентификаторам (сами обьекты могут быть разные).
            if (result == 0)
            {
                result = Id.CompareTo(other.Id);
            }

            return result;
        }

        #endregion
    }
}
