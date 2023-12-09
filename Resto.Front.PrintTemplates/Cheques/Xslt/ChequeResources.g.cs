﻿// This file was generated with T4.
// Do not edit it manually.


using System.Xml.Linq;
using Resto.Front.Localization.Resources;

namespace Resto.Front.PrintTemplates.Cheques.Xslt
{
    public static class ChequeResources
    {
        internal static void AppendToXml(XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            container.Add(new XAttribute("BillFooterTotal", ChequeLocalResources.BillFooterTotal));
            container.Add(new XAttribute("BillFooterTotalWithoutDiscounts", ChequeLocalResources.BillFooterTotalWithoutDiscounts));
            container.Add(new XAttribute("BillFooterTotalGuestPattern", ChequeLocalResources.BillFooterTotalGuestPattern));
            container.Add(new XAttribute("BillFooterTotalPlain", ChequeLocalResources.BillFooterTotalPlain));
            container.Add(new XAttribute("BillDiscountMarketingCampaignNameFormat", ChequeLocalResources.BillDiscountMarketingCampaignNameFormat));
            container.Add(new XAttribute("BillHeaderCashier", ChequeLocalResources.BillHeaderCashier));
            container.Add(new XAttribute("BillHeaderCashierPattern", ChequeLocalResources.BillHeaderCashierPattern));
            container.Add(new XAttribute("BillHeaderOrderNumberPattern", ChequeLocalResources.BillHeaderOrderNumberPattern));
            container.Add(new XAttribute("BillHeaderOrderOpenPattern", ChequeLocalResources.BillHeaderOrderOpenPattern));
            container.Add(new XAttribute("BillHeaderTablePattern", ChequeLocalResources.BillHeaderTablePattern));
            container.Add(new XAttribute("BillHeaderSectionPattern", ChequeLocalResources.BillHeaderSectionPattern));
            container.Add(new XAttribute("BillHeaderLocationAndGuestsPattern", ChequeLocalResources.BillHeaderLocationAndGuestsPattern));
            container.Add(new XAttribute("ServiceChequeHeaderLocationPattern", ChequeLocalResources.ServiceChequeHeaderLocationPattern));
            container.Add(new XAttribute("BillHeaderTerminalSessionPattern", ChequeLocalResources.BillHeaderTerminalSessionPattern));
            container.Add(new XAttribute("BillHeaderTitle", ChequeLocalResources.BillHeaderTitle));
            container.Add(new XAttribute("BillHeaderWaiterPattern", ChequeLocalResources.BillHeaderWaiterPattern));
            container.Add(new XAttribute("BillMultiplier", ChequeLocalResources.BillMultiplier));
            container.Add(new XAttribute("KitchenCourse1Title", ChequeLocalResources.KitchenCourse1Title));
            container.Add(new XAttribute("KitchenCourse2Title", ChequeLocalResources.KitchenCourse2Title));
            container.Add(new XAttribute("KitchenCourse3Title", ChequeLocalResources.KitchenCourse3Title));
            container.Add(new XAttribute("KitchenCourse4Title", ChequeLocalResources.KitchenCourse4Title));
            container.Add(new XAttribute("KitchenCourseTitleFormat", ChequeLocalResources.KitchenCourseTitleFormat));
            container.Add(new XAttribute("KitchenCourseVipTitle", ChequeLocalResources.KitchenCourseVipTitle));
            container.Add(new XAttribute("KitchenCourseVipTitlePrefix", ChequeLocalResources.KitchenCourseVipTitlePrefix));
            container.Add(new XAttribute("KitchenHeaderPattern", ChequeLocalResources.KitchenHeaderPattern));
            container.Add(new XAttribute("KitchenModifiersDeletedWarningPattern", ChequeLocalResources.KitchenModifiersDeletedWarningPattern));
            container.Add(new XAttribute("KitchenProductAmountPattern", ChequeLocalResources.KitchenProductAmountPattern));
            container.Add(new XAttribute("KitchenProductDeletedWarningPattern", ChequeLocalResources.KitchenProductDeletedWarningPattern));
            container.Add(new XAttribute("KitchenTableChangedWarningPattern", ChequeLocalResources.KitchenTableChangedWarningPattern));
            container.Add(new XAttribute("KitchenProductMovedWarningPattern", ChequeLocalResources.KitchenProductMovedWarningPattern));
            container.Add(new XAttribute("KitchenRepeated", ChequeLocalResources.KitchenRepeated));
            container.Add(new XAttribute("KitchenTablePattern", ChequeLocalResources.KitchenTablePattern));
            container.Add(new XAttribute("Signature", ChequeLocalResources.Signature));
            container.Add(new XAttribute("BillFooterDiscountNamePatternDetailed", ChequeLocalResources.BillFooterDiscountNamePatternDetailed));
            container.Add(new XAttribute("BillFooterDiscountNamePatternShort", ChequeLocalResources.BillFooterDiscountNamePatternShort));
            container.Add(new XAttribute("BillFooterDiscount", ChequeLocalResources.BillFooterDiscount));
            container.Add(new XAttribute("BillFooterIncreaseNamePatternDetailed", ChequeLocalResources.BillFooterIncreaseNamePatternDetailed));
            container.Add(new XAttribute("BillFooterIncreaseNamePatternShort", ChequeLocalResources.BillFooterIncreaseNamePatternShort));
            container.Add(new XAttribute("ForDishTitle", ChequeLocalResources.ForDishTitle));
            container.Add(new XAttribute("KitchenModifierAmountPattern", ChequeLocalResources.KitchenModifierAmountPattern));
            container.Add(new XAttribute("KitchenModifierAbsoluteAmountPattern", ChequeLocalResources.KitchenModifierAbsoluteAmountPattern));
            container.Add(new XAttribute("BillFooterDiscountPercentPattern", ChequeLocalResources.BillFooterDiscountPercentPattern));
            container.Add(new XAttribute("Mix", ChequeLocalResources.Mix));
            container.Add(new XAttribute("PartOfMixText", ChequeLocalResources.PartOfMixText));
            container.Add(new XAttribute("TimeToServeCourseTemplate", ChequeLocalResources.TimeToServeCourseTemplate));
            container.Add(new XAttribute("TimeToServeDishes", ChequeLocalResources.TimeToServeDishes));
            container.Add(new XAttribute("SeeDetailsInCookingRequestText", ChequeLocalResources.SeeDetailsInCookingRequestText));
            container.Add(new XAttribute("Prepay", ChequeLocalResources.Prepay));
            container.Add(new XAttribute("PrepayCheque", ChequeLocalResources.PrepayCheque));
            container.Add(new XAttribute("PrepayReturn", ChequeLocalResources.PrepayReturn));
            container.Add(new XAttribute("PrepayReturnCheque", ChequeLocalResources.PrepayReturnCheque));
            container.Add(new XAttribute("ResultSum", ChequeLocalResources.ResultSum));
            container.Add(new XAttribute("VatSum", ChequeLocalResources.VatSum));
            container.Add(new XAttribute("ResultSumWithoutNds", ChequeLocalResources.ResultSumWithoutNds));
            container.Add(new XAttribute("BanquetNumberServiceChequeHeaderTemplate", ChequeLocalResources.BanquetNumberServiceChequeHeaderTemplate));
            container.Add(new XAttribute("BanquetComments", ChequeLocalResources.BanquetComments));
            container.Add(new XAttribute("PreliminaryOrder", ChequeLocalResources.PreliminaryOrder));
            container.Add(new XAttribute("BanquetEndTime", ChequeLocalResources.BanquetEndTime));
            container.Add(new XAttribute("BanquetEndTimeFormat", ChequeLocalResources.BanquetEndTimeFormat));
            container.Add(new XAttribute("BanquetGuestCount", ChequeLocalResources.BanquetGuestCount));
            container.Add(new XAttribute("BanquetOrderIsEmpty", ChequeLocalResources.BanquetOrderIsEmpty));
            container.Add(new XAttribute("BanquetStartDate", ChequeLocalResources.BanquetStartDate));
            container.Add(new XAttribute("BanquetStartDateFormat", ChequeLocalResources.BanquetStartDateFormat));
            container.Add(new XAttribute("BanquetStartDayOfWeekFormat", ChequeLocalResources.BanquetStartDayOfWeekFormat));
            container.Add(new XAttribute("BanquetStartTime", ChequeLocalResources.BanquetStartTime));
            container.Add(new XAttribute("BanquetStartTimeFormat", ChequeLocalResources.BanquetStartTimeFormat));
            container.Add(new XAttribute("BanquetSumPerGuestFormat", ChequeLocalResources.BanquetSumPerGuestFormat));
            container.Add(new XAttribute("BanquetTables", ChequeLocalResources.BanquetTables));
            container.Add(new XAttribute("BanquetType", ChequeLocalResources.BanquetType));
            container.Add(new XAttribute("ClientDiscountCardType", ChequeLocalResources.ClientDiscountCardType));
            container.Add(new XAttribute("ClientFullName", ChequeLocalResources.ClientFullName));
            container.Add(new XAttribute("ClientFullNameFormat", ChequeLocalResources.ClientFullNameFormat));
            container.Add(new XAttribute("ClientPhone", ChequeLocalResources.ClientPhone));
            container.Add(new XAttribute("ProductAmount", ChequeLocalResources.ProductAmount));
            container.Add(new XAttribute("ProductName", ChequeLocalResources.ProductName));
            container.Add(new XAttribute("ProductPrice", ChequeLocalResources.ProductPrice));
            container.Add(new XAttribute("ProductSum", ChequeLocalResources.ProductSum));
            container.Add(new XAttribute("Account", ChequeLocalResources.Account));
            container.Add(new XAttribute("AuthorizedByFormat", ChequeLocalResources.AuthorizedByFormat));
            container.Add(new XAttribute("CafeSessionClose", ChequeLocalResources.CafeSessionClose));
            container.Add(new XAttribute("CashRegisterFormat", ChequeLocalResources.CashRegisterFormat));
            container.Add(new XAttribute("CounteragentFormat", ChequeLocalResources.CounteragentFormat));
            container.Add(new XAttribute("PaidIn", ChequeLocalResources.PaidIn));
            container.Add(new XAttribute("PaidOut", ChequeLocalResources.PaidOut));
            container.Add(new XAttribute("PayIn", ChequeLocalResources.PayIn));
            container.Add(new XAttribute("PayOut", ChequeLocalResources.PayOut));
            container.Add(new XAttribute("ReasonFormat", ChequeLocalResources.ReasonFormat));
            container.Add(new XAttribute("AuthorizedByList", ChequeLocalResources.AuthorizedByList));
            container.Add(new XAttribute("BulletMinusFormat", ChequeLocalResources.BulletMinusFormat));
            container.Add(new XAttribute("CafeSession", ChequeLocalResources.CafeSession));
            container.Add(new XAttribute("Recalculation", ChequeLocalResources.Recalculation));
            container.Add(new XAttribute("RecalculationCompanyHeader", ChequeLocalResources.RecalculationCompanyHeader));
            container.Add(new XAttribute("RecalculationExact", ChequeLocalResources.RecalculationExact));
            container.Add(new XAttribute("RecalculationFinalSum", ChequeLocalResources.RecalculationFinalSum));
            container.Add(new XAttribute("RecalculationInitiator", ChequeLocalResources.RecalculationInitiator));
            container.Add(new XAttribute("RecalculationMinusSum", ChequeLocalResources.RecalculationMinusSum));
            container.Add(new XAttribute("RecalculationPlusSum", ChequeLocalResources.RecalculationPlusSum));
            container.Add(new XAttribute("RecalculationPriorSum", ChequeLocalResources.RecalculationPriorSum));
            container.Add(new XAttribute("RecalculationRealSum", ChequeLocalResources.RecalculationRealSum));
            container.Add(new XAttribute("ResponsibleCashiersList", ChequeLocalResources.ResponsibleCashiersList));
            container.Add(new XAttribute("ChequeNumberFormat", ChequeLocalResources.ChequeNumberFormat));
            container.Add(new XAttribute("TableNumberFormat", ChequeLocalResources.TableNumberFormat));
            container.Add(new XAttribute("TableAndSectionNumberFormat", ChequeLocalResources.TableAndSectionNumberFormat));
            container.Add(new XAttribute("CookingPlaceTemplate", ChequeLocalResources.CookingPlaceTemplate));
            container.Add(new XAttribute("StornoCheque", ChequeLocalResources.StornoCheque));
            container.Add(new XAttribute("CafeSessionOpenDate", ChequeLocalResources.CafeSessionOpenDate));
            container.Add(new XAttribute("DateTimeFormat", ChequeLocalResources.DateTimeFormat));
            container.Add(new XAttribute("CurrentDate", ChequeLocalResources.CurrentDate));
            container.Add(new XAttribute("PayInOutType", ChequeLocalResources.PayInOutType));
            container.Add(new XAttribute("PrepayForOrder", ChequeLocalResources.PrepayForOrder));
            container.Add(new XAttribute("CreatedColon", ChequeLocalResources.CreatedColon));
            container.Add(new XAttribute("DeliveryBill", ChequeLocalResources.DeliveryBill));
            container.Add(new XAttribute("Copy", ChequeLocalResources.Copy));
            container.Add(new XAttribute("ConsignorColon", ChequeLocalResources.ConsignorColon));
            container.Add(new XAttribute("ConsigneeColon", ChequeLocalResources.ConsigneeColon));
            container.Add(new XAttribute("HeadCafeTaxIdNoColonFormat", ChequeLocalResources.HeadCafeTaxIdNoColonFormat));
            container.Add(new XAttribute("DeliveryCreatedColon", ChequeLocalResources.DeliveryCreatedColon));
            container.Add(new XAttribute("DeliveryBillPrintedColon", ChequeLocalResources.DeliveryBillPrintedColon));
            container.Add(new XAttribute("DeliveryDateTimeColon", ChequeLocalResources.DeliveryDateTimeColon));
            container.Add(new XAttribute("DeliveryRealDateTimeColon", ChequeLocalResources.DeliveryRealDateTimeColon));
            container.Add(new XAttribute("AddressRegionFormat", ChequeLocalResources.AddressRegionFormat));
            container.Add(new XAttribute("Discount", ChequeLocalResources.Discount));
            container.Add(new XAttribute("Increase", ChequeLocalResources.Increase));
            container.Add(new XAttribute("FreightDeliveriedFormat", ChequeLocalResources.FreightDeliveriedFormat));
            container.Add(new XAttribute("FreightReceivedColon", ChequeLocalResources.FreightReceivedColon));
            container.Add(new XAttribute("DeliveryProcessingCommentsColon", ChequeLocalResources.DeliveryProcessingCommentsColon));
            container.Add(new XAttribute("ModifiedColon", ChequeLocalResources.ModifiedColon));
            container.Add(new XAttribute("ManagerColon", ChequeLocalResources.ManagerColon));
            container.Add(new XAttribute("ClientFormat", ChequeLocalResources.ClientFormat));
            container.Add(new XAttribute("SumFormat", ChequeLocalResources.SumFormat));
            container.Add(new XAttribute("ItemsCount", ChequeLocalResources.ItemsCount));
            container.Add(new XAttribute("MemoChequeSignature", ChequeLocalResources.MemoChequeSignature));
            container.Add(new XAttribute("Released", ChequeLocalResources.Released));
            container.Add(new XAttribute("CashMemoChequeTitle", ChequeLocalResources.CashMemoChequeTitle));
            container.Add(new XAttribute("NameTitle", ChequeLocalResources.NameTitle));
            container.Add(new XAttribute("BillFooterTotalLower", ChequeLocalResources.BillFooterTotalLower));
            container.Add(new XAttribute("FooterTotalUpper", ChequeLocalResources.FooterTotalUpper));
            container.Add(new XAttribute("BarcodeAmount", ChequeLocalResources.BarcodeAmount));
            container.Add(new XAttribute("BarcodePrice", ChequeLocalResources.BarcodePrice));
            container.Add(new XAttribute("BarcodeSum", ChequeLocalResources.BarcodeSum));
            container.Add(new XAttribute("BarcodeExpiration", ChequeLocalResources.BarcodeExpiration));
            container.Add(new XAttribute("BillFooterFullSum", ChequeLocalResources.BillFooterFullSum));
            container.Add(new XAttribute("NameColumnHeader", ChequeLocalResources.NameColumnHeader));
            container.Add(new XAttribute("ChequeNumberTemplate", ChequeLocalResources.ChequeNumberTemplate));
            container.Add(new XAttribute("RoomTemplate", ChequeLocalResources.RoomTemplate));
            container.Add(new XAttribute("GuestNameTemplate", ChequeLocalResources.GuestNameTemplate));
            container.Add(new XAttribute("PrepayTemplate", ChequeLocalResources.PrepayTemplate));
            container.Add(new XAttribute("ZeroChequeBody", ChequeLocalResources.ZeroChequeBody));
            container.Add(new XAttribute("AmountShort", ChequeLocalResources.AmountShort));
            container.Add(new XAttribute("ChangePayIn", ChequeLocalResources.ChangePayIn));
            container.Add(new XAttribute("CardPattern", ChequeLocalResources.CardPattern));
            container.Add(new XAttribute("HeadCashRegisterShift", ChequeLocalResources.HeadCashRegisterShift));
            container.Add(new XAttribute("OrderPaymentReceipt", ChequeLocalResources.OrderPaymentReceipt));
            container.Add(new XAttribute("OrderBuyReceipt", ChequeLocalResources.OrderBuyReceipt));
            container.Add(new XAttribute("AllSumsInFormat", ChequeLocalResources.AllSumsInFormat));
            container.Add(new XAttribute("Total", ChequeLocalResources.Total));
            container.Add(new XAttribute("Change", ChequeLocalResources.Change));
            container.Add(new XAttribute("AmountWithPriceTitle", ChequeLocalResources.AmountWithPriceTitle));
            container.Add(new XAttribute("GuestSign", ChequeLocalResources.GuestSign));
            container.Add(new XAttribute("DiscountCardNumber", ChequeLocalResources.DiscountCardNumber));
            container.Add(new XAttribute("CalculatedChequeSaleFormat", ChequeLocalResources.CalculatedChequeSaleFormat));
            container.Add(new XAttribute("GuestPlaceTitle", ChequeLocalResources.GuestPlaceTitle));
            container.Add(new XAttribute("ActivityTypeFormat", ChequeLocalResources.ActivityTypeFormat));
            container.Add(new XAttribute("BanquetReserveType", ChequeLocalResources.BanquetReserveType));
            container.Add(new XAttribute("DiscountCardTypeNameFormat", ChequeLocalResources.DiscountCardTypeNameFormat));
            container.Add(new XAttribute("GuestsCountFormat", ChequeLocalResources.GuestsCountFormat));
            container.Add(new XAttribute("ReserveCommentFormat", ChequeLocalResources.ReserveCommentFormat));
            container.Add(new XAttribute("ReserveCustomerNameFormat", ChequeLocalResources.ReserveCustomerNameFormat));
            container.Add(new XAttribute("ReserveCustomerPhoneNumbersFormat", ChequeLocalResources.ReserveCustomerPhoneNumbersFormat));
            container.Add(new XAttribute("ReservedSectionTablesFormat", ChequeLocalResources.ReservedSectionTablesFormat));
            container.Add(new XAttribute("ReserveDurationFormat", ChequeLocalResources.ReserveDurationFormat));
            container.Add(new XAttribute("ReserveNumberFormat", ChequeLocalResources.ReserveNumberFormat));
            container.Add(new XAttribute("ReserveReserveType", ChequeLocalResources.ReserveReserveType));
            container.Add(new XAttribute("TablesReserveHeaderTitle", ChequeLocalResources.TablesReserveHeaderTitle));
            container.Add(new XAttribute("ParentProductForModifierPattern", ChequeLocalResources.ParentProductForModifierPattern));
            container.Add(new XAttribute("CashMemoChequeDiscountItemNameAndPercentFormat", ChequeLocalResources.CashMemoChequeDiscountItemNameAndPercentFormat));
            container.Add(new XAttribute("IikoCardFooterOperationDate", ChequeLocalResources.IikoCardFooterOperationDate));
            container.Add(new XAttribute("IikoCardCardOwner", ChequeLocalResources.IikoCardCardOwner));
            container.Add(new XAttribute("IikoCardCardNumber", ChequeLocalResources.IikoCardCardNumber));
            container.Add(new XAttribute("IikoCardOperationFailed", ChequeLocalResources.IikoCardOperationFailed));
            container.Add(new XAttribute("IikoCardOperationSuccessful", ChequeLocalResources.IikoCardOperationSuccessful));
            container.Add(new XAttribute("IikoCardRowSum", ChequeLocalResources.IikoCardRowSum));
            container.Add(new XAttribute("Terminal", ChequeLocalResources.Terminal));
            container.Add(new XAttribute("AdditionalServiceHeaderTitle", ChequeLocalResources.AdditionalServiceHeaderTitle));
            container.Add(new XAttribute("AdditionalServiceHeaderOrderItemsAddedPattern", ChequeLocalResources.AdditionalServiceHeaderOrderItemsAddedPattern));
            container.Add(new XAttribute("AdditionalServiceFooterTotalGuestPattern", ChequeLocalResources.AdditionalServiceFooterTotalGuestPattern));
            container.Add(new XAttribute("AdditionalServiceAddedFooterTotalUpper", ChequeLocalResources.AdditionalServiceAddedFooterTotalUpper));
            container.Add(new XAttribute("AdditionalServiceFooterTotalUpper", ChequeLocalResources.AdditionalServiceFooterTotalUpper));
            container.Add(new XAttribute("AdditionalServiceLimit", ChequeLocalResources.AdditionalServiceLimit));
            container.Add(new XAttribute("AddressFormat", ChequeLocalResources.AddressFormat));
            container.Add(new XAttribute("CourierColon", ChequeLocalResources.CourierColon));
            container.Add(new XAttribute("Cash", ChequeLocalResources.Cash));
            container.Add(new XAttribute("VatFormat", ChequeLocalResources.VatFormat));
            container.Add(new XAttribute("GuestNumFormat", ChequeLocalResources.GuestNumFormat));
            container.Add(new XAttribute("BillHeaderOrderPattern", ChequeLocalResources.BillHeaderOrderPattern));
            container.Add(new XAttribute("CounteragentCardFormat", ChequeLocalResources.CounteragentCardFormat));
            container.Add(new XAttribute("CounteragentNameCardFormat", ChequeLocalResources.CounteragentNameCardFormat));
            container.Add(new XAttribute("CaloricityFormat", ChequeLocalResources.CaloricityFormat));
            container.Add(new XAttribute("CarbohydrateFormat", ChequeLocalResources.CarbohydrateFormat));
            container.Add(new XAttribute("ExpirationPeriodFormat", ChequeLocalResources.ExpirationPeriodFormat));
            container.Add(new XAttribute("FatFormat", ChequeLocalResources.FatFormat));
            container.Add(new XAttribute("PrintTimeColon", ChequeLocalResources.PrintTimeColon));
            container.Add(new XAttribute("ProteinFormat", ChequeLocalResources.ProteinFormat));
            container.Add(new XAttribute("CategoryColon", ChequeLocalResources.CategoryColon));
            container.Add(new XAttribute("ProductCostColon", ChequeLocalResources.ProductCostColon));
            container.Add(new XAttribute("ProductPriceColon", ChequeLocalResources.ProductPriceColon));
            container.Add(new XAttribute("FullAddressFormat", ChequeLocalResources.FullAddressFormat));
            container.Add(new XAttribute("AddressPartsDelimiter", ChequeLocalResources.AddressPartsDelimiter));
            container.Add(new XAttribute("ShortAddressStreetFormat", ChequeLocalResources.ShortAddressStreetFormat));
            container.Add(new XAttribute("ShortAddressHouseFormat", ChequeLocalResources.ShortAddressHouseFormat));
            container.Add(new XAttribute("ShortAddressBuildingFormat", ChequeLocalResources.ShortAddressBuildingFormat));
            container.Add(new XAttribute("ShortAddressFlatFormat", ChequeLocalResources.ShortAddressFlatFormat));
            container.Add(new XAttribute("ShortAddressCityFormat", ChequeLocalResources.ShortAddressCityFormat));
            container.Add(new XAttribute("ShortAddressFloorFormat", ChequeLocalResources.ShortAddressFloorFormat));
            container.Add(new XAttribute("ShortAddressDoorphoneFormat", ChequeLocalResources.ShortAddressDoorphoneFormat));
            container.Add(new XAttribute("ShortAddressEntranceFormat", ChequeLocalResources.ShortAddressEntranceFormat));
            container.Add(new XAttribute("AggregatedDiscountNameFormat", ChequeLocalResources.AggregatedDiscountNameFormat));
            container.Add(new XAttribute("AggregatedIncreaseNameFormat", ChequeLocalResources.AggregatedIncreaseNameFormat));
            container.Add(new XAttribute("DeliveryRouteList", ChequeLocalResources.DeliveryRouteList));
            container.Add(new XAttribute("RouteListMainInformation", ChequeLocalResources.RouteListMainInformation));
            container.Add(new XAttribute("PaymentItemIsProcessedNameFormat", ChequeLocalResources.PaymentItemIsProcessedNameFormat));
            container.Add(new XAttribute("IndependentModifierAmountForFormat", ChequeLocalResources.IndependentModifierAmountForFormat));
            container.Add(new XAttribute("PaymentWithDiscountSplitFormatDiscount", ChequeLocalResources.PaymentWithDiscountSplitFormatDiscount));
            container.Add(new XAttribute("PaymentWithDiscountSplitFormatPayment", ChequeLocalResources.PaymentWithDiscountSplitFormatPayment));
            container.Add(new XAttribute("PercentPattern", ChequeLocalResources.PercentPattern));
            container.Add(new XAttribute("OriginalVatInvoice", ChequeLocalResources.OriginalVatInvoice));
            container.Add(new XAttribute("DuplicateVatInvoice", ChequeLocalResources.DuplicateVatInvoice));
            container.Add(new XAttribute("PhonePattern", ChequeLocalResources.PhonePattern));
            container.Add(new XAttribute("VatInvoice_Seller", ChequeLocalResources.VatInvoice_Seller));
            container.Add(new XAttribute("VatInvoice_Purchaser", ChequeLocalResources.VatInvoice_Purchaser));
            container.Add(new XAttribute("VatInvoice_DateOfIssue", ChequeLocalResources.VatInvoice_DateOfIssue));
            container.Add(new XAttribute("VatInvoice_DateOfDelivery", ChequeLocalResources.VatInvoice_DateOfDelivery));
            container.Add(new XAttribute("VatInvoice_NameOfService", ChequeLocalResources.VatInvoice_NameOfService));
            container.Add(new XAttribute("VatInvoice_TotalNetPrice", ChequeLocalResources.VatInvoice_TotalNetPrice));
            container.Add(new XAttribute("VatInvoice_VATRate", ChequeLocalResources.VatInvoice_VATRate));
            container.Add(new XAttribute("VatInvoice_VATAmount", ChequeLocalResources.VatInvoice_VATAmount));
            container.Add(new XAttribute("VatInvoice_GrossValue", ChequeLocalResources.VatInvoice_GrossValue));
            container.Add(new XAttribute("VatInvoice_TOTAL", ChequeLocalResources.VatInvoice_TOTAL));
            container.Add(new XAttribute("VatInvoice_Taxrate", ChequeLocalResources.VatInvoice_Taxrate));
            container.Add(new XAttribute("VatInvoice_TotalDue", ChequeLocalResources.VatInvoice_TotalDue));
            container.Add(new XAttribute("VatInvoice_InWords", ChequeLocalResources.VatInvoice_InWords));
            container.Add(new XAttribute("VatInvoice_MethodOfPayment", ChequeLocalResources.VatInvoice_MethodOfPayment));
            container.Add(new XAttribute("VatInvoice_Invoice", ChequeLocalResources.VatInvoice_Invoice));
            container.Add(new XAttribute("VatInvoice_SellerSignature", ChequeLocalResources.VatInvoice_SellerSignature));
            container.Add(new XAttribute("VatInvoice_PurchaseSignature", ChequeLocalResources.VatInvoice_PurchaseSignature));
            container.Add(new XAttribute("Ampersand", ChequeLocalResources.Ampersand));
            container.Add(new XAttribute("InnPattern", ChequeLocalResources.InnPattern));
            container.Add(new XAttribute("ProductNameWithSizeFormat", ChequeLocalResources.ProductNameWithSizeFormat));
            container.Add(new XAttribute("CurrencyFormat", ChequeLocalResources.CurrencyFormat));
            container.Add(new XAttribute("CurrencyRateFormat", ChequeLocalResources.CurrencyRateFormat));
            container.Add(new XAttribute("AccountingReasonCodePatern", ChequeLocalResources.AccountingReasonCodePatern));
            container.Add(new XAttribute("VatInvoice_FiscalСheque", ChequeLocalResources.VatInvoice_FiscalСheque));
            container.Add(new XAttribute("VatInvoice_FiscalRegister", ChequeLocalResources.VatInvoice_FiscalRegister));
            container.Add(new XAttribute("VatInvoice_Sum", ChequeLocalResources.VatInvoice_Sum));
            container.Add(new XAttribute("VatPercentFormat", ChequeLocalResources.VatPercentFormat));
            container.Add(new XAttribute("Counteragent", ChequeLocalResources.Counteragent));
            container.Add(new XAttribute("RepeateBillHeaderTitleFormat", ChequeLocalResources.RepeateBillHeaderTitleFormat));
            container.Add(new XAttribute("BillHeaderTabPattern", ChequeLocalResources.BillHeaderTabPattern));
            container.Add(new XAttribute("AllergenGroupsFormat", ChequeLocalResources.AllergenGroupsFormat));
            container.Add(new XAttribute("DonationsPattern", ChequeLocalResources.DonationsPattern));
            container.Add(new XAttribute("KitchenModifierPlusMinusAmountPattern", ChequeLocalResources.KitchenModifierPlusMinusAmountPattern));
            container.Add(new XAttribute("BillHeaderOrderExternalNumberPattern", ChequeLocalResources.BillHeaderOrderExternalNumberPattern));
            container.Add(new XAttribute("PredictedCookingCompleteTime", ChequeLocalResources.PredictedCookingCompleteTime));
            container.Add(new XAttribute("DonationsAndPaymentTypeFormat", ChequeLocalResources.DonationsAndPaymentTypeFormat));
            parent.Add(container);
        }
    }
}