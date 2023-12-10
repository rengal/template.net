using System;
using Resto.Common.Extensions;
using Resto.Common.Localization;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class PayrollDocumentListRecord : IComparable<PayrollDocumentListRecord>
    {
        public override string ToString()
        {
            return string.Format(Resources.PayrollDocumentStringFormat,
                                 DocumentType.PAYROLL.GetLocalName(),
                                 Number,
                                 DateInterval.From.GetValueOrFakeDefault().ToShortDateString(),
                                 DateInterval.To.GetValueOrFakeDefault().ToShortDateString());
        }

        public int CompareTo(PayrollDocumentListRecord record)
        {
            var result = Department.CompareTo(record.Department);

            if (result != 0) return result;

            return DateTime.Compare(DateInterval.From.GetValueOrFakeDefault(), record.DateInterval.From.GetValueOrFakeDefault());
        }
    }
}