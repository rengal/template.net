using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Barcodes
{
    /// <summary>
    /// Базовый интерфейс штрихкодов.
    /// </summary>
    public interface IBarcode
    {
        /// <summary>
        /// Значение баркода
        /// </summary>
        [NotNull]
        string BarcodeString { get; }
    }
}
