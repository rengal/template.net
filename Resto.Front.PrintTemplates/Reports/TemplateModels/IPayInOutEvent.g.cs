// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Событие внесения/изъятия
    /// </summary>
    public interface IPayInOutEvent
    {
        /// <summary>
        /// Дата
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Счёт
        /// </summary>
        [CanBeNull]
        IAccount Account { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Тип внесения/изъятия. При автоматических внесениях/изъятиях в процессе закрытия смены свойство равно null
        /// </summary>
        [CanBeNull]
        IPayInOutType PayInOutType { get; }

        /// <summary>
        /// Сумма внесения/изъятия
        /// </summary>
        decimal PaymentSum { get; }

        /// <summary>
        /// Аутентификационные данные пользователя, подтвердившего внесение/изъятие
        /// </summary>
        [CanBeNull]
        IAuthData Auth { get; }

        /// <summary>
        /// Залогиненный на момент события пользователь
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Контрагент
        /// </summary>
        IUser CounterAgent { get; }

        /// <summary>
        /// Тип транзакции события внесения/изъятия
        /// </summary>
        PayInOutTransactionType TransactionType { get; }

        /// <summary>
        /// Признак того, что внесение/изъятие было произведено за счет официанта
        /// </summary>
        bool IsWaiterDebt { get; }

        /// <summary>
        /// Идентификатор заказа. Обязательное поле при оплате заказа на сотрудника.
        /// </summary>
        Guid? OrderId { get; }

        /// <summary>
        /// Признак того, что было произведено изъятие денег, выданных курьеру на сдачу.
        /// </summary>
        bool IsPayOutForCourierChange { get; }

    }
}
