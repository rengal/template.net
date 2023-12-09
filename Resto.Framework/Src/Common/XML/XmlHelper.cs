using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Resto.Framework.Common;
using Resto.Framework.Properties;

namespace Resto.Framework.Xml
{
    /// <summary>
    /// Вспомогательные функции для работы с XML
    /// </summary>
    public struct XmlHelper
    {
        /// <summary>
        /// Добавляет к элементу xmlRootNode дочерний элемент
        /// </summary>
        /// <param name="xmlRootNode">Элемент к которому будет добавлен дочерний элемент</param>
        /// <param name="tagName">Имя дочернего элемента</param>
        /// <param name="value">Значение тега</param>
        /// <param name="attributes">
        /// Массив аттрибутов добавляемого элемента.
        /// Каждый элемент массива является массивом из двух строковых элеметнов.
        /// Первый - имя атрибута, второй - значение атрибута.
        /// </param>
        public static XmlNode AddChildNode(XmlNode xmlRootNode, string tagName, string value, params string[][] attributes)
        {
            var xmlNode = AddChildNode(xmlRootNode, tagName, attributes);
            xmlNode.InnerText = value;
            return xmlNode;
        }

        /// <summary>
        /// Добавляет к элементу xmlRootNode дочерний элемент
        /// </summary>
        /// <param name="xmlRootNode">Элемент к которому будет добавлен дочерний элемент</param>
        /// <param name="tagName">Имя дочернего элемента</param>
        /// <param name="attributes">
        /// Массив аттрибутов добавляемого элемента.
        /// Каждый элемент массива является массивом из двух строковых элеметнов.
        /// Первый - имя атрибута, второй - значение атрибута.
        /// </param>
        public static XmlNode AddChildNode(XmlNode xmlRootNode, string tagName, params string[][] attributes)
        {
            XmlNode xmlNode = xmlRootNode.OwnerDocument.CreateElement(tagName);
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    var xmlAttribute = xmlRootNode.OwnerDocument.CreateAttribute(attribute[0]);
                    xmlAttribute.Value = attribute[1];
                    xmlNode.Attributes.Append(xmlAttribute);
                }

            }
            xmlRootNode.AppendChild(xmlNode);
            return xmlNode;
        }

        /// <summary>
        /// Добавить к узлу секцию CDATA
        /// </summary>
        /// <param name="xmlParentNode">Родительский узел</param>
        /// <param name="value">Значение CDATA</param>
        public static void AppendCDataSection(XmlNode xmlParentNode, string value)
        {
            var cdata = xmlParentNode.OwnerDocument.CreateCDataSection(value);
            xmlParentNode.AppendChild(cdata);
        }

        /// <summary>
        /// Сохранить XML-документ в файл
        /// </summary>
        /// <param name="doc">Xml-документ, который сохраняем</param>
        /// <param name="fileName">Xml-документ, который сохраняем</param>
        public static void Save(XmlDocument doc, string fileName)
        {
            try
            {
                if (doc.DocumentElement == null)
                    throw new RestoException("Invalid XmlDocument: DocumentElement is null");

                if (doc.DocumentElement.HasAttribute("xmlns"))
                    doc.DocumentElement.RemoveAttribute("xmlns");

                using (var writer = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    writer.IndentChar = '\t';
                    writer.Indentation = 1;
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(Resources.XmlHelperImpossibleSaveFile, fileName, e.Message));
            }
        }

        /// <summary>
        /// Освободить память занимаемую XML документом
        /// </summary>
        /// <param name="doc">документ</param>
        public static void ClearDomDocument(ref XmlDocument doc)
        {
            if (doc == null)
                return;

            doc.LoadXml("<xml></xml>");
            doc = null;
        }

        /// <summary>
        /// Проверить наличие атрибута
        /// </summary>
        /// <param name="xn">Родительский узел</param>
        /// <param name="attrName">Имя атрибута</param>
        /// <returns>true - атрибут присутствует / false - атрибута нет</returns>
        public static bool CheckAttr(XmlNode xn, string attrName)
        {
            if (xn == null)
                return false;

            return xn.Attributes.GetNamedItem(attrName) != null;
        }

        /// <summary>
        /// Возвращает значение атрибута
        /// </summary>
        /// <param name="xn">хмл-элемент</param>
        /// <param name="attrName">Наименование атрибута</param>
        /// <param name="defaultValue">Значение атрибута по умолчанию (если не найден или не удалось распарсить значение)</param>
        /// <returns>Значение атрибута</returns>
        public static T GetAttrValue<T>(XmlNode xn, string attrName, T defaultValue) where T : IConvertible
        {
            try
            {
                if (!CheckAttr(xn, attrName))
                    return defaultValue;

                var value = xn.Attributes[attrName].Value;
                if (string.IsNullOrEmpty(value))
                    return defaultValue;

                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), value);

                switch (defaultValue.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        bool resBool;
                        if (bool.TryParse(value, out resBool))
                            return (T)(object)resBool;
                        break;
                    case TypeCode.Int32:
                        int resInt;
                        if (int.TryParse(value, out resInt))
                            return (T)(object)resInt;
                        break;
                    case TypeCode.String:
                        return (T)(object)value;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Устанавливает значение атрибута, если атрибута нет, то создает его
        /// </summary>
        /// <param name="node">Элемент в котором устанавливается атрибут</param>
        /// <param name="attributeName">Имя атрибута</param>
        /// <param name="value">Значение</param>
        public static XmlAttribute SetAttribute(XmlNode node, string attributeName, string value)
        {
            var attribute = node.Attributes[attributeName];

            if (attribute == null)
            {
                attribute = node.OwnerDocument.CreateAttribute(attributeName);
                node.Attributes.Append(attribute);
            }

            attribute.Value = value;
            return attribute;
        }

        /// <summary>
        /// Удаляет атрибут из указанного элемента
        /// </summary>
        /// <param name="xn">Элемент</param>
        /// <param name="attrName">Имя атрибута</param>
        public static void RemoveAttribute(XmlNode xn, string attrName)
        {
            if (xn.Attributes[attrName] != null)
                xn.Attributes.Remove(xn.Attributes[attrName]);
        }

        /// <summary>
        /// Установить значение дочернего узла. Если его нет - добавить
        /// </summary>
        /// <param name="parentNode">Родительский узел</param>
        /// <param name="nodeName">Название узла</param>
        /// <param name="nodeValue">Значение узла</param>
        /// <returns>Найденный/добавленный узел</returns>
        public static XmlNode SetNode(XmlNode parentNode, string nodeName, string nodeValue)
        {
            var result = parentNode.SelectSingleNode(nodeName);
            if (result == null)
            {
                result = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, nodeName, String.Empty);
                parentNode.AppendChild(result);
            }

            if (!string.IsNullOrEmpty(nodeValue))
                result.Value = nodeValue;

            return result;
        }

        /// <summary>
        /// Получить список всех дочерних узлов xml документа.
        /// </summary>
        /// <param name="doc">Документ</param>
        public static IEnumerable<XmlNode> GetAllDocumentNodes(XmlDocument doc)
        {
            return doc.ChildNodes.OfType<XmlNode>().SelectMany(GetChildNodesInternal).Concat(doc.ChildNodes.OfType<XmlNode>());
        }

        private static IEnumerable<XmlNode> GetChildNodesInternal(XmlNode node)
        {
            return node.ChildNodes.OfType<XmlNode>().SelectMany(GetChildNodesInternal).Concat(node.ChildNodes.OfType<XmlNode>());
        }
    }
}