using System;
using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Framework.Data;

namespace Resto.Data
{
    using PaymentValidator = Func<AbstractScheduledPayment, bool>;

    public partial class AbstractScheduledPayment
    {
        /// <summary>
        /// Валидаторы полей платежа. Мап "Имя поля - Валидатор для этого поля"
        /// </summary>
        /// <remarks>
        /// При создании платежа валидация менее строгая, чем при его оплате
        /// (некоторые поля, необходимые для создания проводки, могут быть
        /// не заполнены). Поэтому перед оплатой платежей нужно дополнительно
        /// их проверить на наличие всех нужных данных.
        /// Валидация проводится не сразу при инстанцировании платежа, а при
        /// первой необходимости (т.е. когда этот платёж пытаются оплатить).
        /// В этом случае перед оплатой платежи валидируются, если какие-то
        /// из них невалидны, то они не оплачиваются, а в гриде подсвечиваются
        /// ячейки, соответствующие незаполненным полям платежа. Ячейки остаются
        /// подсвеченными до тех пор, пока платёж не отредактируют, либо пока
        /// грид не обновят.
        /// </remarks>
        [Transient]
        private readonly IDictionary<string, PaymentValidator> validators = new Dictionary
            <string, PaymentValidator>
        {
            {nameof(Number), payment => !string.IsNullOrWhiteSpace(payment.Number)},
            {nameof(Name), payment => !string.IsNullOrWhiteSpace(payment.Name)},
            {nameof(Sum), payment => payment.Sum > 0m},
            {nameof(WriteoffAccount), payment => payment.WriteoffAccount != null},
            {nameof(ExpenseAccount), payment => payment.ExpenseAccount != null},
            {nameof(Department), payment => payment.Department != null}
        };

        [Transient]
        [CanBeNull]
        private string[] invalidFields;

        [UsedImplicitly]
        public string Status
        {
            get { return Paid ? Resources.ScheduledPaymentPaid : Resources.ScheduledPaymentNotPaid; }
        }

        public void Validate()
        {
            invalidFields = validators
                .Where(pair => !pair.Value(this))
                .Select(pair => pair.Key)
                .ToArray();

            if (invalidFields.IsEmpty())
            {
                invalidFields = null;
            }
        }

        public bool IsInvalid(string fieldName = null)
        {
            return invalidFields != null &&
                   (string.IsNullOrWhiteSpace(fieldName) || invalidFields.Contains(fieldName));
        }

        public abstract AbstractScheduledPayment Copy();
    }
}