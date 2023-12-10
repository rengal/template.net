namespace Resto.Data
{
    partial class DiscountPeriod
    {
        public PeriodScheduleInfo ExtractPeriodScheduleInfo()
        {
            var result = new PeriodScheduleInfo(Id, NameLocal);
            result.Periods.AddRange(Periods);

            return result;
        }
    }
}
