using System;

namespace Resto.Common
{
    /// <summary>
    /// Исключение, выбрасываемое в случаях, когда в коде не учтено
    /// какое-либо значение enum'а (можно использовать в ветках default
    /// оператора "switch (enum)" или в эквивалентных операторах "if"
    /// для enum'ов, сгенерированных ClassConverter'ом)
    /// </summary>
    /// <typeparam name="T">Тип enum'а</typeparam>
    public class UnsupportedEnumValueException<T> : Exception
    {
        public UnsupportedEnumValueException(T value)
            : this(typeof(T).Name, value)
        {
        }

        public UnsupportedEnumValueException(string argumentName, T value)
            : base("Unsupported " + argumentName + " value: " + value)
        {
        }
    }
}
