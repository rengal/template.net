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
    /// Внесение/изъятие
    /// </summary>
    public interface IPayInOut
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Информация о квитанциях
        /// </summary>
        ICashChequeInfo Info { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Флаг внесения
        /// </summary>
        bool IsPayIn { get; }
        bool IsChangePayIn { get; }
        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }
        string EventTypeName { get; }
        /// <summary>
        /// Название счёта
        /// </summary>
        string AccountName { get; }
    }

    public static class PayInOutChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IPayInOut obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "PayInOut");
            
            return document;
        }
        
        internal static void AppendToXml(this IPayInOut obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("IsPayIn", obj.IsPayIn));
                
            container.Add(new XAttribute("IsChangePayIn", obj.IsChangePayIn));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            if (obj.EventTypeName != null)
                container.Add(new XAttribute("EventTypeName", obj.EventTypeName));
                
            if (obj.AccountName != null)
                container.Add(new XAttribute("AccountName", obj.AccountName));
                
            parent.Add(container);
        }
        
        
    }
}
