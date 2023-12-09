// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.RmsEntityWrappers
{
    /// <summary>
    /// Группа
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Название группы
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Режим обслуживания
        /// </summary>
        ServiceMode ServiceMode { get; }

    }
}
