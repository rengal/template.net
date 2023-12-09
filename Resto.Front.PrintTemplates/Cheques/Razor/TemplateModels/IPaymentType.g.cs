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

    internal abstract class PaymentType : TemplateModelBase, IPaymentType
    {
        #region Fields
        private readonly bool deleted;
        private readonly bool enabled;
        private readonly string name;
        private readonly PaymentGroup group;
        private readonly bool proccessAsDiscount;
        private readonly bool printCheque;
        private readonly bool validForOrders;
        private readonly bool canDisplayChange;
        #endregion

        #region Ctor
        protected PaymentType()
        {}

        protected PaymentType([NotNull] CopyContext context, [NotNull] IPaymentType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            deleted = src.Deleted;
            enabled = src.Enabled;
            name = src.Name;
            group = src.Group;
            proccessAsDiscount = src.ProccessAsDiscount;
            printCheque = src.PrintCheque;
            validForOrders = src.ValidForOrders;
            canDisplayChange = src.CanDisplayChange;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static PaymentType Convert([NotNull] CopyContext context, [CanBeNull] IPaymentType source)
        {
            if (source == null)
                return null;

            if (source is IWriteoffPaymentType)
                return WriteoffPaymentType.Convert(context, (IWriteoffPaymentType)source);
            else if (source is IConfigurablePaymentType)
                return ConfigurablePaymentType.Convert(context, (IConfigurablePaymentType)source);
            else if (source is ICreditPaymentType)
                return CreditPaymentType.Convert(context, (ICreditPaymentType)source);
            else if (source is ICashPaymentType)
                return CashPaymentType.Convert(context, (ICashPaymentType)source);
            else if (source is INonCashPaymentType)
                return NonCashPaymentType.Convert(context, (INonCashPaymentType)source);
            else
                throw new ArgumentException(string.Format("Type {0} not supported", source.GetType()), "source");
        }
        #endregion

        #region Props
        public bool Deleted
        {
            get { return deleted; }
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public PaymentGroup Group
        {
            get { return group; }
        }

        public bool ProccessAsDiscount
        {
            get { return proccessAsDiscount; }
        }

        public bool PrintCheque
        {
            get { return printCheque; }
        }

        public bool ValidForOrders
        {
            get { return validForOrders; }
        }

        public bool CanDisplayChange
        {
            get { return canDisplayChange; }
        }

        #endregion
    }

}
