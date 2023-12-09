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
    /// Электронная почта
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [NotNull]
        string Value { get; }

        /// <summary>
        /// Флаг основного адреса электронной почты
        /// </summary>
        bool IsMain { get; }

    }

    internal sealed class Email : TemplateModelBase, IEmail
    {
        #region Fields
        private readonly string value;
        private readonly bool isMain;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Email()
        {}

        private Email([NotNull] CopyContext context, [NotNull] IEmail src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            value = src.Value;
            isMain = src.IsMain;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Email Convert([NotNull] CopyContext context, [CanBeNull] IEmail source)
        {
            if (source == null)
                return null;

            return new Email(context, source);
        }
        #endregion

        #region Props
        public string Value
        {
            get { return GetLocalizedValue(value); }
        }

        public bool IsMain
        {
            get { return isMain; }
        }

        #endregion
    }

}
