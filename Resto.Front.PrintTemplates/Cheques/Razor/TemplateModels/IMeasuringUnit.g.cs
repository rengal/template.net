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

    internal sealed class MeasuringUnit : TemplateModelBase, IMeasuringUnit
    {
        #region Fields
        private readonly string name;
        private readonly string fullName;
        private readonly MeasuringUnitKind kind;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private MeasuringUnit()
        {}

        private MeasuringUnit([NotNull] CopyContext context, [NotNull] IMeasuringUnit src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            fullName = src.FullName;
            kind = src.Kind;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static MeasuringUnit Convert([NotNull] CopyContext context, [CanBeNull] IMeasuringUnit source)
        {
            if (source == null)
                return null;

            return new MeasuringUnit(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string FullName
        {
            get { return GetLocalizedValue(fullName); }
        }

        public MeasuringUnitKind Kind
        {
            get { return kind; }
        }

        #endregion
    }

}
