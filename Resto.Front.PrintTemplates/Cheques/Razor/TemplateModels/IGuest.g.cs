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
    /// Гость в заказе
    /// </summary>
    public interface IGuest
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid GuestId { get; }

        /// <summary>
        /// Имя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Величина для сортировки гостей в заказе
        /// </summary>
        int OrderRank { get; }

        /// <summary>
        /// Элементы заказа (блюда/услуги) гостя
        /// </summary>
        [NotNull]
        IEnumerable<IOrderItem> Items { get; }

    }

    internal sealed class Guest : TemplateModelBase, IGuest
    {
        #region Fields
        private readonly Guid guestId;
        private readonly string name;
        private readonly int orderRank;
        private readonly List<OrderItem> items = new List<OrderItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Guest()
        {}

        private Guest([NotNull] CopyContext context, [NotNull] IGuest src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            guestId = src.GuestId;
            name = src.Name;
            orderRank = src.OrderRank;
            items = src.Items.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderItem.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Guest Convert([NotNull] CopyContext context, [CanBeNull] IGuest source)
        {
            if (source == null)
                return null;

            return new Guest(context, source);
        }
        #endregion

        #region Props
        public Guid GuestId
        {
            get { return guestId; }
        }

        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public int OrderRank
        {
            get { return orderRank; }
        }

        public IEnumerable<IOrderItem> Items
        {
            get { return items; }
        }

        #endregion
    }

}
