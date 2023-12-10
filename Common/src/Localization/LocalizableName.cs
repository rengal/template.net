
using System;

namespace Resto.Common.Localization
{
    /// <summary>
    /// Базовый класс реализующий 
    /// + тестовое представление локализованных элементов 
    /// + сравнение локализованных элементов
    /// </summary>
    public abstract class LocalizableName<T> : ILocalizableName, IComparable, IComparable<T> where T : LocalizableName<T>
    {
        public int CompareTo(object obj)
        {
            return CompareTo(obj as T);
        }

        public int CompareTo(T other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(this.GetLocalName(), other.GetLocalName(), StringComparison.CurrentCulture);
        }

        public override string ToString()
        {
            return this.GetLocalName();
        }

        public abstract string NameResId { get; }
    }
}
