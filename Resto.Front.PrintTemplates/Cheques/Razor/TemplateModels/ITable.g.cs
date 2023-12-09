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

    internal sealed class Table : TemplateModelBase, ITable
    {
        #region Fields
        private readonly int number;
        private readonly string name;
        private readonly RestaurantSection section;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Table()
        {}

        private Table([NotNull] CopyContext context, [NotNull] ITable src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
            name = src.Name;
            section = context.GetConverted(src.Section, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Table Convert([NotNull] CopyContext context, [CanBeNull] ITable source)
        {
            if (source == null)
                return null;

            return new Table(context, source);
        }
        #endregion

        #region Props
        public int Number
        {
            get { return number; }
        }

        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public IRestaurantSection Section
        {
            get { return section; }
        }

        #endregion
    }

}
