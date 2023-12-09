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
    /// Позиция заказа (для отдельного тарифа повременной услуги)
    /// </summary>
    public interface IRateScheduleEntry : IOrderEntry
    {
    }

    internal sealed class RateScheduleEntry : OrderEntry, IRateScheduleEntry
    {
        #region Fields
        #endregion

        #region Ctor
        [UsedImplicitly]
        private RateScheduleEntry()
        {}

        private RateScheduleEntry([NotNull] CopyContext context, [NotNull] IRateScheduleEntry src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static RateScheduleEntry Convert([NotNull] CopyContext context, [CanBeNull] IRateScheduleEntry source)
        {
            if (source == null)
                return null;

            return new RateScheduleEntry(context, source);
        }
        #endregion

        #region Props
        #endregion
    }

}
