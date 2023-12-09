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
    /// Накладная доставки.
    /// </summary>
    public interface IDeliveryBillCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Доставка.
        /// </summary>
        [NotNull]
        IDelivery Delivery { get; }

        /// <summary>
        /// Дополнительные элементы.
        /// </summary>
        [NotNull]
        IBillChequeExtensions Extensions { get; }

    }

    internal sealed class DeliveryBillCheque : TemplateModelBase, IDeliveryBillCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly Delivery delivery;
        private readonly BillChequeExtensions extensions;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DeliveryBillCheque()
        {}

        internal DeliveryBillCheque([NotNull] CopyContext context, [NotNull] IDeliveryBillCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            delivery = context.GetConverted(src.Delivery, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Delivery.Convert);
            extensions = context.GetConverted(src.Extensions, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.BillChequeExtensions.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IDelivery Delivery
        {
            get { return delivery; }
        }

        public IBillChequeExtensions Extensions
        {
            get { return extensions; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IDeliveryBillCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new DeliveryBillCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IDeliveryBillCheque>(copy, "DeliveryBillCheque");
        }
    }
}
