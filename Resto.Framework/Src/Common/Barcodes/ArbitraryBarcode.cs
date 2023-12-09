using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Произвольный штрихкод, представленный в виде строки.
    /// </summary>
    public sealed class ArbitraryBarcode : IBarcode
    {
        [NotNull]
        private readonly string barcodeString = string.Empty;

        /// <summary>
        /// Пустой конструктор для корректной работы десериализатора
        /// </summary>
        [UsedImplicitly]
        private ArbitraryBarcode()
        {

        }

        public ArbitraryBarcode([NotNull] string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            barcodeString = value;
        }

        public string BarcodeString => barcodeString;

        [NotNull]
        public string Value => barcodeString;

        public override string ToString()
        {
            return Value;
        }
    }
}
