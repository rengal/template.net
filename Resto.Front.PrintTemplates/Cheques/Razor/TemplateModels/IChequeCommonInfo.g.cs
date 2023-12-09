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
    /// Общая информация
    /// </summary>
    public interface IChequeCommonInfo
    {
        /// <summary>
        /// Текущее время, зафиксированное на момент печати
        /// </summary>
        DateTime CurrentTime { get; }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        [CanBeNull]
        IUser CurrentUser { get; }

        /// <summary>
        /// Группа
        /// </summary>
        [NotNull]
        IGroup Group { get; }

        /// <summary>
        /// Настройки торгового предприятия
        /// </summary>
        [NotNull]
        ICafeSetup CafeSetup { get; }

        /// <summary>
        /// Название текущего терминала
        /// </summary>
        [NotNull]
        string CurrentTerminal { get; }

    }

    internal sealed class ChequeCommonInfo : TemplateModelBase, IChequeCommonInfo
    {
        #region Fields
        private readonly DateTime currentTime;
        private readonly User currentUser;
        private readonly Group group;
        private readonly CafeSetup cafeSetup;
        private readonly string currentTerminal;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private ChequeCommonInfo()
        {}

        private ChequeCommonInfo([NotNull] CopyContext context, [NotNull] IChequeCommonInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            currentTime = src.CurrentTime;
            currentUser = context.GetConverted(src.CurrentUser, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            group = context.GetConverted(src.Group, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Group.Convert);
            cafeSetup = context.GetConverted(src.CafeSetup, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CafeSetup.Convert);
            currentTerminal = src.CurrentTerminal;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static ChequeCommonInfo Convert([NotNull] CopyContext context, [CanBeNull] IChequeCommonInfo source)
        {
            if (source == null)
                return null;

            return new ChequeCommonInfo(context, source);
        }
        #endregion

        #region Props
        public DateTime CurrentTime
        {
            get { return currentTime; }
        }

        public IUser CurrentUser
        {
            get { return currentUser; }
        }

        public IGroup Group
        {
            get { return group; }
        }

        public ICafeSetup CafeSetup
        {
            get { return cafeSetup; }
        }

        public string CurrentTerminal
        {
            get { return GetLocalizedValue(currentTerminal); }
        }

        #endregion
    }

}
