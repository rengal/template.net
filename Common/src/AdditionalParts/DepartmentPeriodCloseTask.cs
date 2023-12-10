namespace Resto.Data
{
    /// <summary>
    /// Класс добавляет поле nullablePeriod, чтобы была возможность установить [NotNull]Period в null. 
    /// При сохранении DepartmentPeriodCloseTask, те таски, у которых дата выполнения, дата будущего мягкого закрытия периода
    /// или прериод равны null удаляются.
    /// </summary>
    public partial class DepartmentPeriodCloseTask
    {
        private ClosePeriodSchedulerPeriods nullablePeriod;
        public ClosePeriodSchedulerPeriods NullablePeriod
        {
            get { return nullablePeriod; }
            set
            {
                nullablePeriod = value;
                if (value != null)
                {
                    Period = value;
                }
            }
        }
    }
}