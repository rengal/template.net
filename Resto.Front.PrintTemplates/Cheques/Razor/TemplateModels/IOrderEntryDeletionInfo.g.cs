// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
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

    }

    internal sealed class OrderEntryDeletionInfo : TemplateModelBase, IOrderEntryDeletionInfo
    {
        #region Fields
        private readonly AuthData authData;
        private readonly OrderEntryDeletionType deletionType;
        private readonly string comment;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrderEntryDeletionInfo()
        {}

        private OrderEntryDeletionInfo([NotNull] CopyContext context, [NotNull] IOrderEntryDeletionInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            authData = context.GetConverted(src.AuthData, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.AuthData.Convert);
            deletionType = src.DeletionType;
            comment = src.Comment;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrderEntryDeletionInfo Convert([NotNull] CopyContext context, [CanBeNull] IOrderEntryDeletionInfo source)
        {
            if (source == null)
                return null;

            return new OrderEntryDeletionInfo(context, source);
        }
        #endregion

        #region Props
        public IAuthData AuthData
        {
            get { return authData; }
        }

        public OrderEntryDeletionType DeletionType
        {
            get { return deletionType; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        #endregion
    }

}
