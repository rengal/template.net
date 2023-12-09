using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Модуль сжатия по методу DES
    /// </summary>
    public class DESCrypter : CrypterBase
    {
        // вектор инициализации
        private static readonly byte[] rgbIV = ASCIIEncoding.ASCII.GetBytes("iiko2008");

        /// <summary>
        /// Зашифровать или расшифровать блок данных
        /// </summary>
        /// <param name="data">Блок данных</param>
        /// <param name="encrypt">true - зашифровать / false - расшифровать</param>
        /// <returns>Блок зашифрованных или расшифрованных данных</returns>
        private byte[] ProcessData(byte[] data, bool encrypt)
        {
            byte[] result;
            using (var desProvider = new DESCryptoServiceProvider())
            {
                // в зависимости от настроек создаем encrypter или decrypter
                var encryptor = encrypt
                                                 ? desProvider.CreateEncryptor(settings.Key, rgbIV)
                                                 : desProvider.CreateDecryptor(settings.Key, rgbIV);

                // создаем CryptoStream на основе MemoryStream и производим обработку блока данных
                using (var memoryStream = new MemoryStream(data.Length))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Flush();

                        memoryStream.Position = 0;
                        // возвращаем результат в виде массива байт
                        result = memoryStream.ToArray();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Зашифровать блок данных
        /// </summary>
        /// <param name="data">Блок исходных данных</param>
        /// <returns>Блок зашифрованных данных</returns>
        public override byte[] EncryptData(byte[] data)
        {
            return ProcessData(data, true);
        }

        /// <summary>
        /// Расшифровать блок данных
        /// </summary>
        /// <param name="data">Блок зашифрованных данных</param>
        /// <returns>Блок исходных данных</returns>
        public override byte[] DecryptData(byte[] data)
        {
            return ProcessData(data, false);
        }

        /// <summary>
        /// Получить HASH ключа алгоритма в виде строки BASE64
        /// </summary>
        /// <returns>HASH ключа</returns>
        private string GetKeyHashString()
        {
            using (var crypter = new SHA1Managed())
            {
                var hashBuff = crypter.ComputeHash(settings.Key);
                return Convert.ToBase64String(hashBuff);
            }
        }

        /// <summary>
        /// Записать в XmlContainer секцию с идентифицирующими параметрами алгоритма.
        /// В данном случае - хэш ключа.
        /// </summary>
        /// <param name="writer">XmlWriter в позиции с которой начинается секция данных криптоалгоритма</param>
        public override void WriteContainerInfo(XmlWriter writer)
        {
            writer.WriteStartElement(CryptoXmlConsts.desSettingsRootNode);
            writer.WriteStartAttribute(CryptoXmlConsts.desSettingsKeyHash);
            writer.WriteString(GetKeyHashString());
            writer.WriteEndAttribute();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Тип алгоритма (CryptoAlgorithmType.DES)
        /// </summary>
        public override CryptoAlgorithmType Algorithm
        {
            get { return CryptoAlgorithmType.DES; }
        }

        private DesSettings settings = new DesSettings();
        /// <summary>
        /// Специфические настройки криптоалгоритма
        /// </summary>
        public override IXmlStored Settings
        {
            get { return settings; }
            set { settings = (DesSettings)value; }
        }

        /// <summary>
        /// Может ли контейнер быть прочитан расшифрован данным объектом
        /// </summary>
        /// <param name="info">Информация о контейнере</param>
        /// <returns>true - контейнер может быть расшифрован / false - нет</returns>       
        public override bool ContainerCanBeProcessed(CryptoContainerInfo info)
        {
            // проверяем тип алгоритма (должен быть DES)
            if (info.Algorithm != CryptoAlgorithmType.DES)
                return false;

            try
            {
                // проверяем содержимое секции описания алгоритма
                using (var stringReader = new StringReader(info.CryptoSettingsStr))
                {
                    using (XmlReader xmlReader = new XmlTextReader(stringReader))
                    {
                        if (!xmlReader.Read())
                            return false;
                        // проверяем корректность корневого узла
                        if (xmlReader.Name != CryptoXmlConsts.desSettingsRootNode)
                            return false;
                        // читаем хэш ключа, которым были зашифрованы данные
                        if (!xmlReader.MoveToAttribute(CryptoXmlConsts.desSettingsKeyHash))
                            return false;
                        // сравниваем его с хэшем нашего ключа (должны быть равны)
                        return xmlReader.Value == GetKeyHashString();
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Сгенерировать случайный ключ для алгоритма
        /// </summary>
        /// <returns>Ключ в виде byte[]</returns>
        public static byte[] GenerateKey()
        {
            using (var crypto = new DESCryptoServiceProvider())
            {
                crypto.GenerateKey();
                return crypto.Key;
            }
        }
    }
}