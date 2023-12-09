using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.OlapReports
{
    public interface IOlapReportsProvider
    {
        [PublicAPI, NotNull]
        IOlapReport BuildReport([NotNull] OlapReportSettings reportSettings);

        [PublicAPI, NotNull]
        List<IOlapReport> BuildReports([NotNull] IEnumerable<OlapReportSettings> reportsSettings);
    }
}