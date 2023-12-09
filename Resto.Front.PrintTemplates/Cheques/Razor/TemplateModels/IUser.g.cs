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

    internal sealed class User : TemplateModelBase, IUser
    {
        #region Fields
        private readonly string name;
        private readonly string card;
        private readonly string roleName;
        private readonly bool isEmployee;
        private readonly bool isSystem;
        private readonly bool isClient;
        private readonly bool isSupplier;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private User()
        {}

        private User([NotNull] CopyContext context, [NotNull] IUser src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            card = src.Card;
            roleName = src.RoleName;
            isEmployee = src.IsEmployee;
            isSystem = src.IsSystem;
            isClient = src.IsClient;
            isSupplier = src.IsSupplier;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static User Convert([NotNull] CopyContext context, [CanBeNull] IUser source)
        {
            if (source == null)
                return null;

            return new User(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string Card
        {
            get { return GetLocalizedValue(card); }
        }

        public string RoleName
        {
            get { return GetLocalizedValue(roleName); }
        }

        public bool IsEmployee
        {
            get { return isEmployee; }
        }

        public bool IsSystem
        {
            get { return isSystem; }
        }

        public bool IsClient
        {
            get { return isClient; }
        }

        public bool IsSupplier
        {
            get { return isSupplier; }
        }

        #endregion
    }

}
