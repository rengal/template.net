namespace Resto.Framework.Common.Crypto
{
    public static class CryptoXmlConsts
    {
        #region CryptoSettings
        public const string cryptoRootNode = "cryptoSettings";
        public const string cryptoAlgorithm = "algorithm";
        public const string cryptoAlgorithmDES = "des";
        public const string cryptoAlgorithmNone = "none";
        #endregion

        #region DESSettings
        public const string desSettingsRootNode = "desSettings";
        public const string desSettingsKey = "key";
        public const string desSettingsKeyHash = "keyHash";
        #endregion

    }
}