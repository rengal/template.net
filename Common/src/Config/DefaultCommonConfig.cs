using System;

namespace Resto.Common
{
    /// <summary>
    /// Дефолтные настройки конфига по умолчанию.
    /// Требуются клиентам, которые неявно пользуются настройками конфига в OfficeCommon.
    /// </summary>
    internal class DefaultCommonConfig : ICommonConfig
    {
        public bool CheckObjectInCacheForConverter { get; set; }
        public bool AutoUpdateNomenclature { get; set; }
        public string HomePath { get; private set; }
        public bool KeepAliveHttpWebRequestFlag { get; set; }
        public string MainRegKey { get; private set; }
        public bool NeedSubstituteSupplierPrice { get; set; }
        public int RepeatCallsServerCount { get; set; }
        public int RepeatCallsServerTimeoutInMs { get; set; }
        public string ServerUrl { get; private set; }
        public string UiCultureName { get; set; }
        public bool UseSupplierProducts { get; set; }
        public bool UpdateCacheAfterReconnection { get; set; }
        public bool DailyCacheSaving { get; set; }
        public int CacheSavingPeriod { get; set; }
        public int CacheSavingMinutesTime { get; set; }
        public bool CanArchiveCache { get; set; }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
