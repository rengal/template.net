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
    /// Телефон
    /// </summary>
    public interface IPhone
    {
        /// <summary>
        /// Номер
        /// </summary>
        [NotNull]
        string Number { get; }

        /// <summary>
        /// Флаг основного номера
        /// </summary>
        bool IsMain { get; }

    }

    internal sealed class Phone : TemplateModelBase, IPhone
    {
        #region Fields
        private readonly string number;
        private readonly bool isMain;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Phone()
        {}

        private Phone([NotNull] CopyContext context, [NotNull] IPhone src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
            isMain = src.IsMain;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Phone Convert([NotNull] CopyContext context, [CanBeNull] IPhone source)
        {
            if (source == null)
                return null;

            return new Phone(context, source);
        }
        #endregion

        #region Props
        public string Number
        {
            get { return GetLocalizedValue(number); }
        }

        public bool IsMain
        {
            get { return isMain; }
        }

        #endregion
    }

}
