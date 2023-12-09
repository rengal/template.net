namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Поддерживаемые типы алгоритмов шифрования
    /// </summary>
    public enum CryptoAlgorithmType
    {
        /// <summary>
        /// Без сжатия (исходные данные не изменяются)
        /// </summary>
        None,
        /// <summary>
        /// Сжатие по алгоритму DES
        /// </summary>
        DES
    }
}