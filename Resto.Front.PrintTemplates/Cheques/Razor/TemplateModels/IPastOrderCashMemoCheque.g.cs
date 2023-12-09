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
    /// Товарный чек заказа, закрытого в прошлые кассовые смены
    /// </summary>
    public interface IPastOrderCashMemoCheque : ITemplateRootModel
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
        /// Заказ
        /// </summary>
        [NotNull]
        IPastOrder Order { get; }

    }

    internal sealed class PastOrderCashMemoCheque : TemplateModelBase, IPastOrderCashMemoCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly PastOrder order;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PastOrderCashMemoCheque()
        {}

        internal PastOrderCashMemoCheque([NotNull] CopyContext context, [NotNull] IPastOrderCashMemoCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PastOrder.Convert);
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

        public IPastOrder Order
        {
            get { return order; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IPastOrderCashMemoCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new PastOrderCashMemoCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IPastOrderCashMemoCheque>(copy, "PastOrderCashMemoCheque");
        }
    }
}
