using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class IncomingServiceListRecord
    {
        public override string EmployeePassToAccountString
        {
            get { return EmployeePassToAccount != null ? EmployeePassToAccount.NameLocal: string.Empty; }
        }
        public override string FromDateString
        {
            get { return FromDate != null ? FromDate.GetValueOrFakeDefault().ToShortDateString() : ""; }
        }
    }
}