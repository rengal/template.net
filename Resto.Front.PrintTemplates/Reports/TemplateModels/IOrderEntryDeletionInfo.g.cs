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
    /// Информация об удалении позиции заказа
    /// </summary>
    public interface IOrderEntryDeletionInfo
    {
        /// <summary>
        /// Аутентификационные данные пользователя, выполнявшего/подтвердившего удаление
        /// </summary>
        [CanBeNull]
        IAuthData AuthData { get; }

        /// <summary>
        /// Тип удаления позиции заказа
        /// </summary>
        OrderEntryDeletionType DeletionType { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Тип удаления
        /// </summary>
        [CanBeNull]
        IRemovalType RemovalType { get; }

    }
}
