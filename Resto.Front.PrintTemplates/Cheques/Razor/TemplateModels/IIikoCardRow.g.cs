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
    /// Пары, получаемые с сервера
    /// </summary>
    public interface IIikoCardRow
    {
        /// <summary>
        /// Имя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Значение
        /// </summary>
        decimal Value { get; }

        /// <summary>
        /// Дополнительные строки
        /// </summary>
        [CanBeNull]
        string Lines { get; }

    }

    internal sealed class IikoCardRow : TemplateModelBase, IIikoCardRow
    {
        #region Fields
        private readonly string name;
        private readonly decimal value;
        private readonly string lines;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private IikoCardRow()
        {}

        private IikoCardRow([NotNull] CopyContext context, [NotNull] IIikoCardRow src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            value = src.Value;
            lines = src.Lines;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static IikoCardRow Convert([NotNull] CopyContext context, [CanBeNull] IIikoCardRow source)
        {
            if (source == null)
                return null;

            return new IikoCardRow(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public decimal Value
        {
            get { return value; }
        }

        public string Lines
        {
            get { return GetLocalizedValue(lines); }
        }

        #endregion
    }

}
