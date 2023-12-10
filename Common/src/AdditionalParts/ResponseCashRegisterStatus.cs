using System;
using Resto.Framework.Data;

namespace Resto.Data
{
    [DataClass("ResponseCashRegisterStatus")]
    public class ResponseCashRegisterStatus
    {
        private CashRegisterStatusField statusField;
        private string response;
        private Nullable<int> testValue;

        public ResponseCashRegisterStatus() { }

        public ResponseCashRegisterStatus(CashRegisterStatusField statusField, string response)
        {
            this.statusField = statusField;
            this.response = response;
        }

        public CashRegisterStatusField StatusField
        {
            get { return statusField; }
            set { statusField = value; }
        }

        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        public Nullable<int> TestValue
        {
            get { return testValue; }
            set { testValue = value; }
        }

    }
}
