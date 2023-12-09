using System.Xml;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Абстрактный класс-предок для всех поддерживаемых алгоритмов шифрования
    /// </summary>
    public abstract class CrypterBase
    {
        /// <summary>
        /// Тип алгоритма
        /// </summary>
        public abstract CryptoAlgorithmType Algorithm { get; }

        /// <summary>
        /// Зашифровать блок данных
        /// </summary>
        /// <param name="data">Блок исходных данных</param>
        /// <returns>Блок зашифрованных данных</returns>
        public abstract byte[] EncryptData(byte[] data);

        /// <summary>
        /// Расшифровать блок данных
        /// </summary>
        /// <param name="data">Блок зашифрованных данных</param>
        /// <returns>Блок исходных данных</returns>
        public abstract byte[] DecryptData(byte[] data);

        /// <summary>
        /// Записать в XmlContainer секцию с идентифицирующими параметрами алгоритма (хэш ключа и т.п.)
        /// </summary>
        /// <param name="writer">XmlWriter в позиции с которой начинается секция данных криптоалгоритма</param>
        public abstract void WriteContainerInfo(XmlWriter writer);

        /// <summary>
        /// Может ли контейнер быть расшифрован данным объектом
        /// </summary>
        /// <param name="info">Информация о контейнере</param>
        /// <returns>true - контейнер может быть расшифрован / false - нет</returns>
        public abstract bool ContainerCanBeProcessed(CryptoContainerInfo info);

        /// <summary>
        /// Специфические настройки криптоалгоритма
        /// </summary>
        public abstract IXmlStored Settings { get; set; }

        #region Static
        public static CrypterBase InitFromSettings(XmlNode cryptoSettingsNode)
        {
            var settings = new CryptoSettings();
            settings.Load(cryptoSettingsNode);
            return InitFromSettings(settings);
        }

        public static CrypterBase InitFromSettings(CryptoSettings settings)
        {
            CrypterBase result;
            switch (settings.Algorithm)
            {
                case CryptoAlgorithmType.None:
                    result = new NoneCrypter();
                    break;
                case CryptoAlgorithmType.DES:
                    result = new DESCrypter();
                    break;
                default:
                    throw new RestoException("Unknown algorithm type: " + settings.Algorithm);
            }
            result.Settings = settings.AlgorithmSettings;
            return result;
        }
        #endregion

    }
}