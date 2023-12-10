using System;
using System.Xml.Serialization;

namespace Resto.Common.Plugins
{
    [Serializable]
    [XmlInclude(typeof(TextBox))]
    [XmlInclude(typeof(CheckBox))]
    public abstract class Setting
    {
        /// <summary>
        /// Ключ настройки.
        /// </summary>
        public string Key { get; set; }
    }
}
