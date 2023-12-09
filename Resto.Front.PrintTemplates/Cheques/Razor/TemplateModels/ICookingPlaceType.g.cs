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

    internal sealed class CookingPlaceType : TemplateModelBase, ICookingPlaceType
    {
        #region Fields
        private readonly string name;
        private readonly TimeSpan cookingTimeNormal;
        private readonly TimeSpan cookingTimePeak;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CookingPlaceType()
        {}

        private CookingPlaceType([NotNull] CopyContext context, [NotNull] ICookingPlaceType src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            cookingTimeNormal = src.CookingTimeNormal;
            cookingTimePeak = src.CookingTimePeak;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CookingPlaceType Convert([NotNull] CopyContext context, [CanBeNull] ICookingPlaceType source)
        {
            if (source == null)
                return null;

            return new CookingPlaceType(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public TimeSpan CookingTimeNormal
        {
            get { return cookingTimeNormal; }
        }

        public TimeSpan CookingTimePeak
        {
            get { return cookingTimePeak; }
        }

        #endregion
    }

}
