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
    /// Информация о кассовом чеке
    /// </summary>
    public interface ICashChequeInfo
    {
        /// <summary>
        /// Формируется ли чек для отчёта «Кассовая лента»
        /// </summary>
        bool IsForReport { get; }

        /// <summary>
        /// Кассир
        /// </summary>
        [CanBeNull]
        IUser Cashier { get; }

        /// <summary>
        /// Дата/время операции
        /// </summary>
        DateTime OperationTime { get; }

        /// <summary>
        /// Кассовая смена операции
        /// </summary>
        [NotNull]
        ICafeSession Session { get; }

    }

    internal sealed class CashChequeInfo : TemplateModelBase, ICashChequeInfo
    {
        #region Fields
        private readonly bool isForReport;
        private readonly User cashier;
        private readonly DateTime operationTime;
        private readonly CafeSession session;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CashChequeInfo()
        {}

        private CashChequeInfo([NotNull] CopyContext context, [NotNull] ICashChequeInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isForReport = src.IsForReport;
            cashier = context.GetConverted(src.Cashier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            operationTime = src.OperationTime;
            session = context.GetConverted(src.Session, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CafeSession.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CashChequeInfo Convert([NotNull] CopyContext context, [CanBeNull] ICashChequeInfo source)
        {
            if (source == null)
                return null;

            return new CashChequeInfo(context, source);
        }
        #endregion

        #region Props
        public bool IsForReport
        {
            get { return isForReport; }
        }

        public IUser Cashier
        {
            get { return cashier; }
        }

        public DateTime OperationTime
        {
            get { return operationTime; }
        }

        public ICafeSession Session
        {
            get { return session; }
        }

        #endregion
    }

}
