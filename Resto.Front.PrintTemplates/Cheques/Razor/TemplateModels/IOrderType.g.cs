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

    internal sealed class OrderType : TemplateModelBase, IOrderType
    {
        #region Fields
        private readonly string name;
        private readonly OrderServiceType orderServiceType;
        private readonly bool defaultForServiceType;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrderType()
        {}

        private OrderType([NotNull] CopyContext context, [NotNull] IOrderType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            orderServiceType = src.OrderServiceType;
            defaultForServiceType = src.DefaultForServiceType;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderType Convert([NotNull] CopyContext context, [CanBeNull] IOrderType source)
        {
            if (source == null)
                return null;

            return new OrderType(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public OrderServiceType OrderServiceType
        {
            get { return orderServiceType; }
        }

        public bool DefaultForServiceType
        {
            get { return defaultForServiceType; }
        }

        #endregion
    }

}
