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
    /// Квитанция об оплате/возврате
    /// </summary>
    public interface IReceiptCheque : ITemplateRootModel
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
        /// Чек для печати на ФРе
        /// </summary>
        [NotNull]
        IChequeTask ChequeTask { get; }

        /// <summary>
        /// Флаг, определяющий, нужно ли печатать полный чек
        /// </summary>
        bool IsFullCheque { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Дополнительные элементы
        /// </summary>
        [NotNull]
        IReceiptChequeExtensions Extensions { get; }

        /// <summary>
        /// Первоначальный заказ, если данный заказ результат деления по гостям
        /// </summary>
        [CanBeNull]
        IOrder ParentOrder { get; }

        /// <summary>
        /// Скидки, применённые к заказу оплатами, которые проводятся как скидки
        /// </summary>
        [NotNull]
        IEnumerable<IDiscountItem> DiscountItemsForPaymentsAsDiscount { get; }

    }

    internal sealed class ReceiptCheque : TemplateModelBase, IReceiptCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly ChequeTask chequeTask;
        private readonly bool isFullCheque;
        private readonly Order order;
        private readonly ReceiptChequeExtensions extensions;
        private readonly Order parentOrder;
        private readonly List<DiscountItem> discountItemsForPaymentsAsDiscount = new List<DiscountItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ReceiptCheque()
        {}

        internal ReceiptCheque([NotNull] CopyContext context, [NotNull] IReceiptCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            chequeTask = context.GetConverted(src.ChequeTask, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTask.Convert);
            isFullCheque = src.IsFullCheque;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            extensions = context.GetConverted(src.Extensions, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ReceiptChequeExtensions.Convert);
            parentOrder = context.GetConverted(src.ParentOrder, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            discountItemsForPaymentsAsDiscount = src.DiscountItemsForPaymentsAsDiscount.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountItem.Convert)).ToList();
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

        public IChequeTask ChequeTask
        {
            get { return chequeTask; }
        }

        public bool IsFullCheque
        {
            get { return isFullCheque; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public IReceiptChequeExtensions Extensions
        {
            get { return extensions; }
        }

        public IOrder ParentOrder
        {
            get { return parentOrder; }
        }

        public IEnumerable<IDiscountItem> DiscountItemsForPaymentsAsDiscount
        {
            get { return discountItemsForPaymentsAsDiscount; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IReceiptCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new ReceiptCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IReceiptCheque>(copy, "ReceiptCheque");
        }
    }
}
