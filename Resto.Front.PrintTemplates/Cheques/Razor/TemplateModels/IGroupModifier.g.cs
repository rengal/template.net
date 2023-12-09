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
    /// Групповой модификатор
    /// </summary>
    public interface IGroupModifier
    {
        /// <summary>
        /// Минимальное количество
        /// </summary>
        int MinimumAmount { get; }

        /// <summary>
        /// Максимальное количество
        /// </summary>
        int MaximumAmount { get; }

        /// <summary>
        /// Группа продуктов
        /// </summary>
        [NotNull]
        IProductGroup ProductGroup { get; }

        /// <summary>
        /// Дочерние модификаторы
        /// </summary>
        [NotNull]
        IEnumerable<IChildModifier> ChildModifiers { get; }

    }

    internal sealed class GroupModifier : TemplateModelBase, IGroupModifier
    {
        #region Fields
        private readonly int minimumAmount;
        private readonly int maximumAmount;
        private readonly ProductGroup productGroup;
        private readonly List<ChildModifier> childModifiers = new List<ChildModifier>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private GroupModifier()
        {}

        private GroupModifier([NotNull] CopyContext context, [NotNull] IGroupModifier src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            minimumAmount = src.MinimumAmount;
            maximumAmount = src.MaximumAmount;
            productGroup = context.GetConverted(src.ProductGroup, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductGroup.Convert);
            childModifiers = src.ChildModifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChildModifier.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static GroupModifier Convert([NotNull] CopyContext context, [CanBeNull] IGroupModifier source)
        {
            if (source == null)
                return null;

            return new GroupModifier(context, source);
        }
        #endregion

        #region Props
        public int MinimumAmount
        {
            get { return minimumAmount; }
        }

        public int MaximumAmount
        {
            get { return maximumAmount; }
        }

        public IProductGroup ProductGroup
        {
            get { return productGroup; }
        }

        public IEnumerable<IChildModifier> ChildModifiers
        {
            get { return childModifiers; }
        }

        #endregion
    }

}
