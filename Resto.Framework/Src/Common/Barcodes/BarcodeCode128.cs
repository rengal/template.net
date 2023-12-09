using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Линейный штрихкод Code128.
    /// </summary>
    public class BarcodeCode128 : IBarcode, IEquatable<BarcodeCode128>
    {
        [NotNull]
        private readonly string barcodeString = string.Empty;

        #region Ctor

        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        [UsedImplicitly]
        private BarcodeCode128()
        {

        }

        /// <summary>
        /// Создаёт экземпляр штрихкода по строке <paramref name="barcodeString"/>
        /// </summary>
        /// <param name="barcodeString">Строка с цифрами штрихкода</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="barcodeString"/><c> == null</c>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="barcodeString"/> не является допустимой строкой для штрихкода Code128
        /// (см. <see cref="IsUkraineMark(string)"/>)
        /// </exception>
        public BarcodeCode128([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));

            this.barcodeString = barcodeString;
        }
        #endregion

        #region Props

        public string BarcodeString => barcodeString;

        #endregion

        #region Methods
        /// <summary>
        /// Формат штрихового кода маркировки продукции на Украине.
        /// Марка продукции состоит из 10 символов: букв и цифр.
        /// </summary>
        public static bool IsUkraineMark([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));

            if (barcodeString.Length != 10)
                return false;
            for (var i = 0; i < 10; i++)   // Простой цикл гораздо быстрее LINQ и тем более регулярок
            {
                if (!char.IsLetterOrDigit(barcodeString[i]))
                    return false;
            }
            return true;
        }

        public bool IsUkraineMark()
        {
            return IsUkraineMark(barcodeString);
        }

        public override string ToString()
        {
            return barcodeString;
        }
        #endregion

        #region Equality Members
        public bool Equals(BarcodeCode128 other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(other.barcodeString, barcodeString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(BarcodeCode128))
                return false;
            return Equals((BarcodeCode128)obj);
        }

        public override int GetHashCode()
        {
            return barcodeString.GetHashCode();
        }

        public static bool operator ==(BarcodeCode128 left, BarcodeCode128 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodeCode128 left, BarcodeCode128 right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
