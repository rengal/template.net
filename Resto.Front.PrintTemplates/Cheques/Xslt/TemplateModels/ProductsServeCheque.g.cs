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
    /// Чек подачи блюд
    /// </summary>
    public interface IProductsServe
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
    }

    public static class ProductsServeChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IProductsServe obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "ProductsServe");
            
            return document;
        }
        
        internal static void AppendToXml(this IProductsServe obj, XContainer parent, string elementName)
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
            
            parent.Add(container);
        }
        
        
    }
}
