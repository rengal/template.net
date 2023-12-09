using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.OlapReports
{
    [PublicAPI]
    public sealed class OlapReportSettings
    {
        public OlapReportSettings(OlapReportType reportType,
            [NotNull] List<string> groupByRowFields,
            [NotNull] List<string> groupByColumnFields,
            [NotNull] List<string> aggregateFields,
            [NotNull] Dictionary<string, OlapReportFilterCriteria> filters)
        {
            if (groupByRowFields == null)
                throw new ArgumentNullException(nameof(groupByRowFields));
            if (groupByColumnFields == null)
                throw new ArgumentNullException(nameof(groupByColumnFields));
            if (aggregateFields == null)
                throw new ArgumentNullException(nameof(aggregateFields));
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            ReportType = reportType;
            GroupByRowFields = groupByRowFields;
            GroupByColumnFields = groupByColumnFields;
            AggregateFields = aggregateFields;
            Filters = filters;
        }

        public OlapReportType ReportType { get; }

        public List<string> GroupByRowFields { get; }

        public List<string> GroupByColumnFields { get; }

        public List<string> AggregateFields { get; }

        public Dictionary<string, OlapReportFilterCriteria> Filters { get; }
    }
}