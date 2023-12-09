using System;

namespace Resto.Framework.Src
{
    /// <summary>
    /// Отметка времени, позволяет получить фактически прошедшее между двумя отметками время в рамках одного сеанса работы системы.
    /// В отличие от <see cref="DateTime.Now"/> гарантируется монотонное возрастание.
    /// Недопустимо хранение или обмен метками между компьютерами.
    /// </summary>
    /// <remarks>
    /// Рекомендуется использовать для реализации независящих от часов задержек
    /// (часы могут перескакивать вперёд или назад в результате синхронизации времени, перехода на летнее время, смены часового пояса и т.п.).
    /// <br/>
    /// Действуют все ограничения <see cref="Environment.TickCount"/>:
    /// <list type="bullet">
    /// <item>Не допускается хранение, обмен между компьютерами (после перезагрузки отсчёт начнётся заново, на других компьютерах свой отсчёт).</item>    
    /// <item>Не пригодно для измерения временных интервалов больше 49 дней (значения циклически повторяются).</item>
    /// <item>Не пригодно для измерения временных интервалов отрицательной длины.</item>
    /// </list>
    /// </remarks>    
    public struct TimeStamp : IComparable<TimeStamp>
    {
        /// <summary>
        /// Половина периода обращения <see cref="Environment.TickCount"/>.
        /// </summary>
        private const long SemicircleTicks = 2147483648 * TimeSpan.TicksPerMillisecond;

        private readonly int ticksCount;

        private TimeStamp(int ticksCount)
        {
            this.ticksCount = ticksCount;
        }

        /// <summary>
        /// Возвращает соответствующую текущему моменту времени отметку.
        /// </summary>
        public static TimeStamp Now()
        {
            return new TimeStamp(Environment.TickCount);
        }

        /// <summary>
        /// Возвращает наиболее старую отметку времени. Аналог <see cref="DateTime.MinValue"/> с учётом циклических повторов.
        /// </summary>        
        public static TimeStamp GetOldestTime()
        {
            // вычисляем наиболее удалённую точку цикла — она диаметрально противоположна,
            // т.е. была максимально давно и в следующий раз повторится максимально не скоро.
            unchecked
            {
                return new TimeStamp(Environment.TickCount - int.MaxValue);
            }
        }

        /// <summary>
        /// Возвращает отметку времени, соответствующую указанному количеству тиков (миллисекунд, прошедших с момента запуска системы).
        /// </summary>
        public static TimeStamp FromTicksCount(uint ticksCount)
        {
            unchecked
            {
                return new TimeStamp((int)ticksCount);
            }
        }

        /// <summary>
        /// Вычислить интервал времени, прошедший с момента <see cref="subtrahend"/> до момента <see cref="minuend"/>.
        /// </summary>
        /// <param name="minuend">Конец интервала.</param>
        /// <param name="subtrahend">Начало интервала.</param>
        /// <remarks>Длина интервала времени вычисляется по модулю 2^32 мс, поскольку в реализации используется <see cref="Environment.TickCount"/>.</remarks>
        public static TimeSpan operator -(TimeStamp minuend, TimeStamp subtrahend)
        {
            var elapsedTime = minuend.ticksCount >= subtrahend.ticksCount
                ? (long)minuend.ticksCount - subtrahend.ticksCount
                // если subtrahend больше currentTickCount, значит, currentTickCount «дотикал» до int.MaxValue и сбросился на int.MinValue
                : (long)int.MaxValue - subtrahend.ticksCount + minuend.ticksCount - int.MinValue + 1;

            return TimeSpan.FromMilliseconds(elapsedTime);
        }

        public static bool operator <(TimeStamp timeStamp1, TimeStamp timeStamp2)
        {
            return timeStamp1.CompareTo(timeStamp2) < 0;
        }

        public static bool operator >(TimeStamp timeStamp1, TimeStamp timeStamp2)
        {
            return timeStamp1.CompareTo(timeStamp2) > 0;
        }

        public int CompareTo(TimeStamp other)
        {
            if (ticksCount == other.ticksCount)
                return 0;
            var comparisionResult = SemicircleTicks.CompareTo((this - other).Ticks);
            return comparisionResult != 0
                ? comparisionResult
                // Если две точки диаметрально противоположны на круге,
                // можно с равной вероятностью предполагать, что первая больше или меньше второй.
                // При этом гарантированно одна из точек имеет отрицательное значение, а вторая положительное или ноль.
                // Для определённости будем считать, что в таких случаях больше та точка, знак которой «больше» (+ или 0).
                : other.ticksCount.CompareTo(ticksCount);
        }

        internal static TimeStamp CreateForTest(int environmentTickCount)
        {
            return new TimeStamp(environmentTickCount);
        }

        public override string ToString()
        {
            return ticksCount.ToString();
        }
    }
}
