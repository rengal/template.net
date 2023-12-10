using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;

namespace Resto.Common
{
    /// <summary>
    /// Класс-расширений для интерфейса INamed
    /// </summary>
    public static class NamedEntityExtention
    {
        /// <summary>
        /// Метод возвращает список объектов типа <see cref="INamed"/> 
        /// перечисленных через строку
        /// </summary>
        /// <param name="namedCollection"> 
        /// Список объектов для печати 
        /// </param>
        /// <param name="splitter">символ разделитель</param>
        /// <remarks>namedCollection!=null</remarks>
        public static string AsString(this IEnumerable<INamed> namedCollection, string splitter = ", ")
        {
            return string.Join(splitter, namedCollection.Select(s => s.NameLocal).ToArray());
        }
    }
}