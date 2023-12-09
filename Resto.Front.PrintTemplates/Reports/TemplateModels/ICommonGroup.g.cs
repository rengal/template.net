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
    /// Все события
    /// </summary>
    public interface ICommonGroup
    {
        /// <summary>
        /// Название
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Время события
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        /// Пользователь
        /// </summary>
        IUser User { get; }

    }
}
