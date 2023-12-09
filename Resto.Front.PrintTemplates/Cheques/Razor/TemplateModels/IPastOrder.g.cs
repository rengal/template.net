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
    /// Заказ, закрытый в прошлые кассовые смены
    /// </summary>
    public interface IPastOrder
    {
        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Дата/время открытия
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        DateTime CloseTime { get; }

        /// <summary>
        /// Номер стола
        /// </summary>
        int TableNumber { get; }

        /// <summary>
        /// Номер фискального чека
        /// </summary>
        [Obsolete("В заказе хранятся номера чеков, которые были напечатаны при закрытии заказа. Для обратной совместимости, здесь возвращается номер последнего чека. Вместо этого свойства необходимо использовать FiscalChequeNumbers.")]
        int? FiscalChequeNumber { get; }

        /// <summary>
        /// Номера фискальных чеков
        /// </summary>
        [NotNull]
        IEnumerable<int> FiscalChequeNumbers { get; }

        /// <summary>
        /// Элементы заказа
        /// </summary>
        [NotNull]
        IEnumerable<IPastOrderItem> Items { get; }

        /// <summary>
        /// Оплаты
        /// </summary>
        [NotNull]
        IEnumerable<IPastOrderPaymentItem> Payments { get; }

        /// <summary>
        /// Чаевые
        /// </summary>
        [NotNull]
        IEnumerable<IPastOrderPaymentItem> Donations { get; }

    }

    internal sealed class PastOrder : TemplateModelBase, IPastOrder
    {
        #region Fields
        private readonly int number;
        private readonly DateTime openTime;
        private readonly DateTime closeTime;
        private readonly int tableNumber;
        private readonly int? fiscalChequeNumber;
        private readonly List<int> fiscalChequeNumbers = new List<int>();
        private readonly List<PastOrderItem> items = new List<PastOrderItem>();
        private readonly List<PastOrderPaymentItem> payments = new List<PastOrderPaymentItem>();
        private readonly List<PastOrderPaymentItem> donations = new List<PastOrderPaymentItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PastOrder()
        {}

        private PastOrder([NotNull] CopyContext context, [NotNull] IPastOrder src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
            openTime = src.OpenTime;
            closeTime = src.CloseTime;
            tableNumber = src.TableNumber;
            fiscalChequeNumber = src.FiscalChequeNumber;
            fiscalChequeNumbers = src.FiscalChequeNumbers.ToList();
            items = src.Items.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PastOrderItem.Convert)).ToList();
            payments = src.Payments.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PastOrderPaymentItem.Convert)).ToList();
            donations = src.Donations.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PastOrderPaymentItem.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PastOrder Convert([NotNull] CopyContext context, [CanBeNull] IPastOrder source)
        {
            if (source == null)
                return null;

            return new PastOrder(context, source);
        }
        #endregion

        #region Props
        public int Number
        {
            get { return number; }
        }

        public DateTime OpenTime
        {
            get { return openTime; }
        }

        public DateTime CloseTime
        {
            get { return closeTime; }
        }

        public int TableNumber
        {
            get { return tableNumber; }
        }

        public int? FiscalChequeNumber
        {
            get { return fiscalChequeNumber; }
        }

        public IEnumerable<int> FiscalChequeNumbers
        {
            get { return fiscalChequeNumbers; }
        }

        public IEnumerable<IPastOrderItem> Items
        {
            get { return items; }
        }

        public IEnumerable<IPastOrderPaymentItem> Payments
        {
            get { return payments; }
        }

        public IEnumerable<IPastOrderPaymentItem> Donations
        {
            get { return donations; }
        }

        #endregion
    }

}
