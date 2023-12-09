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
    /// Аутентификационные данные
    /// </summary>
    public interface IAuthData
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        [NotNull]
        IUser User { get; }

        /// <summary>
        /// Прокатанная карта
        /// </summary>
        [CanBeNull]
        ICard Card { get; }

        /// <summary>
        /// Текстовое представление аутентификационных данных
        /// </summary>
        [NotNull]
        string InfoText { get; }

    }
}
