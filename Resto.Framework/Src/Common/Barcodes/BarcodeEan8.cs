using System;
using System.Diagnostics;
using Resto.Framework.Attributes.JetBrains;
using System.Linq;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Линейный штрихкод EAN8.
    /// </summary>
    public sealed class BarcodeEan8 : IBarcode, IEquatable<BarcodeEan8>
    {
        private const int ZeroCharCode = '0';
        [NotNull]
        private readonly string barcodeString = string.Empty;

        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        public BarcodeEan8()
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
        /// <paramref name="barcodeString"/> не является допустимой строкой для штрихкода EAN8
        /// (см. <see cref="IsValid"/>)
        /// </exception>
        /// <seealso cref="IsValid"/>
        public BarcodeEan8([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            if (!IsValid(barcodeString))
                throw new ArgumentOutOfRangeException(nameof(barcodeString), barcodeString, "Invalid barcode string");

            this.barcodeString = barcodeString;
        }

        public string BarcodeString => barcodeString;

        public override string ToString()
        {
            return barcodeString;
        }

        public bool Equals(BarcodeEan8 other)
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
            if (obj.GetType() != typeof(BarcodeEan8))
                return false;
            return Equals((BarcodeEan8)obj);
        }

        public override int GetHashCode()
        {
            return barcodeString.GetHashCode();
        }

        public static bool operator ==(BarcodeEan8 left, BarcodeEan8 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodeEan8 left, BarcodeEan8 right)
        {
            return !Equals(left, right);
        }

        #region Helpers
        public static char GetCheckSum([NotNull] string barcodeString)
        {
            Debug.Assert(barcodeString != null);
            // CheckSum считается по первым 7 символам
            Debug.Assert(barcodeString.Length >= 7);
            Debug.Assert(barcodeString.All(char.IsDigit));

            var checkSum = 0;

            for (var i = 0; i < 7; i++)
            {
                checkSum += (barcodeString[i] - ZeroCharCode) * (((i + 1) % 2 != 0) ? 3 : 1);
            }

            checkSum = (checkSum / 10 + (checkSum % 10 != 0 ? 1 : 0)) * 10 - checkSum;

            Debug.Assert(checkSum >= 0 && checkSum <= 9);
            return (char)(checkSum + ZeroCharCode);
        }

        /// <summary>
        /// Проверяет, является ли строка <paramref name="barcodeString"/> допустимым штрихкодом.
        /// </summary>
        /// <param name="barcodeString">Проверяемая строка</param>
        /// <returns>Результат проверки</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="barcodeString"/> <c> == null</c>
        /// </exception>
        public static bool IsValid([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));

            if (barcodeString.Length != 8)
                return false;

            for (var i = 0; i < 8; i++)    // Простой цикл гораздо быстрее LINQ и тем более регулярок
            {
                if (!char.IsDigit(barcodeString[i]))
                    return false;
            }

            return GetCheckSum(barcodeString) == barcodeString[7];
        }
        #endregion

        /// <summary>
        /// Возвращает штрих-код для заданного числа
        /// </summary>     
        public static string GetBarcode(int value)
        {
            var barCode = value.ToString("0000000");
            return barCode + GetCheckSum(barCode);
        }
    }
}