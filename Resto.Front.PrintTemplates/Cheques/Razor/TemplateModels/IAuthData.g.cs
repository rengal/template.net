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

    internal sealed class AuthData : TemplateModelBase, IAuthData
    {
        #region Fields
        private readonly User user;
        private readonly Card card;
        private readonly string infoText;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private AuthData()
        {}

        private AuthData([NotNull] CopyContext context, [NotNull] IAuthData src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            user = context.GetConverted(src.User, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            card = context.GetConverted(src.Card, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Card.Convert);
            infoText = src.InfoText;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static AuthData Convert([NotNull] CopyContext context, [CanBeNull] IAuthData source)
        {
            if (source == null)
                return null;

            return new AuthData(context, source);
        }
        #endregion

        #region Props
        public IUser User
        {
            get { return user; }
        }

        public ICard Card
        {
            get { return card; }
        }

        public string InfoText
        {
            get { return GetLocalizedValue(infoText); }
        }

        #endregion
    }

}
