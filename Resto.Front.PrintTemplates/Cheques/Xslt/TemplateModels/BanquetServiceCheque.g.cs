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
    /// Чек заказа на банкет
    /// </summary>
    public interface IBanquetService
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Информация о кухонном чеке
        /// </summary>
        IKitchenChequeInfo Info { get; }
        /// <summary>
        /// Элементы чека
        /// </summary>
        IEnumerable<IKitchenChequeCourseGroup> CourseGroups { get; }
        /// <summary>
        /// Флаг повторной печати
        /// </summary>
        bool IsRepeatedPrint { get; }
        /// <summary>
        /// Номер банкета
        /// </summary>
        int BanquetNumber { get; }
    }

    public static class BanquetServiceChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IBanquetService obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "BanquetService");
            
            return document;
        }
        
        internal static void AppendToXml(this IBanquetService obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            if (obj.CourseGroups != null && obj.CourseGroups.Any(o => o != null))
            {
                var courseGroupsContainer = new XElement("CourseGroups");
                foreach (var item in obj.CourseGroups)
                    item.AppendToXml(courseGroupsContainer, "CourseGroup");
                container.Add(courseGroupsContainer);
            }
            
            container.Add(new XAttribute("IsRepeatedPrint", obj.IsRepeatedPrint));
                
            container.Add(new XAttribute("BanquetNumber", obj.BanquetNumber));
                
            parent.Add(container);
        }
        
        
    }
}
