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
    /// Информация о компонентах составных блюд
    /// </summary>
    public interface ICompoundOrderItemInfo
    {
        /// <summary>
        /// Основная половина составного блюда
        /// </summary>
        bool IsPrimaryComponent { get; }

        /// <summary>
        /// Название схемы модификаторов
        /// </summary>
        string ModifierSchemaName { get; }

    }

    internal sealed class CompoundOrderItemInfo : TemplateModelBase, ICompoundOrderItemInfo
    {
        #region Fields
        private readonly bool isPrimaryComponent;
        private readonly string modifierSchemaName;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CompoundOrderItemInfo()
        {}

        private CompoundOrderItemInfo([NotNull] CopyContext context, [NotNull] ICompoundOrderItemInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isPrimaryComponent = src.IsPrimaryComponent;
            modifierSchemaName = src.ModifierSchemaName;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CompoundOrderItemInfo Convert([NotNull] CopyContext context, [CanBeNull] ICompoundOrderItemInfo source)
        {
            if (source == null)
                return null;

            return new CompoundOrderItemInfo(context, source);
        }
        #endregion

        #region Props
        public bool IsPrimaryComponent
        {
            get { return isPrimaryComponent; }
        }

        public string ModifierSchemaName
        {
            get { return GetLocalizedValue(modifierSchemaName); }
        }

        #endregion
    }

}
