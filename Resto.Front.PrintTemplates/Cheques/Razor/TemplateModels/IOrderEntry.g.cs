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
    /// Позиция заказа
    /// </summary>
    public interface IOrderEntry
    {
        /// <summary>
        /// Блюдо/продукт/услуга
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Произвольное название для блюда/продукта/услуги
        /// </summary>
        [CanBeNull]
        string ProductCustomName { get; }

        /// <summary>
        /// Категория блюда, зафиксированная на момент добавления позиции в заказ
        /// </summary>
        [CanBeNull]
        IProductCategory ProductCategory { get; }

        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Цена за единицу
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Стоимость
        /// </summary>
        decimal Cost { get; }

        /// <summary>
        /// НДС включен в стоимость
        /// </summary>
        bool VatIncludedInPrice { get; }

        /// <summary>
        /// Ставка НДС
        /// </summary>
        decimal Vat { get; }

        /// <summary>
        /// Сумма НДС, не включенная в стоимость позиции заказа
        /// </summary>
        decimal ExcludedVat { get; }

        /// <summary>
        /// Сумма НДС, включенная в стоимость позиции заказа
        /// </summary>
        decimal IncludedVat { get; }

        /// <summary>
        /// Информация об удалении позиции заказа. null, если позиция не удалена
        /// </summary>
        [CanBeNull]
        IOrderEntryDeletionInfo DeletionInfo { get; }

        /// <summary>
        /// Список аллергенов.
        /// </summary>
        [NotNull]
        IEnumerable<string> Allergens { get; }

    }

    internal abstract class OrderEntry : TemplateModelBase, IOrderEntry
    {
        #region Fields
        private readonly Product product;
        private readonly string productCustomName;
        private readonly ProductCategory productCategory;
        private readonly decimal amount;
        private readonly decimal price;
        private readonly decimal cost;
        private readonly bool vatIncludedInPrice;
        private readonly decimal vat;
        private readonly decimal excludedVat;
        private readonly decimal includedVat;
        private readonly OrderEntryDeletionInfo deletionInfo;
        private readonly List<string> allergens = new List<string>();
        #endregion

        #region Ctor
        protected OrderEntry()
        {}

        protected OrderEntry([NotNull] CopyContext context, [NotNull] IOrderEntry src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            product = context.GetConverted(src.Product, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert);
            productCustomName = src.ProductCustomName;
            productCategory = context.GetConverted(src.ProductCategory, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductCategory.Convert);
            amount = src.Amount;
            price = src.Price;
            cost = src.Cost;
            vatIncludedInPrice = src.VatIncludedInPrice;
            vat = src.Vat;
            excludedVat = src.ExcludedVat;
            includedVat = src.IncludedVat;
            deletionInfo = context.GetConverted(src.DeletionInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderEntryDeletionInfo.Convert);
            allergens = src.Allergens.ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderEntry Convert([NotNull] CopyContext context, [CanBeNull] IOrderEntry source)
        {
            if (source == null)
                return null;

            if (source is IProductItem)
                return ProductItem.Convert(context, (IProductItem)source);
            else if (source is ITimePayServiceItem)
                return TimePayServiceItem.Convert(context, (ITimePayServiceItem)source);
            else if (source is IModifierEntry)
                return ModifierEntry.Convert(context, (IModifierEntry)source);
            else if (source is IRateScheduleEntry)
                return RateScheduleEntry.Convert(context, (IRateScheduleEntry)source);
            else
                throw new ArgumentException(string.Format("Type {0} not supported", source.GetType()), "source");
        }
        #endregion

        #region Props
        public IProduct Product
        {
            get { return product; }
        }

        public string ProductCustomName
        {
            get { return GetLocalizedValue(productCustomName); }
        }

        public IProductCategory ProductCategory
        {
            get { return productCategory; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public decimal Price
        {
            get { return price; }
        }

        public decimal Cost
        {
            get { return cost; }
        }

        public bool VatIncludedInPrice
        {
            get { return vatIncludedInPrice; }
        }

        public decimal Vat
        {
            get { return vat; }
        }

        public decimal ExcludedVat
        {
            get { return excludedVat; }
        }

        public decimal IncludedVat
        {
            get { return includedVat; }
        }

        public IOrderEntryDeletionInfo DeletionInfo
        {
            get { return deletionInfo; }
        }

        public IEnumerable<string> Allergens
        {
            get { return allergens; }
        }

        #endregion
    }

}
