using System;

namespace Resto.Common
{
    /// <summary>
    /// Контейнер, для получения общих настроек конфига.
    /// </summary>
    public class CommonConfig
    {
        private static readonly Lazy<DefaultCommonConfig> defaultCommonConfig = new Lazy<DefaultCommonConfig>(() => new DefaultCommonConfig());        
        private static readonly Object mutex = new Object();

        private static ICommonConfig instance;        

        /// <summary>
        /// Экземпляр объекта.
        /// </summary>
        public static ICommonConfig Instance
        {
            get
            {
                lock (mutex)
                {
                    if (instance != null)
                    {
                        return instance;
                    }                    
                } 
                return defaultCommonConfig.Value;
            }
            set
            {
                lock (mutex)
                {
                    instance = value;   
                }                
            }
        }        
    }
}
