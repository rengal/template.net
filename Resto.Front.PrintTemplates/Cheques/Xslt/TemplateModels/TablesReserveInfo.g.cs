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
    /// Данные для таблички о зарезервированных столах, корневой элемент
    /// </summary>
    public interface ITablesReserve
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Резерв или банкет
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Номер резерва (банкета)
        /// </summary>
        int Number { get; }
        /// <summary>
        /// Время начала банкета
        /// </summary>
        DateTime StartTime { get; }
        /// <summary>
        /// Время завершения банкета
        /// </summary>
        DateTime EndTime { get; }
        /// <summary>
        /// День недели даты начала банкета
        /// </summary>
        string StartDayOfWeekName { get; }
        /// <summary>
        /// Залы
        /// </summary>
        IEnumerable<IReservedSection> Sections { get; }
        /// <summary>
        /// Количество гостей
        /// </summary>
        int GuestsCount { get; }
        /// <summary>
        /// Тип мероприятия
        /// </summary>
        string ActivityType { get; }
        /// <summary>
        /// Комментарий
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// Клиент
        /// </summary>
        ICustomer Customer { get; }
    }

    /// <summary>
    /// Зарезервированный зал
    /// </summary>
    public interface IReservedSection
    {
        /// <summary>
        /// Название зала
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Зарезервированные столы
        /// </summary>
        string Tables { get; }
    }

    /// <summary>
    /// Клиент
    /// </summary>
    public interface ICustomer
    {
        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Фамилия
        /// </summary>
        string Surname { get; }
        /// <summary>
        /// Номера телефонов
        /// </summary>
        IEnumerable<string> PhoneNumbers { get; }
        /// <summary>
        /// Тип скидочной карты
        /// </summary>
        string DiscountCardType { get; }
        /// <summary>
        /// Дополнительные сведения
        /// </summary>
        string Comment { get; }
    }

    public static class TablesReserveInfoToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this ITablesReserve obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "TablesReserve");
            
            return document;
        }
        
        internal static void AppendToXml(this ITablesReserve obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Type != null)
                container.Add(new XAttribute("Type", obj.Type));
                
            container.Add(new XAttribute("Number", obj.Number));
                
            container.Add(new XAttribute("StartTime", obj.StartTime));
                
            container.Add(new XAttribute("EndTime", obj.EndTime));
                
            if (obj.StartDayOfWeekName != null)
                container.Add(new XAttribute("StartDayOfWeekName", obj.StartDayOfWeekName));
                
            if (obj.Sections != null && obj.Sections.Any(o => o != null))
            {
                var sectionsContainer = new XElement("Sections");
                foreach (var item in obj.Sections)
                    item.AppendToXml(sectionsContainer, "Section");
                container.Add(sectionsContainer);
            }
            
            container.Add(new XAttribute("GuestsCount", obj.GuestsCount));
                
            if (obj.ActivityType != null)
                container.Add(new XAttribute("ActivityType", obj.ActivityType));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            if (obj.Customer != null)
                obj.Customer.AppendToXml(container, "Customer");
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IReservedSection obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.Tables != null)
                container.Add(new XAttribute("Tables", obj.Tables));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICustomer obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.Surname != null)
                container.Add(new XAttribute("Surname", obj.Surname));
                
            if (obj.PhoneNumbers != null && obj.PhoneNumbers.Any(o => o != null))
            {
                var phoneNumbersContainer = new XElement("PhoneNumbers");
                foreach (var item in obj.PhoneNumbers.Where(o => o != null))
                    phoneNumbersContainer.Add(new XElement("Value", item));
                container.Add(phoneNumbersContainer);
            }
            
            if (obj.DiscountCardType != null)
                container.Add(new XAttribute("DiscountCardType", obj.DiscountCardType));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            parent.Add(container);
        }
        
        
    }
}
