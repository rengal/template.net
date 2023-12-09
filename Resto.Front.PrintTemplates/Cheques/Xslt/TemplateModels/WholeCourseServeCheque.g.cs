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
    /// Чек подачи всех блюд курса
    /// </summary>
    public interface IWholeCourseServe
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Счётчик печати
        /// </summary>
        int PrinterCounter { get; }
        /// <summary>
        /// Информация о заказе
        /// </summary>
        IOrderInfo Order { get; }
        /// <summary>
        /// Номер курса
        /// </summary>
        int Course { get; }
        /// <summary>
        /// Информация о кухонном чеке
        /// </summary>
        IKitchenChequeInfo Info { get; }
        /// <summary>
        /// Элементы чека
        /// </summary>
        IEnumerable<IKitchenChequeItem> ChequeItems { get; }
    }

    public static class WholeCourseServeChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IWholeCourseServe obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "WholeCourseServe");
            
            return document;
        }
        
        internal static void AppendToXml(this IWholeCourseServe obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            container.Add(new XAttribute("PrinterCounter", obj.PrinterCounter));
                
            if (obj.Order != null)
                obj.Order.AppendToXml(container, "Order");
            
            container.Add(new XAttribute("Course", obj.Course));
                
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            if (obj.ChequeItems != null && obj.ChequeItems.Any(o => o != null))
            {
                var chequeItemsContainer = new XElement("ChequeItems");
                foreach (var item in obj.ChequeItems)
                    item.AppendToXml(chequeItemsContainer, "ChequeItem");
                container.Add(chequeItemsContainer);
            }
            
            parent.Add(container);
        }
        
        
    }
}
