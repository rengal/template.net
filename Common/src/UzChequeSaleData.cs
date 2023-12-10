using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Common
{
    /// <summary>
    /// Дополнительные фискальные данные для Узбекистана
    /// </summary>
    [DataClass("UzChequeSaleData")]
    public class UzChequeSaleData
    {
        public UzChequeSaleData()
        {}

        public UzChequeSaleData([CanBeNull] string fiscalCode)
        {
            FiscalCode = fiscalCode;
        }

        /// <summary>
        /// ИКПУ, Service or Product Identifier Code (SPIC) - фискальный код товара для Узбекистана
        /// </summary>
        [CanBeNull]
        public string FiscalCode { get; set; }

    }
}
