using System;
using System.Linq;
using Resto.Common.Localization;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class DocumentNumberTemplateElement
    {
        public static string GetToolTip(bool isVatInvoice = false)
        {
            var excludedVatInvoiceValues = new[] {AUTOINCREMENT, DELIVERY_NUMBER};
            var excludedNotVatInvoiceValues = new[] {CASH_NUMBER, SESSION_NUMBER, ORDER_NUMBER, INVOICE_NUMBER };
            var values = isVatInvoice
                ? VALUES.Where(v => v.NotIn(excludedVatInvoiceValues))
                : VALUES.Where(v => v.NotIn(excludedNotVatInvoiceValues));

            return string.Join(Environment.NewLine,
                values.Select(te => $"{te.Symbol} - {te.GetLocalName()}").ToArray());
        }
    }
}