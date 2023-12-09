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
    /// Чек об окончании приготовления блюда
    /// </summary>
    public interface ICookingCompleteCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Место приготовления, для которого печатается чек
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IKitchenOrder Order { get; }

        /// <summary>
        /// Элемент заказа, для которого печатается чек
        /// </summary>
        [NotNull]
        IKitchenOrderItem Item { get; }

        /// <summary>
        /// Правая половинка разделенной пиццы, для которой печатается чек
        /// </summary>
        [CanBeNull]
        IKitchenOrderProductItem CompoundItemSecondaryComponent { get; }

        /// <summary>
        /// Элементы заказа, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<ICookingCompleteChequeItem> Items { get; }

    }

    internal sealed class CookingCompleteCheque : TemplateModelBase, ICookingCompleteCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly RestaurantSection cookingPlace;
        private readonly KitchenOrder order;
        private readonly KitchenOrderItem item;
        private readonly KitchenOrderProductItem compoundItemSecondaryComponent;
        private readonly List<CookingCompleteChequeItem> items = new List<CookingCompleteChequeItem>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CookingCompleteCheque()
        {}

        internal CookingCompleteCheque([NotNull] CopyContext context, [NotNull] ICookingCompleteCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            cookingPlace = context.GetConverted(src.CookingPlace, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrder.Convert);
            item = context.GetConverted(src.Item, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderItem.Convert);
            compoundItemSecondaryComponent = context.GetConverted(src.CompoundItemSecondaryComponent, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItem.Convert);
            items = src.Items.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CookingCompleteChequeItem.Convert)).ToList();
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IRestaurantSection CookingPlace
        {
            get { return cookingPlace; }
        }

        public IKitchenOrder Order
        {
            get { return order; }
        }

        public IKitchenOrderItem Item
        {
            get { return item; }
        }

        public IKitchenOrderProductItem CompoundItemSecondaryComponent
        {
            get { return compoundItemSecondaryComponent; }
        }

        public IEnumerable<ICookingCompleteChequeItem> Items
        {
            get { return items; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this ICookingCompleteCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new CookingCompleteCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ICookingCompleteCheque>(copy, "CookingCompleteCheque");
        }
    }
}
