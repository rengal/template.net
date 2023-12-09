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
    /// Чек iikoCard
    /// </summary>
    public interface IIikoCardCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Информация об операции и карте
        /// </summary>
        [NotNull]
        IIikoCardInfo CardInfo { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [CanBeNull]
        IOrder Order { get; }

        /// <summary>
        /// Успешна ли операция
        /// </summary>
        bool IsSuccessful { get; }

    }

    internal sealed class IikoCardCheque : TemplateModelBase, IIikoCardCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly IikoCardInfo cardInfo;
        private readonly Order order;
        private readonly bool isSuccessful;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private IikoCardCheque()
        {}

        internal IikoCardCheque([NotNull] CopyContext context, [NotNull] IIikoCardCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            cardInfo = context.GetConverted(src.CardInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.IikoCardInfo.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            isSuccessful = src.IsSuccessful;
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IIikoCardInfo CardInfo
        {
            get { return cardInfo; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public bool IsSuccessful
        {
            get { return isSuccessful; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IIikoCardCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new IikoCardCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IIikoCardCheque>(copy, "IikoCardCheque");
        }
    }
}
