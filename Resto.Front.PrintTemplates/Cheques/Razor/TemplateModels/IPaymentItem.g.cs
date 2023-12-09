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
    /// Элемент оплаты заказа
    /// </summary>
    public interface IPaymentItem
    {
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        IPaymentType Type { get; }

        /// <summary>
        /// Тип чаевых
        /// </summary>
        [CanBeNull]
        IDonationType DonationType { get; }

        /// <summary>
        /// Флаг предоплаты
        /// </summary>
        bool IsPrepay { get; }

        /// <summary>
        /// Флаг предварительно добавленной оплаты (не предоплата!)
        /// </summary>
        bool IsPreliminary { get; }

        /// <summary>
        /// Является ли внешняя позиция оплаты заранее проведенной вовне
        /// </summary>
        bool IsProcessedExternally { get; }

        /// <summary>
        /// Статус
        /// </summary>
        PaymentItemStatus Status { get; }

        /// <summary>
        /// Признак того что предоплату не удалось удалить при сторнировании
        /// </summary>
        bool FailedToRemovePrepayOnStorno { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Контрагент при оплате в кредит
        /// </summary>
        [CanBeNull]
        [Obsolete("Используйте ICreditPaymentItem.Counteragent.")]
        IUser CreditCounteragent { get; }

        /// <summary>
        /// Информация об оплате в дополнительной валюте. null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        ICurrencyInfo CurrencyInfo { get; }

        /// <summary>
        /// Идентификатор гостя в заказе, которому принадлежит оплата.
        /// </summary>
        Guid? GuestId { get; }

    }

    internal class PaymentItem : TemplateModelBase, IPaymentItem
    {
        #region Fields
        private readonly decimal sum;
        private readonly PaymentType type;
        private readonly DonationType donationType;
        private readonly bool isPrepay;
        private readonly bool isPreliminary;
        private readonly bool isProcessedExternally;
        private readonly PaymentItemStatus status;
        private readonly bool failedToRemovePrepayOnStorno;
        private readonly string comment;
        private readonly User creditCounteragent;
        private readonly CurrencyInfo currencyInfo;
        private readonly Guid? guestId;
        #endregion

        #region Ctor
        protected PaymentItem()
        {}

        protected PaymentItem([NotNull] CopyContext context, [NotNull] IPaymentItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            sum = src.Sum;
            type = context.GetConverted(src.Type, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
            donationType = context.GetConverted(src.DonationType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DonationType.Convert);
            isPrepay = src.IsPrepay;
            isPreliminary = src.IsPreliminary;
            isProcessedExternally = src.IsProcessedExternally;
            status = src.Status;
            failedToRemovePrepayOnStorno = src.FailedToRemovePrepayOnStorno;
            comment = src.Comment;
            creditCounteragent = context.GetConverted(src.CreditCounteragent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            currencyInfo = context.GetConverted(src.CurrencyInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CurrencyInfo.Convert);
            guestId = src.GuestId;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PaymentItem Convert([NotNull] CopyContext context, [CanBeNull] IPaymentItem source)
        {
            if (source == null)
                return null;

            return new PaymentItem(context, source);
        }
        #endregion

        #region Props
        public decimal Sum
        {
            get { return sum; }
        }

        public IPaymentType Type
        {
            get { return type; }
        }

        public IDonationType DonationType
        {
            get { return donationType; }
        }

        public bool IsPrepay
        {
            get { return isPrepay; }
        }

        public bool IsPreliminary
        {
            get { return isPreliminary; }
        }

        public bool IsProcessedExternally
        {
            get { return isProcessedExternally; }
        }

        public PaymentItemStatus Status
        {
            get { return status; }
        }

        public bool FailedToRemovePrepayOnStorno
        {
            get { return failedToRemovePrepayOnStorno; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public IUser CreditCounteragent
        {
            get { return creditCounteragent; }
        }

        public ICurrencyInfo CurrencyInfo
        {
            get { return currencyInfo; }
        }

        public Guid? GuestId
        {
            get { return guestId; }
        }

        #endregion
    }

}
