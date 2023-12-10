using System;
using System.Collections.Generic;
using System.Linq;

using Resto.Framework.Attributes.JetBrains;

using Resto.Data;

namespace Resto.Common.Extensions
{
    public static class FrontReportExtensions
    {
        [NotNull]
        public static IEnumerable<ReportParametersPage> GetPagesNullSafe([NotNull] this FrontReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            return report.Pages ?? Enumerable.Empty<ReportParametersPage>();
        }

        [NotNull]
        public static IEnumerable<ReportParameter> GetParametersNullSafe([NotNull] this ReportParametersPage page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            return page.Parameters ?? Enumerable.Empty<ReportParameter>();
        }
    }
}