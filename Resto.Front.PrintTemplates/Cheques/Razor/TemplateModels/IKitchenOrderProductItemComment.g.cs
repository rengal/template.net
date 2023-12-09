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
    /// Комментарий к блюду
    /// </summary>
    public interface IKitchenOrderProductItemComment
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

    internal sealed class KitchenOrderProductItemComment : TemplateModelBase, IKitchenOrderProductItemComment
    {
        #region Fields
        private readonly string text;
        private readonly bool deleted;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private KitchenOrderProductItemComment()
        {}

        private KitchenOrderProductItemComment([NotNull] CopyContext context, [NotNull] IKitchenOrderProductItemComment src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            text = src.Text;
            deleted = src.Deleted;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static KitchenOrderProductItemComment Convert([NotNull] CopyContext context, [CanBeNull] IKitchenOrderProductItemComment source)
        {
            if (source == null)
                return null;

            return new KitchenOrderProductItemComment(context, source);
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
