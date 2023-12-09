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

    internal sealed class Group : TemplateModelBase, IGroup
    {
        #region Fields
        private readonly string name;
        private readonly ServiceMode serviceMode;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Group()
        {}

        private Group([NotNull] CopyContext context, [NotNull] IGroup src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            serviceMode = src.ServiceMode;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Group Convert([NotNull] CopyContext context, [CanBeNull] IGroup source)
        {
            if (source == null)
                return null;

            return new Group(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public ServiceMode ServiceMode
        {
            get { return serviceMode; }
        }

        #endregion
    }

}
