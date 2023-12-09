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
    /// Информация о кассовой смене
    /// </summary>
    public interface ICafeSession
    {
        /// <summary>
        /// Номер смены
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Дата/время открытия смены
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Номер ФРа
        /// </summary>
        int CashRegisterNumber { get; }

    }

    internal sealed class CafeSession : TemplateModelBase, ICafeSession
    {
        #region Fields
        private readonly int number;
        private readonly DateTime openTime;
        private readonly int cashRegisterNumber;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CafeSession()
        {}

        private CafeSession([NotNull] CopyContext context, [NotNull] ICafeSession src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
            openTime = src.OpenTime;
            cashRegisterNumber = src.CashRegisterNumber;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CafeSession Convert([NotNull] CopyContext context, [CanBeNull] ICafeSession source)
        {
            if (source == null)
                return null;

            return new CafeSession(context, source);
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

        public int CashRegisterNumber
        {
            get { return cashRegisterNumber; }
        }

        #endregion
    }

}
