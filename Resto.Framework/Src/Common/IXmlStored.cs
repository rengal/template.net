using System.Xml;

namespace Resto.Framework.Common
{
    public interface IXmlStored
    {
        /// <summary>
        /// Загрузки объекта из XmlNode
        /// </summary>
        /// <param name="settingsNode">Родительский узел</param>
        void Load(XmlNode settingsNode);

        /// <summary>
        /// Сохранение объекта в XML 
        /// </summary>
        /// <param name="settingsNode">Родительский узел</param>
        void Save(XmlNode settingsNode);
    }
}