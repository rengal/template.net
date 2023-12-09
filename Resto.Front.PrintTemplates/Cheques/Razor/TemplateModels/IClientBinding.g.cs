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
    /// Привязка заказа к карте клиента («Карта на входе»)
    /// </summary>
    public interface IClientBinding
    {
        /// <summary>
        /// Номер карты клиента
        /// </summary>
        [NotNull]
        string CardNumber { get; }

        /// <summary>
        /// Лимит клиента
        /// </summary>
        decimal? PaymentLimit { get; }

    }

    internal sealed class ClientBinding : TemplateModelBase, IClientBinding
    {
        #region Fields
        private readonly string cardNumber;
        private readonly decimal? paymentLimit;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ClientBinding()
        {}

        private ClientBinding([NotNull] CopyContext context, [NotNull] IClientBinding src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            cardNumber = src.CardNumber;
            paymentLimit = src.PaymentLimit;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ClientBinding Convert([NotNull] CopyContext context, [CanBeNull] IClientBinding source)
        {
            if (source == null)
                return null;

            return new ClientBinding(context, source);
        }
        #endregion

        #region Props
        public string CardNumber
        {
            get { return GetLocalizedValue(cardNumber); }
        }

        public decimal? PaymentLimit
        {
            get { return paymentLimit; }
        }

        #endregion
    }

}
