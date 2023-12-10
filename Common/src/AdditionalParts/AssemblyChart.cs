using System;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class AssemblyChart
    {
        public string AppearanceNotNull
        {
            get { return appearance ?? ""; }
            set { appearance = value; }
        }

        public string OrganolepticNotNull
        {
            get { return organoleptic ?? ""; }
            set { organoleptic = value; }
        }

        public bool isIntersectInTime(AssemblyChart chart)
        {
            return !((chart.DateFrom.GetValueOrFakeDefault() > DateTo.GetValueOrFakeDefault()) || (chart.DateTo.GetValueOrFakeDefault() < DateFrom.GetValueOrFakeDefault()));
        }

        public bool isIntersectInTime(DateTime anotherDateFrom, DateTime anotherDateTo)
        {
            return !((anotherDateFrom > DateTo.GetValueOrFakeDefault()) || (anotherDateTo < DateFrom.GetValueOrFakeDefault()));
        }

        public bool IsInInterval(DateInterval interval)
        {
            return DateFrom.GetValueOrDefault() >= interval.DateFrom && DateTo.GetValueOrDefault() <= interval.DateTo;
        }

        public AssemblyChart CloneChartWithNewDate(DateTime dateFrom, DateTime dateTo)
        {
            AssemblyChart result = (AssemblyChart)DeepClone(GuidGenerator.Next());
            result.ModifiedInfo = new OperationInfo() { Date = DateTime.Now, User = ServerSession.CurrentSession.GetCurrentUser() };
            result.Items.Clear();
            foreach (AssemblyChartItem item in Items)
            {
                AssemblyChartItem newItem = (AssemblyChartItem)item.DeepClone(GuidGenerator.Next());
                newItem.AssemblyChart = result;
                result.Items.Add(newItem);
            }
            result.DateFrom = dateFrom;
            result.DateTo = dateTo;
            return result;
        }
    }
}