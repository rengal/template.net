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
    /// Этикетка, печатающаяся по кухонному заказу по окончании приготовления блюда.
    /// </summary>
    public interface ICookingCompleteSticker : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Кухонный заказ.
        /// </summary>
        [NotNull]
        IKitchenOrder Order { get; }

        /// <summary>
        /// Элемент кухонного заказа, для которого печатается этикетка; либо основное блюдо для модификатора <see cref="Modifier"/>.
        /// </summary>
        [NotNull]
        IKitchenOrderProductItem Item { get; }

        /// <summary>
        /// Модификатор, готовящийся отдельно от основного блюда, для которого печатается этикетка.
        /// </summary>
        [CanBeNull]
        IKitchenOrderModifierItem Modifier { get; }

        /// <summary>
        /// Правая половинка разделенной пиццы, для которой печатается чек
        /// </summary>
        [CanBeNull]
        IKitchenOrderProductItem CompoundItemSecondaryComponent { get; }

    }

    internal sealed class CookingCompleteSticker : TemplateModelBase, ICookingCompleteSticker
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly KitchenOrder order;
        private readonly KitchenOrderProductItem item;
        private readonly KitchenOrderModifierItem modifier;
        private readonly KitchenOrderProductItem compoundItemSecondaryComponent;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CookingCompleteSticker()
        {}

        internal CookingCompleteSticker([NotNull] CopyContext context, [NotNull] ICookingCompleteSticker src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrder.Convert);
            item = context.GetConverted(src.Item, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItem.Convert);
            modifier = context.GetConverted(src.Modifier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderModifierItem.Convert);
            compoundItemSecondaryComponent = context.GetConverted(src.CompoundItemSecondaryComponent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItem.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IKitchenOrder Order
        {
            get { return order; }
        }

        public IKitchenOrderProductItem Item
        {
            get { return item; }
        }

        public IKitchenOrderModifierItem Modifier
        {
            get { return modifier; }
        }

        public IKitchenOrderProductItem CompoundItemSecondaryComponent
        {
            get { return compoundItemSecondaryComponent; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this ICookingCompleteSticker cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new CookingCompleteSticker(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ICookingCompleteSticker>(copy, "CookingCompleteSticker");
        }
    }
}
