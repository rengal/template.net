// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Событие внесения/изъятия
    /// </summary>
    public interface IPayInOutEvent
    {
        /// <summary>
        /// Сумма внесения/изъятия (абсолютное значение)
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Признак внесения
        /// </summary>
        bool IsPayIn { get; }

        /// <summary>
        /// Признак того, что внесение является пополнением разменного фонда
        /// </summary>
        bool IsChangePayIn { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Название типа события
        /// </summary>
        [CanBeNull]
        string EventTypeName { get; }

        /// <summary>
        /// Счёт
        /// </summary>
        [CanBeNull]
        IAccount Account { get; }

        /// <summary>
        /// Контрагент
        /// </summary>
        [CanBeNull]
        IUser Counteragent { get; }

    }

    internal sealed class PayInOutEvent : TemplateModelBase, IPayInOutEvent
    {
        #region Fields
        private readonly decimal sum;
        private readonly bool isPayIn;
        private readonly bool isChangePayIn;
        private readonly string comment;
        private readonly string eventTypeName;
        private readonly Account account;
        private readonly User counteragent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PayInOutEvent()
        {}

        private PayInOutEvent([NotNull] CopyContext context, [NotNull] IPayInOutEvent src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            isPayIn = src.IsPayIn;
            isChangePayIn = src.IsChangePayIn;
            comment = src.Comment;
            eventTypeName = src.EventTypeName;
            account = context.GetConverted(src.Account, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Account.Convert);
            counteragent = context.GetConverted(src.Counteragent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PayInOutEvent Convert([NotNull] CopyContext context, [CanBeNull] IPayInOutEvent source)
        {
            if (source == null)
                return null;

            return new PayInOutEvent(context, source);
        }
        #endregion

        #region Props
        public decimal Sum
        {
            get { return sum; }
        }

        public bool IsPayIn
        {
            get { return isPayIn; }
        }

        public bool IsChangePayIn
        {
            get { return isChangePayIn; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public string EventTypeName
        {
            get { return GetLocalizedValue(eventTypeName); }
        }

        public IAccount Account
        {
            get { return account; }
        }

        public IUser Counteragent
        {
            get { return counteragent; }
        }

        #endregion
    }

}
