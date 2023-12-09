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

    internal sealed class CafeSetup : TemplateModelBase, ICafeSetup
    {
        #region Fields
        private readonly string reportHeader;
        private readonly string billHeader;
        private readonly string billFooter;
        private readonly string cafeName;
        private readonly string cafeAddress;
        private readonly string legalName;
        private readonly string legalAddress;
        private readonly string taxId;
        private readonly string phone;
        private readonly string accountingReasonCode;
        private readonly string departmentCode;
        private readonly string currencyName;
        private readonly string currencyIsoName;
        private readonly string currencyShortName;
        private readonly string shortCurrencyName;
        private readonly bool includeVatInDishPrice;
        private readonly bool displayWaiterRevenueByDishes;
        private readonly bool displayRelativeNumberOfModifiers;
        private readonly Dictionary<int, string> courseCustomNames = new Dictionary<int, string>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private CafeSetup()
        {}

        private CafeSetup([NotNull] CopyContext context, [NotNull] ICafeSetup src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            reportHeader = src.ReportHeader;
            billHeader = src.BillHeader;
            billFooter = src.BillFooter;
            cafeName = src.CafeName;
            cafeAddress = src.CafeAddress;
            legalName = src.LegalName;
            legalAddress = src.LegalAddress;
            taxId = src.TaxId;
            phone = src.Phone;
            accountingReasonCode = src.AccountingReasonCode;
            departmentCode = src.DepartmentCode;
            currencyName = src.CurrencyName;
            currencyIsoName = src.CurrencyIsoName;
            currencyShortName = src.CurrencyShortName;
            shortCurrencyName = src.ShortCurrencyName;
            includeVatInDishPrice = src.IncludeVatInDishPrice;
            displayWaiterRevenueByDishes = src.DisplayWaiterRevenueByDishes;
            displayRelativeNumberOfModifiers = src.DisplayRelativeNumberOfModifiers;
            courseCustomNames = new Dictionary<int, string>(src.CourseCustomNames);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static CafeSetup Convert([NotNull] CopyContext context, [CanBeNull] ICafeSetup source)
        {
            if (source == null)
                return null;

            return new CafeSetup(context, source);
        }
        #endregion

        #region Props
        public string ReportHeader
        {
            get { return GetLocalizedValue(reportHeader); }
        }

        public string BillHeader
        {
            get { return GetLocalizedValue(billHeader); }
        }

        public string BillFooter
        {
            get { return GetLocalizedValue(billFooter); }
        }

        public string CafeName
        {
            get { return GetLocalizedValue(cafeName); }
        }

        public string CafeAddress
        {
            get { return GetLocalizedValue(cafeAddress); }
        }

        public string LegalName
        {
            get { return GetLocalizedValue(legalName); }
        }

        public string LegalAddress
        {
            get { return GetLocalizedValue(legalAddress); }
        }

        public string TaxId
        {
            get { return GetLocalizedValue(taxId); }
        }

        public string Phone
        {
            get { return GetLocalizedValue(phone); }
        }

        public string AccountingReasonCode
        {
            get { return GetLocalizedValue(accountingReasonCode); }
        }

        public string DepartmentCode
        {
            get { return GetLocalizedValue(departmentCode); }
        }

        public string CurrencyName
        {
            get { return GetLocalizedValue(currencyName); }
        }

        public string CurrencyIsoName
        {
            get { return GetLocalizedValue(currencyIsoName); }
        }

        public string CurrencyShortName
        {
            get { return GetLocalizedValue(currencyShortName); }
        }

        public string ShortCurrencyName
        {
            get { return GetLocalizedValue(shortCurrencyName); }
        }

        public bool IncludeVatInDishPrice
        {
            get { return includeVatInDishPrice; }
        }

        public bool DisplayWaiterRevenueByDishes
        {
            get { return displayWaiterRevenueByDishes; }
        }

        public bool DisplayRelativeNumberOfModifiers
        {
            get { return displayRelativeNumberOfModifiers; }
        }

        public IDictionary<int, string> CourseCustomNames
        {
            get { return courseCustomNames; }
        }

        #endregion
    }

}
