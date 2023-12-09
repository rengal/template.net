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
    /// Элемент заказа
    /// </summary>
    public interface IOrderItem : IOrderEntry
    {
        /// <summary>
        /// Официант, добавивший или изменивший блюдо.
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Отпечатан ли элемент заказа
        /// </summary>
        bool Printed { get; }

    }
}
