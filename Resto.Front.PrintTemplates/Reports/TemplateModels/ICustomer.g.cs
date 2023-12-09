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
    /// Клиент
    /// </summary>
    public interface ICustomer
    {
        /// <summary>
        /// Имя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [CanBeNull]
        string Surname { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Номер карты
        /// </summary>
        [CanBeNull]
        string CardNumber { get; }

        /// <summary>
        /// Клиент в «чёрном» списке
        /// </summary>
        bool InBlackList { get; }

        /// <summary>
        /// Причина внесения клиента в чёрный список
        /// </summary>
        [CanBeNull]
        string BlackListReason { get; }

        /// <summary>
        /// Телефоны
        /// </summary>
        [NotNull]
        IEnumerable<IPhone> Phones { get; }

    }
}
