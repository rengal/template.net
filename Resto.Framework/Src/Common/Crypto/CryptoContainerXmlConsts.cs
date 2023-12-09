namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Константы XML, используемые для шифрования
    /// </summary>
    public static class CryptoContainerXmlConsts
    {
        /// <summary>
        /// Корневой узел контейнера
        /// </summary>
        public const string RootNode = "integrationData";
        /// <summary>
        /// Версия формата
        /// </summary>
        public const string Version = "version";
        /// <summary>
        /// Владелец данных
        /// </summary>
        public const string OwnerType = "ownerType";
        /// <summary>
        /// Дата создания
        /// </summary>
        public const string Created = "created";
        /// <summary>
        /// Секция настроек криптомодуля
        /// </summary>
        public const string CryptoNode = "crypto";
        /// <summary>
        /// Тип алгоритма
        /// </summary>
        public const string Algorithm = "algorithm";
        /// <summary>
        /// Данные, опеределяемые вызывающей стороной
        /// </summary>
        public const string MetadataNode = "metadata";
        /// <summary>
        /// Данные
        /// </summary>
        public const string DataNode = "data";
        /// <summary>
        /// Размер данных
        /// </summary>
        public const string DataSize = "size";
        /// <summary>
        /// Контрольная сумма
        /// </summary>
        public const string DataCrc32 = "crc32";
    }
}