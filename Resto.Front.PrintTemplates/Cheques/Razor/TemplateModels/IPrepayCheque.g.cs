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
    /// Чек внесения/возврата предоплаты
    /// </summary>
    public interface IPrepayCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Информация о кассовом чеке
        /// </summary>
        [NotNull]
        ICashChequeInfo ChequeInfo { get; }

        /// <summary>
        /// Флаг возврата
        /// </summary>
        bool IsReturn { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Obsolete("Оставлено для обратной совместимости. Нужно использовать IPrepayCheque.Prepays.")]
        decimal Sum { get; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [NotNull]
        [Obsolete("Оставлено для обратной совместимости. Нужно использовать IPrepayCheque.Prepays.")]
        IPaymentType PaymentType { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        [Obsolete("Оставлено для обратной совместимости. Нужно использовать IPrepayCheque.Prepays.")]
        string Comment { get; }

        /// <summary>
        /// Информация об оплате в дополнительной валюте. null, если оплата в основной валюте
        /// </summary>
        [CanBeNull]
        [Obsolete("Оставлено для обратной совместимости. Нужно использовать IPrepayCheque.Prepays.")]
        ICurrencyInfo CurrencyInfo { get; }

        /// <summary>
        /// Предоплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPrepayInfo> Prepays { get; }

    }

    internal sealed class PrepayCheque : TemplateModelBase, IPrepayCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly bool isReturn;
        private readonly Order order;
        private readonly decimal sum;
        private readonly PaymentType paymentType;
        private readonly string comment;
        private readonly CurrencyInfo currencyInfo;
        private readonly List<PrepayInfo> prepays = new List<PrepayInfo>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PrepayCheque()
        {}

        internal PrepayCheque([NotNull] CopyContext context, [NotNull] IPrepayCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            isReturn = src.IsReturn;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            sum = src.Sum;
            paymentType = context.GetConverted(src.PaymentType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PaymentType.Convert);
            comment = src.Comment;
            currencyInfo = context.GetConverted(src.CurrencyInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CurrencyInfo.Convert);
            prepays = src.Prepays.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PrepayInfo.Convert)).ToList();
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public ICashChequeInfo ChequeInfo
        {
            get { return chequeInfo; }
        }

        public bool IsReturn
        {
            get { return isReturn; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public IPaymentType PaymentType
        {
            get { return paymentType; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public ICurrencyInfo CurrencyInfo
        {
            get { return currencyInfo; }
        }

        public IEnumerable<IPrepayInfo> Prepays
        {
            get { return prepays; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IPrepayCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new PrepayCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IPrepayCheque>(copy, "PrepayCheque");
        }
    }
}
