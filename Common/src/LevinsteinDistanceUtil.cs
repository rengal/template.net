using System;
namespace Resto.OfficeCommon
{
    public class LevinsteinDistanceUtil
    {
        /// <summary>
        /// Метод вычисляющий расстояние Левинштейна между двумя строками
        /// (подробнее http://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D0%9B%D0%B5%D0%B2%D0%B5%D0%BD%D1%88%D1%82%D0%B5%D0%B9%D0%BD%D0%B0)
        /// </summary>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        /// <returns>расстояние Левинштейна между двумя строками(количество операций вставки удаления и замены, 
        /// которые  необходимо совершить, чтобы получить одну строку из другой
        /// </returns>
        public static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null)
                string1 = "";
            if (string2 == null)
                string2 = "";
            string1 = string1.ToUpper();
            string2 = string2.ToUpper();
            int diff;
            int[,] m = new int[string1.Length + 1,string2.Length + 1];

            for (int i = 0; i <= string1.Length; i++) m[i, 0] = i;
            for (int j = 0; j <= string2.Length; j++) m[0, j] = j;

            for (int i = 1; i <= string1.Length; i++)
                for (int j = 1; j <= string2.Length; j++)
                {
                    diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                                m[i, j - 1] + 1),
                                       m[i - 1, j - 1] + diff);
                }

            return m[string1.Length, string2.Length];
        }

    }
}