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
    /// Пользователь
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Номер карты
        /// </summary>
        [NotNull]
        string Card { get; }

        /// <summary>
        /// Название роли пользователя
        /// </summary>
        [CanBeNull]
        string RoleName { get; }

        /// <summary>
        /// Является ли пользователь работником
        /// </summary>
        bool IsEmployee { get; }

        /// <summary>
        /// Является ли пользователь специальным служебным пользователем
        /// </summary>
        bool IsSystem { get; }

        /// <summary>
        /// Является ли пользователь клиентом
        /// </summary>
        bool IsClient { get; }

        /// <summary>
        /// Является ли пользователь поставщиком
        /// </summary>
        bool IsSupplier { get; }

    }
}
