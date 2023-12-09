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
    /// Сведения для промежуточной квитанции дозаказа
    /// </summary>
    public interface IAdditionalServiceChequeInfo
    {
        /// <summary>
        /// Добавленные элементы заказа
        /// </summary>
        [NotNull]
        IEnumerable<IOrderItem> AddedOrderItems { get; }

    }

    internal sealed class AdditionalServiceChequeInfo : TemplateModelBase, IAdditionalServiceChequeInfo
    {
        #region Fields
        private readonly List<OrderItem> addedOrderItems = new List<OrderItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private AdditionalServiceChequeInfo()
        {}

        private AdditionalServiceChequeInfo([NotNull] CopyContext context, [NotNull] IAdditionalServiceChequeInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            addedOrderItems = src.AddedOrderItems.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderItem.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static AdditionalServiceChequeInfo Convert([NotNull] CopyContext context, [CanBeNull] IAdditionalServiceChequeInfo source)
        {
            if (source == null)
                return null;

            return new AdditionalServiceChequeInfo(context, source);
        }
        #endregion

        #region Props
        public IEnumerable<IOrderItem> AddedOrderItems
        {
            get { return addedOrderItems; }
        }

        #endregion
    }

}
