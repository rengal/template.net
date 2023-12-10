using System;
using Resto.Framework.Data;

namespace Resto.Data
{

    [DataClass("CashRegisterStatusField")]
    [EnumClass]
    public class CashRegisterStatusField
    {
        public static readonly CashRegisterStatusField ErrorCode = new CashRegisterStatusField("ErrorCode", "ErrorCode");
        public static readonly CashRegisterStatusField RegisterDateTime = new CashRegisterStatusField("RegisterDateTime", "RegisterDateTime");
        public static readonly CashRegisterStatusField IsOfdConnected = new CashRegisterStatusField("IsOfdConnected", "IsOfdConnected");
        public static readonly CashRegisterStatusField SessionStatus = new CashRegisterStatusField("SessionStatus", "SessionStatus");
        public static readonly CashRegisterStatusField RestaurantMode = new CashRegisterStatusField("RestaurantMode", "RestaurantMode");
        public static readonly CashRegisterStatusField OfflineMode = new CashRegisterStatusField("OfflineMode", "OfflineMode");
        public static readonly CashRegisterStatusField FiscalStorage = new CashRegisterStatusField("FiscalStorage", "FiscalStorage");
        public static readonly CashRegisterStatusField ExtendedStatus = new CashRegisterStatusField("ExtendedStatus", "ExtendedStatus");
        public static readonly CashRegisterStatusField OfdResponseCode = new CashRegisterStatusField("OfdResponseCode", "OfdResponseCode");
        public static readonly CashRegisterStatusField FnResponseCode = new CashRegisterStatusField("FnResponseCode", "FnResponseCode");
        public static readonly CashRegisterStatusField OfdBufferedDocuments = new CashRegisterStatusField("OfdBufferedDocuments", "OfdBufferedDocuments");
        public static readonly CashRegisterStatusField OfdDocumentNumber = new CashRegisterStatusField("OfdDocumentNumber", "OfdDocumentNumber");
        public static readonly CashRegisterStatusField OfdUrl = new CashRegisterStatusField("OfdUrl", "OfdUrl");
        public static readonly CashRegisterStatusField OfdOrgName = new CashRegisterStatusField("OfdOrgName", "OfdOrgName");
        public static readonly CashRegisterStatusField OfdSalesAddress = new CashRegisterStatusField("OfdSalesAddress", "OfdSalesAddress");
        public static readonly CashRegisterStatusField OfdKkmRegistrationNumber = new CashRegisterStatusField("OfdKkmRegistrationNumber", "OfdKkmRegistrationNumber");
        public static readonly CashRegisterStatusField OfdChequeDateTime = new CashRegisterStatusField("OfdChequeDateTime", "OfdChequeDateTime");
        public static readonly CashRegisterStatusField OfdChequeSum = new CashRegisterStatusField("OfdChequeSum", "OfdChequeSum");
        public static readonly CashRegisterStatusField OfdTaxpayerIdNumber = new CashRegisterStatusField("OfdTaxpayerIdNumber", "OfdTaxpayerIdNumber");
        public static readonly CashRegisterStatusField StatusBarInfo = new CashRegisterStatusField("StatusBarInfo", "StatusBarInfo");
        public static readonly CashRegisterStatusField OfdFirstUnsentDocumentNumber = new CashRegisterStatusField("OfdFirstUnsentDocumentNumber", "OfdFirstUnsentDocumentNumber");

        public static readonly CashRegisterStatusField SerialNumber = new CashRegisterStatusField("SerialNumber", "SerialNumber");
        public static readonly CashRegisterStatusField SessionNumber = new CashRegisterStatusField("SessionNumber", "SessionNumber");
        public static readonly CashRegisterStatusField SalesSumTotal = new CashRegisterStatusField("SalesSumTotal", "SalesSumTotal");
        public static readonly CashRegisterStatusField SalesSum = new CashRegisterStatusField("SalesSum", "SalesSum");
        public static readonly CashRegisterStatusField RefundsCount = new CashRegisterStatusField("RefundsCount", "RefundsCount");
        public static readonly CashRegisterStatusField RefundsSum = new CashRegisterStatusField("RefundsSum", "RefundsSum");
        public static readonly CashRegisterStatusField CancelCount = new CashRegisterStatusField("CancelCount", "CancelCount");
        public static readonly CashRegisterStatusField CancelSum = new CashRegisterStatusField("CancelSum", "CancelSum");
        public static readonly CashRegisterStatusField CashPaymentSum = new CashRegisterStatusField("CashPaymentSum", "CashPaymentSum");
        public static readonly CashRegisterStatusField NonCashPaymentsSum = new CashRegisterStatusField("NonCashPaymentsSum", "NonCashPaymentsSum");
        public static readonly CashRegisterStatusField KkmRegistrationNumber = new CashRegisterStatusField("KkmRegistrationNumber", "KkmRegistrationNumber");
        public static readonly CashRegisterStatusField SalesCount = new CashRegisterStatusField("SalesCount", "SalesCount");

        private readonly string name;
        private readonly string __value;

        private CashRegisterStatusField(string __value, string name)
        {
            this.__value = __value;
            this.name = name;
        }

        public static CashRegisterStatusField Parse(string value)
        {
            switch (value)
            {
                case "ErrorCode":
                    return ErrorCode;
                case "RegisterDateTime":
                    return RegisterDateTime;
                case "IsOfdConnected":
                    return IsOfdConnected;
                case "SessionStatus":
                    return SessionStatus;
                case "RestaurantMode":
                    return RestaurantMode;
                case "OfflineMode":
                    return OfflineMode;
                case "FiscalStorage":
                    return FiscalStorage;
                case "ExtendedStatus":
                    return ExtendedStatus;
                case "OfdResponseCode":
                    return OfdResponseCode;
                case "FnResponseCode":
                    return FnResponseCode;
                case "OfdBufferedDocuments":
                    return OfdBufferedDocuments;
                case "StatusBarInfo":
                    return StatusBarInfo;
                case "OfdFirstUnsentDocumentNumber":
                    return OfdFirstUnsentDocumentNumber;
                default:
                    throw new ArgumentException("Undefined enum constant:" + value);
            }
        }

        public static CashRegisterStatusField[] VALUES
        {
            get
            {
                return new[]
                {
                    ErrorCode,
                    RegisterDateTime,
                    IsOfdConnected,
                    SessionStatus,
                    RestaurantMode,
                    OfflineMode,
                    FiscalStorage,
                    ExtendedStatus,
                    OfdResponseCode,
                    FnResponseCode,
                    OfdBufferedDocuments,
                    StatusBarInfo
                };
            }
        }

        public string _Value
        {
            get { return __value; }
        }

        public override string ToString()
        {
            return this.__value;
        }

        public string Name
        {
            get { return name; }
        }

    }
}