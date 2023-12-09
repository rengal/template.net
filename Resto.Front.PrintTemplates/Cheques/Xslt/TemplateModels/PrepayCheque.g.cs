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
    /// Предоплата
    /// </summary>
    public interface IPrepay
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
        /// Флаг возврата
        /// </summary>
        bool IsReturn { get; }
        /// <summary>
        /// Имя официанта
        /// </summary>
        string WaiterName { get; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        int OrderNumber { get; }
        /// <summary>
        /// Название типа оплаты
        /// </summary>
        string PaymentTypeName { get; }
    }

    public static class PrepayChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IPrepay obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "Prepay");
            
            return document;
        }
        
        internal static void AppendToXml(this IPrepay obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("IsReturn", obj.IsReturn));
                
            if (obj.WaiterName != null)
                container.Add(new XAttribute("WaiterName", obj.WaiterName));
                
            container.Add(new XAttribute("OrderNumber", obj.OrderNumber));
                
            if (obj.PaymentTypeName != null)
                container.Add(new XAttribute("PaymentTypeName", obj.PaymentTypeName));
                
            parent.Add(container);
        }
        
        
    }
}
