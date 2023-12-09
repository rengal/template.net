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
    /// Базовый тип сервисного чека
    /// </summary>
    public interface IServiceChequeBase : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Счётчик печати
        /// </summary>
        int PrinterCounter { get; }

        /// <summary>
        /// Место приготовления, для которого печатается чек
        /// </summary>
        [NotNull]
        IRestaurantSection CookingPlace { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

    }

    internal abstract class ServiceChequeBase : TemplateModelBase, IServiceChequeBase
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly int printerCounter;
        private readonly RestaurantSection cookingPlace;
        private readonly Order order;
        #endregion

        #region Ctor
        protected ServiceChequeBase()
        {}

        protected ServiceChequeBase([NotNull] CopyContext context, [NotNull] IServiceChequeBase src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            printerCounter = src.PrinterCounter;
            cookingPlace = context.GetConverted(src.CookingPlace, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public int PrinterCounter
        {
            get { return printerCounter; }
        }

        public IRestaurantSection CookingPlace
        {
            get { return cookingPlace; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        #endregion
    }

}
