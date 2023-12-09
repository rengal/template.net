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
    /// Специфические настройки для алгоритма сжатия DES
    /// </summary>
    [Serializable]
    public sealed class DesSettings : IXmlStored
    {
        private const int KEY_LENGTH = 8;

        #region Field / Properties
        private byte[] key;
        [LocalizedDisplayName("Key", typeof(Resources))]
        [LocalizedDescription("Key", typeof(Resources))]
        [TypeConverter(typeof(ByteArrayToBase64Converter))]
        public byte[] Key
        {
            get { return key; }
            set
            {
                if ((value == null) || value.Length != KEY_LENGTH)
                {
                    throw new RestoException("Invalid key length for DES algorithm. Must be: " + KEY_LENGTH);
                }
                key = value;
            }
        }
        #endregion

        #region IXmlStored Members

        /// <summary>
        /// Метод загрузки настроек из XmlNode
        /// </summary>
        /// <param name="settingsNode">узел настроек</param>
        public void Load(XmlNode settingsNode)
        {
            var node = settingsNode.SelectSingleNode(CryptoXmlConsts.desSettingsKey);
            // TODO : чтение ключа
            if ((node.ChildNodes.Count == 0) || 
                (node.ChildNodes[0].NodeType != XmlNodeType.CDATA))
            {
                throw new RestoException("Can not find KEY section");
            }
            Key = Convert.FromBase64String(node.ChildNodes[0].Value);
        }

        /// <summary>
        /// Метод сохранения настроек в XML 
        /// </summary>
        /// <param name="settingsNode">Родительский узел настроек</param>
        public void Save(XmlNode settingsNode)
        {
            var node = XmlHelper.SetNode(settingsNode, CryptoXmlConsts.desSettingsKey, null);
            // TODO : запись ключа
            XmlHelper.AppendCDataSection(node, Convert.ToBase64String(key));
        }

        #endregion
    }
}