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
    /// Дополнение к фискальному чеку
    /// </summary>
    public interface ICashRegisterChequeAddition : ITemplateRootModel
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
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Информация об организации
        /// </summary>
        [CanBeNull]
        IOrganizationDetailsInfo OrganizationDetails { get; }

    }

    internal sealed class CashRegisterChequeAddition : TemplateModelBase, ICashRegisterChequeAddition
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly ChequeTask chequeTask;
        private readonly Order order;
        private readonly OrganizationDetailsInfo organizationDetails;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CashRegisterChequeAddition()
        {}

        internal CashRegisterChequeAddition([NotNull] CopyContext context, [NotNull] ICashRegisterChequeAddition src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            chequeTask = context.GetConverted(src.ChequeTask, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeTask.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            organizationDetails = context.GetConverted(src.OrganizationDetails, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrganizationDetailsInfo.Convert);
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

        public IOrder Order
        {
            get { return order; }
        }

        public IOrganizationDetailsInfo OrganizationDetails
        {
            get { return organizationDetails; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this ICashRegisterChequeAddition cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new CashRegisterChequeAddition(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ICashRegisterChequeAddition>(copy, "CashRegisterChequeAddition");
        }
    }
}
