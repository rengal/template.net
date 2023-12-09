using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.OlapReports
{
    public interface IOlapReport
    {
        [NotNull]
        List<IOlapReportItem> Data { get; }

        [CanBeNull]
        IOlapReportItem Totals { get; }
    }

    public interface IOlapReportItem
    {
        object this[[NotNull] string field] { get; }

        T GetValue<T>([NotNull] string field);
    }
}