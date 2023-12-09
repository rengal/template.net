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

    internal sealed class CashRegister : TemplateModelBase, ICashRegister
    {
        #region Fields
        private readonly int number;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CashRegister()
        {}

        private CashRegister([NotNull] CopyContext context, [NotNull] ICashRegister src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CashRegister Convert([NotNull] CopyContext context, [CanBeNull] ICashRegister source)
        {
            if (source == null)
                return null;

            return new CashRegister(context, source);
        }
        #endregion

        #region Props
        public int Number
        {
            get { return number; }
        }

        #endregion
    }

}
