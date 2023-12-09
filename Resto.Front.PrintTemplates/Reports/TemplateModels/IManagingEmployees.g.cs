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
    /// Управление персоналом
    /// </summary>
    public interface IManagingEmployees : ICommonGroup
    {
        /// <summary>
        /// Дата
        /// </summary>
        DateTime? Date { get; }

        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string UserName { get; }

    }
}
