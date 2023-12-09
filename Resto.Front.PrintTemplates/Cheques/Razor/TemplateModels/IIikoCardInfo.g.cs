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
    /// Общая информация iikoCard
    /// </summary>
    public interface IIikoCardInfo
    {
        /// <summary>
        /// Владелец карты
        /// </summary>
        string CardOwner { get; }

        /// <summary>
        /// Номер карты
        /// </summary>
        string CardNumber { get; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Название операции, отображаемое на чеке
        /// </summary>
        string OperationName { get; }

        /// <summary>
        /// Имеет ли чек место для подписи покупателем
        /// </summary>
        bool HasSignature { get; }

        /// <summary>
        /// Имеет ли чек строку с суммой операции
        /// </summary>
        bool HasAmount { get; }

        /// <summary>
        /// Является ли оплатой
        /// </summary>
        bool IsPayment { get; }

        /// <summary>
        /// Отображать идентификаторы операций
        /// </summary>
        bool HasOperationIds { get; }

        /// <summary>
        /// Название серверного номера операции, отображаемое на чеке
        /// </summary>
        string OperationNumTitle { get; }

        /// <summary>
        /// Название клиентского номера операции (контрольного кода), отображаемое на чеке
        /// </summary>
        string RequestNumTitle { get; }

        /// <summary>
        /// Серверный номер операции
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// Клиентский номер операции (контрольный код)
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// Дата и время на сервере
        /// </summary>
        DateTime? HostTime { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Данные с сервера iikoCard
        /// </summary>
        [NotNull]
        IEnumerable<IIikoCardRow> ChequeRows { get; }

    }

    internal sealed class IikoCardInfo : TemplateModelBase, IIikoCardInfo
    {
        #region Fields
        private readonly string cardOwner;
        private readonly string cardNumber;
        private readonly decimal amount;
        private readonly string operationName;
        private readonly bool hasSignature;
        private readonly bool hasAmount;
        private readonly bool isPayment;
        private readonly bool hasOperationIds;
        private readonly string operationNumTitle;
        private readonly string requestNumTitle;
        private readonly string operationId;
        private readonly string requestId;
        private readonly DateTime? hostTime;
        private readonly string error;
        private readonly List<IikoCardRow> chequeRows = new List<IikoCardRow>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private IikoCardInfo()
        {}

        private IikoCardInfo([NotNull] CopyContext context, [NotNull] IIikoCardInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            cardOwner = src.CardOwner;
            cardNumber = src.CardNumber;
            amount = src.Amount;
            operationName = src.OperationName;
            hasSignature = src.HasSignature;
            hasAmount = src.HasAmount;
            isPayment = src.IsPayment;
            hasOperationIds = src.HasOperationIds;
            operationNumTitle = src.OperationNumTitle;
            requestNumTitle = src.RequestNumTitle;
            operationId = src.OperationId;
            requestId = src.RequestId;
            hostTime = src.HostTime;
            error = src.Error;
            chequeRows = src.ChequeRows.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.IikoCardRow.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static IikoCardInfo Convert([NotNull] CopyContext context, [CanBeNull] IIikoCardInfo source)
        {
            if (source == null)
                return null;

            return new IikoCardInfo(context, source);
        }
        #endregion

        #region Props
        public string CardOwner
        {
            get { return GetLocalizedValue(cardOwner); }
        }

        public string CardNumber
        {
            get { return GetLocalizedValue(cardNumber); }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string OperationName
        {
            get { return GetLocalizedValue(operationName); }
        }

        public bool HasSignature
        {
            get { return hasSignature; }
        }

        public bool HasAmount
        {
            get { return hasAmount; }
        }

        public bool IsPayment
        {
            get { return isPayment; }
        }

        public bool HasOperationIds
        {
            get { return hasOperationIds; }
        }

        public string OperationNumTitle
        {
            get { return GetLocalizedValue(operationNumTitle); }
        }

        public string RequestNumTitle
        {
            get { return GetLocalizedValue(requestNumTitle); }
        }

        public string OperationId
        {
            get { return GetLocalizedValue(operationId); }
        }

        public string RequestId
        {
            get { return GetLocalizedValue(requestId); }
        }

        public DateTime? HostTime
        {
            get { return hostTime; }
        }

        public string Error
        {
            get { return GetLocalizedValue(error); }
        }

        public IEnumerable<IIikoCardRow> ChequeRows
        {
            get { return chequeRows; }
        }

        #endregion
    }

}
