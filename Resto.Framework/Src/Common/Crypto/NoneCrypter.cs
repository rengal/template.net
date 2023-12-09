using System;
using System.Xml;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Класс-заглушка, реализует методы CrypterBase без использования шифрования
    /// </summary>
    public class NoneCrypter : CrypterBase
    {
        /// <summary>
        /// Зашифровать блок данных (входящий блок данных не изменяется)
        /// </summary>
        /// <param name="data">Блок исходных данных</param>
        /// <returns>Блок зашифрованных данных</returns>
        public override byte[] EncryptData(byte[] data)
        {
            return data;
        }

        /// <summary>
        /// Расшифровать блок данных (входящий блок данных не изменяется)
        /// </summary>
        /// <param name="data">Блок зашифрованных данных</param>
        /// <returns>Блок исходных данных</returns>
        public override byte[] DecryptData(byte[] data)
        {
            return data;
        }

        /// <summary>
        /// Записать в XmlContainer секцию с идентифицирующими параметрами алгоритма.
        /// В данном случае - ничего не пишется.
        /// </summary>
        /// <param name="writer">XmlWriter в позиции, с которой начинается секция данных криптоалгоритма</param>
        public override void WriteContainerInfo(XmlWriter writer)
        {            
        }

        /// <summary>
        /// Тип алгоритма (CryptoAlgorithmType.None)
        /// </summary>
        public override CryptoAlgorithmType Algorithm
        {
            get { return CryptoAlgorithmType.None; }
        }

        /// <summary>
        /// Специфические настройки криптоалгоритма (всегда null)
        /// </summary>
        public override IXmlStored Settings
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Может ли контейнер быть прочитан расшифрован данным объектом
        /// </summary>
        /// <param name="info">Информация о контейнере</param>
        /// <returns>true - контейнер может быть расшифрован / false - нет</returns>        
        public override bool ContainerCanBeProcessed(CryptoContainerInfo info)
        {
            // тип алгоритма должен быть "без кодирования", дополнительных настроек быть не должно
            return (info.Algorithm == CryptoAlgorithmType.None) && 
                (String.IsNullOrEmpty(info.CryptoSettingsStr));
        }

    }
}