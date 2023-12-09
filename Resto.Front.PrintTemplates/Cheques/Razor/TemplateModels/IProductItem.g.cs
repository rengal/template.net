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
    /// Элемент заказа (блюдо)
    /// </summary>
    public interface IProductItem : IOrderItem
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        IProductItemComment Comment { get; }

        /// <summary>
        /// Размер
        /// </summary>
        [CanBeNull]
        IProductItemSize ProductSize { get; }

        /// <summary>
        /// Суммарный фактический выход на 1 норму закладки (вес 1 единицы измерения), кг
        /// </summary>
        [CanBeNull]
        decimal? UnitWeight { get; }

        /// <summary>
        /// Информация о компонентах составных блюд
        /// </summary>
        [CanBeNull]
        ICompoundOrderItemInfo CompoundsInfo { get; }

        /// <summary>
        /// Курс
        /// </summary>
        int Course { get; }

        /// <summary>
        /// Состояние
        /// </summary>
        ProductItemState State { get; }

        /// <summary>
        /// Время печати
        /// </summary>
        DateTime? PrintTime { get; }

        /// <summary>
        /// Время начала приготовления
        /// </summary>
        DateTime? CookingStartTime { get; }

        /// <summary>
        /// Время окончания приготовления
        /// </summary>
        DateTime? CookingFinishTime { get; }

        /// <summary>
        /// Время подачи
        /// </summary>
        DateTime? DeliverTime { get; }

        /// <summary>
        /// Номер подачи
        /// </summary>
        int ServeGroupNumber { get; }

        /// <summary>
        /// Тип места приготовления
        /// </summary>
        [NotNull]
        ICookingPlaceType CookingPlaceType { get; }

        /// <summary>
        /// Место приготовления
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// У элемента заказа есть микс
        /// </summary>
        bool HasMix { get; }

        /// <summary>
        /// Микс удалён (только для элементов заказа с HasMix == true)
        /// </summary>
        bool MixDeleted { get; }

        /// <summary>
        /// Штрихкод для сервисного чека
        /// </summary>
        [NotNull]
        string ServiceChequeBarcode { get; }

        /// <summary>
        /// Простые модификаторы, доступные для добавления
        /// </summary>
        [NotNull]
        IEnumerable<ISimpleModifier> SimpleModifiers { get; }

        /// <summary>
        /// Групповые модификаторы, доступные для добавления
        /// </summary>
        [NotNull]
        IEnumerable<IGroupModifier> GroupModifiers { get; }

        /// <summary>
        /// Модификаторы, добавленные в заказ
        /// </summary>
        [NotNull]
        IEnumerable<IModifierEntry> ModifierEntries { get; }

    }

    internal sealed class ProductItem : OrderItem, IProductItem
    {
        #region Fields
        private readonly ProductItemComment comment;
        private readonly ProductItemSize productSize;
        private readonly decimal? unitWeight;
        private readonly CompoundOrderItemInfo compoundsInfo;
        private readonly int course;
        private readonly ProductItemState state;
        private readonly DateTime? printTime;
        private readonly DateTime? cookingStartTime;
        private readonly DateTime? cookingFinishTime;
        private readonly DateTime? deliverTime;
        private readonly int serveGroupNumber;
        private readonly CookingPlaceType cookingPlaceType;
        private readonly RestaurantSection cookingPlace;
        private readonly bool hasMix;
        private readonly bool mixDeleted;
        private readonly string serviceChequeBarcode;
        private readonly List<SimpleModifier> simpleModifiers = new List<SimpleModifier>();
        private readonly List<GroupModifier> groupModifiers = new List<GroupModifier>();
        private readonly List<ModifierEntry> modifierEntries = new List<ModifierEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ProductItem()
        {}

        private ProductItem([NotNull] CopyContext context, [NotNull] IProductItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            comment = context.GetConverted(src.Comment, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItemComment.Convert);
            productSize = context.GetConverted(src.ProductSize, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItemSize.Convert);
            unitWeight = src.UnitWeight;
            compoundsInfo = context.GetConverted(src.CompoundsInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CompoundOrderItemInfo.Convert);
            course = src.Course;
            state = src.State;
            printTime = src.PrintTime;
            cookingStartTime = src.CookingStartTime;
            cookingFinishTime = src.CookingFinishTime;
            deliverTime = src.DeliverTime;
            serveGroupNumber = src.ServeGroupNumber;
            cookingPlaceType = context.GetConverted(src.CookingPlaceType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CookingPlaceType.Convert);
            cookingPlace = context.GetConverted(src.CookingPlace, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            hasMix = src.HasMix;
            mixDeleted = src.MixDeleted;
            serviceChequeBarcode = src.ServiceChequeBarcode;
            simpleModifiers = src.SimpleModifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.SimpleModifier.Convert)).ToList();
            groupModifiers = src.GroupModifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.GroupModifier.Convert)).ToList();
            modifierEntries = src.ModifierEntries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ModifierEntry.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ProductItem Convert([NotNull] CopyContext context, [CanBeNull] IProductItem source)
        {
            if (source == null)
                return null;

            return new ProductItem(context, source);
        }
        #endregion

        #region Props
        public IProductItemComment Comment
        {
            get { return comment; }
        }

        public IProductItemSize ProductSize
        {
            get { return productSize; }
        }

        public decimal? UnitWeight
        {
            get { return unitWeight; }
        }

        public ICompoundOrderItemInfo CompoundsInfo
        {
            get { return compoundsInfo; }
        }

        public int Course
        {
            get { return course; }
        }

        public ProductItemState State
        {
            get { return state; }
        }

        public DateTime? PrintTime
        {
            get { return printTime; }
        }

        public DateTime? CookingStartTime
        {
            get { return cookingStartTime; }
        }

        public DateTime? CookingFinishTime
        {
            get { return cookingFinishTime; }
        }

        public DateTime? DeliverTime
        {
            get { return deliverTime; }
        }

        public int ServeGroupNumber
        {
            get { return serveGroupNumber; }
        }

        public ICookingPlaceType CookingPlaceType
        {
            get { return cookingPlaceType; }
        }

        public IRestaurantSection CookingPlace
        {
            get { return cookingPlace; }
        }

        public bool HasMix
        {
            get { return hasMix; }
        }

        public bool MixDeleted
        {
            get { return mixDeleted; }
        }

        public string ServiceChequeBarcode
        {
            get { return GetLocalizedValue(serviceChequeBarcode); }
        }

        public IEnumerable<ISimpleModifier> SimpleModifiers
        {
            get { return simpleModifiers; }
        }

        public IEnumerable<IGroupModifier> GroupModifiers
        {
            get { return groupModifiers; }
        }

        public IEnumerable<IModifierEntry> ModifierEntries
        {
            get { return modifierEntries; }
        }

        #endregion
    }

}
