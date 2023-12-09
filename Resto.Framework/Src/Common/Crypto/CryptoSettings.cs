using System;
using System.ComponentModel;
using System.Xml;
using Resto.Framework.Properties;
using Resto.Framework.Attributes;
using Resto.Framework.Common.Crypto.PropertyGrid;
using Resto.Framework.Xml;

namespace Resto.Framework.Common.Crypto
{
    /// <summary>
    /// Объектное представление настроек модуля шифрования
    /// </summary>
    [Serializable]
    public sealed class CryptoSettings : IXmlStored
    {
        #region Fields / Properties
        private CryptoAlgorithmType algorithm;
        [LocalizedDisplayName("Algorithm", typeof(Resources))]
        [LocalizedDescription("TypeEncryptionAlgorithm", typeof(Resources))]
        public CryptoAlgorithmType Algorithm
        {
            get { return algorithm; }
            set { algorithm = value; }
        }

        private IXmlStored algorithmSettings;
        [LocalizedDisplayName("EncryptionOptions", typeof(Resources))]
        [LocalizedDescription("SpecificParameters", typeof(Resources))]
        [TypeConverter(typeof(ChildObjectConverter))]
        public IXmlStored AlgorithmSettings
        {
            get { return algorithmSettings; }
        }
        #endregion

        #region IXmlStored Members

        /// <summary>
        /// Метод загрузки настроек из XmlNode
        /// </summary>
        /// <param name="settingsNode">узел настроек</param>
        public void Load(XmlNode settingsNode)        
        {
            var algorithmStr = XmlHelper.GetAttrValue(
                settingsNode, CryptoXmlConsts.cryptoAlgorithm, String.Empty);
            switch (algorithmStr)
            {
                case CryptoXmlConsts.cryptoAlgorithmDES:
                    algorithm = CryptoAlgorithmType.DES;
                    algorithmSettings = new DesSettings();
                    algorithmSettings.Load(
                        settingsNode.SelectSingleNode(CryptoXmlConsts.desSettingsRootNode));
                    break;
                case CryptoXmlConsts.cryptoAlgorithmNone:
                    algorithm = CryptoAlgorithmType.None;
                    algorithmSettings = null;
                    break;
                default:
                    throw new RestoException(
                        String.Format("Invalid attribute value: '{0}'", algorithmStr));
            }
        }

        /// <summary>
        /// Метод сохранения настроек в XML 
        /// </summary>
        /// <param name="settingsNode">Родительский узел настроек</param>
        public void Save(XmlNode settingsNode)
        {
            switch (algorithm)
            {
                case CryptoAlgorithmType.DES:
                    XmlHelper.SetAttribute(
                        settingsNode, 
                        CryptoXmlConsts.cryptoAlgorithm,
                        CryptoXmlConsts.cryptoAlgorithmDES);
                    var node = XmlHelper.SetNode(settingsNode, CryptoXmlConsts.desSettingsRootNode, null);
                    AlgorithmSettings.Save(node);
                    break;
                case CryptoAlgorithmType.None:
                    XmlHelper.SetAttribute(
                        settingsNode,
                        CryptoXmlConsts.cryptoAlgorithm,
                        CryptoXmlConsts.cryptoAlgorithmNone);
                    break;
            }
        }

        #endregion
    }
}