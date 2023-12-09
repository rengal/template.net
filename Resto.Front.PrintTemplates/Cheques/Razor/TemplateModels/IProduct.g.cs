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

    internal sealed class Product : TemplateModelBase, IProduct
    {
        #region Fields
        private readonly string name;
        private readonly string fullName;
        private readonly string fullNameForeignLanguage;
        private readonly string kitchenName;
        private readonly ProductType type;
        private readonly ProductCategory category;
        private readonly AccountingCategory accountingCategory;
        private readonly FoodValue foodValue;
        private readonly MeasuringUnit measuringUnit;
        private readonly decimal? unitWeight;
        private readonly string description;
        private readonly string descriptionForeignLanguage;
        private readonly TimeSpan expirationPeriod;
        private readonly decimal salePrice;
        private readonly decimal vat;
        private readonly bool isTimePayProduct;
        private readonly bool useBalanceForSell;
        private readonly string article;
        private readonly string fastCode;
        private readonly ProductGroup productGroup;
        private readonly bool prechequePrintable;
        private readonly bool chequePrintable;
        private readonly bool useDefaultCookingTime;
        private readonly TimeSpan cookingTimeNormal;
        private readonly TimeSpan cookingTimePeak;
        private readonly bool cookWithMainDish;
        private readonly CookingPlaceType cookingPlaceType;
        private readonly List<ProductTag> productTags = new List<ProductTag>();
        private readonly List<string> allergens = new List<string>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Product()
        {}

        private Product([NotNull] CopyContext context, [NotNull] IProduct src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            fullName = src.FullName;
            fullNameForeignLanguage = src.FullNameForeignLanguage;
            kitchenName = src.KitchenName;
            type = src.Type;
            category = context.GetConverted(src.Category, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductCategory.Convert);
            accountingCategory = context.GetConverted(src.AccountingCategory, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AccountingCategory.Convert);
            foodValue = context.GetConverted(src.FoodValue, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.FoodValue.Convert);
            measuringUnit = context.GetConverted(src.MeasuringUnit, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.MeasuringUnit.Convert);
            unitWeight = src.UnitWeight;
            description = src.Description;
            descriptionForeignLanguage = src.DescriptionForeignLanguage;
            expirationPeriod = src.ExpirationPeriod;
            salePrice = src.SalePrice;
            vat = src.Vat;
            isTimePayProduct = src.IsTimePayProduct;
            useBalanceForSell = src.UseBalanceForSell;
            article = src.Article;
            fastCode = src.FastCode;
            productGroup = context.GetConverted(src.ProductGroup, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductGroup.Convert);
            prechequePrintable = src.PrechequePrintable;
            chequePrintable = src.ChequePrintable;
            useDefaultCookingTime = src.UseDefaultCookingTime;
            cookingTimeNormal = src.CookingTimeNormal;
            cookingTimePeak = src.CookingTimePeak;
            cookWithMainDish = src.CookWithMainDish;
            cookingPlaceType = context.GetConverted(src.CookingPlaceType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CookingPlaceType.Convert);
            productTags = src.ProductTags.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductTag.Convert)).ToList();
            allergens = src.Allergens.ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Product Convert([NotNull] CopyContext context, [CanBeNull] IProduct source)
        {
            if (source == null)
                return null;

            return new Product(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string FullName
        {
            get { return GetLocalizedValue(fullName); }
        }

        public string FullNameForeignLanguage
        {
            get { return GetLocalizedValue(fullNameForeignLanguage); }
        }

        public string KitchenName
        {
            get { return GetLocalizedValue(kitchenName); }
        }

        public ProductType Type
        {
            get { return type; }
        }

        public IProductCategory Category
        {
            get { return category; }
        }

        public IAccountingCategory AccountingCategory
        {
            get { return accountingCategory; }
        }

        public IFoodValue FoodValue
        {
            get { return foodValue; }
        }

        public IMeasuringUnit MeasuringUnit
        {
            get { return measuringUnit; }
        }

        public decimal? UnitWeight
        {
            get { return unitWeight; }
        }

        public string Description
        {
            get { return GetLocalizedValue(description); }
        }

        public string DescriptionForeignLanguage
        {
            get { return GetLocalizedValue(descriptionForeignLanguage); }
        }

        public TimeSpan ExpirationPeriod
        {
            get { return expirationPeriod; }
        }

        public decimal SalePrice
        {
            get { return salePrice; }
        }

        public decimal Vat
        {
            get { return vat; }
        }

        public bool IsTimePayProduct
        {
            get { return isTimePayProduct; }
        }

        public bool UseBalanceForSell
        {
            get { return useBalanceForSell; }
        }

        public string Article
        {
            get { return GetLocalizedValue(article); }
        }

        public string FastCode
        {
            get { return GetLocalizedValue(fastCode); }
        }

        public IProductGroup ProductGroup
        {
            get { return productGroup; }
        }

        public bool PrechequePrintable
        {
            get { return prechequePrintable; }
        }

        public bool ChequePrintable
        {
            get { return chequePrintable; }
        }

        public bool UseDefaultCookingTime
        {
            get { return useDefaultCookingTime; }
        }

        public TimeSpan CookingTimeNormal
        {
            get { return cookingTimeNormal; }
        }

        public TimeSpan CookingTimePeak
        {
            get { return cookingTimePeak; }
        }

        public bool CookWithMainDish
        {
            get { return cookWithMainDish; }
        }

        public ICookingPlaceType CookingPlaceType
        {
            get { return cookingPlaceType; }
        }

        public IEnumerable<IProductTag> ProductTags
        {
            get { return productTags; }
        }

        public IEnumerable<string> Allergens
        {
            get { return allergens; }
        }

        #endregion
    }

}
