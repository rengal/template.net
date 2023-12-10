namespace Resto.Data
{
    public abstract partial class FilterCriteria
    {
        /// <summary>
        /// Возвращает true, если значение theValue должно быть отмечено в попапе (редакторе фильтра) для колонки грида.
        /// </summary>
        public virtual bool IsValueChecked(object theValue)
        {
            return true;
        }
    }
}
