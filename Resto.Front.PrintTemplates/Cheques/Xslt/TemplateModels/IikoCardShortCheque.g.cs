// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Xslt.TemplateModels
{
    /// <summary>
    /// Чек iikoCard для встравания в чек оплаты
    /// </summary>
    public interface IIikoCardShort
    {
        /// <summary>
        /// Информация об операции и картой
        /// </summary>
        IIikoCardInfo CardInfo { get; }
        IChequeSettings Settings { get; }
    }

    public static class IikoCardShortChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IIikoCardShort obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "IikoCardShort");
            
            return document;
        }
        
        internal static void AppendToXml(this IIikoCardShort obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.CardInfo != null)
                obj.CardInfo.AppendToXml(container, "CardInfo");
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            parent.Add(container);
        }
        
        
    }
}
