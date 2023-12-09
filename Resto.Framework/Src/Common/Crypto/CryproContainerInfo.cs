using System;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Класс-объектное представление криптоконтейнера
    /// </summary>
    [Serializable]
    public sealed class CryptoContainerInfo
    {
        private int version;
        /// <summary>
        /// Версия формата криптоконтейнера
        /// </summary>
        public int Version
        {
            get { return version; }
            internal set { version = value; }
        }

        private string ownerTypeStr;
        /// <summary>
        /// Название типа объекта владельца данных
        /// </summary>
        public string OwnerTypeStr
        {
            get { return ownerTypeStr; }
            internal set { ownerTypeStr = value; }
        }

        private DateTime created;
        /// <summary>
        /// Дата и время создания контейнера
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            internal set { created = value; }
        }

        private CryptoAlgorithmType algorithm;
        /// <summary>
        /// Метод шифрования который применен к секции данных
        /// </summary>
        public CryptoAlgorithmType Algorithm
        {
            get { return algorithm; }
            internal set { algorithm = value; }
        }

        private byte[] data;
        /// <summary>
        /// Зашифрованные данные
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            internal set { data = value; }
        }

        private int length;
        /// <summary>
        /// Размер зашифрованных данных (из xml)
        /// </summary>
        public int Length
        {
            get { return length; }
            internal set { length = value; }
        }

        private uint crc32;
        /// <summary>
        /// Контрольная сумма зашифрованных данных (из xml)
        /// </summary>
        public uint Crc32
        {
            get { return crc32; }
            internal set { crc32 = value; }
        }

        private string metadataStr;
        /// <summary>
        /// Пользовательские данные, которые были записаны в криптоконтенер. Обычно - xml-строка
        /// </summary>
        public string MetadataStr
        {
            get { return metadataStr; }
            internal set { metadataStr = value; }
        }

        private string cryptoSettingsStr;
        /// <summary>
        /// Специфические параметры модуля шифрования (хэш ключа и т.п.). Может использоваться для
        /// определения возможности расшифровки
        /// </summary>
        public string CryptoSettingsStr
        {
            get { return cryptoSettingsStr; }
            internal set { cryptoSettingsStr = value; }
        }

        /// <summary>
        /// Проверить целостность данных контейнера (размер и контрольную сумму секции данных)
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        /// <returns>true - данные контейнера целостны / false - целостность данных контейнера нарушена</returns>
        public bool ValidateData(out string error)
        {
            // проверяем наличие данных
            if ((data == null) || (data.Length == 0))
            {
                error = "Container has not data.";
                return false;
            }
            // сравниваем фактический размер данных и размер, сохраненный в xml
            if (data.Length != Length)
            {
                error = String.Format("Saved data size ({0}) not equal actual data size ({1}).", 
                    data.Length, Length);
                return false;
            }
            // сравниваем фактическую контрольную сумму и сумму, сохраненную в xml
            if (Crc32 != Crc32Helper.CalcCrc32(data, 0, data.Length))
            {
                error = "Data section corrupted (crc checking failed).";
                return false;
            }
            // если все проверки пройдены - данные контейнера целостны
            error = null;
            return true;
        }
    }
}