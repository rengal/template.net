using System;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Двумерный штрихкод PDF417,в котором представлены марки ЕГАИС
    /// </summary>
    public sealed class BarcodePdf417 : IBarcode, IEquatable<BarcodePdf417>
    {
        #region Fields
        [NotNull]
        private readonly string barcodeString = string.Empty;
        #endregion

        #region Ctor
        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        public BarcodePdf417()
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
        /// <paramref name="barcodeString"/> не является допустимой строкой для штрихкода PDF417
        /// (см. <see cref="IsEgaisMark"/>)
        /// </exception>
        /// <seealso cref="IsEgaisMark"/>
        public BarcodePdf417([NotNull] string barcodeString)
        {
            if (barcodeString == null)
                throw new ArgumentNullException(nameof(barcodeString));
            if (!IsEgaisMark(barcodeString))
                throw new ArgumentOutOfRangeException(nameof(barcodeString), barcodeString, @"Invalid barcode string");

            this.barcodeString = barcodeString;
        }
        #endregion

        #region Props
        public string BarcodeString => barcodeString;

        #endregion

        #region Methods
        public static string TryGetEgaisAlcCode([NotNull] string barcodeString)
        {
            const string charList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (!IsEgaisMark(barcodeString))
                return null;

            var sub = barcodeString.Substring(7, 12).ToUpper();
            UInt64 ex = 1;
            UInt64 alcCode = 0;
            for (int i = sub.Length; i > 0; i--)
            {
                alcCode += (UInt64)charList.IndexOf(sub[i - 1]) * ex;
                ex *= 36;
            }
            return alcCode.ToString(CultureInfo.InvariantCulture).PadLeft(19, '0');
        }

        public static bool IsEgaisMark(string mark)
        {
            return mark.Length == 68 && mark[2] == 'N'
                || mark.Length == 69 && mark.StartsWith("10N");
        }

        public override string ToString()
        {
            return barcodeString;
        }
        #endregion

        #region Equality Members
        public bool Equals(BarcodePdf417 other)
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
            if (obj.GetType() != typeof(BarcodePdf417))
                return false;
            return Equals((BarcodePdf417)obj);
        }

        public override int GetHashCode()
        {
            return barcodeString.GetHashCode();
        }

        public static bool operator ==(BarcodePdf417 left, BarcodePdf417 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BarcodePdf417 left, BarcodePdf417 right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
