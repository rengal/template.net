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
    /// Информация о компонентах составных блюд
    /// </summary>
    public interface ICompoundOrderItemInfo
    {
        /// <summary>
        /// Основная половина составного блюда
        /// </summary>
        bool IsPrimaryComponent { get; }

    }
}
