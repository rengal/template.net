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
    /// Удаление модификаторов
    /// </summary>
    public interface IDeleteModifiers
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
        /// Причина удаления
        /// </summary>
        string DeleteReason { get; }
    }

    public static class DeleteModifiersServiceChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IDeleteModifiers obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "DeleteModifiers");
            
            return document;
        }
        
        internal static void AppendToXml(this IDeleteModifiers obj, XContainer parent, string elementName)
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
            
            if (obj.DeleteReason != null)
                container.Add(new XAttribute("DeleteReason", obj.DeleteReason));
                
            parent.Add(container);
        }
        
        
    }
}
