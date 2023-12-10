using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class CashRegister
    {
        public const string OfdVersionNone = "";
        public const string OfdVersion100 = "1.0";
        public const string OfdVersion105 = "1.05";
        public const string OfdVersion110 = "1.1";
        public const string OfdVersion120 = "1.2";
        public const string OfdVersionWebkassa = "webkassa.kz";

        public override string ToString()
        {
            var name = base.ToString();
            return !string.IsNullOrEmpty(name) ? string.Format(Resources.CashRegisterCashNameAndNumber, Number, name) : Number.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is CashRegister cashRegister && cashRegister.Id == Id;
        }

        /// <summary>
        /// Поддерживается ли ОФД.
        /// </summary>
        public bool IsOfdSupported =>
            string.Equals(ofdProtocolVersion, OfdVersion100) ||
            string.Equals(ofdProtocolVersion, OfdVersion105) ||
            string.Equals(ofdProtocolVersion, OfdVersion110) ||
            string.Equals(ofdProtocolVersion, OfdVersion120);

        /// <summary>
        /// Версия ОФД выше, чем 1.0.
        /// </summary>
        public bool IsOfdVersionHigher10 =>
            string.Equals(ofdProtocolVersion, OfdVersion105) ||
            string.Equals(ofdProtocolVersion, OfdVersion110) ||
            string.Equals(ofdProtocolVersion, OfdVersion120);
    }
}