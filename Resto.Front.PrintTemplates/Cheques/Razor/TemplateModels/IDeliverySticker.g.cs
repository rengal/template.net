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
    /// Этикетка для доставки.
    /// </summary>
    public interface IDeliverySticker : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Доставка.
        /// </summary>
        [NotNull]
        IDelivery Delivery { get; }

        /// <summary>
        /// Блюдо, для которого печатается этикетка; либо основное блюдо для модификатора <see cref="Modifier"/>.
        /// </summary>
        [NotNull]
        IProductItem Item { get; }

        /// <summary>
        /// Модификатор, готовящийся отдельно от основного блюда, для которого печатается этикетка.
        /// </summary>
        [CanBeNull]
        IModifierEntry Modifier { get; }

        /// <summary>
        /// Правая половинка разделенной пиццы, для которой печатается чек
        /// </summary>
        [CanBeNull]
        IProductItem CompoundItemSecondaryComponent { get; }

    }

    internal sealed class DeliverySticker : TemplateModelBase, IDeliverySticker
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly Delivery delivery;
        private readonly ProductItem item;
        private readonly ModifierEntry modifier;
        private readonly ProductItem compoundItemSecondaryComponent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DeliverySticker()
        {}

        internal DeliverySticker([NotNull] CopyContext context, [NotNull] IDeliverySticker src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            delivery = context.GetConverted(src.Delivery, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Delivery.Convert);
            item = context.GetConverted(src.Item, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItem.Convert);
            modifier = context.GetConverted(src.Modifier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ModifierEntry.Convert);
            compoundItemSecondaryComponent = context.GetConverted(src.CompoundItemSecondaryComponent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItem.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IDelivery Delivery
        {
            get { return delivery; }
        }

        public IProductItem Item
        {
            get { return item; }
        }

        public IModifierEntry Modifier
        {
            get { return modifier; }
        }

        public IProductItem CompoundItemSecondaryComponent
        {
            get { return compoundItemSecondaryComponent; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IDeliverySticker cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new DeliverySticker(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IDeliverySticker>(copy, "DeliverySticker");
        }
    }
}
