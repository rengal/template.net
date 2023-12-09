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
    /// Комментарий к элементу заказа (блюду)
    /// </summary>
    public interface IProductItemComment
    {
        /// <summary>
        /// Текст комментария
        /// </summary>
        [NotNull]
        string Text { get; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        bool Deleted { get; }

    }

    internal sealed class ProductItemComment : TemplateModelBase, IProductItemComment
    {
        #region Fields
        private readonly string text;
        private readonly bool deleted;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductItemComment()
        {}

        private ProductItemComment([NotNull] CopyContext context, [NotNull] IProductItemComment src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            text = src.Text;
            deleted = src.Deleted;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ProductItemComment Convert([NotNull] CopyContext context, [CanBeNull] IProductItemComment source)
        {
            if (source == null)
                return null;

            return new ProductItemComment(context, source);
        }
        #endregion

        #region Props
        public string Text
        {
            get { return GetLocalizedValue(text); }
        }

        public bool Deleted
        {
            get { return deleted; }
        }

        #endregion
    }

}
