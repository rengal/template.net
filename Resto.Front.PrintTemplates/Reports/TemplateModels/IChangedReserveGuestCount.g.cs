// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Изменение количества гостей резерва
    /// </summary>
    public interface IChangedReserveGuestCount : IReservesGroup
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }

    }
}
