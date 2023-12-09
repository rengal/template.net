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
    /// Выдача подменной карты
    /// </summary>
    public interface ISubstitutionCardRegistered : IManagingEmployees
    {
        /// <summary>
        /// Доп. авторизация
        /// </summary>
        IUser Auth { get; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        IUser Employee { get; }

        /// <summary>
        /// Должность
        /// </summary>
        string RoleName { get; }

        /// <summary>
        /// Номер карты
        /// </summary>
        string CardNumber { get; }

        /// <summary>
        /// Причина
        /// </summary>
        string Reason { get; }

    }
}
