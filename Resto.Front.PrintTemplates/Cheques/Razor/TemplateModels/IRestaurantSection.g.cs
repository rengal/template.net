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

    internal sealed class RestaurantSection : TemplateModelBase, IRestaurantSection
    {
        #region Fields
        private readonly string name;
        private readonly bool printProductItemCommentInCheque;
        private readonly bool printBarcodeInServiceCheque;
        private readonly PrintKitchenBarcodeType printKitchenBarcodeType;
        private readonly bool displayGuests;
        private readonly bool printSummaryServiceCheque;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private RestaurantSection()
        {}

        private RestaurantSection([NotNull] CopyContext context, [NotNull] IRestaurantSection src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            printProductItemCommentInCheque = src.PrintProductItemCommentInCheque;
            printBarcodeInServiceCheque = src.PrintBarcodeInServiceCheque;
            printKitchenBarcodeType = src.PrintKitchenBarcodeType;
            displayGuests = src.DisplayGuests;
            printSummaryServiceCheque = src.PrintSummaryServiceCheque;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static RestaurantSection Convert([NotNull] CopyContext context, [CanBeNull] IRestaurantSection source)
        {
            if (source == null)
                return null;

            return new RestaurantSection(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public bool PrintProductItemCommentInCheque
        {
            get { return printProductItemCommentInCheque; }
        }

        public bool PrintBarcodeInServiceCheque
        {
            get { return printBarcodeInServiceCheque; }
        }

        public PrintKitchenBarcodeType PrintKitchenBarcodeType
        {
            get { return printKitchenBarcodeType; }
        }

        public bool DisplayGuests
        {
            get { return displayGuests; }
        }

        public bool PrintSummaryServiceCheque
        {
            get { return printSummaryServiceCheque; }
        }

        #endregion
    }

}
