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
    /// Маршрутный лист для курьера.
    /// </summary>
    public interface ICourierRouteList : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Курьер.
        /// </summary>
        [NotNull]
        IUser Courier { get; }

        /// <summary>
        /// Список доставок, в порядке распределения.
        /// </summary>
        [NotNull]
        IEnumerable<IDelivery> Deliveries { get; }

    }

    internal sealed class CourierRouteList : TemplateModelBase, ICourierRouteList
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly User courier;
        private readonly List<Delivery> deliveries = new List<Delivery>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CourierRouteList()
        {}

        internal CourierRouteList([NotNull] CopyContext context, [NotNull] ICourierRouteList src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            courier = context.GetConverted(src.Courier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            deliveries = src.Deliveries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Delivery.Convert)).ToList();
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IUser Courier
        {
            get { return courier; }
        }

        public IEnumerable<IDelivery> Deliveries
        {
            get { return deliveries; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this ICourierRouteList cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new CourierRouteList(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ICourierRouteList>(copy, "CourierRouteList");
        }
    }
}
