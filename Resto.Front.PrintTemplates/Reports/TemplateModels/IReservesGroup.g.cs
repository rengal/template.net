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
    /// События работы с резервами/банкетами
    /// </summary>
    public interface IReservesGroup : ICommonGroup
    {
        /// <summary>
        /// Резерв/банкет
        /// </summary>
        IReserve Reserve { get; }

        /// <summary>
        /// К-во гостей
        /// </summary>
        int? NumGuests { get; }

        /// <summary>
        /// Доп. авторизация
        /// </summary>
        IUser Auth { get; }

    }
}
