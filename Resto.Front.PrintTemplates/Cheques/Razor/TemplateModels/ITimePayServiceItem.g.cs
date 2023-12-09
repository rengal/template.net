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
    /// Элемент заказа (повременная услуга)
    /// </summary>
    public interface ITimePayServiceItem : IOrderItem
    {
        /// <summary>
        /// Ограничение по времени
        /// </summary>
        TimeSpan? TimeLimit { get; }

        /// <summary>
        /// Позиции по тарифам повременной услуги
        /// </summary>
        [NotNull]
        IEnumerable<IRateScheduleEntry> RateScheduleEntries { get; }

    }

    internal sealed class TimePayServiceItem : OrderItem, ITimePayServiceItem
    {
        #region Fields
        private readonly TimeSpan? timeLimit;
        private readonly List<RateScheduleEntry> rateScheduleEntries = new List<RateScheduleEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private TimePayServiceItem()
        {}

        private TimePayServiceItem([NotNull] CopyContext context, [NotNull] ITimePayServiceItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            timeLimit = src.TimeLimit;
            rateScheduleEntries = src.RateScheduleEntries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RateScheduleEntry.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static TimePayServiceItem Convert([NotNull] CopyContext context, [CanBeNull] ITimePayServiceItem source)
        {
            if (source == null)
                return null;

            return new TimePayServiceItem(context, source);
        }
        #endregion

        #region Props
        public TimeSpan? TimeLimit
        {
            get { return timeLimit; }
        }

        public IEnumerable<IRateScheduleEntry> RateScheduleEntries
        {
            get { return rateScheduleEntries; }
        }

        #endregion
    }

}
