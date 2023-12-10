using System;

namespace Resto.Data
{
    public partial class LabelPosition
    {
        public override bool Equals(object obj)
        {
            var lp = (LabelPosition)obj;
            return Day == lp.Day && Page == lp.Page && X == lp.X && Y == lp.Y;
        }

        public override int GetHashCode()
        {
            // Настройка "Меню зависит от дня недели" устанавливает day = null
            // Константа нужна, чтоб при вызове GetHashCode
            // значение у day не было равно null
            const int independentDay = int.MaxValue;
            int hashCode = day ?? independentDay;
            hashCode = 31*hashCode + page;
            hashCode = 31*hashCode + x;
            hashCode = 31*hashCode + y;
            return hashCode;
        }
    }
}