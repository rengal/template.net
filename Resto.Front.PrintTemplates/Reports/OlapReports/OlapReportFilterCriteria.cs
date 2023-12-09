using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.OlapReports
{
    [PublicAPI]
    public abstract class OlapReportFilterCriteria
    {}

    [PublicAPI]
    public class OlapReportFilterRangeCriteria : OlapReportFilterCriteria
    {
        public OlapReportFilterRangeCriteria([NotNull] IComparable from, bool fromInclusive, [NotNull] IComparable to, bool toInclusive)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            From = from;
            To = to;
            FromInclusive = fromInclusive;
            ToInclusive = toInclusive;
        }

        public IComparable From { get; }

        public IComparable To { get; }

        public bool FromInclusive { get; }

        public bool ToInclusive { get; }
    }

    [PublicAPI]
    public sealed class OlapReportFilterDateRangeCriteria : OlapReportFilterRangeCriteria
    {
        public OlapReportFilterDateRangeCriteria(DateTime from, bool fromInclusive, DateTime to, bool toInclusive)
            : base(from, fromInclusive, to, toInclusive)
        {}
    }

    [PublicAPI]
    public abstract class OlapReportFilterValuesSetCriteria : OlapReportFilterCriteria
    {
        protected OlapReportFilterValuesSetCriteria([NotNull] IEnumerable<object> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Values = values.ToList();
        }

        public List<object> Values { get; }
    }

    [PublicAPI]
    public sealed class OlapReportFilterExcludeValuesCriteria : OlapReportFilterValuesSetCriteria
    {
        public OlapReportFilterExcludeValuesCriteria([NotNull] IEnumerable<object> values)
            : base(values)
        {}
    }

    [PublicAPI]
    public sealed class OlapReportFilterIncludeValuesCriteria : OlapReportFilterValuesSetCriteria
    {
        public OlapReportFilterIncludeValuesCriteria([NotNull] IEnumerable<object> values)
            : base(values)
        { }
    }
}